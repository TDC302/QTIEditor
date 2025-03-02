using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using QTIEditor.Export.PDF;
using QTIEditor.QTI.Interfaces;
using System.IO;
using System.Runtime.InteropServices.Swift;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Windows.ApplicationModel.Appointments;
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
        private readonly InMemoryRandomAccessStream stream = new();



        DateTime lastChange = DateTime.MinValue;

        public ExportPDF(List<IManifestLinkable> questions)
        {
            this.questions = questions;
            ReloadFinished += ExportPDF_ReloadFinished;
            InitializeComponent();

        }

        private void ExportPDF_ReloadFinished(object? sender, EventArgs e)
        {
            awaitingReload = false;
        }

        private bool _isInReload = false;
        private bool awaitingReload
        {
            get
            {
                return _isInReload;
            }
            set
            {
                _isInReload = value;
                LabelPreviewReload.Visibility = awaitingReload ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private event EventHandler? ReloadFinished;

        public void DrawQuestionsToPdf()
        {
            documentPreview = new();

            PdfPage titlePage = documentPreview.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(titlePage);
            
            XFont font = new("Verdana", 20);

            gfx.DrawString(TextBoxAssessmentTitle.Text, font, XBrushes.Black,
                        new XRect(0, 0, titlePage.Width.Point, titlePage.Height.Point/2),
                        XStringFormats.Center);


            XTextBlock block = new(TextBoxAssessmentDescription.Text, XBrushes.Black, new("Verdana", 10), titlePage.Width.Point / 3, titlePage.Height.Point / 3, titlePage.Width.Point / 3);

            block.Draw(gfx);            
            
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



        private void ChangesMade(object sender, TextChangedEventArgs e)
        {
            lastChange = DateTime.Now;
            if (awaitingReload)
                return;

            awaitingReload = true;

            UpdatePreview().ContinueWith(_ => ReloadFinished?.Invoke(this, EventArgs.Empty), TaskScheduler.FromCurrentSynchronizationContext());

        }


        private async Task UpdatePreview()
        {
            // wait for the user to stop making changes before rendering
            while (DateTime.Now - lastChange < TimeSpan.FromMilliseconds(500))
            {
                await Task.Delay(DateTime.Now - lastChange);
            }
            DrawQuestionsToPdf();
            documentPreview.Save(stream.AsStream());
            await Windows.Data.Pdf.PdfDocument.LoadFromStreamAsync(stream).AsTask()
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
    }
}
