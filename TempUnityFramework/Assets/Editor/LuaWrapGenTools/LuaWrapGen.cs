using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Object = UnityEngine.Object;

namespace Lshain
{
	[InitializeOnLoad]
	public static class LuaWrapGen
	{
		private static readonly string TAG = "LuaWrapGen";

		public static readonly string LUA_BASE_DIR = Application.dataPath + "/Script/LuaEngine/Base/";
		public static readonly string LUA_CORE_DIR = Application.dataPath + "/Script/LuaEngine/Core/";
		public static readonly string LUA_WRAPFILE_DIR = Application.dataPath + "/Script/LuaEngine/Wrap/";
		public static readonly string LUA_BINDER_FILE = Application.dataPath + "/Script/LuaEngine/Base/LuaBinder.cs";
		public static readonly string LUA_DELEGATE_FACTORY_FILE = Application.dataPath + "/Script/LuaEngine/Base/DelegateFactory.cs";

		static string GetOS ()
		{
#if UNITY_STANDALONE
			return "Win";
#elif UNITY_ANDROID
		return "Android";
#elif UNITY_IPHONE
		return "IOS";
#endif
		}

		static void CreateDir (string dir)
		{
			if (!Directory.Exists (dir)) {
				Directory.CreateDirectory (dir);
			}
		}

		static LuaWrapGen ()
		{

		}

		private class BindType
		{
			/*
	         * 类名称
	         */
			public string Name { get; set; }

			/*
	         * 类Type结构
	         */
			public Type Type { get; set; }

			/*
	         * 是否静态类
	         */
			public bool IsStatic { get; set; }

			/*
	         * 父类名字
	         */
			public string BaseName { get; set; }

			/*
	         * 产生的wrap文件名字
	         */
			public string WrapFileName { get; set; }

			/*
	         * 注册到lua中的名字
	         */
			public string LuaTableName { get; set; }

			public string GetTypeStr (Type t)
			{
				string str = t.ToString ();

				if (t.IsGenericType) {// 判断是否是泛型类型
					str = GetGenericName (t);
				} else if (str.Contains ("+")) {
					str = str.Replace ('+', '.');
				}

				return str;
			}

			private static string[] GetGenericName (Type[] types)
			{
				string[] results = new string[types.Length];

				for (int i = 0; i < types.Length; i++) {
					if (types [i].IsGenericType) {
						results [i] = GetGenericName (types [i]);
					} else {
						results [i] = ToLuaExport.GetTypeStr (types [i]);
					}

				}

				return results;
			}

			private static string GetGenericName (Type Type)
			{
				if (Type.GetGenericArguments ().Length == 0) {
					return Type.Name;
				}

				Type[] gArgs = Type.GetGenericArguments ();
				string typeName = Type.Name;
				string pureTypeName = typeName.Substring (0, typeName.IndexOf ('`'));

				return pureTypeName + "<" + string.Join (",", GetGenericName (gArgs)) + ">";
			}

			public BindType (Type t)
			{
				Type = t;

				Name = ToLuaExport.GetTypeStr (t);

				if (t.IsGenericType) {
					LuaTableName = ToLuaExport.GetGenericLibName (t);
					WrapFileName = ToLuaExport.GetGenericLibName (t);
				} else {
					LuaTableName = t.FullName.Replace ("+", ".");
					WrapFileName = Name.Replace ('.', '_');

					if (Name == "object") {
						WrapFileName = "System_Object";
					}
				}

				if (t.BaseType != null) {
					BaseName = ToLuaExport.GetTypeStr (t.BaseType);

					if (BaseName == "ValueType") {
						BaseName = null;
					}
				}

				if (t.GetConstructor (Type.EmptyTypes) == null && t.IsAbstract && t.IsSealed) {
					BaseName = null;
					IsStatic = true;
				}
			}

			public BindType SetBaseName (string baseName)
			{
				BaseName = baseName;
				return this;
			}

			public BindType SetWrapFileName (string wrapFileName)
			{
				WrapFileName = wrapFileName;
				return this;
			}

			public BindType SetLuaTableName (string luaTableName)
			{
				LuaTableName = luaTableName;
				return this;
			}
		}

		private static BindType _GT (Type t)
		{
			return new BindType (t);
		}

		private static BindType[] _BIND_CLASS_LIST_ = new BindType[] {
			_GT (typeof(object)),
			_GT (typeof(System.String)),
			_GT (typeof(System.Enum)),   
			_GT (typeof(IEnumerator)),
			_GT (typeof(System.Delegate)),        
			_GT (typeof(Type)).SetBaseName ("System.Object"),                                                     
			_GT (typeof(UnityEngine.Object)),

			//测试模板
			//_GT(typeof(Dictionary<int,string>)).SetWrapName("DictInt2Str").SetLibName("DictInt2Str"),

			//custom    
			_GT (typeof(LogManager)),                                                                                    

			//unity                        
			_GT (typeof(Component)),
			_GT (typeof(Behaviour)),
			_GT (typeof(MonoBehaviour)),        
			_GT (typeof(GameObject)),
			_GT (typeof(Transform)),
			_GT (typeof(Space)),

			_GT (typeof(Camera)),   
			_GT (typeof(CameraClearFlags)),           
			_GT (typeof(Material)),
			_GT (typeof(Renderer)),        
			_GT (typeof(MeshRenderer)),
			_GT (typeof(SkinnedMeshRenderer)),
			_GT (typeof(Light)),
			_GT (typeof(LightType)),     
			_GT (typeof(ParticleEmitter)),
			_GT (typeof(ParticleRenderer)),
			_GT (typeof(ParticleAnimator)),        

			_GT (typeof(Physics)),
			_GT (typeof(Collider)),
			_GT (typeof(BoxCollider)),
			_GT (typeof(MeshCollider)),
			_GT (typeof(SphereCollider)),

			_GT (typeof(CharacterController)),

			_GT (typeof(Animation)),             
			_GT (typeof(AnimationClip)).SetBaseName ("UnityEngine.Object"),
			_GT (typeof(TrackedReference)),
			_GT (typeof(AnimationState)),  
			_GT (typeof(QueueMode)),  
			_GT (typeof(PlayMode)),                  

			_GT (typeof(AudioClip)),
			_GT (typeof(AudioSource)),                

			_GT (typeof(Application)),
			_GT (typeof(Input)),    
			_GT (typeof(TouchPhase)),            
			_GT (typeof(KeyCode)),             
			_GT (typeof(Screen)),
			_GT (typeof(Time)),
			_GT (typeof(RenderSettings)),
			_GT (typeof(SleepTimeout)),        

			_GT (typeof(AsyncOperation)).SetBaseName ("System.Object"),
			_GT (typeof(AssetBundle)),   
			_GT (typeof(BlendWeights)),   
			_GT (typeof(QualitySettings)),          
			_GT (typeof(AnimationBlendMode)),    
			_GT (typeof(Texture)),
			_GT (typeof(RenderTexture)),
			_GT (typeof(ParticleSystem)),

			//ngui
			/*
			_GT(typeof(UICamera)),
			_GT(typeof(Localization)),
			_GT(typeof(NGUITools)),

			_GT(typeof(UIRect)),
			_GT(typeof(UIWidget)),        
			_GT(typeof(UIWidgetContainer)),     
			_GT(typeof(UILabel)),        
			_GT(typeof(UIToggle)),
			_GT(typeof(UIBasicSprite)),        
			_GT(typeof(UITexture)),
			_GT(typeof(UISprite)),           
			_GT(typeof(UIProgressBar)),
			_GT(typeof(UISlider)),
			_GT(typeof(UIGrid)),
			_GT(typeof(UIInput)),
			_GT(typeof(UIScrollView)),
			
			_GT(typeof(UITweener)),
			_GT(typeof(TweenWidth)),
			_GT(typeof(TweenRotation)),
			_GT(typeof(TweenPosition)),
			_GT(typeof(TweenScale)),
			_GT(typeof(UICenterOnChild)),    
			_GT(typeof(UIAtlas)),
			*/         
		};

		[MenuItem ("LuaEngine/Gen Lua Wrap Files", false, 1)]
		public static void GenLuaWrapFiles ()
		{
			if (!Application.isPlaying) {
				EditorApplication.isPlaying = true;
			}

			CreateDir (LUA_WRAPFILE_DIR);

			BindType[] list = _BIND_CLASS_LIST_;

			for (int i = 0; i < list.Length; i++) {
				ToLuaExport.Clear ();
				ToLuaExport.Name = list [i].Name;
				ToLuaExport.Type = list [i].Type;
				ToLuaExport.IsStatic = list [i].IsStatic;
				ToLuaExport.BaseName = list [i].BaseName;
				ToLuaExport.WrapFileName = list [i].WrapFileName;
				ToLuaExport.LuaTableName = list [i].LuaTableName;
				ToLuaExport.Generate (null);
			}

			EditorApplication.isPlaying = false;
			LogManager.V ("Generate lua binding files over");
			AssetDatabase.Refresh ();
		}

		[MenuItem ("LuaEngine/Gen Lua Binder Files", false, 3)]
		public static void GenLuaBinderFiles ()
		{
			StringBuilder sb = new StringBuilder ();
			sb.AppendLine ("using System;");
			sb.AppendLine ();
			sb.AppendLine ("public static class LuaBinder");
			sb.AppendLine ("{");
			sb.AppendLine ("\tpublic static void Bind(IntPtr L)");
			sb.AppendLine ("\t{");

			string[] files = Directory.GetFiles (LUA_WRAPFILE_DIR, "*.cs", SearchOption.TopDirectoryOnly);

			for (int i = 0; i < files.Length; i++) {
				string wrapName = Path.GetFileName (files [i]);
				int pos = wrapName.LastIndexOf (".");
				wrapName = wrapName.Substring (0, pos);
				sb.AppendFormat ("\t\t{0}.Register(L);\r\n", wrapName);
			}

			sb.AppendLine ("\t}");
			sb.AppendLine ("}");

			using (StreamWriter textWriter = new StreamWriter (LUA_BINDER_FILE, false, Encoding.UTF8)) {
				textWriter.Write (sb.ToString ());
				textWriter.Flush ();
				textWriter.Close ();
			}
		}

		[MenuItem ("LuaEngine/Clear Lua Binder Files", false, 5)]
		public static void ClearLuaBinderFiles ()
		{
			StringBuilder sb = new StringBuilder ();
			sb.AppendLine ("using System;");
			sb.AppendLine ();
			sb.AppendLine ("public static class LuaBinder");
			sb.AppendLine ("{");
			sb.AppendLine ("\tpublic static void Bind(IntPtr L)");
			sb.AppendLine ("\t{");
			sb.AppendLine ("\t}");
			sb.AppendLine ("}");

			using (StreamWriter textWriter = new StreamWriter (LUA_BINDER_FILE, false, Encoding.UTF8)) {
				textWriter.Write (sb.ToString ());
				textWriter.Flush ();
				textWriter.Close ();
			}

			AssetDatabase.Refresh ();
		}

		public static DelegateType _DT (Type t)
		{
			return new DelegateType (t);
		}

		public static HashSet<Type> GetCustomDelegateTypes ()
		{
			BindType[] list = _BIND_CLASS_LIST_;
			HashSet<Type> set = new HashSet<Type> ();
			BindingFlags binding = BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.Instance;

			for (int i = 0; i < list.Length; i++) {
				Type Type = list [i].Type;
				FieldInfo[] fields = Type.GetFields (BindingFlags.GetField | BindingFlags.SetField | binding);
				PropertyInfo[] props = Type.GetProperties (BindingFlags.GetProperty | BindingFlags.SetProperty | binding);
				MethodInfo[] methods = null;

				if (Type.IsInterface) {
					methods = Type.GetMethods ();
				} else {
					methods = Type.GetMethods (BindingFlags.Instance | binding);
				}

				for (int j = 0; j < fields.Length; j++) {
					Type t = fields [j].FieldType;

					if (typeof(System.Delegate).IsAssignableFrom (t)) {
						set.Add (t);
					}
				}

				for (int j = 0; j < props.Length; j++) {
					Type t = props [j].PropertyType;

					if (typeof(System.Delegate).IsAssignableFrom (t)) {
						set.Add (t);
					}
				}

				for (int j = 0; j < methods.Length; j++) {
					MethodInfo m = methods [j];

					if (m.IsGenericMethod) {
						continue;
					}

					ParameterInfo[] pifs = m.GetParameters ();

					for (int k = 0; k < pifs.Length; k++) {
						Type t = pifs [k].ParameterType;

						if (typeof(System.MulticastDelegate).IsAssignableFrom (t)) {
							set.Add (t);
						}
					}
				}

			}

			return set;
		}

		[MenuItem ("LuaEngine/Gen Lua Delegates", false, 2)]
		public static void GenLuaDelegates ()
		{
			ToLuaExport.Clear ();

			List<DelegateType> list = new List<DelegateType> ();

			list.AddRange (new DelegateType[] {
				_DT (typeof(Action<GameObject>)),
				_DT (typeof(Action)),
				_DT (typeof(UnityEngine.Events.UnityAction)),                     
			});

			HashSet<Type> set = GetCustomDelegateTypes ();
			List<Type> typeList = new List<Type> ();

			foreach (Type t in set) {
				if (null == list.Find ((p) => {
					return p.Type == t;
				})) {
					list.Add (_DT (t));
				}
			}

			ToLuaExport.GenDelegates (list.ToArray ());
			set.Clear ();
			ToLuaExport.Clear ();
			AssetDatabase.Refresh ();
			Debug.Log ("Create lua delegate over");
		}

		static void CopyLuaToOut (string dir)
		{
			string[] files = Directory.GetFiles (Application.dataPath + "/Lua/" + dir, "*.lua", SearchOption.TopDirectoryOnly);
			string outDir = Application.dataPath + "/Lua/Out/" + dir + "/";

			CreateDir (outDir);

			for (int i = 0; i < files.Length; i++) {
				string fname = Path.GetFileName (files [i]);
				FileUtil.CopyFileOrDirectory (files [i], outDir + fname + ".bytes");
			}
		}

		static void BuildLuaBundle (string dir)
		{
			BuildAssetBundleOptions options = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle;

			string[] files = Directory.GetFiles ("Assets/Lua/Out/" + dir, "*.lua.bytes");
			List<Object> list = new List<Object> ();
			string bundleName = dir == null ? "Lua.unity3d" : "Lua_" + dir + ".unity3d";

			CreateDir (Application.dataPath + "/Bundle/");
			CreateDir (string.Format ("{0}/{1}/", Application.persistentDataPath, GetOS ()));

			for (int i = 0; i < files.Length; i++) {
				Object obj = AssetDatabase.LoadMainAssetAtPath (files [i]);
				list.Add (obj);
			}

			if (files.Length > 0) {
				string output = string.Format ("{0}/Bundle/" + bundleName, Application.dataPath);
				BuildPipeline.BuildAssetBundle (null, list.ToArray (), output, options, EditorUserBuildSettings.activeBuildTarget);
				string output1 = string.Format ("{0}/{1}/" + bundleName, Application.persistentDataPath, GetOS ());
				File.Delete (output1);
				File.Copy (output, output1);
				AssetDatabase.Refresh ();
			}
		}

		[MenuItem ("LuaEngine/Build Lua Without JIT", false, 100)]
		public static void BuildLuaNoJit ()
		{
			string dir = Application.dataPath + "/Lua/Out/";

			CreateDir (dir);

			string[] files = Directory.GetFiles (dir, "*.lua.bytes", SearchOption.AllDirectories);

			for (int i = 0; i < files.Length; i++) {
				FileUtil.DeleteFileOrDirectory (files [i]);
			}

			CopyLuaToOut (null);
			AssetDatabase.Refresh ();
			BuildLuaBundle (null);
			UnityEngine.Debug.Log ("编译lua without jit结束");
		}

		[MenuItem ("LuaEngine/Gen U3D Wrap Files", false, 4)]
		public static void U3dBinding ()
		{
			List<string> dropList = new List<string> {      
				//特殊修改
				"UnityEngine.Object",

				//一般情况不需要的类, 编辑器相关的
				"HideInInspector",
				"ExecuteInEditMode",
				"AddComponentMenu",
				"ContextMenu",
				"RequireComponent",
				"DisallowMultipleComponent",
				"SerializeField",
				"AssemblyIsEditorAssembly",
				"Attribute",  //一些列文件，都是编辑器相关的     
				"FFTWindow",

				"Types",
				"UnitySurrogateSelector",            
				"TypeInferenceRules",            
				"ThreadPriority",
				"Debug",        //自定义debugger取代
				"GenericStack",

				//异常，lua无法catch
				"PlayerPrefsException",
				"UnassignedReferenceException",            
				"UnityException",
				"MissingComponentException",
				"MissingReferenceException",

				//RPC网络
				"RPC",
				"Network",
				"MasterServer",
				"BitStream",
				"HostData",
				"ConnectionTesterStatus",

				//unity 自带编辑器GUI
				"GUI",
				"EventType",
				"EventModifiers",
				//"Event",
				"FontStyle",
				"TextAlignment",
				"TextEditor",
				"TextEditorDblClickSnapping",
				"TextGenerator",
				"TextClipping",
				"TextGenerationSettings",
				"TextAnchor",
				"TextAsset",
				"TextWrapMode",
				"Gizmos",
				"ImagePosition",
				"FocusType",


				//地形相关
				"Terrain",                            
				"Tree",
				"SplatPrototype",
				"DetailPrototype",
				"DetailRenderMode",

				//其他
				"MeshSubsetCombineUtility",
				"AOT",
				"Random",
				"Mathf",
				"Social",
				"Enumerator",       
				"SendMouseEvents",               
				"Cursor",
				"Flash",
				"ActionScript",


				//非通用的类
				"ADBannerView",
				"ADInterstitialAd",            
				"Android",
				"jvalue",
				"iPhone",
				"iOS",
				"CalendarIdentifier",
				"CalendarUnit",
				"CalendarUnit",
				"FullScreenMovieControlMode",
				"FullScreenMovieScalingMode",
				"Handheld",
				"LocalNotification",
				"Motion",   //空类
				"NotificationServices",
				"RemoteNotificationType",      
				"RemoteNotification",
				"SamsungTV",
				"TextureCompressionQuality",
				"TouchScreenKeyboardType",
				"TouchScreenKeyboard",
				"MovieTexture",

				//我不需要的
				//2d 类
				"AccelerationEventWrap", //加速
				"AnimatorUtility",
				"AudioChorusFilter",		
				"AudioDistortionFilter",
				"AudioEchoFilter",
				"AudioHighPassFilter",		    
				"AudioLowPassFilter",
				"AudioReverbFilter",
				"AudioReverbPreset",
				"AudioReverbZone",
				"AudioRolloffMode",
				"AudioSettings",		    
				"AudioSpeakerMode",
				"AudioType",
				"AudioVelocityUpdateMode",

				"Ping",
				"Profiler",
				"StaticBatchingUtility",
				"Font",
				"Gyroscope",                        //不需要重力感应
				"ISerializationCallbackReceiver",   //u3d 继承的序列化接口，lua不需要
				"ImageEffectOpaque",                //后处理
				"ImageEffectTransformsToLDR",
				"PrimitiveType",                // 暂时不需要 GameObject.CreatePrimitive           
				"Skybox",                       //不会u3d自带的Skybox
				"SparseTexture",                // mega texture 不需要
				"Plane",
				"PlayerPrefs",

				//不用ugui
				"SpriteAlignment",
				"SpriteMeshType",
				"SpritePackingMode",
				"SpritePackingRotation",
				"SpriteRenderer",
				"Sprite",
				"UIVertex",
				"CanvasGroup",
				"CanvasRenderer",
				"ICanvasRaycastFilter",
				"Canvas",
				"RectTransform",
				"DrivenRectTransformTracker",
				"DrivenTransformProperties",
				"RectTransformAxis",
				"RectTransformEdge",
				"RectTransformUtility",
				"RectTransform",
				"UICharInfo",
				"UILineInfo",

				//不需要轮子碰撞体
				"WheelCollider",
				"WheelFrictionCurve",
				"WheelHit",

				//手机不适用雾
				"FogMode",

				"UnityEventBase",
				"UnityEventCallState",
				"UnityEvent",

				"LightProbeGroup",
				"LightProbes",

				"NPOTSupport", //只是SystemInfo 的一个枚举值

				//没用到substance纹理
				"ProceduralCacheSize",
				"ProceduralLoadingBehavior",
				"ProceduralMaterial",
				"ProceduralOutputType",
				"ProceduralProcessorUsage",
				"ProceduralPropertyDescription",
				"ProceduralPropertyType",
				"ProceduralTexture",

				//物理关节系统
				"JointDriveMode",
				"JointDrive",
				"JointLimits",		
				"JointMotor",
				"JointProjectionMode",
				"JointSpring",
				"SoftJointLimit",
				"SpringJoint",
				"HingeJoint",
				"FixedJoint",
				"ConfigurableJoint",
				"CharacterJoint",            
				"Joint",

				"LODGroup",
				"LOD",

				"DataUtility",          //给sprite使用的
				"CrashReport",
				"CombineInstance",
			};

			List<BindType> list = new List<BindType> ();
			Assembly assembly = Assembly.Load ("UnityEngine");
			Type[] types = assembly.GetExportedTypes ();

			for (int i = 0; i < types.Length; i++) {
				//不导出： 模版类，event委托, c#协同相关, obsolete 类
				if (!types [i].IsGenericType && types [i].BaseType != typeof(System.MulticastDelegate) &&
				   !typeof(YieldInstruction).IsAssignableFrom (types [i]) && !ToLuaExport.IsObsolete (types [i])) {
					list.Add (_GT (types [i]));
				} else {
					Debug.Log ("drop generic Type " + types [i].ToString ());
				}
			}

			for (int i = 0; i < dropList.Count; i++) {
				list.RemoveAll ((p) => {
					return p.Type.ToString ().Contains (dropList [i]);
				});
			}

			//for (int i = 0; i < list.Count; i++)
			//{
			//    if (!typeof(UnityEngine.Object).IsAssignableFrom(list[i].Type) && !list[i].Type.IsEnum && !typeof(UnityEngine.TrackedReference).IsAssignableFrom(list[i].Type)
			//        && !list[i].Type.IsValueType && !list[i].Type.IsSealed)            
			//    {
			//        Debug.Log(list[i].Type.Name);
			//    }
			//}

			for (int i = 0; i < list.Count; i++) {
				try {
					ToLuaExport.Clear ();
					ToLuaExport.Name = list [i].Name;
					ToLuaExport.Type = list [i].Type;
					ToLuaExport.IsStatic = list [i].IsStatic;
					ToLuaExport.BaseName = list [i].BaseName;
					ToLuaExport.WrapFileName = list [i].WrapFileName;
					ToLuaExport.LuaTableName = list [i].LuaTableName;
					ToLuaExport.Generate (null);
				} catch (Exception e) {
					Debug.LogWarning ("Generate wrap file error: " + e.ToString ());
				}
			}

			GenLuaBinderFiles ();
			Debug.Log ("Generate lua binding files over， Generate " + list.Count + " files");
			AssetDatabase.Refresh ();
		}
	}
}
