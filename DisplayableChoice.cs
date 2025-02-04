
using System.ComponentModel;
using System.Globalization;

using System.Windows.Controls;
using System.Windows.Data;

namespace QTIEditor
{
    public class DisplayableChoice : INotifyPropertyChanged, IEditableObject
    {

        public uint _points = 1;

        public uint Points
        {
            get => _points;
            set
            {
                _points = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Points)));
            }
        }
        public string Prompt { get; set; } = "";

        public string CorrectAnswer { get; set; } = "";


        public string this[int index]
        {
            get => GetChoice(index);
            set => SetChoice(index, value);
        }

        public string GetChoice(int index)
        {
            if (index >= Choices.Count)
                return "";
            else return Choices[index];
        }

        public void SetChoice(int index, string? value)
        {
            if (index >= Choices.Count)
            {
                if (value == "" || value == null)
                    return;

                Choices.Add(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Choice" + (index + 1)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Choice" + Choices.Count));
            }
            else
            {
                if (value == null || value == "")
                {
                    Choices.RemoveAt(index);
                    for (int i = index + 1; i <= 5; i++)
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Choice" + i));
                    }
                }
                else 
                {
                    Choices[index] = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Choice" + index + 1));
                }   
            }
        }

        public HashSet<int>? ParseAnswerIdx()
        {

            string[] correctAnswers = CorrectAnswer.Split(',');
            HashSet<int> answerIdx = new(correctAnswers.Length);

            for (int i = 0; i < correctAnswers.Length; i++)
            {
                string ans = correctAnswers[i];

                if (int.TryParse(ans, out int res))
                {
                    answerIdx.Add(res - 1);
                } else
                {
                    return null;
                }

            }

            return answerIdx;

        }

        private uint savedPoints;
        private string savedPrompt = "";
        private string savedAnswer = "";
        private List<string> savedChoices = [];

        public void BeginEdit()
        {
            savedPoints = _points;
            savedPrompt = Prompt;
            savedAnswer = CorrectAnswer;
            savedChoices = Choices;
        }

        public void CancelEdit()
        {
            _points = savedPoints;
            Prompt = savedPrompt;
            CorrectAnswer = savedAnswer;
            Choices = savedChoices;
        }

        public void EndEdit()
        {
            
        }

        internal List<string> Choices = [];

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public class ValidateChoice : ValidationRule
    {


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            if (value is BindingGroup bindingGroup && bindingGroup.Items.Count > 0 && bindingGroup.Items[0] is DisplayableChoice choice)
            {
                
                if (choice.Choices.Count <= 1)
                {
                    return new ValidationResult(false, "There are not enough choices!");
                }
                

                return ValidateCorrectAnswer(choice);
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }


        private ValidationResult ValidateCorrectAnswer(DisplayableChoice choice)
        {
            HashSet<int>? answers = choice.ParseAnswerIdx();
            if (answers == null)
                return new ValidationResult(false, $"CorrectAnswer should be one or more numbers separated by commas, not \"{choice.CorrectAnswer}\"");

            if (answers.Count < 1)
            {
                return new ValidationResult(false, "No correct answers were specified.");
            }

            foreach (int answer in answers)
            {
                if (answer >= choice.Choices.Count || answer < 0)
                {
                    return new ValidationResult(false, $"CorrectAnswer must be within the range of valid answers (1-{choice.Choices.Count}), not {answer+1}");
                }
            }


            return ValidationResult.ValidResult;
        }
    }


}
