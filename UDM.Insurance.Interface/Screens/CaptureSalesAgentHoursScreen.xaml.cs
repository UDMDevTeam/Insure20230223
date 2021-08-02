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

namespace UDM.Insurance.Interface.Screens
{

    public partial class CaptureSalesAgentHoursScreen
    {

        #region Private Data Members

        private bool isFirstLoad = true;
        private long _shiftTypeID;
        private long _userTypeID;
        private long _selectedAgentID = -1;
        private long _selectedCampaignId = -1;
        private long _selectedUserHourID = -1;
        private readonly long UserHourID;
        private bool _isPublicHoliday;
        private bool _isRedeemableGiftCampaign;

        #endregion Private Data Members

        #region Constructor

        public CaptureSalesAgentHoursScreen(long userHourID)
        {
            InitializeComponent();


            if(GlobalSettings.ApplicationUser.ID == 1 || GlobalSettings.ApplicationUser.ID == 1016 || GlobalSettings.ApplicationUser.ID == 6091 || GlobalSettings.ApplicationUser.ID == 7194 || GlobalSettings.ApplicationUser.ID == 45 || GlobalSettings.ApplicationUser.ID == 394
                || GlobalSettings.ApplicationUser.ID == 403 || GlobalSettings.ApplicationUser.ID == 2232 || GlobalSettings.ApplicationUser.ID == 2767 || GlobalSettings.ApplicationUser.ID == 2810 || GlobalSettings.ApplicationUser.ID == 2857 || GlobalSettings.ApplicationUser.ID == 3165
                || GlobalSettings.ApplicationUser.ID == 3388 || GlobalSettings.ApplicationUser.ID == 6181 || GlobalSettings.ApplicationUser.ID == 7206 || GlobalSettings.ApplicationUser.ID == 40416)
            {
                ConfirmationBody.Visibility = Visibility.Visible;
                Body.Visibility = Visibility.Collapsed;

                userHourID = GlobalSettings.ApplicationUser.ID; 

                if (userHourID > 0)
                {
                    UserHourID = userHourID;

                    LoadConfirmationAgentsScreen();

                    _selectedUserHourID = UserHourID;

                    DataRow selectedItem = Methods.GetTableData("select * from UserHours where ID = " + userHourID).Rows[0];

                    #region Morning Shift
                    string morningShiftStart = selectedItem["MorningShiftStartTime"].ToString();
                    string morningShiftEnd = selectedItem["MorningShiftEndTime"].ToString();

                    if (morningShiftStart != string.Empty)
                    {
                        dteMorningShiftStartTime.Value = morningShiftStart;
                        dteConfirmationMorningShiftStartTime.Value = morningShiftStart;
                    }
                    else
                    {
                        dteMorningShiftStartTime.Value = null;
                        dteConfirmationMorningShiftStartTime.Value = null;
                    }
                    if (morningShiftEnd != string.Empty)
                    {
                        dteMorningShiftEndTime.Value = morningShiftEnd;
                        dteConfirmationMorningShiftEndTime.Value = morningShiftEnd;
                    }
                    else
                    {
                        dteMorningShiftEndTime.Value = null;
                        dteConfirmationMorningShiftEndTime.Value = null;
                    }
                    #endregion Morning Shift
                    #region Normal Shift
                    string normalShiftStart = selectedItem["NormalShiftStartTime"].ToString();
                    string normalShiftEnd = selectedItem["NormalShiftEndTime"].ToString();
                    if (normalShiftStart != string.Empty)
                    {
                        dteNormalShiftStartTime.Value = normalShiftStart;
                    }
                    else
                    {
                        dteNormalShiftStartTime.Value = null;
                    }
                    if (normalShiftEnd != string.Empty)
                    {
                        dteNormalShiftEndTime.Value = normalShiftEnd;
                    }
                    else
                    {
                        dteNormalShiftEndTime.Value = null;
                    }
                    #endregion Normal Shift
                    #region evening shift
                    string eveningShiftStart = selectedItem["EveningShiftStartTime"].ToString();
                    string eveningShiftEnd = selectedItem["EveningShiftEndTime"].ToString();
                    if (eveningShiftStart != string.Empty)
                    {
                        dteEveningShiftStartTime.Value = eveningShiftStart;
                    }
                    else
                    {
                        dteEveningShiftStartTime.Value = null;
                    }
                    if (eveningShiftEnd != string.Empty)
                    {

                        dteEveningShiftEndTime.Value = eveningShiftEnd;
                    }
                    else
                    {
                        dteEveningShiftEndTime.Value = null;
                    }
                    #endregion evening shift
                    #region weekend and public holiday
                    string weekendPublicHolidayStart = selectedItem["PublicHolidayWeekendShiftStartTime"].ToString();
                    string weekendPublicHolidayEnd = selectedItem["PublicHolidayWeekendShiftEndTime"].ToString();
                    if (weekendPublicHolidayStart != string.Empty)
                    {
                        dteWeekendPublicHolidayShiftStartTime.Value = weekendPublicHolidayStart;
                    }
                    else
                    {
                        dteWeekendPublicHolidayShiftStartTime.Value = null;
                    }
                    if (weekendPublicHolidayEnd != string.Empty)
                    {
                        dteWeekendPublicHolidayShiftEndTime.Value = weekendPublicHolidayEnd;
                    }
                    else
                    {
                        dteWeekendPublicHolidayShiftEndTime.Value = null;
                    }
                    #endregion weekend and public holiday
                    lblEditMode.Visibility = Visibility.Visible;
                    imgEditMode.Visibility = Visibility.Visible;
                    btnStopEditing.Visibility = Visibility.Visible;
                    dteDate.Value = DateTime.Parse(selectedItem["WorkingDate"].ToString());


                    #region Campaign
                    cmbCampaigns.SelectedValue = Convert.ToString(selectedItem["FKINCampaignID"] as string);
                    #endregion Campaign

                    string shiftTypeID = selectedItem["FKShiftTypeID"].ToString();
                    if (shiftTypeID != string.Empty)
                    {
                        cmbConfirmationShiftType.SelectedValue = selectedItem["FKShiftTypeID"].ToString();
                    }
                    long userID = long.Parse(selectedItem["FKUserID"].ToString());
                    int index = 0;

                    //foreach (DataRowView item in txtboxConfirmationAgent.Text)
                    //{
                    //    long agentID;
                    //    agentID = long.Parse(item.Row[0].ToString());
                    //    if (agentID == userID)
                    //    {
                    //        cmbConfirmationAgents.SelectedIndex = index;
                    //        break;
                    //    }
                    //    index++;
                    //}


                }
            }
            else 
            {
                ConfirmationBody.Visibility = Visibility.Collapsed;
                Body.Visibility = Visibility.Visible;

                if (userHourID > 0)
                {
                    UserHourID = userHourID;
                    LoadAgentsScreen();

                    _selectedUserHourID = UserHourID;
                    DataRow selectedItem = Methods.GetTableData("select * from UserHours where ID = " + userHourID).Rows[0];

                    #region Morning Shift
                    string morningShiftStart = selectedItem["MorningShiftStartTime"].ToString();
                    string morningShiftEnd = selectedItem["MorningShiftEndTime"].ToString();

                    if (morningShiftStart != string.Empty)
                    {
                        dteMorningShiftStartTime.Value = morningShiftStart;
                        dteConfirmationMorningShiftStartTime.Value = morningShiftStart;
                    }
                    else
                    {
                        dteMorningShiftStartTime.Value = null;
                        dteConfirmationMorningShiftStartTime.Value = null;
                    }
                    if (morningShiftEnd != string.Empty)
                    {
                        dteMorningShiftEndTime.Value = morningShiftEnd;
                        dteConfirmationMorningShiftEndTime.Value = morningShiftEnd;
                    }
                    else
                    {
                        dteMorningShiftEndTime.Value = null;
                        dteConfirmationMorningShiftEndTime.Value = null;
                    }
                    #endregion Morning Shift
                    #region Normal Shift
                    string normalShiftStart = selectedItem["NormalShiftStartTime"].ToString();
                    string normalShiftEnd = selectedItem["NormalShiftEndTime"].ToString();
                    if (normalShiftStart != string.Empty)
                    {
                        dteNormalShiftStartTime.Value = normalShiftStart;
                    }
                    else
                    {
                        dteNormalShiftStartTime.Value = null;
                    }
                    if (normalShiftEnd != string.Empty)
                    {
                        dteNormalShiftEndTime.Value = normalShiftEnd;
                    }
                    else
                    {
                        dteNormalShiftEndTime.Value = null;
                    }
                    #endregion Normal Shift
                    #region evening shift
                    string eveningShiftStart = selectedItem["EveningShiftStartTime"].ToString();
                    string eveningShiftEnd = selectedItem["EveningShiftEndTime"].ToString();
                    if (eveningShiftStart != string.Empty)
                    {
                        dteEveningShiftStartTime.Value = eveningShiftStart;
                    }
                    else
                    {
                        dteEveningShiftStartTime.Value = null;
                    }
                    if (eveningShiftEnd != string.Empty)
                    {

                        dteEveningShiftEndTime.Value = eveningShiftEnd;
                    }
                    else
                    {
                        dteEveningShiftEndTime.Value = null;
                    }
                    #endregion evening shift
                    #region weekend and public holiday
                    string weekendPublicHolidayStart = selectedItem["PublicHolidayWeekendShiftStartTime"].ToString();
                    string weekendPublicHolidayEnd = selectedItem["PublicHolidayWeekendShiftEndTime"].ToString();
                    if (weekendPublicHolidayStart != string.Empty)
                    {
                        dteWeekendPublicHolidayShiftStartTime.Value = weekendPublicHolidayStart;
                    }
                    else
                    {
                        dteWeekendPublicHolidayShiftStartTime.Value = null;
                    }
                    if (weekendPublicHolidayEnd != string.Empty)
                    {
                        dteWeekendPublicHolidayShiftEndTime.Value = weekendPublicHolidayEnd;
                    }
                    else
                    {
                        dteWeekendPublicHolidayShiftEndTime.Value = null;
                    }
                    #endregion weekend and public holiday
                    lblEditMode.Visibility = Visibility.Visible;
                    imgEditMode.Visibility = Visibility.Visible;
                    btnStopEditing.Visibility = Visibility.Visible;
                    dteDate.Value = DateTime.Parse(selectedItem["WorkingDate"].ToString());


                    #region Campaign
                    cmbCampaigns.SelectedValue = Convert.ToString(selectedItem["FKINCampaignID"] as string);
                    #endregion Campaign

                    string shiftTypeID = selectedItem["FKShiftTypeID"].ToString();
                    if (shiftTypeID != string.Empty)
                    {
                        cmbShiftType.SelectedValue = selectedItem["FKShiftTypeID"].ToString();
                    }
                    long userID = long.Parse(selectedItem["FKUserID"].ToString());
                    int index = 0;

                    foreach (DataRowView item in cmbAgents.Items)
                    {
                        long agentID;
                        agentID = long.Parse(item.Row[0].ToString());
                        if (agentID == userID)
                        {
                            cmbAgents.SelectedIndex = index;
                            break;
                        }
                        index++;
                    }


                }
            }

            

        }

        #endregion Constructor

        #region Private Methods

        private void LoadAgentsScreen()
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@UserID", GlobalSettings.ApplicationUser.ID);
                DataSet dsAgentDetails = Methods.ExecuteStoredProcedure("SpSalesAgentHoursScreen", parameters);
                if (dsAgentDetails.Tables[3].Rows.Count > 0)//shift types
                {
                    cmbShiftType.Populate(dsAgentDetails.Tables[3], "Description", "ID");

                    //if (_userTypeID > 0)
                    //{

                    //}
                    //else//if (UserHourID == 0)
                    //{
                    //    cmbShiftType.SelectedIndex = 0;
                    //}
                }
                if (dsAgentDetails.Tables[0].Rows.Count > 0)//agents
                {
                    cmbAgents.Populate(dsAgentDetails.Tables[0], "AgentName", "ID");
                }
                if (dsAgentDetails.Tables[1].Rows.Count > 0)//campaigns
                {
                    cmbCampaigns.Populate(dsAgentDetails.Tables[1], "CamapaigName", "ID");
                }
                if (dsAgentDetails.Tables[2].Rows.Count > 0)
                {
                    _userTypeID = long.Parse(dsAgentDetails.Tables[2].Rows[0]["FKUserType"].ToString());
                    //if its a sales agent select current user
                    if (_userTypeID == 2)
                    {
                        dteDate.Value = DateTime.Now.Date;
                        cmbAgents.SelectedIndex = 0;//there will be only one agent name in this list
                        dteDate.IsEnabled = false;//agent cannot change date
                        cmbShiftType.IsEnabled = false;
                        cmbAgents.IsEnabled = false;
                    }
                }
                

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void LoadConfirmationAgentsScreen()
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@UserID", GlobalSettings.ApplicationUser.ID);
                DataSet dsAgentDetails = Methods.ExecuteStoredProcedure("SpConfirmationAgentHoursScreen", parameters);
                if (dsAgentDetails.Tables[2].Rows.Count > 0)//shift types
                {
                    cmbConfirmationShiftType.Populate(dsAgentDetails.Tables[2], "Description", "ID");

                    //if (_userTypeID > 0)
                    //{

                    //}
                    //else//if (UserHourID == 0)
                    //{
                    //    cmbShiftType.SelectedIndex = 0;
                    //}
                }
                if (dsAgentDetails.Tables[1].Rows.Count > 0)//agents
                {

                    //cmbConfirmationAgents.Populate(dsAgentDetails.Tables[1], "AgentName", "ID");

                    //cmbConfirmationAgents.SelectedValue = dsAgentDetails.Tables[1].Columns["AgentName"].ToString();

                    //cmbConfirmationAgents.Text = dsAgentDetails.Tables[1].Columns["AgentName"].ToString();

                    SqlParameter[] userParameters = new SqlParameter[1];
                    userParameters[0] = new SqlParameter("@userId", GlobalSettings.ApplicationUser.ID);
                    DataTable dtData = Methods.ExecuteStoredProcedure("sp_GetUserNameConfirmation", userParameters).Tables[0];
                    //double timeDiffRecordedNormalHours = 0;

                        //cmbConfirmationAgents.Populate(dsSingleAgentDetailsData.Tables[0], "AgentName", "ID");

                        //cmbConfirmationAgents.ItemsSource = 

                        string userName = dtData.Rows[0]["AgentName"].ToString();

                        txtboxConfirmationAgent.Text = userName; 

                        _selectedAgentID = Convert.ToInt64(dtData.Rows[0]["UserID"]);


                }
                if (dsAgentDetails.Tables[0].Rows.Count > 0)//campaigns
                {

                    cmbConfirmationCampaigns.Populate(dsAgentDetails.Tables[0], "CamapaigName", "ID");

                    //cmbConfirmationCampaigns.Items.Add("Admin");
                    //cmbConfirmationCampaigns.Items.Add("Base");
                    //cmbConfirmationCampaigns.Items.Add("Upgrade");


                }

                //if (dsAgentDetails.Tables[2].Rows.Count > 0)
                //{
                //    _userTypeID = long.Parse(dsAgentDetails.Tables[2].Rows[0]["FKUserType"].ToString());
                //    //if its a sales agent select current user
                //    if (_userTypeID == 2)
                //    {
                //        dteConfirmationDate.Value = DateTime.Now.Date;
                //        cmbConfirmationAgents.SelectedIndex = 0;
                //        //cmbConfirmationAgents.SelectedIndex = 0;//there will be only one agent name in this list
                //        dteConfirmationDate.IsEnabled = false;//agent cannot change date
                //        cmbConfirmationShiftType.IsEnabled = false;
                //        cmbConfirmationAgents.IsEnabled = false;
                //    }
                //}


            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void ShiftChange()
        {
            try
            {
                bool isPublicHoliday = false;
                bool isUdmHoliday = false;
                DateTime selectedDate = (DateTime)dteDate.Value;

                DataSet dsHolidays = Methods.ExecuteStoredProcedure("spGetHolidays", null);
                DataTable dtPublicHolidays = dsHolidays.Tables[0];
                DataTable dtUDMHolidays = dsHolidays.Tables[1];

                #region Checking if the selected date is a public holiday

                //check if selected date is public holiday
                var pubHoliday = dtPublicHolidays.AsEnumerable().Where(x => (DateTime)x["Date"] == selectedDate.Date);
                foreach (var holiday in pubHoliday)//this will loop once because only onde day can be a holiday
                {
                    isPublicHoliday = true;

                    lblDescriptionText.Text = holiday["Description"] + " :" + selectedDate.DayOfWeek;
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@ShiftTypeID", _shiftTypeID);
                    parameters[1] = new SqlParameter("@ShiftID", 4);  //weekend or public holiday shift
                    DataTable dtshiftHours = Methods.ExecuteStoredProcedure("SpGetShiftTimes", parameters).Tables[0];

                    if (dtshiftHours.Rows.Count > 0)
                    {
                        string startTime = dtshiftHours.Rows[0]["StartTime"].ToString();
                        string endTime = dtshiftHours.Rows[0]["EndTime"].ToString();
                        if (startTime != string.Empty)
                        {
                            dteWeekendPublicHolidayShiftStartTime.Value = startTime;
                        }
                        else
                        {
                            dteWeekendPublicHolidayShiftStartTime.Value = null;
                        }
                        if (endTime != string.Empty)
                        {
                            dteWeekendPublicHolidayShiftEndTime.Value = endTime;
                        }
                        else
                        {
                            dteWeekendPublicHolidayShiftEndTime.Value = null;
                        }

                        dteNormalShiftStartTime.Value = null;
                        dteNormalShiftEndTime.Value = null;
                        dteMorningShiftStartTime.Value = null;
                        dteMorningShiftEndTime.Value = null;
                        dteEveningShiftStartTime.Value = null;
                        dteEveningShiftEndTime.Value = null;
                        dteMorningShiftEndTime.Value = null;
                    }
                }
                _isPublicHoliday = isPublicHoliday;

                #endregion Checking if the selected date is a public holiday

                #region Checking if the selected date is a UDM holiday

                //check if selected date is company recess
                var udmHoliday = dtUDMHolidays.AsEnumerable().Where(x => (DateTime)x["Date"] == selectedDate.Date);
                foreach (var unused in udmHoliday)
                {
                    isUdmHoliday = true;
                    lblDescriptionText.Text = "Company Recess: " + selectedDate.DayOfWeek;
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@ShiftTypeID", _shiftTypeID);
                    parameters[1] = new SqlParameter("@ShiftID", 4);  //weekend or public holiday shift
                    DataTable dtshiftHours = Methods.ExecuteStoredProcedure("SpGetShiftTimes", parameters).Tables[0];

                    if (dtshiftHours.Rows.Count > 0)
                    {
                        string startTime = dtshiftHours.Rows[0]["StartTime"].ToString();
                        string endTime = dtshiftHours.Rows[0]["EndTime"].ToString();
                        if (startTime != string.Empty)
                        {
                            dteWeekendPublicHolidayShiftStartTime.Value = startTime;
                        }
                        else
                        {
                            dteWeekendPublicHolidayShiftStartTime.Value = null;
                        }
                        if (endTime != string.Empty)
                        {
                            dteWeekendPublicHolidayShiftEndTime.Value = endTime;
                        }
                        else
                        {
                            dteWeekendPublicHolidayShiftEndTime.Value = null;
                        }

                        dteNormalShiftStartTime.Value = null;
                        dteNormalShiftEndTime.Value = null;
                        dteMorningShiftStartTime.Value = null;
                        dteMorningShiftEndTime.Value = null;
                        dteEveningShiftStartTime.Value = null;
                        dteEveningShiftEndTime.Value = null;
                        dteMorningShiftEndTime.Value = null;
                    }
                }

                #endregion Checking if the selected date is a UDM holiday

                if (isPublicHoliday == false && isUdmHoliday == false)
                {
                    #region Friday

                    if (selectedDate.DayOfWeek == DayOfWeek.Friday)
                    {
                        lblDescriptionText.Text = selectedDate.DayOfWeek.ToString(); //"Normal Business Day: " + 
                        //get shift type hours
                        SqlParameter[] parameters = new SqlParameter[2];
                        parameters[0] = new SqlParameter("@ShiftTypeID", _shiftTypeID);
                        long shift = 5;
                        if (_shiftTypeID == 1)
                        {
                            shift = 7;
                        }
                        else if (_shiftTypeID == 3)
                        {
                            shift = 10;
                        }
                        parameters[1] = new SqlParameter("@ShiftID", shift);  //friday shift
                        DataTable dtshiftHours = Methods.ExecuteStoredProcedure("SpGetShiftTimes", parameters).Tables[0];

                        if (dtshiftHours.Rows.Count > 0)
                        {
                            string startTime = dtshiftHours.Rows[0]["StartTime"].ToString();
                            string endTime = dtshiftHours.Rows[0]["EndTime"].ToString();
                            if (startTime != string.Empty)
                            {
                                dteNormalShiftStartTime.Value = startTime;
                            }
                            else
                            {
                                dteNormalShiftStartTime.Value = null;
                            }
                            if (endTime != string.Empty)
                            {
                                dteNormalShiftEndTime.Value = endTime;
                            }
                            else
                            {
                                dteNormalShiftEndTime.Value = null;
                            }
                            dteMorningShiftStartTime.Value = null;
                            dteMorningShiftEndTime.Value = null;
                            dteEveningShiftStartTime.Value = null;
                            dteEveningShiftEndTime.Value = null;
                            dteMorningShiftEndTime.Value = null;
                            dteWeekendPublicHolidayShiftStartTime.Value = null;
                            dteWeekendPublicHolidayShiftEndTime.Value = null;
                        }
                    }

                    #endregion Friday

                    #region Normal Working Days - Monday through Thursday

                    if (selectedDate.DayOfWeek != DayOfWeek.Friday && selectedDate.DayOfWeek != DayOfWeek.Saturday && selectedDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        lblDescriptionText.Text = selectedDate.DayOfWeek.ToString(); //"Normal Business Day: " + 
                        //get shift type hours
                        int shiftID = 2;//normal Shift
                        if (_shiftTypeID == 1)
                        {
                            shiftID = 6;
                        }
                        else if (_shiftTypeID == 3)
                        {
                            shiftID = 8;
                        }
                        SqlParameter[] parameters = new SqlParameter[2];
                        parameters[0] = new SqlParameter("@ShiftTypeID", _shiftTypeID);
                        parameters[1] = new SqlParameter("@ShiftID", shiftID);  //normal shift
                        DataTable dtshiftHours = Methods.ExecuteStoredProcedure("SpGetShiftTimes", parameters).Tables[0];

                        if (dtshiftHours.Rows.Count > 0)
                        {
                            string startTime = dtshiftHours.Rows[0]["StartTime"].ToString();
                            string endTime = dtshiftHours.Rows[0]["EndTime"].ToString();
                            if (startTime != string.Empty)
                            {
                                dteNormalShiftStartTime.Value = startTime;
                            }
                            else
                            {
                                dteNormalShiftStartTime.Value = null;
                            }
                            if (endTime != string.Empty)
                            {
                                dteNormalShiftEndTime.Value = endTime;
                            }
                            else
                            {
                                dteNormalShiftEndTime.Value = null;
                            }
                            dteMorningShiftStartTime.Value = null;
                            dteMorningShiftEndTime.Value = null;
                            dteEveningShiftStartTime.Value = null;
                            dteEveningShiftEndTime.Value = null;
                            dteMorningShiftEndTime.Value = null;
                            dteWeekendPublicHolidayShiftStartTime.Value = null;
                            dteWeekendPublicHolidayShiftEndTime.Value = null;
                        }
                    }

                    #endregion Normal Working Days - Monday through Thursday

                    #region Weekends

                    else
                    {
                        if (selectedDate.DayOfWeek != DayOfWeek.Friday)
                        {
                            lblDescriptionText.Text = selectedDate.DayOfWeek.ToString(); //"Weekend: " + 
                            SqlParameter[] parameters = new SqlParameter[2];
                            parameters[0] = new SqlParameter("@ShiftTypeID", _shiftTypeID);
                            parameters[1] = new SqlParameter("@ShiftID", 4);  //weekend shift
                            DataTable dtshiftHours = Methods.ExecuteStoredProcedure("SpGetShiftTimes", parameters).Tables[0];

                            if (dtshiftHours.Rows.Count > 0)
                            {
                                string startTime = dtshiftHours.Rows[0]["StartTime"].ToString();
                                string endTime = dtshiftHours.Rows[0]["EndTime"].ToString();
                                if (startTime != string.Empty)
                                {
                                    dteWeekendPublicHolidayShiftStartTime.Value = startTime;
                                }
                                else
                                {
                                    dteWeekendPublicHolidayShiftStartTime.Value = null;
                                }
                                if (endTime != string.Empty)
                                {
                                    dteWeekendPublicHolidayShiftEndTime.Value = endTime;
                                }
                                else
                                {
                                    dteWeekendPublicHolidayShiftEndTime.Value = null;
                                }

                                dteNormalShiftStartTime.Value = null;
                                dteNormalShiftEndTime.Value = null;
                                dteMorningShiftStartTime.Value = null;
                                dteMorningShiftEndTime.Value = null;
                                dteEveningShiftStartTime.Value = null;
                                dteEveningShiftEndTime.Value = null;
                                dteMorningShiftEndTime.Value = null;
                            }
                        }
                    }

                    #endregion Weekends

                }

                LoadAgentShiftTimes();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ConfirmationShiftChange()
        {
            try
            {
                bool isPublicHoliday = false;
                bool isUdmHoliday = false;
                DateTime selectedDate = (DateTime)dteConfirmationDate.Value;

                DataSet dsHolidays = Methods.ExecuteStoredProcedure("spGetHolidays", null);
                DataTable dtPublicHolidays = dsHolidays.Tables[0];
                DataTable dtUDMHolidays = dsHolidays.Tables[1];

                #region Checking if the selected date is a public holiday

                //check if selected date is public holiday
                var pubHoliday = dtPublicHolidays.AsEnumerable().Where(x => (DateTime)x["Date"] == selectedDate.Date);
                foreach (var holiday in pubHoliday)//this will loop once because only onde day can be a holiday
                {
                    isPublicHoliday = true;

                    lblDescriptionText.Text = holiday["Description"] + " :" + selectedDate.DayOfWeek;
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@ShiftTypeID", _shiftTypeID);
                    parameters[1] = new SqlParameter("@ShiftID", 4);  //weekend or public holiday shift
                    DataTable dtshiftHours = Methods.ExecuteStoredProcedure("SpGetShiftTimes", parameters).Tables[0];

                    if (dtshiftHours.Rows.Count > 0)
                    {
                        string startTime = dtshiftHours.Rows[0]["StartTime"].ToString();
                        string endTime = dtshiftHours.Rows[0]["EndTime"].ToString();
                        if (startTime != string.Empty)
                        {
                            dteConfirmationWeekendPublicHolidayShiftStartTime.Value = startTime;
                        }
                        else
                        {
                            dteConfirmationWeekendPublicHolidayShiftStartTime.Value = null;
                        }
                        if (endTime != string.Empty)
                        {
                            dteConfirmationWeekendPublicHolidayShiftEndTime.Value = endTime;
                        }
                        else
                        {
                            dteConfirmationWeekendPublicHolidayShiftEndTime.Value = null;
                        }

                        dteConfirmationNormalShiftStartTime.Value = null;
                        dteConfirmationNormalShiftEndTime.Value = null;
                        dteConfirmationMorningShiftStartTime.Value = null;
                        dteConfirmationMorningShiftEndTime.Value = null;
                        dteConfirmationEveningShiftStartTime.Value = null;
                        dteConfirmationEveningShiftEndTime.Value = null;
                        dteConfirmationMorningShiftEndTime.Value = null;
                    }
                }
                _isPublicHoliday = isPublicHoliday;

                #endregion Checking if the selected date is a public holiday

                #region Checking if the selected date is a UDM holiday

                //check if selected date is company recess
                var udmHoliday = dtUDMHolidays.AsEnumerable().Where(x => (DateTime)x["Date"] == selectedDate.Date);
                foreach (var unused in udmHoliday)
                {
                    isUdmHoliday = true;
                    lblDescriptionText.Text = "Company Recess: " + selectedDate.DayOfWeek;
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@ShiftTypeID", _shiftTypeID);
                    parameters[1] = new SqlParameter("@ShiftID", 4);  //weekend or public holiday shift
                    DataTable dtshiftHours = Methods.ExecuteStoredProcedure("SpGetShiftTimes", parameters).Tables[0];

                    if (dtshiftHours.Rows.Count > 0)
                    {
                        string startTime = dtshiftHours.Rows[0]["StartTime"].ToString();
                        string endTime = dtshiftHours.Rows[0]["EndTime"].ToString();
                        if (startTime != string.Empty)
                        {
                            dteConfirmationWeekendPublicHolidayShiftStartTime.Value = startTime;
                        }
                        else
                        {
                            dteConfirmationWeekendPublicHolidayShiftStartTime.Value = null;
                        }
                        if (endTime != string.Empty)
                        {
                            dteConfirmationWeekendPublicHolidayShiftEndTime.Value = endTime;
                        }
                        else
                        {
                            dteConfirmationWeekendPublicHolidayShiftEndTime.Value = null;
                        }

                        dteConfirmationNormalShiftStartTime.Value = null;
                        dteConfirmationNormalShiftEndTime.Value = null;
                        dteConfirmationMorningShiftStartTime.Value = null;
                        dteConfirmationMorningShiftEndTime.Value = null;
                        dteConfirmationEveningShiftStartTime.Value = null;
                        dteConfirmationEveningShiftEndTime.Value = null;
                        dteConfirmationMorningShiftEndTime.Value = null;
                    }
                }

                #endregion Checking if the selected date is a UDM holiday

                if (isPublicHoliday == false && isUdmHoliday == false)
                {
                    #region Friday

                    if (selectedDate.DayOfWeek == DayOfWeek.Friday)
                    {
                        lblDescriptionText.Text = selectedDate.DayOfWeek.ToString(); //"Normal Business Day: " + 
                        //get shift type hours
                        SqlParameter[] parameters = new SqlParameter[2];
                        parameters[0] = new SqlParameter("@ShiftTypeID", _shiftTypeID);
                        long shift = 5;
                        if (_shiftTypeID == 1)
                        {
                            shift = 7;
                        }
                        else if (_shiftTypeID == 3)
                        {
                            shift = 10;
                        }
                        parameters[1] = new SqlParameter("@ShiftID", shift);  //friday shift
                        DataTable dtshiftHours = Methods.ExecuteStoredProcedure("SpGetShiftTimes", parameters).Tables[0];

                        if (dtshiftHours.Rows.Count > 0)
                        {
                            string startTime = dtshiftHours.Rows[0]["StartTime"].ToString();
                            string endTime = dtshiftHours.Rows[0]["EndTime"].ToString();
                            if (startTime != string.Empty)
                            {
                                dteConfirmationNormalShiftStartTime.Value = startTime;
                            }
                            else
                            {
                                dteConfirmationNormalShiftStartTime.Value = null;
                            }
                            if (endTime != string.Empty)
                            {
                                dteConfirmationNormalShiftEndTime.Value = endTime;
                            }
                            else
                            {
                                dteConfirmationNormalShiftEndTime.Value = null;
                            }
                            dteConfirmationMorningShiftStartTime.Value = null;
                            dteConfirmationMorningShiftEndTime.Value = null;
                            dteConfirmationEveningShiftStartTime.Value = null;
                            dteConfirmationEveningShiftEndTime.Value = null;
                            dteConfirmationMorningShiftEndTime.Value = null;
                            dteConfirmationWeekendPublicHolidayShiftStartTime.Value = null;
                            dteConfirmationWeekendPublicHolidayShiftEndTime.Value = null;
                        }
                    }

                    #endregion Friday

                    #region Normal Working Days - Monday through Thursday

                    if (selectedDate.DayOfWeek != DayOfWeek.Friday && selectedDate.DayOfWeek != DayOfWeek.Saturday && selectedDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        lblDescriptionText.Text = selectedDate.DayOfWeek.ToString(); //"Normal Business Day: " + 
                        //get shift type hours
                        int shiftID = 2;//normal Shift
                        if (_shiftTypeID == 1)
                        {
                            shiftID = 6;
                        }
                        else if (_shiftTypeID == 3)
                        {
                            shiftID = 8;
                        }
                        SqlParameter[] parameters = new SqlParameter[2];
                        parameters[0] = new SqlParameter("@ShiftTypeID", _shiftTypeID);
                        parameters[1] = new SqlParameter("@ShiftID", shiftID);  //normal shift
                        DataTable dtshiftHours = Methods.ExecuteStoredProcedure("SpGetShiftTimes", parameters).Tables[0];

                        if (dtshiftHours.Rows.Count > 0)
                        {
                            string startTime = dtshiftHours.Rows[0]["StartTime"].ToString();
                            string endTime = dtshiftHours.Rows[0]["EndTime"].ToString();
                            if (startTime != string.Empty)
                            {
                                dteConfirmationNormalShiftStartTime.Value = startTime;
                            }
                            else
                            {
                                dteConfirmationNormalShiftStartTime.Value = null;
                            }
                            if (endTime != string.Empty)
                            {
                                dteConfirmationNormalShiftEndTime.Value = endTime;
                            }
                            else
                            {
                                dteConfirmationNormalShiftEndTime.Value = null;
                            }
                            dteConfirmationMorningShiftStartTime.Value = null;
                            dteConfirmationMorningShiftEndTime.Value = null;
                            dteConfirmationEveningShiftStartTime.Value = null;
                            dteConfirmationEveningShiftEndTime.Value = null;
                            dteConfirmationMorningShiftEndTime.Value = null;
                            dteConfirmationWeekendPublicHolidayShiftStartTime.Value = null;
                            dteConfirmationWeekendPublicHolidayShiftEndTime.Value = null;
                        }
                    }

                    #endregion Normal Working Days - Monday through Thursday

                    #region Weekends

                    else
                    {
                        if (selectedDate.DayOfWeek != DayOfWeek.Friday)
                        {
                            lblDescriptionText.Text = selectedDate.DayOfWeek.ToString(); //"Weekend: " + 
                            SqlParameter[] parameters = new SqlParameter[2];
                            parameters[0] = new SqlParameter("@ShiftTypeID", _shiftTypeID);
                            parameters[1] = new SqlParameter("@ShiftID", 4);  //weekend shift
                            DataTable dtshiftHours = Methods.ExecuteStoredProcedure("SpGetShiftTimes", parameters).Tables[0];

                            if (dtshiftHours.Rows.Count > 0)
                            {
                                string startTime = dtshiftHours.Rows[0]["StartTime"].ToString();
                                string endTime = dtshiftHours.Rows[0]["EndTime"].ToString();
                                if (startTime != string.Empty)
                                {
                                    dteConfirmationWeekendPublicHolidayShiftStartTime.Value = startTime;
                                }
                                else
                                {
                                    dteConfirmationWeekendPublicHolidayShiftStartTime.Value = null;
                                }
                                if (endTime != string.Empty)
                                {
                                    dteConfirmationWeekendPublicHolidayShiftEndTime.Value = endTime;
                                }
                                else
                                {
                                    dteConfirmationWeekendPublicHolidayShiftEndTime.Value = null;
                                }

                                dteConfirmationNormalShiftStartTime.Value = null;
                                dteConfirmationNormalShiftEndTime.Value = null;
                                dteConfirmationMorningShiftStartTime.Value = null;
                                dteConfirmationMorningShiftEndTime.Value = null;
                                dteConfirmationEveningShiftStartTime.Value = null;
                                dteConfirmationEveningShiftEndTime.Value = null;
                                dteConfirmationMorningShiftEndTime.Value = null;
                            }
                        }
                    }

                    #endregion Weekends

                }

                LoadConfirmationAgentShiftTimes();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void LoadAgentShiftTimes()
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@UserID", _selectedAgentID);
                parameters[1] = new SqlParameter("@WorkDate", dteDate.Value);

                DataTable dtAgentShiftTimes = null;
                switch (((User)GlobalSettings.ApplicationUser).FKUserType)
                {
                    case (int)lkpUserType.SeniorAdministrator:
                    case (int)lkpUserType.Administrator:
                    case (int)lkpUserType.Manager:
                        dtAgentShiftTimes = Methods.ExecuteStoredProcedure("spGetAgentShiftTimes", parameters).Tables[0];
                        break;
                    case (int)lkpUserType.SalesAgent:
                    case (int)lkpUserType.DataCapturer:
                    case (int)lkpUserType.ConfirmationAgent:
                    case (int)lkpUserType.StatusLoader:
                        dtAgentShiftTimes =
                            Methods.ExecuteStoredProcedure("spGetAgentShiftTimesAgent", parameters).Tables[0];
                        if (dtAgentShiftTimes.Rows.Count > 0)
                        {
                            DataTable dtTemp = dtAgentShiftTimes.Clone();
                            dtAgentShiftTimes.DefaultView.Sort = "Date";
                            dtAgentShiftTimes = dtAgentShiftTimes.DefaultView.ToTable();

                            DateTime? date1 = null;
                            DateTime? date2 = null;
                            TimeSpan totalHours = TimeSpan.Parse("0:0");
                            foreach (DataRow dr in dtAgentShiftTimes.Rows)
                            {
                                if (!(date2 == null))
                                    date1 = dr["Date"] as DateTime?;

                                if (!(date2 == null) && date1 != date2)
                                {
                                    dtTemp.Rows.Add(null, null, "Total Hours", totalHours, null);
                                    dtTemp.ImportRow(dr);
                                    totalHours = TimeSpan.Parse("0:0");
                                }
                                else
                                {
                                    dtTemp.ImportRow(dr);
                                }

                                date2 = dr["Date"] as DateTime?;
                                if (dr["Time"] != null)
                                {
                                    totalHours = totalHours + TimeSpan.Parse(dr["Time"].ToString());
                                }
                            }

                            dtTemp.Rows.Add(null, null, "Total Hours", totalHours, null);
                            dtAgentShiftTimes.Clear();
                            dtAgentShiftTimes = dtTemp.Copy();
                        }
                        break;

                        //default:

                        //    break;
                }

                if (dtAgentShiftTimes != null && dtAgentShiftTimes.Rows.Count > 0)
                {
                    dgAgentTimes.ItemsSource = dtAgentShiftTimes.DefaultView;

                    if (dgAgentTimes.Columns.Count == 4) //Admin
                    {
                        dgAgentTimes.Columns[0].Visibility = Visibility.Hidden;
                        dgAgentTimes.Columns[1].Width = 220;
                        dgAgentTimes.Columns[2].Width = 100;
                        dgAgentTimes.Columns[3].Width = 70;
                    }

                    if (dgAgentTimes.Columns.Count == 5) //Agents
                    {
                        dgAgentTimes.Columns[0].Visibility = Visibility.Hidden;
                        dgAgentTimes.Columns[1].Width = 220;
                        dgAgentTimes.Columns[2].Width = 100;
                        dgAgentTimes.Columns[3].Width = 70;
                        dgAgentTimes.Columns[4].Width = 100;
                    }
                }
                else
                {
                    dgAgentTimes.ItemsSource = null;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void LoadConfirmationAgentShiftTimes()
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@UserID", _selectedAgentID);
                parameters[1] = new SqlParameter("@WorkDate", dteConfirmationDate.Value);

                DataTable dtAgentShiftTimes = null;
                switch (((User)GlobalSettings.ApplicationUser).FKUserType)
                {
                    case (int)lkpUserType.SeniorAdministrator:
                    case (int)lkpUserType.Administrator:
                    case (int)lkpUserType.Manager:
                        dtAgentShiftTimes = Methods.ExecuteStoredProcedure("spGetAgentShiftTimes", parameters).Tables[0];
                        break;
                    case (int)lkpUserType.SalesAgent:
                    case (int)lkpUserType.DataCapturer:
                    case (int)lkpUserType.ConfirmationAgent:
                    case (int)lkpUserType.CallMonitoringAgent:
                    case (int)lkpUserType.StatusLoader:
                        dtAgentShiftTimes =
                            Methods.ExecuteStoredProcedure("spGetAgentShiftTimesAgent", parameters).Tables[0];
                        if (dtAgentShiftTimes.Rows.Count > 0)
                        {
                            DataTable dtTemp = dtAgentShiftTimes.Clone();
                            dtAgentShiftTimes.DefaultView.Sort = "Date";
                            dtAgentShiftTimes = dtAgentShiftTimes.DefaultView.ToTable();

                            DateTime? date1 = null;
                            DateTime? date2 = null;
                            TimeSpan totalHours = TimeSpan.Parse("0:0");
                            foreach (DataRow dr in dtAgentShiftTimes.Rows)
                            {
                                if (!(date2 == null))
                                    date1 = dr["Date"] as DateTime?;

                                if (!(date2 == null) && date1 != date2)
                                {
                                    dtTemp.Rows.Add(null, null, "Total Hours", totalHours, null);
                                    dtTemp.ImportRow(dr);
                                    totalHours = TimeSpan.Parse("0:0");
                                }
                                else
                                {
                                    dtTemp.ImportRow(dr);
                                }

                                date2 = dr["Date"] as DateTime?;
                                if (dr["Time"] != null)
                                {
                                    totalHours = totalHours + TimeSpan.Parse(dr["Time"].ToString());
                                }
                            }

                            dtTemp.Rows.Add(null, null, "Total Hours", totalHours, null);
                            dtAgentShiftTimes.Clear();
                            dtAgentShiftTimes = dtTemp.Copy();
                        }
                        break;

                        //default:

                        //    break;
                }

                if (dtAgentShiftTimes != null && dtAgentShiftTimes.Rows.Count > 0)
                {
                    dgConfirmationAgentTimes.ItemsSource = dtAgentShiftTimes.DefaultView;

                    if (dgConfirmationAgentTimes.Columns.Count == 4) //Admin
                    {
                        dgConfirmationAgentTimes.Columns[0].Visibility = Visibility.Hidden;
                        dgConfirmationAgentTimes.Columns[1].Width = 220;
                        dgConfirmationAgentTimes.Columns[2].Width = 100;
                        dgConfirmationAgentTimes.Columns[3].Width = 70;
                    }

                    if (dgConfirmationAgentTimes.Columns.Count == 5) //Agents
                    {
                        dgConfirmationAgentTimes.Columns[0].Visibility = Visibility.Hidden;
                        dgConfirmationAgentTimes.Columns[1].Width = 220;
                        dgConfirmationAgentTimes.Columns[2].Width = 100;
                        dgConfirmationAgentTimes.Columns[3].Width = 70;
                        dgConfirmationAgentTimes.Columns[4].Width = 100;
                    }
                }
                else
                {
                    dgConfirmationAgentTimes.ItemsSource = null;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void StopEditing()
        {
            dgAgentTimes.ItemsSource = null;
            cmbCampaigns.SelectedValue = null;
            _selectedUserHourID = -1;
            lblEditMode.Visibility = Visibility.Collapsed;
            imgEditMode.Visibility = Visibility.Collapsed;
            btnStopEditing.Visibility = Visibility.Collapsed;

            if (dteDate.DateValue != null)
            {
                DateTime tempDateValue = dteDate.DateValue.Value;
                dteDate.Value = tempDateValue.AddDays(-1);
                dteDate.Value = tempDateValue; //reason for this we want to trigger value changed event
            }

            rbIsRedeemedNo.IsChecked = false;
            rbIsRedeemedYes.IsChecked = false;
            rbIsRedeemedNeither.IsChecked = true;
            ShowGiftRedeemedControls(false);

            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@StaffID", (object)0);
            parameters[1] = new SqlParameter("@UserID", _selectedAgentID);
            var shiftID = Methods.ExecuteFunction("GetStaffShiftID", parameters, "Server=udmsql; Database=Blush; User ID=apollo; Password=J5TeXhzC6_8RVu9q;", IsolationLevel.ReadUncommitted);
            if (Convert.ToInt64(shiftID) == 4) shiftID = 3; // Blush and Insure ShiftType tables differ
            cmbShiftType.SelectedValue = shiftID;
            _shiftTypeID = Convert.ToInt64(shiftID);


            if (_userTypeID == 2)
            {
                dteDate.DateValue = DateTime.Today;
            }
        }

        private void ConfirmationStopEditing()
        {
            dgConfirmationAgentTimes.ItemsSource = null;
            cmbConfirmationCampaigns.SelectedValue = null;
            _selectedUserHourID = -1;
            lblConfirmationEditMode.Visibility = Visibility.Collapsed;
            imgConfirmationEditMode.Visibility = Visibility.Collapsed;
            btnConfirmationStopEditing.Visibility = Visibility.Collapsed;

            if (dteConfirmationDate.DateValue != null)
            {
                DateTime tempDateValue = dteConfirmationDate.DateValue.Value;
                dteConfirmationDate.Value = tempDateValue.AddDays(-1);
                dteConfirmationDate.Value = tempDateValue; //reason for this we want to trigger value changed event
            }

            rbConfirmationIsRedeemedNo.IsChecked = false;
            rbConfirmationIsRedeemedYes.IsChecked = false;
            rbConfirmationIsRedeemedNeither.IsChecked = true;
            ShowGiftRedeemedControls(false);

            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@StaffID", (object)0);
            parameters[1] = new SqlParameter("@UserID", _selectedAgentID);
            var shiftID = Methods.ExecuteFunction("GetStaffShiftID", parameters, "Server=udmsql; Database=Blush; User ID=apollo; Password=J5TeXhzC6_8RVu9q;", IsolationLevel.ReadUncommitted);
            if (Convert.ToInt64(shiftID) == 4) shiftID = 3; // Blush and Insure ShiftType tables differ
            cmbConfirmationShiftType.SelectedValue = shiftID;
            _shiftTypeID = Convert.ToInt64(shiftID);


            if (_userTypeID == 2)
            {
                dteConfirmationDate.DateValue = DateTime.Today;
            }
        }

        private void ShowGiftRedeemedControls(bool show)
        {
            Visibility visibility;

            if (show)
            {
                visibility = Visibility.Visible;
            }
            else
            {
                visibility = Visibility.Hidden;
            }

            lblIsRedeemedHours.Visibility = visibility;
            lblIsRedeemedHoursYes.Visibility = visibility;
            lblIsRedeemedHoursNo.Visibility = visibility;
            rbIsRedeemedYes.Visibility = visibility;
            rbIsRedeemedNo.Visibility = visibility;
            rbIsRedeemedNeither.Visibility = visibility;
            lblIsRedeemedHoursNeither.Visibility = visibility;
        }



        #endregion Private Methods

        #region Event Handlers

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void BaseControl_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                if (GlobalSettings.ApplicationUser.ID == 1 || GlobalSettings.ApplicationUser.ID == 1016 || GlobalSettings.ApplicationUser.ID == 6091 || GlobalSettings.ApplicationUser.ID == 7194 || GlobalSettings.ApplicationUser.ID == 45 || GlobalSettings.ApplicationUser.ID == 394
                   || GlobalSettings.ApplicationUser.ID == 403 || GlobalSettings.ApplicationUser.ID == 2232 || GlobalSettings.ApplicationUser.ID == 2767 || GlobalSettings.ApplicationUser.ID == 2810 || GlobalSettings.ApplicationUser.ID == 2857 || GlobalSettings.ApplicationUser.ID == 3165
                   || GlobalSettings.ApplicationUser.ID == 3388 || GlobalSettings.ApplicationUser.ID == 6181 || GlobalSettings.ApplicationUser.ID == 7206 || GlobalSettings.ApplicationUser.ID == 40416)
                {

                    LoadConfirmationAgentsScreen();

                    //cmbConfirmationAgents.Text = GlobalSettings.ApplicationUser.ID.ToString(); 

                    dteConfirmationDate.Value = DateTime.Now.Date;

                    //SqlParameter[] parameters = new SqlParameter[1];
                    //parameters[0] = new SqlParameter("@userId", GlobalSettings.ApplicationUser.ID);
                    //DataTable dtData = Methods.ExecuteStoredProcedure("sp_GetUserName", parameters).Tables[0];
                    //double timeDiffRecordedNormalHours = 0;

                    //if (dtData.Rows.Count > 0)
                    //{

                    //    string userName = dtData.Rows[0]["FirstName"].ToString();

                    //    cmbConfirmationAgents.SelectedItem = userName;

                    //}

                }
                else
                {
                    LoadAgentsScreen();

                    dteDate.Value = DateTime.Now.Date;
                }
            }
            catch (Exception ex) 
            {
                HandleException(ex);
            }
        }

        private void cmbShiftType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DataRowView selectedItem = (DataRowView)cmbShiftType.SelectedItem;

            _shiftTypeID = Convert.ToInt64(selectedItem?.Row[0].ToString());

            if (_shiftTypeID == 1)
            {
                //Nicolas Stephenson commented the following out because an overtime shift was requested from 07:00 to 07:30. See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/219628533/comments
                //start
                //lblMorningShift.Visibility = Visibility.Collapsed;
                //dteMorningShiftStartTime.Visibility = Visibility.Collapsed;
                //lblToMorning.Visibility = Visibility.Collapsed;
                //dteMorningShiftEndTime.Visibility = Visibility.Collapsed;
                //dteMorningShiftStartTime.Value = null;
                //dteMorningShiftEndTime.Value = null;
                //finish

                lblMorningShift.Visibility = Visibility.Visible;
                dteMorningShiftStartTime.Visibility = Visibility.Visible;
                lblToMorning.Visibility = Visibility.Visible;
                dteMorningShiftEndTime.Visibility = Visibility.Visible;

                ////normal
                //Grid.SetRow(lblNormalShift, Grid.GetRow(lblNormalShift) - 1);
                //Grid.SetRow(lblToNormal, Grid.GetRow(lblToNormal) - 1);
                //Grid.SetRow(dteNormalShiftStartTime, Grid.GetRow(dteNormalShiftStartTime) - 1);
                //Grid.SetRow(dteNormalShiftEndTime, Grid.GetRow(dteNormalShiftEndTime) - 1);

                ////evening
                //Grid.SetRow(lblEveningShift, Grid.GetRow(lblEveningShift) - 1);
                //Grid.SetRow(lblToEvening, Grid.GetRow(lblToEvening) - 1);
                //Grid.SetRow(dteEveningShiftStartTime, Grid.GetRow(dteEveningShiftStartTime) - 1);
                //Grid.SetRow(dteEveningShiftEndTime, Grid.GetRow(dteEveningShiftEndTime) - 1);
                ////weekend
                //Grid.SetRow(lblWeekend, Grid.GetRow(lblWeekend) - 1);
                //Grid.SetRow(lblToWeekend, Grid.GetRow(lblToWeekend) - 1);
                //Grid.SetRow(dteWeekendPublicHolidayShiftStartTime, Grid.GetRow(dteWeekendPublicHolidayShiftStartTime) - 1);
                //Grid.SetRow(dteWeekendPublicHolidayShiftEndTime, Grid.GetRow(dteWeekendPublicHolidayShiftEndTime) - 1);


            }
            else
            {
                lblMorningShift.Visibility = Visibility.Visible;
                dteMorningShiftStartTime.Visibility = Visibility.Visible;
                lblToMorning.Visibility = Visibility.Visible;
                dteMorningShiftEndTime.Visibility = Visibility.Visible;

                ////normal
                //Grid.SetRow(lblNormalShift, Grid.GetRow(lblNormalShift) + 1);
                //Grid.SetRow(lblToNormal, Grid.GetRow(lblToNormal) + 1);
                //Grid.SetRow(dteNormalShiftStartTime, Grid.GetRow(dteNormalShiftStartTime) + 1);
                //Grid.SetRow(dteNormalShiftEndTime, Grid.GetRow(dteNormalShiftEndTime) + 1);
                ////evening
                //Grid.SetRow(lblEveningShift, Grid.GetRow(lblEveningShift) + 1);
                //Grid.SetRow(lblToEvening, Grid.GetRow(lblToEvening) + 1);
                //Grid.SetRow(dteEveningShiftStartTime, Grid.GetRow(dteEveningShiftStartTime) + 1);
                //Grid.SetRow(dteEveningShiftEndTime, Grid.GetRow(dteEveningShiftEndTime) + 1);
                ////weekend
                //Grid.SetRow(lblWeekend, Grid.GetRow(lblWeekend) + 1);
                //Grid.SetRow(lblToWeekend, Grid.GetRow(lblToWeekend) + 1);
                //Grid.SetRow(dteWeekendPublicHolidayShiftStartTime, Grid.GetRow(dteWeekendPublicHolidayShiftStartTime) + 1);
                //Grid.SetRow(dteWeekendPublicHolidayShiftEndTime, Grid.GetRow(dteWeekendPublicHolidayShiftEndTime) + 1);
            }
            if (isFirstLoad == false)
            {
                ShiftChange();
            }
            if (isFirstLoad)
            {
                isFirstLoad = false;
            }

        }

        private void dteDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ShiftChange();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region Declarations & Initializations

                bool? result = true;
                bool canSave = false;
                DataTable dtShiftTimes = Methods.GetTableData("select * from Shift");
                TimeSpan morningShiftStart = new TimeSpan(07, 0, 0);//these will be defaults if table has no data
                TimeSpan morningShiftEnd = new TimeSpan(08, 30, 0);
                TimeSpan normalShiftStart = new TimeSpan(08, 30, 0);//these will be defaults if table has no data
                TimeSpan normalShiftEnd = new TimeSpan(16, 0, 0);
                TimeSpan eveningShiftStart = new TimeSpan(16, 0, 0);
                TimeSpan eveningShiftEnd = new TimeSpan(23, 59, 0);
                TimeSpan normalShift1Start = new TimeSpan(7, 30, 0);
                TimeSpan normalShift1End = new TimeSpan(15, 00, 0);
                //double morningMaxHours = 1;
                double normalMaxHours = 8.5;
                //double eveningMaxHours = 9;
                double normalShift1MaxHours = 7.5;

                double timeDiffNormalHours = 0;

                #endregion Declarations & Initializations

                if (
                    (!dteMorningShiftStartTime.IsValueValid || !dteMorningShiftEndTime.IsValueValid ||
                     !dteEveningShiftStartTime.IsValueValid || !dteEveningShiftEndTime.IsValueValid ||
                     !dteWeekendPublicHolidayShiftStartTime.IsValueValid || !dteWeekendPublicHolidayShiftEndTime.IsValueValid)
                     ||
                    (dteMorningShiftStartTime.Value == null && dteMorningShiftEndTime.Value == null &&
                     dteEveningShiftStartTime.Value == null && dteEveningShiftEndTime.Value == null &&
                     dteWeekendPublicHolidayShiftStartTime.Value == null && dteWeekendPublicHolidayShiftEndTime.Value == null)
                     ||
                    ((dteMorningShiftStartTime.Value != null && dteMorningShiftEndTime.Value == null) || (dteMorningShiftStartTime.Value == null && dteMorningShiftEndTime.Value != null))
                    ||
                    ((dteEveningShiftStartTime.Value != null && dteEveningShiftEndTime.Value == null) || (dteEveningShiftStartTime.Value == null && dteEveningShiftEndTime.Value != null))
                    ||
                    ((dteWeekendPublicHolidayShiftStartTime.Value != null && dteWeekendPublicHolidayShiftEndTime.Value == null) || (dteWeekendPublicHolidayShiftStartTime.Value == null && dteWeekendPublicHolidayShiftEndTime.Value != null))
                    ||
                    (dteMorningShiftStartTime.DateValue >= dteMorningShiftEndTime.DateValue ||
                     dteEveningShiftStartTime.DateValue >= dteEveningShiftEndTime.DateValue ||
                     dteWeekendPublicHolidayShiftStartTime.DateValue >= dteWeekendPublicHolidayShiftEndTime.DateValue)
                   )
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "One or more hour values not valid.", "Hours Not Saved", ShowMessageType.Error);

                    return;
                }

                if (dtShiftTimes.Rows.Count > 0)
                {
                    foreach (DataRow rw in dtShiftTimes.Rows)
                    {
                        if (rw["ID"].ToString() == "1")//morning shift
                        {
                            morningShiftStart = (TimeSpan)rw["StartTime"];
                            morningShiftEnd = (TimeSpan)rw["EndTime"];
                            //morningMaxHours = morningShiftEnd.Subtract(morningShiftStart).TotalHours;

                        }
                        if (rw["ID"].ToString() == "2")//normal shift
                        {
                            normalShiftStart = (TimeSpan)rw["StartTime"];
                            normalShiftEnd = (TimeSpan)rw["EndTime"];
                            normalMaxHours = normalShiftEnd.Subtract(normalShiftStart).TotalHours;

                        }
                        if (rw["ID"].ToString() == "3")//evening shift
                        {
                            eveningShiftStart = (TimeSpan)rw["StartTime"];
                            eveningShiftEnd = (TimeSpan)rw["EndTime"];
                            //eveningMaxHours = eveningShiftEnd.Subtract(eveningShiftStart).TotalHours;
                        }
                        if (rw["ID"].ToString() == "6")//normal shift 1
                        {
                            normalShift1Start = (TimeSpan)rw["StartTime"];
                            normalShift1End = (TimeSpan)rw["EndTime"];
                            normalShift1MaxHours = normalShift1End.Subtract(normalShift1Start).TotalHours;
                        }
                    }
                }
                if (_shiftTypeID == 3)
                {
                    normalShift1MaxHours = 15;
                    normalMaxHours = 15;
                }
                if (_selectedAgentID > 0) // && _selectedCampaignId > 0
                {
                    UserHours userHours = new UserHours();
                    if (_selectedUserHourID > -1)
                    {
                        result = ShowMessageBox(new INMessageBoxWindow2(), "Would you like to Save Changes to the following times?", "Save Changes?", ShowMessageType.Information);
                        userHours = new UserHours(_selectedUserHourID);
                    }
                    if (Convert.ToBoolean(result))
                    {
                        userHours.FKUserID = _selectedAgentID;
                        userHours.FKINCampaignID = _selectedCampaignId > 0 ? (long?) _selectedCampaignId : null;
                        userHours.FKShiftTypeID = _shiftTypeID;
                        userHours.WorkingDate = DateTime.Parse(dteDate.Value.ToString());

                        #region Ensuring that at least 1 of the 3 redeemed hours options is selected

                        if ((Convert.ToBoolean(rbIsRedeemedYes.IsChecked) == false) &&
                            (Convert.ToBoolean(rbIsRedeemedNo.IsChecked) == false) &&
                            (Convert.ToBoolean(rbIsRedeemedNeither.IsChecked) == false))
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), "Please indicate the hours as being either redeemed, non-redeemed or default.", "Save Result", ShowMessageType.Exclamation);
                            return;
                        }

                        #endregion Ensuring that at least 1 of the 3 redeemed hours options is selected

                        #region Evaluating the morning shift time range

                        if (dteMorningShiftStartTime.Value != null && dteMorningShiftEndTime.Value != null)
                        {
                            DateTime selectedDate = DateTime.Parse(dteDate.Value.ToString());
                            if (selectedDate.DayOfWeek == DayOfWeek.Saturday || selectedDate.DayOfWeek == DayOfWeek.Sunday)
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "Cannot record Morning Time for Weekend ", "Incorrect Hours", ShowMessageType.Exclamation);
                                return;
                            }
                            if (_isPublicHoliday)
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "Morning Time cannot be recorded for public Holiday ", "Incorrect Hours", ShowMessageType.Exclamation);
                                return;
                            }
                            DateTime shiftStart = DateTime.Parse(dteMorningShiftStartTime.Value.ToString());
                            DateTime shiftEnd = DateTime.Parse(dteMorningShiftEndTime.Value.ToString());
                            if (_shiftTypeID == 1)
                            {
                                morningShiftEnd = morningShiftEnd.Add(new TimeSpan(-1, 0, 0));
                                //morningShiftStart = morningShiftStart.Add(new TimeSpan(3, 30, 0));
                            }
                            if (_shiftTypeID == 3)
                            {
                                morningShiftEnd = morningShiftEnd.Add(new TimeSpan(2, 30, 0));
                                //morningShiftStart = morningShiftStart.Add(new TimeSpan(3, 30, 0));
                            }
                            if (shiftStart.TimeOfDay < morningShiftStart)
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "The Start Time is not allowed for Morning Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                return;
                            }
                            if (shiftEnd.TimeOfDay > morningShiftEnd)
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "The End Time is not allowed for Morning Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                return;
                            }
                            userHours.MorningShiftStartTime = shiftStart.TimeOfDay;
                            userHours.MorningShiftEndTime = shiftEnd.TimeOfDay;
                            canSave = true;
                        }

                        #endregion Evaluating the morning shift time range

                        #region Evaluating the normal shift time range

                        if (dteNormalShiftStartTime.Value != null && dteNormalShiftEndTime.Value != null)
                        {
                          
                            //first check if its not public holiday
                            if (_isPublicHoliday)
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "Normal Time cannot be recorded for public Holiday ", "Incorrect Hours", ShowMessageType.Exclamation);
                                return;
                            }
                            DateTime shiftStart = DateTime.Parse(dteNormalShiftStartTime.Value.ToString());
                            DateTime shiftEnd = DateTime.Parse(dteNormalShiftEndTime.Value.ToString());
                            //check if shift exceeds 8.5 hours and 8 hours if selected date is friday
                            DateTime selectedDate = DateTime.Parse(dteDate.Value.ToString());
                            if (selectedDate.DayOfWeek == DayOfWeek.Saturday || selectedDate.DayOfWeek == DayOfWeek.Sunday)
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "Cannot record Normal Time for Weekend ", "Incorrect Hours", ShowMessageType.Exclamation);
                                return;
                            }
                            if (selectedDate.DayOfWeek == DayOfWeek.Friday)//8 hours max
                            {
                                double timeDiff = shiftEnd.Subtract(shiftStart).TotalHours;
                                timeDiffNormalHours = timeDiff;
                                //if (_shiftTypeID == 2)
                                //{
                                //    if (shiftStart.TimeOfDay < normalShiftStart)
                                //    {
                                //        ShowMessageBox(new INMessageBoxWindow1(), "The Start Time is not allowed for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                //        return;
                                //    }

                                //    if (timeDiff > (normalMaxHours))
                                //    {
                                //        ShowMessageBox(new INMessageBoxWindow1(), "The Hours You have captured Exceed the allowed time for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                //        return;
                                //    }
                                //}
                                //else
                                //{
                                //    if (shiftStart.TimeOfDay < normalShift1Start)
                                //    {
                                //        ShowMessageBox(new INMessageBoxWindow1(), "The Start Time is not allowed for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                //        return;
                                //    }

                                //    if (timeDiff > (normalShift1MaxHours))
                                //    {
                                //        ShowMessageBox(new INMessageBoxWindow1(), "The Hours You have captured Exceed the allowed time for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                //        return;
                                //    }
                                //}

                            }
                            else //8.5 hours max (normal time
                            {
                                double timeDiff = shiftEnd.Subtract(shiftStart).TotalHours;
                                timeDiffNormalHours = timeDiff;
                                if (_shiftTypeID == 2)
                                {
                                    if (shiftStart.TimeOfDay < normalShiftStart)
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "The Start Time is not allowed for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                        return;
                                    }
                                    if (shiftEnd.TimeOfDay > normalShiftEnd)
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "The End Time is not allowed for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                        return;
                                    }
                                    if (timeDiff > normalMaxHours)
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "The Hours You have captured Exceed the allowed time for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                        return;
                                    }
                                }
                                else
                                {
                                    if (_shiftTypeID == 3)
                                    {
                                        normalShift1End = normalShift1End.Add(new TimeSpan(3, 30, 0));
                                        normalShift1Start = normalShift1Start.Add(new TimeSpan(3, 30, 0));
                                    }

                                    if (shiftStart.TimeOfDay < normalShift1Start)
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "The Start Time is not allowed for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                        return;
                                    }
                                    if (shiftEnd.TimeOfDay > normalShift1End)
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "The End Time is not allowed for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                        return;
                                    }
                                    if (timeDiff > normalShift1MaxHours)
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "The Hours You have captured Exceed the allowed time for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                        return;
                                    }
                                }
                            }
                            userHours.NormalShiftStartTime = shiftStart.TimeOfDay;
                            userHours.NormalShiftEndTime = shiftEnd.TimeOfDay;
                            canSave = true;
                        }

                        #endregion Evaluating the normal shift time range

                        #region Evaluating the evening shift time range

                        if (dteEveningShiftStartTime.Value != null && dteEveningShiftEndTime.Value != null)
                        {
                            if (_isPublicHoliday)
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "Evening Time cannot be recorded for public Holiday ", "Incorrect Hours", ShowMessageType.Exclamation);
                                return;
                            }
                            DateTime shiftStart = DateTime.Parse(dteEveningShiftStartTime.Value.ToString());
                            DateTime shiftEnd = DateTime.Parse(dteEveningShiftEndTime.Value.ToString());

                            DateTime selectedDate = DateTime.Parse(dteDate.Value.ToString());
                            if (shiftEnd.TimeOfDay > eveningShiftEnd)
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "The End Time is not allowed for Evening Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                return;
                            }
                            if (selectedDate.DayOfWeek == DayOfWeek.Friday)
                            {
                                if (_shiftTypeID == 3)
                                {
                                    normalShift1End.Add(new TimeSpan(2, 0, 0));
                                }
                                if (_shiftTypeID == 1)
                                {
                                    if (shiftStart.TimeOfDay < eveningShiftStart.Subtract(new TimeSpan(0,30,0)))
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "The Start Time is not allowed for Evening Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                        return;
                                    }
                                }
                                else
                                {
                                    if (shiftStart.TimeOfDay < eveningShiftStart)
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "The Start Time is not allowed for Evening Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (_shiftTypeID == 3)
                                {
                                    eveningShiftStart = eveningShiftStart.Add(new TimeSpan(3, 30, 0));
                                }
                                if (_shiftTypeID == 2)
                                {
                                    eveningShiftStart = eveningShiftStart.Add(new TimeSpan(1, 0, 0));
                                }
                                if (shiftStart.TimeOfDay < eveningShiftStart)
                                {
                                    ShowMessageBox(new INMessageBoxWindow1(), "The Start Time is not allowed for Evening Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                    return;
                                }
                            }

                            userHours.EveningShiftStartTime = shiftStart.TimeOfDay;
                            userHours.EveningShiftEndTime = shiftEnd.TimeOfDay;
                            canSave = true;
                        }

                        #endregion Evaluating the evening shift time range

                        #region Evaluating the weekend / public holiday shift time range

                        if (dteWeekendPublicHolidayShiftStartTime.Value != null && dteWeekendPublicHolidayShiftEndTime.Value != null)
                        {
                            DateTime shiftStart = DateTime.Parse(dteWeekendPublicHolidayShiftStartTime.Value.ToString());
                            DateTime shiftEnd = DateTime.Parse(dteWeekendPublicHolidayShiftEndTime.Value.ToString());

                            userHours.PublicHolidayWeekendShiftStartTime = shiftStart.TimeOfDay;
                            userHours.PublicHolidayWeekendShiftEndTime = shiftEnd.TimeOfDay;
                            canSave = true;
                        }

                        #endregion Evaluating the weekend / public holiday shift time range

                        #region Are the hours being captured for a redeemed batch?

                        //if (rbIsRedeemedNeither.IsChecked.HasValue) //((!rbIsRedeemedYes.IsChecked.HasValue) && (!rbIsRedeemedNo.IsChecked.HasValue))
                        //{
                        //    if (rbIsRedeemedNeither.IsChecked.Value)
                        //    {
                        //        userHours.IsRedeemedHours = null;
                        //    }
                        //}

                        //else
                        //{
                        //    if (rbIsRedeemedYes.IsChecked.HasValue)
                        //    {
                        //        if (rbIsRedeemedYes.IsChecked.Value)
                        //        {
                        //            userHours.IsRedeemedHours = true;
                        //        }
                        //    }

                        //    else if (rbIsRedeemedNo.IsChecked.HasValue)
                        //    {
                        //        if (rbIsRedeemedNo.IsChecked.Value)
                        //        {
                        //            userHours.IsRedeemedHours = false;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        userHours.IsRedeemedHours = null;
                        //    }
                        //}


                        if (Convert.ToBoolean(rbIsRedeemedNeither.IsChecked.Value))
                        {
                            userHours.IsRedeemedHours = null;
                        }

                        else if (Convert.ToBoolean(rbIsRedeemedYes.IsChecked.Value))
                        {
                            userHours.IsRedeemedHours = true;
                        }

                        else if (Convert.ToBoolean(rbIsRedeemedNo.IsChecked.Value))
                        {
                            userHours.IsRedeemedHours = false;
                        }
                        else
                        {
                            userHours.IsRedeemedHours = null;
                        }

                        #endregion Are the hours being captured for a redeemed batch?

                        if (canSave)
                        {
                            if (_selectedUserHourID == -1)
                            {
                                //check total hours already recorded for day shift
                                SqlParameter[] parameters = new SqlParameter[3];
                                parameters[0] = new SqlParameter("@AgentIDs", _selectedAgentID + ",");
                                parameters[1] = new SqlParameter("@FromDate", dteDate.Value);
                                parameters[2] = new SqlParameter("@ToDate", dteDate.Value);
                                DataTable dtData = Methods.ExecuteStoredProcedure("spReportHours", parameters).Tables[0];
                                double timeDiffRecordedNormalHours = 0;
                                if (dtData.Rows.Count > 0)
                                {
                                    foreach (DataRow rw in dtData.Rows)
                                    {
                                        if (rw["EmployeeName"].ToString() == cmbAgents.Text)
                                        {
                                            string totalNormaltTimeWorked = dtData.Rows[0]["TotalNormalTimeHoursWorked"].ToString().Replace(" ", "");
                                            var totalHoursWorked = TimeSpan.Parse(totalNormaltTimeWorked);
                                            timeDiffRecordedNormalHours = double.Parse(totalHoursWorked.TotalHours.ToString());
                                        }
                                    }
                                }

                                #region Checking for duplicates

                                //see if there is duplicate time for any campaign today

                                DateTime selDate = (DateTime)dteDate.Value;
                                DataTable dtTimesToday = Methods.GetTableData("select * from UserHours where WorkingDate = '" + selDate.ToString("yyyy-MM-dd") + "' and FKUserID = " + _selectedAgentID);
                                if (dtTimesToday.Rows.Count > 0)
                                {
                                    foreach (DataRow row in dtTimesToday.Rows)
                                    {
                                        if (dteMorningShiftStartTime.Value != null && dteMorningShiftEndTime.Value != null)
                                        {
                                            DateTime MorningTimeCapturedStart = DateTime.Parse(dteMorningShiftStartTime.Value.ToString());
                                            DateTime MorningTimeCapturedEnd = DateTime.Parse(dteMorningShiftEndTime.Value.ToString());

                                            string morningSavedStartStr = row["MorningShiftStartTime"].ToString();
                                            string morningSavedEndStr = row["MorningShiftEndTime"].ToString();
                                            if (morningSavedStartStr != string.Empty)
                                            {
                                                DateTime morningSavedStart = DateTime.Parse(morningSavedStartStr);
                                                DateTime morningSavedEnd = DateTime.Parse(morningSavedEndStr);
                                                if ((morningSavedStart.TimeOfDay == MorningTimeCapturedStart.TimeOfDay) && (morningSavedEnd.TimeOfDay == MorningTimeCapturedEnd.TimeOfDay))
                                                {
                                                    ShowMessageBox(new INMessageBoxWindow1(), "Cannot Insert Duplicate Morning Shift Time ", "Incorrect Hours", ShowMessageType.Exclamation);
                                                    return;
                                                }
                                            }
                                        }
                                        if (dteNormalShiftStartTime.Value != null && dteNormalShiftEndTime.Value != null)
                                        {
                                            DateTime normalTimeCapturedStart = DateTime.Parse(dteNormalShiftStartTime.Value.ToString());
                                            DateTime normalTimeCapturedEnd = DateTime.Parse(dteNormalShiftEndTime.Value.ToString());

                                            string normalSavedStartStr = row["NormalShiftStartTime"].ToString();
                                            string normalSavedEndStr = row["NormalShiftEndTime"].ToString();
                                            if (normalSavedStartStr != string.Empty)
                                            {
                                                DateTime NormalSavedStart = DateTime.Parse(normalSavedStartStr);
                                                DateTime NormalSavedEnd = DateTime.Parse(normalSavedEndStr);
                                                if ((NormalSavedStart.TimeOfDay == normalTimeCapturedStart.TimeOfDay) && (NormalSavedEnd.TimeOfDay == normalTimeCapturedEnd.TimeOfDay))
                                                {
                                                    ShowMessageBox(new INMessageBoxWindow1(), "Cannot Insert Duplicate Normal Shift Time ", "Incorrect Hours", ShowMessageType.Exclamation);
                                                    return;
                                                }
                                            }
                                        }

                                        if (dteEveningShiftStartTime.Value != null && dteNormalShiftEndTime.Value != null)
                                        {
                                            DateTime eveningTimeCapturedStart = DateTime.Parse(dteEveningShiftStartTime.Value.ToString());
                                            DateTime eveningTimeCapturedEnd = DateTime.Parse(dteEveningShiftEndTime.Value.ToString());

                                            string eveningSavedStartStr = row["EveningShiftStartTime"].ToString();
                                            string eveningSavedEndStr = row["EveningShiftEndTime"].ToString();
                                            if (eveningSavedStartStr != string.Empty)
                                            {
                                                DateTime eveningSavedStart = DateTime.Parse(eveningSavedStartStr);
                                                DateTime eveningSavedEnd = DateTime.Parse(eveningSavedEndStr);
                                                if ((eveningSavedStart.TimeOfDay == eveningTimeCapturedStart.TimeOfDay) && (eveningSavedEnd.TimeOfDay == eveningTimeCapturedEnd.TimeOfDay))
                                                {
                                                    ShowMessageBox(new INMessageBoxWindow1(), "Cannot Insert Duplicate Evening Shift Time ", "Incorrect Hours", ShowMessageType.Exclamation);
                                                    return;
                                                }
                                            }
                                        }

                                        if (dteWeekendPublicHolidayShiftStartTime.Value != null && dteWeekendPublicHolidayShiftEndTime.Value != null)
                                        {
                                            DateTime weekendTimeCapturedStart = DateTime.Parse(dteWeekendPublicHolidayShiftStartTime.Value.ToString());
                                            DateTime weekendTimeCapturedEnd = DateTime.Parse(dteWeekendPublicHolidayShiftEndTime.Value.ToString());

                                            string weekendSavedStartStr = row["PublicHolidayWeekendShiftStartTime"].ToString();
                                            string weekendSavedEndStr = row["PublicHolidayWeekendShiftEndTime"].ToString();
                                            if (weekendSavedStartStr != string.Empty)
                                            {
                                                DateTime weekendSavedStart = DateTime.Parse(weekendSavedStartStr);
                                                DateTime weekendSavedEnd = DateTime.Parse(weekendSavedEndStr);
                                                if ((weekendSavedStart.TimeOfDay == weekendTimeCapturedStart.TimeOfDay) && (weekendSavedEnd.TimeOfDay == weekendTimeCapturedEnd.TimeOfDay))
                                                {
                                                    ShowMessageBox(new INMessageBoxWindow1(), "Cannot Insert Duplicate Public Holiday / Weekend Shift Time ", "Incorrect Hours", ShowMessageType.Exclamation);
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                }

                                #endregion Checking for duplicates

                                Double totalNormalTime = timeDiffNormalHours + timeDiffRecordedNormalHours;
                                if (_shiftTypeID == 1)
                                {
                                    if (totalNormalTime > normalShift1MaxHours)
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "The hours you are capturing exceed the day total", "Incorrect Hours", ShowMessageType.Exclamation);
                                        return;
                                    }
                                }
                                else
                                {
                                    if (totalNormalTime > normalMaxHours)
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "The hours you are capturing exceed the day total", "Incorrect Hours", ShowMessageType.Exclamation);
                                        return;
                                    }
                                }
                                //add normal hours recorded to existing ones

                            }

                            userHours.Save(_validationResult);

                            ShowMessageBox(new INMessageBoxWindow1(), "Hours Record Succesfully Saved ", "Save result", ShowMessageType.Information);
                            dteMorningShiftStartTime.Value = null;
                            dteMorningShiftEndTime.Value = null;

                            dteNormalShiftStartTime.Value = null;
                            dteNormalShiftEndTime.Value = null;

                            dteEveningShiftStartTime.Value = null;
                            dteEveningShiftEndTime.Value = null;

                            dteWeekendPublicHolidayShiftStartTime.Value = null;
                            dteWeekendPublicHolidayShiftEndTime.Value = null;

                            rbIsRedeemedNo.IsChecked = false;
                            rbIsRedeemedYes.IsChecked = false;
                            rbIsRedeemedNeither.IsChecked = true;

                            //dteDate.Value = DateTime.Now.AddDays(1);
                            //dteDate.Value = DateTime.Now;
                        }
                        else
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), "Hours could not be saved. Please ensure that all relevant fields have been completed.", "Save result", ShowMessageType.Exclamation);
                        }
                       

                        switch (((User)GlobalSettings.ApplicationUser).FKUserType)
                        {
                            case (int)lkpUserType.SeniorAdministrator:
                            case (int)lkpUserType.Administrator:
                            case (int)lkpUserType.Manager:
                                _selectedCampaignId = 0;
                                cmbCampaigns.SelectedItem = null;
                                ShowGiftRedeemedControls(false);
                                break;

                            case (int)lkpUserType.SalesAgent:
                            case (int)lkpUserType.DataCapturer:
                            case (int)lkpUserType.ConfirmationAgent:
                            case (int)lkpUserType.StatusLoader:
                                dteDate.Value = DateTime.Now;
                                _selectedCampaignId = 0;
                                cmbCampaigns.SelectedItem = null;
                                ShowGiftRedeemedControls(false);
                                break;

                            default:
                                dteDate.Value = DateTime.Now;
                                _selectedCampaignId = 0;
                                cmbCampaigns.SelectedItem = null;
                                ShowGiftRedeemedControls(false);
                                break;
                        }

                        LoadAgentShiftTimes();
                    }
                    if (_selectedUserHourID > -1)//clear edit mode
                    {
                        StopEditing();
                    }

                }
                else
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please Select Agent ", "Select Agent", ShowMessageType.Error);
                }

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void dgAgentTimes_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime))
            {
                var dataGridTextColumn = e.Column as DataGridTextColumn;
                if (dataGridTextColumn != null)
                    dataGridTextColumn.Binding.StringFormat = "yyyy-MM-dd";
            }
        }

        private void cmbAgents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbAgents.SelectedItem != null)
                {
                    DataRowView selectedItem = (DataRowView)cmbAgents.SelectedItem;               
                    _selectedAgentID = long.Parse(selectedItem.Row[0].ToString());
                    DataTable dtUserType = Methods.GetTableData("select * from dbo.[User] where ID = " + _selectedAgentID);
                    if (dtUserType.Rows[0]["FKUserType"].ToString() != "2")
                    {
                        cmbCampaigns.SelectedValue = 93;
                        //cmbShiftType.SelectedValue = 3;
                        dteNormalShiftStartTime.Value = null;
                        dteNormalShiftEndTime.Value = null;
                    }
                    else
                    {
                        cmbCampaigns.SelectedItem = null;
                    }

                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@StaffID", (object)0);
                    parameters[1] = new SqlParameter("@UserID", _selectedAgentID);
                    var shiftID = Methods.ExecuteFunction("GetStaffShiftID", parameters, "Server=udmsql; Database=Blush; User ID=apollo; Password=J5TeXhzC6_8RVu9q;", IsolationLevel.ReadUncommitted);
                    if (Convert.ToInt64(shiftID) == 4) shiftID = 3; // Blush and Insure ShiftType tables differ

                    cmbShiftType.SelectedValue = shiftID;
                    
                    _shiftTypeID = Convert.ToInt64(shiftID);

                    LoadAgentShiftTimes();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cmbCampaigns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbCampaigns.SelectedItem != null)
                {
                    DataRowView selectedItem = (DataRowView)cmbCampaigns.SelectedItem;
                    _selectedCampaignId = long.Parse(selectedItem.Row[0].ToString());
                    _isRedeemableGiftCampaign = Convert.ToBoolean(selectedItem.Row["IsRedeemableGiftCampaign"].ToString());
                    if (!_isRedeemableGiftCampaign)
                    {
                        rbIsRedeemedYes.IsChecked = false;
                        rbIsRedeemedNo.IsChecked = false;
                        rbIsRedeemedNeither.IsChecked = true;
                    }
                    ShowGiftRedeemedControls(_isRedeemableGiftCampaign);
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void dgAgentTimes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //get select hour user id
                if (dgAgentTimes.Items.Count > 0)
                {
                    DataRowView selectedItemRow = (DataRowView)dgAgentTimes.SelectedItem;
                    _selectedUserHourID = long.Parse(selectedItemRow.Row["UserHoursID"].ToString());
                    DataRow selectedItem = Methods.GetTableData("select * from UserHours where ID = " + _selectedUserHourID).Rows[0];

                    #region Date
                    dteDate.Value = selectedItem["WorkingDate"];
                    #endregion

                    #region Campaign
                    cmbCampaigns.SelectedValue = selectedItem["FKINCampaignID"];
                    #endregion Campaign

                    #region Shift Type
                    cmbShiftType.SelectedValue = selectedItem["FKShiftTypeID"];
                    #endregion ShiftType

                    #region Morning Shift
                    string morningShiftStart = selectedItem["MorningShiftStartTime"].ToString();
                    string morningShiftEnd = selectedItem["MorningShiftEndTime"].ToString();
                    if (morningShiftStart != string.Empty)
                    {
                        dteMorningShiftStartTime.Value = morningShiftStart;
                    }
                    else
                    {
                        dteMorningShiftStartTime.Value = null;
                    }
                    if (morningShiftEnd != string.Empty)
                    {
                        dteMorningShiftEndTime.Value = morningShiftEnd;
                    }
                    else
                    {
                        dteMorningShiftEndTime.Value = null;
                    }
                    #endregion Morning Shift

                    #region Normal Shift
                    string normalShiftStart = selectedItem["NormalShiftStartTime"].ToString();
                    string normalShiftEnd = selectedItem["NormalShiftEndTime"].ToString();
                    if (normalShiftStart != string.Empty)
                    {
                        dteNormalShiftStartTime.Value = normalShiftStart;
                    }
                    else
                    {
                        dteNormalShiftStartTime.Value = null;
                    }
                    if (normalShiftEnd != string.Empty)
                    {
                        dteNormalShiftEndTime.Value = normalShiftEnd;
                    }
                    else
                    {
                        dteNormalShiftEndTime.Value = null;
                    }
                    #endregion Normal Shift

                    #region evening shift
                    string eveningShiftStart = selectedItem["EveningShiftStartTime"].ToString();
                    string eveningShiftEnd = selectedItem["EveningShiftEndTime"].ToString();
                    if (eveningShiftStart != string.Empty)
                    {
                        dteEveningShiftStartTime.Value = eveningShiftStart;
                    }
                    else
                    {
                        dteEveningShiftStartTime.Value = null;
                    }
                    if (eveningShiftEnd != string.Empty)
                    {

                        dteEveningShiftEndTime.Value = eveningShiftEnd;
                    }
                    else
                    {
                        dteEveningShiftEndTime.Value = null;
                    }
                    #endregion evening shift

                    #region weekend and public holiday
                    string weekendPublicHolidayStart = selectedItem["PublicHolidayWeekendShiftStartTime"].ToString();
                    string weekendPublicHolidayEnd = selectedItem["PublicHolidayWeekendShiftEndTime"].ToString();
                    if (weekendPublicHolidayStart != string.Empty)
                    {
                        dteWeekendPublicHolidayShiftStartTime.Value = weekendPublicHolidayStart;
                    }
                    else
                    {
                        dteWeekendPublicHolidayShiftStartTime.Value = null;
                    }
                    if (weekendPublicHolidayEnd != string.Empty)
                    {
                        dteWeekendPublicHolidayShiftEndTime.Value = weekendPublicHolidayEnd;
                    }
                    else
                    {
                        dteWeekendPublicHolidayShiftEndTime.Value = null;
                    }
                    #endregion weekend and public holiday

                    #region Redeemed / Non-Redeemed Hours

                    bool? isRedeemedHours = selectedItem["IsRedeemedHours"] != DBNull.Value ? (bool)selectedItem["IsRedeemedHours"] : (bool?)null;

                    if (isRedeemedHours.HasValue)
                    {
                        if (isRedeemedHours.Value)
                        {
                            rbIsRedeemedYes.IsChecked = true;
                            rbIsRedeemedNo.IsChecked = false;
                            rbIsRedeemedNeither.IsChecked = false;
                        }
                        else
                        {
                            rbIsRedeemedYes.IsChecked = false;
                            rbIsRedeemedNo.IsChecked = true;
                            rbIsRedeemedNeither.IsChecked = false;
                        }
                    }
                    else
                    {
                        rbIsRedeemedYes.IsChecked = false;
                        rbIsRedeemedNo.IsChecked = false;
                        rbIsRedeemedNeither.IsChecked = true;
                    }

                    #endregion Redeemed / Non-Redeemed Hours

                    lblEditMode.Visibility = Visibility.Visible;
                    imgEditMode.Visibility = Visibility.Visible;
                    btnStopEditing.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnStopEditing_Click(object sender, RoutedEventArgs e)
        {
            StopEditing();
        }
     
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selValue = (DataRowView)dgAgentTimes.SelectedValue;
                //get userHour id
                if (selValue != null)
                {
                    string columnName = selValue[2].ToString();
                    if (columnName != "Total Hours")
                    {
                        bool? result = ShowMessageBox(new INMessageBoxWindow2(), "Would you like to Remove following times ?" + Environment.NewLine
                            + columnName + " " + selValue[3], "Save Changes ?", ShowMessageType.Information);
                        if (Convert.ToBoolean(result))
                        {
                            long? UserHourID = long.Parse(selValue[0].ToString());

                            object param1 = Database.GetParameter("@UserHourID", UserHourID);
                            object[] paramArray = new[] { param1 };
                            Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spRemoveHours", paramArray, 600);
                            ShowMessageBox(new INMessageBoxWindow1(), "Time Removed ", "Remove Result", ShowMessageType.Information);
                            LoadAgentShiftTimes();
                        }
                    }
                    else
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "Cannot Delete This Value ", "Remove Result", ShowMessageType.Exclamation);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Event Handlers

        //private void cmbConfirmationAgents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        if (cmbConfirmationAgents.SelectedItem != null)
        //        {

        //            DataRowView selectedItem = (DataRowView)cmbConfirmationAgents.SelectedItem;
        //            _selectedAgentID = long.Parse(selectedItem.Row[0].ToString());
        //            DataTable dtUserType = Methods.GetTableData("select firstname, lastname from dbo.[User] where ID = " + _selectedAgentID);

        //            if (dtUserType.Rows[0]["FKUserType"].ToString() != "2")
        //            {
        //                cmbConfirmationCampaigns.SelectedValue = 93;
        //                //cmbShiftType.SelectedValue = 3;
        //                dteConfirmationNormalShiftStartTime.Value = null;
        //                dteConfirmationNormalShiftEndTime.Value = null;
        //            }
        //            else
        //            {
        //                cmbConfirmationCampaigns.SelectedItem = null;
        //            }

        //            SqlParameter[] parameters = new SqlParameter[2];
        //            parameters[0] = new SqlParameter("@StaffID", (object)0);
        //            parameters[1] = new SqlParameter("@UserID", _selectedAgentID);
        //            var shiftID = Methods.ExecuteFunction("GetStaffShiftID", parameters, "Server=udmsql; Database=Blush; User ID=apollo; Password=J5TeXhzC6_8RVu9q;", IsolationLevel.ReadUncommitted);
        //            if (Convert.ToInt64(shiftID) == 4) shiftID = 3; // Blush and Insure ShiftType tables differ

        //            cmbConfirmationShiftType.SelectedValue = shiftID;

        //            _shiftTypeID = Convert.ToInt64(shiftID);

        //            LoadConfirmationAgentShiftTimes();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        private void cmbConfirmationCampaigns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbConfirmationCampaigns.SelectedItem != null)
                {
                    DataRowView selectedItem = (DataRowView)cmbConfirmationCampaigns.SelectedItem;
                    _selectedCampaignId = long.Parse(selectedItem.Row[0].ToString());

                    _isRedeemableGiftCampaign = Convert.ToBoolean(selectedItem.Row["IsRedeemableGiftCampaign"].ToString());
                    if (!_isRedeemableGiftCampaign)
                    {
                        rbConfirmationIsRedeemedYes.IsChecked = false;
                        rbConfirmationIsRedeemedNo.IsChecked = false;
                        rbConfirmationIsRedeemedNeither.IsChecked = true;
                    }
                    ShowGiftRedeemedControls(_isRedeemableGiftCampaign);

                    //cmbConfirmationCampaigns.SelectedItem.ToString(); 




                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void dteConfirmationDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ConfirmationShiftChange();
        }

        private void cmbConfirmationShiftType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DataRowView selectedItem = (DataRowView)cmbConfirmationShiftType.SelectedItem;

            _shiftTypeID = Convert.ToInt64(selectedItem?.Row[0].ToString());

            if (_shiftTypeID == 1)
            {
                //Nicolas Stephenson commented the following out because an overtime shift was requested from 07:00 to 07:30. See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/219628533/comments
                //start
                //lblMorningShift.Visibility = Visibility.Collapsed;
                //dteMorningShiftStartTime.Visibility = Visibility.Collapsed;
                //lblToMorning.Visibility = Visibility.Collapsed;
                //dteMorningShiftEndTime.Visibility = Visibility.Collapsed;
                //dteMorningShiftStartTime.Value = null;
                //dteMorningShiftEndTime.Value = null;
                //finish

                lblConfirmationMorningShift.Visibility = Visibility.Visible;
                dteConfirmationMorningShiftStartTime.Visibility = Visibility.Visible;
                lblConfirmationToMorning.Visibility = Visibility.Visible;
                dteConfirmationMorningShiftEndTime.Visibility = Visibility.Visible;

                ////normal
                //Grid.SetRow(lblNormalShift, Grid.GetRow(lblNormalShift) - 1);
                //Grid.SetRow(lblToNormal, Grid.GetRow(lblToNormal) - 1);
                //Grid.SetRow(dteNormalShiftStartTime, Grid.GetRow(dteNormalShiftStartTime) - 1);
                //Grid.SetRow(dteNormalShiftEndTime, Grid.GetRow(dteNormalShiftEndTime) - 1);

                ////evening
                //Grid.SetRow(lblEveningShift, Grid.GetRow(lblEveningShift) - 1);
                //Grid.SetRow(lblToEvening, Grid.GetRow(lblToEvening) - 1);
                //Grid.SetRow(dteEveningShiftStartTime, Grid.GetRow(dteEveningShiftStartTime) - 1);
                //Grid.SetRow(dteEveningShiftEndTime, Grid.GetRow(dteEveningShiftEndTime) - 1);
                ////weekend
                //Grid.SetRow(lblWeekend, Grid.GetRow(lblWeekend) - 1);
                //Grid.SetRow(lblToWeekend, Grid.GetRow(lblToWeekend) - 1);
                //Grid.SetRow(dteWeekendPublicHolidayShiftStartTime, Grid.GetRow(dteWeekendPublicHolidayShiftStartTime) - 1);
                //Grid.SetRow(dteWeekendPublicHolidayShiftEndTime, Grid.GetRow(dteWeekendPublicHolidayShiftEndTime) - 1);


            }
            else
            {
                lblConfirmationMorningShift.Visibility = Visibility.Visible;
                dteConfirmationMorningShiftStartTime.Visibility = Visibility.Visible;
                lblConfirmationToMorning.Visibility = Visibility.Visible;
                dteConfirmationMorningShiftEndTime.Visibility = Visibility.Visible;

                ////normal
                //Grid.SetRow(lblNormalShift, Grid.GetRow(lblNormalShift) + 1);
                //Grid.SetRow(lblToNormal, Grid.GetRow(lblToNormal) + 1);
                //Grid.SetRow(dteNormalShiftStartTime, Grid.GetRow(dteNormalShiftStartTime) + 1);
                //Grid.SetRow(dteNormalShiftEndTime, Grid.GetRow(dteNormalShiftEndTime) + 1);
                ////evening
                //Grid.SetRow(lblEveningShift, Grid.GetRow(lblEveningShift) + 1);
                //Grid.SetRow(lblToEvening, Grid.GetRow(lblToEvening) + 1);
                //Grid.SetRow(dteEveningShiftStartTime, Grid.GetRow(dteEveningShiftStartTime) + 1);
                //Grid.SetRow(dteEveningShiftEndTime, Grid.GetRow(dteEveningShiftEndTime) + 1);
                ////weekend
                //Grid.SetRow(lblWeekend, Grid.GetRow(lblWeekend) + 1);
                //Grid.SetRow(lblToWeekend, Grid.GetRow(lblToWeekend) + 1);
                //Grid.SetRow(dteWeekendPublicHolidayShiftStartTime, Grid.GetRow(dteWeekendPublicHolidayShiftStartTime) + 1);
                //Grid.SetRow(dteWeekendPublicHolidayShiftEndTime, Grid.GetRow(dteWeekendPublicHolidayShiftEndTime) + 1);
            }
            if (isFirstLoad == false)
            {
                ShiftChange();
            }
            if (isFirstLoad)
            {
                isFirstLoad = false;
            }
        }

        private void btnConfirmationSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region Declarations & Initializations

                bool? result = true;
                bool canSave = false;
                DataTable dtShiftTimes = Methods.GetTableData("select * from Shift");

                TimeSpan ConfirmationMorningShiftStart = new TimeSpan(00, 0, 0); //these will be defaults if table has no data
                TimeSpan ConfirmationMorningShiftEnd = new TimeSpan(00, 00, 0);
                TimeSpan ConfirmationNormalShiftStart = new TimeSpan(00, 00, 0); //these will be defaults if table has no data
                TimeSpan ConfirmationNormalShiftEnd = new TimeSpan(00, 0, 0);
                TimeSpan ConfirmationEveningShiftStart = new TimeSpan(00, 0, 0);
                TimeSpan ConfirmationEveningShiftEnd = new TimeSpan(00, 00, 0);
                
                //TimeSpan normalShift1Start = new TimeSpan(7, 30, 0);
                //TimeSpan normalShift1End = new TimeSpan(15, 00, 0);

                //double morningMaxHours = 1;
                double normalMaxHours = 8.5;
                //double eveningMaxHours = 9;
                double normalShift1MaxHours = 7.5;

                double timeDiffNormalHours = 0;

                #endregion Declarations & Initializations

                if (


                    (!dteConfirmationNormalShiftStartTime.IsValueValid || !dteConfirmationNormalShiftEndTime.IsValueValid ||
                    !dteConfirmationMorningShiftStartTime.IsValueValid || !dteConfirmationMorningShiftEndTime.IsValueValid ||
                     !dteConfirmationEveningShiftStartTime.IsValueValid || !dteConfirmationEveningShiftEndTime.IsValueValid ||
                     !dteConfirmationWeekendPublicHolidayShiftStartTime.IsValueValid || !dteConfirmationWeekendPublicHolidayShiftEndTime.IsValueValid)
                     ||

                    (dteConfirmationNormalShiftStartTime.Value == null && dteConfirmationNormalShiftStartTime.Value == null &&
                     dteConfirmationMorningShiftStartTime.Value == null && dteConfirmationMorningShiftEndTime.Value == null &&
                     dteConfirmationEveningShiftStartTime.Value == null && dteConfirmationEveningShiftEndTime.Value == null &&
                     dteConfirmationWeekendPublicHolidayShiftStartTime.Value == null && dteConfirmationWeekendPublicHolidayShiftEndTime.Value == null)
                     ||
                     ((dteConfirmationNormalShiftStartTime.Value != null && dteConfirmationNormalShiftEndTime.Value == null) || (dteConfirmationNormalShiftEndTime.Value == null && dteConfirmationNormalShiftEndTime.Value != null))
                     ||
                    ((dteConfirmationMorningShiftStartTime.Value != null && dteConfirmationMorningShiftEndTime.Value == null) || (dteConfirmationMorningShiftStartTime.Value == null && dteConfirmationMorningShiftEndTime.Value != null))
                    ||
                    ((dteConfirmationEveningShiftStartTime.Value != null && dteConfirmationEveningShiftEndTime.Value == null) || (dteConfirmationEveningShiftStartTime.Value == null && dteConfirmationEveningShiftEndTime.Value != null))
                    ||
                    ((dteConfirmationWeekendPublicHolidayShiftStartTime.Value != null && dteConfirmationWeekendPublicHolidayShiftEndTime.Value == null) || (dteConfirmationWeekendPublicHolidayShiftStartTime.Value == null && dteConfirmationWeekendPublicHolidayShiftEndTime.Value != null))
                    ||
                    (dteConfirmationNormalShiftStartTime.DateValue >= dteConfirmationNormalShiftEndTime.DateValue ||
                     dteConfirmationMorningShiftStartTime.DateValue >= dteConfirmationMorningShiftEndTime.DateValue ||
                     dteConfirmationEveningShiftStartTime.DateValue >= dteConfirmationEveningShiftEndTime.DateValue ||
                     dteConfirmationWeekendPublicHolidayShiftStartTime.DateValue >= dteConfirmationWeekendPublicHolidayShiftEndTime.DateValue)
                   )
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "One or more hour values not valid.", "Hours Not Saved", ShowMessageType.Error);

                    return;
                }

                if (dtShiftTimes.Rows.Count > 0)
                {
                    foreach (DataRow rw in dtShiftTimes.Rows)
                    {
                        if (rw["ID"].ToString() == "1")//morning shift
                        {
                            ConfirmationMorningShiftStart = (TimeSpan)rw["StartTime"];
                            ConfirmationMorningShiftEnd = (TimeSpan)rw["EndTime"];
                            //morningMaxHours = morningShiftEnd.Subtract(morningShiftStart).TotalHours;

                        }
                        if (rw["ID"].ToString() == "2")//normal shift
                        {
                            ConfirmationNormalShiftStart = (TimeSpan)rw["StartTime"];
                            ConfirmationNormalShiftEnd = (TimeSpan)rw["EndTime"];
                            normalMaxHours = ConfirmationNormalShiftEnd.Subtract(ConfirmationNormalShiftStart).TotalHours;

                        }
                        if (rw["ID"].ToString() == "3")//evening shift
                        {
                            ConfirmationEveningShiftStart = (TimeSpan)rw["StartTime"];
                            ConfirmationEveningShiftEnd = (TimeSpan)rw["EndTime"];
                            //eveningMaxHours = eveningShiftEnd.Subtract(eveningShiftStart).TotalHours;
                        }
                        //if (rw["ID"].ToString() == "6")//normal shift 1
                        //{
                        //    normalShift1Start = (TimeSpan)rw["StartTime"];
                        //    normalShift1End = (TimeSpan)rw["EndTime"];
                        //    normalShift1MaxHours = normalShift1End.Subtract(normalShift1Start).TotalHours;
                        //}
                    }
                }
                if (_shiftTypeID == 3)
                {
                    normalShift1MaxHours = 15;
                    normalMaxHours = 15;
                } 
                if (_selectedAgentID > 0) // && _selectedCampaignId > 0
                {
                    UserHours userHours = new UserHours();
                    if (_selectedUserHourID > -1)
                    {
                        result = ShowMessageBox(new INMessageBoxWindow2(), "Would you like to Save Changes to the following times?", "Save Changes?", ShowMessageType.Information);
                        userHours = new UserHours(_selectedUserHourID);
                    }
                    if (Convert.ToBoolean(result))
                    {
                        userHours.FKUserID = _selectedAgentID;
                        userHours.FKINCampaignID = _selectedCampaignId > 0 ? (long?)_selectedCampaignId : null;
                        userHours.FKShiftTypeID = _shiftTypeID;
                        userHours.WorkingDate = DateTime.Parse(dteConfirmationDate.Value.ToString());

                        #region Ensuring that at least 1 of the 3 redeemed hours options is selected

                        if ((Convert.ToBoolean(rbConfirmationIsRedeemedYes.IsChecked) == false) &&
                            (Convert.ToBoolean(rbConfirmationIsRedeemedNo.IsChecked) == false) &&
                            (Convert.ToBoolean(rbConfirmationIsRedeemedNeither.IsChecked) == false))
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), "Please indicate the hours as being either redeemed, non-redeemed or default.", "Save Result", ShowMessageType.Exclamation);
                            return;
                        }

                        #endregion Ensuring that at least 1 of the 3 redeemed hours options is selected

                        #region Evaluating the morning shift time range

                        if (dteConfirmationMorningShiftStartTime.Value != null && dteConfirmationMorningShiftEndTime.Value != null)
                        {
                            DateTime selectedDate = DateTime.Parse(dteConfirmationDate.Value.ToString());
                            if (selectedDate.DayOfWeek == DayOfWeek.Saturday || selectedDate.DayOfWeek == DayOfWeek.Sunday)
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "Cannot record Morning Time for Weekend ", "Incorrect Hours", ShowMessageType.Exclamation);
                                return;
                            }
                            if (_isPublicHoliday)
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "Morning Time cannot be recorded for public Holiday ", "Incorrect Hours", ShowMessageType.Exclamation);
                                return;
                            }
                            DateTime shiftStart = DateTime.Parse(dteConfirmationMorningShiftStartTime.Value.ToString());
                            DateTime shiftEnd = DateTime.Parse(dteConfirmationMorningShiftEndTime.Value.ToString());

                            if (_shiftTypeID == 1)
                            {
                                ConfirmationMorningShiftEnd = ConfirmationMorningShiftEnd.Add(new TimeSpan(-1, 0, 0));
                                //morningShiftStart = morningShiftStart.Add(new TimeSpan(3, 30, 0));
                            }

                            if (_shiftTypeID == 3)
                            {
                                ConfirmationMorningShiftEnd = ConfirmationMorningShiftEnd.Add(new TimeSpan(2, 30, 0));
                                //morningShiftStart = morningShiftStart.Add(new TimeSpan(3, 30, 0));
                            }

                            if (shiftStart.TimeOfDay < ConfirmationMorningShiftStart)
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "The Start Time is not allowed for Morning Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                return;
                            }

                            if (shiftEnd.TimeOfDay > ConfirmationMorningShiftEnd)
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "The End Time is not allowed for Morning Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                return;
                            }
                            userHours.MorningShiftStartTime = shiftStart.TimeOfDay;
                            userHours.MorningShiftEndTime = shiftEnd.TimeOfDay;
                            canSave = true;
                        }

                        #endregion Evaluating the morning shift time range

                        #region Evaluating the normal shift time range

                        if (dteConfirmationNormalShiftStartTime.Value != null && dteConfirmationNormalShiftEndTime.Value != null)
                        {

                            //first check if its not public holiday
                            if (_isPublicHoliday)
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "Normal Time cannot be recorded for public Holiday ", "Incorrect Hours", ShowMessageType.Exclamation);
                                return;
                            }
                            DateTime shiftStart = DateTime.Parse(dteConfirmationNormalShiftStartTime.Value.ToString());
                            DateTime shiftEnd = DateTime.Parse(dteConfirmationNormalShiftEndTime.Value.ToString());
                            //check if shift exceeds 8.5 hours and 8 hours if selected date is friday
                            DateTime selectedDate = DateTime.Parse(dteConfirmationDate.Value.ToString());
                            if (selectedDate.DayOfWeek == DayOfWeek.Saturday || selectedDate.DayOfWeek == DayOfWeek.Sunday)
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "Cannot record Normal Time for Weekend ", "Incorrect Hours", ShowMessageType.Exclamation);
                                return;
                            }
                            if (selectedDate.DayOfWeek == DayOfWeek.Friday)//8 hours max
                            {
                                double timeDiff = shiftEnd.Subtract(shiftStart).TotalHours;
                                timeDiffNormalHours = timeDiff;

                                //if (_shiftTypeID == 2)
                                //{
                                //    if (shiftStart.TimeOfDay < normalShiftStart)
                                //    {
                                //        ShowMessageBox(new INMessageBoxWindow1(), "The Start Time is not allowed for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                //        return;
                                //    }

                                //    if (timeDiff > (normalMaxHours))
                                //    {
                                //        ShowMessageBox(new INMessageBoxWindow1(), "The Hours You have captured Exceed the allowed time for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                //        return;
                                //    }
                                //}
                                //else
                                //{
                                //    if (shiftStart.TimeOfDay < normalShift1Start)
                                //    {
                                //        ShowMessageBox(new INMessageBoxWindow1(), "The Start Time is not allowed for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                //        return;
                                //    }

                                //    if (timeDiff > (normalShift1MaxHours))
                                //    {
                                //        ShowMessageBox(new INMessageBoxWindow1(), "The Hours You have captured Exceed the allowed time for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                //        return;
                                //    }
                                //}

                            }

                            else //8.5 hours max (normal time
                            {
                                double timeDiff = shiftEnd.Subtract(shiftStart).TotalHours;
                                timeDiffNormalHours = timeDiff;

                                if (_shiftTypeID == 2)
                                {
                                    if (shiftStart.TimeOfDay < ConfirmationNormalShiftStart)
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "The Start Time is not allowed for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                        return;
                                    }
                                    if (shiftEnd.TimeOfDay > ConfirmationNormalShiftEnd)
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "The End Time is not allowed for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                        return;
                                    }
                                    if (timeDiff > normalMaxHours)
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "The Hours You have captured Exceed the allowed time for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                        return;
                                    }
                                }

                                else
                                {
                                    //if (_shiftTypeID == 3)
                                    //{
                                    //    normalShift1End = normalShift1End.Add(new TimeSpan(3, 30, 0));
                                    //    normalShift1Start = normalShift1Start.Add(new TimeSpan(3, 30, 0));
                                    //}

                                    //if (shiftStart.TimeOfDay < normalShift1Start)
                                    //{
                                    //    ShowMessageBox(new INMessageBoxWindow1(), "The Start Time is not allowed for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                    //    return;
                                    //}
                                    //if (shiftEnd.TimeOfDay > normalShift1End)
                                    //{
                                    //    ShowMessageBox(new INMessageBoxWindow1(), "The End Time is not allowed for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                    //    return;
                                    //}

                                    if (timeDiff > normalShift1MaxHours)
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "The Hours You have captured Exceed the allowed time for Normal Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                        return;
                                    }
                                }
                            }

                            userHours.NormalShiftStartTime = shiftStart.TimeOfDay;
                            userHours.NormalShiftEndTime = shiftEnd.TimeOfDay;
                            canSave = true;
                        }

                        #endregion Evaluating the normal shift time range

                        #region Evaluating the evening shift time range

                        if (dteConfirmationEveningShiftStartTime.Value != null && dteConfirmationEveningShiftEndTime.Value != null)
                        {
                            if (_isPublicHoliday)
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "Evening Time cannot be recorded for public Holiday ", "Incorrect Hours", ShowMessageType.Exclamation);
                                return;
                            }
                            DateTime shiftStart = DateTime.Parse(dteConfirmationEveningShiftStartTime.Value.ToString());
                            DateTime shiftEnd = DateTime.Parse(dteConfirmationEveningShiftEndTime.Value.ToString());

                            DateTime selectedDate = DateTime.Parse(dteConfirmationDate.Value.ToString());
                            if (shiftEnd.TimeOfDay > ConfirmationEveningShiftEnd)
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "The End Time is not allowed for Evening Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                return;
                            }
                            if (selectedDate.DayOfWeek == DayOfWeek.Friday)
                            {
                                //if (_shiftTypeID == 3)
                                //{
                                //    normalShift1End.Add(new TimeSpan(2, 0, 0));
                                //}
                                if (_shiftTypeID == 1)
                                {
                                    if (shiftStart.TimeOfDay < ConfirmationEveningShiftStart.Subtract(new TimeSpan(0, 30, 0)))
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "The Start Time is not allowed for Evening Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                        return;
                                    }
                                }
                                else
                                {
                                    if (shiftStart.TimeOfDay < ConfirmationEveningShiftStart)
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "The Start Time is not allowed for Evening Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (_shiftTypeID == 3)
                                {
                                    ConfirmationEveningShiftStart = ConfirmationEveningShiftStart.Add(new TimeSpan(3, 30, 0));
                                }
                                if (_shiftTypeID == 2)
                                {
                                    ConfirmationEveningShiftStart = ConfirmationEveningShiftStart.Add(new TimeSpan(1, 0, 0));
                                }
                                if (shiftStart.TimeOfDay < ConfirmationEveningShiftStart)
                                {
                                    ShowMessageBox(new INMessageBoxWindow1(), "The Start Time is not allowed for Evening Shift ", "Incorrect Hours", ShowMessageType.Exclamation);
                                    return;
                                }
                            }

                            userHours.EveningShiftStartTime = shiftStart.TimeOfDay;
                            userHours.EveningShiftEndTime = shiftEnd.TimeOfDay;
                            canSave = true;
                        }

                        #endregion Evaluating the evening shift time range

                        #region Evaluating the weekend / public holiday shift time range

                        if (dteConfirmationWeekendPublicHolidayShiftStartTime.Value != null && dteConfirmationWeekendPublicHolidayShiftEndTime.Value != null)
                        {
                            DateTime shiftStart = DateTime.Parse(dteConfirmationWeekendPublicHolidayShiftStartTime.Value.ToString());
                            DateTime shiftEnd = DateTime.Parse(dteConfirmationWeekendPublicHolidayShiftEndTime.Value.ToString());

                            userHours.PublicHolidayWeekendShiftStartTime = shiftStart.TimeOfDay;
                            userHours.PublicHolidayWeekendShiftEndTime = shiftEnd.TimeOfDay;
                            canSave = true;
                        }

                        #endregion Evaluating the weekend / public holiday shift time range

                        #region Are the hours being captured for a redeemed batch?

                        //if (rbIsRedeemedNeither.IsChecked.HasValue) //((!rbIsRedeemedYes.IsChecked.HasValue) && (!rbIsRedeemedNo.IsChecked.HasValue))
                        //{
                        //    if (rbIsRedeemedNeither.IsChecked.Value)
                        //    {
                        //        userHours.IsRedeemedHours = null;
                        //    }
                        //}

                        //else
                        //{
                        //    if (rbIsRedeemedYes.IsChecked.HasValue)
                        //    {
                        //        if (rbIsRedeemedYes.IsChecked.Value)
                        //        {
                        //            userHours.IsRedeemedHours = true;
                        //        }
                        //    }

                        //    else if (rbIsRedeemedNo.IsChecked.HasValue)
                        //    {
                        //        if (rbIsRedeemedNo.IsChecked.Value)
                        //        {
                        //            userHours.IsRedeemedHours = false;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        userHours.IsRedeemedHours = null;
                        //    }
                        //}


                        if (Convert.ToBoolean(rbConfirmationIsRedeemedNeither.IsChecked.Value))
                        {
                            userHours.IsRedeemedHours = null;
                        }

                        else if (Convert.ToBoolean(rbConfirmationIsRedeemedYes.IsChecked.Value))
                        {
                            userHours.IsRedeemedHours = true;
                        }

                        else if (Convert.ToBoolean(rbConfirmationIsRedeemedNo.IsChecked.Value))
                        {
                            userHours.IsRedeemedHours = false;
                        }
                        else
                        {
                            userHours.IsRedeemedHours = null;
                        }

                        #endregion Are the hours being captured for a redeemed batch?

                        if (canSave)
                        {
                            if (_selectedUserHourID == -1)
                            {
                                //check total hours already recorded for day shift
                                SqlParameter[] parameters = new SqlParameter[3];
                                parameters[0] = new SqlParameter("@AgentIDs", _selectedAgentID + ",");
                                parameters[1] = new SqlParameter("@FromDate", dteConfirmationDate.Value);
                                parameters[2] = new SqlParameter("@ToDate", dteConfirmationDate.Value);
                                DataTable dtData = Methods.ExecuteStoredProcedure("spReportHours", parameters).Tables[0];
                                double timeDiffRecordedNormalHours = 0;
                                if (dtData.Rows.Count > 0)
                                {
                                    foreach (DataRow rw in dtData.Rows)
                                    {
                                        if (rw["EmployeeName"].ToString() == cmbAgents.Text)
                                        {
                                            string totalNormaltTimeWorked = dtData.Rows[0]["TotalNormalTimeHoursWorked"].ToString().Replace(" ", "");
                                            var totalHoursWorked = TimeSpan.Parse(totalNormaltTimeWorked);
                                            timeDiffRecordedNormalHours = double.Parse(totalHoursWorked.TotalHours.ToString());
                                        }
                                    }
                                }

                                #region Checking for duplicates

                                //see if there is duplicate time for any campaign today

                                DateTime selDate = (DateTime)dteConfirmationDate.Value;
                                DataTable dtTimesToday = Methods.GetTableData("select * from UserHours where WorkingDate = '" + selDate.ToString("yyyy-MM-dd") + "' and FKUserID = " + _selectedAgentID);
                                if (dtTimesToday.Rows.Count > 0)
                                {
                                    foreach (DataRow row in dtTimesToday.Rows)
                                    {
                                        if (dteConfirmationMorningShiftStartTime.Value != null && dteConfirmationMorningShiftEndTime.Value != null)
                                        {
                                            DateTime MorningTimeCapturedStart = DateTime.Parse(dteConfirmationMorningShiftStartTime.Value.ToString());
                                            DateTime MorningTimeCapturedEnd = DateTime.Parse(dteConfirmationMorningShiftEndTime.Value.ToString());

                                            string morningSavedStartStr = row["MorningShiftStartTime"].ToString();
                                            string morningSavedEndStr = row["MorningShiftEndTime"].ToString();
                                            if (morningSavedStartStr != string.Empty)
                                            {
                                                DateTime morningSavedStart = DateTime.Parse(morningSavedStartStr);
                                                DateTime morningSavedEnd = DateTime.Parse(morningSavedEndStr);
                                                if ((morningSavedStart.TimeOfDay == MorningTimeCapturedStart.TimeOfDay) && (morningSavedEnd.TimeOfDay == MorningTimeCapturedEnd.TimeOfDay))
                                                {
                                                    ShowMessageBox(new INMessageBoxWindow1(), "Cannot Insert Duplicate Morning Shift Time ", "Incorrect Hours", ShowMessageType.Exclamation);
                                                    return;
                                                }
                                            }
                                        }
                                        if (dteConfirmationNormalShiftStartTime.Value != null && dteConfirmationNormalShiftEndTime.Value != null)
                                        {
                                            DateTime normalTimeCapturedStart = DateTime.Parse(dteConfirmationNormalShiftStartTime.Value.ToString());
                                            DateTime normalTimeCapturedEnd = DateTime.Parse(dteConfirmationNormalShiftEndTime.Value.ToString());

                                            string normalSavedStartStr = row["NormalShiftStartTime"].ToString();
                                            string normalSavedEndStr = row["NormalShiftEndTime"].ToString();
                                            if (normalSavedStartStr != string.Empty)
                                            {
                                                DateTime NormalSavedStart = DateTime.Parse(normalSavedStartStr);
                                                DateTime NormalSavedEnd = DateTime.Parse(normalSavedEndStr);
                                                if ((NormalSavedStart.TimeOfDay == normalTimeCapturedStart.TimeOfDay) && (NormalSavedEnd.TimeOfDay == normalTimeCapturedEnd.TimeOfDay))
                                                {
                                                    ShowMessageBox(new INMessageBoxWindow1(), "Cannot Insert Duplicate Normal Shift Time ", "Incorrect Hours", ShowMessageType.Exclamation);
                                                    return;
                                                }
                                            }
                                        }

                                        if (dteConfirmationEveningShiftStartTime.Value != null && dteConfirmationNormalShiftEndTime.Value != null)
                                        {
                                            DateTime eveningTimeCapturedStart = DateTime.Parse(dteConfirmationEveningShiftStartTime.Value.ToString());
                                            DateTime eveningTimeCapturedEnd = DateTime.Parse(dteConfirmationEveningShiftEndTime.Value.ToString());

                                            string eveningSavedStartStr = row["EveningShiftStartTime"].ToString();
                                            string eveningSavedEndStr = row["EveningShiftEndTime"].ToString();
                                            if (eveningSavedStartStr != string.Empty)
                                            {
                                                DateTime eveningSavedStart = DateTime.Parse(eveningSavedStartStr);
                                                DateTime eveningSavedEnd = DateTime.Parse(eveningSavedEndStr);
                                                if ((eveningSavedStart.TimeOfDay == eveningTimeCapturedStart.TimeOfDay) && (eveningSavedEnd.TimeOfDay == eveningTimeCapturedEnd.TimeOfDay))
                                                {
                                                    ShowMessageBox(new INMessageBoxWindow1(), "Cannot Insert Duplicate Evening Shift Time ", "Incorrect Hours", ShowMessageType.Exclamation);
                                                    return;
                                                }
                                            }
                                        }

                                        if (dteConfirmationWeekendPublicHolidayShiftStartTime.Value != null && dteConfirmationWeekendPublicHolidayShiftEndTime.Value != null)
                                        {
                                            DateTime weekendTimeCapturedStart = DateTime.Parse(dteConfirmationWeekendPublicHolidayShiftStartTime.Value.ToString());
                                            DateTime weekendTimeCapturedEnd = DateTime.Parse(dteConfirmationWeekendPublicHolidayShiftEndTime.Value.ToString());

                                            string weekendSavedStartStr = row["PublicHolidayWeekendShiftStartTime"].ToString();
                                            string weekendSavedEndStr = row["PublicHolidayWeekendShiftEndTime"].ToString();
                                            if (weekendSavedStartStr != string.Empty)
                                            {
                                                DateTime weekendSavedStart = DateTime.Parse(weekendSavedStartStr);
                                                DateTime weekendSavedEnd = DateTime.Parse(weekendSavedEndStr);
                                                if ((weekendSavedStart.TimeOfDay == weekendTimeCapturedStart.TimeOfDay) && (weekendSavedEnd.TimeOfDay == weekendTimeCapturedEnd.TimeOfDay))
                                                {
                                                    ShowMessageBox(new INMessageBoxWindow1(), "Cannot Insert Duplicate Public Holiday / Weekend Shift Time ", "Incorrect Hours", ShowMessageType.Exclamation);
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                }

                                #endregion Checking for duplicates

                                Double totalNormalTime = timeDiffNormalHours + timeDiffRecordedNormalHours;
                                if (_shiftTypeID == 1)
                                {
                                    if (totalNormalTime > normalShift1MaxHours)
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "The hours you are capturing exceed the day total", "Incorrect Hours", ShowMessageType.Exclamation);
                                        return;
                                    }
                                }
                                else
                                {
                                    if (totalNormalTime > normalMaxHours)
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "The hours you are capturing exceed the day total", "Incorrect Hours", ShowMessageType.Exclamation);
                                        return;
                                    }
                                }
                                //add normal hours recorded to existing ones

                            }

                            userHours.Save(_validationResult);
                            ShowMessageBox(new INMessageBoxWindow1(), "Hours Record Succesfully Saved ", "Save result", ShowMessageType.Information);
                            dteConfirmationMorningShiftStartTime.Value = null;
                            dteConfirmationMorningShiftEndTime.Value = null;

                            dteConfirmationNormalShiftStartTime.Value = null;
                            dteConfirmationNormalShiftEndTime.Value = null;

                            dteConfirmationEveningShiftStartTime.Value = null;
                            dteConfirmationEveningShiftEndTime.Value = null;

                            dteConfirmationWeekendPublicHolidayShiftStartTime.Value = null;
                            dteConfirmationWeekendPublicHolidayShiftEndTime.Value = null;

                            rbConfirmationIsRedeemedNo.IsChecked = false;
                            rbConfirmationIsRedeemedYes.IsChecked = false;
                            rbConfirmationIsRedeemedNeither.IsChecked = true;

                            //dteDate.Value = DateTime.Now.AddDays(1);
                            //dteDate.Value = DateTime.Now;
                        }
                        else
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), "Hours could not be saved. Please ensure that all relevant fields have been completed.", "Save result", ShowMessageType.Exclamation);
                        }


                        switch (((User)GlobalSettings.ApplicationUser).FKUserType)
                        {
                            case (int)lkpUserType.SeniorAdministrator:
                            case (int)lkpUserType.Administrator:
                            case (int)lkpUserType.Manager:
                                _selectedCampaignId = 0;
                                //txtboxConfirmationAgent.Text != null;
                                ShowGiftRedeemedControls(false);
                                break;

                            case (int)lkpUserType.SalesAgent:
                            case (int)lkpUserType.DataCapturer:
                            case (int)lkpUserType.ConfirmationAgent:
                            case (int)lkpUserType.CallMonitoringAgent:
                            case (int)lkpUserType.StatusLoader:
                                dteConfirmationDate.Value = DateTime.Now;
                                _selectedCampaignId = 0;
                                //txtboxConfirmationAgent.Text != null;
                                ShowGiftRedeemedControls(false);
                                break;

                            default:
                                dteConfirmationDate.Value = DateTime.Now;
                                _selectedCampaignId = 0;
                                //txtboxConfirmationAgent.Text != "";
                                ShowGiftRedeemedControls(false);
                                break;
                        }

                        LoadConfirmationAgentShiftTimes();
                    }
                    if (_selectedUserHourID > -1)//clear edit mode
                    {
                        ConfirmationStopEditing();
                    }

                }
                else
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please Select Agent ", "Select Agent", ShowMessageType.Error);
                }

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void dgConfirmationAgentTimes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //get select hour user id
                if (dgConfirmationAgentTimes.Items.Count > 0)
                {
                    DataRowView selectedItemRow = (DataRowView)dgConfirmationAgentTimes.SelectedItem;
                    _selectedUserHourID = long.Parse(selectedItemRow.Row["UserHoursID"].ToString());
                    DataRow selectedItem = Methods.GetTableData("select * from UserHours where ID = " + _selectedUserHourID).Rows[0];

                    #region Date
                    dteConfirmationDate.Value = selectedItem["WorkingDate"];
                    #endregion

                    #region Campaign
                    cmbConfirmationCampaigns.SelectedValue = selectedItem["FKINCampaignID"];


                    #endregion Campaign

                    #region Shift Type
                    cmbConfirmationShiftType.SelectedValue = selectedItem["FKShiftTypeID"];
                    #endregion ShiftType

                    #region Morning Shift
                    string morningShiftStart = selectedItem["MorningShiftStartTime"].ToString();
                    string morningShiftEnd = selectedItem["MorningShiftEndTime"].ToString();
                    if (morningShiftStart != string.Empty)
                    {
                        dteConfirmationMorningShiftStartTime.Value = morningShiftStart;
                    }
                    else
                    {
                        dteConfirmationMorningShiftStartTime.Value = null;
                    }
                    if (morningShiftEnd != string.Empty)
                    {
                        dteConfirmationMorningShiftEndTime.Value = morningShiftEnd;
                    }
                    else
                    {
                        dteConfirmationMorningShiftEndTime.Value = null;
                    }
                    #endregion Morning Shift

                    #region Normal Shift
                    string normalShiftStart = selectedItem["NormalShiftStartTime"].ToString();
                    string normalShiftEnd = selectedItem["NormalShiftEndTime"].ToString();
                    if (normalShiftStart != string.Empty)
                    {
                        dteConfirmationNormalShiftStartTime.Value = normalShiftStart;
                    }
                    else
                    {
                        dteConfirmationNormalShiftStartTime.Value = null;
                    }
                    if (normalShiftEnd != string.Empty)
                    {
                        dteConfirmationNormalShiftEndTime.Value = normalShiftEnd;
                    }
                    else
                    {
                        dteConfirmationNormalShiftEndTime.Value = null;
                    }
                    #endregion Normal Shift

                    #region evening shift
                    string eveningShiftStart = selectedItem["EveningShiftStartTime"].ToString();
                    string eveningShiftEnd = selectedItem["EveningShiftEndTime"].ToString();
                    if (eveningShiftStart != string.Empty)
                    {
                        dteConfirmationEveningShiftStartTime.Value = eveningShiftStart;
                    }
                    else
                    {
                        dteConfirmationEveningShiftStartTime.Value = null;
                    }
                    if (eveningShiftEnd != string.Empty)
                    {

                        dteConfirmationEveningShiftEndTime.Value = eveningShiftEnd;
                    }
                    else
                    {
                        dteConfirmationEveningShiftEndTime.Value = null;
                    }
                    #endregion evening shift

                    #region weekend and public holiday
                    string weekendPublicHolidayStart = selectedItem["PublicHolidayWeekendShiftStartTime"].ToString();
                    string weekendPublicHolidayEnd = selectedItem["PublicHolidayWeekendShiftEndTime"].ToString();
                    if (weekendPublicHolidayStart != string.Empty)
                    {
                        dteConfirmationWeekendPublicHolidayShiftStartTime.Value = weekendPublicHolidayStart;
                    }
                    else
                    {
                        dteConfirmationWeekendPublicHolidayShiftStartTime.Value = null;
                    }
                    if (weekendPublicHolidayEnd != string.Empty)
                    {
                        dteConfirmationWeekendPublicHolidayShiftEndTime.Value = weekendPublicHolidayEnd;
                    }
                    else
                    {
                        dteConfirmationWeekendPublicHolidayShiftEndTime.Value = null;
                    }
                    #endregion weekend and public holiday

                    #region Redeemed / Non-Redeemed Hours

                    bool? isRedeemedHours = selectedItem["IsRedeemedHours"] != DBNull.Value ? (bool)selectedItem["IsRedeemedHours"] : (bool?)null;

                    if (isRedeemedHours.HasValue)
                    {
                        if (isRedeemedHours.Value)
                        {
                            rbConfirmationIsRedeemedYes.IsChecked = true;
                            rbConfirmationIsRedeemedNo.IsChecked = false;
                            rbConfirmationIsRedeemedNeither.IsChecked = false;
                        }
                        else
                        {
                            rbConfirmationIsRedeemedYes.IsChecked = false;
                            rbConfirmationIsRedeemedNo.IsChecked = true;
                            rbConfirmationIsRedeemedNeither.IsChecked = false;
                        }
                    }
                    else
                    {
                        rbConfirmationIsRedeemedYes.IsChecked = false;
                        rbConfirmationIsRedeemedNo.IsChecked = false;
                        rbConfirmationIsRedeemedNeither.IsChecked = true;
                    }

                    #endregion Redeemed / Non-Redeemed Hours

                    lblConfirmationEditMode.Visibility = Visibility.Visible;
                    imgConfirmationEditMode.Visibility = Visibility.Visible;
                    btnConfirmationStopEditing.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void dgConfirmationAgentTimes_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime))
            {
                var dataGridTextColumn = e.Column as DataGridTextColumn;
                if (dataGridTextColumn != null)
                    dataGridTextColumn.Binding.StringFormat = "yyyy-MM-dd";
            }
        }

    }
}