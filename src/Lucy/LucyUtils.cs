using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Nancy;

namespace Lucy
{
    public static class LucyUtils
    {
        public static ExtendedMemberInfo GetTargetMemberInfo([NotNull] Expression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            switch (expression.NodeType)
            {
                case ExpressionType.Convert:
                    return GetTargetMemberInfo(((UnaryExpression)expression).Operand);
                case ExpressionType.Lambda:
                    return GetTargetMemberInfo(((LambdaExpression)expression).Body);
                case ExpressionType.Call:
                    throw new NotSupportedException();
                case ExpressionType.MemberAccess:
                    {
                        var memberExpression = (MemberExpression)expression;
                        var parent = GetTargetMemberInfo(memberExpression.Expression);
                        var path = parent == null
                            ? memberExpression.Member.Name
                            : string.Format("{0}.{1}", parent.Path, memberExpression.Member.Name);
                        return new ExtendedMemberInfo(memberExpression.Member, path);
                    }
                default:
                    return null;
            }
        }

        [NotNull]
        public static Dictionary<string, object> ToDictionary([CanBeNull] dynamic dyn)
        {
            if (dyn == null)
                return new Dictionary<string, object>();
            var dictionary = dyn as Dictionary<string, object>;
            if (dictionary != null)
                return dictionary;
            Dictionary<string, object> result;
            {
                IDictionary dict2 = dyn as IDictionary;
                if (dict2 != null)
                {
                    result = new Dictionary<string, object>();
                    foreach (var a in dict2.Keys)
                    {
                        result[a.ToString()] = dict2[a];
                    }
                    return result;
                }
            }
            var dynamicDictionary = dyn as DynamicDictionary;
            if (dynamicDictionary != null)
                return dynamicDictionary.ToDictionary();
            result = new Dictionary<string, object>();
            PropertyInfo[] properties = dyn.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var pp in properties)
                if (pp.CanRead && pp.GetIndexParameters().Length == 0)
                    result[pp.Name] = pp.GetValue(dyn);
            return result;
        }

        [NotNull]
        public static string SerializeToHtml([CanBeNull] this object value)
        {
            if (value == null)
                return "";
            var asString = value as string;
            if (asString != null)
                return asString;
            if (value is int)
                return ((int)value).ToString(CultureInfo.InvariantCulture);
            if (value is long)
                return ((long)value).ToString(CultureInfo.InvariantCulture);
            return value.ToString();
        }
    }
}
