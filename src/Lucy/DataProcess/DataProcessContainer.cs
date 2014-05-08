using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lucy.DataProcess
{
    public class DataProcessContainer
    {
        #region Constructors

        public DataProcessContainer(MemberInfo member, Request request, dynamic dictionary)
        {
            this.Member = member;
            this.Request = request;
            this.Dictionary = dictionary;
        }

        #endregion Constructors

        #region Methods

        // Public Methods 

        public DataProcessContainer Process(Func<object, object> processFunction)
        {
            var value = Dictionary[Member.Name] as Nancy.DynamicDictionaryValue;
            if (value.HasValue)
                Dictionary[Member.Name] = processFunction(value.Value);
            else
                Dictionary[Member.Name] = processFunction(null);
            return this;
        }

        public DataProcessContainer Trim()
        {
            return Process((value) =>
            {
                if (value == null)
                    return null;
                return value.ToString().Trim();
            });
        }

        public DataProcessContainer NormalizeString()
        {
            return Process((value) =>
            {
                if (value == null)
                    return null;
                var text = value.ToString().Trim();
                while (text.IndexOf("  ") > 0)
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
