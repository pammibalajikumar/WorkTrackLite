using Npgsql;
using WorkTrackLite.Models;
using WorkTrackLite.Models.WorkTrackLite.Models;

namespace WorkTrackLite.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString =
            "Host=localhost;Port=5432;Username=postgres;Password=p@ssw0rd;Database=WorkTrackLiteDB";

        public async Task InsertSessionAsync(WorkSession session)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            await conn.OpenAsync();            

            session.UserName = Environment.UserName;
            session.MachineName = Environment.MachineName;


            string query = @"
                INSERT INTO work_sessions
                (MachineName,UserName,CheckInTime, CheckOutTime, Status, Synced)
                VALUES
                (@machinename,@username,@checkin, @checkout, @status, @synced)";

            using var cmd = new NpgsqlCommand(query, conn);


            cmd.Parameters.AddWithValue("@machinename", session.MachineName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@username", session.UserName ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@checkin", session.CheckInTime ?? (object)DBNull.Value);
            
            cmd.Parameters.AddWithValue("@checkout",
                session.CheckOutTime ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@status", session.Status ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@synced", session.Synced ?? (object)DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<WorkSession>> GetSessionsAsync()
        {
            var sessions = new List<WorkSession>();

            using var conn = new NpgsqlConnection(_connectionString);

            await conn.OpenAsync();

            string query = "SELECT * FROM work_sessions ORDER BY SerialNo";

            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                sessions.Add(new WorkSession
                {
                    SerialNo = Convert.ToInt32(reader["SerialNo"]),
                    MachineName = reader["MachineName"].ToString(),
                    UserName = reader["UserName"].ToString(),

                    CheckInTime = Convert.ToDateTime(reader["CheckInTime"]),
                    CheckOutTime = reader["CheckOutTime"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["CheckOutTime"]),
                    Status = reader["Status"].ToString(),
                    Synced = Convert.ToBoolean(reader["Synced"])
                });
            }

            return sessions;
        }

        public async Task<List<WorkSession>> GetUnsyncedSessionsAsync()
        {
            var sessions = new List<WorkSession>();

            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = "SELECT * FROM work_sessions WHERE Synced=false";

            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                sessions.Add(new WorkSession
                {
                    SerialNo = Convert.ToInt32(reader["SerialNo"]),
                    MachineName = reader["MachineName"].ToString(),
                    UserName = reader["UserName"].ToString(),
                    CheckInTime = Convert.ToDateTime(reader["CheckInTime"]),
                    CheckOutTime = reader["CheckOutTime"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["CheckOutTime"]),
                    Status = reader["Status"].ToString(),
                    Synced = Convert.ToBoolean(reader["Synced"])
                });
            }

            return sessions;
        }

        public async Task MarkAsSyncedAsync(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            await conn.OpenAsync();

            string query = "UPDATE work_sessions SET synced=true WHERE SerialNo=@id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}