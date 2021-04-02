using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.App_Helper
{
    public static class CommonHelper
    {
        public static T ConvertFormToObj<T>(IFormCollection form) where T : class, new() 
        {
            var obj = new T();
            var type = obj.GetType();
            foreach(var key in form.Keys)
            {
                var prop = type.GetProperty(key);
                var value = Convert.ChangeType(form[key], prop.PropertyType);
                prop.SetValue(obj, value);
            }
            return obj;
        }
    }
}
