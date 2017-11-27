using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace CyberArk.WebApi.Internal
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
        /// Converts a deserialized json hashtable to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oarr"></param>
        /// <returns></returns>
        public static T DeserializeArrayToObject<T>(object[] oarr) where T : new()
        {
            //Create a new instance of the input type
            T result = new T();

            //Get type of the inputtype
            Type Tresult = result.GetType();

            //Iterate each item of the object array
            foreach (var value in oarr)            
                DeserializeKeyValuePairToObject(result, value);              
            
            return result;
        }


        public static void DeserializeKeyValuePairToObject(object obj, object keyValuePair)
        {
            //Check if item is a Dictionary
            Dictionary<string, object> ds = keyValuePair as Dictionary<string, object>;
            if (ds != null)
            {
                object name;
                object val;
                ds.TryGetValue("Key", out name);
                ds.TryGetValue("Value", out val);

                PropertyInfo Prop = obj.GetType().GetProperty(name.ToString());

                //Check if property exist
                if (Prop != null)
                {
                    //compare property type and value type
                    if (Prop.PropertyType == val.GetType())
                        //Set propertyvalue
                        Prop.SetValue(obj, val);
                }

                //Clean
                name = null;
                val  = null;
                Prop = null;
            }
        }

        public static Hashtable DeserializeArrayToHashtable(object[] oarr)
        {
            //Create a new instance of the input type
            Hashtable result = new Hashtable();

            //Iterate each item of the object array
            foreach (var value in oarr)
            {
                //Check if item is a Dictionary
                Dictionary<string, object> ds = value as Dictionary<string, object>;
                if (ds != null)
                {
                    object name;
                    object val;
                    if (ds.TryGetValue("Key", out name))
                    {
                        if (ds.TryGetValue("Value", out val))
                        {
                            if (val != null)
                                result.Add(name.ToString(), val);
                        }                      
                   }                   
                    //Clean
                    name = null;
                    val  = null;                    
                }
            }
            return result;
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
