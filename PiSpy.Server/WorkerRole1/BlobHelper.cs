using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Diagnostics;

namespace WorkerRole1
{
    public static class BlobHelper
    {
        public static string StoreVideoFile(byte[] h264file, int FileNum, string SerialNum)
        {
            // klayton's blob account
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=klayton;AccountKey=uJMdipdfSA5zmlJNharZUAeihOPNm0lrLUTDsPmHF47ZgH+Y4cSdSuRZFIiY6DyaRfRsW3gr/NJF7jpFKLD96w==");

            // john's blob account
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=pispyblobs;AccountKey=J+6pFHcKzMY6uW2/VWuSb4jRnOh5o6xwuuBZt4haK5SsljxgWKJB0tKeoqAHdSvtUKoCSwBFlvigaDUYnZt5+w==");
            
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // klayton
            //CloudBlobContainer container = blobClient.GetContainerReference("klaytonblob");
            //container.CreateIfNotExists();
            //LocalResource converter_path = RoleEnvironment.GetLocalResource("VideoLocalStorage");

            // john
            CloudBlobContainer container = blobClient.GetContainerReference("videos");
            container.CreateIfNotExists();
            LocalResource converter_path = RoleEnvironment.GetLocalResource("VideoLocalStorage");

            string rootPathName = converter_path.RootPath;
    
            //get the path of ffmpeg.exe, xxx.aac and xxx.mp3 in the local storage:
            string h264_path = rootPathName + "\\" + SerialNum + "video" + FileNum.ToString() + ".h264";
            string mp4_path = rootPathName + "\\" + SerialNum + "video" + FileNum.ToString() + ".mp4";
            string exe_path = rootPathName + "\\" + "ffmpeg.exe";
    
            //write the .h264 file to local storage:
            System.IO.File.WriteAllBytes(h264_path, h264file);

            //keep in mind that the local storage is not guaranteed to be stable or durable, so check the existence of the ffmpeg.exe -- if it doesn't exist, download it from blob.
            if (System.IO.File.Exists(exe_path) == false)
            {
                var exeblob = container.GetBlobReferenceFromServer("ffmpeg.exe");
                exeblob.DownloadToFile(exe_path, System.IO.FileMode.Create);
            }

            //initial and run the ffmpeg.exe process:
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = exe_path;
            psi.Arguments = string.Format(@"-i ""{0}"" -y -framerate 10 ""{1}""", 
                                               h264_path, mp4_path);
            psi.CreateNoWindow = true;
            psi.ErrorDialog = false;
            psi.UseShellExecute = false;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardInput = false;
            psi.RedirectStandardError = true;

            bool hasRun = false;
            int numAttempts = 0;
            while (!hasRun)
            {
                try
                {
                    Process exeProcess = Process.Start(psi);
                    exeProcess.PriorityClass = ProcessPriorityClass.High;
                    string outString = string.Empty;
                    exeProcess.OutputDataReceived += (s, e) =>
                    {
                        outString += e.Data;
                    };
                    exeProcess.BeginOutputReadLine();
                    string errString = exeProcess.StandardError.ReadToEnd();
                    Trace.WriteLine(outString);
                    Trace.TraceError(errString);
                    exeProcess.WaitForExit();
                    hasRun = true;
                }
                catch (System.Exception e)
                {
                    if (numAttempts > 10)
                    {
                        throw new System.Exception("Too Many tries to access ffmpeg.exe for " + SerialNum + "video" + FileNum);
                    }
                    Trace.TraceError(SerialNum + " has collided when attempting to use FFMPEG.exe for video" + FileNum + ". Waiting 100ms to try again.");
                    System.Threading.Thread.Sleep(100);
                    numAttempts++;
                }
            }

            //upload the output of ffmpeg.exe into the blob storage:
            //byte[] mp4_video_byte_array = System.IO.File.ReadAllBytes(mp4_path);
            var mp4blob = container.GetBlockBlobReference(SerialNum + "video" + FileNum.ToString() + ".mp4");
            mp4blob.DeleteIfExists();
            mp4blob.UploadFromFile(mp4_path, System.IO.FileMode.Open);
            mp4blob.Properties.ContentType = "video/mp4";
            mp4blob.SetProperties();

            //clean the temp files:
            System.IO.File.Delete(h264_path);
            System.IO.File.Delete(mp4_path);
            return mp4blob.Uri.ToString();
        }
    }
}
