   M S B A M L   `   `   `  ???? B  >UDM.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullH1clr-namespace:UDM.WPF.Converters;assembly=UDM.WPFUDM.WPF.Converters   UDM.InsureTest.Interface6   0UDM.Insurance.Interface.Screens.LiveProductivity   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ~ zInfragisticsWPF4.Controls.Charts.XamGauge.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbx	 tInfragisticsWPF4.Controls.Schedules.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb?
 ?InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb? ~InfragisticsWPF4.Controls.Editors.XamCalendar.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb| xInfragisticsWPF4.Controls.Menus.XamMenu.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe aInfragisticsWPF4.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb7ig$http://schemas.infragistics.com/xaml  	 
    L
converters1clr-namespace:UDM.WPF.Converters;assembly=UDM.WPF  	 
    -     ? Title$  LiveProductivity??$	??300??$	??220??
??????  ? WindowStyle$
 None=? ? Topmost . $??0q? ? Left$ 0?? ? AllowsTransparency . {?. 
 ? Top ??  ?? Source# Zo? ?? Path$ Height? ?? 	Converter &    UDM.WPF.Converters.MathConverter  	 ?? ConverterParameter$	 x*0.4??.??? 
 ? RowDefinitions
 ?? ?? ?? ?? ?? ?? ?? ?? ?? .+??? -    ??Name$ tbLiveProductivityLabel??$??0??$??0q?$??Center=?$	??Top=?   INPageHeadingText1#???   $??Live Productivity??$y?Visible=?1  +Infragistics.Controls.Charts.XamRadialGauge  -     Name$ xrgLiveProductivity??$??1??$??6??$??5??$??0q?$??Center=?$??Bottom=?$y?Visible=?   Scales 3  -Infragistics.Controls.Charts.RadialGaugeScale     LabelGroups 8  2Infragistics.Controls.Charts.RadialGaugeLabelGroup     Extent$
 0.65Q?   Interval$ 10Q?   Orientation$ Outward=?   PreTerminal$ 0Q?$??14(?   Needles 4  .Infragistics.Controls.Charts.RadialGaugeNeedle     	EndExtent$
 0.65Q?   InverseBrushRotation .    Value$ 0Q?   Ranges 3  -Infragistics.Controls.Charts.RadialGaugeRange     
StartValue$ 0Q?   EndValue$ 20Q?   InnerExtentStart$
 0.57Q?   InnerExtentEnd$
 0.60Q?   OuterExtent$
 0.55Q?   Fill ?#CD0000  $ 20Q?$ 40Q?$
 0.60Q?$
 0.63Q?$
 0.55Q? ?#FF7F00  $ 40Q?$ 60Q?$
 0.63Q?$
 0.66Q?$
 0.55Q? ?#FFD700  $ 60Q?$ 80Q?$
 0.66Q?$
 0.69Q?$
 0.55Q? ?#66CD00  $ 80Q?$	 100Q?$
 0.69Q?$
 0.72Q?$
 0.55Q? ?#00B2EE   TickMarkGroups ;  5Infragistics.Controls.Charts.RadialGaugeTickMarkGroup     	EndExtent$	 0.6Q?    Interval$  10Q?!   StartExtent$	! 0.5Q?  $	 0.6Q?$  5Q?$	! 0.5Q?