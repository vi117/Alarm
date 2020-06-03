using Model.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Timers;

namespace Model
{
    public enum PublishedStatusCode
    {
        OK,
        ConnectionFailError,
        InvaildArgumentError,
        InvaildFormatError,
        UnknownError
    }
    public class PublishedEventArg : EventArgs {
        private Queue<PubDocument> documents;
        public PublishedStatusCode Code { get; private set; }
        public string DetailErrorMessage { get; private set; }

        public PublishedEventArg(Queue<PubDocument> documents)
        {
            Documents = documents;
            Code = PublishedStatusCode.OK;
        }
        public PublishedEventArg(IEnumerable<PubDocument> documents):this(new Queue<PubDocument>(documents))
        {}
        public PublishedEventArg(PublishedStatusCode code,string message)
        {
            Code = code;
            DetailErrorMessage = message;
            Documents = new Queue<PubDocument>();
        }

        public Queue<PubDocument> Documents { get => documents; set => documents = value; }
    }
    public delegate void PublishedEventHandler(object sender, PublishedEventArg arg);
}
