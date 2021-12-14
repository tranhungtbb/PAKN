using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
    public class WebRequestHelper
    {
        private readonly IConfiguration Configuration;
        public WebRequestHelper(IConfiguration configuration) {
            this.Configuration = configuration;
        }
        public string SendTextFirebaseRequest(string url, object data, string method = "post")
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                var myHttpWebRequest = (HttpWebRequest)request;
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Headers.Add("Authorization", "key=" + Configuration["TokenFireBase"].ToString());
                myHttpWebRequest.Accept = "application/json";
                myHttpWebRequest.Method = method;
                string postData = null;
                if (data.GetType() == typeof(string))
                {
                    postData = data.ToString();
                }
                else
                {
                    postData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                }
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.ContentLength = byteArray.Length;
                Stream dataStream = myHttpWebRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = myHttpWebRequest.GetResponse();
                string contentAll = "";
                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    contentAll = reader.ReadToEnd();
                }
                response.Close();
                return contentAll;
            }
            catch (Exception)
            {

            }
            return "";
        }
    }
}
