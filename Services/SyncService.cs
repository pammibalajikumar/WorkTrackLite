using System.Net.Http;
using System.Text;
using System.Text.Json;
using WorkTrackLite.Models;

namespace WorkTrackLite.Services
{
    public class SyncService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        private readonly DatabaseService _databaseService;

        public SyncService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task SyncAsync()
        {
            var unsyncedSessions =
                await _databaseService.GetUnsyncedSessionsAsync();

            foreach (var session in unsyncedSessions)
            {
                try
                {
                    string json = JsonSerializer.Serialize(session);

                    var content = new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json");
                    //dummy API 
                    var response = await _httpClient.PostAsync(
                        "https://jsonplaceholder.typicode.com/posts",
                        content);

                    if (response.IsSuccessStatusCode)
                    {
                        await _databaseService.MarkAsSyncedAsync(session.SerialNo);
                    }
                }
                catch
                {
                    // retry later
                }
            }
        }
    }
}