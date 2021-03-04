using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UDM.Insurance.Interface.Windows
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Insure";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
