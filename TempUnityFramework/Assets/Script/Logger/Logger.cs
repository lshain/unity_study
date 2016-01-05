using UnityEngine;

using System.Collections;
using System.Diagnostics;

public static class Logger
{
	[Conditional("DEBUG")]
    public static void E(string tag, string msg)
    {
		UnityEngine.Debug.LogError(tag + ": " + msg);
    }

	[Conditional("DEBUG")]
    public static void V(string tag, string msg)
    {
		UnityEngine.Debug.Log(tag + ": " + msg);
    }

	[Conditional("DEBUG")]
    public static void W(string tag, string msg)
    {
		UnityEngine.Debug.LogWarning(tag + ": " + msg);
    }
}
