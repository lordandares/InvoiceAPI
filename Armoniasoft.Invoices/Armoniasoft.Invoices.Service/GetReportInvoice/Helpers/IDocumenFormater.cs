using System;
using System.Collections.Generic;
using System.Text;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Renderer;
using iText.Layout.Element;
using iText.Pdfa;
using iText.Kernel.Pdf.Canvas.Parser.ClipperLib;
using System.Security.Policy;
using iText.Forms;
using iText.Forms.Fields;
using iText.Layout;
using iText.Layout.Borders;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Geom;
using iText.Kernel.Colors;
using System.Threading.Tasks;
using System.IO;
using iText.Layout.Properties;

namespace Armoniasoft.Invoices.Service.GetReportInvoice.Helpers
{
    public interface IDocumenFormater
    {
        void ReplaceVariablesTemplate(IDictionary<string, string> dictionary, PdfDocument pdf);
        Task<byte[]> GetTemplate();
        void DrawcellItem(string text, float pageSize, bool ratingColor, Table table, TextAlignment textAlignment, bool border);
        void DrawcellHeader(string text, float pageSize, Color backgrund, Table table, Color fontColor, TextAlignment textAlignment, 
            bool border);
        Color RatingColor(string number);
        void ParagraphFormated(string text, Color fontColor, float fontSize, bool center, Document doc, bool bold);
        Table BlankLineDoc(int height);
        Table LineDoc(int height);
    }
}
