   M S B A M L   `   `   `  ???? O  KUDM.Insure.Interface, Version=2.6.6.3, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:UDM.Insurance.Interface.ContentUDM.Insurance.Interface.Content  G CEmbriant.WPF, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullS9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPFEmbriant.WPF.Controls 0    *UDM.Insurance.Interface.Screens.NoteScreen   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Acontent-clr-namespace:UDM.Insurance.Interface.Content     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     Isrc9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPF     u qInfragisticsWPF4.Documents.Excel.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbs	 oInfragisticsWPF4.DataPresenter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb?
 }InfragisticsWPF4.DataPresenter.ExcelExporter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe aInfragisticsWPF4.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbm iInfragisticsWPF4.Editors.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb<igWPF(http://schemas.infragistics.com/xaml/wpf  	 
   $??0q???Z?   ? DataContext  ??  ?? RelativeSource ?? *Self+ ?? Path$ NoteData?.??(?  (? Stretch$ Uniform=?.??? $
??1920??$
??1080??.+?? -    ?Name$
 Size??$	??800??$	??600??.+??? 
???   ?   BrandedBrushIN#??[  $??2q? ?? CornerRadius$ 12??._?? -   $ 
LayoutRoot??$	??800??$	??600?? ? RowDefinitions ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ??  ? ColumnDefinitions ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? .+??? -    ??Name$ btnClose??$??18??$??18??  INCloseButton#??[ $??29??6   0UDM.Insurance.Interface.Content.LacePopupWindow1  $??0??$??30??$??6??$	??-18q?$??Stretch=?$??Stretch=??? -   	 ??Name$%	 headingSelectCancellationReason??$??2??$??2??$??4??$	??Top=?  INPageHeadingText1#??[ $??Notes??F? $??1??$??2??$??2??$??4??$??0,0,0,5q?$??Bottom=?$
??Left=?$??60?????? E, )??s?]???@ )?	
 ?? Stretch$

 Fill=?
???????5 	 /Infragistics.Windows.DataPresenter.XamDataCards  -     Name$ xdcNotes??$??4??$??2??$??9??$??17??$??Stretch=?$??Stretch=?$??Stretch=?
??????  ? 	Resources 0  *Infragistics.Windows.Editors.XamTextEditor ?? 
TargetType ?? Property ?? Value??%?   '        ?? "  .???  ?@ AcceptsReturn . ??  ?@ TextWrapping$
 Wrap=???  ? ??$ Stretch=??? # ?@ VerticalScrollBarVisibility$
 Auto=?   ViewSettings 9 	 3Infragistics.Windows.DataPresenter.CardViewSettings     MaxCardCols$ 1??   MaxCardRows$ 3??   AllowCardWidthResizing .    AutoFitCards$ HorizontallyAndVertically=?   InterCardSpacingY$ 10Q?   Padding$ 0,10,0,0q?   HeaderVisibility$ 	Collapsed=?   	CardWidth$	 570Q?: 	 4Infragistics.Windows.DataPresenter.DataPresenterBase   FieldLayoutSettings < 	 6Infragistics.Windows.DataPresenter.FieldLayoutSettings     AllowAddNew$ FalseY?   FieldLayouts 4 	 .Infragistics.Windows.DataPresenter.FieldLayout  
   Key$ NotesLayout??   Fields. . 	 (Infragistics.Windows.DataPresenter.Field     Name$ ID??   Label$ ID??    
Visibility$  	Collapsed=?  $ Sequence??$ Note #??$  Visible=?!   Settings! 6	 	 0Infragistics.Windows.DataPresenter.FieldSettings	  " 	  	AllowEdit?
  ?Infragistics.Windows.Helpers.NullableConverter`1[[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]$" False
   $ FKINImportID??$ FKINImportID??$  	Collapsed=?  $ FKUserID??$ FKUserID??$  	Collapsed=?  $ SalesConsultant??$ Sales Consultant??$  Visible=?! 	  $" False
   $ NoteDate??$
 Date??$  	Collapsed=?! 	  $" False
   $
 Note??$
 Note??$  Visible=?#   Height$# 50X?! 	  $ 	  
EditorType"$  " ?? *CanBeModified+  $ CanBeModified??$ CanBeModified??$  	Collapsed=??? -   $ btnSave??$??13??$??16??$??3??$	??100??$??Right=?$??Bottom=?$
??Save??% ?? 	IsDefault% .    INFlatButton#??[ $??5??{?.  