   M S B A M L   `   `   `  ????O  KUDM.Insure.Interface, Version=2.6.6.2, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:UDM.Insurance.Interface.ContentUDM.Insurance.Interface.Content  5      G CEmbriant.WPF, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullS9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPFEmbriant.WPF.Controls 5      E    ?UDM.Insurance.Interface.Screens.SelectCMAgentToAssignTSRSavedCF   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Acontent-clr-namespace:UDM.Insurance.Interface.Content     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      Isrc9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPF     5      $??0q?5
        ? AllowsTransparency  .  ? 
ResizeMode$ CanMinimize=?5       ? ShowInTaskbar .   ? WindowStartupLocation$ CenterScreen=?5       ? WindowState$ Normal=?5       ? WindowStyle$
 None=?5       ? Topmost . ??5	      Z?  ? 	Resources 5         StyleSelectButton  INFlatButton ?? BasedOn	 ?? 
TargetType
 ?? Property ?? Value ?? Triggers ?? Binding ?? ElementName ?? Path ?? Value??%?  &	        7  ?? 5   
   8   "	 ??5      .?5      ?? 
 ?@^?ShowOnDisabled . 5      ?? 5      
 ?@?ToolTip 6(   Z? 5        5!      ?? 5"       5#      ?? $ cmbCarriedForwardReason??$ SelectedValue? 5%      Z? .a?5&      ?? 
 ? {? .  62   ?? 5'      
 ?@?ToolTip$) #Carried Forward Reason not Selected??60   5(      5*      5,      .??5.      (?  (? Stretch$ Uniform=?5/   	   .??50   
   ? $
??1920??51      $
??1080??52      .+?53      ? -   5         ?Name$
 Size??54      $	??515??55      $	??318??56      .+?57      (? $
 Fill=?58      .??59      ? -   5        $ 
LayoutRoot??5:      $	??515??5;      $	??318??5<      .+?5>      ?? -   5         ??Name$ BGRectangle??5?      $??Stretch=?5@      
???   ? ?? RadiusX$ 12??5B       ?? RadiusY$ 12??5C        BrandedBrushIN#??[ 5D      ?? 5F      -   5         ??Name$ 	btnSelect??5G      $??168,0,169,40q?5H      $??Center=?5I      $??Bottom=?5J      $
??Save??5L       ?? 	IsDefault . #??[  5N      $??5??5O      {?. 6*   6   0UDM.Insurance.Interface.Content.LacePopupWindow1  5Q      $??2,6,0,0q?5R      $	??Top=?5S      ?? 5U      -   5         ??Name$% headingSelectCancellationReason??5V      $??	81,49,0,0q?5W      $	??Top=?5X        INPageHeadingText1#??[ 5Y      $??CM Agent To Assign??5Z      F? 5\      $??1??5]      $??81.449,73.241,238,0q?5^      $	??Top=?5_      ???? E, )??s?]???@ )?	 ?? Stretch$
 Fill=?5a      
???????5b      ,  &Embriant.WPF.Controls.EmbriantComboBox  5d      -   5          Name$ 
cmbCMAgent??5e      $??Center=?5f      $	??290??5h       	 cmbIN#??[ 5j      $??0??5k      6*   ? 5m      $??Right=?5n      $	??Top=?5o      .+?5p      ?? -   5        $ btnClose??5q   !   $??18??5r   !   $??18??5s   !     INCloseButton#??[ 5u   !   5v      5x      5y      5z      5{      5|      5~      