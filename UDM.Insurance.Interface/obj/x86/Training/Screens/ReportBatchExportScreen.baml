   M S B A M L   `   `   `  ???? G  CEmbriant.WPF, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullS9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPFEmbriant.WPF.Controls  Q MUDM.Insurance.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null_Dclr-namespace:UDM.Insurance.Business;assembly=UDM.Insurance.BusinessUDM.Insurance.Business  UDM.Insure.Interface=   7UDM.Insurance.Interface.Screens.ReportBatchExportScreen   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     u	 qInfragisticsWPF4.Documents.Excel.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbs
 oInfragisticsWPF4.DataPresenter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb? }InfragisticsWPF4.DataPresenter.ExcelExporter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe aInfragisticsWPF4.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbm iInfragisticsWPF4.Editors.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb<igWPF(http://schemas.infragistics.com/xaml/wpf 	 
    Ksrc9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPF 	 
    [businessDclr-namespace:UDM.Insurance.Business;assembly=UDM.Insurance.Business 	 
      ? DataContext  ??  ?? RelativeSource ?? *Self+ ?? Path$ RData?.???? -    ??Name$ 	DimBorder??$
??1200??$	??750??
?????? $??0q? ?? CornerRadius$ 12?? J? Opacity$ 1Q????? .???  ?? Property ? ?? ?? Value
 ????  ?? Triggers P? 	 P? Property	 ? {?
 P? Value
 .  . ???  ? ??
 ?????._??? -   $ 
MainBorder??$
??1200??$	??750??
???   ?   BrandedBrushIN#???   $??2q?$ 12?????? .???  ?@J?Opacity$ 1Q? P? 	 ? {?
 .  . ???  ?@J?Opacity$	 0.7Q?._?(? .??? -    ?Name$ 
LayoutRoot??$
??1200??$	??750?? ? RowDefinitions ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ??  ? ColumnDefinitions ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? .+??? -    ??Name$ btnClose??$??27????.   INCloseButton#???  ?? -    ??Name$ hdrBatchExport??$??1??$??2??$??5??$??0q?$	??Top=?  INPageHeadingText1#???  $??Batch Export??F? -    F?Name$ hdrLine??$??1??$??2??$??3??$??0,0,4,14q?$
??Left=?  INUnderlineText1#???  ?? -   $ lblCampaigns??$??2??$??2??$??6??$??12,0,0,6q?$??Bottom=?  INLabelText2#???  $??Select Campaign(s)??$??NoWrap=?$y?Visible=?4 
 .Infragistics.Windows.DataPresenter.XamDataGrid  -     Name$ xdgCampaigns??$??3??$??13??$??2??$??8??$??0q?$??Stretch=?$??Stretch=?: 
 4Infragistics.Windows.DataPresenter.DataPresenterBase   AutoFit?  ?Infragistics.Windows.Helpers.NullableConverter`1[[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]$ False    GroupByAreaLocation$
 None=?  INXamDataGridStyle1#???  $??0?? ? 	Resources 9 
 3Infragistics.Windows.DataPresenter.HeaderPrefixArea ?? 
TargetType ?? 
TargetType ??Name d? 	IsChecked  INCheckboxStyle7 
 1Infragistics.Windows.DataPresenter.RecordSelector ?? Mode??%T  '        '  ?     ?? "  .???  ? y?$ Visible=???  ? ?? ?? "  .R??? $ HeaderPrefixAreaCheckbox??-	   $??Center=?$??Center=?$ FalseY?#???  ?? "  .???  ? ?? ?? "  .R??? $ RecordSelectorCheckbox??-
   $??Center=?$??Center=?|?.  #???   ?? *DataItem.Select+$ TwoWay=?   FieldLayouts 4 
 .Infragistics.Windows.DataPresenter.FieldLayout     Fields. . 
 (Infragistics.Windows.DataPresenter.Field     Name$ 
CampaignID??   Label$ 
CampaignID??   
Visibility$ 	Collapsed=?  $ CampaignCode??   Width$	 250X?$ Campaign Code??$ 	Collapsed=?  $ CampaignName??$	 290X?$ Campaign Name??  $ Select??$ Select??$ 	Collapsed=??? -   $ lblCal1??$??2??$??12??$??6??$??12,0,0,6q?$??Bottom=?#???  $??Select Date??$??NoWrap=?$y?Visible=?(? $??3??$??6??$??12??$??6??$	??250??$	??250??$??0q?$??Right=?$	??Top=?  (? Stretch$
  Fill=?.??3  -Infragistics.Windows.Editors.XamMonthCalendar  -   !  Name$
! Cal1??$??0q?$??16(?  INXamMonthCalendarStyle2#???  $??0??$y?Visible=??? -   $ tbPlatinumBatchCode??$??10??$??12??$??5??$??10,0,0,0q?$??Bottom=?#???  $??Platinum Life Batch Code??2	  ,Infragistics.Windows.Editors.XamMaskedEditor	  -   " 	 Name$" medPlatinumBatchCode??$??11??$??12??$??6??$??0q?$??Right=?$??Bottom=?# 	  Mask$#  ??$ 	  
PromptChar$$  ?? 	 medIN#???  $??0??$	??250???? $??13??$??12??$??4??% ?? Orientation$% Vertical=?$??2??.+?(? $??20??$
??Left=?.??	? -   & 	?Name$& radNormalBatch??$??0,0,10,0q?' 	? 	GroupName$' rbgBatchType?? 	 INRadioButton#??[	 $??54200?? ?? $ 	BatchType? 
 rbCheckedConverter( ?? 	Converter#( [
 +
  %UDM.Insurance.Business.lkpINBatchType) 
  Normal* ?? ConverterParameter#* Z) .???? $??-4,0,0,0q?Normal? $??12??(? $??20??$
??Left=?.??	? -   $& radRedeemed??$??0,0,10,0q?$' rbgBatchType??#??[	 $??54300?? ?? $ 	BatchType?#( [
 + 
  Redeemed#* Z+ .???? $??-4,0,0,0q?
Redeemed? $??12??(? $??20??$
??Left=?.??	? -   $& radNonRedeemed??$??0,0,10,0q?$' rbgBatchType??#??[	 $??54300?? ?? $ 	BatchType?#( [
 , 
  NonRedeemed#* Z, .???? $??-4,0,0,0q?NonRedeemed?? -   $ 	btnExport??$??15??$??22??$??4??$??0q?$??Right=?$??Bottom=?$??Export??{?.    INFlatButton#???  $??0??