   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089x tInfragisticsWPF4.Controls.Schedules.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb� �InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb�	 ~InfragisticsWPF4.Controls.Editors.XamCalendar.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb|
 xInfragisticsWPF4.Controls.Menus.XamMenu.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe aInfragisticsWPF4.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbu qInfragisticsWPF4.Documents.Excel.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbs oInfragisticsWPF4.DataPresenter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb� }InfragisticsWPF4.DataPresenter.ExcelExporter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbm iInfragisticsWPF4.Editors.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb 
  Freeze  WorkDayForeground  �� Color  NonWorkDayForeground  RadialGradientBrush1 
� Center 
� GradientOrigin 
� RadiusX 
� RadiusY � Offset � Color  BrandedColourIN_mediumlight:   4Infragistics.Controls.Primitives.ValueConverterGroup  boolToFontBold    
Converters:  4Infragistics.Controls.Primitives.FixedValueConverter   SourceValue	   DestinationValue  visibilityTo52Converter  System.Windows.Visibility  INXamMonthCalendarStyle1
 �� 
TargetType3  -Infragistics.Windows.Editors.XamMonthCalendar �� Property �� Value1  +Infragistics.Controls.Primitives.XamlHelper�  �Infragistics.Windows.Helpers.NullableConverter`1[[System.DayOfWeek, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]�  �Infragistics.Windows.Helpers.NullableConverter`1[[System.DateTime, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]  CalendarDayStyle.  (Infragistics.Windows.Editors.CalendarDay �� 
TargetType'  !System.Windows.VisualStateManager   VisualStateGroups%	  System.Windows.VisualStateGroup 	  States 
  System.Windows.VisualState 
  
Storyboard �� 
TargetName �� TargetProperty R� KeyTime R� Value ��Name 	 BrandedBrushIN_dark �� CornerRadius ��Name   SnapsToDevicePixels 
 BrandedBrushIN_mediumdark �� Triggers P� Property P� Value �� 
TargetName/  )Infragistics.Windows.Editors.CalendarItem  INXamMonthCalendarStyle2 �� Padding �Name � RowDefinitions9  3Infragistics.Windows.Editors.CalendarItemGroupPanel  BrandedBrushIN  INXamMonthCalendarStyle3�� H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      :x,http://schemas.microsoft.com/winfx/2006/xaml     YPresentationOptionsAhttp://schemas.microsoft.com/winfx/2006/xaml/presentation/options  DigPrim/http://schemas.infragistics.com/xaml/primitives   	 
  <igWPF(http://schemas.infragistics.com/xaml/wpf      .sys&clr-namespace:System;assembly=mscorlib  %�  &	       &	      &	 8     7  &	 �     &	 �     &	 A    &	 �    7  7 	 7  7 	 7 
 &	 �	    7  &	     7  �� 4true  $  Black���� 4true  $  	LightGray��
� $ 0.5,0.5&�$ 0.5,0.5&�$	 0.5Q�$	 0.5Q���� $ 0Q�$ White��� $	 0.5Q�$ White��� $	 1.0Q�8      .    ��True��	 #�Bold"�   .     Visible=�	 ��52��  	 Z�   	Collapsed=��� "
  .���  � ��$ Stretch=���  � ��$ Stretch=���  �@ 	Focusable . ��  � ��$ 14.667(���  � ��$ Segoe UI)���  �@ TodayButtonVisibility$ 	Collapsed=���  �@ FirstDayOfWeek$ Sunday ��  �@ SelectionType$ Single=���  �@ ReferenceDate$ 
2013-01-01 �� "
  .���  �@�OverridesDefaultStyle .  ��  � �� .  ��  � ��$ 30����  � ��$ 36����  � ��$ Stretch=���  � ��$ Stretch=���  � ��$ Center=���  � ��$ Center=���  � ��$ 1q���  � ��$ 4,1,4,1q���  � ��$ 14.667(���  � ��$ Segoe UI)���  � ��
 �   ���  � �� �� "  .R��  	  WorkDayStates��. 
  WorkDay��. �� .��T� $ 	textBlock��$ 
Foreground�..�i� $ 0��8   T� $ 	textBlock��$ (TextBlock.FontWeight)�..�i� $ 0�� #�
Normal"�	  CommonStates��. 
  
Normal��
  	MouseOver��. �� .��T� $ MouseOverItem��$ (UIElement.Visibility)�..�i� $ 0��  Visible=�	  SelectionStates��. 
  Selected��. �� .��T� $ SelectedDate��$ (UIElement.Visibility)�..�i� $ 0��  Visible=�
  
Unselected��. �� .��T� $ SelectedDate��$ (UIElement.Visibility)�..�i� $ 0��  	Collapsed=�.+��� $ MouseOverItem��
������ 8�� #��z��$ 24��$y�	Collapsed=��� $ 	textBlock��#��z��#��z��
���   �#��z���� $ SelectedDate��#��z��#��z��8�� 8�� #��z��$ 24��$y�	Collapsed=� . ._��� $ textBlockSelectedDate��#��z��#��z��$
��Bold"�8�� #��z���� $ Weekend��#��z��#��z��#��z��$y�	Collapsed=� . ._��� $ textBlockWeekend��#��z��#��z��$
��Bold"�
���   �#��z���� $ OtherDay��#��z��#��z��#��z��$y�	Collapsed=� . ._��� $ textBlockOtherDay��#��z��#��z��$��Normal"�
�������#��z�� P�  �@ 	IsWorkday .  . ��� $ Weekend�� � y�$ Visible=�P�  �@ IsLeadingOrTrailingItem . . ��� $ 	textBlock�� � y�$ 	Collapsed=��� $ Weekend�� � y�$ 	Collapsed=��� $ OtherDay�� � y�$ Visible=��� "
  .���  � ��$ Stretch=���  � ��$ Stretch=���  �@ 	Focusable . ��  � ��$ 14.667(���  � ��$ Segoe UI)���  �@ TodayButtonVisibility$ 	Collapsed=���  �@ FirstDayOfWeek$ Sunday ��  �@ SelectionType$ Single=���  �@ CalendarDayStyle8   ��  � �� �� "  .R��� $ 
mainBorder��
�������
�������$��2q�$ 4��$ 6q� . ._�� $ 	RootPanel��# z  �� $��* ��� $
��Auto �.+�  #��z��#��z�� P�  �@J�IsKeyboardFocusWithin . . ��� $ 
mainBorder��# �   � ���� "
  .���  � ��$ Stretch=���  � ��$ Stretch=���  �@ 	Focusable . ��  � ��$ 14.667(���  � ��$ Segoe UI)���  �@ TodayButtonVisibility$ 	Collapsed=���  �@ FirstDayOfWeek$ Sunday ��  �@ SelectionType$ Single=���  �@ CalendarDayStyle8   ��  � �� �� "  .R��� $ 
mainBorder��
�������
�������$��0q�$ 4��$ 6q� . ._�� $ 	RootPanel��# z  �� $��* ��� $
��Auto �.+�  #��z��#��z�� P�  �@J�IsKeyboardFocusWithin . . ��� $ 
mainBorder��# �   � ��