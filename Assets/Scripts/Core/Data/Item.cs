using UnityEngine;
using System.Collections;

namespace CFCore.Data
{
	public class Item : BaseLuaWrappable<Item.LuaWrapper>
	{

		public class LuaWrapper : BaseLuaWrapper<Item>
		{
			public LuaWrapper() { }
		}
	}
}
