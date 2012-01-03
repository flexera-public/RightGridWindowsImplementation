using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using Amazon;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;


namespace WinRightGrid
{
    class Storage
    {
        public static void Put(string fileName) {
            Console.WriteLine(fileName);
        }
        public static void Get(string bucket, string path, string fileName) { }
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
            Storage.Put("test");
        }
    }
}
 