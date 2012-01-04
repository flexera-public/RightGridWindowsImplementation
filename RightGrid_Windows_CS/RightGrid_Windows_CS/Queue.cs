using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

//Amazon Libraries
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace WinRightGrid
{
   class Queue
    {
        public static string Send(string queue_url, string msg) {
            AmazonSQS sqs = AWSClientFactory.CreateAmazonSQSClient();
            SendMessageRequest msgreq = new SendMessageRequest();
            msgreq.QueueUrl = queue_url;
            msgreq.MessageBody = msg;
            SendMessageResponse msgres = sqs.SendMessage(msgreq);
            SendMessageResult msgrst = msgres.SendMessageResult;
            return msgrst.ToString();
        }
        public static Message Get(string queue_url) { 
            AmazonSQS sqs = AWSClientFactory.CreateAmazonSQSClient();
            ReceiveMessageRequest r_msgreq = new ReceiveMessageRequest();
            r_msgreq.MaxNumberOfMessages = 1;
            r_msgreq.QueueUrl = queue_url;
            Decimal Vis_Timeout = System.Convert.ToDecimal(ConfigurationManager.AppSettings["SQS_Visibility"]);
            r_msgreq.VisibilityTimeout = Vis_Timeout;
            ReceiveMessageResponse r_msgres = sqs.ReceiveMessage(r_msgreq);
            //ChangeMessageVisibilityRequest chg_message_vis = new ChangeMessageVisibilityRequest();
            //chg_message_vis.QueueUrl = ConfigurationManager.AppSettings["SQSUrl"];
            //chg_message_vis.ReceiptHandle = r_msgres.ResponseMetadata.RequestId
            ReceiveMessageResult r_msgrst = r_msgres.ReceiveMessageResult;
            Message msg = r_msgrst.Message.FirstOrDefault();
            return msg;
        }
        public static string Delete(string queue_url, string msg_id) {
            AmazonSQS sqs = AWSClientFactory.CreateAmazonSQSClient();
            DeleteMessageRequest d_msgreq = new DeleteMessageRequest();
            d_msgreq.QueueUrl = queue_url;
            d_msgreq.ReceiptHandle = msg_id;
            DeleteMessageResponse d_msgres = sqs.DeleteMessage(d_msgreq);
            return "Deleted Message \n" + d_msgres.ResponseMetadata.ToString();
        }
        public static int Count(string queue_url) { 
            AmazonSQS sqs = AWSClientFactory.CreateAmazonSQSClient();
            GetQueueAttributesRequest gqreq = new GetQueueAttributesRequest();
            gqreq.QueueUrl = queue_url;
            List<string> attr = new List<string>(); 
            attr.Add("All");
            gqreq.AttributeName = attr;
            GetQueueAttributesResponse gqres = sqs.GetQueueAttributes(gqreq);
            GetQueueAttributesResult gqrst = gqres.GetQueueAttributesResult;
            //Console.WriteLine("Invisible Messages:" + gqrst.ApproximateNumberOfMessagesNotVisible.ToString());
            //Console.WriteLine("Messages:" + gqrst.ApproximateNumberOfMessages);
            return gqrst.ApproximateNumberOfMessages;
        }
        public static string ListSQSQueues()
        {
            AmazonSQS sqs = AWSClientFactory.CreateAmazonSQSClient();
            ListQueuesRequest sqsrequest = new ListQueuesRequest();
            ListQueuesResponse sqsresponse = sqs.ListQueues(sqsrequest);
            ListQueuesResult sqsrst = sqsresponse.ListQueuesResult;
            return sqsrst.ToString();
        }
        public static void Test()
        {

            Console.WriteLine(Queue.ListSQSQueues());
            //input_test
            List<string> queues = new List<string>();
            queues.Add(ConfigurationManager.AppSettings["input_queue_url"]);
            queues.Add(ConfigurationManager.AppSettings["results_queue_url"]);
            queues.ForEach(delegate(string url) {
                Console.WriteLine(Queue.Count(url));
                Console.WriteLine("sending Message");
                Console.WriteLine(Queue.Send(url,"test"));
                Console.WriteLine("Getting Message");
                Message msg = Queue.Get(url);
                if (msg != null)
                {
                    Console.WriteLine(msg.ReceiptHandle.ToString() ?? null);
                    Console.WriteLine(msg.Body.ToString() ?? null);
                    Console.WriteLine("New Count");
                    Console.WriteLine(Queue.Count(url));
                    Console.WriteLine("Delete Message");
                    Console.WriteLine(Queue.Delete(url,msg.ReceiptHandle.ToString()));
                }
                Queue.Count(url);
            });
        }
    }
}
