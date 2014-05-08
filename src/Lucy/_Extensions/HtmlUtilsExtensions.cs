using Nancy.Helpers;
using Nancy.ViewEngines.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lucy
{
    public static class HtmlUtilsExtensions
    {
        public static IHtmlString ValidationMessageFor<TModel>(this HtmlHelpers<TModel> x, Expression<Func<TModel, object>> forNameExpression)
        {
            return new NonEncodedHtmlString("ValidationMessage is not implemented yet");
        }

        public static IHtmlString EditFor<TModel>(this HtmlHelpers<TModel> x, Expression<Func<TModel, object>> forNameExpression)
        {

            var member = forNameExpression.GetTargetMemberInfo();
            Type type;
            string markup;
            if (member is PropertyInfo)
                type = (member as PropertyInfo).PropertyType;
            else if (member is FieldInfo)
                type = (member as FieldInfo).FieldType;
            else
                throw new NotSupportedException();
            if (type == typeof(string) || type == typeof(int))
            {
                Dictionary<string, string> attr = new Dictionary<string, string>() {
                    { "class", "text-box single-line"},
                    { "data-val", "true"},
                    { "id", member.Name},
                    { "name", member.Name }
                };

                ////attr.ValidationNumber(Translation.MustBeANumberMessage(member.Name));
                ////if (type == typeof(int))
                ////{
                ////    attr["type"] = "number";
                ////}
                ////else
                ////{
                ////    attr
                ////        .ValidationMinLen(2, Translation.MinLengthMessage(member.Name, 2))
                ////        .ValidationMaxLen(40, Translation.MaxLengthMessage(member.Name, 40));
                ////}

                ////var func = forNameExpression.Compile();
                ////var value = func.Invoke(Owner.Model);
                ////attr["value"] = value == null ? "" : value.ToString();

                //  <input class="text-box single-line" data-val="true" 
                // data-val-number="The field Sort order must be a number." 
                // data-val-required="The Sort order field is required." 
                // id="SortOrder" name="SortOrder" type="number" value="10" />

                //<input class="text-box single-line valid" 
                //data-val="true" 
                ////data-val-maxlength="The field Group name must be a string or array type with a maximum length of '40'." 
                ////data-val-maxlength-max="40" 
                ////data-val-minlength="The field Group name must be a string or array type with a minimum length of '2'." 
                ////data-val-minlength-min="2" 
                //data-val-required="The Group name field is required." 
                //id="GroupName" 
                //name="GroupName" 
                //value="Foundation" 
                //type="text">
                var toys = x.GetLucyToys();
                toys.Javascripts.Add("/Scripts/jquery.validate.js");
                toys.Javascripts.Add("/Scripts/jquery.validate.unobtrusive.js");
                markup = attr.CompileToAttributesWithSelfClosingTag("input");
            }
            else
                markup = "??????????";
            return new NonEncodedHtmlString(markup);
        }

        public static string GetTranslationFormat<TModel>(this HtmlHelpers<TModel> x, string format, params object[] parameters)
        {
            format = GetTranslation(x, format);
            return string.Format(format, parameters);
        }
        public static string GetTranslation<TModel>(this HtmlHelpers<TModel> x, string text)
        {
            var toys = GetLucyToys(x);
            text = toys.NameProvider.GetTranslation(text, x.CurrentLocale);
            return text;
        }

        public static IHtmlString ValidationSummary<TModel>(this HtmlHelpers<TModel> x, bool param)
        {
            return new NonEncodedHtmlString("ValidationSummary is not implemented yet");
        }

        public static bool UserIsInRole<TModel>(this HtmlHelpers<TModel> x, string role)
        {
            var u = x.CurrentUser;
            if (u == null || u.Claims == null)
                return false;
            return u.Claims.Where(i => i == role).Any();
        }

        public static IHtmlString RenderScripts<TModel>(this HtmlHelpers<TModel> x)
        {
            var toys = GetLucyToys(x);
            if (toys.Javascripts.Any())
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in toys.Javascripts.Distinct())
                    sb.AppendFormat("<script src=\"{0}\"></script>\r\n", i);
                return new NonEncodedHtmlString(sb.ToString());
            }
            return new NonEncodedHtmlString(string.Empty);
        }

        public static IHtmlString Link<TModel>(this HtmlHelpers<TModel> x, string label, string url, dynamic htmlAttributes = null)
        {
            url = x.RenderContext.Context.ToFullPath2(url);
            Dictionary<string, string> attr = new Dictionary<string, string>()
            {
                {"href", HttpUtility.HtmlEncode(url)},
            };
            attr.Append((object)htmlAttributes);


            var markup = attr.CompileToAttributesWithTag("a", HttpUtility.HtmlEncode(label));
            return new NonEncodedHtmlString(markup);
        }

        public static string GetEncodedTranslation<TModel>(this HtmlHelpers<TModel> x, string text)
        {
            return HttpUtility.HtmlEncode(GetTranslation(x, text));
        }



        public static IHtmlString LabelFor<TModel>(this HtmlHelpers<TModel> x, Expression<Func<TModel, object>> forNameExpression, dynamic htmlAttributes = null)
        {
            var member = forNameExpression.GetTargetMemberInfo();
            var toys = GetLucyToys(x);
            var text = toys.NameProvider.GetLabelForObjectMember(member, x.CurrentLocale);
            return LabelFor2(member.Name, text, htmlAttributes);
        }

        public static IHtmlString LabelFor2(string forName, string text, dynamic htmlAttributes = null)
        {
            Dictionary<string, string> attr = new Dictionary<string, string>()
            {
                {"for", forName}
            };
            attr.Append((object)htmlAttributes);
            var markup = attr.CompileToAttributesWithTag("label", HttpUtility.HtmlEncode(text));
            return new NonEncodedHtmlString(markup);
        }


        public static LucyToys GetLucyToys<TModel>(this HtmlHelpers<TModel> x)
        {
            var a = x.RenderContext.Context.ViewBag[LucyToys.KEY];
            if (a.HasValue)
            {
                var t = a.Value as LucyToys;
                return t;
            }
            throw new Exception("Lucy's data has not been initialized properly. Derive your view from LucyRazorViewBase.");

        }
        public static IDisposable BeginForm<TModel>(this HtmlHelpers<TModel> x, string action = null, FormMethod method = FormMethod.Post, dynamic htmlAttributes = null)
        {
            Dictionary<string, string> attr = new Dictionary<string, string>()
            {
                {"method", method.ToString().ToLower()}
            };
            if (action != null)
                attr["action"] = Nancy.Extensions.ContextExtensions.ToFullPath(x.RenderContext.Context, action);

            attr.Append((object)htmlAttributes);
            return OpenClose(x, "form", attr);
        }

        public static IDisposable DivFormGroup<TModel>(this HtmlHelpers<TModel> x)
        {
            return OpenClose(x, "div", "class", "form-group");
        }

        public static IDisposable OpenClose<TModel>(this HtmlHelpers<TModel> x, string tag, Dictionary<string, string> noEncodedAttributes)
        {
            var lucyToys = x.GetLucyToys();
            StringBuilder sb = new StringBuilder();
            sb.Append("<" + tag);
            if (noEncodedAttributes != null)
                foreach (var kv in noEncodedAttributes)
                    sb.AppendFormat(" {0}=\"{1}\"", HttpUtility.HtmlEncode(kv.Key), HttpUtility.HtmlEncode(kv.Value));
            sb.AppendFormat(">");
            string openingTag = sb.ToString();
            lucyToys.WriteLiteral(openingTag);
            string closingTag = "</" + tag + ">";
            return new DisposableWithAction(() => lucyToys.WriteLiteral(closingTag));
        }

        public static IDisposable OpenClose<TModel>(this HtmlHelpers<TModel> x, string tag, params string[] attr)
        {
            var lucyToys = x.GetLucyToys();
            StringBuilder sb = new StringBuilder();
            sb.Append("<" + tag);
            if (attr != null)
                for (int idx = 1; idx < attr.Length; idx += 2)
                    sb.AppendFormat(" {0}=\"{1}\"", attr[idx - 1], attr[idx]);
            sb.AppendFormat(">");
            string openingTag = sb.ToString();
            string closingTag = "</" + tag + ">";
            lucyToys.WriteLiteral(openingTag);
            return new DisposableWithAction(() => lucyToys.WriteLiteral(closingTag));
        }
    }
}
