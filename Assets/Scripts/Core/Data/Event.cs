using UnityEngine;
using System.Collections;

namespace CFCore.Data
{
	public class Event : BaseLuaWrappable<Event.LuaWrapper>
	{

		public class LuaWrapper : BaseLuaWrapper<Event>
		{
			public LuaWrapper() { }
		}
	}
}
