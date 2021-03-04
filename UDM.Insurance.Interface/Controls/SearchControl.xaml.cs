using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace UDM.Insurance.Interface.Controls
{
    #region Delegates

    public delegate void SearchTermChangedHandler(string searchTerm);

    #endregion

	public partial class SearchControl
	{

        //DateTime dtLastKeyUp = DateTime.Now;
        //private readonly DispatcherTimer _TimerKeyPress = new DispatcherTimer();



        #region Events

        public event SearchTermChangedHandler SearchChanged;

        #endregion



        #region Constructor

        public SearchControl()
		{
			InitializeComponent();

            //_TimerKeyPress.Tick += TimerKeyPress;
            //_TimerKeyPress.Interval = new TimeSpan(0,0,0,0, 100);
		}

        #endregion



        #region Timers

     //   private void TimerKeyPress(object sender, EventArgs eventArgs)
	    //{
     //       TimeSpan timeSinceLastKeyUp = DateTime.Now.Subtract(dtLastKeyUp);

     //       if (timeSinceLastKeyUp.TotalMilliseconds > 800)
	    //    {
	    //        _TimerKeyPress.Stop();

     //           if (txtSearch.Text != null && SearchChanged != null) //txtSearch.Text.Length > 1
     //           {
     //               SearchChanged(txtSearch.Text.Trim());
     //           }
	    //    }
	    //}

        #endregion



        #region Event Handlers

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key != Key.Tab)
            //{
            //    dtLastKeyUp = DateTime.Now;
            //    _TimerKeyPress.Start();
            //}
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (txtSearch.Text != null && SearchChanged != null) //txtSearch.Text.Length > 1
            {
                SearchChanged(txtSearch.Text.Trim());
            }
        }

        #endregion


    }
}