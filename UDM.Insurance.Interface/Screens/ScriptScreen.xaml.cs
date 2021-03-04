using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Xps.Packaging;
using Embriant.Framework.Configuration;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Data;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ScriptScreen
    {
        #region Constants



        #endregion


        #region Dependency Properties

        public long? ScriptID
        {
            get { return (long?)GetValue(ScriptIDProperty); }
            set { SetValue(ScriptIDProperty, value); }
        }
        public static readonly DependencyProperty ScriptIDProperty = DependencyProperty.Register("ScriptID", typeof(long?), typeof(ScriptScreen), new UIPropertyMetadata(null));

        public lkpScriptType? ScriptType
        {
            get { return (lkpScriptType?)GetValue(ScriptTypeProperty); }
            set { SetValue(ScriptTypeProperty, value); }
        }
        public static readonly DependencyProperty ScriptTypeProperty = DependencyProperty.Register("ScriptType", typeof(lkpScriptType?), typeof(ScriptScreen), new UIPropertyMetadata(null));

        public long? ScriptLanguageID
        {
            get { return (long?)GetValue(ScriptLanguageIDProperty); }
            set { SetValue(ScriptLanguageIDProperty, value); }
        }
        public static readonly DependencyProperty ScriptLanguageIDProperty = DependencyProperty.Register("ScriptLanguageID", typeof(long?), typeof(ScriptScreen), new UIPropertyMetadata(null));
        
        #endregion


        #region Members

        private string _strXpsDoc;
        private XpsDocument _xpsDocument;

        public LeadApplicationData LaData;

        #endregion


        #region Constructors

        public ScriptScreen()
        {
            InitializeComponent();
        }

        #endregion


        #region Private methods

        public void LoadScriptDocument()
        {
            DataTable dt;
            StringBuilder strSQL = new StringBuilder(string.Empty);
            ScriptLanguageID = ScriptLanguageID ?? (long)lkpLanguage.English;

            //strSQL.Append("SELECT TOP 1 * FROM Scripts WHERE FKScriptTypeID = '" + (long?)ScriptType + "' ");

            //strSQL.Append("AND FKLanguageID = '" + ScriptLanguageID + "' ");
            //strSQL.Append("AND  IsActive = '1' AND (");
            //strSQL.Append("(FKCampaignTypeGroupID IS NOT NULL AND FKCampaignTypeGroupID = '" + (long?)LaData.AppData.CampaignTypeGroup + "' ");
            //strSQL.Append("AND FKCampaignGroupTypeID IS NOT NULL AND FKCampaignGroupTypeID = '" + (long?)LaData.AppData.CampaignGroupType + "') ");           
            //strSQL.Append(")");

            strSQL.Append("SELECT TOP 1 * FROM Scripts WHERE FKScriptTypeID = '" + (long?)ScriptType + "' ");
            strSQL.Append("AND (");
            strSQL.Append("(FKCampaignID IS NOT NULL AND FKCampaignID = '" + LaData.AppData.CampaignID + "') ");
            strSQL.Append("OR (");
            strSQL.Append("(FKCampaignTypeID IS NOT NULL AND FKCampaignTypeID = '" + (long?)LaData.AppData.CampaignType + "') ");
            strSQL.Append("AND (FKCampaignGroupID IS NOT NULL AND FKCampaignGroupID = '" + (long?)LaData.AppData.CampaignGroup + "') ");
            strSQL.Append(") OR (");
            strSQL.Append("(FKCampaignTypeGroupID IS NOT NULL AND FKCampaignTypeGroupID = '" + (long?)LaData.AppData.CampaignTypeGroup + "') ");
            strSQL.Append("AND (FKCampaignGroupTypeID IS NOT NULL AND FKCampaignGroupTypeID = '" + (long?)LaData.AppData.CampaignGroupType + "') ");
            strSQL.Append(")) ");

            //switch (ScriptType)
            //{
            //    case lkpScriptType.ObjectionInformation:

            //        break;

            //    case lkpScriptType.ClaimInformation:

            //        break;

            //    case lkpScriptType.ExclusionInformation:

            //        break;
            //}

            strSQL.Append("AND FKLanguageID = '" + ScriptLanguageID + "' ");
            strSQL.Append("AND  IsActive = '1'");




            dt = Methods.GetTableData(strSQL.ToString());

            if (dt != null && dt.Rows.Count == 1)
            {
                ScriptID = dt.Rows[0]["ID"] as long?;
                ScriptLanguageID = dt.Rows[0]["FKLanguageID"] as long?;
                byte[] data = dt.Rows[0]["Document"] as byte[];

                if (data != null)
                {
                    _strXpsDoc = GlobalSettings.UserFolder + "script ~ " + DateTime.Now.Millisecond + ".xps";
                    FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                    objFileStream.Write(data, 0, data.Length);
                    objFileStream.Close();

                    _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                    dvScript.Document = _xpsDocument.GetFixedDocumentSequence();

                    objFileStream.Close();
                    objFileStream.Dispose();
                }
            }

        }

        #endregion


        #region Event Handlers

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            //Application.Current.MainWindow.Opacity = 1;
            //Application.Current.MainWindow.ShowInTaskbar = true;

            Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void dvScript_Loaded(object sender, RoutedEventArgs e)
        {
            LoadScriptDocument();
        }

        private void chkAfrikaans_Checked(object sender, RoutedEventArgs e)
        {
            ScriptID = null;
            ScriptLanguageID = (long)lkpLanguage.Afrikaans;
            LoadScriptDocument();
        }

        private void chkAfrikaans_Unchecked(object sender, RoutedEventArgs e)
        {
            ScriptID = null;
            ScriptLanguageID = (long)lkpLanguage.English;
            LoadScriptDocument();
        }


        #endregion
    }
}
