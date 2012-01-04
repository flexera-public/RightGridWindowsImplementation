using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;


namespace WinRightGrid
{
    class Storage
    {
        public static void Put(string bucket,string key,string fileName) {
            AmazonS3 s3Client = AWSClientFactory.CreateAmazonS3Client();
            FileInfo file = new FileInfo(fileName);
            Console.Write("Uploading " + file.Name + " to " + bucket + ":" + key + file.Name);
            PutObjectRequest po_req = new PutObjectRequest();
            po_req.BucketName = bucket;
            po_req.Key = key + file.Name;
            po_req.FilePath = fileName;
            po_req.AutoCloseStream = true;
            PutObjectResponse po_res = s3Client.PutObject(po_req);
            Console.WriteLine(po_res.AmazonId2);
        }
        public static void Get(string bucket, string key, string fileName) 
        {
            AmazonS3 s3Client = AWSClientFactory.CreateAmazonS3Client();
            FileInfo file = new FileInfo(key);
            Console.WriteLine("Download File " + bucket + ":" + key + " to " + fileName);
            GetObjectRequest get_req = new GetObjectRequest();
            get_req.BucketName = bucket;
            get_req.Key = key;
            GetObjectResponse get_res = s3Client.GetObject(get_req);
            get_res.WriteResponseStreamToFile(fileName);
            Console.WriteLine(get_res.Metadata.AllKeys.FirstOrDefault());
        }
        public static void Test() {
            Console.WriteLine("Testing Storage");
            Console.WriteLine("Creating Temp File");
            Random random = new Random();
            int counter = 0;
            StringBuilder sb = new StringBuilder(2048);
            StringWriter sr = new StringWriter(sb);
            while (counter <= 262144)
            {
                sr.Write(random.Next(65535).ToString("X"));
                counter++;
            }
            sr.Close();
            FileStream f = File.Open(ConfigurationManager.AppSettings["StrorageSampleFile"], FileMode.Create);
            using (StreamWriter sw = new StreamWriter(f))
            {
                sw.Write(sb.ToString());
            }
            Storage.Put(ConfigurationManager.AppSettings["S3_Bucket"],"output/",ConfigurationManager.AppSettings["StrorageSampleFile"]);
            FileInfo o_samp_file = new FileInfo(ConfigurationManager.AppSettings["StrorageSampleFile"]);
            Storage.Get(ConfigurationManager.AppSettings["S3_Bucket"], "output/" + o_samp_file.Name, ConfigurationManager.AppSettings["StrorageSampleFile"]+"_output");
        }
    }
}
 