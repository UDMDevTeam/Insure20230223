using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UDM.Insurance.Interface.Screens
{
    public partial class Window1 : Window
    {
        //private System.Windows.Forms.Panel _panel;
        private Process _process;

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32")]
        private static extern IntPtr SetParent(IntPtr hWnd, IntPtr hWndParent);

        [DllImport("user32")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_NOACTIVATE = 0x0010;
        private const int GWL_STYLE = -16;
        private const int WS_CAPTION = 0x00C00000;
        private const int WS_THICKFRAME = 0x00040000;
        const string patran = "patran";

        public Window1()
        {
            InitializeComponent();

            Loaded += (s, e) => LaunchChildProcess();
            //_panel = new System.Windows.Forms.Panel();
            //windowsFormsHost1.Child = _panel;
        }

        private void LaunchChildProcess()
        {
            _process = Process.Start("calc.exe");
            _process.WaitForInputIdle();

            var helper = new WindowInteropHelper(this);

            SetParent(_process.MainWindowHandle, helper.Handle);

            // remove control box
            int style = GetWindowLong(_process.MainWindowHandle, GWL_STYLE);
            style = style & ~WS_CAPTION & ~WS_THICKFRAME;
            SetWindowLong(_process.MainWindowHandle, GWL_STYLE, style);
            // resize embedded application & refresh
            ResizeEmbeddedApp();
        }

        private void ResizeEmbeddedApp()
        {
            if (_process == null)
                return;
            SetWindowPos(_process.MainWindowHandle, IntPtr.Zero, 0, 0, (int)ActualWidth, (int)ActualHeight, SWP_NOZORDER | SWP_NOACTIVATE);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size size = base.MeasureOverride(availableSize);
            ResizeEmbeddedApp();
            return size;
        }

        //private void button1_Click(object sender, RoutedEventArgs e)
        //{
        //    //button1.Visibility = Visibility.Hidden;
        //    //ProcessStartInfo psi = new ProcessStartInfo("calc.exe");
        //    //psi.UseShellExecute = false;
        //    //_process = Process.Start(psi);
        //    ////_process.WaitForInputIdle();
        //    //while (_process.MainWindowHandle == IntPtr.Zero)
        //    //{
        //    //    Thread.Sleep(1);
        //    //}
        //    //SetParent(_process.MainWindowHandle, _panel.Handle);

        //    // remove control box
        //    //int style = GetWindowLong(_process.MainWindowHandle, GWL_STYLE);
        //    //style = style & ~WS_CAPTION & ~WS_THICKFRAME;
        //    //SetWindowLong(_process.MainWindowHandle, GWL_STYLE, style);

        //    // resize embedded application & refresh
        //    //ResizeEmbeddedApp();
        //}

        //protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        //{
        //    base.OnClosing(e);
        //    if (_process != null)
        //    {
        //        _process.Refresh();
        //        _process.Close();
        //    }
        //}

        //private void ResizeEmbeddedApp()
        //{
        //    if (_process == null)
        //        return;

        //    SetWindowPos(_process.MainWindowHandle, IntPtr.Zero, 0, 0, (int)_panel.ClientSize.Width, (int)_panel.ClientSize.Height, SWP_NOZORDER | SWP_NOACTIVATE);
        //}

        //protected override Size MeasureOverride(Size availableSize)
        //{
        //    Size size = base.MeasureOverride(availableSize);
        //    ResizeEmbeddedApp();
        //    return size;
        //}

        //protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        //{
        //    ProcessStartInfo psi = new ProcessStartInfo("notepad.exe");
        //    psi.WindowStyle = ProcessWindowStyle.Minimized;
        //    _process = Process.Start(psi);
        //    _process.WaitForInputIdle();

        //    // The main window handle may be unavailable for a while, just wait for it
        //    while (_process.MainWindowHandle == IntPtr.Zero)
        //    {
        //        Thread.Yield();
        //    }

        //    IntPtr notepadHandle = _process.MainWindowHandle;

        //    int style = GetWindowLong(notepadHandle, GWL_STYLE);
        //    style = style & ~((int)WS_CAPTION) & ~((int)WS_THICKFRAME); // Removes Caption bar and the sizing border
        //    style |= ((int)WS_CHILD); // Must be a child window to be hosted

        //    SetWindowLong(notepadHandle, GWL_STYLE, style);
        //    SetParent(notepadHandle, hwndParent.Handle);

        //    this.InvalidateVisual();

        //    HandleRef hwnd = new HandleRef(this, notepadHandle);
        //    return hwnd;
        //}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //ProcessStartInfo psi = new ProcessStartInfo("notepad.exe");
            //psi.WindowStyle = ProcessWindowStyle.Minimized;
            //_process = Process.Start(psi);
            //_process.WaitForInputIdle();

            //// The main window handle may be unavailable for a while, just wait for it
            //while (_process.MainWindowHandle == IntPtr.Zero)
            //{
            //    Thread.Yield();
            //}

            //IntPtr notepadHandle = _process.MainWindowHandle;

            //int style = GetWindowLong(notepadHandle, GWL_STYLE);
            //style = style & ~((int)WS_CAPTION) & ~((int)WS_THICKFRAME); // Removes Caption bar and the sizing border
            //style |= ((int)WS_CHILD); // Must be a child window to be hosted

            //SetWindowLong(notepadHandle, GWL_STYLE, style);
            //SetParent(notepadHandle, hwndParent.Handle);

            //// Create the interop host control.
            //System.Windows.Forms.Integration.WindowsFormsHost host =
            //    new System.Windows.Forms.Integration.WindowsFormsHost();

            //// Create the MaskedTextBox control.
            //MaskedTextBox mtbDate = new MaskedTextBox("00/00/0000");

            //// Assign the MaskedTextBox control as the host control's child.
            //host.Child = mtbDate;

            //// Add the interop host control to the Grid
            //// control's collection of child controls.
            //this.grid1.Children.Add(host);
        }
    }
}
