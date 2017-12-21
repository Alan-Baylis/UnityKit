using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityKit {
    static public class TypeHelper {
        public static Type Tshort   = typeof(short);
        public static Type Tushort  = typeof(ushort);
        public static Type Tint     = typeof(int);
        public static Type Tuint    = typeof(uint);
        public static Type Tlong    = typeof(long);
        public static Type Tulong   = typeof(ulong);
        public static Type Tbyte    = typeof(byte);
        public static Type Tsbyte   = typeof(sbyte);
        public static Type Tfloat   = typeof(float);
        public static Type Tdouble  = typeof(double);
        public static Type Tbool    = typeof(bool);
        public static Type Tchar    = typeof(char);
        public static Type Tstring  = typeof(string);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static object Parse(this Type type, string str) {
            if (type == Tshort) {
                short s;
                return short.TryParse(str, out s) ? s : 0;
            } else if (type == Tushort) {
                ushort us;
                return ushort.TryParse(str, out us) ? us : 0;
            } else if (type == Tint) {
                int i;
                return int.TryParse(str, out i) ? i : 0;
            } else if (type == Tuint) {
                uint ui;
                return uint.TryParse(str, out ui) ? ui : 0;
            } else if (type == Tlong) {
                long l;
                return long.TryParse(str, out l) ? l : 0;
            } else if (type == Tulong) {
                ulong ul;
                return ulong.TryParse(str, out ul) ? ul : 0;
            } else if (type == Tbyte) {
                byte b;
                return byte.TryParse(str, out b) ? b : 0;
            } else if (type == Tsbyte) {
                sbyte sb;
                return sbyte.TryParse(str, out sb) ? sb : 0;
            } else if (type == Tfloat) {
                float f;
                return float.TryParse(str, out f) ? f : 0;
            } else if (type == Tdouble) {
                double d;
                return double.TryParse(str, out d) ? d : 0;
            } else if (type == Tbool) {
                float num;
                if (str.ToLower() == bool.TrueString.ToLower()) return true;
                if (str.ToLower() == bool.FalseString.ToLower()) return false;
                if (float.TryParse(str, out num)) return !(num == 0);
                else return !string.IsNullOrEmpty(str);
            } else if (type == Tchar) {
                char c;
                return char.TryParse(str, out c) ? c : 0;
            } else if (type == Tstring) {
                return string.IsNullOrEmpty(str) ? string.Empty : str;
            }
            return null;
        }

        /// <summary>
        /// 是否是最基本的值类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBasicType(this Type type) {
            return type == Tshort   || type == Tushort  || type == Tint     ||
                   type == Tuint    || type == Tlong    || type == Tulong   ||
                   type == Tbyte    || type == Tsbyte   || type == Tfloat   ||
                   type == Tdouble  || type == Tbool    || type == Tchar    ||
                   type == Tstring;
        }

        public static bool IsListType(this Type type) {
            while (type != null) {
                if (type.ToString().StartsWith("System.Collections.Generic.List`1")) {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }

        public static bool GetListParamType(this Type type, out Type itemType) {
            if (IsListType(type)) {
                while (type != null) {
                    if (type.IsGenericType) {
                        itemType = type.GetGenericArguments()[0];
                        return true;
                    }
                    type = type.BaseType;
                }
            }

            itemType = null;
            return true;
        }

        public static bool IsMapType(this Type type) {
            while (type != null) {
                if (type.ToString().StartsWith("System.Collections.Generic.Map`2")) {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }

        public static bool GetMapParamType(this Type type, out Type keyType, out Type valueType) {
            if (IsMapType(type)) {
                while (type != null) {
                    if (type.IsGenericType) {
                        var types = type.GetGenericArguments();
                        keyType = types[0];
                        valueType = types[1];
                        return true;
                    }
                    type = type.BaseType;
                }
            }
            keyType = null;
            valueType = null;
            return false;
        }

        public static T GetAttribute<T>(this Type type) {
            var attr = type.GetCustomAttributes(false).ToList().Find(x => x is T);
            return attr == null ? default(T) : (T)attr;
        }
    }
}
