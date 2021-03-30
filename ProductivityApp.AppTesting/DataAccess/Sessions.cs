using ProductivityApp.AppTesting.DataAccess;
using ProductivityApp.Model;
using System;
using System.Threading.Tasks;

namespace ProductivityApp.Apptesting.DataAccess
{
    class Sessions : CrudOperations
    {
        static readonly Uri sessionsBaseUri = new Uri("http://localhost:60098/api/sessions");

        /// <summary>
        /// Gets the sessions table asynchronous.
        /// </summary>
        /// <returns>The table</returns>
        public async Task<Session[]> GetSessionsAsync()
        {
            //var session = new Session() { SessionId = 8 };

            var specificEntry = GetEntryFromDatabase<Session>(sessionsBaseUri, 1);

            //var test = await AddEntryToDatabase(sessionsBaseUri, session);

            //var second = DeleteDatabaseEntry(sessionsBaseUri, session);

            return await GetDataFromUri<Session>(sessionsBaseUri);
        }
    }
}
