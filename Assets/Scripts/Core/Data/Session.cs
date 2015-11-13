using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CFCore.Data
{
	/// <summary>
	/// 게임 세션.
	/// </summary>
	public class Session : BaseLuaWrappable<Session.LuaWrapper>
	{

		// Members

		/// <summary>
		/// 현재 턴 (1턴 = 1주)
		/// </summary>
		public int currentTurn { get; private set; }
		/// <summary>
		/// 최대 턴, 이 제한선에 걸리면 강제로 엔딩 체크를 하게 된다
		/// 현재는 144턴 고정 ( = 36개월 = 3년 )
		/// </summary>
		public int maxTurn { get { return 144; } }



		Player					m_player;		// 플레이어 오브젝트
		Resource				m_resource;		// 게임 내 운용 가능한 자원

		Emperor					m_emperor;		// 황제

		Dictionary<string, Individual>	m_individualDict;	// 개인 목록
		Dictionary<string, Faction>		m_factionDict;		// 세력 목록

		SharedTable				m_sharedTable;	// 공유 테이블




		public Player.IStat		playerStat		{ get { return m_player.stat; } }
		public Resource.IStat	resourceStat	{ get { return m_resource.stat; } }
		public Emperor.IStat	emperorStat		{ get { return m_emperor.stat; } }

		public Individual.IStat GetIndividualStat(string id)
		{
			return m_individualDict[id].stat;
		}

		public Faction.IStat GetFactionStat(string id)
		{
			return m_factionDict[id].stat;
		}

		public SharedTable sharedTable { get { return m_sharedTable; } }




		private Session() { }

		/// <summary>
		/// LuaEngine의 Game 테이블에 오브젝트들을 등록한다.
		/// </summary>
		private void RegisterObjectsToLua()
		{
			var engine	= LuaEngine.instance;

			engine.AddToGameObjectTable("Session", luaWrapper);
			engine.AddToGameObjectTable("Player", m_player.luaWrapper);
			engine.AddToGameObjectTable("Resource", m_resource.luaWrapper);
			engine.AddToGameObjectTable("Emperor", m_emperor.luaWrapper);

			engine.AddToGameObjectTable("SharedTable", m_sharedTable.luaWrapper);

			var individualTbl	= engine.CreateNewLuaTable();
			engine.AddToGameObjectTable("Individual", individualTbl);
			foreach(var pair in m_individualDict)
			{
				individualTbl.Table.Set(pair.Key, pair.Value.luaWrapper);
			}

			var factionTbl		= engine.CreateNewLuaTable();
			engine.AddToGameObjectTable("Faction", factionTbl);
			foreach(var pair in m_factionDict)
			{
				factionTbl.Table.Set(pair.Key, pair.Value.luaWrapper);
			}
		}

		/// <summary>
		/// 새 세션 (게임) 용으로 Session 생성
		/// </summary>
		/// <returns></returns>
		public static Session CreateForNewSession()
		{
			var session	= new Session();

			session.m_player	= Player.CreateForNewSession();
			session.m_resource	= Resource.CreateForNewSession();
			session.m_emperor	= Emperor.CreateForNewSession();

			session.m_sharedTable	= SharedTable.CreateForNewSession();


			// ID값이 있는 객체들은 Lua쪽에서 ID 리스트를 가져온다.

			session.m_individualDict	= new Dictionary<string, Individual>();
			var indTableList	= LuaEngine.instance.individualTableList;
			foreach (var id in indTableList.keyList)
			{
				session.m_individualDict[id]	= Individual.CreateForNewSession(id);
			}

			session.m_factionDict	= new Dictionary<string, Faction>();
			var facTableList	= LuaEngine.instance.factionTableList;
			foreach (var id in facTableList.keyList)
			{
				session.m_factionDict[id]	= Faction.CreateForNewSession(id);
			}


			session.RegisterObjectsToLua();	// 오브젝트들을 Lua쪽으로 등록

			return session;
		}



		// Lua 관련

		public class LuaWrapper : BaseLuaWrapper<Session>
		{
			public LuaWrapper() { }

			/// <summary>
			/// 현재 턴
			/// </summary>
			public int turn
			{
				get { return baseObj.currentTurn; }
			}
		}
	}
}
