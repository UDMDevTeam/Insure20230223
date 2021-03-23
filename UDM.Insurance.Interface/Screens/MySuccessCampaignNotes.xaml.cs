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
        string campaignID;
        string agentNotesID;
        public MySuccessCampaignNotes(string CampaignID, string AgentNotesID)
        {
            InitializeComponent();
            campaignID = CampaignID;
            agentNotesID = AgentNotesID;
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
                System.Data.DataTable dtClosure;
                System.Data.DataTable dtScript;
                string CampaignID;


                if (agentNotesID == "1")
                {

                    ////dt = Methods.GetTableData(
                    //        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");
                    string strSQLScript = "SELECT FKCampaignID [FKCampaignID], Tone [Tone] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + campaignID + "";

                    if (strSQLScript != null)
                    {

                        dtScript = Methods.GetTableData(strSQLScript);

                        if (dtScript != null && dtScript.Rows.Count == 1)
                        {
                            CampaignID = dtScript.Rows[0]["FKCampaignID"] as string;

                            byte[] data = dtScript.Rows[0]["Tone"] as byte[];

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
                }
                else if(agentNotesID == "2")
                {
                    string strSQLScript = "SELECT FKCampaignID [FKCampaignID], Tips [Tips] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + campaignID + "";

                    if (strSQLScript != null)
                    {

                        dtScript = Methods.GetTableData(strSQLScript);

                        if (dtScript != null && dtScript.Rows.Count == 1)
                        {
                            CampaignID = dtScript.Rows[0]["FKCampaignID"] as string;

                            byte[] data = dtScript.Rows[0]["Tips"] as byte[];

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
                }
                else if(agentNotesID == "3")
                {
                    string strSQLScript = "SELECT FKCampaignID [FKCampaignID], MessagesFromAgent [Tips] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + campaignID + "";

                    if (strSQLScript != null)
                    {

                        dtScript = Methods.GetTableData(strSQLScript);

                        if (dtScript != null && dtScript.Rows.Count == 1)
                        {
                            CampaignID = dtScript.Rows[0]["FKCampaignID"] as string;

                            byte[] data = dtScript.Rows[0]["Tips"] as byte[];

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
                }


                #region Closure
                string strSQLClosure = "SELECT FKCampaignID [FKCampaignID], ClosureEng [ClosureEng] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + campaignID + "";

                if (strSQLClosure != null)
                {

                    dtClosure = Methods.GetTableData(strSQLClosure);

                    if (dtClosure != null && dtClosure.Rows.Count == 1)
                    {
                        campaignID = dtClosure.Rows[0]["FKCampaignID"] as string;

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


                #endregion

                //#region Scripts



                //string strSQLScript = "SELECT FKCampaignID [FKCampaignID], ClosureEng [ClosureEng] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + "";

                ////dt = Methods.GetTableData(
                ////        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                //if (strSQLScript != null)
                //{

                //    dtScript = Methods.GetTableData(strSQLScript);

                //    if (dtScript != null && dtScript.Rows.Count == 1)
                //    {
                //        CampaignID = dtScript.Rows[0]["FKCampaignID"] as string;

                //        byte[] data = dtScript.Rows[0]["ClosureEng"] as byte[];

                //        if (data != null)
                //        {


                //            _strXpsDoc = Convert.ToString(data);

                //            FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                //            objFileStream.Write(data, 0, data.Length);
                //            objFileStream.Close();

                //            _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                //            dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();

                //            objFileStream.Close();
                //            objFileStream.Dispose();

                //        }
                //    }

                //    strSQLScript = null;
                //}

                //#endregion


                //#region Objection Handling

                //string strSQLObjectionHandling = "SELECT FKCampaignID [FKCampaignID], ClosureEng [ClosureEng] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + "";

                ////dt = Methods.GetTableData(
                ////        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                //if (strSQLScript != null)
                //{

                //    dtScript = Methods.GetTableData(strSQLScript);

                //    if (dtScript != null && dtScript.Rows.Count == 1)
                //    {
                //        CampaignID = dtScript.Rows[0]["FKCampaignID"] as string;

                //        byte[] data = dtScript.Rows[0]["ClosureEng"] as byte[];

                //        if (data != null)
                //        {


                //            _strXpsDoc = Convert.ToString(data);

                //            FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                //            objFileStream.Write(data, 0, data.Length);
                //            objFileStream.Close();

                //            _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                //            dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();

                //            objFileStream.Close();
                //            objFileStream.Dispose();

                //        }
                //    }

                //    strSQLObjectionHandling = null;
                //}

                //#endregion

                //#region Options

                //string strSQLOptions = "SELECT FKCampaignID [FKCampaignID], ClosureEng [ClosureEng] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + "";

                ////dt = Methods.GetTableData(
                ////        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                //if (strSQLOptions != null)
                //{

                //    dtScript = Methods.GetTableData(strSQLOptions);

                //    if (dtScript != null && dtScript.Rows.Count == 1)
                //    {
                //        CampaignID = dtScript.Rows[0]["FKCampaignID"] as string;

                //        byte[] data = dtScript.Rows[0]["ClosureEng"] as byte[];

                //        if (data != null)
                //        {


                //            _strXpsDoc = Convert.ToString(data);

                //            FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                //            objFileStream.Write(data, 0, data.Length);
                //            objFileStream.Close();

                //            _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                //            dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();

                //            objFileStream.Close();
                //            objFileStream.Dispose();

                //        }
                //    }

                //    strSQLOptions = null;
                //}

                //#endregion

                //#region Covers

                //string strSQLCovers = "SELECT FKCampaignID [FKCampaignID], ClosureEng [ClosureEng] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + "";

                ////dt = Methods.GetTableData(
                ////        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                //if (strSQLCovers != null)
                //{

                //    dtScript = Methods.GetTableData(strSQLScript);

                //    if (dtScript != null && dtScript.Rows.Count == 1)
                //    {
                //        CampaignID = dtScript.Rows[0]["FKCampaignID"] as string;

                //        byte[] data = dtScript.Rows[0]["ClosureEng"] as byte[];

                //        if (data != null)
                //        {


                //            _strXpsDoc = Convert.ToString(data);

                //            FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                //            objFileStream.Write(data, 0, data.Length);
                //            objFileStream.Close();

                //            _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                //            dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();

                //            objFileStream.Close();
                //            objFileStream.Dispose();

                //        }
                //    }

                //    strSQLCovers = null;
                //}

                //#endregion

                //#region Incentive Structure

                //string strSQLIncentiveStructure = "SELECT FKCampaignID [FKCampaignID], ClosureEng [ClosureEng] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + "";

                ////dt = Methods.GetTableData(
                ////        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                //if (strSQLIncentiveStructure != null)
                //{

                //    dtScript = Methods.GetTableData(strSQLIncentiveStructure);

                //    if (dtScript != null && dtScript.Rows.Count == 1)
                //    {
                //        CampaignID = dtScript.Rows[0]["FKCampaignID"] as string;

                //        byte[] data = dtScript.Rows[0]["ClosureEng"] as byte[];

                //        if (data != null)
                //        {


                //            _strXpsDoc = Convert.ToString(data);

                //            FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                //            objFileStream.Write(data, 0, data.Length);
                //            objFileStream.Close();

                //            _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                //            dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();

                //            objFileStream.Close();
                //            objFileStream.Dispose();

                //        }
                //    }

                //    strSQLIncentiveStructure = null;
                //}

                //#endregion

                //#region Need Creation

                //string strSQLNeedCreation = "SELECT FKCampaignID [FKCampaignID], ClosureEng [ClosureEng] FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + "";

                ////dt = Methods.GetTableData(
                ////        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                //if (strSQLNeedCreation != null)
                //{

                //    dtScript = Methods.GetTableData(strSQLNeedCreation);

                //    if (dtScript != null && dtScript.Rows.Count == 1)
                //    {
                //        CampaignID = dtScript.Rows[0]["FKCampaignID"] as string;

                //        byte[] data = dtScript.Rows[0]["ClosureEng"] as byte[];

                //        if (data != null)
                //        {


                //            _strXpsDoc = Convert.ToString(data);

                //            FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                //            objFileStream.Write(data, 0, data.Length);
                //            objFileStream.Close();

                //            _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                //            dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();

                //            objFileStream.Close();
                //            objFileStream.Dispose();

                //        }
                //    }

                //    strSQLNeedCreation = null;
                //}

                //#endregion
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
