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



// Yaml Parser - http://www.codeproject.com/KB/recipes/yamlparser.aspx
// AWS.NetSDK - http://aws.amazon.com/sdkfornet/
//http://sufianrashid.wordpress.com/2011/10/17/c-modify-app-config-file-at-run-time/

namespace WinRightGrid
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(ConfigurationManager.AppSettings["AWSAccessKey"]);
            Console.WriteLine(Queue.ListSQSQueues());
            Console.WriteLine(AddSQSMessage());
            Console.Write(GetServiceOutput());
            Console.Read();
        }

        public static string AddSQSMessage()
        {
            AmazonSQS sqs = AWSClientFactory.CreateAmazonSQSClient();
            SendMessageRequest msgreq = new SendMessageRequest();
            msgreq.QueueUrl = ConfigurationManager.AppSettings["SQSUrl"];
            string yamltest = @"Key1:
                              A: Item A
                              B: Item B
                            Key2:
                              A: Item A
                              B: Item B
                            ";
            Sequence sequence = new Sequence (
                new Node [] {
                    new Yaml.String("rightworker"),
                    new Mapping (
                        new MappingNode []
                        {
                            new MappingNode(new Yaml.String("sqs_url"), new Yaml.String(ConfigurationManager.AppSettings["SQSUrl"].ToString())),
                            new MappingNode(new Yaml.String("s3_bucket"),new Yaml.String("rs-test"))
                        }
                        )
                }
                );
            Console.WriteLine(sequence);
            msgreq.MessageBody = sequence.ToString();
            SendMessageResponse msgres = sqs.SendMessage(msgreq);
            SendMessageResult msgrst = msgres.SendMessageResult;
            return msgrst.ToString();
        }
        public static string GetServiceOutput()
        {
            StringBuilder sb = new StringBuilder(1024);
            using (StringWriter sr = new StringWriter(sb))
            {
                sr.WriteLine("===========================================");
                sr.WriteLine("Welcome to the AWS .NET SDK!");
                sr.WriteLine("===========================================");

                // Print the number of Amazon EC2 instances.
                AmazonEC2 ec2 = AWSClientFactory.CreateAmazonEC2Client();
                DescribeInstancesRequest ec2Request = new DescribeInstancesRequest();

                try
                {
                    DescribeInstancesResponse ec2Response = ec2.DescribeInstances(ec2Request);
                    int numInstances = 0;
                    numInstances = ec2Response.DescribeInstancesResult.Reservation.Count;
                    sr.WriteLine("You have " + numInstances + " Amazon EC2 instance(s) running in the US-East (Northern Virginia) region.");

                }
                catch (AmazonEC2Exception ex)
                {
                    if (ex.ErrorCode != null && ex.ErrorCode.Equals("AuthFailure"))
                    {
                        sr.WriteLine("The account you are using is not signed up for Amazon EC2.");
                        sr.WriteLine("You can sign up for Amazon EC2 at http://aws.amazon.com/ec2");
                    }
                    else
                    {
                        sr.WriteLine("Caught Exception: " + ex.Message);
                        sr.WriteLine("Response Status Code: " + ex.StatusCode);
                        sr.WriteLine("Error Code: " + ex.ErrorCode);
                        sr.WriteLine("Error Type: " + ex.ErrorType);
                        sr.WriteLine("Request ID: " + ex.RequestId);
                        sr.WriteLine("XML: " + ex.XML);
                    }
                }
                sr.WriteLine();

                // Print the number of Amazon SimpleDB domains.
                AmazonSimpleDB sdb = AWSClientFactory.CreateAmazonSimpleDBClient();
                ListDomainsRequest sdbRequest = new ListDomainsRequest();

                try
                {
                    ListDomainsResponse sdbResponse = sdb.ListDomains(sdbRequest);

                    if (sdbResponse.IsSetListDomainsResult())
                    {
                        int numDomains = 0;
                        numDomains = sdbResponse.ListDomainsResult.DomainName.Count;
                        sr.WriteLine("You have " + numDomains + " Amazon SimpleDB domain(s) in the US-East (Northern Virginia) region.");
                    }
                }
                catch (AmazonSimpleDBException ex)
                {
                    if (ex.ErrorCode != null && ex.ErrorCode.Equals("AuthFailure"))
                    {
                        sr.WriteLine("The account you are using is not signed up for Amazon SimpleDB.");
                        sr.WriteLine("You can sign up for Amazon SimpleDB at http://aws.amazon.com/simpledb");
                    }
                    else
                    {
                        sr.WriteLine("Caught Exception: " + ex.Message);
                        sr.WriteLine("Response Status Code: " + ex.StatusCode);
                        sr.WriteLine("Error Code: " + ex.ErrorCode);
                        sr.WriteLine("Error Type: " + ex.ErrorType);
                        sr.WriteLine("Request ID: " + ex.RequestId);
                        sr.WriteLine("XML: " + ex.XML);
                    }
                }
                sr.WriteLine();

                // Print the number of Amazon S3 Buckets.
                AmazonS3 s3Client = AWSClientFactory.CreateAmazonS3Client();

                try
                {
                    ListBucketsResponse response = s3Client.ListBuckets();
                    int numBuckets = 0;
                    if (response.Buckets != null &&
                        response.Buckets.Count > 0)
                    {
                        numBuckets = response.Buckets.Count;
                    }
                    sr.WriteLine("You have " + numBuckets + " Amazon S3 bucket(s) in the US Standard region.");
                }
                catch (AmazonS3Exception ex)
                {
                    if (ex.ErrorCode != null && (ex.ErrorCode.Equals("InvalidAccessKeyId") ||
                        ex.ErrorCode.Equals("InvalidSecurity")))
                    {
                        sr.WriteLine("Please check the provided AWS Credentials.");
                        sr.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                    }
                    else
                    {
                        sr.WriteLine("Caught Exception: " + ex.Message);
                        sr.WriteLine("Response Status Code: " + ex.StatusCode);
                        sr.WriteLine("Error Code: " + ex.ErrorCode);
                        sr.WriteLine("Request ID: " + ex.RequestId);
                        sr.WriteLine("XML: " + ex.XML);
                    }
                }
                sr.WriteLine("Press any key to continue...");
            }
            return sb.ToString();
        }
    }
}