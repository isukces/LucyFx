using Lucy.DataProcess;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lucy
{
    public static class RequestExtensions
    {
        #region Static Methods

        // Public Methods 

        public static DataProcessContainer ProcessFormFor<TModel>(this Request request, Expression<Func<TModel, object>> forNameExpression)
        {
            var member = forNameExpression.GetTargetMemberInfo();
            return new DataProcessContainer(member, request, request.Form);
        }

        public static DataProcessContainer ProcessQueryFor<TModel>(this Request request, Expression<Func<TModel, object>> forNameExpression)
        {
            var member = forNameExpression.GetTargetMemberInfo();
            return new DataProcessContainer(member, request, request.Query);
        }

        #endregion Static Methods
    }
}
