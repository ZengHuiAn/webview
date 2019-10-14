using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Debug = UnityEngine.Debug;

namespace Log
{
    //
    public static class LogWin
    {
        private static List<string> ClassInfo { get; } = new List<string>
        {
            "UnityEngine",
            "UnityEditor"
        };


        public static List<string> ArrayFilter { get; } = new List<string>
        {
            "Length"
        };

        /*
         *  获取obj的类型
         */
        public static Type GetObjectType(object obj)
        {
            return obj.GetType();
        }

        /*
         * 获取类型的属性
         */
        private static FieldInfo[] GetFieldInfos(Type type)
        {
            return type.GetFields(LogWin.flag);
        }

        public static BindingFlags flag = BindingFlags.Public | BindingFlags.Instance;
        /*
 * 获取类型的属性
 */
        private static PropertyInfo[] GetPropertiesInfos(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }


        private static string FormatFieldInfo(object obj, string tabSpace = "")
        {
            var fieldInfos = GetFieldInfos(obj.GetType());
            var str = tabSpace + "Field:" + "{\n";
            for (var i = 0; i < fieldInfos.Length; i++)
            {
                var field = fieldInfos[i];
                var localStr = $"{tabSpace + "  "}[{field.Name}] = {field.GetValue(obj)}\n";
                str += localStr;
            }

            str = str += tabSpace + "},\n";

            return str;
        }

        #region 将类型转换为字符串  统一入口

        /**
         *  打印日志
         */
        public static string GetObejctInfo(object obj, string tabSpace = "")
        {
            if (Utils.GetLogWinAsset().logMode == LogMode.PRODUCTION)
            {
                return $"{tabSpace}{obj.ToString()}";
            }
            
            var types = obj.GetType();
            var containsRelectionInfo = CheckUnityObject(obj);
            if (types.IsValueType && !types.IsEnum && !types.IsPrimitive)
            {
                return LogWin.GetStructInfo(obj,tabSpace);
            }
            
            // Unity的东西
            if (containsRelectionInfo)
            {
                return GetUnityObejctInfo(obj, tabSpace);
            }

            // 数组
            if (obj.GetType().IsArray)
            {
                return GetObjectArrayInfo(obj, tabSpace);
            }

            
      
            // 泛型
            if (obj.GetType().IsGenericType)
            {
                Debug.Log("泛型");
                return LogWin.GetGenericInfos(obj,tabSpace);
            }

            return $"{tabSpace}{obj.ToString()}";
        }

        #endregion

        public static string GetObjectArrayInfo(object obj, string tabSpace = "")
        {
            if (!obj.GetType().IsArray) return "";

            var str = tabSpace + "Properties:" + "{\n";


            var array = obj as Array;
            var lenStr = string.Format("{0}[{1}] = {2} \n", tabSpace + "  ", "Length", array.Length);
            str += lenStr;
            for (var i = 0; i < array.Length; i++)
            {
                var item = array.GetValue(i);
                var itemStr = GetObejctInfo(item);
                var localStr = string.Format("{0}[{1}] = {2} \n", tabSpace + "  ", i, itemStr);
                str += localStr;
            }

            str = str += tabSpace + "},\n";

            return str;
        }


        public static bool CheckUnityObject(object obj)
        {
            var ns = obj.GetType().Namespace;
            foreach (var value in ClassInfo)
                if (ns != null && ns.Contains(value))
                {
                    return true;
                }

            return false;
        }

        public static string GetUnityObejctInfo(object obj, string tabSpace = "")
        {
            //

            var containsRelectionInfo = CheckUnityObject(obj);


            if (containsRelectionInfo) return GetClassInfo(obj, tabSpace);

            return "";
        }

        public static string GetClassInfo(object obj, string tabSpace = "")
        {
            var str = tabSpace + "{\n";
            str += FormatFieldInfo(obj, tabSpace + "  ");
            
            str += FormatPropertiesInfo(obj, tabSpace + "  ");
            str = str += tabSpace + "},\n";
            return str;
        }

        public static string GetStructInfo(object obj, string tabSpace = "")
        {

            var containsRelectionInfo = CheckUnityObject(obj);
            
            //如果字段的长度是0  并且是
            if (LogWin.GetFieldsLength(obj) == 0 && containsRelectionInfo)
            {
                return "";
            }

            var str = tabSpace +"{\n";
            str += LogWin.GetFieldsLength(obj) == 0 ? "":FormatFieldInfo(obj, tabSpace + "  ");

            if (!containsRelectionInfo)
            {
                str +=$"{ FormatPropertiesInfo(obj, tabSpace + "  ")}";
//                Debug.Log($"{}");
            }
            
            str += (tabSpace + "}\n");
            return str;
        }

        public static int GetFieldsLength(object obj)
        {
            return obj.GetType().GetFields(LogWin.flag).Length;
        }

        public static string GetPropertiesStr(object obj, string tabspace = "")
        {
            var propertyInfos = GetPropertiesInfos(obj.GetType());

            if (propertyInfos.Length == 0)
            {
                return "";
            }
            var str = tabspace + "{\n";

            for (var i = 0; i < propertyInfos.Length; i++)
            {
                var field = propertyInfos[i];

                var attr = field.GetCustomAttributes();
                var isContinus = true;
                foreach (var vv in attr)
                    if (vv is ObsoleteAttribute)
                    {
                        isContinus = false;
                        break;
                    }

                if (!isContinus) continue;
                var localStr = $"{tabspace + "  "}[{field.Name}] = {field.GetValue(obj).ToString()}\n";
                str += localStr;
            }
            str = str += tabspace + "},\n";

            return str;
        }

        public static string FormatPropertiesInfo(object obj, string tabspace = "")
        {
            var propertyInfos = GetPropertiesInfos(obj.GetType());
            return GetPropertiesStr(obj, tabspace);
        }


        public static string GetGenericInfos(object obj, string tabSpace = "")
        {
            var str = tabSpace  +"{\n";
            
            
            var types = obj.GetType();
            var count = types.GetProperty("Count")?.GetValue(obj);
            var gens = (IEnumerable) obj;
            var index = 0;
            foreach (var variable in gens)
            {
                #region 可以优化的版本

                //                var protos = variable.GetType().GetProperty("Key");
//                Debug.Log(protos);
//                if (protos !=null)
//                {
//                    var key = protos.GetValue(variable);
//
//                    var values = variable.GetType().GetProperty("Value");
//
//                    if (values!=null)
//                    {
//                        var fieldsStr = LogWin.GetObejctInfo(variable, tabSpace + "    ");
//                    }
//                }
//                else
//                {
                var fieldsStr = LogWin.GetObejctInfo(variable, tabSpace + "    ");
                str += $"{tabSpace}   [{index++}] = {fieldsStr},\n";
//                } 

                #endregion

            }

            var args = types.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            
            str = str += tabSpace + "},\n";
            return str;
        }
        
        


        public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            var expressionBody = (MemberExpression) memberExpression.Body.Reduce();
            return expressionBody.Member.Name;
        }
    }
}