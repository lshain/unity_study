using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace LuaInterface
{
    public static class LuaRegistrationHelper
    {
        #region Tagged instance methods
        /// <summary>
        /// Registers all public instance methods in an object tagged with <see cref="LuaGlobalAttribute"/> as Lua global functions
        /// </summary>
        /// <param name="lua">The Lua VM to add the methods to</param>
        /// <param name="o">The object to get the methods from</param>
        public static void TaggedInstanceMethods(LuaState lua, object o)
        {
            #region Sanity checks
            if (lua == null) throw new ArgumentNullException("lua");
            if (o == null) throw new ArgumentNullException("o");
            #endregion

            foreach (MethodInfo method in o.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                foreach (LuaGlobalAttribute attribute in method.GetCustomAttributes(typeof(LuaGlobalAttribute), true))
                {
                    if (string.IsNullOrEmpty(attribute.Name))
                        lua.RegisterFunction(method.Name, o, method); // CLR name
                    else
                        lua.RegisterFunction(attribute.Name, o, method); // Custom name
                }
            }
        }
        #endregion

        #region Tagged static methods
        /// <summary>
        /// Registers all public static methods in a class tagged with <see cref="LuaGlobalAttribute"/> as Lua global functions
        /// </summary>
        /// <param name="lua">The Lua VM to add the methods to</param>
        /// <param name="Type">The class Type to get the methods from</param>
        public static void TaggedStaticMethods(LuaState lua, Type Type)
        {
            #region Sanity checks
            if (lua == null) throw new ArgumentNullException("lua");
            if (Type == null) throw new ArgumentNullException("Type");
            if (!Type.IsClass) throw new ArgumentException("The Type must be a class!", "Type");
            #endregion

            foreach (MethodInfo method in Type.GetMethods(BindingFlags.Static | BindingFlags.Public))
            {
                foreach (LuaGlobalAttribute attribute in method.GetCustomAttributes(typeof(LuaGlobalAttribute), false))
                {
                    if (string.IsNullOrEmpty(attribute.Name))
                        lua.RegisterFunction(method.Name, null, method); // CLR name
                    else
                        lua.RegisterFunction(attribute.Name, null, method); // Custom name
                }
            }
        }
        #endregion

        #region Enumeration
        /// <summary>
        /// Registers an enumeration's values for usage as a Lua variable table
        /// </summary>
        /// <typeparam name="T">The enum Type to register</typeparam>
        /// <param name="lua">The Lua VM to add the enum to</param>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "The Type parameter is used to select an enum Type")]
        public static void Enumeration<T>(LuaState lua)
        {
            #region Sanity checks
            if (lua == null) throw new ArgumentNullException("lua");
            #endregion

            Type Type = typeof(T);
            if (!Type.IsEnum) throw new ArgumentException("The Type must be an enumeration!");

            string[] names = Enum.GetNames(Type);
            T[] values = (T[])Enum.GetValues(Type);

            lua.NewTable(Type.Name);
            for (int i = 0; i < names.Length; i++)
            {
                string path = Type.Name + "." + names[i];
                lua[path] = values[i];
            }
        }
        #endregion
    }
}
