using QTIEditor.QTI.Interfaces;
using QTIEditor.QTI.Manifest;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace QTIEditor
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            Title = Constants.TOOL_NAME + " v" + Constants.VERSION;
         
            PopulateContextMenuAddItem();

        }

        private void PopulateContextMenuAddItem()
        {
            
            foreach (Type type in Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(mytype => mytype.GetInterfaces().Contains(typeof(IInteractionControl))))
            {
                FieldInfo fieldInfo = type.GetField("FriendlyName") ?? throw new UnreachableException();
                string propertyName = (string)(fieldInfo.GetValue(null) ?? throw new UnreachableException());
                MenuItem menuItem = new()
                {
                    Header = propertyName,
                    Tag = type,
                    
                };
                menuItem.Click += ContextMenuItemAddItem_Click;
                MenuAddItem.Items.Add(menuItem);

            }
        }



        public void ImportCsv(string path)
        {
            Helpers.suppressUpdate = true;

            var stream = File.OpenRead(path);

            BufferedStream fileReader = new(stream);
            Encoding utf8 = Encoding.UTF8;
            Queue<string[]> lines = new();
            Queue<string> lineValues = new();
            Queue<byte> cValue = new();
            int buf = fileReader.ReadByte();
            bool isParen = false;
            do
            {
                byte data = (byte)buf;
                if (isParen && data != '"')
                {
                    cValue.Enqueue(data);
                } else
                {

                    if (data == '\n')
                    {
                        lineValues.Enqueue(utf8.GetString([..cValue]));
                        cValue.Clear();
                        lines.Enqueue([.. lineValues]);
                        lineValues.Clear();

                    }
                    else if (data == '\r') { }
                    else if (data == '"')
                    {
                        isParen = !isParen;
                    }
                    else if (data == ',')
                    {
                        lineValues.Enqueue(utf8.GetString([.. cValue]));
                        cValue.Clear();
                    }
                    else
                    {
                        cValue.Enqueue(data);
                    }

                }


                buf = fileReader.ReadByte();

                if (buf < 0)
                {
                    lineValues.Enqueue(utf8.GetString([.. cValue]));
                    lines.Enqueue([.. lineValues]);
                    break;
                }
            }
            while (true);



            fileReader.Close();

            while (lines.Count > 0)
            {
                string[] line = lines.Dequeue();

                if (line[0] == "Type")
                    continue;

                bool trueFalse = line[0] == "TF";

                if (trueFalse && line[4] == "0")
                {
                    line[4] = "1";
                } else if (trueFalse)
                {
                    line[4] = "2";
                }
                List<string> choices;

                if (trueFalse)
                {
                    choices = ["True", "False"];
                } else
                {
                    choices = new(line[5..].Where(item => !string.IsNullOrWhiteSpace(item)));
                }

                string[] correctAnswers = line[4].Split(',');
                HashSet<uint> answerIdx = new(correctAnswers.Length);

                foreach (string ans in correctAnswers)
                {
                    answerIdx.Add(uint.Parse(ans) - 1);
                }

                var question = new ChoiceInteractionControl()
                {
                    Points = line[2],
                    Prompt = line[3],

                };


                
                uint i = 0;
                foreach (var choice in choices)
                {
                    question.ChoiceControlList.Add(new()
                    {
                        ChoiceValue = choice,
                        IsCorrect = answerIdx.Contains(i) 
                    });
                    i++;
                }

                question.ChoiceControlList.AddEmpty();


                InteractionStack.Children.Insert(InteractionStack.Children.Count - 1, question);
             

            }

            Helpers.suppressUpdate = false;
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog
            {
                AddExtension = true,
                DefaultExt = ".csv",
                Multiselect = false,
                Title = "Import data",
                Filter = "Comma Separated Values|*.csv|All Files|*.*"
            };
            var result = fileDialog.ShowDialog(this);

            if (result == true)
            {
                ImportCsv(fileDialog.FileName);
            }
            
            
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
                }
                catch (Exception exportException)
                {
                    MessageBox.Show($"An issue occurred during export. Error: \n {exportException}","Export Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }

        }

        public void ExportXml(string path)
        {

            List<IManifestLinkable> questions = [];

            foreach (UIElement interaction in InteractionStack.Children)
            {
                if (interaction is IInteractionControl associateInteraction)
                {
                    questions.Add(associateInteraction.ToQTIAssessmentItem());
                }
            }

            Manifest manifest = new()
            {
                metadata = new()
                {
                    title = "Export",
                    description = "",
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

        private void ContextMenuItemAddItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem contextMenu = (MenuItem)sender;
            Type newItemType = (Type)contextMenu.Tag;
            InteractionStack.Children.Insert(InteractionStack.Children.Count - 1, (UIElement)Activator.CreateInstance(newItemType));

        }

        
    }
}