   M S B A M L   `   `   `  ???? B  >UDM.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullH1clr-namespace:UDM.WPF.Converters;assembly=UDM.WPFUDM.WPF.Converters   UDM.InsureTest.Interface>   8UDM.Insurance.Interface.Screens.LeadContactDetailsScreen   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     H
converters1clr-namespace:UDM.WPF.Converters;assembly=UDM.WPF     @d2http://schemas.microsoft.com/expression/blend/2008     s oInfragisticsWPF4.DataPresenter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb0igDP%http://infragistics.com/DataPresenter  u	 qInfragisticsWPF4.Documents.Excel.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb?
 }InfragisticsWPF4.DataPresenter.ExcelExporter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe aInfragisticsWPF4.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbm iInfragisticsWPF4.Editors.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb<igWPF(http://schemas.infragistics.com/xaml/wpf 	  
   Lmc;http://schemas.openxmlformats.org/markup-compatibility/2006 	  
   -     ? Title$  Lead Contact Details?? ? AllowsTransparency . 
?????? $??0q? ? Icon$ ../Resources/icons/in.ico?? ? 
ResizeMode$ CanMinimize=? ? ShowInTaskbar .  ? Topmost .  ? WindowStartupLocation$ CenterScreen=? ? WindowState$ Normal=? ? WindowStyle$
 None=????? 	 ?? Source#	 Zo?
 ?? Path$
 Width? ?? 	Converter &    UDM.WPF.Converters.MathConverter   ?? ConverterParameter$ x*0.4688??????  ?? RelativeSource ?? *Self+$
 Width?   $ x*0.5655??.??? .+?(?  (? Stretch$ Uniform=?.??? $	??900??$	??510??.+??? -    ??Name$ BGRectangle??$??0q?
???   ?   BrandedBrushIN#??[  $??2q? ?? CornerRadius$ 10??????  ?? 
TargetType" ?? ?? Triggers ._?? .+??? -    ??Name$ hdrScriptScreen??$??	18,12,0,0q?$	??Top=?$??18(?  INPageHeadingText1#??[ $??Lead Contact Details???? $??	0,12,12,0q?$??Right=? ?? Orientation$ 
Horizontal=?.+??? -    ??Name$ btnMinimize??$??Right=?$	??Top=?$??-??  RoundedButton#??[ $??0??? $??12???? -   $ buttonClose??$??Right=?$	??Top=?$??X??#??[ $??0???? .+?? $??46?? ? ColumnDefinitions ?? $??* ??? $??* ??? -   $ bdrDataGrid??$	??440??$??	18,0,18,0q?._?? .+?4  .Infragistics.Windows.DataPresenter.XamDataGrid  -     Name$ xdgContactDetails??$??0q?:  4Infragistics.Windows.DataPresenter.DataPresenterBase   GroupByAreaLocation$
 None=?????   INXamDataGridStyle1 ?? BasedOn# [ "      FieldLayouts 4  .Infragistics.Windows.DataPresenter.FieldLayout     Fields. .  (Infragistics.Windows.DataPresenter.Field     Name$ ID??   Label$ ID??   
Visibility$ 	Collapsed=?  $ Person??   Width$ 70X?$ Person??$ Visible=?  $ Title??$ 70X?$ Title??$ Visible=?  $
 Name??$	 160X?$
 Name??$ Visible=?  $ Surname??$	 120X?$ Surname??$ Visible=?  $ Relationship??$	 120X?$ Relationship??$ Visible=?  $
 Type??$	 100X?$
 Type??$ Visible=?  $ Number??$	 120X?$ Number??$ Visible=?  $
 Date??$	 120X?$
 Date??$ Visible=?? -     ?Name$  grdProgress??$	??250??$??30??$??Center=?$??Center=?$y?	Collapsed=?.+?? -	   ! ?Name$! pbStatus??#??[  " ? IsIndeterminate" . # ? Maximum$	# 100Q?$ ? Minimum$$ 0Q?% ? Value$% 0Q??? $??Center=?$??Center=?$??16(?
???   ????? & ?? ElementName$& pbStatus??$
 Value?' ?? StringFormat$' retreiving data ... {0}s??