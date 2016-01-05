using UnityEngine;
using System.Collections;
using System;

public class BindType
{
	/*
	 * 类名称
	 */
	public string Name{ get; set; }

	/*
	 * 类Type结构
	 */
	public Type Type{ get; set; }

	/*
	 * 是否静态类
	 */
	public bool IsStatic{ get; set; }

	/*
	 * 父类名字
	 */
	public string BaseName{ get; set; }

	/*
	 * 产生的bind文件名字
	 */
	public string BindFileName{ get; set; }

	/*
	 * 注册到lua中的名字
	 */
	public string BindLuaName{ get; set; }

	public string GetTypeStr(Type t)
	{
		string str = t.ToString();

		if (t.IsGenericType)// 判断是否是泛型类型
		{
			str = GetGenericName(t);
		}
		else if (str.Contains("+"))
		{
			str = str.Replace('+', '.');
		}

		return str;
	}

	private static string[] GetGenericName(Type[] types)
	{
		string[] results = new string[types.Length];

		for (int i = 0; i < types.Length; i++)
		{
			if (types[i].IsGenericType)
			{
				results[i] = GetGenericName(types[i]);
			}
			else
			{
				results[i] = ToLuaExport.GetTypeStr(types[i]);
			}

		}

		return results;
	}

	private static string GetGenericName(Type type)
	{
		if (type.GetGenericArguments().Length == 0)
		{
			return type.Name;
		}

		Type[] gArgs = type.GetGenericArguments();
		string typeName = type.Name;
		string pureTypeName = typeName.Substring(0, typeName.IndexOf('`'));

		return pureTypeName + "<" + string.Join(",", GetGenericName(gArgs)) + ">";
	}

	public BindType(Type t)
	{
		Type = t;

		Name = ToLuaExport.GetTypeStr(t);

		if (t.IsGenericType)
		{
			BindLuaName = ToLuaExport.GetGenericLibName(t);
			BindFileName = ToLuaExport.GetGenericLibName(t);
		}
		else
		{
			BindLuaName = t.FullName.Replace( "+", "." );
			BindFileName = Name.Replace('.', '_');

			if ( Name == "object" )
			{
				BindFileName = "System_Object";
			}
		}

		if ( t.BaseType != null )
		{
			BaseName = ToLuaExport.GetTypeStr(t.BaseType);

			if (BaseName == "ValueType")
			{
				BaseName = null;
			}
		}

		if ( t.GetConstructor(Type.EmptyTypes) == null && t.IsAbstract && t.IsSealed )
		{
			BaseName = null;
			IsStatic = true;
		}
	}

	public BindType SetBaseName(string baseName)
	{
		BaseName = baseName;
		return this;
	}

	public BindType SetBindFileName(string bindFileName)
	{
		BindFileName = bindFileName;
		return this;
	}

	public BindType SetBindLuaName(string bindLuaName)
	{
		BindLuaName = bindLuaName;
		return this;
	}
}
