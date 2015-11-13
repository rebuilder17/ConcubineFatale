using UnityEngine;
using System.Collections;

namespace CFCore.Data
{
	/// <summary>
	/// 세력의 스탯 이름들
	/// </summary>
	public enum FactionStatNames
	{
		power,			// 세력의 힘
	}

	public class Faction : BaseStatObject<FactionStatNames, Faction.LuaWrapper>
	{
		/// <summary>
		/// 팩션을 구분하는 문자열 ID
		/// </summary>
		public string id { get; set; }



		private Faction() { }


		/// <summary>
		/// 새 게임을 위해 생성
		/// </summary>
		/// <returns></returns>
		public static Faction CreateForNewSession(string id)
		{
			var faction	= new Faction();
			faction.id	= id;

			return faction;
		}




		// Wrapper

		public class LuaWrapper : BaseLuaWrapper<Faction>
		{
			public LuaWrapper() { }


			/// <summary>
			/// 스탯 객체
			/// </summary>
			public MoonSharp.Interpreter.DynValue stat
			{
				get { return baseObj.luaStatWrapper; }
			}
		}
	}
}
