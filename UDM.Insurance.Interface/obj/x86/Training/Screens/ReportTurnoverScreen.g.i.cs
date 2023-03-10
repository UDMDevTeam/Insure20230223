#pragma checksum "..\..\..\..\Screens\ReportTurnoverScreen.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "CB82E9AB4D23AB8909B804C1933781F52A6553FD906B3CD76E568E1E3FFCEE4C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Embriant.WPF.Controls;
using Infragistics.Documents.Excel;
using Infragistics.Shared;
using Infragistics.Windows;
using Infragistics.Windows.Controls;
using Infragistics.Windows.Controls.Markup;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.Calculations;
using Infragistics.Windows.DataPresenter.ExcelExporter;
using Infragistics.Windows.Editors;
using Infragistics.Windows.Reporting;
using Infragistics.Windows.Themes;
using Infragistics.Windows.Tiles;
using Microsoft.Windows.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using UDM.Insurance.Business;


namespace UDM.Insurance.Interface.Screens {
    
    
    /// <summary>
    /// ReportTurnoverScreen
    /// </summary>
    public partial class ReportTurnoverScreen : Embriant.WPF.Controls.BaseControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 11 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border DimBorder;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border MainBorder;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnClose;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock hdrTurnoverReport;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Path hdrLine;
        
        #line default
        #line hidden
        
        
        #line 117 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbSalaryReportType;
        
        #line default
        #line hidden
        
        
        #line 129 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Embriant.WPF.Controls.EmbriantComboBox cmbCampaignType;
        
        #line default
        #line hidden
        
        
        #line 141 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblCampaigns;
        
        #line default
        #line hidden
        
        
        #line 170 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Infragistics.Windows.DataPresenter.XamDataGrid xdgCampaigns;
        
        #line default
        #line hidden
        
        
        #line 259 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblCal1;
        
        #line default
        #line hidden
        
        
        #line 282 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Infragistics.Windows.Editors.XamMonthCalendar calStartDate;
        
        #line default
        #line hidden
        
        
        #line 303 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Infragistics.Windows.Editors.XamMonthCalendar calEndDate;
        
        #line default
        #line hidden
        
        
        #line 326 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radByCampaign;
        
        #line default
        #line hidden
        
        
        #line 338 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radByTSR;
        
        #line default
        #line hidden
        
        
        #line 350 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radByQA;
        
        #line default
        #line hidden
        
        
        #line 367 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radInsurance;
        
        #line default
        #line hidden
        
        
        #line 379 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radIG;
        
        #line default
        #line hidden
        
        
        #line 391 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radBoth;
        
        #line default
        #line hidden
        
        
        #line 406 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkIncludeBumpups;
        
        #line default
        #line hidden
        
        
        #line 422 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkIncludeAdmin;
        
        #line default
        #line hidden
        
        
        #line 451 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblSaffType;
        
        #line default
        #line hidden
        
        
        #line 463 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Embriant.WPF.Controls.EmbriantComboBox cmbStaffType;
        
        #line default
        #line hidden
        
        
        #line 475 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnReport;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/UDM.Insure.Interface;component/screens/reportturnoverscreen.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.DimBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.MainBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 3:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.btnClose = ((System.Windows.Controls.Button)(target));
            
            #line 92 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            this.btnClose.Click += new System.Windows.RoutedEventHandler(this.btnClose_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.hdrTurnoverReport = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.hdrLine = ((System.Windows.Shapes.Path)(target));
            return;
            case 7:
            this.tbSalaryReportType = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.cmbCampaignType = ((Embriant.WPF.Controls.EmbriantComboBox)(target));
            
            #line 137 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            this.cmbCampaignType.DropDownClosed += new System.EventHandler(this.cmbCampaignType_DropDownClosed);
            
            #line default
            #line hidden
            
            #line 137 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            this.cmbCampaignType.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cmbCampaignType_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.lblCampaigns = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.xdgCampaigns = ((Infragistics.Windows.DataPresenter.XamDataGrid)(target));
            
            #line 181 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            this.xdgCampaigns.FieldLayoutInitialized += new System.EventHandler<Infragistics.Windows.DataPresenter.Events.FieldLayoutInitializedEventArgs>(this.xdgCampaigns_FieldLayoutInitialized);
            
            #line default
            #line hidden
            return;
            case 13:
            this.lblCal1 = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 14:
            this.calStartDate = ((Infragistics.Windows.Editors.XamMonthCalendar)(target));
            
            #line 285 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            this.calStartDate.SelectedDatesChanged += new System.EventHandler<Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs>(this.Cal1_SelectedDatesChanged);
            
            #line default
            #line hidden
            return;
            case 15:
            this.calEndDate = ((Infragistics.Windows.Editors.XamMonthCalendar)(target));
            
            #line 306 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            this.calEndDate.SelectedDatesChanged += new System.EventHandler<Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs>(this.Cal2_SelectedDatesChanged);
            
            #line default
            #line hidden
            return;
            case 16:
            this.radByCampaign = ((System.Windows.Controls.RadioButton)(target));
            
            #line 330 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            this.radByCampaign.Checked += new System.Windows.RoutedEventHandler(this.radByCampaign_Checked);
            
            #line default
            #line hidden
            return;
            case 17:
            this.radByTSR = ((System.Windows.Controls.RadioButton)(target));
            
            #line 342 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            this.radByTSR.Checked += new System.Windows.RoutedEventHandler(this.radByTSR_Checked);
            
            #line default
            #line hidden
            return;
            case 18:
            this.radByQA = ((System.Windows.Controls.RadioButton)(target));
            
            #line 354 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            this.radByQA.Checked += new System.Windows.RoutedEventHandler(this.radByQA_Checked);
            
            #line default
            #line hidden
            return;
            case 19:
            this.radInsurance = ((System.Windows.Controls.RadioButton)(target));
            
            #line 371 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            this.radInsurance.Checked += new System.Windows.RoutedEventHandler(this.radCompanyType_Checked);
            
            #line default
            #line hidden
            return;
            case 20:
            this.radIG = ((System.Windows.Controls.RadioButton)(target));
            
            #line 383 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            this.radIG.Checked += new System.Windows.RoutedEventHandler(this.radCompanyType_Checked);
            
            #line default
            #line hidden
            return;
            case 21:
            this.radBoth = ((System.Windows.Controls.RadioButton)(target));
            
            #line 395 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            this.radBoth.Checked += new System.Windows.RoutedEventHandler(this.radCompanyType_Checked);
            
            #line default
            #line hidden
            return;
            case 22:
            this.chkIncludeBumpups = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 23:
            this.chkIncludeAdmin = ((System.Windows.Controls.CheckBox)(target));
            
            #line 429 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            this.chkIncludeAdmin.Checked += new System.Windows.RoutedEventHandler(this.chkIncludeAdmin_Checked);
            
            #line default
            #line hidden
            return;
            case 24:
            this.lblSaffType = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 25:
            this.cmbStaffType = ((Embriant.WPF.Controls.EmbriantComboBox)(target));
            
            #line 471 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            this.cmbStaffType.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cmbStaffType_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 26:
            this.btnReport = ((System.Windows.Controls.Button)(target));
            
            #line 482 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            this.btnReport.Click += new System.Windows.RoutedEventHandler(this.btnReport_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 11:
            
            #line 194 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.HeaderPrefixAreaCheckbox_Checked);
            
            #line default
            #line hidden
            
            #line 196 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Loaded += new System.Windows.RoutedEventHandler(this.HeaderPrefixAreaCheckbox_Loaded);
            
            #line default
            #line hidden
            
            #line 198 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.HeaderPrefixAreaCheckbox_Unchecked);
            
            #line default
            #line hidden
            break;
            case 12:
            
            #line 213 "..\..\..\..\Screens\ReportTurnoverScreen.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Click += new System.Windows.RoutedEventHandler(this.RecordSelectorCheckbox_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

