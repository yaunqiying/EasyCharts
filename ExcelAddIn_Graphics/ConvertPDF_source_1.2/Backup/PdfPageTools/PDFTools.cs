using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PdfPageTools
{
    /// <summary>
    /// Author: TaGoH
    /// This class provide simple tools to edit a pdf file
    /// </summary>
    public static class PDFTools
    {
        /// <summary>Create a copy of a PDF without security</summary>
        /// <param name="source">The original file</param>
        /// <param name="destination">the destination file</param>
        /// <returns>true if succed</returns>
        static public bool StripSecurity(string source, string destination)
        {
            return StripSecurity(source, destination, null,null,null,null);
        }
        /// <summary>Create a copy of a PDF without security</summary>
        /// <param name="source">The original file</param>
        /// <param name="destination">the destination file</param>
        /// <param name="author">the new Author of this document</param>
        /// <param name="title">the new Title of this document</param>
        /// <param name="Application">the new Application that create this document</param>
        /// <returns>true if succed</returns>
        static public bool StripSecurity(string source, string destination,string author,string title,string application)
        {
            return StripSecurity(source, destination, null,author,title,application);
        }
        /// <summary>Create a copy of a PDF without security</summary>
        /// <param name="source">The original file</param>
        /// <param name="destination">the destination file</param>
        /// <param name="pages">the pages i want to copy</param>
        /// <param name="author">the new Author of this document</param>
        /// <param name="title">the new Title of this document</param>
        /// <param name="Application">the new Application that create this document</param>
        /// <returns>true if succed</returns>
        static public bool StripSecurity(string source, string destination, int[] pages, string author, string title, string application)
        {
            //If a file doesn't exist don't even bother to continue
            if (!System.IO.File.Exists(source)) return false;
            //import document using PDFSharp
            PdfDocument maindoc = PdfReader.Open(source,PdfDocumentOpenMode.Import);
            //Create the Output Document as a new PDF Document
            PdfDocument OutputDoc = new PdfDocument();
            if (author != null) OutputDoc.Info.Author = author;
            if (title != null) OutputDoc.Info.Title = title;
            if (application != null) OutputDoc.Info.Creator = application;
            if ((pages == null) || (pages.Length == 0))
            {
                //Copy over all the pages from original document
                foreach (PdfPage page in maindoc.Pages)
                    OutputDoc.AddPage(page);
            }
            else
            {
                for (int i = 0; i < maindoc.PageCount; i++)
                    if (pages.Contains(i))
                        OutputDoc.AddPage(maindoc.Pages[i]);
            }
            //save new document
            OutputDoc.Save(destination);
            //dispose of objects
            maindoc.Dispose();
            OutputDoc.Dispose();
            return true;
        }
    }
}
