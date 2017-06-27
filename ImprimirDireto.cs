using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Drawing;

namespace Utils
{
    public class ImprimirDireto : IDisposable
    {
        private int m_currentPageIndex;
        private IList<Stream> m_streams;
        public ImprimirDireto()
        {

        }

        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        public virtual void Print(LocalReport Report, PageProps PageProps)
        {
            Export(Report, PageProps);

            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Erro: Não é possível localizar a impressora padrão.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
                MessageBox.Show("Recibo do pedido enviado para impressora com sucesso.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        private void Export(LocalReport Report, PageProps PageProps)
        {            
            string deviceInfo = string.Format(
          @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>{0}in</PageWidth>
                <PageHeight>{1}in</PageHeight>
                <MarginTop>{2}in</MarginTop>
                <MarginLeft>{3}in</MarginLeft>
                <MarginRight>{4}in</MarginRight>
                <MarginBottom>{5}in</MarginBottom>
            </DeviceInfo>",
            PageProps.PageWidth,
            PageProps.PageHeight,
            PageProps.MarginTop, 
            PageProps.MarginLeft, 
            PageProps.MarginRight, 
            PageProps.MarginBottom);
            Warning[] warnings;
            m_streams = new List<Stream>();
            Report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }
        public void Dispose()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }
    }
}
