using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// Lua Script를 확장자를 변경하지 않고 처리하기 해주는 스크립트.
/// Note : 모든 Lua 스크립트 파일은 Assets/Lua/ 폴더 안에 들어가야 한다.
/// </summary>
class LuaAssetProcessor : AssetPostprocessor
{
	const string	c_luaExt		= ".lua";									// 파일 확장자
	const string	c_txtExt		= ".txt";
	const string	c_origBasePath	= "Assets/Lua/";							// 원래 경로
	const string	c_copyBasePath	= "Assets/Resources/MoonSharp/Scripts/";	// .txt 로 확장자를 변경한 스크립트 파일 경로

	static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (var str in importedAssets)
		{
			if (CheckLuaFilePathAndExt(str))			// 올바른 Lua 파일일 때만
			{
				MakeCopy(str);
			}
		}

		foreach (var str in deletedAssets)
		{
			if (CheckLuaFilePathAndExt(str))			// 올바른 Lua 파일일 때만
			{
				RemoveCopy(str);
			}
		}

		for (var i=0; i<movedAssets.Length; i++)
		{
			var from	= movedFromAssetPaths[i];
			var to		= movedAssets[i];
			if (CheckLuaFilePathAndExt(from))
			{
				RemoveCopy(from);
			}

			if (CheckLuaFilePathAndExt(to))
			{
				MakeCopy(to);
			}
		}
	}

	/// <summary>
	/// 해당 파일이 올바른 경로에 들어간 lua 파일인지
	/// </summary>
	/// <param name="origpath"></param>
	/// <returns></returns>
	static bool CheckLuaFilePathAndExt(string origpath)
	{
		return origpath.StartsWith(c_origBasePath) && origpath.EndsWith(c_luaExt);
	}

	/// <summary>
	/// 원래 경로에서 불필요한 부분을 제거, 상세 경로와 파일 이름으로 나눠서 리턴한다.
	/// 반드시 CheckLuaFilePathAndExt 를 통과한 문자열이어야 한다.
	/// </summary>
	/// <param name="origpath"></param>
	/// <param name="path"></param>
	/// <param name="name"></param>
	static void stripOriginalPath(string origpath, out string path, out string name)
	{
		var baselen	= c_origBasePath.Length;
		var baserip	= origpath.Substring(baselen, origpath.Length - baselen - c_luaExt.Length);
		//Debug.Log("baserip : " + baserip);

		stripPathAndName(baserip, out path, out name);
		//Debug.Log("path : " + path + ", name : " + name);
	}

	static void stripPathAndName(string origpath, out string path, out string name)
	{
		var pathdel	= origpath.LastIndexOf('/');
		if (pathdel != -1)
		{
			path	= origpath.Substring(0, pathdel);
			name	= origpath.Substring(pathdel + 1);
		}
		else
		{
			path	= "";
			name	= origpath;
		}
	}

	/// <summary>
	/// 해당 파일을 지정된 경로에 확장자를 변경하여 복제한다.
	/// </summary>
	/// <param name="origpath"></param>
	static void MakeCopy(string origpath)
	{
		string path, name;
		stripOriginalPath(origpath, out path, out name);

		MakeTargetDirectory(c_copyBasePath + path);		// 폴더가 없을 경우 만들어주기

		var pathname	= path.Length > 0? path + "/" + name : name;
		var from		= c_origBasePath + pathname + c_luaExt;
		var to			= c_copyBasePath + pathname + c_txtExt;

		if (!string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(to)))	// 이미 복사본이 존재할 경우엔 해당 복사본을 제거한다.
		{
			AssetDatabase.DeleteAsset(to);
		}
		AssetDatabase.CopyAsset(from, to);
		//Debug.LogFormat("from : {0}, to : {1}", from, to);
		AssetDatabase.Refresh();
	}

	/// <summary>
	/// 해당 파일의 복사본을 삭제한다.
	/// </summary>
	/// <param name="origpath"></param>
	static void RemoveCopy(string origpath)
	{
		string path, name;
		stripOriginalPath(origpath, out path, out name);

		var pathname	= path.Length > 0? path + "/" + name : name;
		var target		= c_copyBasePath + pathname + c_txtExt;
		AssetDatabase.DeleteAsset(target);
		//Debug.Log("remove : " + target);
		AssetDatabase.Refresh();
	}

	/// <summary>
	/// 타겟 경로가 유효하도록, 필요할 경우 폴더를 생성한다.
	/// </summary>
	/// <param name="targetpath"></param>
	static void MakeTargetDirectory(string targetdir)
	{
		if(!string.IsNullOrEmpty(targetdir) && !AssetDatabase.IsValidFolder(targetdir))
		{
			string parent, current;
			stripPathAndName(targetdir, out parent, out current);

			MakeTargetDirectory(parent);
			if (!string.IsNullOrEmpty(current))
			{
				AssetDatabase.CreateFolder(parent, current);
				//Debug.LogFormat("making dir : {0}, {1}", parent, current);
			}
		}
	}
}