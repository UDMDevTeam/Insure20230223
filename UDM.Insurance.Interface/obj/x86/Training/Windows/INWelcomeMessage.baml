   M S B A M L   `   `   `  ���� G  CEmbriant.WPF, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullQ8clr-namespace:Embriant.WPF.Windows;assembly=Embriant.WPFEmbriant.WPF.Windows   UDM.Insure.Interface6   0UDM.Insurance.Interface.Windows.INWelcomeMessage   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     Hsrc8clr-namespace:Embriant.WPF.Windows;assembly=Embriant.WPF     -   /   )Embriant.WPF.Windows.BaseMessageBoxWindow   Name$  Window�� � AllowsTransparency . 
������  � 
ResizeMode$ NoResize=� � ShowInTaskbar .   � WindowStartupLocation$ CenterScreen=� � WindowState$ 	Maximized=� � WindowStyle$
 None=� +� FocusedElement ��  �� ElementName$ buttonOK��.��(� .��� -   	 �Name$	 grdRoot��
 � RowDefinitions
 �� $	��294 ��� $
��Auto ��� $	��334 �.+�� -   $	 
LayoutRoot��$��1��$	��678��$	��452��.+��� $��Center=�$��Center=�$9��3pack://application:,,,/Resources/WelcomeMessage.jpg��$
��None=��� -    ��Name$ BGRectangle��$��0q�
������  �� RadiusX$ 6�� �� RadiusY$ 6������    BrandedColourIN �� Color# �   �� -    ��Name$ imageInformation��$��44��$��44��$��0,36,0,0q�$��Center=�$	��Top=�$$��Resources\imageInformation.png��$
��Fill=�$y�	Collapsed=��� -   $ imageExclamation��$��44��$��44��$��0,36,0,0q�$��Center=�$	��Top=�$$��Resources\imageExclamation.png��$
��Fill=�$y�	Collapsed=��� -   $ 
imageError��$��44��$��44��$��0,36,0,0q�$��Center=�$	��Top=�$��Resources\imageError.png��$
��Fill=�$y�	Collapsed=��� -    ��Name$ 
txtHeading��$	��400��$��36��$��0,36,0,0q�$��Center=�$	��Top=�$��Comic Sans MS)�$��24(�$
��Bold"�
���   � J� Opacity$ 1Q�$��Welcome�� �� TextAlignment$ Center=�$��NoWrap=��� -	    ��Name$ txtDescription��$	��400��$	��200��$��0q�$��Center=�$��Center=�$��0q�$��Comic Sans MS)�$��18(�
���   �$ 1Q���.   BrandedBrushIN �� SelectionBrush# �  $��This is a sample message�� �� TextAlignment$ Center=� �� TextWrapping$
 Wrap=�$y�Visible=���Z� ��Z� �� -
    ��Name$ btnOK��$��60��$��	0,0,12,12q�$��Right=�$��Bottom=�$��OK�� �� IsCancel .   �� 	IsDefault . $ 1Q�  INFlatButton#���  