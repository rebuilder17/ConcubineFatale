using UnityEngine;
using System.Collections;

namespace CFCore.Data
{
	public class Activity : BaseLuaWrappable<Activity.LuaWrapper>
	{


		public class LuaWrapper : BaseLuaWrapper<Activity>
		{
			public LuaWrapper() { }
		}
	}
}
