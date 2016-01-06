using UnityEngine;
using System.Diagnostics;

namespace Lshain
{
	public static class LogManager
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

	    [Conditional("DEBUG")]
	    public static void E(string msg)
	    {
	        UnityEngine.Debug.LogError(msg);
	    }

	    [Conditional("DEBUG")]
	    public static void V(string msg)
	    {
	        UnityEngine.Debug.Log(msg);
	    }

	    [Conditional("DEBUG")]
	    public static void W(string msg)
	    {
	        UnityEngine.Debug.LogWarning(msg);
	    }
	}
}
