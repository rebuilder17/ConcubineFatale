using UnityEngine;
using System.Collections;

public class CFCoreTest : MonoBehaviour
{
	CFCoreComponent	m_cfcore;


	void Awake()
	{
		m_cfcore	= GetComponent<CFCoreComponent>();
	}

	// Use this for initialization
	void Start()
	{
		m_cfcore.NewSession();
		CFCore.LuaEngine.instance.Test();
	}
}
