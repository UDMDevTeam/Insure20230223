   M S B A M L   `   `   `  ????B  >UDM.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullH1clr-namespace:UDM.WPF.Converters;assembly=UDM.WPFUDM.WPF.Converters  5   	    UDM.Insure.Interface6   0UDM.Insurance.Interface.Screens.LiveProductivity   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ~ zInfragisticsWPF4.Controls.Charts.XamGauge.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbx	 tInfragisticsWPF4.Controls.Schedules.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb?
 ?InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb? ~InfragisticsWPF4.Controls.Editors.XamCalendar.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb| xInfragisticsWPF4.Controls.Menus.XamMenu.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe aInfragisticsWPF4.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb7ig$http://schemas.infragistics.com/xaml  	 
    5   	   L
converters1clr-namespace:UDM.WPF.Converters;assembly=UDM.WPF  	 
    5   	   -   5          ? Title$  LiveProductivity??5   	   $	??300??6"   $	??220??6/   
??????  ? WindowStyle$
 None=?5   	    ? Topmost . $??0q?5   	    ? Left$ 0??5   	    ? AllowsTransparency . {?. 
 ? Top 5   	   ??  ?? Source# Zo? ?? Path$ Height? ?? 	Converter &    UDM.WPF.Converters.MathConverter  	 ?? ConverterParameter$	 x*0.4??.??5      ? 
 ? RowDefinitions
 5   
   ?? 5      ?? 5      ?? 5      ?? 5      ?? 5      ?? 5      ?? 5      ?? 5      ?? 5      .+?5   
   ?? -   5         ??Name$ tbLiveProductivityLabel??5      $??0??5       $??0q?5!      $??Center=?5"      $	??Top=?5#         INPageHeadingText1#???   5$      $??Live Productivity??5%      $y?Visible=?5&      1  +Infragistics.Controls.Charts.XamRadialGauge  5(   
   -   5          Name$ xrgLiveProductivity??5(      $??1??69   $??6??6F   $??5??6W   $??0q?6k   $??Center=?6w   $??Bottom=?6?   $y?Visible=?6?      Scales 5*      3  -Infragistics.Controls.Charts.RadialGaugeScale     LabelGroups 5,      8  2Infragistics.Controls.Charts.RadialGaugeLabelGroup     Extent$
 0.65Q?63      Interval$ 10Q?6A      Orientation$ Outward=?6O      PreTerminal$ 0Q?6e   $??14(?6u      Needles 5/      4  .Infragistics.Controls.Charts.RadialGaugeNeedle     	EndExtent$
 0.65Q?6/      InverseBrushRotation .    Value$ 0Q?6\      Ranges 52      3  -Infragistics.Controls.Charts.RadialGaugeRange     
StartValue$ 0Q?6.      EndValue$ 20Q?6=      InnerExtentStart$
 0.57Q?6K      InnerExtentEnd$
 0.60Q?6c      OuterExtent$
 0.55Q?6y      Fill ?#CD00006?     53      $ 20Q?6.   $ 40Q?6>   $
 0.60Q?6L   $
 0.63Q?6d   $
 0.55Q?6z    ?#FF7F006?     54      $ 40Q?6.   $ 60Q?6>   $
 0.63Q?6L   $
 0.66Q?6d   $
 0.55Q?6z    ?#FFD7006?     55      $ 60Q?6.   $ 80Q?6>   $
 0.66Q?6L   $
 0.69Q?6d   $
 0.55Q?6z    ?#66CD006?     56      $ 80Q?6.   $	 100Q?6>   $
 0.69Q?6M   $
 0.72Q?6e   $
 0.55Q?6{    ?#00B2EE6?      TickMarkGroups 59      ;  5Infragistics.Controls.Charts.RadialGaugeTickMarkGroup     	EndExtent$	 0.6Q?66       Interval$  10Q?6F   !   StartExtent$	! 0.5Q?6T     5:      $	 0.6Q?66   $  5Q?6F   $	! 0.5Q?6S   5<      5>      5?      5@      