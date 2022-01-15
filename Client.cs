
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System;
using Exceptions;
using VirtualTscModel;
using ModelClient;

namespace Client
{
    public static partial class ClientClass
    {
        public static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default(T);

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                var js = new JsonSerializer();
                var searchResult = js.Deserialize<T>(jtr);
                return searchResult;
            }
        }

        public static object DeserializeJsonFromStream2(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return null;

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                var js = new JsonSerializer();
                var searchResult = js.Deserialize(jtr);
                return searchResult;
            }
        }

        public static T DeserializeJsonFromString<T>(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return default(T);

            using (var sr = new StringReader(str))
            using (var jtr = new JsonTextReader(sr))
            {
                var js = new JsonSerializer();
                var searchResult = js.Deserialize<T>(jtr);
                return searchResult;
            }
        }

        public static async Task<string> StreamToStringAsync(Stream stream)
        {
            if (stream != null)
                using (var sr = new StreamReader(stream))
                    return await sr.ReadToEndAsync();
            return null;
        }

        #region Post
        public static async Task<T1> Post<T1, T2>(string urlBaseAddress, string controllerName, T2 parameters, CancellationToken cancellationToken) where T1 : class =>
            await Post<T1, T2>(urlBaseAddress, controllerName, "", parameters, cancellationToken);

        public static async Task<T1> PostScalar<T1, T2>(string urlBaseAddress, string controllerName, T2 parameters, CancellationToken cancellationToken) where T1 : struct =>
            await PostScalar<T1, T2>(urlBaseAddress, controllerName, "", parameters, cancellationToken);

        public static async Task<T1> Post<T1, T2>(string urlBaseAddress, string controllerName, string function, T2 parameters, CancellationToken cancellationToken) where T1 : class
        {
            var json = JsonConvert.SerializeObject(parameters);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var urlParameters = $"{urlBaseAddress}/api/{controllerName}/{function}";
            using (var client = new HttpClient())
            using (var response = await client.PostAsync(urlParameters, data, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                if (response.IsSuccessStatusCode)
                {
                    //var dynJson = DeserializeJsonFromStream<dynamic>(stream);
                    //return dynJson != null ? JsonConvert.DeserializeObject<T1>(JsonConvert.SerializeObject(dynJson)) : null;
                    var dynJson = DeserializeJsonFromStream2(stream);
                    return dynJson == null ? null : JsonConvert.DeserializeObject<T1>(JsonConvert.SerializeObject(dynJson));
                }
                var content = await StreamToStringAsync(stream);
                throw new ApiException((int)response.StatusCode, content);
            }
        }

        public static async Task<T1> PostScalar<T1, T2>(string urlBaseAddress, string controllerName, string function, T2 parameters, CancellationToken cancellationToken) where T1 : struct
        {
            var json = JsonConvert.SerializeObject(parameters);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var urlParameters = $"{urlBaseAddress}/api/{controllerName}/{function}";
            using (var client = new HttpClient())
            using (var response = await client.PostAsync(urlParameters, data, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                if (response.IsSuccessStatusCode)
                {
                    //var dynJson = DeserializeJsonFromStream<dynamic>(stream);
                    //return dynJson != null ? JsonConvert.DeserializeObject<T1>(JsonConvert.SerializeObject(dynJson)) : null;
                    var dynJson = DeserializeJsonFromStream2(stream);
                    return dynJson != null ? JsonConvert.DeserializeObject<T1>(JsonConvert.SerializeObject(dynJson)) : default;
                }
                var content = await StreamToStringAsync(stream);
                throw new ApiException((int)response.StatusCode, content);
            }
        }
        #endregion

        #region Get
        public static async Task<T> Get<T>(string urlBaseAddress, string controllerName, UniqueKeyValuePair<string, dynamic>[] parameters, CancellationToken cancellationToken) where T : class =>
            await Get<T>(urlBaseAddress, controllerName, "", parameters, cancellationToken);
        public static async Task<T> GetScalar<T>(string urlBaseAddress, string controllerName, UniqueKeyValuePair<string, dynamic>[] parameters, CancellationToken cancellationToken) where T : struct =>
            await GetScalar<T>(urlBaseAddress, controllerName, "", parameters, cancellationToken);

        public static async Task<T> Get<T>(string urlBaseAddress, string controllerName, IList<PredicateFilter> predicates, CancellationToken cancellationToken) where T : class =>
            await Get<T>(urlBaseAddress, controllerName, "", predicates, cancellationToken);

        public static async Task<T> GetScalar<T>(string urlBaseAddress, string controllerName, IList<PredicateFilter> predicates, CancellationToken cancellationToken) where T : struct =>
            await GetScalar<T>(urlBaseAddress, controllerName, "", predicates, cancellationToken);

        public static async Task<T> Get<T>(string urlBaseAddress, string controllerName, CancellationToken cancellationToken) where T : class =>
            await Get<T>(urlBaseAddress, controllerName, "", cancellationToken);

        public static async Task<T> GetScalar<T>(string urlBaseAddress, string controllerName, CancellationToken cancellationToken) where T : struct =>
            await GetScalar<T>(urlBaseAddress, controllerName, "", cancellationToken);

        public static async Task<T> Get<T>(string urlBaseAddress, string controllerName, string functionName, UniqueKeyValuePair<string, dynamic>[] parameters, CancellationToken cancellationToken) where T : class
        {
            var urlParameters = $"{urlBaseAddress}/api/{controllerName}/{functionName}";
            if (parameters != null)
            {
                urlParameters += "?";
                for (var i = 0; i < parameters.Length; ++i)
                {
                    if (parameters[i] == null) continue;
                    urlParameters += $"{parameters[i].Key}={parameters[i].Value}";
                    if (i < parameters.Length - 1) urlParameters += "&";
                }
            }
            return await Get<T>(urlParameters, cancellationToken);
        }

        public static async Task<T> GetScalar<T>(string urlBaseAddress, string controllerName, string functionName, UniqueKeyValuePair<string, dynamic>[] parameters, CancellationToken cancellationToken) where T : struct
        {
            var urlParameters = $"{urlBaseAddress}/api/{controllerName}/{functionName}";
            if (parameters != null)
            {
                urlParameters += "?";
                for (var i = 0; i < parameters.Length; ++i)
                {
                    if (parameters[i] == null) continue;
                    urlParameters += $"{parameters[i].Key}={parameters[i].Value}";
                    if (i < parameters.Length - 1) urlParameters += "&";
                }
            }
            return await GetScalar<T>(urlParameters, cancellationToken);
        }

        public static async Task<T> Get<T>(string urlBaseAddress, string controllerName, string functionName, IList<PredicateFilter> predicates, CancellationToken cancellationToken) where T : class
        {
            var sB = new StringBuilder();
            for (var nLcv = 0; nLcv < predicates.Count; ++nLcv)
                sB.Append(nLcv < predicates.Count - 1 ? $"{predicates[nLcv].ToCustomerParameter()}&" : predicates[nLcv].ToCustomerParameter());
            var urlParameters = $"{urlBaseAddress}/api/{controllerName}/{functionName}?{sB}";
            return await Get<T>(urlParameters, cancellationToken);
        }

        public static async Task<T> GetScalar<T>(string urlBaseAddress, string controllerName, string functionName, IList<PredicateFilter> predicates, CancellationToken cancellationToken) where T : struct
        {
            var sB = new StringBuilder();
            for (var nLcv = 0; nLcv < predicates.Count; ++nLcv)
                sB.Append(nLcv < predicates.Count - 1 ? $"{predicates[nLcv].ToCustomerParameter()}&" : predicates[nLcv].ToCustomerParameter());
            var urlParameters = $"{urlBaseAddress}/api/{controllerName}/{functionName}?{sB}";
            return await GetScalar<T>(urlParameters, cancellationToken);
        }

        public static async Task<T> Get<T>(string urlBaseAddress, string controllerName, string functionName, CancellationToken cancellationToken) where T : class
        {
            var urlParameters = $"{urlBaseAddress}/api/{controllerName}/{functionName}";
            return await Get<T>(urlParameters, cancellationToken);
        }

        public static async Task<T> GetScalar<T>(string urlBaseAddress, string controllerName, string functionName, CancellationToken cancellationToken) where T : struct
        {
            var urlParameters = $"{urlBaseAddress}/api/{controllerName}/{functionName}";
            return await GetScalar<T>(urlParameters, cancellationToken);
        }

        public static async Task<T> Get<T>(string url, CancellationToken cancellationToken) where T : class
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            using (var response = await client.SendAsync(request, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                if (response.IsSuccessStatusCode)
                {
                    //var jsonD = DeserializeJsonFromStream<dynamic>(stream);
                    //return jsonD != null ? JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(jsonD)) : (T)null;
                    var dynJson = DeserializeJsonFromStream2(stream);
                    return dynJson != null ? JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(dynJson)) : null;
                }
                var content = await StreamToStringAsync(stream);
                throw new ApiException((int)response.StatusCode, content);
            }
        }

        public static async Task<T> GetScalar<T>(string url, CancellationToken cancellationToken) where T : struct
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            using (var response = await client.SendAsync(request, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                if (response.IsSuccessStatusCode)
                {
                    //var jsonD = DeserializeJsonFromStream<dynamic>(stream);
                    //return jsonD != null ? JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(jsonD)) : (T)null;
                    var dynJson = DeserializeJsonFromStream2(stream);
                    return dynJson != null ? JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(dynJson)) : default;
                }
                var content = await StreamToStringAsync(stream);
                throw new ApiException((int)response.StatusCode, content);
            }
        }
        #endregion

        #region Put
        public static async Task Put(string urlBaseAddress, string controllerName, int id, List<PredicateFilter> filter, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(filter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var urlParameters = $"{urlBaseAddress}/api/{controllerName}/{id}";
            using (var client = new HttpClient())
            using (var response = await client.PutAsync(urlParameters, data, cancellationToken))
            {
                if (response.IsSuccessStatusCode) return;
                var stream = await response.Content.ReadAsStreamAsync();
                var content = await StreamToStringAsync(stream);
                throw new ApiException((int)response.StatusCode, content);
            }
        }
        #endregion
    }
}
