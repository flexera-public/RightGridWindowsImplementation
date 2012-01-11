using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Management;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using YamlDotNet;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Converters;

namespace WinRightGrid
{
  class Program
    {
      public static void Main(string[] args)
        {
          if (ConfigurationManager.AppSettings["QueueTest"] == "1") { Queue.Test(); };
          if (ConfigurationManager.AppSettings["StorageTest"] == "1") { Storage.Test(); };
          Run();
          Console.Read();
        }
        public static void Run()
        {
            Console.WriteLine("Starting");
            while (Queue.Count(ConfigurationManager.AppSettings["input_queue_url"]) >= 1)
            {
                Console.WriteLine("pass");
                Message msg = Queue.Get(ConfigurationManager.AppSettings["input_queue_url"]);
                Console.WriteLine("got message");
                Console.WriteLine(msg.Body);
                //Node node = Node.Parse(msg.Body);
                Console.WriteLine("parsed message");
                Stream input = new MemoryStream(Encoding.UTF8.GetBytes(msg.Body));
                TextReader yamltxt = new StreamReader(input);
                YamlStream yaml = new YamlStream();
                yaml.Load(yamltxt);
                YamlMappingNode mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
                foreach (KeyValuePair<YamlNode, YamlNode> entry in mapping.Children)
                {
                    Console.WriteLine(((YamlScalarNode)entry.Key).Value);
                }
                 
                //Storage.Get(ConfigurationManager.AppSettings["S3_Bucket"], ConfigurationManager.AppSettings["S3_Input_Path"], node["input_file"]);
                //Runner.run(node["input_file"],node["output_file"]));
                //Storage.Put(ConfigurationManager.AppSettings["S3_Bucket"], ConfigurationManager.AppSettings["S3_Output_Path"], node["output_file"]);
                Console.WriteLine("node");
                Queue.Delete(ConfigurationManager.AppSettings["input_queue_url"], msg.ReceiptHandle.ToString());
            }
        }

        
    }
}