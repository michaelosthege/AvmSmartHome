using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace AvmSmartHome.NET
{
    public static class Helpers
    {
        public static async Task<string> GetAsync(string url)
        {
            var httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            Task<string> responseBody = response.Content.ReadAsStringAsync();
            string result = responseBody.Result;
            httpClient.Dispose();
            return result;
        }

        public static async Task<string> PostFormAsync(string url, Dictionary<string,string> parameters)
        {
            string result = null;
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                MultipartFormDataContent form = new MultipartFormDataContent();
                
                foreach (KeyValuePair<string,string> item in parameters)
                {
                    form.Add(new StringContent(item.Value), item.Key);
                }
                System.Net.Http.HttpResponseMessage response = await httpClient.PostAsync(url, form);

                response.EnsureSuccessStatusCode();

                Task<string> responseBody = response.Content.ReadAsStringAsync();
                result = responseBody.Result;
            }
            return result;
        }
        
        public static T DeserializeXML<T>(this string objectData)
        {
            var serializer = new XmlSerializer(typeof(T));
            T result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = (T)serializer.Deserialize(reader);
            }

            return result;
        }

        public static string ComputeMD5(string str, Encoding encoding)
        {
            MD5 md5 = MD5.Create();
            byte[] buffer = encoding.GetBytes(str);
            byte[] hashed = md5.ComputeHash(buffer);
            string hex = BitConverter.ToString(hashed).Replace("-", string.Empty).ToLower();
            return hex;
        }
    }
}
