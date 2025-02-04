using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for AssociatedInteractionChoice.xaml
    /// </summary>
    public partial class AssociateInteractionChoice : UserControl
    {

        public event EventHandler? IsFilled;

        public AssociateInteractionChoice()
        {
            InitializeComponent();
        }

       

        private void Text_IsChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextAssociateLeft.Text) && !string.IsNullOrWhiteSpace(TextAssociateRight.Text))
            {
                IsFilled?.Invoke(this, new EventArgs());
            }
        }
    }
}
