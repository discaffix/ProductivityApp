using ProductivityApp.AppTesting.DataAccess;
using ProductivityApp.Model;
using System;
using System.Threading.Tasks;

namespace ProductivityApp.Apptesting.DataAccess
{
    class Sessions : CrudOperations
    {
        static readonly Uri SessionsBaseUri = new Uri("http://localhost:60098/api/sessions");

        //public async void AddSessionAsync(Session session)
        //{
        //    await AddEntryToDatabase(SessionsBaseUri, session);
        //}

        //public async void DeleteSessionAsync(Session session)
        //{
        //    await DeleteDatabaseEntry(SessionsBaseUri, session);
        //}

        //public async Task<Session> GetOneSessionAsync(int id)
        //{
        //    return await GetEntryFromDatabase<Session>(SessionsBaseUri, id);
        //}

        //public async Task<Session[]> GetSessionsAsync()
        //{
        //    return await GetDataFromUri<Session>(SessionsBaseUri);
        //}
    }
}
