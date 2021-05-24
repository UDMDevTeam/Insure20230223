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
    public partial class MySuccessAgentNotes
    {

        string AgentNotesID;
        public MySuccessAgentNotes(string _AgentNoteIDs)
        {
            InitializeComponent();

            AgentNotesID = _AgentNoteIDs;

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

                System.Data.DataTable dtTone;
                System.Data.DataTable dtSellingApproach;
                System.Data.DataTable dtTips;
                System.Data.DataTable dtTechniques;
                System.Data.DataTable dtMessagesFromAgent;

                string CampaignID = GlobalSettings.CampaignID;
                GlobalSettings.AgentNotesID = AgentNotesID;

                if (AgentNotesID == "1")
                {
                    #region Tone



                    string strSQLTone = "SELECT FKCampaignID [FKCampaignID], Tone [Tone] FROM INMySuccessAgentsNotesDetails WHERE FKCampaignID = " + CampaignID + "";

                    //dt = Methods.GetTableData(
                    //        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                    if (strSQLTone != null)
                    {

                        dtTone = Methods.GetTableData(strSQLTone);

                        if (dtTone.Rows[0]["Tone"] != null)
                        {
                            CampaignID = dtTone.Rows[0]["FKCampaignID"] as string;

                            byte[] data = dtTone.Rows[0]["Tone"] as byte[];

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

                        strSQLTone = null;
                    }

                    #endregion

                }
                else if (AgentNotesID == "2")
                {
                    #region SellingApproach

                    string strSQLSellingApproach = "SELECT FKCampaignID [FKCampaignID], SellingApproach [SellingApproach] FROM INMySuccessAgentsNotesDetails WHERE FKCampaignID = " + CampaignID + "";

                    //dt = Methods.GetTableData(
                    //        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                    if (strSQLSellingApproach != null)
                    {

                        dtSellingApproach = Methods.GetTableData(strSQLSellingApproach);

                        if (dtSellingApproach.Rows[0]["SellingApproach"] != null)
                        {
                            CampaignID = dtSellingApproach.Rows[0]["FKCampaignID"] as string;

                            byte[] data = dtSellingApproach.Rows[0]["SellingApproach"] as byte[];

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

                    strSQLSellingApproach = null;
                }

                #endregion


                else if (AgentNotesID == "3")
                {
                    #region Tips

                    string strSQLTips = "SELECT FKCampaignID [FKCampaignID], Tips [Tips] FROM INMySuccessAgentsNotesDetails WHERE FKCampaignID = " + CampaignID + "";

                    //dt = Methods.GetTableData(
                    //        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                    if (strSQLTips != null)
                    {

                        dtTips = Methods.GetTableData(strSQLTips);

                        if (dtTips != null)
                        {
                            CampaignID = dtTips.Rows[0]["FKCampaignID"] as string;

                            byte[] data = dtTips.Rows[0]["Tips"] as byte[];

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

                        strSQLTips = null;
                    }

                    #endregion

                }

                else if (AgentNotesID == "4")
                {
                    #region Techniques

                    string strSQLTechniques = "SELECT FKCampaignID [FKCampaignID], Techniques [Techniques] FROM INMySuccessAgentsNotesDetails WHERE FKCampaignID = " + CampaignID + "";

                    //dt = Methods.GetTableData(
                    //        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                    if (strSQLTechniques != null)
                    {

                        dtTechniques = Methods.GetTableData(strSQLTechniques);

                        if (dtTechniques.Rows[0]["Techniques"] != null)
                        {
                            CampaignID = dtTechniques.Rows[0]["FKCampaignID"] as string;

                            byte[] data = dtTechniques.Rows[0]["Techniques"] as byte[];

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

                        strSQLTechniques = null;
                    }

                    #endregion

                }
                else if (AgentNotesID == "5")
                {
                    #region MessagesFromAgent

                    string strSQLMessagesFromAgent = "SELECT FKCampaignID [FKCampaignID], MessagesFromAgent [MessagesFromAgent] FROM INMySuccessAgentsNotesDetails WHERE FKCampaignID = " + CampaignID + "";

                    //dt = Methods.GetTableData(
                    //        "SELECT FKCampaignID, ClosureEng FROM INMySuccessCampaignDetails WHERE FKCampaignID = " + CampaignID + ")");

                    if (strSQLMessagesFromAgent != null)
                    {

                        dtMessagesFromAgent = Methods.GetTableData(strSQLMessagesFromAgent);

                        if (dtMessagesFromAgent.Rows[0]["MessagesFromAgent"] != null)
                        {
                            CampaignID = dtMessagesFromAgent.Rows[0]["FKCampaignID"] as string;

                            byte[] data = dtMessagesFromAgent.Rows[0]["MessagesFromAgent"] as byte[];

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

                        strSQLMessagesFromAgent = null;
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
