   M S B A M L   `   `   `  ???? G  CEmbriant.WPF, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullS9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPFEmbriant.WPF.Controls   UDM.InsureTest.InterfaceD   >UDM.Insurance.Interface.Screens.ReportTrackUpgradeBasePolicies   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     u qInfragisticsWPF4.Documents.Excel.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbs	 oInfragisticsWPF4.DataPresenter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb?
 }InfragisticsWPF4.DataPresenter.ExcelExporter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe aInfragisticsWPF4.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbm iInfragisticsWPF4.Editors.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb<igWPF(http://schemas.infragistics.com/xaml/wpf  	 
   Ksrc9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPF  	 
   -   .???? -     ??Name$  	DimBorder??$
??1200??$	??750??
?????? $??0q? ?? CornerRadius$ 12?? J? Opacity$ 1Q????? .???  ?? Property ? ?? ?? Value
 ????  ?? Triggers P?  P? Property ? {? P? Value .  . ???  ? ??
 ?????._??? -   $  
MainBorder??$
??1200??$	??750??
???   ?   BrandedBrushIN#???   $??2q?$ 12?????? .???  ?@J?Opacity$ 1Q? P?  ? {? .  . ???  ?@J?Opacity$	 0.7Q?._?(? .??? -    ?Name$ 
LayoutRoot??$
??1200??$	??750??	 ? RowDefinitions	 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 
 ? ColumnDefinitions
 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? .+??? -    ??Name$ btnClose??$??27????.   INCloseButton#???  ?? -    ??Name$' !hdrReportTrackUpgradeBasePolicies??$??1??$??2??$??7??$??0q?$	??Top=?  INPageHeadingText1#???  $!??Track Upgrade Base Policies??F? -    F?Name$ hdrLine??$??1??$??2??$??7??$	??283??$??0,0,0,14q?$
??Left=?  INUnderlineText1#???  ?? -   $ lblCal1??$??2??$??2??$??7??$??0q?$??Bottom=?  INLabelText2#???  $"??Select Start Date for Report??$??NoWrap=?$y?Visible=?3  -Infragistics.Windows.Editors.XamMonthCalendar  -	     Name$ calStartDate??$??3??$??7??$??2??$??7??$??
0,12,24,12q?$??16(?  INXamMonthCalendarStyle2#???  $??10??$y?Visible=??? -
   $ lblCal2??$??2??$??10??$??7??$??0q?$??Bottom=?#???  $ ??Select End Date for Report??$??NoWrap=?$y?Visible=?  -   $ 
calEndDate??$??3??$??7??$??10??$??7??$??
0,12,24,12q?$??16(?#???  $??20??$y?Visible=??? $??2??$??11??$??18??$??9??$??0,18,0,0q?$??18(?
???????$??Target??  gbStyle3#??[ .????  ?? Orientation$ Vertical=?.+?? $??12??$??4,0,4,0q?? $??34??$??4,0,4,0q?.+??? -   $ lblYear??$??0,0,0,6q?$??Bottom=?#???  $
??Year??$??NoWrap=?$y?Visible=?,   &Embriant.WPF.Controls.EmbriantComboBox  -     Name$ cmbYear??$	??150??$??0,0,0,0q?$??Right=?$??Bottom=? ?? SelectedItem$
 2018?? 	 cmbIN#??[ $??30??.@??? $
??2018???? $
??2019???? $
??2020???? $
??2021??? $??12??$??4,0,4,0q?? $??34??$??4,0,4,0q?.+??? -   $ lblMonth??$??0,0,0,6q?$??Bottom=?#???  $??Month??$??NoWrap=?$y?Visible=?  -   $ cmbMonth??$	??150??$??0,0,0,0q?$??Right=?$??Bottom=?#??[ $??30??.@??? $??January???? $??February???? $??March???? $??April???? $	??May???? $
??June???? $
??July???? $??August???? $??	September???? $??October???? $??November???? $??December??? $??12??$??4,0,4,0q?? $??34??$??4,0,4,0q?.+??? -   $ lblUpgradeLeadsReceived??$??0,0,0,6q?$??Bottom=?#???  $??Upgrade Leads Received??$??NoWrap=?$y?Visible=?2  ,Infragistics.Windows.Editors.XamMaskedEditor  -     Name$ medUpgradeLeadsReceived??$	??150??$??0,0,0,0q?$??Right=?$??Right=?   Mask$ {number:0-100000}??   
PromptChar$  ?? 	 medIN#??[ $??40??? $??12??$??4,0,4,0q?? $??34??$??4,0,4,0q?.+??? -   $ lblLesssheMaccLeads??$??0,0,0,6q?$??Bottom=?#???  $??Less SheMacc Leads??$??NoWrap=?$y?Visible=?  -   $ medLesssheMaccLeads??$	??150??$??0,0,0,0q?$??Right=?$??Right=?$ {number:0-100000}??$  ??#??[ $??50??? $??12??$??4,0,4,0q?? $??34??$??4,0,4,0q?.+??? -   $ lblTotalLeads??$??0,0,0,6q?$??Bottom=?#???  $??Total Leads??$??NoWrap=?$y?Visible=??? -   $ txtTotalLeads??$??0,0,0,5q?$??Right=?$??Bottom=? 	 INLabelText3#??? 	 $?? ??$??NoWrap=?$y?Visible=?? $??12??$??4,0,4,0q?? $??34??$??4,0,4,0q?.+??? -   $ lblTargetPercentage??$??0,0,0,6q?$??Bottom=?#???  $??Target Percentage??$??NoWrap=?$y?Visible=?  -   $ medTargetPercentage??$	??150??$??0,0,0,0q?$??Right=?$??Right=?$ {double:2.2:c}??$  ??#??[ $??50??.  (Infragistics.Windows.Editors.ValueEditor   Text$ 47.83??? $??12??$??4,0,4,0q?? $??34??$??4,0,4,0q?.+??? -   $ 	lblTarget??$??0,0,0,6q?$??Bottom=?#???  $??Target??$??NoWrap=?$y?Visible=??? -   $ 	txtTarget??$??0,0,0,5q?$??Right=?$??Bottom=?
??? ???#??? 	 $?? ??$??NoWrap=?$y?Visible=?? $??12??$??4,0,4,0q?? $??34??$??4,0,4,0q?.+??? -   $ btnSave??$	??150??$??0q?$??Right=?$??Bottom=?$
??Save??{?.   
 INFlatButton#??? 
 $??70??? $??12??$??4,0,4,0q?? $??34??$??4,0,4,0q?.+??? -   $ 
lblSavedBy??$??0,0,0,6q?$??Bottom=?#???  $??Saved By??$??NoWrap=?$y?Visible=??? -   $ 
txtSavedBy??$??0,0,0,5q?$??Right=?$??Bottom=?#??? 	 $?? ??$??NoWrap=?$y?Visible=??? -   $ 	btnReport??$??15??$??22??$??4??$??0q?$??Right=?$??Bottom=?$??Report??{?.  #??? 
 $??70??