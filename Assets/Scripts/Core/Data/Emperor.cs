using UnityEngine;
using System.Collections;

namespace CFCore.Data
{
	/// <summary>
	/// 황제의 스탯 이름들
	/// </summary>
	public enum EmperorStatNames
	{
		generosity,			// 성군도(반대 방향에서 보면 폭군도)
	}

	/// <summary>
	/// 황제 (공략 대상)
	/// </summary>
	public class Emperor : BaseStatObject<EmperorStatNames, Emperor.LuaWrapper>
	{



		private Emperor() { }

		public static Emperor CreateForNewSession()
		{
			var emperor	= new Emperor();

			return emperor;
		}


		public class LuaWrapper : BaseLuaWrapper<Emperor>
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
