using Prism.Events;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using UDM.Insurance.Interface.PrismInfrastructure;

namespace UDM.Insurance.Interface.PrismViews
{
    public partial class EditClosureScreenContentView : UserControl
    {

        #region Properties

        IEventAggregator _ea;

        public double[] FontSizes
        {
            get
            {
                return new double[] { 
                    3.0, 4.0, 5.0, 6.0, 6.5, 7.0, 7.5, 8.0, 8.5, 9.0, 9.5, 
                    10.0, 10.5, 11.0, 11.5, 12.0, 12.5, 13.0, 13.5, 14.0, 14.5, 15.0, 15.5,
                    16.0, 17.0, 18.0, 19.0, 20.0, 22.0, 24.0, 26.0, 28.0, 30.0,
                    32.0, 34.0, 36.0, 38.0, 40.0, 44.0, 48.0, 52.0, 56.0, 60.0, 64.0, 68.0, 72.0, 76.0,
                    80.0, 88.0, 96.0, 104.0, 112.0, 120.0, 128.0, 136.0, 144.0
                };
            }
        }

        public string[] lstTags
        {
            get
            {
                return new string[]
                {
                    "(mm)",
                    "(yyyy/mm/dd)",
                    "[Address123]",
                    "[Address45PostalCode]",
                    "[BenDateOfBirth]",
                    "[BenName]",
                    "[BenRelation]",
                    "[CellPhone]",
                    "[ChildCost]",
                    "[ChildCover]",
                    "[Clients name]",
                    "[Clients surname]",
                    "[CurrentLA1Cover]",
                    "[CurrentLA1Death]",
                    "[CurrentLA2Cover]",
                    "[DateOfBirth]",
                    "[EmailAddress]",
                    "[FuneralCover]",
                    "[IdNumber]",
                    "[HomePhone]",
                    "[LA1Cover]",
                    "[LA1Death]",
                    "[LA1FuneralCover]",
                    "[LA2Cover]",
                    "[LA2Death]",
                    "[NameSurname]",
                    "[TotalLA1Cover]",
                    "[TotalLA1Death]",
                    "[TotalLA2Cover]",
                    "[TotalLA2Death]",
                    "[TotalPremium]",
                    "[TSR]",
                    "[WorkPhone]",
                    "[AnnualPremium]",
                    "[LA1Cancer]",
                    "[LA2Cancer]"
                };
            }
        }

        public FontFamily[] FontFamilies
        {
            get
            {
                return new FontFamily[]
                {
                    new FontFamily("Arial"),
                    new FontFamily("Calibri"),
                    new FontFamily("Cambria"),
                    new FontFamily("Consolas"),
                    new FontFamily("Constantia"),
                    new FontFamily("Georgia"),
                    new FontFamily("Palatino"),
                    new FontFamily("Segoe UI"),
                    new FontFamily("Symbol"),
                    new FontFamily("Tahoma"),
                    new FontFamily("Times New Roman"),
                    new FontFamily("Verdana"),
                    new FontFamily("Webdings"),
                    new FontFamily("Wingdings"),
                    new FontFamily("Wingdings 2"),
                    new FontFamily("Wingdings 3")
                };
            }
        }

        #endregion



        #region Constructors

        public EditClosureScreenContentView(IEventAggregator ea)
        {
            _ea = ea;
            InitializeComponent();
            _ea.GetEvent<CloseDocumentEvent>().Subscribe(DocumentClosedEvent);
        }

        #endregion



        #region Events

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ContentPresenter TitleHost = DocRibbon.Template.FindName("PART_TitleHost", DocRibbon) as ContentPresenter;

            if (TitleHost != null)
            {
                ((UIElement)((FrameworkElement)(TitleHost).Parent).Parent).Visibility = Visibility.Collapsed;
            }

            _fontFamily.ItemsSource = FontFamilies;
            _fontSize.ItemsSource = FontSizes;
            _tagList.ItemsSource = lstTags;
        }

        private void FontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                FontFamily editValue = (FontFamily) e.AddedItems[0];
                ApplyPropertyValueToSelectedText(TextElement.FontFamilyProperty, editValue);
            }

            Keyboard.Focus(DocWorkspace);
            DocWorkspace.Focus();
        }

        private void FontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                ApplyPropertyValueToSelectedText(TextElement.FontSizeProperty, e.AddedItems[0]);
            }

            Keyboard.Focus(DocWorkspace);
            DocWorkspace.Focus();
        }

        private void RichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateVisualState();
        }

        private void TagList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                string text = (string) e.AddedItems[0];

                TextPointer caretPosition = DocWorkspace.Selection.Start;
                Run r = new Run(text, caretPosition);
                r.FontFamily = (FontFamily)DocWorkspace.Selection.GetPropertyValue(TextElement.FontFamilyProperty);
                r.FontWeight = (FontWeight)DocWorkspace.Selection.GetPropertyValue(TextElement.FontWeightProperty);
                r.Foreground = (Brush)DocWorkspace.Selection.GetPropertyValue(TextElement.ForegroundProperty);
                r.FontSize = (double)DocWorkspace.Selection.GetPropertyValue(TextElement.FontSizeProperty);
                r.FontStyle = (FontStyle)DocWorkspace.Selection.GetPropertyValue(TextElement.FontStyleProperty);;
                
                DocWorkspace.CaretPosition = r.ContentEnd;

                _tagList.SelectedItem = null;

                Keyboard.Focus(DocWorkspace);
                DocWorkspace.Focus();
            }
        }

        private void FontColor_Click(object sender, RoutedEventArgs e)  
        {  
            fontcolor(DocWorkspace);  
        }

        private void DocumentClosedEvent()
        {
            _fontFamily.SelectedItem = null;
        }

        #endregion



        #region Methods

        private void UpdateVisualState()
        {
            UpdateToggleButtonState();
            UpdateSelectionListType();
            UpdateSelectedFontFamily();
            UpdateSelectedFontSize();
        }

        private void UpdateToggleButtonState()
        {
            UpdateItemCheckedState(_btnBold, TextElement.FontWeightProperty, FontWeights.Bold);
            UpdateItemCheckedState(_btnItalic, TextElement.FontStyleProperty, FontStyles.Italic);
            UpdateItemCheckedState(_btnUnderline, Inline.TextDecorationsProperty, TextDecorations.Underline);

            UpdateItemCheckedState(_btnAlignLeft, Paragraph.TextAlignmentProperty, TextAlignment.Left);
            UpdateItemCheckedState(_btnAlignCenter, Paragraph.TextAlignmentProperty, TextAlignment.Center);
            UpdateItemCheckedState(_btnAlignRight, Paragraph.TextAlignmentProperty, TextAlignment.Right);
            UpdateItemCheckedState(_btnAlignJustify, Paragraph.TextAlignmentProperty, TextAlignment.Right);
        }

        void UpdateItemCheckedState(ToggleButton button, DependencyProperty formattingProperty, object expectedValue)
        {
            object currentValue = DocWorkspace.Selection.GetPropertyValue(formattingProperty);
            button.IsChecked = (currentValue == DependencyProperty.UnsetValue) ? false : currentValue != null && currentValue.Equals(expectedValue);
        }

        private void UpdateSelectionListType()
        {
            Paragraph startParagraph = DocWorkspace.Selection.Start.Paragraph;
            Paragraph endParagraph = DocWorkspace.Selection.End.Paragraph;
            if (startParagraph != null && 
                endParagraph != null && 
                (startParagraph.Parent is ListItem) && 
                (endParagraph.Parent is ListItem) &&
                ReferenceEquals(((ListItem)startParagraph.Parent).List, ((ListItem)endParagraph.Parent).List))
            {
                TextMarkerStyle markerStyle = ((ListItem)startParagraph.Parent).List.MarkerStyle;
                if (markerStyle == TextMarkerStyle.Disc) //bullets
                {
                    _btnBullets.IsChecked = true;
                }
                else if (markerStyle == TextMarkerStyle.Decimal) //numbers
                {
                    _btnNumbers.IsChecked = true;
                }
            }
            else
            {
                _btnBullets.IsChecked = false;
                _btnNumbers.IsChecked = false;
            }
        }

        private void UpdateSelectedFontFamily()
        {
            object value = DocWorkspace.Selection.GetPropertyValue(TextElement.FontFamilyProperty);
            FontFamily currentFontFamily = (FontFamily)((value == DependencyProperty.UnsetValue) ? null : value);
            if (currentFontFamily != null)
            {
                if (FontFamilies.Contains(currentFontFamily))
                {
                    _fontFamily.SelectedItem = currentFontFamily;
                }
                else
                {
                    _fontFamily.SelectedIndex = -1;
                }
            }
            else
            {
                _fontFamily.SelectedIndex = -1;
            }
        }

        private void UpdateSelectedFontSize()
        {
            object value = DocWorkspace.Selection.GetPropertyValue(TextElement.FontSizeProperty);
            _fontSize.SelectedValue = (value == DependencyProperty.UnsetValue) ? null : value;
        }

        void ApplyPropertyValueToSelectedText(DependencyProperty formattingProperty, object value)
        {
            if (value == null)
                return;

            DocWorkspace.Selection.ApplyPropertyValue(formattingProperty, value);
        }

        private void fontcolor(RichTextBox rc)  
        {  
            var colorDialog = new System.Windows.Forms.ColorDialog();  
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)   
            {  
                var wpfcolor = Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);  
                TextRange range = new TextRange(rc.Selection.Start, rc.Selection.End);  
                range.ApplyPropertyValue(FlowDocument.ForegroundProperty, new SolidColorBrush(wpfcolor));  
            }  
        }  

        #endregion
        
    }
}
