   M S B A M L   `   `   `  ���� O  KUDM.Insure.Interface, Version=3.0.8.4, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:UDM.Insurance.Interface.ContentUDM.Insurance.Interface.Content  G CEmbriant.WPF, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullS9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPFEmbriant.WPF.Controls E    ?UDM.Insurance.Interface.Screens.SelectCMAgentToAssignTSRSavedCF   ` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Acontent-clr-namespace:UDM.Insurance.Interface.Content     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     Isrc9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPF     $��0q�  � AllowsTransparency  .  � 
ResizeMode$ CanMinimize=� � ShowInTaskbar .   � WindowStartupLocation$ CenterScreen=� � WindowState$ Normal=� � WindowStyle$
 None=� � Topmost . ��Z�  � 	Resources    StyleSelectButton  INFlatButton �� BasedOn	 �� 
TargetType
 �� Property �� Value �� Triggers �� Binding �� ElementName �� Path �� Value��%  &	        7  �� 8   "	 ��.��� 
 �@^�ShowOnDisabled . �� 
 �@�ToolTip Z�  ��  �� $ cmbCarriedForwardReason��$ SelectedValue� Z� .a��� 
 � {� .  �� 
 �@�ToolTip$) #Carried Forward Reason not Selected��.��(�  (� Stretch$ Uniform=�.��� $
��1920��$
��1080��.+�� -    �Name$
 Size��$	��515��$	��318��.+�(� $
 Fill=�.��� -   $ 
LayoutRoot��$	��515��$	��318��.+��� -    ��Name$ BGRectangle��$��Stretch=�
���   � �� RadiusX$ 12�� �� RadiusY$ 12��  BrandedBrushIN#��[ �� -    ��Name$ 	btnSelect��$��168,0,169,40q�$��Center=�$��Bottom=�$
��Save�� �� 	IsDefault . #��[  $��5��{�. 6   0UDM.Insurance.Interface.Content.LacePopupWindow1  $��2,6,0,0q�$	��Top=��� -    ��Name$% headingSelectCancellationReason��$��	81,49,0,0q�$	��Top=�  INPageHeadingText1#��[ $��CM Agent To Assign��F� $��1��$��81.449,73.241,238,0q�$	��Top=����� E, )��s�]���@ )�	 �� Stretch$
 Fill=�
�������,  &Embriant.WPF.Controls.EmbriantComboBox  -     Name$ 
cmbCMAgent��$��Center=�$	��290�� 	 cmbIN#��[ $��0��� $��Right=�$	��Top=�.+��� -   $ btnClose��$��18��$��18��  INCloseButton#��[ 