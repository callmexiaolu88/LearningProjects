using System.Reflection;
using System;
using Amazon.S3;
using Amazon.Runtime;
using Newtonsoft.Json;
using Amazon.S3.Transfer;

namespace s3Learning
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var propertyInfo = typeof(AmazonS3Client).GetProperty("Credentials", BindingFlags.NonPublic | BindingFlags.Instance);
            var s3Client = new AmazonS3Client();
            var credentials = (propertyInfo.GetValue(s3Client) as AWSCredentials).GetCredentials();
            System.Console.WriteLine($"Credentials: {JsonConvert.SerializeObject(credentials)}");
            System.Console.WriteLine($"Region: {JsonConvert.SerializeObject(s3Client.Config.RegionEndpoint)}");

            var transferUtility = new TransferUtility(s3Client);
            System.Console.WriteLine("upload Test.mp4 to s3test/Test.mp4");
            transferUtility.Upload("/home/operate_sh/s3test/Test.mp4", "pp-va-001","s3test/Test.mp4");
            System.Console.WriteLine("download s3test/Test.mp4 to Test.mp4.download");
            transferUtility.Download("/home/operate_sh/s3test/Test.mp4.download", "pp-va-001","s3test/Test.mp4");
        }
    }
}
