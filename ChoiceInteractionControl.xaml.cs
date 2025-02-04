using QTIEditor.QTI;
using QTIEditor.QTI.Interactions;
using QTIEditor.QTI.SimpleTypes;
using QTIEditor.QTI.VariableProcessing;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace QTIEditor
{

    public class ChoiceItemControlList : ObservableCollection<ChoiceInteractionItemControl>
    {
        public readonly List<ObservableChoice> Responses = [];

        public void OnLastElementFilled(object? sender, EventArgs e)
        {
            if (!Helpers.suppressUpdate)
                AddEmpty();
        }


        public void AddEmpty()
        {
            
            ChoiceInteractionItemControl newItem = new();
            Add(newItem);
        }

        protected override void InsertItem(int index, ChoiceInteractionItemControl item)
        {
            ObservableChoice assoc = new();
            BindInsert(index, assoc);
            
        }

        protected void BindInsert(int index, ObservableChoice item)
        {
            ChoiceInteractionItemControl control = new();
            if (index == Count)
            {
                control.IsFilled += OnLastElementFilled;

                if (Count > 0)
                {
                    this[Count - 1].IsFilled -= OnLastElementFilled;
                }
            }

            Binding textBinding = new("ChoiceValue") { Source = item };
            Binding boolBinding = new("IsCorrect") { Source = item };


            control.AnswerValue.SetBinding(TextBox.TextProperty, textBinding);
            control.IsCorrect.SetBinding(CheckBox.IsCheckedProperty, boolBinding);

            Responses.Add(item);
            base.InsertItem(index, control);
        }


        public void Insert(int index, ObservableChoice item)
        {
            BindInsert(index, item);
        }

        public void Add(ObservableChoice item)
        {
            BindInsert(Count, item);
        }


        public ChoiceItemControlList()
        {
        }

    }

    public class ObservableChoice : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _choiceValue = "";
        public string ChoiceValue
        {
            get => _choiceValue;
            set
            {
                _choiceValue = value;
                PropertyChanged?.Invoke(this, new(nameof(ChoiceValue)));
            }
        }

        private bool _isCorrect = false;

        public bool IsCorrect
        {
            get => _isCorrect;
            set
            {
                _isCorrect = value;
                PropertyChanged?.Invoke(this, new(nameof(IsCorrect)));
            }

        }
    }


    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ChoiceInteractionControl : UserControl, IInteractionControl
    {

        public static string FriendlyName = "Multiple Choice";

        public ChoiceItemControlList ChoiceControlList
        {
            get; set;
        } = [];

        public List<ObservableChoice> Responses
        {
            get => ChoiceControlList.Responses;
        }

        public string Prompt
        {
            get; set;
        } = "";

        public double _points = 1;
        public string Points
        {
            get
            {
                return _points.ToString();
            }
            set
            {
                if (double.TryParse(value, out double possibleValue))
                {
                    if (double.IsNaN(possibleValue) || !double.IsRealNumber(possibleValue))
                    {
                        throw new ArgumentException("Nice try.");
                    }
                    else if (double.IsNegative(possibleValue))
                    {
                        throw new ArgumentException("Score cannot be negative");
                    }

                    _points = possibleValue;
                }
                else
                {
                    throw new ArgumentException("Score should be a number");
                }


            }
        }

        public ChoiceInteractionControl()
        {
            InitializeComponent();
            if (!Helpers.suppressUpdate)
            {
                ChoiceControlList.AddEmpty();
            }
        }




        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Parent is StackPanel panel)
            {
                panel.Children.Remove(this);
            }
        }

        public AssessmentItem ToQTIAssessmentItem()
        {
            AssessmentItem assessmentItem = new() 
            {
                title = "",
                timeDependent = false,
                Body = new()
            };

            List<int> answerIdxes = [];
            List<SimpleChoice> choices = [];

            for (int i = 0; i < Responses.Count; i++)
            {
                ObservableChoice choice = Responses[i];

                if (string.IsNullOrWhiteSpace(choice.ChoiceValue))
                    continue;

                if (choice.IsCorrect)
                    answerIdxes.Add(i);

                
                choices.Add(new() { text = choice.ChoiceValue });
            }

            ChoiceInteraction interaction;
            if (answerIdxes.Count > 1)
            {
                List<UniqueIdentifier> answerIds = [];
                foreach (var idx in answerIdxes)
                {
                    answerIds.Add(choices[idx].identifier);
                }

                interaction = new()
                {
                    choices = choices,
                    Response = ResponseDeclaration.TemplateCorrectResponse(answerIds),
                    maxChoices = 0,
                    prompt = new()
                    {
                        prompt = Prompt
                    }
                };
            }
            else
            {
                interaction = new()
                {
                    choices = choices,
                    CorrectChoiceIndex = answerIdxes.Single(),
                    prompt = new() { prompt = Prompt }
                };
            }

            if (_points != 1 || answerIdxes.Count != 1)
            {

                interaction.Response.ApplyMappingEven(_points);
                assessmentItem.responseProcessing = ResponseProcessing.TemplateMapResponse();

            }
            else
            {
                assessmentItem.responseProcessing = ResponseProcessing.TemplateMatchCorrect();
            }


            assessmentItem.Body.items.Add(interaction);
            assessmentItem.outcomeDeclarations.Add(OutcomeDeclaration.TemplateScore());

            return assessmentItem;

        }
    }
}
