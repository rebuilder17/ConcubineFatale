using UnityEngine;
using System.Collections;


namespace CFCore.Data
{
	/// <summary>
	/// 플레이어 스탯 이름
	/// </summary>
	public enum PlayerStatNames
	{
		rank,				// 계급

		stamina,			// 스태미너
		beauty,				// 용모
		elegance,			// 기품
		allure,				// 색기
		knowledge,			// 지식
		conversation,		// 화술
		arts,				// 예술적 소양
		benevolence,		// 자애
		stress,				// 스트레스
	}

	/// <summary>
	/// 플레이어 정보
	/// </summary>
	public class Player : BaseStatObject<PlayerStatNames, Player.LuaWrapper>
	{
		/// <summary>
		/// 플레이어 이름.
		/// </summary>
		public string name { get; set; }




		private Player() { }

		public static Player CreateForNewSession()
		{
			var player		= new Player();

			player.name		= "테스터";
			player.statObject[PlayerStatNames.stamina]	= 100;

			return player;
		}


		public class LuaWrapper : BaseLuaWrapper<Player>
		{
			public LuaWrapper() { }


			public string name
			{
				get { return baseObj.name; }
				set { baseObj.name = value; }
			}

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