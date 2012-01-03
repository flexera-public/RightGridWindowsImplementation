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
            if (ConfigurationManager.AppSettings["QueueTest"] == "1") { QueueTest(); }; 
            Console.Read();
        }
        public static void Run()
        {
            while (Queue.Count() > 1)
            {
                Console.WriteLine("test");
            }
        }
        public static void QueueTest() {

            Console.WriteLine(Queue.ListSQSQueues());
            Console.WriteLine(Queue.Count());
            Console.WriteLine("sending Message");
            Console.WriteLine(Queue.Send("test"));
            Console.WriteLine("Getting Message");
            Message msg = Queue.Get();
            if (msg != null)
            {
                Console.WriteLine(msg.ReceiptHandle.ToString() ?? null);
                Console.WriteLine(msg.Body.ToString() ?? null);
                Console.WriteLine("New Count");
                Console.WriteLine(Queue.Count());
                Console.WriteLine("Delete Message");
                Console.WriteLine(Queue.Delete(msg.ReceiptHandle.ToString()));
            }
            Queue.Count();
        }
    }
}