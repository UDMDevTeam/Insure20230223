   M S B A M L   `   `   `  ???? G  CEmbriant.WPF, Version=1.0.0.1, Culture=neutral, PublicKeyToken=nullS9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPFEmbriant.WPF.Controls  B >UDM.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullH1clr-namespace:UDM.WPF.Converters;assembly=UDM.WPFUDM.WPF.Converters  UDM.Insure.Interface5   /UDM.Insurance.Interface.Screens.MoveLeadsScreen   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     u	 qInfragisticsWPF4.Documents.Excel.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbs
 oInfragisticsWPF4.DataPresenter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb? }InfragisticsWPF4.DataPresenter.ExcelExporter.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe aInfragisticsWPF4.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbm iInfragisticsWPF4.Editors.v13.1, Version=13.1.20131.2204, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb<igWPF(http://schemas.infragistics.com/xaml/wpf 	 
    Ksrc9clr-namespace:Embriant.WPF.Controls;assembly=Embriant.WPF 	 
    J
Converters1clr-namespace:UDM.WPF.Converters;assembly=UDM.WPF 	 
      ? 	Resources     
PinkButton ?? 
TargetType ?? Property ?? Value ?? 
TargetType ??Name ?? CornerRadius J? RenderTransformOrigin ? RowDefinitions	 ?? 
StartPoint
 ?? EndPoint ? Offset ? Color ??Name ?? Triggers P? Property P? Value ?? 
TargetName0  *UDM.WPF.Converters.InverseBooleanConverter  InverseBooleanConverter??%?  &	        &	 ?    ?? " ??.???  ? ??$ 16(???  ? ??$: 4/UDM.Blush.Interface;component/Fonts/#Century Gothic)???  ? ??$	 600"???  ? ?? ?#000??  ? ??$	 178????  ? ??$ 34????  ? ?? ?? " ??.R??? $ ButtonBorder?????#F9A9BD???	#99FFFFFF$??2q?$ 4??$ 0.5,0.5&?._??  ?? $??* ??? $
??1.5* ?.+??? $??0??$ 4,4,0,0?????? $	 0.5,0&?$
 0.5,1&?.??? $ 0Q?$ 	#08FFFFFF??? $ 1Q?$ 	#88FFFFFF???? $ ButtonContentPresenter??$??2??$??Center=?$??Center=? P?  ?@J?IsKeyboardFocused . . ??? $ ButtonBorder?? ? ?? ?#fad6df?? $ ButtonBorder?? ? ?? ?#df859bP?  ?@??IsDefaulted . . ??? $ ButtonBorder?? ? ?? ?#d06680?? $ ButtonBorder?? ? ?? ?#df859bP?  ?@J?IsMouseOver . . ??? $ ButtonBorder?? ? ?? ?#d06680?? $ ButtonBorder?? ? ?? ?#df859bP?  ? ?? . . ??? $ ButtonBorder?? ? ?? ?#905564P?  ? {? .  . ??? $ ButtonBorder?? ? ?? ?#b6adaf?? $ ButtonBorder?? ? ?? ?#d2c7ca??  ? ?? ?#9c9093  .???? -   $ 	DimBorder??$
??1200??$	??750??
?????? $??0q?$ 12?? J? Opacity$ 1Q????? .???  ? ??
 ????  ?? Triggers P?  ? {? .  . ???  ? ??
 ?????._??? -   $ 
MainBorder??$
??1200??$	??750??
???   ?  BrandedBrushIN#???  $??2q?$ 12?????? .???  ?@J?Opacity$ 1Q? P?  ? {? .  . ???  ?@J?Opacity$	 0.7Q?._?(? .??? -    ?Name$ 
LayoutRoot??$
??1200??$	??750?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ??  ? ColumnDefinitions ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? .+??? -    ??Name$ btnClose??$??27????.   INCloseButton#???  ?? -    ??Name$ hdrLeadCopy??$??1??$??8??$??	21,17,0,0q?$	??Top=?  INPageHeadingText1#???  $??
Move Leads??$??2??$	??322??F? -    F?Name$ hdrLine??$??1??$??1??$??8??$??311.483??$??21,0,0,39.703q?$
??Left=?  INUnderlineText1#???   ?? Stretch$
 Fill=??? -   $ lblCampaign??$??1??$??2??$??0,0,0,10q?$
??Left=?$??Bottom=?  INLabelText2#??[ $??Campaign??$??1??,   &Embriant.WPF.Controls.EmbriantComboBox  -     Name$ cmbCampaigns??$??1??$??4??$??6??$??4,0,0,3q?$??Stretch=?$??Bottom=?  cmbINHighlightBorder#???  $??0???? -	   $ lblBatch??$??2??$??2??$??0,0,0,28q?$
??Left=?$??Bottom=?#??[ $??Batch??$??1??$??57??$??2??  -
   $ cmbBatch??$??2??$??4??$??6??$??4,0,0,29q?$??Stretch=?$??Bottom=?#???  $??0??$??2???? -    ??Name$ lstReferenceNumbers??$??9??$
??Left=?$	??452??$??5??$??11??$	??Top=?$	??358??$??16(?$??	29,10,0,0q?$
??Bold"??? -   $ lblLeadsToCopy??$??4??$??9??$??	29,0,0,22q?$
??Left=?$??Bottom=?#??[ $??Leads In Batch??$	??324???? -   $ lblSearchRefNumber??$??4??$??9??$??
29,0,34,40q?$??Stretch=?$??Bottom=?#??[ $??Ref Number:??$??2???? -    ??Name$ 	txtSearch??$??5??$??3??$??23??$??
10,23,42,0q?$??4?? ?? TextWrapping$
 Wrap=?$	??Top=?$??2???? -   $ 	btnSearch??  INFlatButton#???  $??2??$??Search??$??7??$
??Left=?$??	11,18,0,0q?$??4??$	??Top=?$??75??$??28??$ 	0.693,0.5&?$??2???? -   $ lblDestinationCampaign??$??1??$??8??$??0,0,0,23q?$
??Left=?$??Bottom=?#??[ $??Destination Campaign??$??15??$	??324??  -   $ cmbDestinationCampaigns??$??19??$??7??$??
25,0,22,10q?$??Stretch=?$??Bottom=?#???  $??0??$??2???? -   $ lblDestinationBatch??$??2??$??4??$??0,0,0,3q?$
??Left=?$??Bottom=?#??[ $??Destination Batch??$??15??$	??131???? -   $ lstCopiedReferenceNumbers??{?.  $??7??$??19??$
??Left=?$	??452??$??5??$??11??$	??Top=?$	??254??$??16(?$??25,0,0,0q?$
??Bold"??? -   $ lblCopiedLeads??$??4??$??9??$??	25,0,0,10q?$
??Left=?$??Bottom=?#??[ $??Leads Movied: 0??$??19??$	??324???? -   $ buttonStart??#???  $??2??$??Start Moving??{?.  $
??Auto??$??22??$??
37,10,0,35q?$??16??$??5??$??2?? ? DataContext ?? ?? -   $ txtDestinationBatch??$??7??$??19??$
??Left=?$??32??$??	25,10,0,0q?$??2??$??2??$
 Wrap=?$	??Top=?$	??254??$??16(?$
??Bold"??? -   $ lblStatusesToCopy??$??3??$??4??$??19,0,0,0q?$
??Left=?$??Bottom=?#??[ $??Leads With Status??$??9??$	??143??$??2??  -   $ cmbLeadStatus??$??5??$??9??$??7??$??	19,0,28,4q?$??Stretch=?$??Bottom=?#???  $??0???? -   $ lblLeadsToMoveCount??$??6??$??5??$??	19,0,0,10q?$
??Left=?$??Bottom=?#??[ $??Number Of Leads To Move??$??9??$	??196???? -   $ txtNumberSelected??{?.  $??3??$??9??$
??Left=?$??	19,0,0,10q?$??7??$
 Wrap=?$	??100??$??16(?$
??Bold"?