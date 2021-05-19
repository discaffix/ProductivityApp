using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProductivityApp.App.DataAccess
{
    public class CrudOperations
    {
        public CrudOperations(string baseUri, HttpClient httpClient)
        {
            BaseUri = baseUri;
            HttpClient = httpClient;
        }

        private HttpClient HttpClient { get; }
        public string BaseUri { get; set; }

        /// <summary>
        /// Gets the data from URI.
        /// </summary>
        /// <typeparam name="T">lass from the model</typeparam>
        /// <param name="directTablePath">Name of the table in API.</param>
        /// <returns>A list of objects returned from the GetRequest.</returns>
        internal async Task<T[]> GetDataFromUri<T>(string directTablePath) where T : class
        {
            var uri = new Uri($"{BaseUri}/{directTablePath}");
            var result = await HttpClient.GetAsync(uri);
            var json = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T[]>(json);
        }

        /// <summary>
        /// Gets the entry from database.
        /// </summary>
        /// <typeparam name="T">A class</typeparam>
        /// <param name="directTablePath">Name of table in API.</param>
        /// <param name="id">The ID of the entry.</param>
        /// <returns>An object corresponding to the </returns>
        internal async Task<T> GetEntryFromDatabase<T>(string directTablePath, int id)
        {
            var uri = new Uri(BaseUri + directTablePath);
            var result = await HttpClient.GetAsync($"{uri}/{id}");
            var json = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Adds the entry to database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        internal async Task<bool> AddEntryToDatabase<T>(T item) where T : class
        {
            var typeName = GetTypeName(item);
            var uri = new Uri($"{BaseUri}/{typeName}");
            var json = JsonConvert.SerializeObject(item);
            var result = await HttpClient.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
            return result.IsSuccessStatusCode;
        }

        /// <summary>
        /// Deletes the database entry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <returns>Boolean based on the result of DeleteAsync</returns>
        internal async Task<bool> DeleteDatabaseEntry<T>(T item) where T : class
        {
            var id = GetObjectId(item);
            var typeName = GetTypeName(item);
            var uri = new Uri($"{BaseUri}/{typeName}/{id}");
            var result = await HttpClient.DeleteAsync(uri);
            return result.IsSuccessStatusCode;
        }

        /// <summary>
        /// Updates the database entry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <returns>Boolean based on the result of PutAsync</returns>
        internal async Task<bool> UpdateDatabaseEntry<T>(T item) where T : class
        {
            var id = GetObjectId(item);
            var typeName = GetTypeName(item);
            var uri = new Uri($"{BaseUri}/{typeName}/{id}");
            var json = JsonConvert.SerializeObject(item);
            var result = await HttpClient.PutAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
            return result.IsSuccessStatusCode;
        }

        /// <summary>
        /// Gets the object identifier.
        /// </summary>
        private static int GetObjectId<T>(T item)
        {
            var type = item.GetType();
            var properties = type.GetProperties();
            var idValue = properties[0].GetValue(item, null);
            return (int)idValue;
        }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        private static string GetTypeName<T>(T item)
        {
            return item.GetType().Name + "s";
        }
    }
}
