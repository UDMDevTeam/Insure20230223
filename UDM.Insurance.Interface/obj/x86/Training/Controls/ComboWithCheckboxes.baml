   M S B A M L   `   `   `  ???? O  KUDM.Insure.Interface, Version=2.5.0.8, Culture=neutral, PublicKeyToken=nullS.clr-namespace:UDM.Insurance.Interface.Controls UDM.Insurance.Interface.Controls  :    4UDM.Insurance.Interface.Controls.ComboWithCheckboxes   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     @local.clr-namespace:UDM.Insurance.Interface.Controls     -     >?Name$  UserControl?? ? 	Resources    NormalBrush ?? 
StartPoint ?? EndPoint ? Color ? Offset  NormalBorderBrush  
GlyphBrush ?? Color  	DarkBrush  PressedBrush  DisabledForegroundBrush  DisabledBackgroundBrush  WindowBackgroundBrush  SelectedBackgroundBrush 	 ComboBoxToggleButton ?? 
TargetType ? ColumnDefinitions	 ??Name
 ?? CornerRadius F?Name ?? Triggers P? Property P? Value ?? 
TargetName ?? Value ?? Property 
 ComboBoxTextBox ?? 
TargetType ?? Padding J? SnapsToDevicePixels??%?  &	        &	 V     &	 ?     &	 ?     &	     &	 ?    &	 ?    &	 ?    &	 ?    &		 ?    7   7  7  7  7  7  7  7  7  7  &	
 ?    '?? ?    7  7  ?? $	 0,0&?$	 0,1&???? ? $
 #FFF??$	 0.0Q?? $
 #CCC??$	 1.0Q??? $	 0,0&?$	 0,1&???? ? $
 #CCC??$	 0.0Q?? $
 #444??$	 1.0Q??? $
 #444???? $	 0,0&?$	 0,1&???? ? $
 #FFF??$	 0.0Q?? $
 #AAA??$	 1.0Q??? $	 0,0&?$	 0,1&???? ? $
 #BBB??$	 0.0Q?? $
 #EEE??$	 0.1Q?? $
 #EEE??$	 0.9Q?? $
 #FFF??$	 1.0Q??? $
 #888???? $
 #EEE???? $
 #FFF???? $
 #DDD???? " d?.R??  ?? ?? $??20 ?.+??? $	 Border??$??2??$
 2??8??  8?? $??1q??? $??0??$
 2,0,0,2??$??1q?8?? 8?? $??0,0,1,0q?F? $ Arrow??$??1??8?? $??Center=?$??Center=?"????        ? 	=  	= ? z     	 P?  ?@J?IsMouseOver . . ??? $ Border??8   ? ??P?  ?@d?	IsChecked$
 trueY?. ??? $ Border??8   ? ??P?  ? {? .  . ??? $ Border??8   ? ???? 8   ? ???? $ Arrow??8 	  ? ???? " ??.R??? $	 PART_ContentHost??|?.  #??z???? " ??.???  ?@J?SnapsToDevicePixels . ??  ?@?OverridesDefaultStyle . ??  ? ?? ?? " ??.R??? $??Border??$ 2q? . ._???  P?  ?@??IsHighlighted . . ??? $ Border??8    ? ??P?  ? {? .  . ??? 8   ? ??.???? -    ??Name$ CheckableCombo?? .  ? OverridesDefaultStyle . $
??Auto=?$
??Auto=???. $ ?? IsSynchronizedWithCurrentItem$
 TrueY?$	??120??$??20??????  ?? ElementName$ UserControl?? ?? Path$ ItemsSource? ? DataContext ?? $ UserControl??$ DataContext????? .R??? -   ???? *Title+ d? 	IsChecked ?? $ 
IsSelected? ?? Mode$ TwoWay=?
 ? Tag ?? *FindAncestor+ ?? AncestorType" ?????? " ??.R?? .+?d? $??ToggleButton??#??[	 $??2??|?.   ?? 	ClickMode$ Press=? ?? $ IsDropDownOpen?$ TwoWay=?  ?? RelativeSource  ?? *TemplatedParent+?? ! ??Name$! 	Presenter??" J? IsHitTestVisible" .  $??3,3,23,3q?$??Center=?$
??Left=????? $??CharacterEllipsis=????? $
 Text?$ TwoWay=?  ?? # ?? Mode$# FindAncestor=?$ UserControlL??? $ ??Name$$ EditableTextBox??#??[
 $
??Left=?$??Center=?$??3,3,23,3q?|?. 
?????? $y?Hidden=?% ?? 
IsReadOnly& ?? 
IsReadOnly#& z% ??Z? ? $??Popup??$??Bottom=?' ?? IsDropDownOpen#??z' ( ? AllowsTransparency( . |?.  $??Slide=?.??? $??DropDown?? . ) ? ActualWidth#??z) * ?? MaxDropDownHeight#??z* .+??? $	 DropDownBorder??#??[ $??1q?  SolidBorderBrush#??[ ?? $??4,6,4,6q? .  ?? .???? + K? IsItemsHost+ . -  'System.Windows.Input.KeyboardNavigation,   DirectionalNavigation$, 	Contained=? P?  ?@??HasItems .  . ??? $ DropDownBorder?? ? ??$ 95??P?  ? {? .  . ??? # [  ? ??P?  ?@??
IsGrouping . . ???  ? ?? .  P? - P? 
SourceName$- Popup?? ?@?AllowsTransparency . . ??? $ DropDownBorder?? ?@??CornerRadius$ 4???? $ DropDownBorder?? ? ??$ 0,2,0,0q?P?  ?@??
IsEditable . . ???  ? ?? .  ?? $ EditableTextBox?? ? y?$ Visible=??? $ 	Presenter?? ? y?$ Hidden=?