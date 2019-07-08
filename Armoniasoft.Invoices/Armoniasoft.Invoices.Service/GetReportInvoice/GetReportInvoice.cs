using Armoniasoft.Invoices.Mapping.Models;
using Armoniasoft.Invoices.Service.GetReportInvoice.Helpers;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;
using iText.Layout.Borders;
using iText.Kernel.Colors;
using iText.Barcodes;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Xobject;
using iText.Layout.Properties;

namespace Armoniasoft.Invoices.Service.GetReportInvoice
{
    public class GetReportInvoice<T> : IGetReportInvoice<T> where T : Invoice
    {
        private Color blackColor = new DeviceRgb(0, 0, 0);
        private Color whiteColor = new DeviceRgb(255, 255, 255);
        private Color titleColer = new DeviceRgb(8, 146, 209);
        private Color subTitleColorBackground = new DeviceRgb(237, 239, 240);
        private int HeaderSpace = 60;
        private readonly IDocumenFormater documentItextFormater;

        public GetReportInvoice(IDocumenFormater documentItextFormater)
        {
            this.documentItextFormater = documentItextFormater;
        }

        public byte[] getReport(T invoice)
        {
            MemoryStream memoryStream = new MemoryStream();
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(memoryStream));
            PageSize pageSize = new PageSize(300f, 600f);
            Document document = new Document(pdfDocument, pageSize);
            float fontSize = 8f;

            documentItextFormater.ParagraphFormated(
                $"ACME SRL.", blackColor, fontSize, true, document, false);
            documentItextFormater.ParagraphFormated(
                $"NIT: 459834534", blackColor, fontSize, true, document, false);
            documentItextFormater.ParagraphFormated(
                $"Documento Fiscal Nro.: 4568", blackColor, fontSize, true, document, false);
            documentItextFormater.ParagraphFormated(
                $"Factura No: 134", blackColor, fontSize, true, document, false);
            documentItextFormater.ParagraphFormated(
                $"Autorización: 1234216422134", blackColor, fontSize, true, document, false);
            documentItextFormater.ParagraphFormated(
                $"Fecha: {invoice.Header.FechaEmision}", blackColor, fontSize, true, document, false);
            documentItextFormater.ParagraphFormated(
                $"NIT/CI: {invoice.Header.NumeroDocumento}", blackColor, fontSize, true, document, false);
            documentItextFormater.ParagraphFormated(
                $"Nombre/Razon Social: {invoice.Header.NombreRazonSocial}", blackColor, fontSize, true, document, false);
            documentItextFormater.ParagraphFormated(
                $"Codigo Cliente: 1254", blackColor, fontSize, true, document, false);
            
            DrawTableAreasPdf(document, invoice.Detail);
            
            BarcodeQRCode barcodeQrCode = new BarcodeQRCode(
                $"{invoice.Header.NitEmisor}|{invoice.Header.NitEmisor}|{invoice.Header.NitEmisor}|" +
                $"{invoice.Header.FechaEmision}|{invoice.Header.MontoTotal}|{invoice.Header.MontoDescuento}|");
            PdfFormXObject objectQR = barcodeQrCode.CreateFormXObject(blackColor, 4F, pdfDocument);

            Table table = new Table(5, false);
            table.SetBorder(Border.NO_BORDER);
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER));
            Cell imageCell = new Cell().Add(new Image(objectQR));
            imageCell.SetBorder(Border.NO_BORDER);
            table.AddCell(imageCell);
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER));
            document.Add(table);

//            document.Add(new Image(objectQR));

            documentItextFormater.ParagraphFormated(
                "PRUEBA DE IMPRESION NO VALIDA PARA CREDITO FISCAL", blackColor, fontSize, true, document, true);
            
            document.Close();

            return memoryStream.ToArray();            
        }


        /*
        * Description: Draw Table Result of Areas in PDF
        * ResultDoc: Documento to Draw   //number of spaces to draw
        * Areas: Model Areas   //number of spaces to draw
        */
        private void DrawTableAreasPdf(Document ResultDoc, IEnumerable<IDetail> Details)
        {
            //Set header Table Area
            float pageSize = 8f;
            Table tableItem = new Table(5, false);
            tableItem.SetBorder(new SolidBorder(1f));
            
            documentItextFormater.DrawcellHeader(
                "Codigo Producto", pageSize, subTitleColorBackground, tableItem, blackColor, 
                TextAlignment.CENTER, true);
            documentItextFormater.DrawcellHeader(
                "Cantidad", pageSize, subTitleColorBackground, tableItem, blackColor, 
                TextAlignment.CENTER, true);
            documentItextFormater.DrawcellHeader(
                "Descripcion", pageSize, subTitleColorBackground, tableItem, blackColor, 
                TextAlignment.CENTER, true);
            documentItextFormater.DrawcellHeader(
                "Precio Unitario", pageSize, subTitleColorBackground, tableItem, blackColor, 
                TextAlignment.CENTER, true);
            documentItextFormater.DrawcellHeader(
                "Subtotal", pageSize, subTitleColorBackground, tableItem, blackColor, 
                TextAlignment.CENTER, true);
            
            foreach (var detail in Details)
            {
                string description = detail.Descripcion != null ? detail.Descripcion : "N/D";
                
                //Draw each item on Area
                documentItextFormater.DrawcellItem(
                    detail.CodigoProducto, pageSize, true, tableItem, TextAlignment.LEFT, true);
                documentItextFormater.DrawcellItem(
                    detail.Cantidad.ToString(), pageSize, false, tableItem, TextAlignment.RIGHT, true);
                documentItextFormater.DrawcellItem(
                    description, pageSize, true, tableItem, TextAlignment.LEFT, true);
                documentItextFormater.DrawcellItem(
                    detail.PrecioUnitario, pageSize, false, tableItem, TextAlignment.RIGHT, true);
                documentItextFormater.DrawcellItem(
                    detail.SubTotal.ToString(), pageSize, false, tableItem, TextAlignment.RIGHT, true);   
            }
            
            ResultDoc.Add(tableItem);
        }
    }
}
