using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CFCore.Data
{
	/// <summary>
	/// Lua <-> c# 끼리 값을 공유하기 위해 사용하는 테이블 (나중에 Serialize도 가능하게 할 것임)
	/// </summary>
	public class SharedTable : BaseLuaWrappable<SharedTable.LuaWrapper>
	{
		Dictionary<string, object>	m_dict	= new Dictionary<string,object>();


		public object this[string index]
		{
			get
			{
				object retv;
				if (!m_dict.TryGetValue(index, out retv))
					return null;
				else
					return retv;
			}
			set { m_dict[index] = value; }
		}



		private SharedTable() { }


		/// <summary>
		/// 새 게임 용으로 생성
		/// </summary>
		/// <returns></returns>
		public static SharedTable CreateForNewSession()
		{
			var sharedtbl	= new SharedTable();


			return sharedtbl;
		}






		// Lua 관련

		public class LuaWrapper : BaseLuaWrapper<SharedTable>
		{
			public LuaWrapper() { }

			public object this[string index]
			{
				get { return baseObj[index]; }
				set { baseObj[index] = value; }
			}
		}
	}
}