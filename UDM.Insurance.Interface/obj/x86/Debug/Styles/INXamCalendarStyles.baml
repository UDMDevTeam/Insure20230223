   M S B A M L   `   `   `  ????O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089x tInfragisticsWPF4.Controls.Schedules.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb? ?InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb?	 ~InfragisticsWPF4.Controls.Editors.XamCalendar.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb|
 xInfragisticsWPF4.Controls.Menus.XamMenu.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe aInfragisticsWPF4.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbu qInfragisticsWPF4.Documents.Excel.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbs oInfragisticsWPF4.DataPresenter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb? }InfragisticsWPF4.DataPresenter.ExcelExporter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbm iInfragisticsWPF4.Editors.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb 
  Freeze  WorkDayForeground  ?? Color  NonWorkDayForeground  RadialGradientBrush1 
? Center 
? GradientOrigin 
? RadiusX 
? RadiusY ? Offset ? Color  BrandedColourIN_mediumlight:   4Infragistics.Controls.Primitives.ValueConverterGroup  boolToFontBold    
Converters:  4Infragistics.Controls.Primitives.FixedValueConverter   SourceValue	   DestinationValue  visibilityTo52Converter  System.Windows.Visibility  INXamMonthCalendarStyle1
 ?? 
TargetType3  -Infragistics.Windows.Editors.XamMonthCalendar ?? Property ?? Value1  +Infragistics.Controls.Primitives.XamlHelper?  ?Infragistics.Windows.Helpers.NullableConverter`1[[System.DayOfWeek, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]?  ?Infragistics.Windows.Helpers.NullableConverter`1[[System.DateTime, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]  CalendarDayStyle.  (Infragistics.Windows.Editors.CalendarDay ?? 
TargetType'  !System.Windows.VisualStateManager   VisualStateGroups%	  System.Windows.VisualStateGroup 	  States 
  System.Windows.VisualState 
  
Storyboard ?? 
TargetName ?? TargetProperty R? KeyTime R? Value ??Name 	 BrandedBrushIN_dark ?? CornerRadius ??Name   SnapsToDevicePixels 
 BrandedBrushIN_mediumdark ?? Triggers P? Property P? Value ?? 
TargetName/  )Infragistics.Windows.Editors.CalendarItem  INXamMonthCalendarStyle2 ?? Padding ?Name ? RowDefinitions9  3Infragistics.Windows.Editors.CalendarItemGroupPanel  BrandedBrushIN  INXamMonthCalendarStyle3?? 5      H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      :x,http://schemas.microsoft.com/winfx/2006/xaml     5      YPresentationOptionsAhttp://schemas.microsoft.com/winfx/2006/xaml/presentation/options  5      DigPrim/http://schemas.infragistics.com/xaml/primitives   	 
  5      <igWPF(http://schemas.infragistics.com/xaml/wpf      5      .sys&clr-namespace:System;assembly=mscorlib  5      %?  &	       &	 ,     &	 \     7  &	 <    &	 ?    &	 ?    &	 z    7  7 	 7  7 	 7 
 &	 y    7  &	 k    7  ?? 5	      4true  5   	   $  Black???? 5      4true  5   	   $  	LightGray??
? 5(      $ 0.5,0.5&?67   $ 0.5,0.5&?6H   $	 0.5Q?6a   $	 0.5Q?6o   ??5*      ? $ 0Q?6   $ White??6&   ? 5+      $	 0.5Q?6   $ White??6(   ? 5,      $	 1.0Q?6   8   6(   5.         50      . 52   
      53      ??54      True??6$   	 56      #?57      Bold"?6#   59      5:         5<      . 5>   
      5?       5@      Visible=?6&   	 5B      ??5C      52??6    5E        5G   
   	 5H      Z?  5I       5J      	Collapsed=?6(   5L      5N      ?? 5P      "
  5R   	   .?5S   
   ??  ? ??$ Stretch=?67   ?? 5T   
    ? ??$ Stretch=?65   ?? 5U   
    ?@ 	Focusable . 68   ?? 5V   
    ? ??$ 14.667(?6%   ?? 5W   
    ? ??$ Segoe UI)?6'   ?? 5X   
    ?@ TodayButtonVisibility$ 	Collapsed=?62   ?? 5Y   
    ?@ FirstDayOfWeek$ Sunday 6+   ?? 5Z   
    ?@ SelectionType$ Single=?6*   ?? 5[   
    ?@ ReferenceDate$ 
2013-01-01 6*   5\      ?? 5^      "
  5`   	   .?5a   
   ??  ?@?OverridesDefaultStyle .  62   ?? 5b   
    ? ?? .  6&   ?? 5c   
    ? ??$ 30??6&   ?? 5d   
    ? ??$ 36??6%   ?? 5e   
    ? ??$ Stretch=?60   ?? 5f   
    ? ??$ Stretch=?6.   ?? 5g   
    ? ??$ Center=?67   ?? 5h   
    ? ??$ Center=?65   ?? 5i   
    ? ??$ 1q?6,   ?? 5j   
    ? ??$ 4,1,4,1q?6$   ?? 5k   
    ? ??$ 14.667(?6%   ?? 5l   
    ? ??$ Segoe UI)?6'   ?? 5m   
    ? ??
 ?   ?6'   ?? 5o   
    ? ?? 5p      ?? 5q      "  5r      .R?5s      ?  5t      	  5u      WorkDayStates??. 5w   "   
  WorkDay??. 5y   &   ?? .??5z   *   T? $ 	textBlock??5{   -   $ 
Foreground?5|   -   ..?5}   .   i? $ 0??5~   1   8   5   1   5?   +   T? 5?   *   $ 	textBlock??5?   -   $ (TextBlock.FontWeight)?5?   -   ..?5?   .   i? $ 0??5?   1    5?   2   #?5?   6   
Normal"?6I   5?   /   5?   +   5?   '   5?   #   5?      	  5?      CommonStates??. 5?   "   
  
Normal??5?   %   
  5?   "   	MouseOver??. 5?   &   ?? .??5?   *   T? $ MouseOverItem??5?   -   $ (UIElement.Visibility)?5?   -   ..?5?   .   i? $ 0??5?   1    5?   2    5?   6   Visible=?6J   5?   /   5?   +   5?   '   5?   #   5?      	  5     SelectionStates??. 5  "   
  Selected??. 5  &   ?? .??5  *   T? $ SelectedDate??5  -   $ (UIElement.Visibility)?5  -   ..?5  .   i? $ 0??5  1    5  2    5  6   Visible=?6J   5  /   5  +   52  '   53  #   
  5A  "   
Unselected??. 5C  &   ?? .??5D  *   T? $ SelectedDate??5E  -   $ (UIElement.Visibility)?5F  -   ..?5G  .   i? $ 0??5H  1    5I  2    5J  6   	Collapsed=?6L   5L  /   5M  +   5W  '   5X  #   5Y     .+?5c     ?? $ MouseOverItem??5d     
?????? 8?? #??z??5g     $ 24??5h     $y?	Collapsed=?5i     ?? 5k     $ 	textBlock??5l     #??z??5m     #??z??5n     
???   ?#??z??5p     ?? 5t     $ SelectedDate??5u     #??z??5v     #??z??5w     8?? 8?? #??z??5z     $ 24??5{     $y?	Collapsed=?5|      . ._?5~     ?? $ textBlockSelectedDate??5  !   #??z??5?  !   #??z??5?  !   $
??Bold"?5?  !   8?? #??z??5?  !   5?     ?? 5?     $ Weekend??5?     #??z??5?     #??z??5?     #??z??5?     $y?	Collapsed=?5?      . ._?5?     ?? $ textBlockWeekend??5?  !   #??z??5?  !   #??z??5?  !   $
??Bold"?5?  !   
???   ?#??z??5?  !   5?     ?? 5?     $ OtherDay??5?     #??z??5?     #??z??5?     #??z??5?     $y?	Collapsed=?5?      . ._?5?     ?? $ textBlockOtherDay??5?  !   #??z??5?  !   #??z??5?  !   $??Normal"?5?  !   
???????#??z??5?  !   5?     5?      5?     P? 5?      ?@ 	IsWorkday .  . ?5?     ?? $ Weekend??6%    ? y?$ Visible=?6P   5?     P? 5?      ?@ IsLeadingOrTrailingItem . . ?5?     ?? $ 	textBlock??6%    ? y?$ 	Collapsed=?6R   ?? 5?     $ Weekend??6%    ? y?$ 	Collapsed=?6P   ?? 5?     $ OtherDay??6%    ? y?$ Visible=?6Q   5?     5?     5?     5?     ?? 5?     "
  5?  	   .?5?  
   ??  ? ??$ Stretch=?67   ?? 5?  
    ? ??$ Stretch=?65   ?? 5?  
    ?@ 	Focusable . 68   ?? 5?  
    ? ??$ 14.667(?6%   ?? 5?  
    ? ??$ Segoe UI)?6'   ?? 5?  
    ?@ TodayButtonVisibility$ 	Collapsed=?62   ?? 5?  
    ?@ FirstDayOfWeek$ Sunday 6+   ?? 5?  
    ?@ SelectionType$ Single=?6*   ?? 5?  
    ?@ CalendarDayStyle8   6-   ?? 5?  
    ? ?? 5?     ?? 5?     "  5?     .R?5?     ?? $ 
mainBorder??5?     
???????
???????$??2q?5?     $ 4??5?     $ 6q?5?      . ._?5?     ? $ 	RootPanel??5?     # z 5?      5?     ?? 5?  "   $??* ?5?  %   ?? 5?  "   $
??Auto ?5?  %   .+?5?       #??z??5?  !   #??z??5?  !   5?     5?      5?     P? 5?      ?@J?IsKeyboardFocusWithin . . ?5?     ?? $ 
mainBorder??6%   # ?  6T    ? ??5?     5?     5?     5?     ?? 5?     "
  5?  	   .?5?  
   ??  ? ??$ Stretch=?67   ?? 5?  
    ? ??$ Stretch=?65   ?? 5   
    ?@ 	Focusable . 68   ?? 5  
    ? ??$ 14.667(?6%   ?? 5  
    ? ??$ Segoe UI)?6'   ?? 5  
    ?@ TodayButtonVisibility$ 	Collapsed=?62   ?? 5  
    ?@ FirstDayOfWeek$ Sunday 6+   ?? 5  
    ?@ SelectionType$ Single=?6*   ?? 5  
    ?@ CalendarDayStyle8   6-   ?? 5
  
    ? ?? 5     ?? 5     "  5     .R?5     ?? $ 
mainBorder??5     
???????
???????$??0q?5     $ 4??5     $ 6q?5      . ._?5     ? $ 	RootPanel??5     # z 5      5     ?? 5  "   $??* ?5  %   ?? 5  "   $
??Auto ?5  %   .+?5       #??z??5   !   #??z??5!  !   5"     5#      5$     P? 5%      ?@J?IsKeyboardFocusWithin . . ?5&     ?? $ 
mainBorder??6%   # ?  6T    ? ??5'     5)     5+     5-     5/     