using System.Linq;
using System.Text;
using Nancy;
using System;
using System.Reflection;

namespace Lucy.DataProcess
{
    public class DataProcessContainer
    {
        #region Constructors

        public DataProcessContainer(ExtendedMemberInfo member, Request request, dynamic dictionary)
        {
            Member = member;
            Request = request;
            Dictionary = dictionary;
        }

        #endregion Constructors

        #region Methods

        // Public Methods 

        private DataProcessContainer Process(Func<object, object> processFunction)
        {
            var value = Dictionary[Member.HtmlFormName] as DynamicDictionaryValue;
            Dictionary[Member.HtmlFormName] = value != null && value.HasValue
                ? processFunction(value.Value)
                : processFunction(null);
            return this;
        }

        public DataProcessContainer Trim()
        {
            return Process(value => value == null ? null : value.ToString().Trim());
        }

        /// <summary>
        /// Method removes unnecessary spaces from string value.
        /// </summary>
        /// <returns></returns>
        public DataProcessContainer NormalizeString()
        {
            return Process(value =>
            {
                if (value == null)
                    return null;
                var text = value.ToString().Trim();
                while (text.IndexOf("  ", StringComparison.Ordinal) > 0)
                    text = text.Replace("  ", " ");
                return text;
            });
        }

        public DataProcessContainer RemoveWhiteChars()
        {
            return Process(value =>
            {
                if (value == null)
                    return null;
                var text = value.ToString();
                var s = new StringBuilder(text.Length);
                foreach (var i in text.Where(i => i > ' '))
                    s.Append(i);
                return s.ToString();
            });
        }

        /// <summary>
        /// Converts to upper case string
        /// </summary>
        /// <returns></returns>
        public DataProcessContainer UppercaseString()
        {
            return Process(value => value == null ? null : value.ToString().ToUpper());
        }

        #endregion Methods

        #region Properties

        protected dynamic Dictionary { get; private set; }

        protected ExtendedMemberInfo Member { get; private set; }

        protected Request Request { get; private set; }

        #endregion Properties
    }
}
