using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RequestMapper.Helpers;
using RequestMapper.Models;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Net.Http.Json;
using Azure.Messaging.ServiceBus;
using System.ComponentModel;
using Azure.Core;

namespace RequestMapper
{
    public class RequestMapperFunction
    {
        private readonly ILogger<RequestMapperFunction> _logger;
        private readonly IConfiguration _config;

        public RequestMapperFunction(ILogger<RequestMapperFunction> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [Function("s1a")]
        public async Task<IActionResult> SalesTransactionsRequestMapping([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            Stopwatch swTotal = new Stopwatch();
            swTotal.Start();
            #region variables
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            REQSalesTransactions requestJson = JsonConvert.DeserializeObject<REQSalesTransactions>(requestBody);
            string requestId = req.Headers["RequestId"];
            requestJson.DocumentHeader.Transaction.CorrelationId = requestId;
            string correlationId = requestId;
            string transactionId = requestJson.DocumentHeader.Transaction.TransactionID;
            string transactionIDType = requestJson.DocumentHeader.Transaction.TransactionIDType;
            DateTime transactionTimeStamp = requestJson.DocumentHeader.Transaction.TransactionTimeStamp;
            //this productPromId should be present in requestbody - SalesTransactions/SalesTransactionDetails/Products/ProductProms/ProductPromId
            //but for few requests, it is not present. so gerenating it here because it is not null field
            string productPromId = requestId.Substring(0, 32);
            #endregion

            try
            {
                KeyVaultHelper.ReloadAllSecrets(_config);

                Stopwatch swDatatables = new Stopwatch();
                swDatatables.Start();
                DataTable dtSalesTransactions = new DataTable("SalesTransactions");
                DataTable dtProducts = new DataTable("Products");
                DataTable dtProductProms = new DataTable("ProductProms");
                DataTable dtSaleProms = new DataTable("SaleProms");
                DataTable dtTenders = new DataTable("Tenders");

                Boolean res = MappingHelper.CreateSalesTransactionsDatatables(ref dtSalesTransactions, 
                    ref dtProducts, ref dtProductProms, ref dtSaleProms, ref dtTenders);

                Boolean result = MappingHelper.UpdateSalesTransactionsDatatables(ref dtSalesTransactions, 
                    ref dtProducts, ref dtProductProms, ref dtSaleProms, ref dtTenders, requestJson, productPromId);

                swDatatables.Stop();
                TimeSpan tsDatatables = swDatatables.Elapsed;

                Stopwatch swDatabase = new Stopwatch();
                swDatabase.Start();

                Dictionary<string, Object> parameters = DatabaseHelper.PrepareSalesTransactionsSPParameters(
                    dtSalesTransactions, dtProducts, dtProductProms, dtSaleProms, dtTenders,
                    correlationId, transactionId, transactionIDType, transactionTimeStamp);
                var successMessage = DatabaseHelper.ExecuteStoredProcedure("spAddSalesTransactionBasketInsertV2", parameters);

                swDatabase.Stop();
                TimeSpan tsDatabase = swDatabase.Elapsed;

                swTotal.Stop();
                TimeSpan tsTotal = swTotal.Elapsed;

                var response = new
                {
                    RequestBodySize = req.Headers["RequestBodySize"][0],
                    Endpoint = req.Headers["Endpoint"][0],
                    RequestId = req.Headers["RequestId"][0],
                    Message = successMessage,
                    TimeToCreateDatatables = tsDatatables,
                    TimeToInsertDataIntoDatabase = tsDatabase,
                    TotalTime = tsTotal,
                };

                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + " / " + ex.StackTrace);
                return new BadRequestObjectResult(ex.Message + " / " + ex.StackTrace);
            }

        }

        [Function("s2")]
        public async Task<IActionResult> SalesTotalsRequestMapping([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            Stopwatch swTotal = new Stopwatch();
            swTotal.Start();
            #region variables
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            REQSalesTotals requestJson = JsonConvert.DeserializeObject<REQSalesTotals>(requestBody);
            string requestId = req.Headers["RequestId"];
            requestJson.DocumentHeader.Transaction.CorrelationId = requestId;
            #endregion

            try
            {
                KeyVaultHelper.ReloadAllSecrets(_config);

                Stopwatch swDataTable = Stopwatch.StartNew();
                swDataTable.Start();

                DataTable dtDocumentTransaction = new DataTable("DocumentTransaction");
                DataTable dtSite = new DataTable("Site");
                DataTable dtDepartments = new DataTable("Depts");
                DataTable dtSalesTotals = new DataTable("SalesTotals");

                Boolean res = MappingHelper.CreateSalesTotalsDatatables(ref dtDocumentTransaction, ref dtSite, ref dtDepartments, ref dtSalesTotals);

                Boolean result = MappingHelper.UpdateSalesTotalsDatatables(ref dtDocumentTransaction, ref dtSite, ref dtDepartments, ref dtSalesTotals, requestJson);

                swDataTable.Stop();
                TimeSpan tsDataTables = swDataTable.Elapsed;

                Stopwatch swDatabase = new Stopwatch();
                swDatabase.Start();

                Dictionary<string, Object> parameters = DatabaseHelper.PrepareSalesTotalsSPParameters(
                    dtDocumentTransaction, dtSite, dtDepartments, dtSalesTotals);
                var successMessage = DatabaseHelper.ExecuteStoredProcedure("spInsertSalesTotals2", parameters);

                swDatabase.Stop();
                TimeSpan tsDatabase = swDatabase.Elapsed;

                swTotal.Stop();
                TimeSpan tsTotal = swTotal.Elapsed;

                var response = new
                {
                    SuccessMessage = successMessage,
                    TimeToCreateDatatables = tsDataTables,
                    TimeToInsertDataInDatabase = tsDatabase,
                    TotalTime = tsTotal,
                    Endpoint = req.Headers["Endpoint"][0],
                    RequestId = req.Headers["RequestId"][0],
                    Message = successMessage,
                };
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + " / " + ex.StackTrace);
                var response = new 
                {
                    Message = "Failure",
                    Error = ex.Message + " / " + ex.StackTrace
                };
                return new BadRequestObjectResult(response);
            }

        }

        [Function("s1b")]
        public async Task<IActionResult> SalesTransactionsFileMapping([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            Stopwatch swTotal = new Stopwatch();
            swTotal.Start();
            #region variables
            string requestId = req.Headers["RequestId"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            string unzippedString = string.Empty;
            byte[] requestBytes;
            string blobUrl = string.Empty;
            string productPromId = requestId.Substring(0, 32);
            #endregion

            try
            {
                KeyVaultHelper.ReloadAllSecrets(_config);

                Stopwatch swUnzipFile = new Stopwatch();
                swUnzipFile.Start();

                dynamic jsonData = JsonConvert.DeserializeObject(requestBody);
                string salesFileContent = jsonData?.SalesFileDocument?.SalesFile?.FileContent;

                byte[] byteArray = Convert.FromBase64String(salesFileContent);

                unzippedString = Unzip(byteArray);

                swUnzipFile.Stop();
                TimeSpan tsUnzipFile = swUnzipFile.Elapsed;

                REQSalesTransactions requestJson = JsonConvert.DeserializeObject<REQSalesTransactions>(unzippedString);
                requestJson.DocumentHeader.Transaction.CorrelationId = requestId;
                string correlationId = requestId;
                string transactionId = requestJson.DocumentHeader.Transaction.TransactionID;
                string transactionIDType = requestJson.DocumentHeader.Transaction.TransactionIDType;
                DateTime transactionTimeStamp = requestJson.DocumentHeader.Transaction.TransactionTimeStamp;

                //Stopwatch swUploadToBlob = new Stopwatch();
                //swUploadToBlob.Start();

                //int requesterSiteId = requestJson.SalesTransactions.Site.SiteId;
                //var blobName = $"{requestId}.json";
                //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(KeyVaultHelper.BlobConnectionString);
                //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                //CloudBlobContainer container = blobClient.GetContainerReference("function-files");
                //await container.CreateIfNotExistsAsync();
                //CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

                //requestBytes = Encoding.UTF8.GetBytes(unzippedString);
                //blob.UploadFromByteArrayAsync(requestBytes, 0, requestBytes.Length);
                //while (true)
                //{
                //    if (await blob.ExistsAsync())
                //    {
                //        blobUrl = blob.Uri.AbsoluteUri;
                //        break;
                //    }
                //}
                //swUploadToBlob.Stop();
                //TimeSpan tsUploadToBlob = swUploadToBlob.Elapsed;

                //Stopwatch swSendMessage = new Stopwatch();
                //swSendMessage.Start();

                //var client = new ServiceBusClient(KeyVaultHelper.ServiceBusConnectionString);
                //ServiceBusSender sender = client.CreateSender("sbq-request-mapper");

                //var message = new ServiceBusMessage();
                //message.ApplicationProperties.Add("RequestBlobFilePath", blobUrl);
                //message.ApplicationProperties.Add("RequesterSiteID", requesterSiteId);
                //message.ApplicationProperties.Add("RequestID", requestId);

                //await sender.SendMessageAsync(message);

                //swSendMessage.Stop();
                //TimeSpan tsSendMessage = swSendMessage.Elapsed;

                Stopwatch swDatatables = new Stopwatch();
                swDatatables.Start();

                DataTable dtSite = new DataTable("Site");
                DataTable dtSalesTransactions = new DataTable("SalesTransactions");
                DataTable dtProducts = new DataTable("Products");
                DataTable dtProductProms = new DataTable("ProductProms");
                DataTable dtSaleProms = new DataTable("SaleProms");
                DataTable dtTenders = new DataTable("Tenders");

                Boolean res = MappingHelper.CreateSalesTransactionsDatatables(ref dtSalesTransactions,
                    ref dtProducts, ref dtProductProms, ref dtSaleProms, ref dtTenders);

                Boolean result = MappingHelper.UpdateSalesTransactionsDatatables(ref dtSalesTransactions,
                    ref dtProducts, ref dtProductProms, ref dtSaleProms, ref dtTenders, requestJson, productPromId);

                swDatatables.Stop();
                TimeSpan tsDatatables = swDatatables.Elapsed;

                Stopwatch swDatabase = new Stopwatch();
                swDatabase.Start();

                Dictionary<string, Object> parameters = DatabaseHelper.PrepareSalesTransactionsSPParameters(
                    dtSalesTransactions, dtProducts, dtProductProms, dtSaleProms, dtTenders,
                    correlationId, transactionId, transactionIDType, transactionTimeStamp);
                var successMessage = DatabaseHelper.ExecuteStoredProcedure("spAddSalesTransactionBasketInsertV2", parameters);

                swDatabase.Stop();
                TimeSpan tsDatabase = swDatabase.Elapsed;

                swTotal.Stop();
                TimeSpan tsTotal = swTotal.Elapsed;
                var response = new
                {
                    BlobFileUrl = blobUrl,
                    TimeForUnzipping = tsUnzipFile,
                    //TimeToUploadBlob = tsUploadToBlob,
                    TimeToCreateDatatables = tsDatatables,
                    TimeToInsertDataIntoDatabase = tsDatabase,
                    TotalTime = tsTotal
                };
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + " / " + ex.StackTrace);
                return new BadRequestObjectResult(ex.Message + " / " + ex.StackTrace);
            }

        }

        public static string Unzip(byte[] byteArray)
        {
            using (var zippedStream = new MemoryStream(byteArray))
            {
                using (var archive = new ZipArchive(zippedStream))
                {
                    var entry = archive.Entries.FirstOrDefault();
                    if (entry != null)
                    {
                        using (var unzippedEntryStream = entry.Open())
                        {
                            using (var ms = new MemoryStream())
                            {
                                unzippedEntryStream.CopyTo(ms);
                                var unzippedArray = ms.ToArray();

                                return Encoding.Default.GetString(unzippedArray);
                            }
                        }
                    }

                    return null;
                }
            }
        }

        [Function("DummyFunction")]
        public async Task<IActionResult> DummyFunction([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            try
            {
                KeyVaultHelper.ReloadAllSecrets(_config);
                _logger.LogInformation(req.ToString());
                string requestId = req.Headers["RequestId"];
                string endpoint = req.Headers["Endpoint"][0];
                string requestBodySize = req.Headers["RequestBodySize"][0];
                Dictionary<string, Object> parameters = DatabaseHelper.PrepareAuditParameters(
                    requestId, "Dummy Function", endpoint+" / "+requestBodySize);
                DatabaseHelper.ExecStoredProcedure("spInsertAuditLog", parameters);
                var response = new
                {
                    RequestBodySize = req.Headers["RequestBodySize"][0],
                    Endpoint = req.Headers["Endpoint"][0],
                    RequestId = req.Headers["RequestId"][0],
                    Message = "Dummy message - redirected to secondary endpoint",
                };
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + " / " + ex.StackTrace);
                return new BadRequestObjectResult(ex.Message + " / " + ex.StackTrace);
            }

        }
    }
}
