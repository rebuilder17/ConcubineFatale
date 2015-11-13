using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;


namespace CFCore.Data
{
	/// <summary>
	/// Lua wrapper 클래스의 인터페이스
	/// </summary>
	public interface ILuaWrapper
	{
		ILuaWrappable baseObjGeneral { get; }
		void Initialize(ILuaWrappable baseObj);
	}

	public abstract class BaseLuaWrapper : ILuaWrapper
	{
		public ILuaWrappable baseObjGeneral { get; private set; }
		public void Initialize(ILuaWrappable baseObj)
		{
			this.baseObjGeneral	= baseObj;
		}
	}

	public abstract class BaseLuaWrapper<BaseT> : BaseLuaWrapper
		where BaseT : class
	{
		public BaseT baseObj
		{
			get { return baseObjGeneral as BaseT; }
		}
	}

	public interface ILuaWrappable
	{
		DynValue luaWrapper { get; }
	}
	
	/// <summary>
	/// Lua로 별도 래핑을 지원하는 클래스
	/// </summary>
	/// <typeparam name="WrapperT"></typeparam>
	public abstract class BaseLuaWrappable<WrapperT> : ILuaWrappable
		where WrapperT : class, ILuaWrapper, new()
	{
		DynValue		m_udWrapper;

		/// <summary>
		/// Lua 래퍼
		/// </summary>
		public DynValue luaWrapper
		{
			get
			{
				if (!UserData.IsTypeRegistered<WrapperT>())
				{
					UserData.RegisterType<WrapperT>();
				}

				if (m_udWrapper == null)
				{
					var wrapper	= new WrapperT() as BaseLuaWrapper;
					wrapper.Initialize(this);
					m_udWrapper	= UserData.Create(wrapper);
				}

				return m_udWrapper;
			}
		}
	}
}
