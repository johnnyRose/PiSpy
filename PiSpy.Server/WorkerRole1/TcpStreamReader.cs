using System.Net.Sockets;
using System.Text;

namespace WorkerRole1
{
    public static class TcpStreamReader
    {
        /// <summary>
        /// Accepts message from client.
        /// </summary>
        /// <param name="networkStream"></param>
        /// <returns></returns>
        public static string Read(NetworkStream networkStream, int length = 256)
        {
            byte[] bytes = new byte[length];
            int i = networkStream.Read(bytes, 0, length);
            return Encoding.ASCII.GetString(bytes, 0, i);

            //var header = new byte[4];
            //var bytesLeft = 4;
            //var offset = 0;

            //// get length of content
            //while (bytesLeft > 0)
            //{
            //    var bytesRead = networkStream.Read(header, offset, bytesLeft);
            //    offset += bytesRead;
            //    bytesLeft -= bytesRead;
            //}

            //bytesLeft = BitConverter.ToInt32(header, 0); // length of content
            //offset = 0;

            //byte[] contentBytes = new byte[bytesLeft];

            //// get the actual content
            //while (bytesLeft > 0)
            //{
            //    var bytesRead = networkStream.Read(contentBytes, offset, bytesLeft);
            //    offset += bytesRead;
            //    bytesLeft -= bytesRead;
            //}

            //return Encoding.ASCII.GetString(contentBytes); // return the content
        }
        //I needed a blocking read for the video because I realized it was causing major issues if the
        //Video was not fully recived into our buffer when this function was called.
        //Read would return without reading the whole file
        //This function will not return until it reads the specified number of bytes
        //Or a timeout with a default of 1 second is reached without reading anything
        public static byte[] ReadBytesBlocking(NetworkStream networkStream, int length = 256, int TimeoutMS = 1000)
        {
            byte[] bytes = new byte[length];
            int numBytesRead = 0;
            int lastBytesRead;
            System.Diagnostics.Stopwatch t = System.Diagnostics.Stopwatch.StartNew();
            
            while (numBytesRead < length)
            {
                int i = networkStream.Read(bytes, numBytesRead, length - numBytesRead);
                lastBytesRead = numBytesRead;
                numBytesRead += i;
                if (lastBytesRead != numBytesRead) // if data was recieved
                {
                    t.Restart();// reset timeout
                }
                else
                {
                    System.Threading.Thread.Sleep(100); // wait for data.
                }
                if(t.ElapsedMilliseconds > TimeoutMS)
                {
                    byte[] Error = new byte[1];
                    Error[0] = 0;
                    return Error;
                }
            }
            return bytes;
        }
    }
}