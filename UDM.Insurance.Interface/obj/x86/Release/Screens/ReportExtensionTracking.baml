   M S B A M L   `   `   `  ???? G  CEmbriant.WPF, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullS9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPFEmbriant.WPF.Controls   UDM.Insure.Interface=   7UDM.Insurance.Interface.Screens.ReportExtensionTracking   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     u qInfragisticsWPF4.Documents.Excel.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbs	 oInfragisticsWPF4.DataPresenter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb?
 }InfragisticsWPF4.DataPresenter.ExcelExporter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe aInfragisticsWPF4.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbm iInfragisticsWPF4.Editors.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb<igWPF(http://schemas.infragistics.com/xaml/wpf  	 
   Ksrc9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPF  	 
   -   ????   ?? ImageSource$'  !../Resources/ReportBackground.jpg??.???? -    ??Name$ 	DimBorder??$
??1200??$	??750??
?????? $??0q? ?? CornerRadius$ 12?? J? Opacity$ 1Q????? .???  ?? Property ? ?? ?? Value
 ????  ?? Triggers P?  P? Property ? {? P? Value .  . ???  ? ??
 ?????._??? -   $ 
MainBorder??$
??1200??$	??750??
??????    BrandedBrushIN#???   $??2q?$ 12?????? .???  ?@J?Opacity$ 1Q? P?  ? {? .  . ???  ?@J?Opacity$	 0.7Q?._?(? .??? -   	 ?Name$	 
LayoutRoot??$
??1200??$	??750??
 ? RowDefinitions
 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ??  ? ColumnDefinitions ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? .+??? -    ??Name$ btnClose??$??27????.   INCloseButton#???  ?? -    ??Name$ hdrBumpUpReport??$??1??$??2??$??7??$??0q?$	??Top=?  INPageHeadingText1#???  $??Extension Tracking Report??F? -    F?Name$ hdrLine??$??1??$??2??$??6??$	??221??$??0,0,0,14q?$
??Left=?  INUnderlineText1#???  ?? -   $ lblYear??$??2??$??12??$??6??$??11,0,0,6q?$??Bottom=?  INLabelText2#???  $
??Year??$??NoWrap=?$y?	Collapsed=?$	??247???? -	   $ lblMonth??$??4??$??12??$??6??$??11,0,0,6q?$??Bottom=?#???  $??Month??$??NoWrap=?$y?	Collapsed=?$	??247???? -
   $ 	btnReport??$??15??$??22??$??4??$??0q?$??Right=?$??Bottom=?$??Report??{?.    INFlatButton#???  $??0???? -   $ lblCampaigns??$??2??$??2??$??3??$??	0,0,0,4.4q?$??Bottom=?#???  $??Select Campaign??$??NoWrap=?$y?Visible=?,   &Embriant.WPF.Controls.EmbriantComboBox  -     Name$ cmbCampaign??$??3??$??2??$??8??$??0,1,0,0q?$??Stretch=?$	??Top=?  cmbINHighlightBorder#???  $??0???? -   $ 
lblBatches??$??4??$??2??$??3??$??	0,0,0,5.4q?$??Bottom=?#???  $??Select Batch(es)??$??NoWrap=?$y?Visible=??? -   $??
lstBatches??$??8??$??2??$
??Left=?$	??511??$??5??$??13??$	??Top=?$	??331??$??14(?$
??Bold"??? -   $ lblCal1??$??2??$??12??$??7??$??11,0,0,0q?$??Bottom=?#???  $)??#Select Date Range (Allocation Date)??$??NoWrap=?$y?Visible=?$	??290??$??23??3  -Infragistics.Windows.Editors.XamMonthCalendar  -     Name$ calStartDate??$??
0,10,30,33q?$??16(?  INXamMonthCalendarStyle2#???  $??0??$y?Visible=?$??7??$??7??$??12??$??3??  -   $ 
calEndDate??$??
0,12,30,33q?$??16(?#???  $??0??$y?Visible=?$??7??$??7??$??19??$??3??