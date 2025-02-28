using PdfSharp.Drawing;
using PdfSharp.Pdf;
using QTIEditor.QTI.Interfaces;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Windows.Storage.Streams;

namespace QTIEditor.Export
{
    /// <summary>
    /// Interaction logic for ExportPDF.xaml
    /// </summary>
    public partial class ExportPDF : Window
    {

        readonly List<IManifestLinkable> questions;

        PdfDocument documentPreview = new();

        IRandomAccessStream stream = new InMemoryRandomAccessStream();

        public ExportPDF(List<IManifestLinkable> questions)
        {
            this.questions = questions;
            InitializeComponent();
            DrawQuestionsToPdf();


        }



        public void DrawQuestionsToPdf()
        {
            documentPreview = new();

            PdfPage page = documentPreview.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont font = new("Verdana", 20);


            gfx.DrawString("Hello, World!", font, XBrushes.Black,
                        new XRect(0, 0, page.Width.Point, page.Height.Point),
                        XStringFormats.Center);

            RenderPdfPreview();

        }

        public void RenderPdfPreview()
        {
            documentPreview.Save(stream.AsStream());

            Windows.Data.Pdf.PdfDocument.LoadFromStreamAsync(stream).AsTask()
                .ContinueWith(t2 => PdfToImages(t2.Result), TaskScheduler.FromCurrentSynchronizationContext());

        }




        private async Task PdfToImages(Windows.Data.Pdf.PdfDocument pdfDoc)
        {
            var items = PdfPreview.Items;
            items.Clear();

            if (pdfDoc == null) return;

            for (uint i = 0; i < pdfDoc.PageCount; i++)
            {
                using var page = pdfDoc.GetPage(i);
                var bitmap = await PageToBitmapAsync(page);
                var image = new Image
                {
                    Source = bitmap,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 4, 0, 4),
                    MaxWidth = 800
                };
                items.Add(image);
            }
        }

        private static async Task<BitmapImage> PageToBitmapAsync(Windows.Data.Pdf.PdfPage page)
        {
            BitmapImage image = new();

            using (var stream = new InMemoryRandomAccessStream())
            {
                await page.RenderToStreamAsync(stream);

                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream.AsStream();
                image.EndInit();
            }

            return image;
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = Constants.PDF_FILE_EXT,
                Title = "Export" + Constants.PDF_FILE_EXT,
                Filter = $"{Constants.PDF_FILE_FRIENDLY_NAME}|*{Constants.PDF_FILE_EXT}|All Files|*.*"
            };

            var res = saveDialog.ShowDialog(this);

            if (res == true)
            {
                var pdfStream = new FileStream(saveDialog.FileName, FileMode.Create, FileAccess.Write);
                stream.Seek(0);
                stream.AsStreamForRead().CopyTo(pdfStream);
                pdfStream.Dispose();
                stream.Dispose();
                Close();
            }
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
