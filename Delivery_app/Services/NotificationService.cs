using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Delivery_app.Services
{
    public interface INotificationService
    {
        Task SendNotification(string fcmToken, string collapseKey, string title, string body);
    }

    public class NotificationService : INotificationService
    {
        private static readonly HttpClient client = new HttpClient();

        public NotificationService(IConfiguration configuration)
        {
            string fcmServerKey = configuration.GetValue<String>("fcmServerKey");
            //set authorization header
            var added = client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"key={fcmServerKey}");
            Console.WriteLine("Added :{0} ", added.ToString());
        }

        public async Task SendNotification(string fcmToken, string collapseKey, string title, string body)
        {
            try
            {
                // initialize body object
                var content = new
                {
                    to = fcmToken,
                    collapse_key = collapseKey,
                    priority = "high",
                    notification = new
                    {
                        title = title,
                        body = body
                    },
                    data = new
                    {
                        click_action = "FLUTTER_NOTIFICATION_CLICK",
                        id = 1,
                        status = "done"
                    }
                };

                // convert to json
                var jsonContent = JsonConvert.SerializeObject(content);

                var requestContent = new StringContent(jsonContent);

                // set content type
                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync(
                    "https://fcm.googleapis.com/fcm/send",
                    requestContent
                    );

                response.EnsureSuccessStatusCode();

                // read response body
                string responseBody = await response.Content.ReadAsStringAsync();

                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                throw e;
            }
        }
    }
}
