
using ModelClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Client
{

        class Program
        {
            private static string UrlBase { get; } = "https://localhost:44357";
            private static string SampleController { get; } = "";
            private static string Controller { get; } = "";

            public static void Main()
            {
                Test().Wait();
                Console.ReadKey();
                return;


            }

            private static async Task Test()
            {




                //var server = "http://localhost:8095";
                var server = "https://localhost:44357";
                var method = "s";


            // var result3 = await Get<DbResult>($"https://localhost:44357/api/VirtualTsc/TestSeuquence?seqname=gestown.QR_SERIALCONT", CancellationToken.None);
            //var result4 = await Get<DbResult>($"https://localhost:44357/api/VirtualTsc/TestSeuquence?seqname=gestown.QR_SERIALCONT", CancellationToken.None);
            //var result5 = await Get<DbResult>($"https://localhost:44357/api/VirtualTsc/TestSeuquence?seqname=gestown.QR_SERIALCONT", CancellationToken.None);
            //var result5 = await Get<GetContractInfo>($"https://localhost:44357/api/VirtualTsc/GetContractInfo?saledeviceid=5343&serialno=3466", CancellationToken.None);


            //var param = new VTSource
            //    {
            //        cf = "SLVBNT52C61Z216P",
            //        deviceClassId = 226,
            //        saleDeviceId = 2790,
            //        deviceCode = 2,
            //        Crp = 50,
            //        sellingRegionId = 19,
            //        lingua = VirtualTscModel.Enums.Lingua.IT,
            //        birthDate = new DateTime(1989,12,25),

            //    };


            //    var method2 = "ReadCardWhiteList";
            //    CContractInfo cContractInfo = new CContractInfo();      
            //    cContractInfo.SaleDevice = 2790.ToString();
            //    cContractInfo.DeviceCode = 2.ToString();
            //    cContractInfo.deviceClassId = 226.ToString();
            //    cContractInfo.SellingRegionId = 19.ToString();
            //    cContractInfo.whiteList = false;
            //    var param2 = new VTSourceContracts
            //    {
            //        saledeviceid = 901,
            //        serialno = 23413154,
            //        contractInfo = cContractInfo,


            //    };
            //    string json = Newtonsoft.Json.JsonConvert.SerializeObject(param);

            //    var result0 = await Client.ClientClass.Post<VTInfo, VTSource>(server, VirtualTscController, method, param, CancellationToken.None);
            //    var result1 = await Client.ClientClass.Post<CardInfo, VTSourceContracts>(server, VirtualTscController, method2, param2, CancellationToken.None);

            //    var result2 = await Get<DbResult>($"https://localhost:44357/api/VirtualTsc/CheckTscCf?Cf=TMAMLN97A41F205I", CancellationToken.None);
            ////Console.WriteLine(result.Token);
         
           // var output = result2;




            }

            #region VirtualTscService
            public static async Task InsertSomeRecords()
            {
                var rnd = new Random(DateTime.Now.Millisecond);
                string GetRandomString()
                {
                    var arr = new[]
                    {
                    rnd.Next(65, 91),
                    rnd.Next(65, 91),
                    rnd.Next(65, 91),
                    rnd.Next(65, 91),
                    rnd.Next(65, 91),
                    rnd.Next(65, 91)
                };
                    return string.Join(string.Empty, arr);
                }

                for (var i = 0; i < 3; ++i)
                {
                    var record = new MainTable
                    {
                        Descripton = GetRandomString()
                    };
                    var ok = await Post<bool, MainTable>(UrlBase, Controller, "AddMain", record, CancellationToken.None);
                    Console.WriteLine($"ADD: {(ok ? "SUCCESS" : "FAIL")}");
                }

                var allMain = await Get<List<MainTable>>(UrlBase, Controller, "GetAllFromMain", CancellationToken.None);
                var mainId = (allMain?.Count ?? 0) > 0 ? allMain[rnd.Next(allMain.Count)].Id : Guid.Empty;

                for (var i = 0; i < 3; ++i)
                {
                    var record = new SecondaryTable
                    {
                        MainTableId = mainId,
                        Identifier = GetRandomString(),
                        Value = rnd.Next(1000),
                        AlternativeValue = rnd.Next(2) == 0 ? (decimal?)null : rnd.Next(10)
                    };
                    var ok = await Post<bool, SecondaryTable>(UrlBase, Controller, "AddSecondary", record, CancellationToken.None);
                    Console.WriteLine($"ADD: {(ok ? "SUCCESS" : "FAIL")}");
                }
            }

            public static async Task GetAll()
            {
                var result1 = await Get<List<MainTable>>(UrlBase, Controller, "GetAllFromMain", CancellationToken.None);
                if ((result1?.Count ?? 0) > 0)
                    foreach (var item in result1)
                        Console.WriteLine($"{item.Id}: {item.Descripton}");
                else
                    Console.WriteLine("Empty");
                Console.WriteLine();

                var result2 = await Get<MainTable>(UrlBase, Controller, "GetFromMainById", new UniqueKeyValuePair<string, dynamic>[]
                    {
                    new UniqueKeyValuePair<string, dynamic>("id", "27a8b2a3-f0b2-4462-84eb-889969538fa0")
                    }, CancellationToken.None);
                if (result2 != null)
                {
                    Console.WriteLine($"{result2.Id}: {result2.Descripton}");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Null");
                    Console.WriteLine();
                }

                var result3 = await Get<List<MainTable>>(UrlBase, Controller, "GetFromMainLike", new UniqueKeyValuePair<string, dynamic>[]
                    {
                    new UniqueKeyValuePair<string, dynamic>("like", "part")
                    }, CancellationToken.None);
                if ((result3?.Count ?? 0) > 0)
                    foreach (var item in result3)
                        Console.WriteLine($"{item.Id}: {item.Descripton}");
                else
                    Console.WriteLine("Empty");
                Console.WriteLine();

                var result4 = await Get<List<SecondaryTable>>(UrlBase, Controller, "GetAllFromSecondary", CancellationToken.None);
                if ((result4?.Count ?? 0) > 0)
                    foreach (var item in result4)
                        Console.WriteLine($"{item.Id}: {item.Identifier}, {item.Value}, {item.AlternativeValue}");
                else
                    Console.WriteLine("Empty");
                Console.WriteLine();

                var result5 = await Get<SecondaryTable>(UrlBase, Controller, "GetFromSecondaryById", new UniqueKeyValuePair<string, dynamic>[]
                    {
                    new UniqueKeyValuePair<string, dynamic>("id", "b3071526-8ef7-49a4-aad4-56b77a20f000")
                    }, CancellationToken.None);
                if (result2 != null)
                {
                    Console.WriteLine($"{result5.Id}: {result5.Identifier}, {result5.Value}, {result5.AlternativeValue}");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Null");
                    Console.WriteLine();
                }

                var result6 = await Get<List<SecondaryTable>>(UrlBase, Controller, "GetFromSecondaryByIdentifier", new UniqueKeyValuePair<string, dynamic>[]
                    {
                    new UniqueKeyValuePair<string, dynamic>("identifier", "DEFGHI")
                    }, CancellationToken.None);
                if ((result6?.Count ?? 0) > 0)
                    foreach (var item in result6)
                        Console.WriteLine($"{item.Id}: {item.Identifier}, {item.Value}, {item.AlternativeValue}");
                else
                    Console.WriteLine("Empty");
                Console.WriteLine();

                var result7 = await Get<List<SecondaryTable>>(UrlBase, Controller, "GetFromSecondaryByMainId", new UniqueKeyValuePair<string, dynamic>[]
                    {
                    new UniqueKeyValuePair<string, dynamic>("id", "b3071526-8ef7-49a4-aad4-56b77a20f07a")
                    }, CancellationToken.None);
                if ((result7?.Count ?? 0) > 0)
                    foreach (var item in result7)
                        Console.WriteLine($"{item.Id}: {item.Identifier}, {item.Value}, {item.AlternativeValue}");
                else
                    Console.WriteLine("Empty");
                Console.WriteLine();
            }
            #endregion

            #region Get
            /// <summary>
            /// using RollCodeController
            /// </summary>
            //private static async Task SampleGet()
            //{
            //    //var param = 0 ;
            //    //var result = await Client.ClientClass.Post<Dsd3_GetInfoResult, Dsd3_Info_m>("http://localhost:5000", VirtualTscController, "GetInfo", param, CancellationToken.None);

            //    //var result = await Post<Dsd3_GetInfoResult, Dsd3_Info_m>("http://localhost:5000", RollCodeController, "GetInfo", param, CancellationToken.None);

            //    //var result = await Get<GetInfoModel>($"https://localhost:44357/api/{VirtualTscController}/GetInfo?normalName=Checco&fgjfghf=wewer", CancellationToken.None);
            //    //Console.WriteLine(result.Token);

            //    //var output = result;
            //}
            #endregion

            #region Post
            //private static async Task SamplePost1()
            //{
            //    var parameter = (byte)0; // IMTPORTANT:
            //    var result = await Post<GetInfoModel, byte>(UrlBase, SampleController, "GetInfoViaPost1", parameter, CancellationToken.None);
            //    Console.WriteLine(result.SomeData);

            //    var output = result.SomeData;
            //}

            //private static async Task SamplePost2()
            //{
            //    var parameter = (byte)1; // IMTPORTANT:
            //    var result = await Post<GetInfoModel, byte>(UrlBase, SampleController, "GetInfoViaPost1", parameter, CancellationToken.None);
            //    Console.WriteLine(result.SomeData);

            //    var output = result.SomeData;
            //}

            //private static async Task SamplePost3()
            //{
            //    var parameter = true; // IMTPORTANT:
            //    var result = await Post<GetInfoModel, bool>(UrlBase, SampleController, "GetInfoViaPost2", parameter, CancellationToken.None);
            //    Console.WriteLine(result.SomeData);

            //    var output = result.SomeData;
            //}

            //private static async Task SamplePost4()
            //{
            //    var parameter = false; // IMTPORTANT:
            //    var result = await Post<GetInfoModel, bool>(UrlBase, SampleController, "GetInfoViaPost2", parameter, CancellationToken.None);
            //    Console.WriteLine(result.SomeData);

            //    var output = result.SomeData;
            //}

            //private static async Task SamplePost5()
            //{
            //    var rnd = new Random(DateTime.Now.Millisecond).Next(0, 1000);

            //    var parameter = true; // IMTPORTANT:
            //    var function = "GetInfoViaPost3";
            //    function += $"?";
            //    function += $"additionalData1={rnd}";
            //    function += $"&";
            //    function += $"additionalData2={string.Empty}";
            //    var result = await Post<GetInfoModel, bool>(UrlBase, SampleController, function, parameter, CancellationToken.None);
            //    Console.WriteLine(result.SomeData);

            //    var output = result.SomeData;
            //}

            //private static async Task SamplePost6()
            //{
            //    var rnd = new Random(DateTime.Now.Millisecond).Next(0, 1000);

            //    var parameter = false; // IMTPORTANT:
            //    var function = "GetInfoViaPost3";
            //    function += $"?";
            //    function += $"additionalData1={string.Empty}";
            //    function += $"&";
            //    function += $"additionalData2={rnd}";
            //    var result = await Post<GetInfoModel, bool>(UrlBase, SampleController, function, parameter, CancellationToken.None);
            //    Console.WriteLine(result.SomeData);

            //    var output = result.SomeData;
            //}
            #endregion

            #region Put
            //private static async Task SamplePut()
            //{
            //    try
            //    {
            //        var config = await Get<CommonString>("https://localhost:44357/api/VirtualTsc/GetConfig", CancellationToken.None);
            //        Console.WriteLine(config.Value);

            //        var rnd = new Random(DateTime.Now.Millisecond).Next(0, 1000);
            //        //await Put<CommonString>("https://localhost:44357", Controller, rnd, CancellationToken.None);
            //        //config = await Get<CommonString>("https://localhost:44357/api/VirtualTsc/GetConfig", CancellationToken.None);
            //        //Console.WriteLine(config.Value);

            //        await Put<GetInfoModel>("https://localhost:44357", SampleController, rnd, null, CancellationToken.None);
            //        config = await Get<CommonString>("https://localhost:44357/api/VirtualTsc/GetConfig", CancellationToken.None);
            //        Console.WriteLine(config.Value);
            //        Console.WriteLine("Success");
            //    }
            //    catch
            //    {
            //        Console.WriteLine("Failed");
            //    }
            //}
            #endregion

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

            private static async Task<string> StreamToStringAsync(Stream stream)
            {
                if (stream != null)
                    using (var sr = new StreamReader(stream))
                        return await sr.ReadToEndAsync();
                return null;
            }

            private static async Task<T> Get<T>(string urlBaseAddress, string controllerName, UniqueKeyValuePair<string, dynamic>[] parameters, CancellationToken cancellationToken) =>
                await Get<T>(urlBaseAddress, controllerName, "", parameters, cancellationToken);

            private static async Task<T> Get<T>(string urlBaseAddress, string controllerName, CancellationToken cancellationToken) =>
                await Get<T>(urlBaseAddress, controllerName, "", cancellationToken);

            private static async Task<T> Get<T>(string urlBaseAddress, string controllerName, string functionName, UniqueKeyValuePair<string, dynamic>[] parameters, CancellationToken cancellationToken)
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

            private static async Task<T> Get<T>(string urlBaseAddress, string controllerName, string functionName, CancellationToken cancellationToken)
            {
                var urlParameters = $"{urlBaseAddress}/api/{controllerName}/{functionName}";
                return await Get<T>(urlParameters, cancellationToken);
            }

            private static async Task<T> Get<T>(string url, CancellationToken cancellationToken)
            {
                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                using (var response = await client.SendAsync(request, cancellationToken))
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonD = DeserializeJsonFromStream<dynamic>(stream);
                        return jsonD != null ? JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(jsonD)) : null;
                    }
                    var content = await StreamToStringAsync(stream);
                    throw new Exception();
                }
            }

            private static async Task<T1> Post<T1, T2>(string urlBaseAddress, string controllerName, string function, T2 parameters, CancellationToken cancellationToken)
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
                        var dynJson = DeserializeJsonFromStream<dynamic>(stream);
                        return dynJson != null ? JsonConvert.DeserializeObject<T1>(JsonConvert.SerializeObject(dynJson)) : null;
                    }
                    var content = await StreamToStringAsync(stream);
                    throw new Exception();
                }
            }

            //private static async Task Put<T>(string urlBaseAddress, string controllerName, int id, CancellationToken cancellationToken) where T : class
            //    => await Put<T>(urlBaseAddress, controllerName, id, null as T, cancellationToken);

            private static async Task Put<T>(string urlBaseAddress, string controllerName, int id, T filter, CancellationToken cancellationToken)
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
                    throw new Exception();
                }
            }
        }
    
}
