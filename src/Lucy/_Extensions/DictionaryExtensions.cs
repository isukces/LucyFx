using System.Collections;
using Nancy.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lucy
{
    public static class DictionaryExtensions
    {

        public static Dictionary<string, string> ValidationNumber(this Dictionary<string, string> dictionary, string message)
        {
            dictionary["data-val-number"] = message;
            return dictionary;
        }

        public static Dictionary<string, string> ValidationMinLen(this Dictionary<string, string> dictionary, int minLength, string message)
        {
            dictionary["data-val-minlength"] = message;
            dictionary["data-val-minlength-min"] = minLength.ToString();
            return dictionary;
        }


        public static Dictionary<string, string> ValidationMaxLen(this Dictionary<string, string> dictionary, int minLength, string message)
        {
            dictionary["data-val-maxlength"] = message;
            dictionary["data-val-maxlength-max"] = minLength.ToString();
            return dictionary;
        }

        public static string CompileToAttributesWithSelfClosingTag(this Dictionary<string, string> dictionary, string tag)
        {
            string a = CompileToAttributes(dictionary);
            return "<" + tag + a + " />";
        }

        public static string CompileToAttributesWithTag(this Dictionary<string, string> dictionary, string tag, string encodedValue)
        {
            string a = CompileToAttributes(dictionary);
            return string.Format("<{0}{1}>{2}</{0}>", tag, a, encodedValue);
        }

        public static string CompileToAttributes(this Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
                return "";
            var sb = new StringBuilder();
            foreach (var pair in dictionary)
                sb.AppendFormat(" {0}=\"{1}\"", HttpUtility.HtmlEncode(pair.Key), HttpUtility.HtmlEncode(pair.Value));
            return sb.ToString();
        }

        public static void Append(this Dictionary<string, string> dictionary, object options)
        {
            if (options == null)
                return;
            var type = options.GetType();
            if (type.IsGenericType)
            {
                var t1 = type.GetGenericTypeDefinition();
                if (t1 == typeof (Dictionary<,>))
                {
                    var a = (options as IDictionary);
                    if (a == null)
                        throw new NullReferenceException("Dictionary<,> is not a IDictionary");
                    foreach (var key in a.Keys.Cast<object>().Where(key => key != null))
                    {
                        var kk = a[key];
                        if (kk==null )continue;
                        dictionary[key.ToString()] = kk.ToString();
                    }
                    return;
                }
            }
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in props)
            {
                if (propertyInfo.GetIndexParameters().Length > 0 || !propertyInfo.CanRead) continue;
                var value = propertyInfo.GetValue(options);
                dictionary[propertyInfo.Name] = value == null ? "" : value.ToString();
            }

        }
    }
}
