using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PAKNAPI.Common
{
    public class ClientRestfull
    {
        //public static void SSL()
        //{
        //    ServicePointManager.Expect100Continue = true;
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
        //           | SecurityProtocolType.Tls11
        //           | SecurityProtocolType.Tls12
        //           | SecurityProtocolType.Ssl3;
        //}
        /// Get data
        public static HttpResponseMessage GetStringAsync(string url, string querry, HeaderRess header)
        {
            //SSL();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(header.ContentType));
                foreach (var item in header.Tkey)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
                return client.GetAsync(querry).Result;
            }

        }
        /// application/x-www-form-urlencoded
        public static HttpResponseMessage PostData(string url, string querry, HeaderRess header, IEnumerable<KeyValuePair<string, string>> postData)
        {
            //SSL();
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(postData);
                content.Headers.ContentType = new MediaTypeHeaderValue(header.ContentType);
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(header.ContentType));
                foreach (var item in header.Tkey)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
                return client.PostAsync(querry, content).Result;
            }
        }
        /// application/json
        public static HttpResponseMessage PostData(string url, string querry, HeaderRess header, string data)
        {
            //SSL();
            using (var client = new HttpClient())
            {
                StringContent jsonContent = new StringContent(data);
                jsonContent.Headers.ContentType = new MediaTypeHeaderValue(header.ContentType);
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(header.ContentType));
                foreach (var item in header.Tkey)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
                return client.PostAsync(querry, jsonContent).Result;
            }
        }

        public class HeaderRess
        {
            public List<KeyValuePair<string, string>> Tkey { set; get; }
            public string ContentType { set; get; }
        }

    }
}
