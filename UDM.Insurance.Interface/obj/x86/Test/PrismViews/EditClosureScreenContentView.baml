   M S B A M L   `   `   `  ???? `  \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35?Yclr-namespace:Microsoft.Windows.Controls.Ribbon.Primitives;assembly=RibbonControlsLibrary,Microsoft.Windows.Controls.Ribbon.Primitives   UDM.InsureTest.InterfaceE   ?UDM.Insurance.Interface.PrismViews.EditClosureScreenContentView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation       R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     X TPrism.Unity.Wpf, Version=7.1.0.431, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59R NPrism.Wpf, Version=7.1.0.431, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59&prismhttp://prismlibrary.com/   Mribbon@http://schemas.microsoft.com/winfx/2006/xaml/presentation/ribbon   i	ribbonPriYclr-namespace:Microsoft.Windows.Controls.Ribbon.Primitives;assembly=RibbonControlsLibrary   X	 TXceed.Wpf.Toolkit, Version=3.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f47toolkit)http://schemas.xceed.com/wpf/xaml/toolkit 	 -   !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  .  ? 	Resources =   7Microsoft.Windows.Controls.Ribbon.RibbonApplicationMenu ?? 
TargetType ?? Property ?? Value7   1Microsoft.Windows.Controls.Ribbon.RibbonTabHeader??%u   '        '  *     ?? "  .???  ? y?$ 	Collapsed=??? "  .???  ? ??$ 0????  ? ??$ 0??.??? $??1q?
?????? .+??? -    ??Name$ ContentBorder??
???????$??2q? ?? CornerRadius$ 4??._??  ? RowDefinitions ?? $
??Auto ??? .+?? -    ?Name$ grdDocRibbon??$??0??
?????? .+?.   (Microsoft.Windows.Controls.Ribbon.Ribbon  -   $??	DocRibbon??$??0q?   BrandedBrushIN#??[  
?????? $??2q?	   IsCollapsed	 .  ??.  "
   ShowQuickAccessToolBarOnTop
 .  ? ContextMenu Z? .@?1   +Microsoft.Windows.Controls.Ribbon.RibbonTab  $
??Home??.@?3   -Microsoft.Windows.Controls.Ribbon.RibbonGroup  $
??Edit?? Z? .@?4   .Microsoft.Windows.Controls.Ribbon.RibbonButton  -     Name$ 	_btnPaste??.  (System.Windows.Input.ApplicationCommands   Paste#??Z    Label$ Paste??   LargeImageSource$/ )../PrismInfrastructure/Images/Paste32.png??????  ?? ElementName$ DocWorkspace?? Z?    ControlSizeDefinition C	   =Microsoft.Windows.Controls.Ribbon.RibbonControlSizeDefinition	   	  	ImageSize$ Large=?  -   $ _btnCut??
   Cut#??Z $	 Cut??   SmallImageSource$- '../PrismInfrastructure/Images/Cut16.png?????? $ DocWorkspace?? Z?  	  $ Small=?  -   $ _btnCopy??   Copy#??Z $
 Copy??$. (../PrismInfrastructure/Images/Copy16.png?????? $ DocWorkspace?? Z?  	  $ Small=?  -   $ 	_btnClear??.
  (System.Windows.Documents.EditingCommands 
  Delete#??Z $ Clear??$0 *../PrismInfrastructure/Images/Delete16.png?????? $ DocWorkspace?? Z?  	  $ Small=?  -	   $ _btnUndo??   Undo#??Z $
 Undo??$. (../PrismInfrastructure/Images/Undo16.png?????? $ DocWorkspace?? Z?  	  $ Small=?  -
   $ _bntRedo??   Redo#??Z $
 Redo??$. (../PrismInfrastructure/Images/Redo16.png?????? $ DocWorkspace?? Z?  	  $ Small=?  -   $ _btnSelectAll??   	SelectAll#??Z $ 
Select All?????? $ DocWorkspace?? Z?   $
??Font?? Z? .@?:   4Microsoft.Windows.Controls.Ribbon.RibbonControlGroup  .@??? -    ??Name$ _fontFamily??$	??110?? ?? 
IsEditable .   Z? ?? -   $ 	_fontSize??$??45?? .   Z? ???? " ?? ?? Triggers ??  ?? Binding ?? *ClosureText+ ?? Value Z? .a???  ?@??SelectedIndex$ -1??  .@?:   4Microsoft.Windows.Controls.Ribbon.RibbonToggleButton  -     Name$ _btnBold??  
  
ToggleBold#??Z  !   SmallImageSource$.! (../PrismInfrastructure/Images/Bold16.png?????? $ DocWorkspace?? Z? "   ControlSizeDefinition" 	  $ Small=?# 	  IsLabelVisible# .    -   $ 
_btnItalic??$ 
  ToggleItalic#??Z$ $0! *../PrismInfrastructure/Images/Italic16.png?????? $ DocWorkspace?? Z? " 	  $ Small=?# .    -   $ _btnUnderline??% 
  ToggleUnderline#??Z% $3! -../PrismInfrastructure/Images/Underline16.png?????? $ DocWorkspace?? Z? " 	  $ Small=?# .    $??	Paragraph?? Z? .@?  .@?9   3Microsoft.Windows.Controls.Ribbon.RibbonRadioButton  -   &  Name$& _btnAlignLeft??' 
  	AlignLeft#??Z' (   Label$(  ??)   SmallImageSource$3) -../PrismInfrastructure/Images/LeftAlign16.png?????? $ DocWorkspace?? Z? *   ControlSizeDefinition* 	  $ Small=?# .    -   $& _btnAlignCenter??+ 
  AlignCenter#??Z+ $(  ??$5) /../PrismInfrastructure/Images/CenterAlign16.png?????? $ DocWorkspace?? Z? * 	  $ Small=?# .    -   $& _btnAlignRight??, 
  
AlignRight#??Z, $(  ??$4) .../PrismInfrastructure/Images/RightAlign16.png?????? $ DocWorkspace?? Z?   -   $& _btnAlignJustify??- 
  AlignJustify#??Z- $(  ??$6) 0../PrismInfrastructure/Images/JustifyAlign16.png?????? $ DocWorkspace?? Z? * 	  $ Small=?# .    .@?  -   $& _btnBullets??. 
  ToggleBullets#??Z. $(  ??$1) +../PrismInfrastructure/Images/Bullets16.png?????? $ DocWorkspace??* 	  $ Small=?# .    -   $& _btnNumbers??/ 
  ToggleNumbering#??Z/ $(  ??$3) -../PrismInfrastructure/Images/Numbering16.png?????? $ DocWorkspace??* 	  $ Small=?# .    $??Color?? Z? .@?  -   $  ??$7 1../PrismInfrastructure/Images/ColorDialog_32x.png??0 ? ToolTip$0 Change text color?? Z?  	  $ Large=?  $
??Tags??$y?Visible=? Z? .@?  .@??? -   $ _tagList??$	??145?? .   Z? ???? "   ??  ?? *ClosureText+ Z? .a???  ? {? .  ? -   $ grdDocWorkspace??$??1??
?????? .+?# 	 Xceed.Wpf.Toolkit.RichTextBox  -   1  Name$1 DocWorkspace??$??12q?$	??Top=?
???????
?????? $??2q?$??Calibri)?$??16(???.  $
??Auto=????? $ grdDocWorkspace??2 ?? Path$2 ActualHeight?3   Text3 ?? *ClosureText+3 	 -Xceed.Wpf.Toolkit.RichTextBoxFormatBarManager4   	FormatBar4 , 	 &Xceed.Wpf.Toolkit.RichTextBoxFormatBar  {?.  $y?	Collapsed=?5   TextFormatter5 $ 	 Xceed.Wpf.Toolkit.RtfFormatter  ???? "   ??  ?? *ClosureText+ Z? .a???  ? {? .  