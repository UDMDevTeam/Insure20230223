   M S B A M L   `   `   `  ���� G  CEmbriant.WPF, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullS9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPFEmbriant.WPF.Controls  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  UDM.Insure.InterfaceA   ;UDM.Insurance.Interface.Screens.AssignPossibleBumpUpsScreen   ` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     u	 qInfragisticsWPF4.Documents.Excel.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbs
 oInfragisticsWPF4.DataPresenter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe aInfragisticsWPF4.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbm iInfragisticsWPF4.Editors.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb� }InfragisticsWPF4.DataPresenter.ExcelExporter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb<igWPF(http://schemas.infragistics.com/xaml/wpf 	 
    Lmc;http://schemas.openxmlformats.org/markup-compatibility/2006 	 
    Ksrc9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPF 	 
    8sys&clr-namespace:System;assembly=mscorlib 	 
    -   '   !Embriant.WPF.Controls.BaseControl   Name$%  CtrlAssignPossibleBumpUpsScreen��
������ $��0q� � 	Resources    RadialBrush 
� Center 
� GradientOrigin 
� RadiusX 
� RadiusY � Offset � Color��%{   &	        
� $ 0.5,0.5&�$ 0.5,0.5&�$	 0.5Q�$	 0.5Q���� $ 0Q�$ White��� $	 1.0Q�$ 	LightGray��.��(� .��� -    �Name$ grdRoot��	 � RowDefinitions	 �� $	��170 ��� $
��Auto ��� $	��250 �.+�(� $��1��.��� -   $ 
LayoutRoot��$
��1000��$	��660��
 � ColumnDefinitions
 �� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* �	 �� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* ��� $
��0.1* �.+��� -    ��Name$ BGRectangle��$��15��$��22��
���   � �� RadiusX$ 12�� �� RadiusY$ 12��$��2������   BrandedColourIN �� Color# �  �� -    ��Name$ btnClose��$��0��$��20��$��2��$��X�� �� IsCancel .   INCloseButton#���  $��0���� -    ��Name$ headerAssignLeads��$��1��$��1��$��6��$��0q�$
��Left=�$	��Top=�  INPageHeadingText1#���  $��Assign Possible Bump-Ups��F� -    F�Name$ 
headerLine��$��1��$��1��$��6��$��	0,0,10,17q�$��Right=�  INUnderlineText1#���  4 
 .Infragistics.Windows.DataPresenter.XamDataGrid  -     Name$ xdgAssignSales��$��2��$��11��$��11��$��10��: 
 4Infragistics.Windows.DataPresenter.DataPresenterBase   AutoFit�  �Infragistics.Windows.Helpers.NullableConverter`1[[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]$ False    GroupByAreaLocation$
 None=�  INXamDataGridStyle1#���  $��0��   BrandedColourIN_mediumlight9 
 3Infragistics.Windows.DataPresenter.HeaderPrefixArea �� 
TargetType �� Property �� Value �� 
TargetType ��Name d� 	IsChecked  INCheckboxStyle7 
 1Infragistics.Windows.DataPresenter.RecordSelector �� Mode��%�  (��       * SystemColors.HighlightBrushKey+)'       '  �     �� # �  �� "  .���  � y�$ Visible=���  � �� �� "  .R��� $ HeaderPrefixAreaCheckbox��-	   $��Center=�$��Center=�$ FalseY�#���  �� "  .���  � �� �� "  .R��� $ RecordSelectorCheckbox��$��Center=�$��Center=�|�.  #���   �� *DataItem.Select+$ TwoWay=�   FieldLayouts 4 
 .Infragistics.Windows.DataPresenter.FieldLayout  
   Key$ AssignSalesLayout��   Fields. . 
 (Infragistics.Windows.DataPresenter.Field      Name$  UserID��!   Label$! User ID��"   
Visibility$" 	Collapsed=�  $  
SalesAgent��#   Width$	# 250X�$! Bump-Up Agent��  $  LeadsAllocated��$# 90X�$! 	Allocated��  $  Select��$! Select��$" 	Collapsed=�  $  Index��$! Index��$" 	Collapsed=�  $  Assign��$# 75X�$! Assign��$   Settings$ 6	 
 0Infragistics.Windows.DataPresenter.FieldSettings	  % 	  	AllowEdit$
% True & 	  
EditAsType"& ��' 	  EditorStyle' �� 3
  -Infragistics.Windows.Editors.XamNumericEditor" 
 .��� 2  ,Infragistics.Windows.Editors.XamMaskedEditor �@ 
PromptChar$  ���� -
   $ lblCampaign��$��2��$��1��$��3��$��0q�$��Bottom=�  INLabelText2#���  $��Campaign���� -   $ 
tbCampaign��$��2��$��5��$��5��$��0q�$
��Left=�$��Bottom=� 	 INLabelText3#��� 	 $��Campaign��$��NoWrap=��� -   $ lblDateOfSale��$��3��$��1��$��2��$��0q�$��Bottom=�#���  $��Date of Sale���� -   $ tbDateOfSale��$��3��$��5��$��2��$��0q�$
��Left=�$��Bottom=�#��� 	 $��Date of Sale��$��NoWrap=��� -   $ lblTotalAssigned��$��4��$��1��$��3��$��0q�$��Bottom=�#���  $��Total Assigned���� -   $ tbTotalAssigned��$��4��$��5��$��2��$��0q�$
��Left=�$��Bottom=�#��� 	 $��Assigned��$��NoWrap=��� -   $ lblTotalUnassigned��$��5��$��1��$��4��$��0q�$��Bottom=�#���  $��Total Unassigned���� -   $ tbTotalUnassigned��$��5��$��5��$��2��$
��Left=�$��Bottom=�#��� 	 $��
Unassigned��$��NoWrap=��� -   $ lblNewAllocationDate��$��9��$��1��$��7��$��0q�$��Bottom=�#���  $��New allocation date��$y�Hidden=�4  .Infragistics.Windows.Editors.XamDateTimeEditor  -   (  Name$( dteNewAllocationDate��$��9��$��5��$��5��$��	0,0,0,-10q� 
 INXamDateTimeEditorStyle1#��[
 $��0��$y�Hidden=�.  (Infragistics.Windows.Editors.ValueEditor)   Value) �� *NewAllocationDate+�� -   $ btnAutoAssign��$��13��$��13��$��4��$��0q�$��Right=�$��Bottom=�$��Auto Assign��* �� 	IsDefault* .   INFlatButton#���  $��0���� -   $ 	btnAssign��$��13��$��17��$��4��$��0q�$��Right=�$��Bottom=�$��Apply��* . #���  $��0���� -   + ��Name$+ bAllocateMessage��$��6��$��3��$��5��$��12��#��[  $��0q�, �� CornerRadius$, 12��- J� Opacity$
- 0.95Q�$y�	Collapsed=� ��%    ._��� $��Center=�.+��� -   $ lblAllocateMessage1��$��Center=�$��20(�$
��Bold"�
���   �$��Allocate Message Line 1��� $��6���� -   $ lblAllocateMessage2��$��Center=�$��20(�
���   �$��Allocate Message Line 2��� $��6���� -   $ lblAllocateMessage3��$��Center=�$��20(�
���   �$��Allocate Message Line 3��