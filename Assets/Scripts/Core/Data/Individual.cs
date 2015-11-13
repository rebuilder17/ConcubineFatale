using UnityEngine;
using System.Collections;

namespace CFCore.Data
{
	/// <summary>
	/// 각 인물의 스탯 이름들
	/// </summary>
	public enum IndividualStatNames
	{
		trust,			// 주인공을 향한 신뢰도
	}

	/// <summary>
	/// 각 인물
	/// </summary>
	public class Individual : BaseStatObject<IndividualStatNames, Individual.LuaWrapper>
	{
		/// <summary>
		/// 인물을 구분할 때 사용하는 문자열 ID
		/// </summary>
		public string id { get; set; }




		private Individual() { }

		/// <summary>
		/// 새 게임을 위해 생성
		/// </summary>
		/// <returns></returns>
		public static Individual CreateForNewSession(string id)
		{
			var ind	= new Individual();
			ind.id	= id;
			return ind;
		}



		public class LuaWrapper : BaseLuaWrapper<Individual>
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
