   M S B A M L   `   `   `  ����G  CEmbriant.WPF, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullS9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPFEmbriant.WPF.Controls  5       UDM.Insure.Interface9   3UDM.Insurance.Interface.Screens.StatusLoadingScreen   5      ` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      u qInfragisticsWPF4.Documents.Excel.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbs	 oInfragisticsWPF4.DataPresenter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe
 aInfragisticsWPF4.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbm iInfragisticsWPF4.Editors.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb� }InfragisticsWPF4.DataPresenter.ExcelExporter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb<igWPF(http://schemas.infragistics.com/xaml/wpf  	 
   5      Lmc;http://schemas.openxmlformats.org/markup-compatibility/2006  	 
   5      Ksrc9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPF  	 
   5      -   5        '   !Embriant.WPF.Controls.BaseControl   Name$  CtrlStatusLoadingScreen��5	      
������ $��0q�5       � 	Resources 5         cmbStyle 	 cmbIN �� BasedOn �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� 	ClickMode  cmbToggleButton	 d� 	IsChecked
 �� Mode �� RelativeSource �� SelectionBoxItem �� SelectionBoxItemTemplate J� IsHitTestVisible �� 
IsReadOnly �� 
IsReadOnly  ComboBoxTextBox � AllowsTransparency � HorizontalOffset �� IsDropDownOpen � VerticalOffset �Name � ActualWidth �� MaxDropDownHeight J� SnapsToDevicePixels ��Name  BrandedBrushIN K� IsItemsHost-  'System.Windows.Input.KeyboardNavigation   DirectionalNavigation �� Triggers P� Property P� Value �� 
TargetName  BrandedBrushIN_light��%�  &	        7  7  7  7  7  �� 5   
   8   " ��5      .�5      ��  � �� 5      �� 5      " ��5      .R�5      �� $��cmbMainBorder��5      
����������#9d9999$��2q�5      $ 4��5      ._�5      � .+�5   "   d� $��ToggleButton��5   %   $��Stretch=�5    %   $ Press=�5!   %   |�.  8�� 	 5#   %   �� *IsDropDownOpen+$
 TwoWay=� �� *TemplatedParent+5$   %   �� 5%   "   $��ContentSite��5&   %   $��3,3,23,3q�5'   %   $
��Left=�5(   %   $��Center=�5)   %   #��z 5*   %   #��z 5+   %   #��z��5,   %    .  5-   %   �� 5.   "   $��PART_EditableTextBox��5/   %   $��3,3,23,3q�50   %   $
��Left=�51   %   $��Center=�52   %   
������ |�. # z 55   %   8�� $y�Hidden=�58   %   ��56   %   Z� 58   %   � 59   "   $��Popup��5:   %    . |�.  $ 5��5=   %   #��z 5>   %   $��Relative=�5?   %   $��Slide=�5@   %   $ 30��5A   %   .��5B   &   � $ DropDown��5C   )   #��z 5D   )   #��z 5E   )    . .+�5G   *   �� $ DropDownBorder��5H   -   
�������#���  5J   -   $��1q�5K   -   �� 5L   *   $��4,6,4,6q�5M   -    . .��5O   .   ��  . $ 	Contained=�5Q   1   5R   +   5S   '   5T   #   5U      5V       5W      P� 5X       �@J�	IsFocused . . �5Y   "   �� $ cmbMainBorder��6)   8   � ��6Z   �� 5Z   "   $ cmbMainBorder��6)   8   � ��6[   5[      5]      5_      5`      5a      .��5c      (� .��5d   
   � -   5        $ grdRoot��5e        +� FocusedElement  5f      �� ! �� ElementName$! cmbLead��" � RowDefinitions" 5g      �� 5h      $	��170 �5i      �� 5j      $
��Auto �5k      �� 5l      $	��250 �5m      .+�5o      (� $��1��5p      .��5q      �� 
���   �#��[ 5s      $��2q�5t      $ 12��5u      ._�5v      � -   5        $ 
LayoutRoot��5w      $
��1200��5x      $	��660��5y      # � ColumnDefinitions# 5z      �� 5{      $
��0.1* �5|   !   �� 5}      $
��0.1* �5~   !   �� 5      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �6/   �� 5�      $
��0.1* �6/   �� 5�      $
��0.1* �6/   �� 5�      $
��0.1* �6/   �� 5�      $
��0.1* �6/   �� 5�      $
��0.1* �6/   " 5�      �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   �� 5�      $
��0.1* �5�   !   .+�5�      �� $��1��5�      $��1��5�      $��5��5�      $	��Top=�5�        INDialogHeadingText1#��[ 5�      $��Lead Status Loading��5�      �� 5�      -   5        $ ��Name$$ btnClose��5�      $��0��5�      $��25��5�      $��2��5�        INCloseButton#��[ 5�      $��0��5�      �� 5�      -   5        % ��Name$% lblAgent��5�      $��1��5�      $��16��5�      $��3��5�      $��0,6,12,0q�5�      $��Right=�5�      $	��Top=�5�        INLabelText2#���  5�      $��Agent��5�      ,   &Embriant.WPF.Controls.EmbriantComboBox  5�      -   5        &  Name$& cmbAgent��5�      $��1��5�      $��19��5�      $��7��5�      $	��Top=�5�      #��[  5�      $��0��5�      �� 5�      -   5        $% lblLeadBook��5�      $��2��5�      $��16��5�      $��3��5�      $��0,6,12,0q�5�      $��Right=�5�      $	��Top=�5�      #���  5�      $��	Lead Book��5�      �� 5�      -   5        $% lblTotalCaptures��5�      $��14��5      $��15��5     $��4��5     $��0,11,0,0q�5     $��Right=�5     $	��Top=�5     #���  5     $��Total Captured: 0��5     $	��167��66   �� 5	     -	   5        $% lblTotalCapturedToday��5
     $��14��5     $��21��5     $��5��5     $��0,11,0,0q�5     $��Right=�5     $	��Top=�5     #���  5     $��Total Captured Today: 0��5     $	��207��6<     5     -
   5        $& cmbLeadBook��5     $��2��5     $��19��5     $��7��5     $	��Top=�5     #��[  5     $��0��5     4 	 .Infragistics.Windows.DataPresenter.XamDataGrid  5     -   5        '  Name$' xdgLeads��5     $��3��5      $��11��5!     $��1��5"     $��25��5#     $��0,0,0,-6q�5$      	 INBorderThickness1#��[	 5%     : 	 4Infragistics.Windows.DataPresenter.DataPresenterBase(   GroupByAreaLocation$
( None=�5(     ) � FocusVisualStyle) 5'     Z� ��5,     �� 5-  "   "  5.  %   .R�5/  &   �� $��Border��50  )   #��z��51  )   
�������#��z��53  )   $ 4��54  )   * �� Padding#* z��55  )    . ._�57  *   � $ PART_ContentSiteGrid��58  -   # z 59  -   5:  '    5;  &   P� 5<  *    �@J�	IsFocused . . �5=  .   �� $ Border��65   # [ 6`    � ��5>  +   5@  #   +   FieldLayoutSettings+ 5B     < 	 6Infragistics.Windows.DataPresenter.FieldLayoutSettings  5C  "   ,   SelectionTypeCell$
, None=�5D  %   -   SelectionTypeRecord$- Single=�5E  %   .   FieldSettings. 5G     6 	 0Infragistics.Windows.DataPresenter.FieldSettings  5H  "   /   FieldLayouts/ 5J     4 	 .Infragistics.Windows.DataPresenter.FieldLayout  5K  "   
0   Key$0 AssignLeadsLayout��5L  %   1   Fields.1 5M  &   .	 	 (Infragistics.Windows.DataPresenter.Field	  2 	  Name$2 ImportID��5N  )   3 	  Label$3 ImportID��5O  )   4 	  
Visibility$4 	Collapsed=�5P  )   	  5Q  &   $2 CampaignCode��5R  )   5 	  Width$5 75X�5S  )   $3 Campaign��5T  )   6 	  Settings6 5U  *     5V  .   7   	AllowEdit�
 
 �Infragistics.Windows.Helpers.NullableConverter`1[[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]$7 False
 5W  1   5Y  '   	  5Z  &   $2 	BatchCode��5[  )   $5 80X�5\  )   $3 Batch��5]  )   6 5^  *     5_  .   $7 False
 5`  1   5b  '   	  5c  &   $2 RefNo��5d  )   $	5 120X�5e  )   $3 Reference Number��5f  )   6 5g  *     5h  .   $7 False
 5i  1   5k  '   	  5l  &   $2 LeadName��5m  )   $	5 200X�5n  )   $3 	Lead Name��5o  )   6 5p  *     5q  .   $7 False
 5r  1   5t  '   	  5u  &   $2 
DateOfSale��5v  )   $5 80X�5w  )   $3 Date of Sale��5x  )   6 5y  *     5z  .   $7 False
 5{  1   5}  '   	  5�  &   $2 
LeadStatus��5�  )   $5 80X�5�  )   $3 Lead Status��5�  )   	  5�  &   $2 DeclineReasonCode��5�  )   $	5 100X�5�  )   $3 Decline Reason��5�  )   	  5�  &   -   5        $2 	IsChecked��5�  )   $5 60X�5�  )   $3 Checked��5�  )   6 5�  *     5�  .   8   
EditorType1  +Infragistics.Windows.Editors.XamCheckEditor"8  6B   $
7 True
 6m   5�  '   	  5�  &   $"2 FKINImportLatentLeadReasonID��5�  )   $5 0X�5�  )   $"3 FKINImportLatentLeadReasonID��5�  )   $4 	Collapsed=�5�  )   	  5�  &   $2 LatentLeadReason1��5�  )   $5 70X�5�  )   $3 
L Reason 1��5�  )   	  5�  &   $2 LatentLeadReason2��5�  )   $5 70X�5�  )   $3 
L Reason 2��5�  )   	  5�  &   $2 LatentLeadReason3��5�  )   $5 70X�5�  )   $3 
L Reason 3��5�  )   5�  #   5�     5�     5�     5�     5�     5�     5�     