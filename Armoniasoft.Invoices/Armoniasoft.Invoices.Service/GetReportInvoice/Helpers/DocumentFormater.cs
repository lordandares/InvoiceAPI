using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Forms;
using iText.Forms.Fields;
using iText.Layout;
using iText.Layout.Borders;
using iText.Kernel.Colors;
using System.Net.Http;
using iText.Layout.Properties;

namespace Armoniasoft.Invoices.Service.GetReportInvoice.Helpers
{
    public class DocumentFormater : IDocumenFormater
    {
        private Color blackColor = new DeviceRgb(0, 0, 0);
        private Color whiteColor = new DeviceRgb(255, 255, 255);
        private Color titleColer = new DeviceRgb(8, 146, 209);
        private Color subTitleColorBackground = new DeviceRgb(237, 239, 240);
        private string template;

     
        /*
        * Description: Replace fields with specific text on template
        * dictionary: dictionary of keys and values to replace
        * pdf: pdfDocument to replace text
        */
        public void ReplaceVariablesTemplate(IDictionary<string, string> dictionary, PdfDocument pdf)
        {
            float noBorder = 0;
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, true);
            IDictionary<string, PdfFormField> fields = form.GetFormFields();
            foreach (KeyValuePair<string, string> entry in dictionary)
            {
                PdfFormField toSet;
                fields.TryGetValue(entry.Key, out toSet);
                toSet.SetValue(entry.Value);
                toSet.SetBorderWidth(noBorder);
            }
            form.FlattenFields();
        }

        /*
       * Description: Method to draw spaces on Pdf
       * Params: Height int   //number of spaces to draw
       */
        public Table BlankLineDoc(int height)
        {
            Table tableSpace = new Table(1, true);
            tableSpace.SetBorder(Border.NO_BORDER);
            for (int i = 0; i < height; i++)
            {
                Paragraph space = new Paragraph(" ");
                Cell column1 = new Cell().Add(space);
                column1.SetBorder(Border.NO_BORDER);
                tableSpace.AddCell(column1);
            }
            return tableSpace;
        }

        public Table LineDoc(int height)
        {
            Table tableSpace = new Table(1, true);
            tableSpace.SetBorder(Border.NO_BORDER);
            for (int i = 0; i < height; i++)
            {
                Paragraph space = new Paragraph("-");
                Cell column1 = new Cell().Add(space);
                column1.SetBorder(Border.NO_BORDER);
                tableSpace.AddCell(column1);
            }
            return tableSpace;
        }

        /*
        * Description: Method to draw Paraghaph with format
        * Params: 
        *   Text: text to draw on paragrph
        *   fontColor: font Color
        *   fontSize: font Size
        *   doc: documento to be drawn
        */
        public void ParagraphFormated(string text, Color fontColor, float fontSize, bool center, Document doc, bool bold)
        {
            Paragraph paragraphFormated = new Paragraph(text);
            paragraphFormated.SetFontColor(fontColor);
            paragraphFormated.SetFontSize(fontSize);
            paragraphFormated.SetFixedLeading(10);
            if (center)
                paragraphFormated.SetTextAlignment(TextAlignment.CENTER);
            if (bold)
                paragraphFormated.SetBold();

            doc.Add(paragraphFormated);
        }



        /*
        * Description: Method to draw ItemCell
        * Params: 
        *   text: text to draw on paragrph
        *   backgrund: Background Color of the cell
        *   table: table to draw cell
        *   fontColor: Font Color
        */
        public void DrawcellHeader(string text, float pageSize, Color backgrund, Table table, Color fontColor, 
            TextAlignment textAlignment, bool border)
        {
            Paragraph paragraph = new Paragraph(text);
            paragraph.SetFontSize(pageSize);
            Cell cellHeader = new Cell().Add(paragraph);
            cellHeader.SetFontColor(fontColor);
            cellHeader.SetBackgroundColor(backgrund);
            cellHeader.SetTextAlignment(textAlignment);
            if (!border)
                cellHeader.SetBorder(Border.NO_BORDER);
            
            table.AddCell(cellHeader);
        }

        /*
        * Description: Method to draw ItemCell
        * Params: 
        *   text: text to draw on paragrph
        *   ratingColor: bool to draw rating Color
        *   table: table to draw cell
        */
        public void DrawcellItem(string text, float pageSize, bool ratingColor, Table table, TextAlignment textAlignment, bool border)
        {
            Paragraph paragraph = new Paragraph(text);
            paragraph.SetFontSize(pageSize);
            Cell columnItem = new Cell().Add(paragraph);
            columnItem.SetTextAlignment(textAlignment);
            if (ratingColor)
                columnItem.SetBackgroundColor(RatingColor(text));
            if (!border)
                columnItem.SetBorder(Border.NO_BORDER);

            table.AddCell(columnItem);
        }


        /*
        * Description: Get the template to Draw
        * Params: 
        *   url: text to draw on paragrph
        */
        public async Task<byte[]> GetTemplate()
        {
            HttpClient client = new HttpClient();
            byte[] templateStream;
            using (Stream file = await client.GetStreamAsync(template).ConfigureAwait(false))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                templateStream = memoryStream.ToArray();
            }
            //return new Task<byte[]>(() => templateStream);
            return templateStream;
        }

        /*
        * Description: Method to Return corresponding color to Rating
        * Params: 
        *   number: string of selected rating (1,2,3,4,5)
        */
        public Color RatingColor(string number)
        {
            Color resultColor;
            switch (number)
            {
                case "1":
                    resultColor = new DeviceRgb(236, 249, 237);
                    break;
                case "2":
                    resultColor = new DeviceRgb(241, 250, 254);
                    break;
                case "3":
                    resultColor = new DeviceRgb(254, 249, 227);
                    break;
                case "4":
                    resultColor = new DeviceRgb(254, 250, 245);
                    break;
                case "5":
                    resultColor = new DeviceRgb(249, 236, 235);
                    break;
                default:
                    resultColor = new DeviceRgb(255, 255, 255);
                    break;
            }
            return resultColor;
        }


    }
}

