using System;

namespace Lshain
{
	public class Lshain_LogManagerWrap
	{
		public static void Register(IntPtr L)
		{
			LuaMethod[] regs = new LuaMethod[]
			{
				new LuaMethod("E", E),
				new LuaMethod("V", V),
				new LuaMethod("W", W),
				new LuaMethod("New", _CreateLshain_LogManager),
				new LuaMethod("GetClassType", GetClassType),
			};

			LuaScriptMgr.RegisterLib(L, "Lshain.LogManager", regs);
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int _CreateLshain_LogManager(IntPtr L)
		{
			LuaDLL.luaL_error(L, "Lshain.LogManager class does not have a constructor function");
			return 0;
		}

		static Type classType = typeof(Lshain.LogManager);

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int GetClassType(IntPtr L)
		{
			LuaScriptMgr.Push(L, classType);
			return 1;
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int E(IntPtr L)
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				string arg0 = LuaScriptMgr.GetLuaString(L, 1);
				Lshain.LogManager.E(arg0);
				return 0;
			}
			else if (count == 2)
			{
				string arg0 = LuaScriptMgr.GetLuaString(L, 1);
				string arg1 = LuaScriptMgr.GetLuaString(L, 2);
				Lshain.LogManager.E(arg0,arg1);
				return 0;
			}
			else
			{
				LuaDLL.luaL_error(L, "invalid arguments to method: Lshain.LogManager.E");
			}

			return 0;
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int V(IntPtr L)
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				string arg0 = LuaScriptMgr.GetLuaString(L, 1);
				Lshain.LogManager.V(arg0);
				return 0;
			}
			else if (count == 2)
			{
				string arg0 = LuaScriptMgr.GetLuaString(L, 1);
				string arg1 = LuaScriptMgr.GetLuaString(L, 2);
				Lshain.LogManager.V(arg0,arg1);
				return 0;
			}
			else
			{
				LuaDLL.luaL_error(L, "invalid arguments to method: Lshain.LogManager.V");
			}

			return 0;
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int W(IntPtr L)
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				string arg0 = LuaScriptMgr.GetLuaString(L, 1);
				Lshain.LogManager.W(arg0);
				return 0;
			}
			else if (count == 2)
			{
				string arg0 = LuaScriptMgr.GetLuaString(L, 1);
				string arg1 = LuaScriptMgr.GetLuaString(L, 2);
				Lshain.LogManager.W(arg0,arg1);
				return 0;
			}
			else
			{
				LuaDLL.luaL_error(L, "invalid arguments to method: Lshain.LogManager.W");
			}

			return 0;
		}
	}
}
