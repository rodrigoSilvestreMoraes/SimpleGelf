using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGelf.Extensions.Logging
{
    public static class GelfSimpleHttpClient 
    {
        private static HttpClient _httpClient;
        private static Uri _baseUrl;
        public static void Configure(GelfLoggerConfig gelfLoggerConfig)
        {
            if (gelfLoggerConfig == null)
                return;

            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(gelfLoggerConfig.HttpTimeoutSeconds)
            };

            _baseUrl = new Uri(gelfLoggerConfig.Host + ":" + gelfLoggerConfig.Port + "/gelf");
            _httpClient.DefaultRequestHeaders.ExpectContinue = false;
            if (gelfLoggerConfig.UsingAuthenticate)
            {
                var byteArray = Encoding.ASCII.GetBytes(gelfLoggerConfig.User + ":" + gelfLoggerConfig.Password);
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            }
        }

        public static void Send(GelfMessage gelfMessage)
        {
            if (gelfMessage == null)
                return;

            Task.Run(() =>
            {
                SendMessageAsync(gelfMessage).Wait();
            });
        }
        public static double GetTimestamp()
        {
            var totalMiliseconds = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var totalSeconds = totalMiliseconds / 1000d;
            return Math.Round(totalSeconds, 2);
        }


        private static async Task SendMessageAsync(GelfMessage message)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
                var result = await _httpClient.PostAsync(_baseUrl, content).ConfigureAwait(false);
                result.EnsureSuccessStatusCode();
            }
            catch
            {
                //Pensar em algum outro tipo de log, pois aqui deu erro para enviar para o grayLog
            }            
        }             

    }    
}
