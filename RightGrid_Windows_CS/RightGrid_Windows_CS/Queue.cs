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
        public static string Get() { return "msg"; }
        public static int Count() { return 1; }
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
