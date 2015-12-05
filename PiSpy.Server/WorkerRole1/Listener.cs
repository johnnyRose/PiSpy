using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WebRole1.Models;

namespace WorkerRole1
{
    internal static class Listener
    {
        private const int port = 27007;
        private const int vidport = 27008;
        private static bool IsRunning = false; // pretty sure this is unnecessary, but don't have time to test right now
        private static bool IsVidRunning = false;

        public static void WaitForListeners()
        {
            while (IsRunning || IsVidRunning) { Thread.Sleep(10000); }
        }
        public static void Listen()
        {
            new Thread(new ThreadStart(BeginListen)).Start();
        }
        private static void BeginListen()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                TcpListener server = new TcpListener(IPAddress.Any, port);
                server.Start();

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    new Thread(new ParameterizedThreadStart(StartServer)).Start(client);
                }
            }

            IsRunning = false;
        }

        private static void StartServer(object clientParameter)
        {
            TcpClient client = (TcpClient)clientParameter;
            NetworkStream networkStream = client.GetStream();
            string ip = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

            while (client.Connected)
            {
                try
                {
                    try
                    {
                        string message = TcpStreamReader.Read(networkStream);
                        PiSpyDataLog log = new PiSpyDataLog(message, ip);

                        //PolicyEngine.CheckLog(log); // can't put this here, unfortunately. Instead, scheduled checker in other thread on webserver.

                        SqlHelper.AddDataLog(log);
                    }
                    catch { } // in the case that we receive invalid data, do nothing, but stay connected.

                    //var messageBytes = Encoding.ASCII.GetBytes(message);
                    //networkStream.Write(messageBytes, 0, messageBytes.Length);
                }
                catch // if we disconnect, stop looping
                {
                    break;
                }
            }
        }

        public static void ListenVideo()
        {
            new Thread(new ThreadStart(BeginListenVideo)).Start();
        }

        private static void BeginListenVideo()
        {
            if (!IsVidRunning)
            {
                IsVidRunning = true;
                TcpListener server = new TcpListener(IPAddress.Any, vidport);
                server.Start();

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    new Thread(new ParameterizedThreadStart(StartVideoServer)).Start(client);
                }
            }

            IsVidRunning = false;
        }

        private static void StartVideoServer(object clientParameter)
        {
            TcpClient client = (TcpClient)clientParameter;
            NetworkStream networkStream = client.GetStream();
            string ip = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

            while (client.Connected)
            {
                try
                {
                    //Format at time this comment was written:
                    // 4-4-24-<size given as int in first 4 bytes>
                    // videoLen-HeaderLength-Header-Video
                    // Header contains an 8 byte double for the timestamp and 16 byte UTF-8 Pi serial number
                    byte[] videoLenBytes = TcpStreamReader.ReadBytesBlocking(networkStream, 4, 30000);// This one waits 30 seconds for data incase client wants to send more video clips.
                    byte[] headerLenBytes = TcpStreamReader.ReadBytesBlocking(networkStream, 4);

                    if (videoLenBytes.Length < 4 || headerLenBytes.Length < 4)
                    {
                        Trace.TraceError("Message not recieved Quickly enough or the message was not long enough!" +
                            "There was more than 1 second in which data was expected but not recievied in the first 8 bytes!");
                        continue;
                    }

                    int videoLen = System.BitConverter.ToInt32(videoLenBytes, 0);
                    int headerLen = System.BitConverter.ToInt32(headerLenBytes, 0);
                    byte[] messageHeader = TcpStreamReader.ReadBytesBlocking(networkStream, headerLen);
                    byte[] h264File = TcpStreamReader.ReadBytesBlocking(networkStream, videoLen);
                    if (messageHeader.Length < headerLen || h264File.Length < videoLen)
                    {
                        Trace.TraceError("Message not recieved Quickly enough, the message was not long enough, or the first 8 bytes were incorrect!" +
                            "There was more than 1 second in which data was expected but not recievied in the header or the video block.");
                        continue;
                    }

                    //Use the serial number to determine what video sequence number is next for that pi
                    byte[] serialnumbytes = new byte[16];
                    System.Array.Copy(messageHeader, headerLen-16, serialnumbytes, 0, 16);
                    string SerialNum = System.Text.Encoding.UTF8.GetString(serialnumbytes);
                    int filenum = SqlHelper.GetVideoNum(SerialNum);
                    string videoPath = BlobHelper.StoreVideoFile(h264File, filenum, SerialNum); // Encode video in mp4 and get it's blob URI

                    PiSpyVideoLog log = new PiSpyVideoLog(messageHeader, ip, videoPath);

                    SqlHelper.AddVideoLog(log);
                }
                catch (System.Exception e)
                {
                    Trace.TraceError(e.ToString());
                }
            }
        }
    }
}
