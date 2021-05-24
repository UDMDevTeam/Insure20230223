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

                            if (data != null)
                            {


                                _strXpsDoc = Convert.ToString(data);

                                FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                                objFileStream.Write(data, 0, data.Length);
                                objFileStream.Close();

                                _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                                dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();

                                objFileStream.Close();
                                objFileStream.Dispose();

                            }
                        }

                        strSQLScript = null;
                    }

                    #endregion

                }
                else if (CampaignNotesID == "2")
                {
                  #region Closure

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


                                    _strXpsDoc = Convert.ToString(data);

                                    FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                                    objFileStream.Write(data, 0, data.Length);
                                    objFileStream.Close();

                                    _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                                    dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();

                                    objFileStream.Close();
                                    objFileStream.Dispose();

                                }
                            }


                        }

                        strSQLClosure = null;
                    }

                    #endregion

                
                else if (CampaignNotesID == "3")
                {   
                    #region Options

                    string strSQLOptions = "SELECT FKCampaignID [FKCampaignID], Options [Options] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + "";

                    //dt = Methods.GetTableData(
                    //        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                    if (strSQLOptions != null)
                    {

                        dtOptions = Methods.GetTableData(strSQLOptions);

                        if (dtOptions != null && dtOptions.Rows.Count == 1)
                        {
                            CampaignID = dtOptions.Rows[0]["FKCampaignID"] as string;

                            byte[] data = dtOptions.Rows[0]["Options"] as byte[];

                            if (data != null)
                            {


                                _strXpsDoc = Convert.ToString(data);

                                FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                                objFileStream.Write(data, 0, data.Length);
                                objFileStream.Close();

                                _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                                dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();

                                objFileStream.Close();
                                objFileStream.Dispose();

                            }
                        }

                        strSQLOptions = null;
                    }

                    #endregion

                    #region Covers

                    string strSQLCovers = "SELECT FKCampaignID [FKCampaignID], Covers [Covers] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + "";

                    //dt = Methods.GetTableData(
                    //        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                    if (strSQLCovers != null)
                    {

                        dtCovers = Methods.GetTableData(strSQLCovers);

                        if (dtCovers.Rows[0]["Covers"] != null)
                        {
                            CampaignID = dtCovers.Rows[0]["FKCampaignID"] as string;

                            byte[] data = dtCovers.Rows[0]["Covers"] as byte[];

                            if (data != null)
                            {

                                _strXpsDoc = Convert.ToString(data);

                                FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                                objFileStream.Write(data, 0, data.Length);
                                objFileStream.Close();

                                _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                                dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();

                                objFileStream.Close();
                                objFileStream.Dispose();

                            }
                        }

                        strSQLCovers = null;
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


                                _strXpsDoc = Convert.ToString(data);

                                FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                                objFileStream.Write(data, 0, data.Length);
                                objFileStream.Close();

                                _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                                dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();

                                objFileStream.Close();
                                objFileStream.Dispose();

                            }
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


                                _strXpsDoc = Convert.ToString(data);

                                FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                                objFileStream.Write(data, 0, data.Length);
                                objFileStream.Close();

                                _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                                dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();

                                objFileStream.Close();
                                objFileStream.Dispose();

                            }
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


                                _strXpsDoc = Convert.ToString(data);

                                FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                                objFileStream.Write(data, 0, data.Length);
                                objFileStream.Close();

                                _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                                dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();

                                objFileStream.Close();
                                objFileStream.Dispose();

                            }
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
            

        }

        private void dvClosure_LayoutUpdated(object sender, EventArgs e)
        {

        }

        private void BaseControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
