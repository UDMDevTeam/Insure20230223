   M S B A M L   `   `   `  ���� B  >UDM.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullH1clr-namespace:UDM.WPF.Converters;assembly=UDM.WPFUDM.WPF.Converters  G CEmbriant.WPF, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullS9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPFEmbriant.WPF.Controls  UDM.Insure.Interface?   9UDM.Insurance.Interface.Screens.MySuccessCopyCampaignData   ` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     H
converters1clr-namespace:UDM.WPF.Converters;assembly=UDM.WPF     u	 qInfragisticsWPF4.Documents.Excel.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbs
 oInfragisticsWPF4.DataPresenter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe aInfragisticsWPF4.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbm iInfragisticsWPF4.Editors.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb� }InfragisticsWPF4.DataPresenter.ExcelExporter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb<igWPF(http://schemas.infragistics.com/xaml/wpf 	 
    Ksrc9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPF 	 
    -   '  !Embriant.WPF.Controls.BaseControl   Name$!  ctrlCampaignsLeadCopyScreen�� � DataContext ��  �� RelativeSource �� *Self+ � 	Resources    cmdCopyLeads0   *UDM.WPF.Converters.InverseBooleanConverter  InverseBooleanConverter  BorderGapMaskConverter �� 
TargetType �� Property �� Value �� 
TargetType J� SnapsToDevicePixels	 � ColumnDefinitions
 � RowDefinitions �� Padding �� CornerRadius J� OpacityMask `� 	Converter `� ConverterParameter �� ElementName �� Path  	StyleCMB1 	 cmbIN �� BasedOn �� Triggers P� Property P� Value  BrandedBrushIN��%  &	        &	      &	 
     '��      7  &	 �    7  7  ��   �� �� " ��.���  � ��
 �������  � ��$ 2q���  � �� �� " ��.R��  . 	 �� $��6 ��� $
��Auto ��� $��* ��� $��6 �
 �� $
��Auto ��� $
��Auto ��� $��* ��� $��6 �.+��� $��Header��$��0��$��2��$��1��$ 3,1,3,0q�._��� $��Header����. # z �� $��1��$��3��$��0��$��4��#��z��
������ #��z��$ 4���� $��1��$��3��$��0��$��4��#��z��#��z��$ 3�� `� 8   $ 7��.1��� $ Header��$ ActualWidth��� $ ActualWidth� �� *Self+�� $ ActualHeight� �� *Self+�� $��2��$��1��$��2��#��z��# z �� 8   " �� P�  �@J�IsKeyboardFocusWithin . . ���  � ��8  .���� -    ��Name$ 	DimBorder��$
��1200��$	��750��
������ $��0q�$ 12�� J� Opacity$ 1Q����� .���  � ��
 ����  P�  � {� .  . ���  � ��
 �����._��� -   $ 
MainBorder��$
��1200��$	��750��
���   �#��[ $��2q�$ 12������ .���  �@J�Opacity$ 1Q� P�  � {� .  . ���  �@J�Opacity$	 0.7Q�._�(� .��� -    �Name$ 
LayoutRoot��$
��1200��$	��750�� +� FocusedElement �� $ cmbCampaigns��
 �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� 	 �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� .+��� -    ��Name$ cbAllowRecordFiltering��$��Center=�$y�	Collapsed=� d� 	IsChecked �� $ xdgSourceLeads��$( "FieldSettings.AllowRecordFiltering��� -    ��Name$ btnClose��$��0��$��27����.   INCloseButton#��[ $��0��{��� *ScrData.IsCopying+ �� 	Converter# [ �� -    ��Name$ hdrLeadCopy��$��1��$��1��$��8��$	��Top=�  INPageHeadingText1#��[ $#��My Success Campaign Data Copy��F� -    F�Name$ hdrLine��$��1��$��1��$��8��$	��168��$��	-1,0,0,12q�$
��Left=�  INUnderlineText1#��[ �� -	     ��Name$  gbSource��$��2��$��15��$��1��$��12��$��	0,-12,4,0q�$��14(�#��[ $��Source���� $��3��$��2��$��4��$
��Left=�! �� Orientation$! 
Horizontal=�$y�Visible=�.+��� -
   $ chkCampaignBase��$
��Left=�$��Center=�$ FalseY�����  	 INCheckboxStyle# [	 " ���� -   $ lblSourceBase��$
��Auto��$
��Auto��$��6,1,0,0q� 
 INLabelText2#��[
 $
��Base���� $��3��$��5��$��4��$
��Left=�$! 
Horizontal=�$y�Visible=�.+��� -   $ chkCampaignUpgrade��$
��Left=�$��Center=�$ FalseY����� # [	 " ���� -   $ lblSourceUpgrade��$
��Auto��$
��Auto��$��6,1,0,0q�#��[
 $��Upgrade���� $��3��$��8��$��4��$
��Left=�$! 
Horizontal=�$y�Visible=�.+��� -   $ chkCampaignResales��$
��0,12q�$
��Left=�$��Center=�$ FalseY�" J� RenderTransformOrigin$" 0.413,-0.703&����� # [	 " ���� -   $ lblCampaignResales��$
��Auto��$
��Auto��$��6,1,0,0q�#��[
 $��Resales���� $��3��$��15��$��4��$
��Left=�$! 
Horizontal=�$y�Visible=�.+��� -   $  chkDestinationCampaignBase��$
��Left=�$��Center=�$ FalseY����� # [	 " ���� -   $ lblDestinationSourceBase��$
��Auto��$
��Auto��$��6,1,0,0q�#��[
 $
��Base���� $��3��$��18��$��4��$
��Left=�$! 
Horizontal=�$y�Visible=�.+��� -   $# chkDestinationCampaignUpgrade��$
��Left=�$��Center=�$ FalseY����� # [	 " ���� -   $! lblDestinationSourceUpgrade��$
��Auto��$
��Auto��$��6,1,0,0q�#��[
 $��Upgrade���� $��3��$��21��$��4��$
��Left=�$! 
Horizontal=�$y�Visible=�.+��� -   $# chkDestinationCampaignResales��$
��0,12q�$
��Left=�$��Center=�$ FalseY�$" 0.413,-0.703&����� # [	 " ���� -   $# lblDestinationCampaignResales��$
��Auto��$
��Auto��$��6,1,0,0q�#��[
 $��Resales���� -   $ lblDocumentType��$��5��$��1��$��4��$	��142��$��8,0,0,10q�$��Bottom=�#��[
 $��Document Type��,  &Embriant.WPF.Controls.EmbriantComboBox  -   #  Name$# cmbDocumentType��$��5��$��5��$��6��$��Stretch=�$��Bottom=�#��[ $��0��4 
 .Infragistics.Windows.DataPresenter.XamDataGrid  -   $  Name$$ xdgSourceCampaigns��$��6��$��11��$��1��$��12��$��	8,27,8,10q�$��Stretch=�$��Stretch=�: 
 4Infragistics.Windows.DataPresenter.DataPresenterBase%   AutoFit�  �Infragistics.Windows.Helpers.NullableConverter`1[[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]$% False &   GroupByAreaLocation$
& None=�  INXamDataGridStyle1#���  $��0�� 9 
 3Infragistics.Windows.DataPresenter.HeaderPrefixArea7 
 1Infragistics.Windows.DataPresenter.RecordSelector' �� Mode��%p  '        '  �     �� "  .���  � y�$ Visible=���  � �� �� "  .R��� $, &SourceCampaignHeaderPrefixAreaCheckbox��-   $��Center=�$��Center=�$ FalseY�#��� 	 �� "  .���  � �� �� "  .R��� $* $SourceCampaignRecordSelectorCheckbox��-   $��Center=�$��Center=�|�.  #��� 	  �� *DataItem.Select+$' TwoWay=�(   FieldLayouts( 4	 
 .Infragistics.Windows.DataPresenter.FieldLayout	  ) 	  Fields.) .
 
 (Infragistics.Windows.DataPresenter.Field
  * 
  Name$* 
CampaignID��+ 
  Label$+ 
CampaignID��, 
  
Visibility$, 	Collapsed=�
  $* CampaignName��- 
  Width$	- 340X�$+ CampaignName��
  $* CampaignCode��$+ CampaignCode��$, 	Collapsed=�
  $* Select��$	- 290X�$+ Select��$, 	Collapsed=��� -   $ lblTotalLeads��$��16��$��1��$��7��$��8,0,0,4q�$��Bottom=�#��[
 $��Total Leads:��$y�	Collapsed=��� -   $ tbTotalLeads��$��16��$��4��$��	-24,0,0,4q�$��Bottom=�$��16(�  INLabelText3#��[ ���� *$"ScrData.Leads.TotalFilteredInLeads+�� -   $ lblLeadsSelected��$��16��$��7��$��7��$��8,0,0,4q�$��Bottom=�#��[
 $��Leads Selected:��$y�	Collapsed=��� -   $ tbLeadsSelected��$��16��$��10��$��0,0,0,4q�$��Bottom=�$��16(�#��[ ���� *ScrData.Leads.TotalChecked+�� -   $ lblLeadsAvailable��$��16��$��15��$��7��$��8,0,0,4q�$��Bottom=�#��[
 $��Leads Available:��$y�	Collapsed=��� -    $ tbLeadsAvailable��$��16��$��16��$��0,0,0,4q�$��Bottom=�$��16(�#��[ $y�	Collapsed=����� *#!ScrData.Leads.TotalLeadsAvailable+�� -!   $  gbDestination��$��2��$��15��$��14��$��12��$��	4,-12,0,0q�$��14(�#��[ $��Destination���� $��2��$��17��$��4��$
��Left=�$! 
Horizontal=�$y�Visible=��� $��2��$��19��$��4��$
��Left=�$! 
Horizontal=�$y�Visible=��� -"   $  lblDestinationDocumentType��$��5��$��14��$��4��$	��154��$��10,0,0,6q�$��Bottom=�#��[
 $��Document Type ��  -#   $ # cmbDestinationDocumentType��$��5��$��18��$��7��$��	10,0,40,0q�$��Stretch=�$��Bottom=�#��[ $��0��  -$   $$ xdgDestinationCampaigns��$��6��$��11��$��14��$��12��$��	8,27,8,10q�$��Stretch=�$��Stretch=�$% False $
& None=�#���  $��0�� ��%z  '        '  �     �� "  .���  � y�$ Visible=���  � �� �� "  .R��� $1 +DestinationCampaignHeaderPrefixAreaCheckbox��-%   $��Center=�$��Center=�$ FalseY�#��� 	 �� "  .���  � �� �� "  .R��� $/ )DestinationCampaignRecordSelectorCheckbox��-&   $��Center=�$��Center=�|�.  #��� 	  �� *DataItem.Select+$' TwoWay=�( 	  .) 
  $* 
CampaignID��$+ 
CampaignID��$, 	Collapsed=�
  $* CampaignName��$	- 340X�$+ CampaignName��
  $* CampaignCode��$+ CampaignCode��$, 	Collapsed=�
  $* Select��$	- 290X�$+ Select��$, 	Collapsed=��� -'   $ lblLeadsCopied��$��16��$��19��$��8��$��8,0,0,4q�$��Bottom=�#��[
 $��Leads Copied:��$y�	Collapsed=��� -(   $ tbLeadsCopied��$��16��$��22��$��4��$��	-12,0,0,4q�$��Bottom=�$��16(�#��[ ���� * ScrData.Leads.TotalLeadsCopied+�� -)   $ btnCopy��$��17��$��22��$��5��$��Right=�$��Bottom=�. �� 	IsDefault. .   INFlatButton#��[ $��0��.���� -*   / ��Name$/ atCopy��$��0q�$��Center=�$��Center=�$��_Copy��$y�Visible=�