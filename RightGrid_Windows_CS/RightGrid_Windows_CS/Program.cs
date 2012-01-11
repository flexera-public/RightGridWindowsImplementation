using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Management;
using System.Diagnostics;

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
      static TimeSpan queue_wait_time = new TimeSpan(0, 0, Convert.ToInt32(ConfigurationManager.AppSettings["Queue_Check_Frequency_Seconds"]));
      public static void Main(string[] args)
        {
          if (ConfigurationManager.AppSettings["QueueTest"] == "1") { Queue.Test(); };
          if (ConfigurationManager.AppSettings["StorageTest"] == "1") { Storage.Test(); };
          DateTime rg_start_time = DateTime.Now;
          Stopwatch stopWatch = new Stopwatch();
          stopWatch.Start();
          while (stopWatch.Elapsed.Minutes < Convert.ToInt32(ConfigurationManager.AppSettings["RightGrid_Timeout"]))
          {
              Run();
              Console.WriteLine("Sleeping " + ConfigurationManager.AppSettings["Queue_Check_Frequency_Seconds"] + " seconds");
              System.Threading.Thread.Sleep(queue_wait_time);
              
          }
          Console.WriteLine(stopWatch.Elapsed);
        #if DEBUG
          Console.Read();
        #endif
        }
        public static void Run()
        {
            Console.WriteLine("Starting");
            string path = Directory.GetCurrentDirectory();
            string input_dir = Path.Combine(path,"input");
            string output_dir = Path.Combine(path,"output");
            if (!Directory.Exists(input_dir)) 
            {
                Console.WriteLine("Creating Input Directory");
                Directory.CreateDirectory(input_dir); 
            }
            if (!Directory.Exists(output_dir)) 
            {
                Console.WriteLine("Creating Output Directory unless exists");
                Directory.CreateDirectory(output_dir); 
            }
            while (Queue.Count(ConfigurationManager.AppSettings["input_queue_url"]) >= 1)
            {
                Message msg = Queue.Get(ConfigurationManager.AppSettings["input_queue_url"]);
                Console.WriteLine("got message");
                if (msg != null)  {
                    Console.WriteLine(msg.Body);
                    //Node node = Node.Parse(msg.Body);
                    Console.WriteLine("parsed message");
                    Stream input = new MemoryStream(Encoding.UTF8.GetBytes(msg.Body));
                    TextReader yamltxt = new StreamReader(input);
                    YamlStream yaml = new YamlStream();
                    yaml.Load(yamltxt);
                    YamlMappingNode mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
                    string input_file = "";
                    string output_file = "";
                    string created_at = "0000-00-00 00:00:00 GMT";
                    int job_id = '0';

                    foreach (KeyValuePair<YamlNode, YamlNode> entry in mapping.Children)
                    {
                        if (entry.Key.ToString() == ":input_file") { input_file = entry.Value.ToString(); }
                        if (entry.Key.ToString() == ":output_file") { output_file = entry.Value.ToString(); }
                        if (entry.Key.ToString() == ":created_at") {  created_at = entry.Value.ToString(); }
                        if (entry.Key.ToString() == ":id") { job_id = Convert.ToInt32(entry.Value.ToString()); }
                        Console.WriteLine(("Key:" + (YamlScalarNode)entry.Key + " Value:" + (YamlScalarNode)entry.Value));
                    }
                    Console.WriteLine("input_file is set to: " + input_file);
                    Console.WriteLine("output_file is set to: " + output_file);
                    Console.WriteLine("created_at is set to: " + created_at);
                    Console.WriteLine("job_id is set to: " + job_id);
                    Storage.Get(ConfigurationManager.AppSettings["S3_Bucket"], ConfigurationManager.AppSettings["S3_Input_Path"]+input_file, Path.Combine(input_dir,input_file));
                    Runner.run(Path.Combine(input_dir, input_file),Path.Combine(output_dir,output_file));
                    Storage.Put(ConfigurationManager.AppSettings["S3_Bucket"], ConfigurationManager.AppSettings["S3_Output_Path"], Path.Combine(output_dir,output_file));
                    Queue.Delete(ConfigurationManager.AppSettings["input_queue_url"], msg.ReceiptHandle.ToString());
                }
                
            }
            Console.WriteLine("Processing Completed");
        }
        public static void gen_finished_message(int job_id, string output_file, string created_at) { }
        public static void shutdown_system() { }
        
    }
}