using Model.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Timers;

namespace Model
{
    public class PublishedEventArg : EventArgs {
        private Queue<PubDocument> documents;

        public PublishedEventArg(Queue<PubDocument> documents)
        {
            Documents = documents;
        }

        public Queue<PubDocument> Documents { get => documents; set => documents = value; }
    }
    public delegate void PublishedEventHandler(object sender, PublishedEventArg arg);
}
