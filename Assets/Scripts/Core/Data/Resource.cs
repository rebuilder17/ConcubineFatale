using UnityEngine;
using System.Collections;

namespace CFCore.Data
{
	/// <summary>
	/// 자원 상황의 스탯 이름들
	/// </summary>
	public enum ResourceStatNames
	{
		money,			// 재산
		favoritism,		// 총애도
	}

	/// <summary>
	/// 게임 내에서 운용 가능한 자원 상황.
	/// </summary>
	public class Resource : BaseStatObject<ResourceStatNames, Resource.LuaWrapper>
	{



		private Resource() { }

		public static Resource CreateForNewSession()
		{
			var resource	= new Resource();

			resource.statObject[ResourceStatNames.money]	= 1000;

			return resource;
		}







		// Lua 관련

		public class LuaWrapper : BaseLuaWrapper<Resource>
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