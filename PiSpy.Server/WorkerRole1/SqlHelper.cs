using System.Data;
using System.Data.SqlClient;
using WebRole1.Models;

namespace WorkerRole1
{
    public static class SqlHelper
    {
        private static string connectionString = "Server=tcp:yfswwuwu7f.database.windows.net,1433;Database=pispydb;User ID=pispy@yfswwuwu7f;Password=temp@123;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
        //private static string connectionString = "Server=tcp:pjukket8nr.database.windows.net,1433;Database=skyspidb;User ID=misterjeff@pjukket8nr;Password=j1mmyj@ck@ss;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
        //private static string connectionString = "Server=tcp:rnjlhjaseg.database.windows.net,1433;Database=Database;User ID=Klayton@rnjlhjaseg;Password=Liv3Lif3;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

        public static void AddDataLog(PiSpyDataLog log)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO PiSpyDataLogs(TimeStamp, IpAddress, Temperature, Humidity, PiSpySerialNumber, TimeReceived) VALUES (@TimeStamp, @IpAddress, @Temperature, @Humidity, @PiSpySerialNumber, @TimeReceived);", cn))
                {
                    cmd.Parameters.Add("@TimeStamp", SqlDbType.DateTime).Value = log.TimeStamp;
                    cmd.Parameters.Add("@IpAddress", SqlDbType.NVarChar).Value = log.IpAddress;
                    cmd.Parameters.Add("@Temperature", SqlDbType.Float).Value = log.Temperature;
                    cmd.Parameters.Add("@Humidity", SqlDbType.Float).Value = log.Humidity;
                    cmd.Parameters.Add("@PiSpySerialNumber", SqlDbType.NVarChar).Value = log.PiSpySerialNumber;
                    cmd.Parameters.Add("@TimeReceived", SqlDbType.DateTime).Value = log.TimeReceived;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void AddVideoLog(PiSpyVideoLog log)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO PiSpyVideoLogs(TimeStamp, IpAddress, FilePath, PiSpySerialNumber, TimeReceived) VALUES (@TimeStamp, @IpAddress, @FilePath, @PiSpySerialNumber, @TimeReceived);", cn))
                {
                    cmd.Parameters.Add("@TimeStamp", SqlDbType.DateTime).Value = log.TimeStamp;
                    cmd.Parameters.Add("@IpAddress", SqlDbType.NVarChar).Value = log.IpAddress;
                    cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar).Value = log.FilePath;
                    cmd.Parameters.Add("@PiSpySerialNumber", SqlDbType.NVarChar).Value = log.PiSpySerialNumber;
                    cmd.Parameters.Add("@TimeReceived", SqlDbType.DateTime).Value = log.TimeReceived;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static int GetVideoNum(string serialNum)//Returns what the next video number should be for a given Pi
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(ID) FROM PiSpyVideoLogs WHERE PiSpySerialNumber = @PSSN", cn))
                {
                    cmd.Parameters.Add("@PSSN", SqlDbType.NVarChar).Value = serialNum;
                    cn.Open();
                    SqlDataReader r = cmd.ExecuteReader();
                    if (r.Read())
                    {
                        int val = 0;
                        int.TryParse(r[0].ToString(), out val);
                        return val;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }
}
