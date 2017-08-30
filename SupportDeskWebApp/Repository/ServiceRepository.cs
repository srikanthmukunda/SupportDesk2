using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Configuration;
using System.Net.Http.Headers;

namespace SupportDeskWebApp.Repository
{
    public class ServiceRepository
    {
        private HttpClient Client { get; set; }

        

        public ServiceRepository()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ServiceUrl"].ToString());
        }

        public HttpResponseMessage GetResponse(string url, string outputMediaType)
        {
            return Client.GetAsync(url).Result;
        }

        public HttpResponseMessage PostRequest(string url, object model, string outputMediaType)
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            if (outputMediaType.ToLower().Equals("plain-text"))
            {
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            }
            else if (outputMediaType.ToUpper().Equals("JSON"))
            {
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            else
            {
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            }

            var result = Client.PostAsJsonAsync(url, model).Result;
            return result;
        }

        public HttpResponseMessage PutRequest(string url, object model, string outputMediaType)
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            if (outputMediaType.ToLower().Equals("plain-text"))
            {
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            }
            else if (outputMediaType.ToUpper().Equals("JSON"))
            {
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            else
            {
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            }

            var result = Client.PutAsJsonAsync(url, model);
            return result.Result;
        }

        public HttpResponseMessage DeleteRequest(string url, string outputMediaType)
        {

            var result = Client.DeleteAsync(url);
            return result.Result;
        }

    }
}