using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Helpers;
using Nancy.ViewEngines.Razor;

namespace Lucy
{
    public static class Tags
    {
        static int AttributeGroup(string attributeName)
        {
            switch (attributeName)
            {
                case "type":
                    return 1;
                case "name":
                    return 10;
                case "value":
                    return 11;
                default:
                    return int.MaxValue;
            }
        }
        public static IHtmlString MakeOpeningOrSelfClosingTag(string tagName, bool sc, dynamic attributes = null)
        {
            var sb = new StringBuilder();
            sb.Append("<" + tagName);
            var addSpace = false;
            if (attributes != null)
            {
                Dictionary<string, object> dictionary = LucyUtils.ToDictionary(attributes);
                foreach (var kv in dictionary.OrderBy(i => AttributeGroup(i.Key)))
                {
                    sb.AppendFormat(" {0}=\"{1}\"", HttpUtility.HtmlEncode(kv.Key), HttpUtility.HtmlEncode(kv.Value.SerializeToHtml()));
                    addSpace = true;
                }
            }
            if (addSpace && sc)
                sb.Append(" ");
            sb.Append(sc ? "/>" : ">");
            return new NonEncodedHtmlString(sb.ToString());
        }

        public static IHtmlString Open(string tagName, dynamic attributes = null)
        {
            return MakeOpeningOrSelfClosingTag(tagName, false, attributes);
        }

        public static IHtmlString SelfClosing(string tagName, dynamic attributes = null)
        {
            return MakeOpeningOrSelfClosingTag(tagName, true, attributes);
        }

        public static class Input
        {
            public static IHtmlString Hiddden(string name, object value, dynamic attributes = null)
            {
                var dict = LucyUtils.ToDictionary(attributes);
                dict["type"] = "hidden";
                dict["name"] = name;
                dict["value"] = value;
                return SelfClosing("input", dict);
            }

            public static IHtmlString CheckBox(string name, object value, bool isChecked, dynamic attributes = null)
            {
                var dict = LucyUtils.ToDictionary(attributes);
                dict["type"] = "checkbox";
                dict["name"] = name;
                dict["value"] = value;
                if (isChecked)
                    dict["checked"] = "checked";
                return SelfClosing("input", dict);
                //                     @Tags.SelfClosing("input", new { type = "checkbox", name = i.HtmlFormName, value = 1, @checked = "checked" });
            }
        }
    }
}
