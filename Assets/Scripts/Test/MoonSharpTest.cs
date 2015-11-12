using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;

public class MoonSharpTest : MonoBehaviour
{
	void Start()
	{
		Debug.Log("start!");
		var script	= new Script();
		script.Options.DebugPrint	= s => Debug.Log(s);
		script.DoFile("testscript");
	}
}
