

using System;

namespace ViewModel
{
    public interface IPageShow
    {
        /// <summary>
        /// For debugging
        /// </summary>
        string ShowingPageName
        {
            get;
        }
        /// <summary>
        /// Show related page
        /// </summary>
        object ShowingPage
        {
            get; set;
        }
    }
    public interface IPageFactory
    {
        object Generate(IPageShow modelExpressed);
    }
}
