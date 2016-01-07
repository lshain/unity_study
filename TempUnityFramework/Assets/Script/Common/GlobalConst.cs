using UnityEngine;
using System.Collections;

namespace LT
{
	public static class GlobalConst
	{
		public static readonly string LUA_BASE_DIR = Application.dataPath + "/Script/LuaEngine/Base/";
		public static readonly string LUA_CORE_DIR = Application.dataPath + "/Script/LuaEngine/Core/";
		public static readonly string LUA_WRAPFILE_DIR = Application.dataPath + "/Script/LuaEngine/Wrap/";
        public static readonly string LUA_WARP_IMPORT_FILE = Application.dataPath + "/ScriptLua/import_wrap.lua";

		public static readonly string LUA_BINDER_FILE = Application.dataPath + "/Script/LuaEngine/Base/LuaBinder.cs";
		public static readonly string LUA_DELEGATE_FACTORY_FILE = Application.dataPath + "/Script/LuaEngine/Base/DelegateFactory.cs";
        public static readonly string LUA_SOURCE_DIR = Application.dataPath + "/ScriptLua/";
        public static readonly string LUA_SOURCE_OUT_DIR = Application.dataPath + "/ScriptLua/Out";

        public static readonly string INDENT_NONE = "";
        public static readonly string INDENT_1 = "\t";
        public static readonly string INDENT_2 = "\t\t";
        public static readonly string INDENT_3 = "\t\t\t";
        public static readonly string INDENT_4 = "\t\t\t\t";
        public static readonly string INDENT_5 = "\t\t\t\t\t";

        public static readonly bool USE_PB = false;
        public static readonly bool USE_PBC = false;
        public static readonly bool USE_LPEG = false;
        public static readonly bool USE_CJSON = false;
        public static readonly bool USE_SPROTO = false;

        public static bool isApplePlatform
        {
            get
            {
                return Application.platform == RuntimePlatform.IPhonePlayer ||
                       Application.platform == RuntimePlatform.OSXEditor ||
                       Application.platform == RuntimePlatform.OSXPlayer;
            }
        }
	}
}
