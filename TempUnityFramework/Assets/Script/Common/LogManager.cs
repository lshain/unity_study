using UnityEngine;

namespace LT
{
	public static class LogManager
	{
	    public static void E(string tag, string msg)
	    {
#if DEBUG
			UnityEngine.Debug.LogError(tag + ": " + msg);
#endif
	    }
			
	    public static void V(string tag, string msg)
	    {
#if DEBUG
			UnityEngine.Debug.Log(tag + ": " + msg);
#endif
        }

	    public static void W(string tag, string msg)
	    {
#if DEBUG
			UnityEngine.Debug.LogWarning(tag + ": " + msg);
#endif
        }

	    public static void E(string msg)
	    {
#if DEBUG
	        UnityEngine.Debug.LogError(msg);
#endif
        }

	    public static void V(string msg)
	    {
#if DEBUG
	        UnityEngine.Debug.Log(msg);
#endif
        }

	    public static void W(string msg)
	    {
#if DEBUG
	        UnityEngine.Debug.LogWarning(msg);
#endif
        }
	}
}
