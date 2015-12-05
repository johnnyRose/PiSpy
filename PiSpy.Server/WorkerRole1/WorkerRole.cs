using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=pispyblobs;AccountKey=J+6pFHcKzMY6uW2/VWuSb4jRnOh5o6xwuuBZt4haK5SsljxgWKJB0tKeoqAHdSvtUKoCSwBFlvigaDUYnZt5+w==");

            //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //CloudBlobContainer container = blobClient.GetContainerReference("videos");
            //container.CreateIfNotExists();
            ////LocalResource converter_path = RoleEnvironment.GetLocalResource("VideoLocalStorage");
            //CloudBlockBlob blockBlob = container.GetBlockBlobReference("ffmpeg.exe");

            //using (var fileStream = System.IO.File.OpenRead(@"C:\Users\John Rosewicz\Desktop\ffmpeg.exe"))
            //{
            //    blockBlob.UploadFromStream(fileStream);
            //}

            //string rootPathName = converter_path.RootPath;

            //string exe_path = rootPathName + "\\" + "ffmpeg.exe";
            //var exeBlob = container.
            //exeBlob.UploadFromFile("ffmpeg.exe", System.IO.FileMode.OpenOrCreate);
            //exeBlob.Properties.ContentType = "application/octet-stream";
            //exeBlob.SetProperties();








            Listener.Listen();
            Listener.ListenVideo();
            Listener.WaitForListeners();

            Trace.TraceInformation("WorkerRole1 is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("WorkerRole1 has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole1 is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("WorkerRole1 has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            //while (!cancellationToken.IsCancellationRequested)
            //{
            //    Trace.TraceInformation("Working");
            //    await Task.Delay(1000);
            //}
        }
    }
}
