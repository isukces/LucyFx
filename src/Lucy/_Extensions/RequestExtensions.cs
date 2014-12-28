using Lucy.DataProcess;
using Nancy;
using System;
using System.Linq.Expressions;

namespace Lucy
{
    public static class RequestExtensions
    {
        #region Static Methods

        // Public Methods 

        public static DataProcessContainer ProcessFormFor<TModel>(this Request request, Expression<Func<TModel, object>> forNameExpression)
        {
            var member = LucyUtils.GetTargetMemberInfo(forNameExpression);
            return new DataProcessContainer(member, request, request.Form);
        }

        public static DataProcessContainer ProcessQueryFor<TModel>(this Request request, Expression<Func<TModel, object>> forNameExpression)
        {
            var member = LucyUtils.GetTargetMemberInfo(forNameExpression);
            return new DataProcessContainer(member, request, request.Query);
        }

        #endregion Static Methods
    }
}
