   M S B A M L   `   `   `  ????G  CEmbriant.WPF, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullQ8clr-namespace:Embriant.WPF.Windows;assembly=Embriant.WPFEmbriant.WPF.Windows  5       UDM.Insure.Interface9   3UDM.Insurance.Interface.Windows.INMessageBoxWindow2   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      Hsrc8clr-namespace:Embriant.WPF.Windows;assembly=Embriant.WPF     5      -   5        /   )Embriant.WPF.Windows.BaseMessageBoxWindow   Name$  Window??5       ? AllowsTransparency . 
??????  ? 
ResizeMode$ NoResize=?5       ? ShowInTaskbar .   ? WindowStartupLocation$ CenterScreen=?5       ? WindowState$ 	Maximized=?5       ? WindowStyle$
 None=?5       +? FocusedElement 5      ??  ?? ElementName$ buttonOK??.??5      (? .??5   
   ? -   5        	 ?Name$	 grdRoot??5      
 ? RowDefinitions
 5      ?? 5      $	??347 ?5      5      ?? 5      $
??Auto ?5      5      ?? 5      $	??388 ?5      5       .+?5"      ? -   5        $	 
LayoutRoot??5#      $??1??5$      $	??660??5%      $	??345??5&      .+?5'      ?? -   5         ??Name$ BGRectangle??5(      $??0q?5)      ???	#FF060000 ?? RadiusX$ 6??5+       ?? RadiusY$ 6??5,      ??5-      ?? 5.         BrandedColourIN ?? Color# ?   5/      51      ?? 52      -   5         ??Name$ imageInformation??53      $??44??54      $??44??55      $??0,36,0,0q?56      $??Center=?57      $	??Top=?58      $$??Resources\imageInformation.png??59      $
??Fill=?5:      $y?	Collapsed=?5;      ?? 5<      -   5        $ imageExclamation??5=      $??44??5>      $??44??5?      $??0,36,0,0q?5@      $??Center=?5A      $	??Top=?5B      $$??Resources\imageExclamation.png??5C      $
??Fill=?5D      $y?	Collapsed=?5E      ?? 5F      -   5        $ 
imageError??5G      $??44??5H      $??44??5I      $??0,36,0,0q?5J      $??Center=?5K      $	??Top=?5L      $??Resources\imageError.png??5M      $
??Fill=?5N      $y?	Collapsed=?5O      ?? 5P      -   5         ??Name$ 
txtHeading??5Q      $	??300??5R      $??30??5S      $??	0,104,0,0q?5T      $??Center=?5U      $	??Top=?5V      $??21.333(?5W      $
??Bold"?5X      
??????? ?? TextAlignment$ Center=?5Z      $
??Wrap=?5[      ?? 6*    ? Language$ en-za?5\      $?Error??5]      5^      ?? 5_      -	   5         ??Name$ txtDescription??5`      $	??400??5a      $??94??5b      $??	0,150,0,0q?5c      $??Center=?5d      $	??Top=?5e      $??0q?5h      $??16(?5i      
?????????.  ??.   BrandedBrushHR ?? SelectionBrush# ?  5m      $??Error Message??5n       ?? TextAlignment$ Center=?5o       ?? TextWrapping$
 Wrap=?5p      ??5f      Z? ??5g      Z? 5p      ?? 5q      -
   5         ??Name$ buttonOK??5r      $??
150,0,0,36q?5s      $
??Left=?5t      $??Bottom=?5u      $??OK??5w       ?? IsCancel .   ?? 	IsDefault .   INFlatButton#???  5{      ?? 5|      -   5        $ buttonCancel??5}      $??
0,0,150,36q?5~      $??Right=?5      $??Bottom=?5?      $??Cancel??5?       .  .  #???  5?      5?      5?      5?      5?      