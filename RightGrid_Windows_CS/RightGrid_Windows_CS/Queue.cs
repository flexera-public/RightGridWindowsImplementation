using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace AWS_Console_App1
{
    class Queue
    {
        public static void Send(string msg) { }
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
