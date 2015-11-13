using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;

public class MoonSharpTest : MonoBehaviour
{
	void Start()
	{
		StartCoroutine(LuaRoutine());
	}

	IEnumerator LuaRoutine()
	{
		bool pause	= false;

		Debug.Log("start!");
		var script		= new Script();
		script.Options.DebugPrint	= s => Debug.Log(s);

		script.Globals["csharpfunc"]	= (System.Action)delegate()
		{
			pause = true;
		};
		var function	= script.DoFile("testscript");
		var coroutine	= script.CreateCoroutine(function);

		do
		{
			coroutine.Coroutine.Resume();
			Debug.Log("C sharp side");
			yield return new WaitForSeconds(3);
		}
		while (coroutine.Coroutine.State != CoroutineState.Dead);

		yield break;
	}
}
