using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityApp.AppTesting.DataAccess
{
    public class CrudOperations
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly Uri BaseUri = new Uri("http://localhost:60098/api/");
        /// <summary>
        /// Gets the data from URI.
        /// </summary>
        /// <typeparam name="T">The type of object that is retrieved</typeparam>
        /// <param name="uri">The URI of the API of which you want to retrieve</param>
        /// <returns>Data</returns>
        internal async Task<T[]> GetDataFromUri<T>(Uri uri) where T : class
        {
            var result = await _httpClient.GetAsync(uri);
            var json = await result.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T[]>(json);
            return data;
        }

        /// <summary>
        /// Gets the entry from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">The URI.</param>
        /// <param name="value">The value.</param>
        /// <returns>One entry</returns>
        internal async Task<T> GetEntryFromDatabase<T>(Uri uri, int value) where T : class
        {
            var result = await _httpClient.GetAsync($"{uri}/{value}");
            var json = await result.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(json);
            return data;
        }

        /// <summary>
        /// Adds the entry to database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">The URI.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        internal async Task<bool> AddEntryToDatabase<T>(Uri uri, T item) where T : class
        {
            var json = JsonConvert.SerializeObject(item);
            var result = await _httpClient.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
            return result.IsSuccessStatusCode;
        }

        /// <summary>
        /// Deletes a database entry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">The URI.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        internal async Task<bool> DeleteDatabaseEntry<T>(Uri uri, T item)
        {
            var controller = item.GetType().Name;
            var properties = item.GetType().GetProperties();
            var idValue = properties[0].GetValue(item, null);
            var newPath = $"{controller.ToLower()}s/{idValue}";

            var result = await _httpClient.DeleteAsync(new Uri(uri, newPath));

            return result.IsSuccessStatusCode;
        }
    }
}
