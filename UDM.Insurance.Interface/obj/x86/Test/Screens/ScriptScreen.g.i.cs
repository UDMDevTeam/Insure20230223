#pragma checksum "..\..\..\..\Screens\ScriptScreen.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "232EBB42B6399090567ABC976B4E103E6BE0DAA1C75F73A3BF02CCBB06ED290A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
using UDM.Insurance.Interface.Screens;
using UDM.WPF.Converters;


namespace UDM.Insurance.Interface.Screens {
    
    
    /// <summary>
    /// ScriptScreen
    /// </summary>
    public partial class ScriptScreen : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\..\..\Screens\ScriptScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border BGRectangle;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\Screens\ScriptScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock hdrScriptScreen;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\..\Screens\ScriptScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMinimize;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\..\Screens\ScriptScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonClose;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\..\Screens\ScriptScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DocumentViewer dvScript;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\..\Screens\ScriptScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkAfrikaans;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\..\..\Screens\ScriptScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblAfrikaans;
        
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
            System.Uri resourceLocater = new System.Uri("/UDM.InsureTest.Interface;component/screens/scriptscreen.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Screens\ScriptScreen.xaml"
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
            
            #line 15 "..\..\..\..\Screens\ScriptScreen.xaml"
            ((UDM.Insurance.Interface.Screens.ScriptScreen)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.BGRectangle = ((System.Windows.Controls.Border)(target));
            return;
            case 3:
            this.hdrScriptScreen = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.btnMinimize = ((System.Windows.Controls.Button)(target));
            
            #line 57 "..\..\..\..\Screens\ScriptScreen.xaml"
            this.btnMinimize.Click += new System.Windows.RoutedEventHandler(this.btnMinimize_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.buttonClose = ((System.Windows.Controls.Button)(target));
            
            #line 65 "..\..\..\..\Screens\ScriptScreen.xaml"
            this.buttonClose.Click += new System.Windows.RoutedEventHandler(this.buttonClose_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.dvScript = ((System.Windows.Controls.DocumentViewer)(target));
            
            #line 82 "..\..\..\..\Screens\ScriptScreen.xaml"
            this.dvScript.Loaded += new System.Windows.RoutedEventHandler(this.dvScript_Loaded);
            
            #line default
            #line hidden
            return;
            case 7:
            this.chkAfrikaans = ((System.Windows.Controls.CheckBox)(target));
            
            #line 97 "..\..\..\..\Screens\ScriptScreen.xaml"
            this.chkAfrikaans.Checked += new System.Windows.RoutedEventHandler(this.chkAfrikaans_Checked);
            
            #line default
            #line hidden
            
            #line 99 "..\..\..\..\Screens\ScriptScreen.xaml"
            this.chkAfrikaans.Unchecked += new System.Windows.RoutedEventHandler(this.chkAfrikaans_Unchecked);
            
            #line default
            #line hidden
            return;
            case 8:
            this.lblAfrikaans = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

