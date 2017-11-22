using System;
using System.Collections;
using System.Linq.Expressions;

namespace CyberArk.WebApi.Extensions
{
    class HashtableHelper
    {
        public Hashtable Result
        {
            get;set;
        } = new Hashtable(); 

        /// <summary>
        /// Gets a name of a variable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }

        /// <summary>
        /// Adds the variable as a key value pair to a hashtable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashtable"></param>
        /// <param name="memberExpression"></param>
        public static void AddMemberToHashtable<T>(Hashtable hashtable, Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            string membername               = expressionBody.Member.Name;
            object value                    = getValue(expressionBody);

            //Add to hashtable if value not null
            if (hashtable != null && value != null)
                hashtable.Add(membername, value);
        }

        /// <summary>
        /// Gets a Value from a MemberExpression
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        private static object getValue(MemberExpression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter       = getterLambda.Compile();
            return getter();
        }

        /// <summary>
        /// Adds the variable as a key value pair to a hashtable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberExpression"></param>
        public void AddMember<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            string membername               = expressionBody.Member.Name;
            object value                    = getValue(expressionBody);

            //Add to hashtable if value not null
            if (Result != null && value != null)
                Result.Add(membername, value);
        }
    }
}
