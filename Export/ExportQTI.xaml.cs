using QTIEditor.QTI.Interfaces;
using QTIEditor.QTI.Manifest;
using System.IO.Compression;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using QTIEditor.QTI;

namespace QTIEditor.Export


{
    /// <summary>
    /// Interaction logic for ExportQTI.xaml
    /// </summary>
    public partial class ExportQTI : Window
    {

        private List<IManifestLinkable> questions;


       



        public ExportQTI(List<IManifestLinkable> questions)
        {
            this.questions = questions;

            

            InitializeComponent();

        }



        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = Constants.QTI_PACKAGE_FILE_EXT,
                Title = "Export" + Constants.QTI_PACKAGE_FRIENDLY_NAME,
                Filter = $"{Constants.QTI_PACKAGE_FRIENDLY_NAME}|*{Constants.QTI_PACKAGE_FILE_EXT}|All Files|*.*"
            };

            var res = saveDialog.ShowDialog(this);

            if (res == true)
            {
                try
                {
                    ExportXml(saveDialog.FileName);
                    Close();
                }
                catch (Exception exportException)
                {
                    MessageBox.Show($"An issue occurred during export. Error: \n {exportException}", "Export Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExportXml(string path)
        {

            if (CheckBoxShuffleItems.IsChecked == true)
            {
                foreach (IManifestLinkable question in questions)
                {
                    if (question is AssessmentItem assessmentItem)
                    {
                        var assessmentItemSections = assessmentItem.Body?.items;

                        if (assessmentItemSections != null)
                        {
                            foreach (var section in assessmentItemSections)
                            {
                                if (section is IShuffleable shuffleable)
                                {
                                    shuffleable.Shuffle = true;
                                }
                            }

                        }
                    }
                }
            }


            Manifest manifest = new()
            {
                metadata = new()
                {
                    title = TextBoxAssessmentTitle.Text,
                    description = TextBoxAssessmentDescription.Text,
                    copyright = ""
                },
                resources = questions,

            };

            var tempDir = Directory.CreateTempSubdirectory();

            Directory.SetCurrentDirectory(tempDir.FullName);

            manifest.WriteToFile();

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            ZipFile.CreateFromDirectory(tempDir.FullName, path);

        }
    }
}
