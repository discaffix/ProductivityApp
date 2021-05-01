using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityApp.AppTesting.DataAccess
{
    public class CrudOperations
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string BaseUri = "http://localhost:60098/api/";
        private Uri _uri;

        internal async Task<T[]> GetDataFromUri<T>(string directTablePath, [Optional]string table, [Optional]string value) where T : class
        {
            _uri = new Uri(BaseUri + directTablePath);
            HttpResponseMessage result;

            if (table != null && value != null)
            {
                result = await _httpClient.GetAsync(_uri);
            }
            else
            {
                result = await _httpClient.GetAsync(_uri);
            }

            var json = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T[]>(json); ;
        }

        internal async Task<T> GetEntryFromDatabase<T>(string directTablePath, int value) where T : class
        {
            _uri = new Uri(BaseUri + directTablePath);

            var result = await _httpClient.GetAsync($"{_uri}/{value}");
            var json = await result.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(json);
            return data;
        }

        internal async Task<string> AddEntryToDatabase<T>(string directTablePath, T item) where T : class
        {
            try
            {
                _uri = new Uri(BaseUri + directTablePath);
                var json = JsonConvert.SerializeObject(item);
                var result = await _httpClient.PostAsync(_uri, new StringContent(json, Encoding.UTF8, "application/json"));
                return result.Headers.Location.Segments[3];
                
                //return result.IsSuccessStatusCode;

            } catch (HttpRequestException e)
            {
                Debug.Write(e);
            }

            return "";
        }

        internal async Task<bool> DeleteDatabaseEntry<T>(string directTablePath, T item)
        {
            _uri = new Uri(BaseUri + directTablePath);

            var type = item.GetType();
            var controller = type.Name;
            var properties = type.GetProperties();
            var idValue = properties[0].GetValue(item, null);

            var newPath = $"{controller.ToLower()}s/{idValue}";
            var result = await _httpClient.DeleteAsync(new Uri(_uri, newPath));

            return result.IsSuccessStatusCode;
        }

    }
}
