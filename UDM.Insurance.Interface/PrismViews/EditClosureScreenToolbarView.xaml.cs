using System.Windows.Controls;
using System.Windows.Input;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.PrismViews
{
    public partial class EditClosureScreenToolbarView : UserControl
    {
        public EditClosureScreenToolbarView()
        {
            InitializeComponent();
        }

        private void EmbriantComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Methods.EmbriantComboBoxPreviewKeyDown(sender, e);
        }
    }
}
