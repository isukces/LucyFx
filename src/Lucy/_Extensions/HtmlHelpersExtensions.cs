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
    public static class HtmlHelpersExtensions
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
                var attr = new Dictionary<string, string>
                {
                    { "class", "text-box single-line"},
                    { "data-val", "true"},
                    { "id", member.Name},
                    { "name", member.Name }
                };
                var value = FormValueFor(x, member.Name);
                attr["value"] = value == null ? "" : value.ToString();
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
                var toys = x.RenderContext.GetLucyToys();
                toys.JavascriptBundle.Include("~/Scripts/jquery.validate.js");
                toys.JavascriptBundle.Include("~/Scripts/jquery.validate.unobtrusive.js");
                markup = attr.CompileToAttributesWithSelfClosingTag("input");
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

        public static IHtmlString LabelFor<TModel>(this HtmlHelpers<TModel> htmlHelpers, Expression<Func<TModel, object>> forNameExpression, dynamic htmlAttributes = null)
        {
            return LabelForMember(htmlHelpers, htmlAttributes, forNameExpression.GetTargetMemberInfo());
        }

        public static IHtmlString LabelForItem<TModel>(this HtmlHelpers<IList<TModel>> htmlHelpers, Expression<Func<TModel, object>> forNameExpression, dynamic htmlAttributes = null)
        {
            return LabelForMember(htmlHelpers, htmlAttributes, forNameExpression.GetTargetMemberInfo());
        }

        public static IHtmlString LabelForItem<TModel>(this HtmlHelpers<IEnumerable<TModel>> htmlHelpers, Expression<Func<TModel, object>> forNameExpression, dynamic htmlAttributes = null)
        {
            return LabelForMember(htmlHelpers, htmlAttributes, forNameExpression.GetTargetMemberInfo());
        }

        public static IHtmlString LabelForItem<TModel>(this HtmlHelpers<TModel[]> htmlHelpers, Expression<Func<TModel, object>> forNameExpression, dynamic htmlAttributes = null)
        {
            return LabelForMember(htmlHelpers, htmlAttributes, forNameExpression.GetTargetMemberInfo());
        }

        public static IHtmlString Link<TModel>(this HtmlHelpers<TModel> x, string label, string url, dynamic htmlAttributes = null)
        {
            url = x.RenderContext.Context.ToFullPath(url);
            var attr = new Dictionary<string, string>
            {
                {"href", HttpUtility.HtmlEncode(url)},
            };
            attr.Append((object)htmlAttributes);


            var markup = attr.CompileToAttributesWithTag("a", HttpUtility.HtmlEncode(label));
            return new NonEncodedHtmlString(markup);
        }

        public static IDisposable OpenClose<TModel>(this HtmlHelpers<TModel> x, string tag, params string[] attr)
        {
            var lucyToys = x.RenderContext.GetLucyToys();
            var sb = new StringBuilder();
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

        public static bool UserIsInRole<TModel>(this HtmlHelpers<TModel> x, string role)
        {
            var u = x.CurrentUser;
            if (u == null || u.Claims == null)
                return false;
            return u.Claims.Any(i => i == role);
        }

        public static IHtmlString ValidationMessageFor<TModel>(this HtmlHelpers<TModel> x, Expression<Func<TModel, object>> forNameExpression)
        {
            var member = forNameExpression.GetTargetMemberInfo();
            #region Try get binding exception
            {
                var pbException = GetPropertyBindingException(x, member.Name);
                if (pbException != null)
                    return new EncodedHtmlString(pbException.InnerException != null ? pbException.InnerException.Message : pbException.Message);
            }
            #endregion
            #region Try get validation error
            {
                var validationResult = x.RenderContext.Context.ModelValidationResult;
                if (validationResult == null || validationResult.Errors == null)
                    return null;
                IList<ModelValidationError> errors;
                if (!validationResult.Errors.TryGetValue(member.Name, out errors))
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

        static object FormValueFor<TModel>(this HtmlHelpers<TModel> x, string propertyName)
        {
            var exception = GetModelBindingException(x);
            if (exception != null)
            {
                var a = x.RenderContext.Context.Request.Form[propertyName];
                return a.HasValue ? a.Value : null;
            }
            var pi = typeof(TModel).GetProperty(propertyName);
            if (pi != null && pi.CanRead && !pi.GetGetMethod().IsStatic && pi.GetIndexParameters().Length == 0)
                return pi.GetValue(x.Model);
            return null;
        }

        private static ModelBindingException GetModelBindingException<TModel>(HtmlHelpers<TModel> x)
        {
            LucyToys lt = x.RenderContext.GetLucyToys();
            ModelBindingException exception = lt.AttachedExceptions.OfType<ModelBindingException>().FirstOrDefault();
            return exception;
        }

        static PropertyBindingException GetPropertyBindingException<TModel>(HtmlHelpers<TModel> x, string propertyName)
        {
            ModelBindingException exception = GetModelBindingException(x);
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

        private static IHtmlString LabelForMember<TModel>(HtmlHelpers<TModel> x, dynamic htmlAttributes, MemberInfo member)
        {
            var toys = GetLucyToys(x.RenderContext);
            var text = toys.NameProvider.GetLabelForObjectMember(member, x.CurrentLocale);
            return LabelFor(member.Name, text, htmlAttributes);
        }

        private static IHtmlString MakeSimpleInput<T>(HtmlHelpers<T> helpers, Expression<Func<T, object>> expression, string tagName)
        {
            var member = expression.GetTargetMemberInfo();
            var func = expression.Compile();
            var value = func.Invoke(helpers.Model);

            var markup = string.Format("<input type=\"{0}\" name=\"{1}\" value=\"{2}\" />",
                tagName,
                HttpUtility.HtmlEncode(member.Name),
                HttpUtility.HtmlEncode(HtmlDataSerialize.ToString(value)));
            return new NonEncodedHtmlString(markup);
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
            string openingTag = sb.ToString();
            lucyToys.WriteLiteral(openingTag);
            string closingTag = "</" + tag + ">";
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
