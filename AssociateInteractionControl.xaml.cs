using QTIEditor.QTI;
using QTIEditor.QTI.Interactions;
using QTIEditor.QTI.SimpleTypes;
using QTIEditor.QTI.VariableProcessing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QTIEditor
{

    public class AssociateInteractionChoiceList : ObservableCollection<AssociateInteractionChoice>
    {
        public readonly List<ObservableAssociation> associations = [];
        
        public void OnLastElementFilled(object? sender, EventArgs e)
        {
            Add();
        }

        public void Add()
        {
            ObservableAssociation assoc = new();
            AssociateInteractionChoice newItem = new();
            newItem.IsFilled += OnLastElementFilled;

            if (Count > 0)
            {
                this[Count - 1].IsFilled -= OnLastElementFilled;
            }

            Binding binding1 = new("Item1") { Source = assoc };
            Binding binding2 = new("Item2") { Source = assoc };


            newItem.TextAssociateLeft.SetBinding(TextBox.TextProperty, binding1);
            newItem.TextAssociateRight.SetBinding(TextBox.TextProperty, binding2);

            associations.Add(assoc);
            Add(newItem);
        }

        public AssociateInteractionChoiceList() 
        {
            Add();

        }

    }

    public class ObservableAssociation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _item1 = "";
        public string Item1 { get => _item1; 
            set
            {
                _item1 = value;
                PropertyChanged?.Invoke(this, new(nameof(Item1)));
            }
        }

        private string _item2 = "";
        public string Item2
        {
            get => _item2;
            set
            {
                _item2 = value;
                PropertyChanged?.Invoke(this, new(nameof(Item2)));
            }
        }
    }


    /// <summary>
    /// Interaction logic for AssociateInteraction.xaml
    /// </summary>
    public partial class AssociateInteractionControl : UserControl, IInteractionControl
    {

        public static string FriendlyName = "Match Interaction";

        private List<ObservableAssociation> choices
        {
            get => ((AssociateInteractionChoiceList)Resources["AssociatedInteractions"]).associations;
        }

        public string Prompt { get; set; } = "";


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


        public AssociateInteractionControl()
        {
            InitializeComponent();

        }
        
        public AssessmentItem ToQTIAssessmentItem()
        {
            SimpleMatchSet leftSet = [];
            SimpleMatchSet rightSet = [];
            List<(UniqueIdentifier, UniqueIdentifier)> matchingResponsePairs = [];
            foreach (ObservableAssociation choice in choices)
            {
                if (string.IsNullOrWhiteSpace(choice.Item1) && string.IsNullOrWhiteSpace(choice.Item2))
                {
                    continue;   
                }

                SimpleAssociableChoice leftChoice = new()
                {
                    matchMax = 1,
                    text = choice.Item1
                };
                SimpleAssociableChoice rightChoice = new()
                {
                    matchMax = 1,
                    text = choice.Item2
                };

                matchingResponsePairs.Add((leftChoice.identifier, rightChoice.identifier));

                leftSet.Add(leftChoice);
                rightSet.Add(rightChoice);
            }

            ResponseDeclaration responseDecl = ResponseDeclaration.TemplateDirectedPairResponse(matchingResponsePairs);
            responseDecl.ApplyMappingEven(_points);

            MatchInteraction interaction = new()
            { 
                simpleMatchSet = (leftSet, rightSet),
                prompt = new() { prompt = Prompt },
                Response = responseDecl
            };

            AssessmentItem ret = new() {
                title = "",
                timeDependent = false,  
                responseProcessing = ResponseProcessing.TemplateMapResponse(),
                Body = new()
                
            };

            ret.Body.items.Add(interaction);
            ret.outcomeDeclarations.Add(OutcomeDeclaration.TemplateScore());
            
            return ret;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Parent is StackPanel panel)
            {
                panel.Children.Remove(this);
            }
        }
    }
}
