using Nancy;
using System;
using System.Reflection;

namespace Lucy.DataProcess
{
    public class DataProcessContainer
    {
        #region Constructors

        public DataProcessContainer(MemberInfo member, Request request, dynamic dictionary)
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
            var value = Dictionary[Member.Name] as DynamicDictionaryValue;
            Dictionary[Member.Name] = value != null && value.HasValue
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

        #endregion Methods

        #region Properties

        protected dynamic Dictionary { get; private set; }

        protected MemberInfo Member { get; private set; }

        protected Request Request { get; private set; }

        #endregion Properties
    }
}
