using System;

namespace LT
{
	public class LT_DelegateFactoryWrap
	{
		public static void Register(IntPtr L)
		{
			LuaMethod[] regs = new LuaMethod[]
			{
				new LuaMethod("Action_GameObject", Action_GameObject),
				new LuaMethod("Action", Action),
				new LuaMethod("UnityEngine_Events_UnityAction", UnityEngine_Events_UnityAction),
				new LuaMethod("System_Reflection_MemberFilter", System_Reflection_MemberFilter),
				new LuaMethod("System_Reflection_TypeFilter", System_Reflection_TypeFilter),
				new LuaMethod("TestLuaDelegate_VoidDelegate", TestLuaDelegate_VoidDelegate),
				new LuaMethod("Camera_CameraCallback", Camera_CameraCallback),
				new LuaMethod("AudioClip_PCMReaderCallback", AudioClip_PCMReaderCallback),
				new LuaMethod("AudioClip_PCMSetPositionCallback", AudioClip_PCMSetPositionCallback),
				new LuaMethod("Application_LogCallback", Application_LogCallback),
				new LuaMethod("Clear", Clear),
				new LuaMethod("New", _CreateLT_DelegateFactory),
				new LuaMethod("GetClassType", GetClassType),
			};

			LuaScriptMgr.RegisterLuaTable(L, "LT.DelegateFactory", regs);
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int _CreateLT_DelegateFactory(IntPtr L)
		{
			LuaDLL.luaL_error(L, "LT.DelegateFactory class does not have a constructor function");
			return 0;
		}

		static Type classType = typeof(LT.DelegateFactory);

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int GetClassType(IntPtr L)
		{
			LuaScriptMgr.Push(L, classType);
			return 1;
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int Action_GameObject(IntPtr L)
		{
			LuaScriptMgr.CheckArgsCount(L, 1);
			LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
			Delegate o = LT.DelegateFactory.Action_GameObject(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int Action(IntPtr L)
		{
			LuaScriptMgr.CheckArgsCount(L, 1);
			LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
			Delegate o = LT.DelegateFactory.Action(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int UnityEngine_Events_UnityAction(IntPtr L)
		{
			LuaScriptMgr.CheckArgsCount(L, 1);
			LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
			Delegate o = LT.DelegateFactory.UnityEngine_Events_UnityAction(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int System_Reflection_MemberFilter(IntPtr L)
		{
			LuaScriptMgr.CheckArgsCount(L, 1);
			LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
			Delegate o = LT.DelegateFactory.System_Reflection_MemberFilter(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int System_Reflection_TypeFilter(IntPtr L)
		{
			LuaScriptMgr.CheckArgsCount(L, 1);
			LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
			Delegate o = LT.DelegateFactory.System_Reflection_TypeFilter(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int TestLuaDelegate_VoidDelegate(IntPtr L)
		{
			LuaScriptMgr.CheckArgsCount(L, 1);
			LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
			Delegate o = LT.DelegateFactory.TestLuaDelegate_VoidDelegate(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int Camera_CameraCallback(IntPtr L)
		{
			LuaScriptMgr.CheckArgsCount(L, 1);
			LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
			Delegate o = LT.DelegateFactory.Camera_CameraCallback(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int AudioClip_PCMReaderCallback(IntPtr L)
		{
			LuaScriptMgr.CheckArgsCount(L, 1);
			LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
			Delegate o = LT.DelegateFactory.AudioClip_PCMReaderCallback(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int AudioClip_PCMSetPositionCallback(IntPtr L)
		{
			LuaScriptMgr.CheckArgsCount(L, 1);
			LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
			Delegate o = LT.DelegateFactory.AudioClip_PCMSetPositionCallback(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int Application_LogCallback(IntPtr L)
		{
			LuaScriptMgr.CheckArgsCount(L, 1);
			LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
			Delegate o = LT.DelegateFactory.Application_LogCallback(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int Clear(IntPtr L)
		{
			LuaScriptMgr.CheckArgsCount(L, 0);
			LT.DelegateFactory.Clear();
			return 0;
		}
	}
}
