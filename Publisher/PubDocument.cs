﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Model.Interface;

namespace Model
{
    [Serializable()]
    public class PubDocument : IDocument
    {
        private string title;
        private string hostUri;
        private string pathUri;
        private string summary;
        /// <summary>
        /// published date
        /// </summary>
        private DateTime date;
        private string guid;

        public string Title { get => title; set => title = value; }

        public string HostUri { get => hostUri; set => hostUri = value; }
        public string PathUri { get => pathUri; set => pathUri = value; }
        public string Uri
        {
            get => hostUri + pathUri;
            set
            {
                var uri = new Uri(value);
                HostUri = uri.Scheme + "://" + uri.Host;
                PathUri = uri.PathAndQuery;
            }
        }
        public string Summary { get => summary; set => summary = value; }
        public DateTime Date { get => date; set => date = value; }
        public string GUID { get => guid; set => guid = value; }
        public bool IsRead { get => false; set => throw new InvalidOperationException(); }
    }

    public class DocumentBuilder
    {
        PubDocument ret;
        DocumentBuilder()
        {
            ret = new PubDocument();
        }
        public static DocumentBuilder Doc()
        {
            return new DocumentBuilder();
        }
        public DocumentBuilder Title(string t)
        {
            ret.Title = t;
            return this;
        }
        public DocumentBuilder BaseURL(string t)
        {
            ret.HostUri = t;
            return this;
        }
        public DocumentBuilder URL(string t)
        {
            ret.Uri = t;
            return this;
        }
        public DocumentBuilder Summary(string t)
        {
            ret.Summary = t;
            return this;
        }
        public DocumentBuilder GUID(string t)
        {
            ret.GUID = t;
            return this;
        }
        public DocumentBuilder pubDate(DateTime d)
        {
            ret.Date = d;
            return this;
        }
        /// <summary>
        /// Try to convert <paramref name="dateString"/> to date and set. if failed, set <c>DateTime.Now</c> to it. 
        /// </summary>
        /// <param name="dateString">The string converted to date</param>
        /// <returns></returns>
        public DocumentBuilder pubDate(string dateString)
        {
            DateTime date;
            if (DateTime.TryParse(dateString, out date))
            {
                ret.Date = date;
            }
            else
            {
                ret.Date = DateTime.Now;
            }
            return this;
        }
        public PubDocument Build()
        {
            return ret;
        }
    }
}
