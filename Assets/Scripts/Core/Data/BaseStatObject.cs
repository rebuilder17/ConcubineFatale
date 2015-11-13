using UnityEngine;
using System.Collections;

namespace CFCore.Data
{
	/// <summary>
	/// 스탯을 여럿 보유하는 오브젝트에 대한 베이스 클래스
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class BaseStatObject<T, WrapperT> : BaseLuaWrappable<WrapperT>
		where T : struct, System.IConvertible, System.IComparable, System.IFormattable
		where WrapperT : class, ILuaWrapper, new()
	{
		/// <summary>
		/// 스탯 값이 변할 때의 이벤트 델리게이트
		/// </summary>
		/// <param name="statName"></param>
		/// <param name="diff"></param>
		/// <param name="newvalue"></param>
		public delegate void StatChangedCallback(T statName, int diff, int newvalue);

		/// <summary>
		/// 스탯을 열람할 수 있게 하는 인터페이스
		/// </summary>
		public interface IStat
		{
			int this[T index] { get; }
		}

		/// <summary>
		/// 실제 스탯 객체
		/// </summary>
		protected class Stat : IStat
		{
			private	int[]	m_stat;

			public event StatChangedCallback StatChanged;
			public int this[T index]
			{
				get { return m_stat[System.Convert.ToInt16(index)]; }
				set
				{
					var ii		= System.Convert.ToInt16(index);
					var orig	= m_stat[ii];
					var change	= value - orig;			// 변화폭을 계산해둔다

					m_stat[ii]	= value;

					// 값 변경 이벤트 호출
					if (change > 0 && StatChanged != null)
						StatChanged(index, change, value);
				}
			}


			public Stat()
			{
				// 스탯 갯수에 맞춰서 배열 초기화
				var namecount	= System.Enum.GetValues(typeof(T)).Length;
				m_stat			= new int[namecount];
			}

			public void AddStatChangeCallback(StatChangedCallback cb)
			{
				StatChanged += cb;
			}
		}
		//


		private Stat		m_stat;			// 스탯 실제 오브젝트

		/// <summary>
		/// 읽기 전용 스탯 오브젝트를 구하기
		/// </summary>
		public IStat stat { get { return m_stat; } }
		/// <summary>
		/// 쓰기 가능한 스탯 오브젝트를 구하기 (내부용)
		/// </summary>
		protected Stat statObject { get { return m_stat; } }



		protected BaseStatObject()
		{
			if (!typeof(T).IsEnum)			// T 는 반드시 Enum이어야 함
			{
				throw new System.InvalidCastException("T of BaseStatObject must be an enum type.");
			}
			m_stat	= new Stat();
		}

		/// <summary>
		/// 스탯 변경에 따른 이벤트 콜백 등록하기
		/// </summary>
		/// <param name="cb"></param>
		public void AddStatChangeCallback(StatChangedCallback cb)
		{
			m_stat.AddStatChangeCallback(cb);
		}




		// Lua 관련 부분

		MoonSharp.Interpreter.DynValue	m_udStat;		// UserData로 컨버팅된 Stat

		class LuaStatWrapper
		{
			private Stat		m_stat;
			private System.Type	m_enumType;

			public int this[string index]
			{
				get
				{
					return m_stat[(T)System.Enum.Parse(m_enumType, index)];
				}

				set
				{
					m_stat[(T)System.Enum.Parse(m_enumType, index)]	= value;
				}
			}

			public LuaStatWrapper(Stat stat)
			{
				
				m_stat		= stat;
				m_enumType	= typeof(T);
			}
		}

		/// <summary>
		/// UserData로 컨버팅된 Stat
		/// </summary>
		protected MoonSharp.Interpreter.DynValue luaStatWrapper
		{
			get
			{
				if (!MoonSharp.Interpreter.UserData.IsTypeRegistered<LuaStatWrapper>())	// UserData 타입 등록
				{
					MoonSharp.Interpreter.UserData.RegisterType<LuaStatWrapper>();
				}

				if (m_udStat == null)											// UserData 생성 (wrapper를 사용한다.)
				{
					m_udStat	= MoonSharp.Interpreter.UserData.Create(new LuaStatWrapper(m_stat));
				}

				return m_udStat;
			}
		}
	}
}