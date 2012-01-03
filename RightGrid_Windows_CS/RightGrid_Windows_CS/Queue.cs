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
        public static string Send(string msg) {
            AmazonSQS sqs = AWSClientFactory.CreateAmazonSQSClient();
            SendMessageRequest msgreq = new SendMessageRequest();
            msgreq.QueueUrl = ConfigurationManager.AppSettings["SQSUrl"];
            msgreq.MessageBody = msg;
            SendMessageResponse msgres = sqs.SendMessage(msgreq);
            SendMessageResult msgrst = msgres.SendMessageResult;
            return msgrst.ToString();
        }
        public static Message Get() { 
            AmazonSQS sqs = AWSClientFactory.CreateAmazonSQSClient();
            ReceiveMessageRequest r_msgreq = new ReceiveMessageRequest();
            r_msgreq.MaxNumberOfMessages = 1;
            r_msgreq.QueueUrl = ConfigurationManager.AppSettings["SQSUrl"];
            Decimal Vis_Timeout = System.Convert.ToDecimal(ConfigurationManager.AppSettings["SQSVisibility"]);
            r_msgreq.VisibilityTimeout = Vis_Timeout;
            ReceiveMessageResponse r_msgres = sqs.ReceiveMessage(r_msgreq);
            //ChangeMessageVisibilityRequest chg_message_vis = new ChangeMessageVisibilityRequest();
            //chg_message_vis.QueueUrl = ConfigurationManager.AppSettings["SQSUrl"];
            //chg_message_vis.ReceiptHandle = r_msgres.ResponseMetadata.RequestId
            ReceiveMessageResult r_msgrst = r_msgres.ReceiveMessageResult;
            Message msg = r_msgrst.Message.FirstOrDefault();
            return msg;
        }
        public static string Delete(string msg_id) {
            AmazonSQS sqs = AWSClientFactory.CreateAmazonSQSClient();
            DeleteMessageRequest d_msgreq = new DeleteMessageRequest();
            d_msgreq.QueueUrl = ConfigurationManager.AppSettings["SQSUrl"];
            d_msgreq.ReceiptHandle = msg_id;
            DeleteMessageResponse d_msgres = sqs.DeleteMessage(d_msgreq);
            return "Deleted Message \n" + d_msgres.ResponseMetadata.ToString();
        }
        public static int Count() { 
            AmazonSQS sqs = AWSClientFactory.CreateAmazonSQSClient();
            GetQueueAttributesRequest gqreq = new GetQueueAttributesRequest();
            gqreq.QueueUrl = ConfigurationManager.AppSettings["SQSUrl"];
            List<string> attr = new List<string>(); 
            attr.Add("All");
            gqreq.AttributeName = attr;
            GetQueueAttributesResponse gqres = sqs.GetQueueAttributes(gqreq);
            GetQueueAttributesResult gqrst = gqres.GetQueueAttributesResult;
            Console.WriteLine("Invisible Messages:" + gqrst.ApproximateNumberOfMessagesNotVisible.ToString());
            Console.WriteLine("Messages:" + gqrst.ApproximateNumberOfMessages);
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

    }
}
