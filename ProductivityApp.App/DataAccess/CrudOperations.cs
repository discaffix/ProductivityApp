using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
        /// Get data from HTTP request
        /// </summary>
        /// <typeparam name="T">Deserialized Object from the request</typeparam>
        /// <param name="directTablePath">Name of the table in API.</param>
        /// <returns>A list of objects returned from the GetRequest.</returns>
        internal async Task<T[]> GetDataFromUri<T>(string directTablePath) where T : class
        {
            T[] returnValue = null;

            try
            {
                var uri = new Uri($"{BaseUri}/{directTablePath}");
                var result = await HttpClient.GetAsync(uri);
                var json = await result.Content.ReadAsStringAsync();
                returnValue = JsonConvert.DeserializeObject<T[]>(json);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e);
            }

            return returnValue;
        }

        /// <summary>
        /// Get an specific entry from the database.
        /// </summary>
        /// <typeparam name="T">Deserialized Object from the request</typeparam>
        /// <param name="directTablePath">Table you want to get</param>
        /// <param name="id">The ID of the entry.</param>
        /// <returns>An object corresponding to the </returns>
        internal async Task<T> GetEntryFromDatabase<T>(string directTablePath, int id)
        {
            var returnValue = default(T);

            try
            {
                var uri = new Uri(BaseUri + directTablePath);
                var result = await HttpClient.GetAsync($"{uri}/{id}");
                var json = await result.Content.ReadAsStringAsync();
                returnValue = JsonConvert.DeserializeObject<T>(json);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e);
            }

            return returnValue;
        }

        /// <summary>
        /// Adds an entry to database.
        /// </summary>
        /// <returns>Whether or not task request was successful</returns>
        internal async Task<bool> AddEntryToDatabase<T>(T item) where T : class
        {
            var returnValue = false;

            try
            {
                var typeName = GetTypeName(item);
                var uri = new Uri($"{BaseUri}/{typeName}");
                var json = JsonConvert.SerializeObject(item);
                var result = await HttpClient.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
                returnValue = result.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e);
            }

            return returnValue;
        }

        /// <summary>
        /// Deletes an entry to the database
        /// </summary>
        /// <returns>Whether or not task request was successful</returns>
        internal async Task<bool> DeleteDatabaseEntry<T>(T item) where T : class
        {
            var returnValue = false;

            try
            {
                var id = GetObjectId(item);
                var typeName = GetTypeName(item);
                var uri = new Uri($"{BaseUri}/{typeName}/{id}");
                var result = await HttpClient.DeleteAsync(uri);
                returnValue = result.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e);
            }

            return returnValue;
        }

        /// <summary>
        /// Updates an entry in the database
        /// </summary>
        /// <param name="item">The values you want to update the old item with</param>
        /// <returns>Whether or not task request was successful</returns>
        internal async Task<bool> UpdateDatabaseEntry<T>(T item) where T : class
        {
            var returnValue = false;

            try
            {
                var id = GetObjectId(item);
                var typeName = GetTypeName(item);
                var uri = new Uri($"{BaseUri}/{typeName}/{id}");
                var json = JsonConvert.SerializeObject(item);
                var result = await HttpClient.PutAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
                returnValue = result.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e);
            }

            return returnValue;
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
