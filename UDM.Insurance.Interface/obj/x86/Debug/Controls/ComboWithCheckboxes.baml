   M S B A M L   `   `   `  ????O  KUDM.Insure.Interface, Version=2.6.6.2, Culture=neutral, PublicKeyToken=nullS.clr-namespace:UDM.Insurance.Interface.Controls UDM.Insurance.Interface.Controls  5      :    4UDM.Insurance.Interface.Controls.ComboWithCheckboxes   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      @local.clr-namespace:UDM.Insurance.Interface.Controls     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      -   5          >?Name$  UserControl??5       ? 	Resources 5         NormalBrush ?? 
StartPoint ?? EndPoint ? Offset ? Color  NormalBorderBrush  
GlyphBrush ?? Color  	DarkBrush  PressedBrush  DisabledForegroundBrush  DisabledBackgroundBrush  WindowBackgroundBrush  SelectedBackgroundBrush 	 ComboBoxToggleButton ?? 
TargetType ? ColumnDefinitions	 ??Name
 ?? CornerRadius F?Name ?? Triggers P? Property P? Value ?? 
TargetName ?? Value ?? Property 
 ComboBoxTextBox ?? 
TargetType ?? Padding J? SnapsToDevicePixels??%
  &	        &	 ?     &	 T    &	 r    &	     &	      &	 >    &	 \    &	 z    &		 ?    7   7  7  7  7  7  7  7  7  7  &	
 ?    '?? g    7  7  ?? 5   
   $	 0,0&?62   $	 0,1&?6C   ??5      ? ? 5      $	 0.0Q?6#   $
 #FFF??60   ? 5      $	 1.0Q?6#   $
 #CCC??60   5      5      ?? 5   
   $	 0,0&?68   $	 0,1&?6I   ??5      ? ? 5      $	 0.0Q?6#   $
 #CCC??60   ? 5      $	 1.0Q?6#   $
 #444??60   5      5      ?? 5    
   $
 #444??6-   ?? 5"   
   $	 0,0&?60   $	 0,1&?6A   ??5$      ? ? 5%      $	 0.0Q?6#   $
 #FFF??60   ? 5&      $	 1.0Q?6#   $
 #AAA??60   5'      5)      ?? 5+   
   $	 0,0&?63   $	 0,1&?6D   ??5-      ? ? 5.      $	 0.0Q?6#   $
 #BBB??60   ? 5/      $	 0.1Q?6#   $
 #EEE??60   ? 50      $	 0.9Q?6#   $
 #EEE??60   ? 51      $	 1.0Q?6#   $
 #FFF??60   52      54      ?? 56   
   $
 #888??6:   ?? 58   
   $
 #EEE??6:   ?? 5:   
   $
 #FFF??68   ?? 5<   
   $
 #DDD??6:   ?? 5>   
   " d?67   .R?5?      ?  5@      ?? 5A      ?? 5B      $??20 ?6'   .+?5D      ?? $	 Border??5E      $??2??5F      8??  8?? $??1q?5I      $
 2??5J      ?? 5K      $??0??5L      $??1q?5M      8?? 8?? $??0,0,1,0q?5P      $
 2,0,0,2??5Q      F? 5R      $ Arrow??5S      $??1??5T      $??Center=?5U      $??Center=?5V      "????        ? 	=  	= ? z     	8?? 5X      5Y       5Z      P? 5[       ?@J?IsMouseOver . . ?5\      ?? $ Border??6   8   ? ??6G   5]      P? 5^       ?@d?	IsChecked$
 trueY?6<   . ?5_      ?? $ Border??6   8   ? ??6G   5`      P? 5a       ? {? .  . ?5b      ?? $ Border??6   8   ? ??6G   ?? 5d      8   ? ??63   ?? 5e      $ Arrow??6   8 	  ? ??6@   5f      5h      ?? 5j   
   " ??62   .R?5k      ?? $	 PART_ContentHost??5l      #??z??5m      |?.  5n      5o      ?? 5q   
   " ??6.   .?5r      ??  ?@J?SnapsToDevicePixels . 64   ?? 5s       ?@?OverridesDefaultStyle . 66   ?? 5t       ? ?? 5u      ?? 5v      " ??6&   .R?5w      ?? $??Border??5x      $ 2q?5y       . ._?5{      ?? 5|       5}      P? 5~       ?@??IsHighlighted . . ?5   "   ?? $ Border??6)   8    ? ??6S   5?      P? 5?       ? {? .  . ?5?   "   ?? 8   ? ??6?   5?      5?      5?      5?      5?      .??5?      ?? -   5         ??Name$ CheckableCombo??5?   	   $	??120??5?   	   $??20??5?   	   $ ?? IsSynchronizedWithCurrentItem$
 TrueY?5?   	    ? OverridesDefaultStyle . ??. $
??Auto=?5?   	   $
??Auto=?5?   	    .  ? DataContext 5?   	   ??  ?? ElementName$ UserControl?? ?? Path$ DataContext???5?   	   ?? $ UserControl??$ ItemsSource???5?   
   ?? 5?      .R?5?      ?? -   5        ??5?      ?? *Title+ d? 	IsChecked 5?      ?? $ 
IsSelected? ?? Mode$ TwoWay=?
 ? Tag 5?      ?? *FindAncestor+ ?? AncestorType" ??5?      ??5?   
   ?? 5?      " ??6   .R?5?      ? .+?5?      d? $??ToggleButton??5?      $??2??5?       ?? 	ClickMode$ Press=?5?      |?.  #??[	 5?       5?      ?? $ IsDropDownOpen?$ TwoWay=?  ?? RelativeSource  ?? *TemplatedParent+5?      ?? 5?      ! ??Name$! 	Presenter??5?      $??3,3,23,3q?5?      $
??Left=?5?      $??Center=?5?      " J? IsHitTestVisible" .  ??5?      ?? 5?      $??CharacterEllipsis=?6?   ??6(   ?? $
 Text?$ TwoWay=?  ?? # ?? Mode$# FindAncestor=?$ UserControlL?6?   5?      ?? 5?      $ ??Name$$ EditableTextBox??5?      $??3,3,23,3q?5?      $
??Left=?5?      $??Center=?5?      
?????? |?. % ?? 
IsReadOnly& ?? 
IsReadOnly#& z% 5?      #??[
 5?      $y?Hidden=?5?      ??5?      Z? 5?      ? 5?      $??Popup??5?      ' ? AllowsTransparency' . |?.  ( ?? IsDropDownOpen#??z( 5?      $??Bottom=?5?      $??Slide=?5?      .??5?      ? $??DropDown??5?      ) ? ActualWidth#??z) 5?      * ?? MaxDropDownHeight#??z* 5?       . .+?5?      ?? $	 DropDownBorder??5?   !   #??[ 5?   !     SolidBorderBrush#??[ 5?   !   $??1q?5?   !   ?? 5?      $??4,6,4,6q?5?   !    .  5?   !   ?? .??5?   "   ?? + K? IsItemsHost+ . -  'System.Windows.Input.KeyboardNavigation,   DirectionalNavigation$, 	Contained=?6@   5?      5?      5?      5?       5?      P? 5?       ?@??HasItems .  . ?5?      ?? $ DropDownBorder??6!    ? ??$ 95??6R   5?      P? 5?       ? {? .  . ?5?      ?? # [ 67    ? ??5?      P? 5?       ?@??
IsGrouping . . ?5?      ??  ? ?? .  6J   5?      P? 5?      - P? 
SourceName$- Popup??6    ?@?AllowsTransparency . . ?5?      ?? $ DropDownBorder??6!    ?@??CornerRadius$ 4??6U   ?? 5?      $ DropDownBorder??6!    ? ??$ 0,2,0,0q?6O   5?      P? 5?       ?@??
IsEditable . . ?5?      ??  ? ?? .  66   ?? 5?      $ EditableTextBox??6!    ? y?$ Visible=?6T   ?? 5?      $ 	Presenter??6!    ? y?$ Hidden=?6N   5?      5?      5?      5?      