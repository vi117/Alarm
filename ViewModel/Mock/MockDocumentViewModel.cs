﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Interface;
using ViewModel.Interface;

namespace ViewModel
{
    public class MockDocumentViewModel : DocumentViewModel
    {
        public override string Title { get; set; }

        public override string HostUri { get; set; }
        public override string PathUri { get; set; }
        public override string Summary { get; set; }
        public override DateTime Date { get; set; }
        public override string GUID { get; set; }
        public override bool IsRead { get; set; } = false;

        public MockDocumentViewModel() : base()
        { }
        public MockDocumentViewModel(IDocument document) : base()
        {
            HostUri = document.HostUri;
            PathUri = document.PathUri;
            Date = document.Date;
            GUID = document.GUID;
            Summary = document.Summary;
            Title = document.Title;
        }
    }
}
