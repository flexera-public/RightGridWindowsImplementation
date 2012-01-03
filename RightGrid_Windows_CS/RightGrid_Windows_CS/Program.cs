using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Yaml;

namespace WinRightGrid
{
  class Program
    {
      public static void Main(string[] args)
        {
          if (ConfigurationManager.AppSettings["QueueTest"] == "1") { Queue.Test(); };
          if (ConfigurationManager.AppSettings["StorageTest"] == "1") { Storage.Test(); };
          Console.Read();
        }
        public static void Run()
        {
            while (Queue.Count() > 1)
            {
                Console.WriteLine("test");
            }
        }
        
    }
}