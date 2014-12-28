using Microsoft.Ajax.Utilities;
using Nancy.Helpers;
using Nancy.ModelBinding;
using Nancy.Validation;
using Nancy.ViewEngines;
using Nancy.ViewEngines.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Nancy.Extensions;

// ReSharper disable once CheckNamespace
namespace Lucy
{
    // ReSharper disable once UnusedMember.Global
    public static class LucyHtmlHelpersExtensions
    {
        #region Static Methods

        // Public Methods 



        public static IDisposable BeginForm<TModel>(this HtmlHelpers<TModel> x, string action = null, FormMethod method = FormMethod.Post, dynamic htmlAttributes = null)
        {
            var attr = new Dictionary<string, string>
            {
                {"method", method.ToString().ToLower()}
            };
            if (action != null)
                attr["action"] = x.RenderContext.Context.ToFullPath(action);

            attr.Append((object)htmlAttributes);
            return OpenClose(x, "form", attr);
        }

        public static IDisposable DivFormGroup<TModel>(this HtmlHelpers<TModel> x)
        {
            return OpenClose(x, "div", "class", "form-group");
        }


        public static IHtmlString EditFor<TModel>(this HtmlHelpers<TModel> helpers, Expression<Func<TModel, object>> forNameExpression)
        {
            var member = LucyUtils.GetTargetMemberInfo(forNameExpression);
            return EditFor(helpers, member, forNameExpression.Compile());
        }
        public static IHtmlString EditFor<TModel>(this HtmlHelpers<TModel> x, MemberInfo member, Func<TModel, object> getValue, Dictionary<string, string> lookup = null)
        {
            var tmp = ExtendedMemberInfo.FromMember(member);
            return x.EditFor(tmp, getValue, lookup);
        }
        public static IHtmlString EditFor<TModel>(this HtmlHelpers<TModel> x, ExtendedMemberInfo member, Func<TModel, object> getValue, Dictionary<string, string> lookup = null)
        {
            var dataType = member.GetDataType();
            string markup;

            if (dataType == typeof(string) || dataType == typeof(int))
            {
                var attr = new Dictionary<string, string>
                    {
                 //       { "class", "text-box single-line"},
                  //      { "data-val", "true"},
                        { "id", member.HtmlId},
                        { "name", member.HtmlFormName }
                    };
                if (lookup != null)
                {
                    var sb = new StringBuilder();
                    sb.Append(Tags.Open("select", attr).ToHtmlString());
                    foreach (var i in lookup)
                        sb.AppendFormat("<option value=\"{0}\">{1}</option>", i.Key,
                            new EncodedHtmlString(i.Value).ToHtmlString());
                    sb.Append("</select>\r\n");
                    markup = sb.ToString();
                }
                else
                {
                    attr["id"] = member.HtmlId;
                    attr["name"] = member.HtmlFormName;
                    var value = FormValueFor(x, member, getValue);
                    attr["value"] = value == null ? "" : value.ToString();
                    var toys = x.RenderContext.GetLucyToys();
                    toys.JavascriptBundle.Include("~/Scripts/jquery.validate.js");
                    toys.JavascriptBundle.Include("~/Scripts/jquery.validate.unobtrusive.js");
                    markup = attr.CompileToAttributesWithSelfClosingTag("input");
                }
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

            }
            else
                markup = "??????????";
            return new NonEncodedHtmlString(markup);
        }

        public static string GetEncodedTranslation<TModel>(this HtmlHelpers<TModel> x, string text)
        {
            return HttpUtility.HtmlEncode(GetTranslation(x, text));
        }

        public static string GetTranslationFormat<TModel>(this HtmlHelpers<TModel> x, string format, params object[] parameters)
        {
            format = GetTranslation(x, format);
            return string.Format(format, parameters);
        }

        public static IHtmlString HiddenFor<T>(this HtmlHelpers<T> helpers, Expression<Func<T, object>> expression)
        {
            return MakeSimpleInput(helpers, expression, "hidden");
        }

        public static IHtmlString InlineJavaScript<TModel>(this HtmlHelpers<TModel> x, string script, bool doMinify = false)
        {
            if (string.IsNullOrEmpty(script))
                return new NonEncodedHtmlString("");
            script = doMinify
                ? new Minifier().MinifyJavaScript(script)
                : script.Trim();
            return string.IsNullOrEmpty(script)
                ? new NonEncodedHtmlString("")
                : new NonEncodedHtmlString("<script type=\"text/javascript\">\r\n" + script + "\r\n</script>");
        }

        public static IHtmlString LabelFor(string forName, string text, dynamic htmlAttributes = null)
        {
            var attr = new Dictionary<string, string>
            {
                {"for", forName}
            };
            attr.Append((object)htmlAttributes);
            var markup = attr.CompileToAttributesWithTag("label", HttpUtility.HtmlEncode(text));
            return new NonEncodedHtmlString(markup);
        }

        public static IHtmlString LabelFor<TModel>(this HtmlHelpers<TModel> htmlHelpers, Expression<Func<TModel, object>> expression, dynamic htmlAttributes = null)
        {
            return LabelForMember(htmlHelpers, LucyUtils.GetTargetMemberInfo(expression), htmlAttributes);
        }

        public static IHtmlString LabelForItem<TModel>(this HtmlHelpers<IList<TModel>> htmlHelpers, Expression<Func<TModel, object>> expression, dynamic htmlAttributes = null)
        {
            return LabelForMember(htmlHelpers, LucyUtils.GetTargetMemberInfo(expression), htmlAttributes);
        }

        public static IHtmlString LabelForItem<TModel>(this HtmlHelpers<IEnumerable<TModel>> htmlHelpers, Expression<Func<TModel, object>> expression, dynamic htmlAttributes = null)
        {
            return LabelForMember(htmlHelpers, LucyUtils.GetTargetMemberInfo(expression), htmlAttributes);
        }

        public static IHtmlString LabelForItem<TModel>(this HtmlHelpers<TModel[]> htmlHelpers, Expression<Func<TModel, object>> expression, dynamic htmlAttributes = null)
        {
            return LabelForMember(htmlHelpers, LucyUtils.GetTargetMemberInfo(expression), htmlAttributes);
        }

        public static IHtmlString Link<TModel>(this HtmlHelpers<TModel> x, string label, string url, dynamic htmlAttributes = null)
        {
            return LinkUnencoded(x, HttpUtility.HtmlEncode(label), url, htmlAttributes);
        }

        public static string Glyph<TModel>(this HtmlHelpers<TModel> x, params string[] names)
        {
            return string.Format("<i class=\"{0}\"></i>", string.Join(" ", names));
            // <i class=\"glyphicon glyphicon-hand-right\"></i>
        }

        public static IHtmlString LinkUnencoded<TModel>(this HtmlHelpers<TModel> x, string label, string url, dynamic htmlAttributes = null)
        {
            url = x.RenderContext.Context.ToFullPath(url);
            var attr = new Dictionary<string, string>
            {
                {"href", HttpUtility.HtmlEncode(url)},
            };
            attr.Append((object)htmlAttributes);


            var markup = attr.CompileToAttributesWithTag("a", label);
            return new NonEncodedHtmlString(markup);
        }


        public static IHtmlString MakeSimpleInput<T>(this HtmlHelpers<T> helpers, Expression<Func<T, object>> expression, string inputType)
        {
            var func = expression.Compile();
            var member = LucyUtils.GetTargetMemberInfo(expression);
            return MakeSimpleInput(helpers, func, member.HtmlFormName, inputType);
        }

        public static IHtmlString MakeSimpleInput<T>(this HtmlHelpers<T> helpers, Func<T, object> func, string name, string inputType)
        {
            //var member = expression.GetTargetMemberInfo();
            // var func = expression.Compile();
            var value = func(helpers.Model);

            var markup = string.Format("<input type=\"{0}\" name=\"{1}\" value=\"{2}\" />",
                inputType,
                HttpUtility.HtmlEncode(name),
                HttpUtility.HtmlEncode(HtmlDataSerialize.ToString(value)));
            return new NonEncodedHtmlString(markup);
        }

        public static IHtmlString MakeSimpleInput<T>(HtmlHelpers<T> helpers, string memberName, object value, string inputType)
        {

            var markup = string.Format("<input type=\"{0}\" name=\"{1}\" value=\"{2}\" />",
                inputType,
                HttpUtility.HtmlEncode(memberName),
                HttpUtility.HtmlEncode(HtmlDataSerialize.ToString(value)));
            return new NonEncodedHtmlString(markup);
        }

        public static IDisposable OpenClose<TModel>(this HtmlHelpers<TModel> x, string tag, params string[] attr)
        {
            var lucyToys = x.RenderContext.GetLucyToys();
            var sb = new StringBuilder();
            sb.Append("<" + tag);
            if (attr != null)
                for (var idx = 1; idx < attr.Length; idx += 2)
                    sb.AppendFormat(" {0}=\"{1}\"", attr[idx - 1], attr[idx]);
            sb.AppendFormat(">");
            var openingTag = sb.ToString();
            var closingTag = "</" + tag + ">";
            lucyToys.WriteLiteral(openingTag);
            return new DisposableWithAction(() => lucyToys.WriteLiteral(closingTag));
        }

        public static bool UserIsInRole<TModel>(this HtmlHelpers<TModel> x, string role)
        {
            var u = x.CurrentUser;
            if (u == null || u.Claims == null)
                return false;
            return u.Claims.Any(i => i == role);
        }

        public static IHtmlString ValidationMessageFor<TModel>(this HtmlHelpers<TModel> helpers, Expression<Func<TModel, object>> expression)
        {
            var member = LucyUtils.GetTargetMemberInfo(expression);
            return ValidationMessageFor(helpers, member);
        }
        public static IHtmlString ValidationMessageFor<TModel>(this HtmlHelpers<TModel> helpers, MemberInfo member)
        {
            return ValidationMessageFor(helpers, ExtendedMemberInfo.FromMember(member));
        }
        public static IHtmlString ValidationMessageFor<TModel>(this HtmlHelpers<TModel> helpers, ExtendedMemberInfo member)
        {
            #region Try get binding exception
            {
                var pbException = GetPropertyBindingException(helpers, member.HtmlFormName);
                if (pbException != null)
                    return new EncodedHtmlString(pbException.InnerException != null ? pbException.InnerException.Message : pbException.Message);
            }
            #endregion
            #region Try get validation error
            {
                var validationResult = helpers.RenderContext.Context.ModelValidationResult;
                if (validationResult == null || validationResult.Errors == null)
                    return null;
                IList<ModelValidationError> errors;
                if (!validationResult.Errors.TryGetValue(member.HtmlFormName, out errors))
                    return null;
                if (errors != null && errors.Any())
                    return new EncodedHtmlString(errors[0].ErrorMessage);
            }
            #endregion
            return null;
        }

        public static IHtmlString ValidationSummary<TModel>(this HtmlHelpers<TModel> x, bool param)
        {
            return new NonEncodedHtmlString("ValidationSummary is not implemented yet");
        }
        // Private Methods 

        static object FormValueFor<TModel>(this HtmlHelpers<TModel> x, ExtendedMemberInfo emi, Func<TModel, object> getValueFunction)
        {
            // dodałem getValueFunction, ponieważ czasem wyrażenia są bardziej skomplikowane niż tylko pon
            var exception = GetModelBindingException(x);
            if (exception != null)
            {
                var a = x.RenderContext.Context.Request.Form[emi.HtmlFormName];
                return a.HasValue ? a.Value : null;
            }
            // ReSharper disable once CompareNonConstrainedGenericWithNull
            var value = x.Model == null ? null : getValueFunction(x.Model);
            /*
            var pi = typeof(TModel).GetProperty(propertyName);
            if (pi != null && pi.CanRead && !pi.GetGetMethod().IsStatic && pi.GetIndexParameters().Length == 0)
                return pi.GetValue(x.Model);
            return null;
             */
            return value;
        }

        private static ModelBindingException GetModelBindingException<TModel>(HtmlHelpers<TModel> x)
        {
            var lucyToys = x.RenderContext.GetLucyToys();
            var exception = lucyToys.AttachedExceptions.OfType<ModelBindingException>().FirstOrDefault();
            return exception;
        }

        static PropertyBindingException GetPropertyBindingException<TModel>(HtmlHelpers<TModel> x, string propertyName)
        {
            var exception = GetModelBindingException(x);
            if (exception == null || exception.PropertyBindingExceptions == null)
                return null;
            return exception.PropertyBindingExceptions
                .FirstOrDefault(i => i.PropertyName == propertyName);
        }

        private static string GetTranslation<TModel>(this HtmlHelpers<TModel> x, string text)
        {
            var toys = GetLucyToys(x.RenderContext);
            text = toys.NameProvider.GetTranslation(text, x.CurrentLocale);
            return text;
        }

        public static IHtmlString LabelForMember<TModel>(this HtmlHelpers<TModel> helpers, ExtendedMemberInfo member, dynamic htmlAttributes)
        {
            var toys = GetLucyToys(helpers.RenderContext);
            var text = toys.NameProvider.GetLabelForObjectMember(member.Member, helpers.CurrentLocale);
            return LabelFor(member.HtmlId, text, htmlAttributes);
        }

        public static IHtmlString LabelForMember<TModel>(this HtmlHelpers<TModel> helpers, MemberInfo member, dynamic htmlAttributes)
        {
            return LabelForMember(helpers, ExtendedMemberInfo.FromMember(member), htmlAttributes);
        }

        private static IDisposable OpenClose<TModel>(this HtmlHelpers<TModel> x, string tag, Dictionary<string, string> noEncodedAttributes)
        {
            var lucyToys = x.RenderContext.GetLucyToys();
            var sb = new StringBuilder();
            sb.Append("<" + tag);
            if (noEncodedAttributes != null)
                foreach (var kv in noEncodedAttributes)
                    sb.AppendFormat(" {0}=\"{1}\"", HttpUtility.HtmlEncode(kv.Key), HttpUtility.HtmlEncode(kv.Value));
            sb.AppendFormat(">");
            var openingTag = sb.ToString();
            lucyToys.WriteLiteral(openingTag);
            var closingTag = "</" + tag + ">";
            return new DisposableWithAction(() => lucyToys.WriteLiteral(closingTag));
        }
        // Internal Methods 

        internal static LucyToys GetLucyToys(this IRenderContext x)
        {
            return LucyToys.Get(x.Context.ViewBag);
        }

        #endregion Static Methods
    }
}
