using UnityEngine;
using System.Collections;


/// <summary>
/// 게임의 메인 로직 MonoBehaviour
/// </summary>
public class CFCoreComponent : MonoBehaviour
{
	// Members

	public CFCore.Data.Session session { get; private set; }



	void Awake()
	{
		CFCore.LuaEngine.instance.Initialize();			// Lua 엔진 초기화
	}

	/// <summary>
	/// 새 세션 만들기 (새 게임)
	/// </summary>
	public void NewSession()
	{
		session	= CFCore.Data.Session.CreateForNewSession();
	}
}
