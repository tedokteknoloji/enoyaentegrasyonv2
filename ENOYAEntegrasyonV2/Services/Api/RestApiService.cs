using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ENOYAEntegrasyonV2.Business;
using ENOYAEntegrasyonV2.Models.Configuration;
using ENOYAEntegrasyonV2.Models.Entities;
using ENOYAEntegrasyonV2.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ENOYAEntegrasyonV2.Services.Api
{
    /// <summary>
    /// REST API servisi (IFS)
    /// Postman collection'daki endpoint'lere göre oluşturuldu
    /// </summary>
    public class RestApiService : IRestApiService
    {
        private readonly ApiSettings _settings;
        private readonly ILoggerService _logger;
        private readonly HttpClient _httpClient;
        private string _accessToken;
        private DateTime _tokenExpiryTime;

        public RestApiService(ApiSettings settings, ILoggerService logger)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_settings.BaseUrl),
                Timeout = TimeSpan.FromSeconds(_settings.TimeoutSeconds)
            };
        }

        /// <summary>
        /// OAuth2 token al
        /// </summary>
        public async Task<string> GetAccessTokenAsync()
        {
            // Token hala geçerliyse mevcut token'ı döndür
            if (!string.IsNullOrEmpty(_accessToken) && DateTime.Now < _tokenExpiryTime)
            {
                return _accessToken;
            }

            try
            {
                var tokenUrl = _settings.GetFullTokenUrl();
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", _settings.ClientId),
                    new KeyValuePair<string, string>("client_secret", _settings.ClientSecret)
                });

                var response = await _httpClient.PostAsync(tokenUrl, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                //string restoke = "{\"access_token\":\"eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJ5bnFWWXk4eEh4UWFjcGh5ZWNJYXhtMHh6M2hZbjFCTW5pYUNvamkxRlo4In0.eyJleHAiOjE3NjM1MDI2NjksImlhdCI6MTc2MzUwMjQ4OSwianRpIjoib25ydGNjOmJlOGUzODQyLTRmZjMtNDNiNi1iNmVhLTRiN2E1MGVhYmM2ZSIsImlzcyI6Imh0dHBzOi8vdGVzdDJpZnMuYnVyc2FiZXRvbi5jb20udHIvYXV0aC9yZWFsbXMvdGVzdDIiLCJhdWQiOlsidGVzdDIiLCJhY2NvdW50Il0sInN1YiI6IjgzYTI1MjI3LTE4YjctNDdkZS04ZTNlLTU4MTQ1OTJhZjYzZSIsInR5cCI6IkJlYXJlciIsImF6cCI6IkJVUkJFVF9NRVNfRU5UIiwic2lkIjoiOWI4M2FjNTMtNjcwMC00MjY4LTk4MDEtNWUwNDE4OWEwNjMyIiwicmVhbG1fYWNjZXNzIjp7InJvbGVzIjpbIm9mZmxpbmVfYWNjZXNzIiwiZGVmYXVsdC1yb2xlcy10ZXN0MiIsInVtYV9hdXRob3JpemF0aW9uIl19LCJyZXNvdXJjZV9hY2Nlc3MiOnsiYWNjb3VudCI6eyJyb2xlcyI6WyJtYW5hZ2UtYWNjb3VudCIsIm1hbmFnZS1hY2NvdW50LWxpbmtzIiwidmlldy1wcm9maWxlIl19fSwic2NvcGUiOiJlbWFpbCBwcm9maWxlIG1pY3JvcHJvZmlsZS1qd3QgYXVkaWVuY2UiLCJ1cG4iOiJzZXJ2aWNlLWFjY291bnQtYnVyYmV0X21lc19lbnQiLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImNsaWVudEhvc3QiOiIxMC4yMTIuMTM1LjIiLCJncm91cHMiOlsib2ZmbGluZV9hY2Nlc3MiLCJkZWZhdWx0LXJvbGVzLXRlc3QyIiwidW1hX2F1dGhvcml6YXRpb24iXSwicHJlZmVycmVkX3VzZXJuYW1lIjoic2VydmljZS1hY2NvdW50LWJ1cmJldF9tZXNfZW50IiwiY2xpZW50QWRkcmVzcyI6IjEwLjIxMi4xMzUuMiIsImNsaWVudF9pZCI6IkJVUkJFVF9NRVNfRU5UIn0.Ka6WFc8Woxdo8osfRfX1aqLWUXPRJTtXJ_2rYNL3dkCO1JjjhPe2PolYguPAkiz3MBvNlh7HozerGYEYY1hB_jzD2kX4JSdFW71-1JH4ulWr6TA47ux-VfsttmJPkPCSDu7_lb8TietuNDwlo_3Xj1BoJEOgTPN_lQqC3VY4Umph2xEzbjeujHOAHqKU9JmrQJJKBg5G1PwLI0txapDyZmFbMXOpBVoRepG0BuoFN-aXVsxYBkRpuQmmeOm_5nCSl7xja3SevLoMEaXbTVwfLfbxAbBULOHuSQb8TOQML5fE8YVvKxTT1EABQABfZdOVV3TX-BkqiCtTVcf4c6YCow\",\"expires_in\":180,\"refresh_expires_in\":1800,\"refresh_token\":\"eyJhbGciOiJIUzUxMiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJlZjc3MDRjZC03ZGQ2LTQxZjYtOWYxYy00MjRjN2RjNjUzYTcifQ.eyJleHAiOjE3NjM1MDQyODksImlhdCI6MTc2MzUwMjQ4OSwianRpIjoiMjAyN2UxN2QtNzBiMC00NTkxLWE2NTctNDY4ODA3NTBmYzRmIiwiaXNzIjoiaHR0cHM6Ly90ZXN0Mmlmcy5idXJzYWJldG9uLmNvbS50ci9hdXRoL3JlYWxtcy90ZXN0MiIsImF1ZCI6Imh0dHBzOi8vdGVzdDJpZnMuYnVyc2FiZXRvbi5jb20udHIvYXV0aC9yZWFsbXMvdGVzdDIiLCJzdWIiOiI4M2EyNTIyNy0xOGI3LTQ3ZGUtOGUzZS01ODE0NTkyYWY2M2UiLCJ0eXAiOiJSZWZyZXNoIiwiYXpwIjoiQlVSQkVUX01FU19FTlQiLCJzaWQiOiI5YjgzYWM1My02NzAwLTQyNjgtOTgwMS01ZTA0MTg5YTA2MzIiLCJzY29wZSI6ImVtYWlsIHdlYi1vcmlnaW5zIHByb2ZpbGUgbWljcm9wcm9maWxlLWp3dCBhdWRpZW5jZSByb2xlcyJ9.Jd0EsemzJ16MPiKMO2imkkTK6W1VmzNcdKmvEh89aXcHCDIuQ9_rUtrzu6aG3KvbkrFJpefjcMY58UqEpxehKQ\",\"token_type\":\"Bearer\",\"not-before-policy\":0,\"session_state\":\"9b83ac53-6700-4268-9801-5e04189a0632\",\"scope\":\"email profile microprofile-jwt audience\"}";
                var tokenResponse = JsonConvert.DeserializeObject<JObject>(responseContent);

                _accessToken = tokenResponse["access_token"].ToString();
                var expiresIn = tokenResponse["expires_in"].ToObject<int>();

                // Token'ı 50 dakika sonra expire olacak şekilde ayarla (güvenlik için)
                _tokenExpiryTime = DateTime.Now.AddSeconds(expiresIn - 600);

                _logger.LogInfo("OAuth2 token başarıyla alındı");
                _tokenExpiryTime = DateTime.Now.AddMinutes(1);
                return _accessToken;
            }
            catch (Exception ex)
            {
                _logger.LogError("OAuth2 token alınamadı", ex);
                throw;
            }
        }

        /// <summary>
        /// İş emri listesini getir
        /// </summary>
        public async Task<List<IFSPLANLine>> GetShopOrderListAsync(string contract = null, string orderNo = "", string routingAlternative = "*")
        {
            try
            {
                routingAlternative = AppGlobals.appSettings.General.AlternativeRoute;
                contract = contract ?? _settings.Contract;
                var token = await GetAccessTokenAsync();

                var url = $"/int/ifsapplications/projection/v1/BurbetIntService.svc/BurbetShopOrdData(Contract='{contract}',OrderNo='{orderNo}',RoutingAlternative='{routingAlternative}')";

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                if (AppGlobals.saveServiceFile)
                    File.WriteAllText("siparis" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".json", JsonConvert.SerializeObject(content));

                var odataResponse = JsonConvert.DeserializeObject<ODataResponse<IFSPLANLine>>(content);
                var plans = odataResponse?.Value ?? new List<IFSPLANLine>();

                _logger.LogInfo($"İş emri listesi başarıyla alındı: {plans.Count} kayıt");
                return plans;
            }
            catch (Exception ex)
            {
                _logger.LogError("İş emri listesi alınamadı", ex);
                throw;
            }
        }

        /// <summary>
        /// MALZEME listesini getir
        /// </summary>
        public async Task<List<MALZEME>> GetMaterialListAsync(string contract = null, string partNo = "", string userId = "", string dateCreated = null)
        {
            try
            {
                contract = contract ?? _settings.Contract;
                dateCreated = AppGlobals.malzemeTarihi.ToString("dd.MM.yyyy HH:mm ss");//dateCreated ?? DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy HH:mm ss");
                var token = await GetAccessTokenAsync();

                var url = $"/int/ifsapplications/projection/v1/BurbetIntService.svc/ServicePartList(Contract='{contract}',PartNo='{partNo}',UserId='{userId}',DateCreated='{dateCreated}')";

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                // TODO: JSON mapping implementasyonu
                if (AppGlobals.saveServiceFile)
                    File.WriteAllText("malzeme.json", JsonConvert.SerializeObject(content), Encoding.UTF8);
                var odataResponse = JsonConvert.DeserializeObject<ODataResponse<MALZEME>>(content);
                var materials = odataResponse?.Value ?? new List<MALZEME>();

                _logger.LogInfo($"MALZEME listesi başarıyla alındı: {materials.Count} kayıt");
                return materials;
            }
            catch (Exception ex)
            {
                _logger.LogError("MALZEME listesi alınamadı", ex);
                throw;
            }
        }

        /// <summary>
        /// Work order raporu gönder
        /// </summary>
        public async Task<bool> ReportWorkOrderAsync(string systemId, string messageType, string messageText)
        {
            try
            {
                var token = await GetAccessTokenAsync();

                var url = "/int/ifsapplications/projection/v1/BurbetIntService.svc/ReportOrder";

                var requestBody = new ReportOrderRequest()
                {
                    SystemId = systemId,
                    MessageType = messageType,
                    MessageText = messageText
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                _logger.LogInfo("Work order raporu başarıyla gönderildi");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Work order raporu gönderilemedi", ex);
                return false;
            }
        }
    }
}

