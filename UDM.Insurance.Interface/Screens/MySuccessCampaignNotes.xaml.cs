using Microsoft.Office.Interop.Word;
using System;
using System.Windows;
using System.Windows.Input;
using Embriant.Framework.Configuration;
using System.Data;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Windows;
using System.Windows.Controls;
using UDM.WPF.Library;
using System.Data.SqlClient;
using Embriant.Framework;
using Embriant.Framework.Data;
using System.Collections.Generic;
using Infragistics.Windows.DataPresenter;
using System.Linq;
using System.Windows.Xps.Packaging;
using System.IO;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for MySuccessCampaignNotes.xaml
    /// </summary>
    public partial class MySuccessCampaignNotes
    {

        string CampaignNotesID; 
        public MySuccessCampaignNotes(string _CampaignNoteIDs)
        {
            InitializeComponent();

            CampaignNotesID = _CampaignNoteIDs;

            LoadDocument();

            
        }

        #region Variables

        //System.Data.DataTable dtCampaigns;

        private string _strXpsDoc;
        private XpsDocument _xpsDocument;

        #endregion

        public void LoadDocument()
        {

            try
            {
                //XpsDocument xpsDocument = new XpsDocument("C:/Users/DaneB/Documents/Company Documents/Blush/Scripts/Script-1-English.xps", System.IO.FileAccess.Read);
                //dvClosure.Document = xpsDocument.GetFixedDocumentSequence(); 

                System.Data.DataTable dtClosure;
                System.Data.DataTable dtScript;
                System.Data.DataTable dtOptions;
                System.Data.DataTable dtCovers;
                System.Data.DataTable dtObjectionHandling;
                System.Data.DataTable dtNeedCreation;
                System.Data.DataTable dtIncentiveStructure;

                string CampaignID = GlobalSettings.CampaignID;
                GlobalSettings.CampaignNotesID = CampaignNotesID;

                if (CampaignNotesID == "1")
                {
                    #region Scripts

                    if (chkClosureAfrikaans.IsChecked == true && chkClosureEnglish.IsChecked == false)
                    {
                        try
                        {
                            string strSQLScript = "SELECT FKCampaignID [FKCampaignID], ScriptAfr [ScriptAfr] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + "";

                            //dt = Methods.GetTableData(
                            //        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                            if (strSQLScript != null)
                            {

                                dtScript = Methods.GetTableData(strSQLScript);

                                if (dtScript.Rows[0]["ScriptAfr"] != null)
                                {
                                    CampaignID = dtScript.Rows[0]["FKCampaignID"] as string;

                                    byte[] data = dtScript.Rows[0]["ScriptAfr"] as byte[];

                                    try
                                    {

                                        if (data != null)
                                        {

                                            _strXpsDoc = Convert.ToString(data);

                                            FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                                            objFileStream.Write(data, 0, data.Length);
                                            objFileStream.Close();

                                            _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                                            dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();
                                            _xpsDocument.Close();

                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        HandleException(ex);
                                    }

                                }

                                try { dtScript.Clear(); } catch { }


                            }

                            strSQLScript = null;
                        }
                        catch (Exception ex) 
                        {
                            HandleException(ex);
                        }
                    }

                    if (chkClosureEnglish.IsChecked == true && chkClosureAfrikaans.IsChecked == false)
                    {

                        try
                        {

                            string strSQLScript = "SELECT FKCampaignID [FKCampaignID], ScriptEng [ScriptEng] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + "";

                            //dt = Methods.GetTableData(
                            //        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                            if (strSQLScript != null)
                            {

                                dtScript = Methods.GetTableData(strSQLScript);

                                if (dtScript.Rows[0]["ScriptEng"] != null)
                                {
                                    CampaignID = dtScript.Rows[0]["FKCampaignID"] as string;

                                    byte[] data = dtScript.Rows[0]["ScriptEng"] as byte[];

                                    try
                                    {

                                        if (data != null)
                                        {

                                            _strXpsDoc = Convert.ToString(data);

                                            FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                                            objFileStream.Write(data, 0, data.Length);
                                            objFileStream.Close();

                                            _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                                            dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();
                                            _xpsDocument.Close();

                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        HandleException(ex);
                                    }

                                }

                                try { dtScript.Clear(); } catch { }

                                strSQLScript = null;
                            }

                            strSQLScript = null;


                        }
                        catch (Exception ex) 
                        {
                            HandleException(ex); 
                        }
                    }


                    #endregion

                }
                else if (CampaignNotesID == "2")
                {
                    #region Closure

                    try
                    {

                        if (chkClosureAfrikaans.IsChecked == true)
                        {

                            try
                            {

                                string strSQLClosure = "SELECT FKCampaignID [FKCampaignID], ClosureAfr [ClosureAfr] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + "";

                                //dt = Methods.GetTableData(
                                //        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                                if (strSQLClosure != null)
                                {

                                    dtClosure = Methods.GetTableData(strSQLClosure);

                                    if (dtClosure.Rows[0]["ClosureAfr"] != null)
                                    {
                                        CampaignID = dtClosure.Rows[0]["FKCampaignID"] as string;

                                        byte[] data = dtClosure.Rows[0]["ClosureAfr"] as byte[];

                                        if (data != null)
                                        {

                                            try
                                            {
                                                _strXpsDoc = Convert.ToString(data);

                                                FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                                                objFileStream.Write(data, 0, data.Length);
                                                objFileStream.Close();

                                                _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                                                dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();
                                                _xpsDocument.Close();

                                            }
                                            catch (Exception ex)
                                            {
                                                HandleException(ex);
                                            }

                                            //data = null;
                                        }

                                        else if (data == null)
                                        {
                                            MessageBox.Show("This message indicates a blank value");
                                        }


                                    }


                                    try { dtClosure.Clear(); } catch { }
                                }

                                strSQLClosure = null;
                            }
                            catch (Exception ex) 
                            {
                                HandleException(ex); 
                            }
                        }
                        
                        if (chkClosureEnglish.IsChecked == true)
                        {
                            try
                            {

                                string strSQLClosure = "SELECT FKCampaignID [FKCampaignID], ClosureEng [ClosureEng] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + "";

                                //dt = Methods.GetTableData(
                                //        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                                if (strSQLClosure != null)
                                {

                                    dtClosure = Methods.GetTableData(strSQLClosure);

                                    if (dtClosure.Rows[0]["ClosureEng"] != null)
                                    {
                                        CampaignID = dtClosure.Rows[0]["FKCampaignID"] as string;

                                        byte[] data = dtClosure.Rows[0]["ClosureEng"] as byte[];

                                        if (data != null)
                                        {

                                            try
                                            {
                                                _strXpsDoc = Convert.ToString(data);

                                                FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                                                objFileStream.Write(data, 0, data.Length);
                                                objFileStream.Close();

                                                _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                                                dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();
                                                _xpsDocument.Close();

                                            }
                                            catch (Exception ex)
                                            {
                                                HandleException(ex);
                                            }

                                            //data = null;
                                        }

                                        else if (data == null)
                                        {
                                            MessageBox.Show("This message indicates a blank value");
                                        }


                                    }


                                    try { dtClosure.Clear(); } catch { }
                                }



                                strSQLClosure = null;

                            }
                            catch (Exception ex) 
                            {
                                HandleException(ex);
                            }
                        }

                        
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex);
                    }

                }

                #endregion


                else if (CampaignNotesID == "3")
                {
                    #region Options

                    string strSQLOptions = "SELECT FKCampaignID [FKCampaignID], Options [Options] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + "";

                    if (strSQLOptions != null)
                    {

                        dtOptions = Methods.GetTableData(strSQLOptions);

                        if (dtOptions.Rows[0]["Options"] != null)
                        {
                            CampaignID = dtOptions.Rows[0]["FKCampaignID"] as string;

                            byte[] data = dtOptions.Rows[0]["Options"] as byte[];

                            if (data != null)
                            {

                                try
                                {
                                    _strXpsDoc = Convert.ToString(data);

                                    FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                                    objFileStream.Write(data, 0, data.Length);
                                    objFileStream.Close();

                                    _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                                    dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();
                                    _xpsDocument.Close();
                                }
                                catch (Exception ex) 
                                {
                                    HandleException(ex); 
                                }


                            }

                            try { dtOptions.Clear(); } catch { }

                        }


                        strSQLOptions = null;
                    }

                    #endregion

                }

                else if (CampaignNotesID == "4")
                {
                    #region Incentive Structure

                    string strSQLIncentiveStructure = "SELECT FKCampaignID [FKCampaignID], IncentiveStructure [IncentiveStructure] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + "";

                    //dt = Methods.GetTableData(
                    //        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                    if (strSQLIncentiveStructure != null)
                    {

                        dtIncentiveStructure = Methods.GetTableData(strSQLIncentiveStructure);

                        if (dtIncentiveStructure.Rows[0]["IncentiveStructure"] != null)
                        {
                            CampaignID = dtIncentiveStructure.Rows[0]["FKCampaignID"] as string;

                            byte[] data = dtIncentiveStructure.Rows[0]["IncentiveStructure"] as byte[];

                            if (data != null)
                            {

                                try 
                                { 
                                _strXpsDoc = Convert.ToString(data);

                                FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                                objFileStream.Write(data, 0, data.Length);
                                objFileStream.Close();

                                _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                                dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();
                                _xpsDocument.Close();

                                }
                                catch (Exception ex) 
                                {
                                    HandleException(ex);
                                }
                            }

                            try { dtIncentiveStructure.Clear(); } catch { }

                        }

                        strSQLIncentiveStructure = null;
                    }

                    #endregion

                }
                else if (CampaignNotesID == "5")
                {
                    #region Objection Handling

                    string strSQLObjectionHandling = "SELECT FKCampaignID [FKCampaignID], Objectionhandling [Objectionhandling] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + "";

                    //dt = Methods.GetTableData(
                    //        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                    if (strSQLObjectionHandling != null)
                    {

                        dtObjectionHandling = Methods.GetTableData(strSQLObjectionHandling);

                        if (dtObjectionHandling.Rows[0]["Objectionhandling"] != null)
                        {
                            CampaignID = dtObjectionHandling.Rows[0]["FKCampaignID"] as string;

                            byte[] data = dtObjectionHandling.Rows[0]["Objectionhandling"] as byte[];

                            if (data != null)
                            {

                                try
                                {
                                    _strXpsDoc = Convert.ToString(data);

                                    FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                                    objFileStream.Write(data, 0, data.Length);
                                    objFileStream.Close();

                                    _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                                    dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();
                                    _xpsDocument.Close();

                                }
                                catch (Exception ex)
                                {
                                    HandleException(ex);
                                }

                            }

                            try { dtObjectionHandling.Clear(); } catch { }


                        }

                        strSQLObjectionHandling = null;
                    }

                    #endregion
                }

                else if (CampaignNotesID == "6")
                {

                    #region Need Creation

                    string strSQLNeedCreation = "SELECT FKCampaignID [FKCampaignID], NeedCreation [NeedCreation] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + "";

                    //dt = Methods.GetTableData(
                    //        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                    if (strSQLNeedCreation != null)
                    {

                        dtNeedCreation = Methods.GetTableData(strSQLNeedCreation);

                        if (dtNeedCreation.Rows[0]["NeedCreation"] != null)
                        {
                            CampaignID = dtNeedCreation.Rows[0]["FKCampaignID"] as string;

                            byte[] data = dtNeedCreation.Rows[0]["NeedCreation"] as byte[];

                            if (data != null)
                            {

                                try
                                {
                                    _strXpsDoc = Convert.ToString(data);

                                    FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                                    objFileStream.Write(data, 0, data.Length);
                                    objFileStream.Close();

                                    _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                                    dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();
                                    _xpsDocument.Close();
                                }
                                catch (Exception ex) 
                                {
                                    HandleException(ex); 
                                }

                            }

                            data = null;

                            try { dtNeedCreation.Clear(); } catch { }


                        }


                        strSQLNeedCreation = null;
                    }

                    #endregion
                }
            }




            catch (Exception ex)
            {
                HandleException(ex);
            }


}



        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {

            OnDialogClose(_dialogResult);

            MySuccess mySuccess = new MySuccess();
            mySuccess.Body.Visibility = Visibility.Collapsed;
            mySuccess.Body2.Visibility = Visibility.Visible;
            mySuccess.LoadAgentCalls();
            mySuccess.LoadAgentNotesDG();
            mySuccess.LoadCampaignNotesDG();
            ShowDialog(mySuccess, new INDialogWindow(mySuccess));

            MySuccessCampaignNotes mySuccessCampaignNotes = new MySuccessCampaignNotes(null); 
            
            

            //mySuccess.Body2.Visibility = Visibility.Visible;
            //mySuccess.LoadCampaignNotesDG();
        }

        private void dvClosure_LayoutUpdated(object sender, EventArgs e)
        {

        }


        private void BaseControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void chkClosureAfrikaans_Checked(object sender, RoutedEventArgs e)
        {
            LoadDocument();
        }

        private void chkClosureAfrikaans_Unchecked(object sender, RoutedEventArgs e)
        {
            //LoadDocument(); 
        }

        private void chkClosureEnglish_Checked(object sender, RoutedEventArgs e)
        {
            LoadDocument(); 
        }

        private void chkClosureEnglish_Unchecked(object sender, RoutedEventArgs e)
        {
            //LoadDocument();
        }
    }
}
