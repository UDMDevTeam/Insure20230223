   M S B A M L   `   `   `  ????G  CEmbriant.WPF, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullS9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPFEmbriant.WPF.Controls  5       UDM.Insure.Interface>   8UDM.Insurance.Interface.Screens.SelectLeadCampaignScreen   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      s oInfragisticsWPF4.DataPresenter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb0igDP%http://infragistics.com/DataPresenter  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5      Csrc9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPF  5      -   5        $	??600??5	      $	??400??5
      .??5      ? -   5          ?Name$  
LayoutRoot??5      ??6   Z?  ? ColumnDefinitions 5   
   ?? 5      $
??0.1* ?6   ?? 5      $
??0.1* ?6   ?? 5      $
??0.1* ?6   ?? 5      $
??0.1* ?6   ?? 5      $
??0.1* ?6   ?? 5      $
??0.1* ?6   ?? 5      $
??0.1* ?6   ?? 5      $
??0.1* ?6   ?? 5      $
??0.1* ?6   ?? 5      $
??0.1* ?6   ?? 5      $
??0.1* ?6   ?? 5      $
??0.1* ?6   ?? 5      $
??0.1* ?6   ?? 5      $
??0.1* ?6   ?? 5      $
??0.1* ?6   ?? 5      $
??0.1* ?6   ?? 5       $
??0.1* ?6   ?? 5!      $
??0.1* ?6    ? RowDefinitions 5$   
   ?? 5%      $
??0.1* ?6   ?? 5&      $
??0.1* ?6   ?? 5'      $
??0.1* ?6   ?? 5(      $
??0.1* ?6   ?? 5)      $
??0.1* ?6   ?? 5*      $
??0.1* ?6   ?? 5+      $
??0.1* ?6   ?? 5,      $
??0.1* ?6   ?? 5-      $
??0.1* ?6   ?? 5.      $
??0.1* ?6   ?? 5/      $
??0.1* ?6   ?? 50      $
??0.1* ?6   .+?53   
   ?? -   5         ??Name$ BGRectangle??54      $??12??55      $??18??56      $??0q?57      
???   ? ?? RadiusX$ 12??59       ?? RadiusY$ 12??5:      $??2??5;      ??5<      ?? 5=         BrandedColourIN ?? Color# ?   6"   5?      ?? 5A   
   -   5         ??Name$ btnClose??5B      $??0??5C      $??2??5D      $??16??5E      $??2??5F        INCloseButton#???  5H      $??0??5I      ?? 5K   
   -   5         ??Name$ headerSelectLeadCampaign??5L      $??1??5M      $??1??5N      $??6??5O      $	??Top=?5P        INPageHeadingText1#???  5Q      $??Select Campaign??5R      4  .Infragistics.Windows.DataPresenter.XamDataGrid  5T   
   -   5        	  Name$	 xdgSelectLeadCampaign??5U      $??2??5V      $??8??5W      $??1??5X      $??16??5Y      $??0,0,0,15q?5Z      $??Stretch=?5[      $??Stretch=?5\      :  4Infragistics.Windows.DataPresenter.DataPresenterBase
   AutoFite	 aInfragisticsWPF4.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb? 	 ?Infragistics.Windows.Helpers.NullableConverter`1[[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]$
 False 5]         GroupByAreaLocation$
 None=?5^        INXamDataGridStyle1#???  5_      $??0??5`         FieldLayouts 5a      4  .Infragistics.Windows.DataPresenter.FieldLayout  5b         Fields. 5c      .  (Infragistics.Windows.DataPresenter.Field     Name$ ImportID??5d         Label$ ImportID??5e         
Visibility$ 	Collapsed=?5f        5g      $ 
CampaignID??5h      $ 
CampaignID??5i      $ 	Collapsed=?5j        5k      $ CampaignCode??5l      $ CampaignCode??5m      $ 	Collapsed=?5n        5o      $ CampaignName??5p         Width$	 200X?5q      $ Campaign Name??5r        5s      $ BatchID??5t      $ BatchID??5u      $ 	Collapsed=?5v        5w      $ 	BatchCode??5x      $	 100X?5y      $ 
Batch Code??5z        5{      $ LeadID??5|      $ LeadID??5}      $ 	Collapsed=?5~        5      $ LeadName??5?      $	 200X?5?      $ 	Lead Name??5?      5?      5?      ?? 5?   
   -   5        $ 	btnSelect??5?      $??10??5?      $??12??5?      $??5??5?      $??0q?5?      $??Right=?5?      $??Bottom=?5?      $??Select??5?       ?? 	IsDefault .   INFlatButton#???  5?      $??0??5?      5?      5?      