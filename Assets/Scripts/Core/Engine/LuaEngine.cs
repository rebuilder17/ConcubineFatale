using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;

namespace CFCore
{
	/// <summary>
	/// CFCore에서 사용하는 루아 스크립트 엔진
	/// </summary>
	public class LuaEngine
	{
		/// <summary>
		/// Script 를 실행하여 얻은 테이블
		/// </summary>
		public interface IScriptTable
		{
			DynValue this[string index] { get; }
			DynValue Get(string objname);
			DynValue Call(string funcname, params DynValue[] paramlist);
		}

		private class ScriptTable : IScriptTable
		{
			private Table m_table;

			public DynValue this[string index]
			{
				get
				{
					return Get(index);
				}
			}

			public DynValue Get(string objname)
			{
				return m_table.Get(objname);
			}

			public DynValue Call(string funcname, params DynValue[] paramlist)
			{
				var obj	= m_table.Get(funcname);
				if (obj.IsNil())					// Nil 일 때는 실행하지 않고 무시한다.
				{
					Debug.LogWarning("[ScriptTable] Cannot call " + funcname + " : is nil");
				}
				else
				{
					try
					{
						return obj.Function.Call(paramlist);
					}
					catch(ScriptRuntimeException ex)
					{
						LuaEngine.HandleScriptError(ex);
					}
				}

				return DynValue.NewNil();
			}

			public ScriptTable(Table tbl)
			{
				m_table = tbl;
			}
		}

		/// <summary>
		/// Script를 실행하여 얻어온 각 테이블을 string indexer로 구할 수 있는 객체
		/// </summary>
		public interface IScriptTableList
		{
			IScriptTable this[string index] { get; }
			ICollection<string> keyList { get; }
		}

		private class ScriptTableList : IScriptTableList
		{
			Dictionary<string, ScriptTable>	m_tableList;

			public IScriptTable this[string index]
			{
				get
				{
					return m_tableList[index];
				}
			}

			public ICollection<string> keyList
			{
				get { return m_tableList.Keys; }
			}

			public ScriptTableList()
			{
				m_tableList	= new Dictionary<string, ScriptTable>();
			}

			/// <summary>
			/// 스크립트 테이블 추가
			/// </summary>
			/// <param name="scrtbl"></param>
			public void AddScriptTable(ScriptTable scrtbl)
			{
				var id	= scrtbl.Get("id").String;		// NOTE : 반드시 id 필드가 테이블에 존재해야 한다.
				m_tableList[id]	= scrtbl;
			}
		}
		//


		// Constants

		const string		c_entryScript				= "main";					// 시작 스크립트

		const string		c_field_playerScript		= "PlayerScript";			// 필드명 - 플레이어 스크립트 경로
		const string		c_field_emperorScript		= "EmperorScript";			// 필드명 - 황제 스크립트 경로
		const string		c_field_individualScripts	= "IndividualScriptList";	// 필드명 - 인물 스크립트 경로 목록
		const string		c_field_factionScripts		= "FactionScriptList";		// 필드명 - 세력 스크립트 경로 목록


		// Static
		static LuaEngine	s_instance;
		public static LuaEngine instance
		{
			get
			{
				if (s_instance == null)
					s_instance	= new LuaEngine();
				return s_instance;
			}
		}


		// Members

		Script						m_engine;				// Lua 엔진 객체
		
		ScriptTable					m_tablePlayer;
		ScriptTable					m_tableEmperor;
		ScriptTableList				m_tableIndivdualList;
		ScriptTableList				m_tableFactionList;

		Table						m_gameTable;			// 게임 오브젝트 (UserData) 테이블


		/// <summary>
		/// 플레이어 Lua 테이블
		/// </summary>
		public IScriptTable playerTable
		{ get { return m_tablePlayer; } }

		/// <summary>
		/// 황제 Lua 테이블
		/// </summary>
		public IScriptTable emperorTable
		{ get { return m_tableEmperor; } }

		/// <summary>
		/// 인물 Lua 테이블 목록
		/// </summary>
		public IScriptTableList individualTableList
		{ get { return m_tableIndivdualList; } }

		/// <summary>
		/// 세력 Lua 테이블 목록
		/// </summary>
		public IScriptTableList factionTableList
		{ get { return m_tableFactionList; } }


		/// <summary>
		/// 초기화
		/// </summary>
		public void Initialize()
		{
			try
			{
				m_engine		= new Script();						// 엔진 인스턴스 초기화
				m_engine.Options.DebugPrint	= (s) =>				// lua의 print 아웃풋 설정
					{
						Debug.Log("[LUA Print] " + s);
					};

				m_engine.DoFile(c_entryScript);						// 시작 스크립트 실행

				// Lua 테이블 가져오기
				m_tablePlayer			= LoadSingleScriptTable(c_field_playerScript);
				m_tableEmperor			= LoadSingleScriptTable(c_field_emperorScript);
				m_tableIndivdualList	= LoadScriptTableArray(c_field_individualScripts);
				m_tableFactionList		= LoadScriptTableArray(c_field_factionScripts);

				m_gameTable				= DynValue.NewTable(m_engine).Table;	// 게임 오브젝트 테이블
				m_engine.Globals["Game"]	= m_gameTable;
			}

			catch(ScriptRuntimeException ex)
			{
				HandleScriptError(ex);
			}
		}

		/// <summary>
		/// 글로벌 필드의 문자열을 경로로 삼아서 스크립트 파일 실행, 결과 테이블을 가져온다.
		/// </summary>
		/// <param name="globalName"></param>
		ScriptTable LoadSingleScriptTable(string globalName)
		{
			return new ScriptTable(m_engine.DoFile(m_engine.Globals.Get(globalName).String).Table);
		}

		/// <summary>
		/// 글로벌 필드의 배열에 있는 문자열을 경로로 삼아 각각 스크립트 파일 실행, 결과 테이블을 얻어와 dictionary에 넣는다.
		/// </summary>
		/// <param name="globalName"></param>
		/// <param name="tableList"></param>
		ScriptTableList LoadScriptTableArray(string globalName)
		{
			var tableList	= new ScriptTableList();

			foreach(var entry in m_engine.Globals.Get(globalName).Table.Values)
			{
				tableList.AddScriptTable(new ScriptTable(m_engine.DoFile(entry.String).Table));
			}
			return tableList;
		}

		/// <summary>
		/// 게임 오브젝트 테이블 (글로벌의 Game) 에 객체를 추가한다. C# 쪽에서 바인딩한 오브젝트는 이걸로 등록
		/// </summary>
		/// <param name="fieldName"></param>
		/// <param name="value"></param>
		public void AddToGameObjectTable(string fieldName, DynValue value)
		{
			m_gameTable[fieldName]	= value;
		}

		/// <summary>
		/// Lua 테이블 객체 생성
		/// </summary>
		/// <returns></returns>
		public DynValue CreateNewLuaTable()
		{
			return DynValue.NewTable(m_engine);
		}




		static void HandleScriptError(ScriptRuntimeException ex)
		{
			string errormsg	= ex.DecoratedMessage + "\nStack Trace :\n";
			
			var stack		= ex.CallStack;
			int count		= stack.Count;
			for (int i = 0; i < count; i++ )
			{
				var loc		= stack[i].Location;
				errormsg	+= string.Format("    {0}\n", loc.FormatLocation(s_instance.m_engine));
			}
			Debug.LogError(errormsg);
		}

		//
		public void Test()
		{
			try
			{
				/*
				m_engine.DoString(
				@"
					print(tostring(Game))
					print(tostring(Game.Player))
					print(tostring(Game.Player.stat))
					print(tostring(Game.Player.stat['stamina']))
					--print(tostring(Game.Player.stattttt['stamina']))
					debug.traceback()");
				 */
				//m_engine.DoFile("testscript2");
			}
			catch(ScriptRuntimeException ex)
			{
				HandleScriptError(ex);
			}
		}
	}
}
