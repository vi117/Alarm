using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Interface
{
    public interface IDocument
    {
        string Title { get; set; }
        string HostUri { get; set; }
        string PathUri { get; set; }
        string Summary { get; set; }
        DateTime Date { get; set; }
        string GUID { get; set; }
        bool IsRead { get; set; }
    }
    public static class IDocumentExtension
    {
        public static void SetAll(this IDocument doc,IDocument other)
        {
            doc.Title = other.Title;
            doc.HostUri = other.HostUri;
            doc.PathUri = other.PathUri;
            doc.Summary = other.Summary;
            doc.Date = other.Date;
            doc.GUID = other.GUID;
            doc.IsRead = other.IsRead;
        }
    }
}
