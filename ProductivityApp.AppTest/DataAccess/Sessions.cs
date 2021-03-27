using Newtonsoft.Json;
using ProductivityApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityApp.AppTest.DataAccess
{
    class Sessions
    {
        readonly HttpClient _httpClient = new HttpClient();
        static readonly Uri sessionsBaseUri = new Uri("http://localhost:60098/api/sessions");

        public async Task<Session[]> GetSessionsAsync()
        {
            HttpResponseMessage result = await _httpClient.GetAsync(sessionsBaseUri);
            string json = await result.Content.ReadAsStringAsync();
            Session[] sessions = JsonConvert.DeserializeObject<Session[]>(json);

            return sessions;
        }

    }
}
