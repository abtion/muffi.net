using System;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace WebAppReact.Services
{
    public class Care1Service : ICare1Service
    {
        private readonly string url;
        private readonly string customerOssNumber;
        private readonly string customer;
        private readonly string apiUsername;
        private readonly string apiPassword;


        public Care1Service(IConfiguration configuration)
        {
            url = configuration["Care1:URL"];
            customerOssNumber = configuration["Care1:CustomerOssNumber"];
            customer = configuration["Care1:Customer"];
            apiUsername = configuration["Care1:Username"];
            apiPassword = configuration["Care1:Password"];
        }

        private async Task<Care1ApiTokenRecord> GetToken()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("user", apiUsername);
                httpClient.DefaultRequestHeaders.Add("Password", apiPassword);

                var response = await httpClient.GetAsync(url + "Token/GetToken");
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception("Care1 call GetToken failed");

                return JsonSerializer.Deserialize<Care1ApiTokenRecord>(await response.Content.ReadAsStringAsync());
            }
        }
        public async Task<string> CreateLinkToOss(string supportTicketId, string customerName, string technicianInitials)
        {
            var createLinkRecord = new CreateLinkRecord(supportTicketId, customerName, customerOssNumber, customer, technicianInitials);

            string jsonString = JsonSerializer.Serialize(createLinkRecord);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var token = await GetToken();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", $"{{\"Token\":\"{token.Token}\",\"Initials\":\"{token.Initials}\"}}");
                var response = await httpClient.PostAsync(url + "VCI/VciLink", content);

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception("Care1 call VciLink failed");

                return (await response.Content.ReadAsStringAsync())?.Replace("\"", "");
            }
        }



        public record CreateLinkRecord(string SupportTicketId, string ConsumerName, string CustomerOssNumber, string Customer, string Tek);
        public record Care1ApiTokenRecord(string Token, string Initials, DateTime EndDate);
    }
}