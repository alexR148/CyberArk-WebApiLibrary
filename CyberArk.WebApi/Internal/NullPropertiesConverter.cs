using CyberArk.WebApi.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Web.Script.Serialization;

namespace CyberArk.WebApi.Internal
{
    class NullPropertiesConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            //Abort Criteria
            if (type == null || dictionary == null || serializer == null) return null;

            //Create Instance of Type
            object instance = Activator.CreateInstance(type);

            //Get all Properties
            PropertyInfo[] properties = type.GetProperties();

            //Get values from dictionary and write them to the instance
            foreach (PropertyInfo prop in properties)
            {                
                object result;

                //Get value from Dictionary; ignore non existing properties
                if (dictionary.TryGetValue(prop.Name, out result)) 
                    prop.SetValue(instance, result);
            }

            return instance;                       
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var jsonDictionary = new Dictionary<string, object>();
            foreach (var prop in obj.GetType().GetProperties())
            {
                bool ignoreProp = prop.IsDefined(typeof(ScriptIgnoreAttribute), true);
                var value       = prop.GetValue(obj, BindingFlags.Public, null, null, null);

                //Do not add null or ignored properties
                if (value == null || ignoreProp)
                    continue;

                //Do not add integer with value of 0
                if (value.GetType() == typeof(int) && (int)value == 0)
                    continue;

                //Special handling for SecureStrings
                if (value.GetType() == typeof(SecureString))
                {
                    jsonDictionary.Add(prop.Name, ((SecureString)value).ToUnsecureString());
                    continue;
                }

                //Add item to dictionary
                jsonDictionary.Add(prop.Name, value);
                
            }
            return jsonDictionary; 
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get { return GetType().Assembly.GetTypes();}
        }
    }
}
