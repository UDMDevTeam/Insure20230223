   M S B A M L   `   `   `  ???? G  CEmbriant.WPF, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullS9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPFEmbriant.WPF.Controls   UDM.InsureTest.Interface7   1UDM.Insurance.Interface.Screens.ReportBatchScreen   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     u qInfragisticsWPF4.Documents.Excel.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbs	 oInfragisticsWPF4.DataPresenter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb?
 }InfragisticsWPF4.DataPresenter.ExcelExporter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe aInfragisticsWPF4.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbm iInfragisticsWPF4.Editors.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb<igWPF(http://schemas.infragistics.com/xaml/wpf  	 
   Ksrc9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPF  	 
   -     ? DataContext  ??  ?? RelativeSource ?? *Self+ ?? Path$ RData?.???? -    ??Name$ 	DimBorder??$
??1200??$	??750??
?????? $??0q? ?? CornerRadius$ 12?? J? Opacity$ 1Q????? .???  ?? Property ? ?? ?? Value
 ????  ?? Triggers P? 	 P? Property	 ? {?
 P? Value
 .  . ???  ? ??
 ?????._??? -   $ 
MainBorder??$
??1200??$	??750??
???   ?   BrandedBrushIN#???   $??2q?$ 12?????? .???  ?@J?Opacity$ 1Q? P? 	 ? {?
 .  . ???  ?@J?Opacity$	 0.7Q?._?(? .??? -    ?Name$ 
LayoutRoot??$
??1200??$	??750?? ? RowDefinitions ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ??  ? ColumnDefinitions ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? .+?? -   $??Popup1??$??9??$??8??$	??700??$	??500??$??
-69,10,0,0q?$
??Left=?$	??Top=???.   ? 	StaysOpen .   ? PlacementTarget ??  ?? ElementName$ chkPopupShow??.??? $??
0,-10,0,-9q?
???   ?.+??? -    ??Name$ BGRectanglePopup??$
??0,10q?
???   ? ?? RadiusX$ 12?? ?? RadiusY$ 12??$??2??????   BrandedColourIN ?? Color# ?  ?? -    ??Name$ lblCampaign??$??
29,0,0,405q?$??Bottom=?  INLabelText2#???  $??Campaign???? -    ??Name$ btnPopupMark??$	??131??$??33??$??	0,0,30,39q?$??Right=?$??Bottom=?$
??Save??{?.    INFlatButton#???  $??0???? $	??279??$??
0,34,392,0q?$??Right=?$	??Top=?  INPageHeadingText1#???  $??Mark Completed Batches??,   &Embriant.WPF.Controls.EmbriantComboBox  -	     Name$ cmbCampaign??$??128,80,178,0q?$??Stretch=?$	??Top=?  cmbINHighlightBorder#???  $??0???? -
   $??
lstBatches??$	??493??$	??347??$??
29,133,0,0q?$
??Left=?$	??Top=?$??16(??? -   $ btnClose??$??27????.   INCloseButton#???  ?? -   $ hdrBumpUpReport??$??1??$??2??$??7??$??0q?$	??Top=?#???  $??Batch Report??F? -    F?Name$ hdrLine??$??1??$??2??$??6??$	??221??$??0,0,0,14q?$
??Left=?  INUnderlineText1#???    -   $ cmbReportType??$??1??$??2??$??4??$??7??$??
10,41,39,0q?$??Stretch=?$	??Top=?#???  $??0???? -   $ lblCampaigns_Copy??$??2??$??2??$??3??$??2,0,0,17q?$??Bottom=?#???  $??Report Type??$??NoWrap=?$y?Visible=??? -   $??chkPopupShow??$??2??$??2??$??11??$??23??$??	10,36,0,0q?$
??Left=?$	??Top=?$?? ??$y?Hidden=??? -   $ lblYear??$??4??$??2??$??11??$??6??$	??247??$??0,0,0,41q?$??Bottom=?#???  $
??Year??$??NoWrap=?$y?Visible=??? -   $&  lblIncludeLeadsCopiedToExtension??$??3??$??2??$??6??$	??258??$??0,0,0,24q?$??Bottom=?#???  $'??!Include Leads Copied to Extension??$??NoWrap=?$y?Visible=??? -   $ lblbIncludeCompleted??$??3??$??2??$??7??$	??258??$??2,0,0,0q?$??Bottom=?#???  $??Include Completed Batches??$??NoWrap=?$y?Visible=??? -   $&?? chkIncludeLeadsCopiedToExtension??$??3??$??8??$??0,3,0,0q?$
??Left=?$	??Top=??? -    ??Name$ chkIncludeCompleted??$??3??$??8??$??0,27,0,0q?$
??Left=?$	??Top=??? -   $* $lblOnlyBatchesImported91DaysAndAfter??$??3??$??10??$??6??$??0,0,0,24q?$	??Top=?#???  $??91 Day Batch Report???? -   $*??$chkOnlyBatchesImported91DaysAndAfter??$??3??$??14??$
??Left=?$	??Top=??? -   $ lblCombineUL??$??3??$??10??$??6??$??2,0,0,0q?$??Bottom=?#???  $??
Combine UL???? -   $??chkCombineUL??$??3??$??14??$??0,27,0,0q?$
??Left=?$	??Top=? d? 	IsChecked ?? *	CombineUL+?? -   $ lblSalesConversion??$??3??$??15??$??6??$??0,0,0,24q?$	??Top=?#???  $??Sales Conversion Report??$y?	Collapsed=??? -   $??chkSalesConversion??$??3??$??20??$
??Left=?$	??Top=?$y?	Collapsed=? ?? *SalesConversionPerBatch+?? -   $ lblContactsConversion??$??3??$??15??$??6??$??Bottom=?#???  $ ??Contacts Conversion Report??$y?	Collapsed=??? -   $??chkContactsConversion??$??3??$??20??$
??Left=?$??Bottom=?$y?	Collapsed=? ?? *ContactsConversionPerBatch+&   System.Windows.Controls.Calendar  -   $??clYear??$??5??$??10??$??7??$	??269??$??32??$??0,10,0,0q?$
??Left=?$	??Top=?$??Arial)?$??32(?
???  ??$y?Visible=??? -   $ lblCampaigns??$??4??$??2??$??4??$??0,0,0,9q?$??Bottom=?#???  $??Select Campaign(s)??$??NoWrap=?$y?Visible=??? -    $ lblMonth??$??6??$??11??$??6??$	??247??$??Bottom=?#???  $??Month??$??NoWrap=?$y?Visible=?4 	 .Infragistics.Windows.DataPresenter.XamDataGrid  -!     Name$ xdgCampaigns??$??5??$??11??$??2??$??8??$??Stretch=?$??Stretch=?: 	 4Infragistics.Windows.DataPresenter.DataPresenterBase   AutoFit?  ?Infragistics.Windows.Helpers.NullableConverter`1[[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]$ False    GroupByAreaLocation$
 None=?  INXamDataGridStyle1#???  $??0?? ? 	Resources 9 	 3Infragistics.Windows.DataPresenter.HeaderPrefixArea ?? 
TargetType  ?? 
TargetType 	 INCheckboxStyle7 	 1Infragistics.Windows.DataPresenter.RecordSelector! ?? Mode??%T  '        '  ?     ?? "  .???  ? y?$ Visible=???  ? ?? ?? "   .R??? $ HeaderPrefixAreaCheckbox??-"   $??Center=?$??Center=?$ FalseY?#??? 	 ?? "  .???  ? ?? ?? "   .R??? $ RecordSelectorCheckbox??-#   $??Center=?$??Center=?|?.  #??? 	  ?? *DataItem.Select+$! TwoWay=?"   FieldLayouts" 4 	 .Infragistics.Windows.DataPresenter.FieldLayout  #   Fields.# .	 	 (Infragistics.Windows.DataPresenter.Field	  $ 	  Name$$ 
CampaignID??% 	  Label$% 
CampaignID??& 	  
Visibility$& 	Collapsed=?	  $$ CampaignCode??' 	  Width$	' 250X?$% Campaign Code??$& 	Collapsed=?	  $$ CampaignName??$	' 290X?$% Campaign Name??	  $$ Select??$% Select??$& 	Collapsed=?  .# 	  $$ CampaignTypeID??$% CampaignTypeID??$& 	Collapsed=?	  $$ CampaignType??$	' 290X?$% Campaign Type??	  $$ Select??$% Select??$& 	Collapsed=??? -$   $??cmbMonth??$??7??$??11??$??5??$	??190??$??31??$??0,10,0,0q?$
??Left=?$	??Top=?$??16(?$
??Bold"?$y?Visible=??? -%   $ 	btnReport??$??15??$??22??$??4??$??0q?$??Right=?$??Bottom=?$??Report??{?.  #???  $??0???? -&   $ btnMarkCompletedBatches??$??16??$??2??$??22??$??5??$	??188??$??	0,0,10,38q?$??Right=?$??Bottom=?$??Mark Completed Batches??#???  $??0??