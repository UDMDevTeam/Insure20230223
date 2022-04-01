using System;
using System.Data;
using Embriant.Framework.Data;
using Embriant.Framework.Configuration;

namespace UDM.Insurance.Business
{
    public class Insure
    {
        public static DataSet INGetDebiCheckAcceptedStatus(DateTime fromDate, DateTime toDate)
        {
            //SqlParameter[] parameters = new SqlParameter[3];
            //parameters[0] = new SqlParameter("@CampaignID", campaignID);
            //parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
            //parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

            //DataSet dsReducedPremiumReport = Methods.ExecuteStoredProcedure("spINReportDiary", parameters);

            object param1 = Database.GetParameter("@DateFrom", fromDate);
            object param2 = Database.GetParameter("@DateTo", toDate);

            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spReportDebiCheckAcceptedStatus", paramArray, 600);
        }

        public static DataSet INGetImportSummary()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetImportSummary", null);
        }
        public static DataTable INGetBatchAnalysisReportTypes()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetBatchAnalysisReportTypes", null).Tables[0];
        }
        public static DataTable INGetBatchAnalysisReportCampaignsOrCampaignTypesByReportType(byte reportType)
        {
            object param1 = Database.GetParameter("@INReportType", reportType);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetBatchAnalysisReportCampaignsOrCampaignTypesByReportType", paramArray).Tables[0];
        }
        public static DataSet INGetAssignLeadsSummaryData()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetAssignLeadsSummaryData", null);
        }

        public static DataSet INGetBatchAssignedLeadsData(long batchID, long activity, long workStatusEmployed)
        {
            object param1 = Database.GetParameter("@BatchID", batchID);
            object param2 = Database.GetParameter("@Activity", activity);
            object param3 = Database.GetParameter("@WorkStatusEmployed", workStatusEmployed);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetBatchAssignedLeadsData", paramArray);
        }

        public static DataSet INGetLeadBookAssignedMiningLeadsData(long leadBookID, long activity, long workStatusEmployed)
        {
            object param1 = Database.GetParameter("@LeadBookID", leadBookID);
            object param2 = Database.GetParameter("@Activity", activity);
            object param3 = Database.GetParameter("@WorkStatusEmployed", workStatusEmployed);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetLeadBookAssignedMiningLeadsData", paramArray);
        }

        #region Lead Printing - Specific Functionalities

        public static DataSet INGetAgentPrintLeadsData(long batchID, long agentID)
        {
            object param1 = Database.GetParameter("@BatchID", batchID);
            object param2 = Database.GetParameter("@AgentID", agentID);

            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetAgentPrintLeadsData", paramArray);
        }

        public static DataSet INGetLeadsToPrintCount(long batchID, long agentID, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@BatchID", batchID);
            object param2 = Database.GetParameter("@AgentID", agentID);
            object param3 = Database.GetParameter("@FromDate", fromDate);
            object param4 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3, param4 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetLeadsToPrintCount", paramArray);
        }

        public static DataSet INGetLeadsForUserAndBatch(long batchID, long agentID, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@BatchID", batchID);
            object param2 = Database.GetParameter("@AgentID", agentID);
            object param3 = Database.GetParameter("@FromDate", fromDate);
            object param4 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3, param4 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetLeadsForUserAndBatch", paramArray);
        }

        public static DataSet INGetLeadsForUserAndBatchACCDIS(long batchID, long agentID)
        {
            object param1 = Database.GetParameter("@BatchID", batchID);
            object param2 = Database.GetParameter("@AgentID", agentID);

            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetLeadsForUserAndBatch", paramArray);
        }

        public static DataTable INGetLeadbookConfigurationByCampaignID(long fkINCampaign)
        {
            object param1 = Database.GetParameter("@FKINCampaignID", fkINCampaign);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetLeadbookConfigurationByCampaignID", paramArray).Tables[0];
        }

        public static DataTable INGetLeadbookConfigurationSheMaccBase(long fkINCampaign)
        {
            object param1 = Database.GetParameter("@FKINCampaignID", fkINCampaign);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetLeadbookConfigurationForSheMaccBase", paramArray).Tables[0];
        }

        

        /// <summary>
        /// TODO: Adapt spINGetLeadsForUserAndBatchGeneric for the other campaigns as well
        /// </summary>
        /// <param name="batchID"></param>
        /// <param name="agentID"></param>
        /// <returns></returns>
        public static DataTable INGetLeadsForUserAndBatchGeneric(long batchID, long agentID, DateTime allocationDate)
        {
            object param1 = Database.GetParameter("@BatchID", batchID);
            object param2 = Database.GetParameter("@AgentID", agentID);
            object param3 = Database.GetParameter("@AllocationDate", allocationDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetLeadsForUserAndBatchGeneric", paramArray).Tables[0];
        }

        public static DataSet INGetLeadsAssignedToUser(long agentID)
        {
            object param1 = Database.GetParameter("@UserID", agentID);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetLeadsAssignedToUser", paramArray);
        }

        public static DataSet INGetLeadsAssignedToUserTraining(long agentID)
        {
            object param1 = Database.GetParameter("@UserID", agentID);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetLeadsAssignedToUserTraining", paramArray);
        }

        #endregion Lead Printing - Specific Functionalities

        #region Generic

        public static DataTable INGetCampaignTypes(bool includeAccDisCampaignGroup)
        {
            object param1 = Database.GetParameter("@IncludeAccDisCampaignGroup", includeAccDisCampaignGroup);
            object[] paramArray = new[] { param1 };
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetCampaignTypes", paramArray).Tables[0];
        }

        public static bool CanClientBeContacted(long? importID)
        {
            bool result = false;

            //if ((lkpUserType?)((User)GlobalSettings.ApplicationUser).FKUserType == lkpUserType.SalesAgent)
            //{
            if (importID.HasValue)
            {
                object param1 = Database.GetParameter("@ImportID", importID);
                object param2 = Database.GetParameter("@FKUserID", ((User)GlobalSettings.ApplicationUser).ID);
                object[] paramArray = new[] { param1, param2 };

                DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINDetermineLeadContactability", paramArray).Tables[0];

                result = (bool)dt.Rows[0][0];
            }
            else
            {
                result = true;
            }
            //}
            //else
            //{
            //    result = true;
            //}

            return result;
        }

        public static bool HasLeadBeenAllocated(long? fkINImportID)
        {
            bool result = false;

            if (fkINImportID.HasValue)
            {
                object param1 = Database.GetParameter("@FKINImportID", fkINImportID);
                object param2 = Database.GetParameter("@FKUserID", ((User)GlobalSettings.ApplicationUser).ID);
                object[] paramArray = new[] { param1, param2 };

                DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINHasLeadBeenAllocated", paramArray).Tables[0];

                result = (bool)dt.Rows[0][0];
            }
            else
            {
                result = true;
            }

            return result;
        }

        //Please see https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/204734160/comments
        public static bool HasLeadBeenCancelled(long? fkINImportID)
        {
            bool result = false;

            if (fkINImportID.HasValue)
            {
                object param1 = Database.GetParameter("@FKINImportID", fkINImportID);
                object param2 = Database.GetParameter("@FKUserID", ((User)GlobalSettings.ApplicationUser).ID);
                object[] paramArray = new[] { param1, param2 };

                DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINHasLeadBeenCancelled", paramArray).Tables[0];

                result = (bool)dt.Rows[0][0];
            }
            else
            {
                result = false;
            }

            return result;
        }

        public static DataSet INDetermineRemainingSales(long fkINImportID)
        {
            object param1 = Database.GetParameter("@FKINImportID", fkINImportID);
            //object param2 = Database.GetParameter("@FKUserID", ((User)GlobalSettings.ApplicationUser).ID);
            object[] paramArray = new[] { param1 };

            //return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINDetermineRemainingSales", paramArray).Tables[0].Rows[0][0].ToString();
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINDetermineRemainingSales", paramArray);
        }

        public static bool IsUpgradeCampaign(long? campaignID)
        {
            bool result = false;

            if (campaignID.HasValue)
            {
                object param1 = Database.GetParameter("@FKINCampaignID", campaignID);
                object[] paramArray = new[] { param1 };

                DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINIsUpgradeCampaign", paramArray).Tables[0];

                result = (bool)dt.Rows[0][0];
            }
            else
            {
                result = false;
            }

            return result;
        }

        public static bool IsUpgradeCampaignCluster(long? campaignClusterID)
        {
            bool result = false;

            if (campaignClusterID.HasValue)
            {
                object param1 = Database.GetParameter("@FKINCampaignClusterID", campaignClusterID);
                object[] paramArray = new[] { param1 };

                DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINIsUpgradeCampaignCluster", paramArray).Tables[0];

                result = (bool)dt.Rows[0][0];
            }
            else
            {
                result = false;
            }

            return result;
        }

        public static DataTable INGetCampaigns(bool onlyUpgradeCampaigns)
        {
            object param1 = Database.GetParameter("@OnlyUpgradeCampaigns", onlyUpgradeCampaigns);
            object[] paramArray = new[] { param1 };
            DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetCampaigns", paramArray).Tables[0];
            return dt;
        }

        public static bool UserHasAccessToReport(long fkUserID)
        {
            bool result = false;

            object param1 = Database.GetParameter("@FKUserID", fkUserID);
            object[] paramArray = new[] { param1 };

            DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINUserHasAccessToReport", paramArray).Tables[0];

            result = (bool)dt.Rows[0][0];

            return result;
        }

        public static DateTime GetAvailableWorkingDayFromDate(DateTime date)
        {
            DateTime availableWorkingDay;

            object param1 = Database.GetParameter("@Date", date);
            object[] paramArray = new[] { param1 };

            DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spGetAvailableWorkingDayFromDate", paramArray).Tables[0];

            availableWorkingDay = (DateTime)dt.Rows[0][0];

            return availableWorkingDay;
        }

        public static DateTime INDetermineBatchingDateFromDateOfSale(DateTime dateOfSale, int daysToAdd)
        {
            DateTime batchingDate;

            object param1 = Database.GetParameter("@DateOfSale", dateOfSale);
            object param2 = Database.GetParameter("@DaysToAdd", daysToAdd);
            object[] paramArray = new[] { param1, param2 };

            DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINDetermineBatchingDateFromDateOfSale", paramArray).Tables[0];

            batchingDate = (DateTime)dt.Rows[0][0];

            return batchingDate;
        }

        public static DataTable INGetBatchesByCampaignID(long fkINCampaignID)
        {
            object param1 = Database.GetParameter("@FKINCampaignID", fkINCampaignID);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetBatchesByCampaignID", paramArray).Tables[0];
        }

        public static DataTable INGetSalesAgents()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spGetSalesAgents", null).Tables[0];
        }

        /// <summary>
        /// Returns a list of sales consultants and adds the list of users in fkUserIDs if they are not in the list of users
        /// </summary>
        /// <param name="fkUserIDs">A string of numeric, comma-separated FKUserID values</param>
        /// <returns></returns>
        public static DataTable INGetUsersAndAddMissingUser(string fkUserIDs)
        {
            object param1 = Database.GetParameter("@FKUserIDs", fkUserIDs);
            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetUsersAndAddMissingUser", paramArray).Tables[0];
        }

        public static bool IsNotesFeatureAvailable(long? fkINImportID)
        {
            bool result = false;

            if (fkINImportID.HasValue)
            {
                object param1 = Database.GetParameter("@FKINImportID", fkINImportID);
                object param2 = Database.GetParameter("@FKUserID", ((User)GlobalSettings.ApplicationUser).ID);
                object[] paramArray = new[] { param1, param2 };

                DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINIsNotesFeatureAvailable", paramArray).Tables[0];

                result = (bool)dt.Rows[0][0];
            }
            else
            {
                result = false;
            }

            return result;
        }

        public static DataTable AddMissingLeadStatuses(long? fkINImportID)
        {
            object param1;

            if (!fkINImportID.HasValue)
            {
                param1 = Database.GetParameter("@FKINImportID", DBNull.Value);
            }
            else
            {
                param1 = Database.GetParameter("@FKINImportID", fkINImportID);
            }

            object param2 = Database.GetParameter("@FKLoggedInUserID", ((User)GlobalSettings.ApplicationUser).ID);
            object[] paramArray = new[] { param1, param2 };

            DataTable dtResultingLeadStatuses = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINAddMissingLeadStatuses", paramArray).Tables[0];

            return dtResultingLeadStatuses;
        }

        public static DataSet UpdateLeadApplicationScreenLookups(long? importID)
        {
            object param1;

            if (!importID.HasValue)
            {
                param1 = Database.GetParameter("@ImportID", DBNull.Value);
            }
            else
            {
                param1 = Database.GetParameter("@ImportID", importID);
            }

            //object param2 = Database.GetParameter("@FKLoggedInUserID", ((User)GlobalSettings.ApplicationUser).ID);
            object[] paramArray = new[] { param1 };

            DataSet dsUpdatedLeadApplicationScreenLookups = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINUpdateLeadApplicationScreenLookups", paramArray);

            return dsUpdatedLeadApplicationScreenLookups;
        }

        public static string GetLoggedInUserNameAndSurname()
        {
            return String.Format("{0} {1}", ((User)GlobalSettings.ApplicationUser).FirstName.Trim(), ((User)GlobalSettings.ApplicationUser).LastName.Trim());
        }

        /// <summary>
        /// TODO: Find a way to specifiy an ImportID to this method so that the loaded date of sale can be obtained from the lead directly
        /// </summary>
        /// <returns></returns>
        public static int INCalculateConfirmationWorkWindow()
        {
            // Give it a default value
            int resultingHoursToDeductFromToday = -120;

            DataTable dtResult = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINCalculateConfirmationWorkWindow", null).Tables[0];
            if (dtResult.Rows.Count > 0)
            {
                resultingHoursToDeductFromToday = Convert.ToInt32(dtResult.Rows[0][0]);
            }

            return resultingHoursToDeductFromToday;
        }

        
        public static bool IsRedeemedGiftFieldsModifiable() //public static bool INCanUserChangeRedeemedGiftDetails()
        {
            bool result = false;

            object param1 = Database.GetParameter("@LoggedInUserID", Embriant.Framework.Configuration.GlobalSettings.ApplicationUser.ID);
            object[] paramArray = new[] { param1 };

            DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spIsRedeemedGiftFieldsModifiable", paramArray).Tables[0];

            result = (bool)dt.Rows[0][0];

            return result;
        }

        public static bool INCanUserEditRedeemedGiftDetails(long? fkINImportID)
        {
            bool result = false;
            if (fkINImportID == null)
            {
                return result;
            }

            object param1 = Database.GetParameter("@LoggedInUserID", GlobalSettings.ApplicationUser.ID);
            object param2 = Database.GetParameter("@FKINImportID", fkINImportID.Value);
            object[] paramArray = new[] { param1, param2 };

            DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINCanUserEditRedeemedGiftDetails", paramArray).Tables[0];

            result = (bool)dt.Rows[0][0];

            return result;
        }

        public static DataTable INGetOptionDetailsFromID(long fkINOptionID)
        {
            object param1 = Database.GetParameter("@FKINOptionID", fkINOptionID);
            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetOptionDetailsFromID", paramArray).Tables[0];
        }


        public static DataTable INGetDeclineReasons(long? fkINImportID)
        {
            object param1 = Database.GetParameter("@LoggedInUserID", Embriant.Framework.Configuration.GlobalSettings.ApplicationUser.ID);
            object param2;

            if (fkINImportID.HasValue)
            {
                param2 = Database.GetParameter("@FKINImportID", fkINImportID.Value);
            }
            else
            {
                param2 = Database.GetParameter("@FKINImportID", DBNull.Value);
            }

            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetDeclineReasons", paramArray).Tables[0];
        }

        public static DataTable INGetCallMonitoringCarriedForwardReasons()
        {
            object param1 = Database.GetParameter("@LoggedInUserID", Embriant.Framework.Configuration.GlobalSettings.ApplicationUser.ID);
            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetCallMonitoringCarriedForwardReasons", paramArray).Tables[0];
        }

        #endregion Generic

        #region Lead Analysis Report - Specific Functionalities

        /// <summary>
        /// Returns a DataSet consisting of 3 DataTables containing:
        /// 1. The campaigns
        /// 2. A list of possible input
        /// </summary>
        /// <returns></returns>
        public static DataSet GetLeadAnalysisReportLookups()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetLeadAnalysisReportLookups", null);
        }


        // Will most likely not be used again. This was used in the Lead Analysis Report
        public static DataTable GetLeadAnalysisDetailsForRefNo(long batchID, string refNo)
        {
            object param1 = Database.GetParameter("@FKINBatchID", batchID);
            object param2 = Database.GetParameter("@RefNo", refNo);
            object[] paramArray = new[] { param1, param2 };
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetLeadAnalysisDetailsForRefNo", paramArray).Tables[0];
        }

        public static DataTable GetLeadAnalysisReportBatchesByCampaignID(long campaignID)
        {
            object param1 = Database.GetParameter("@FKINCampaignID", campaignID);
            object[] paramArray = new[] { param1 };

            DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetLeadAnalysisReportBatchesByCampaignID", paramArray).Tables[0];

            return dt;
        }

        /// <summary>
        /// Will most likely not be used again
        /// </summary>
        /// <param name="batchID"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DataSet INGetLeadAnalysisReportData(long batchID, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@FKINBatchID", batchID);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportLeadsAnalysis", paramArray);
        }

        public static DataSet INGetLeadAnalysisReportDataByCampaign(long campaignID, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@FKINCampaignID", campaignID);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportLeadsAnalysisByCampaign", paramArray);
        }

        #endregion Lead Analysis Report - Specific Functionalities

        #region Salary Report - Specific Functionalities

        //public static bool IsUpgradeCampaignCluster(long? campaignClusterID)
        //{
        //    bool result = false;

        //    if (campaignClusterID.HasValue)
        //    {
        //        object param1 = Database.GetParameter("@INCampaignClusterID", campaignClusterID);
        //        object[] paramArray = new[] { param1 };

        //        DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spIsUpgradeCampaignCluster", paramArray).Tables[0];

        //        result = (bool)dt.Rows[0][0];
        //    }
        //    else
        //    {
        //        result = false;
        //    }

        //    return result;
        //}

        public static DataSet INGetSalaryReportScreenLookups(/*byte reportType*/)
        {
            //object param1 = Database.GetParameter("@ReportType", reportType);
            //object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetSalaryReportScreenLookups", null);

        }

        public static DataTable INGetSalaryReportDataForMultipleCampaignClusters(bool includeSystemUnitsColumn, string campaignClusterIDs, byte reportType, DateTime fromDate, DateTime toDate, bool bonusSales)
        {
            object param1 = Database.GetParameter("@IncludeSystemUnits", includeSystemUnitsColumn);
            object param2 = Database.GetParameter("@CampaignClusterIDList", campaignClusterIDs);
            object param3 = Database.GetParameter("@ReportType", reportType);
            object param4 = Database.GetParameter("@FromDate", fromDate);
            object param5 = Database.GetParameter("@ToDate", toDate);
            object param6 = Database.GetParameter("@BonusSales", bonusSales);

            object[] paramArray = new[] { param1, param2, param3, param4, param5, param6 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSalaryMultipleCampaignClusters", paramArray).Tables[0];
        }

        public static DataTable INGetSalaryReportDataForSingleCampaignCluster(bool includeSystemUnitsColumn, long campaignClusterID, byte reportType, DateTime fromDate, DateTime toDate, bool bonusSales)
        {
            object param1 = Database.GetParameter("@IncludeSystemUnits", includeSystemUnitsColumn);
            object param2 = Database.GetParameter("@CampaignClusterID", campaignClusterID);
            object param3 = Database.GetParameter("@ReportType", reportType);
            object param4 = Database.GetParameter("@FromDate", fromDate);
            object param5 = Database.GetParameter("@ToDate", toDate);
            object param6 = Database.GetParameter("@BonusSales", bonusSales);

            object[] paramArray = new[] { param1, param2, param3, param4, param5, param6 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSalarySingleCampaignCluster", paramArray).Tables[0];
        }

        public static DataTable INGetSalaryReportDataForMultipleCampaigns(bool includeSystemUnitsColumn, string campaignIDList, byte reportType, DateTime fromDate, DateTime toDate, bool bonusSales)
        {
            object param1 = Database.GetParameter("@IncludeSystemUnits", includeSystemUnitsColumn);
            object param2 = Database.GetParameter("@CampaignIDList", campaignIDList);
            object param3 = Database.GetParameter("@ReportType", reportType);
            object param4 = Database.GetParameter("@FromDate", fromDate);
            object param5 = Database.GetParameter("@ToDate", toDate);
            object param6 = Database.GetParameter("@BonusSales", bonusSales);

            object[] paramArray = new[] { param1, param2, param3, param4, param5, param6 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSalaryMultipleCampaigns", paramArray).Tables[0];
        }

        public static DataTable INGetSalaryReportDataForSingleCampaign(bool includeSystemUnitsColumn, long campaignID, byte reportType, DateTime fromDate, DateTime toDate, bool bonusSales)
        {
            object param1 = Database.GetParameter("@IncludeSystemUnits", includeSystemUnitsColumn);
            object param2 = Database.GetParameter("@CampaignID", campaignID);
            object param3 = Database.GetParameter("@ReportType", reportType);
            object param4 = Database.GetParameter("@FromDate", fromDate);
            object param5 = Database.GetParameter("@ToDate", toDate);
            object param6 = Database.GetParameter("@BonusSales", bonusSales);

            object[] paramArray = new[] { param1, param2, param3, param4, param5, param6 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSalarySingleCampaign", paramArray).Tables[0];
        }

        public static DataSet INReportSalaryGeneric(bool includeSystemUnitsColumn, string fkINCampaignFKINCampaignClusterIDs, byte reportType, DateTime fromDate, DateTime toDate, bool bonusSales, bool useCampaignClusters)
        {
            object param1 = Database.GetParameter("@IncludeSystemUnits", includeSystemUnitsColumn);
            object param2 = Database.GetParameter("@FKINCampaignFKINCampaignClusterIDs", fkINCampaignFKINCampaignClusterIDs);
            object param3 = Database.GetParameter("@ReportType", reportType);
            object param4 = Database.GetParameter("@FromDate", fromDate);
            object param5 = Database.GetParameter("@ToDate", toDate);
            object param6 = Database.GetParameter("@BonusSales", bonusSales);
            object param7 = Database.GetParameter("@UseCampaignClusters", useCampaignClusters);

            object[] paramArray = new[] { param1, param2, param3, param4, param5, param6, param7 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSalaryGeneric", paramArray, 300);
        }

        public static DataSet INReportSalaryTemp(string fkUserIDs,  DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@FKUserIDs", fkUserIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);
            object param4 = Database.GetParameter("@IncludeInactiveAgents", false);

            object[] paramArray = new[] { param1, param2, param3, param4 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSalaryTemp", paramArray, 600);
        }

        public static DataSet INReportSalaryTemp(string fkUserIDs, DateTime fromDate, DateTime toDate, bool includeInactiveAgents)
        {
            object param1 = Database.GetParameter("@FKUserIDs", fkUserIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);
            object param4 = Database.GetParameter("@IncludeInactiveAgents", includeInactiveAgents);

            object[] paramArray = new[] { param1, param2, param3, param4 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSalaryTemp", paramArray, 1800);
        }

        public static DataSet INReportSalaryTempPostJune(string fkUserIDs, DateTime fromDate, DateTime toDate, bool includeInactiveAgents)
        {
            object param1 = Database.GetParameter("@FKUserIDs", fkUserIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);
            object param4 = Database.GetParameter("@IncludeInactiveAgents", includeInactiveAgents);

            object[] paramArray = new[] { param1, param2, param3, param4 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSalaryTempPostJune2021", paramArray, 1800);
        }


        #endregion Salary Report - Specific Functionalities

        #region Batch & Stats Report - Specific Functionalities

        public static DataTable INReportBatchStatsDailyTSR(DateTime fromDate, DateTime toDate, long campaignID)
        {
            object param1 = Database.GetParameter("@StartDate", fromDate);
            object param2 = Database.GetParameter("@EndDate", toDate);
            object param3 = Database.GetParameter("@CampaignID", campaignID);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportBatchStatsDailyTSR", paramArray).Tables[0];
        }

        public static DataTable INReportBatchStatsMonthlyCampaigns(DateTime fromDate, DateTime toDate, bool onlyUpgradeCampaigns)
        {
            object param1 = Database.GetParameter("@StartDate", fromDate);
            object param2 = Database.GetParameter("@EndDate", toDate);
            object param3 = Database.GetParameter("@OnlyUpgradeCampaigns", onlyUpgradeCampaigns);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportBatchStatsMonthlyCampaigns", paramArray).Tables[0];
        }

        #endregion Batch & Stats Report - Specific Functionalities

        #region STL Report - Specific Functionalities

        public static DataSet INGetSTLLookups()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetSTLLookups", null);
        }

        public static DataSet INGetUsersByCampaignAndDateRange(long campaignID, DateTime fromDate, DateTime toDate, byte staffType)
        {
            object param1 = Database.GetParameter("@FKINCampaignID", campaignID);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);
            object param4 = Database.GetParameter("@StaffType", staffType);

            object[] paramArray = new[] { param1, param2, param3, param4 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetUsersByCampaignAndDateRange", paramArray);
        }

        public static DataSet INReportSTL(long campaignID, long agentID, DateTime fromDate, DateTime toDate, int stlOption)
        {
            object param1 = Database.GetParameter("@FKINCampaignID", campaignID);
            object param2 = Database.GetParameter("@FKUserID", agentID);
            object param3 = Database.GetParameter("@FromDate", fromDate);
            object param4 = Database.GetParameter("@ToDate", toDate);
            object param5 = Database.GetParameter("@STLOption", stlOption);

            object[] paramArray = new[] { param1, param2, param3, param4, param5 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSTL", paramArray, 1800);
        }

        public static DataSet INReportSTLUpgrades(string fkINCampaignIDs, DateTime fromDate, DateTime toDate, byte staffType)
        {
            object param1 = Database.GetParameter("@FKINCampaignIDs", fkINCampaignIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);
            object param4 = Database.GetParameter("@StaffType", staffType);

            object[] paramArray = new[] { param1, param2, param3, param4 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSTLUpgrades", paramArray, 1800);
        }

        public static DataSet INReportSTLUpgrades2(string fkINCampaignIDs, DateTime fromDate, DateTime toDate, byte fkStaffTypeID, byte stlOption, byte stlConversionPercentageOption)
        {
            object param1 = Database.GetParameter("@FKINCampaignIDs", fkINCampaignIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);
            object param4 = Database.GetParameter("@StaffType", fkStaffTypeID);
            object param5 = Database.GetParameter("@STLOption", stlOption);
            object param6 = Database.GetParameter("@STLConversionPercentageOption", stlConversionPercentageOption);

            object[] paramArray = new[] { param1, param2, param3, param4, param5, param6 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSTLUpgrades2", paramArray, 1800);
        }

        public static DataSet INReportSTLUpgrades20180621(string fkINCampaignIDs, DateTime fromDate, DateTime toDate, byte fkStaffTypeID, byte stlOption, byte stlConversionPercentageOption)
        {
            object param1 = Database.GetParameter("@FKINCampaignIDs", fkINCampaignIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);
            object param4 = Database.GetParameter("@StaffType", fkStaffTypeID);
            object param5 = Database.GetParameter("@STLOption", stlOption);
            object param6 = Database.GetParameter("@STLConversionPercentageOption", stlConversionPercentageOption);

            object[] paramArray = new[] { param1, param2, param3, param4, param5, param6 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSTLUpgrades20180621", paramArray, 1800);
        }

        public static DataSet INReportSTLUpgrades20180723(string fkINCampaignIDs, DateTime fromDate, DateTime toDate, byte fkStaffTypeID, byte stlOption, byte stlConversionPercentageOption, byte batchType)
        {
            object param1 = Database.GetParameter("@FKINCampaignIDs", fkINCampaignIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);
            object param4 = Database.GetParameter("@StaffType", fkStaffTypeID);
            object param5 = Database.GetParameter("@STLOption", stlOption);
            object param6 = Database.GetParameter("@STLConversionPercentageOption", stlConversionPercentageOption);
            object param7 = Database.GetParameter("@BatchType", batchType);

            object[] paramArray = new[] { param1, param2, param3, param4, param5, param6, param7 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSTLUpgrades20180723", paramArray, 1800);
        }

        public static DataSet INReportSTLMultipleCampaigns(string fkINCampaignIDs, DateTime fromDate, DateTime toDate, byte fkStaffTypeID, byte stlOption, byte stlConversionPercentageOption, byte batchType)
        {
            object param1 = Database.GetParameter("@FKINCampaignIDs", fkINCampaignIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);
            object param4 = Database.GetParameter("@FKStaffTypeID", fkStaffTypeID);
            object param5 = Database.GetParameter("@STLOption", stlOption);
            object param6 = Database.GetParameter("@STLConversionPercentageOption", stlConversionPercentageOption);
            object param7 = Database.GetParameter("@BatchType", batchType);

            object[] paramArray = new[] { param1, param2, param3, param4, param5, param6, param7 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSTLMultipleCampaigns", paramArray, 1800);
        }

        public static DataSet INReportSTLMultipleCampaignsLevel2(string fkINCampaignIDs, DateTime fromDate, DateTime toDate, byte fkStaffTypeID, byte stlOption, byte stlConversionPercentageOption, byte batchType)
        {
            object param1 = Database.GetParameter("@FKINCampaignIDs", fkINCampaignIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);
            object param4 = Database.GetParameter("@FKStaffTypeID", fkStaffTypeID);
            object param5 = Database.GetParameter("@STLOption", stlOption);
            object param6 = Database.GetParameter("@STLConversionPercentageOption", stlConversionPercentageOption);
            object param7 = Database.GetParameter("@BatchType", batchType);

            object[] paramArray = new[] { param1, param2, param3, param4, param5, param6, param7 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSTLMultipleCampaignsLevel2", paramArray, 1800);
        }

        public static DataSet INReportSTLMultipleCampaignsJuly2017Structure(string fkINCampaignIDs, DateTime fromDate, DateTime toDate, byte fkStaffTypeID, byte stlOption, byte stlConversionPercentageOption)
        {
            object param1 = Database.GetParameter("@FKINCampaignIDs", fkINCampaignIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);
            object param4 = Database.GetParameter("@FKStaffTypeID", fkStaffTypeID);
            object param5 = Database.GetParameter("@STLOption", stlOption);
            object param6 = Database.GetParameter("@STLConversionPercentageOption", stlConversionPercentageOption);

            object[] paramArray = new[] { param1, param2, param3, param4, param5, param6 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSTLMultipleCampaignsJuly2017Structure", paramArray, 1800);
        }

        public static DataSet INReportSTLMultipleCampaignsUpgrades(string fkINCampaignIDs, DateTime fromDate, DateTime toDate, byte fkStaffTypeID, byte stlOption, byte stlConversionPercentageOption)
        {
            object param1 = Database.GetParameter("@FKINCampaignIDs", fkINCampaignIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);
            object param4 = Database.GetParameter("@FKStaffTypeID", fkStaffTypeID);
            object param5 = Database.GetParameter("@STLOption", stlOption);
            object param6 = Database.GetParameter("@STLConversionPercentageOption", stlConversionPercentageOption);

            object[] paramArray = new[] { param1, param2, param3, param4, param5, param6 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSTLUpgrades2", paramArray, 1800);
        }


        //public static DataSet INGetSTLDataByUserCampaignAndDateRange(long campaignID, long agentID, DateTime fromDate, DateTime toDate)
        //{
        //    object param1 = Database.GetParameter("@FKINCampaignID", campaignID);
        //    object param2 = Database.GetParameter("@FKUserID", agentID);
        //    object param3 = Database.GetParameter("@FromDate", fromDate);
        //    object param4 = Database.GetParameter("@ToDate", toDate);

        //    object[] paramArray = new[] { param1, param2, param3, param4 };

        //    return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSTLByUserCampaignAndDateRange", paramArray);
        //}

        //public static DataSet INGetSTLDataByUserCampaignAndDateRange20151019(long campaignID, long agentID, DateTime fromDate, DateTime toDate)
        //{
        //    object param1 = Database.GetParameter("@FKINCampaignID", campaignID);
        //    object param2 = Database.GetParameter("@FKUserID", agentID);
        //    object param3 = Database.GetParameter("@FromDate", fromDate);
        //    object param4 = Database.GetParameter("@ToDate", toDate);

        //    object[] paramArray = new[] { param1, param2, param3, param4 };

        //    return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSTLByUserCampaignAndDateRange20151019", paramArray);
        //}

        //public static DataSet INGetSTLDataByUserCampaignAndDateRange20151116(long campaignID, long agentID, DateTime fromDate, DateTime toDate)
        //{
        //    object param1 = Database.GetParameter("@FKINCampaignID", campaignID);
        //    object param2 = Database.GetParameter("@FKUserID", agentID);
        //    object param3 = Database.GetParameter("@FromDate", fromDate);
        //    object param4 = Database.GetParameter("@ToDate", toDate);

        //    object[] paramArray = new[] { param1, param2, param3, param4 };

        //    return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSTLByUserCampaignAndDateRange20151116", paramArray);

        //}

        public static DataSet INGetSTLDataByUserCampaignAndDateRange20151116(long campaignID, long agentID, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@FKINCampaignID", campaignID);
            object param2 = Database.GetParameter("@FKUserID", agentID);
            object param3 = Database.GetParameter("@FromDate", fromDate);
            object param4 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3, param4 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSTLByUserCampaignAndDateRange20151116", paramArray);

        }

        #endregion STL Report - Specific Functionalities

        #region Lead Allocation Report - Specific Functionalities

        public static DataSet INGetLeadAllocationReportData(long campaignID, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@StartDate", fromDate);
            object param2 = Database.GetParameter("@EndDate", toDate);
            object param3 = Database.GetParameter("@CampaignId", campaignID);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportLeadAllocation", paramArray);
        }

        #endregion Lead Allocation Report - Specific Functionalities

        #region Web Redeem Report

        public static DataSet INGetWebRedeemReportData(string campaignID, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@StartDate", fromDate);
            object param2 = Database.GetParameter("@EndDate", toDate);
            object param3 = Database.GetParameter("@CampaignId", campaignID);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spWebRedeem", paramArray);
        }

        #endregion

        #region No Contact Report - Specific Functionalities

        public static DataTable INGetReportNoContactReportData(string batchCode, long campaignID)
        {
            object param1 = Database.GetParameter("@BatchCode", batchCode);
            object param2 = Database.GetParameter("@CampaignID", campaignID);

            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportNoContact", paramArray).Tables[0];
        }

        public static DataTable GetBatchesForNoContactReport(long campaignID)
        {
            object param1 = Database.GetParameter("@CampaignID", campaignID);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetBatchesForNoContactReport", paramArray).Tables[0];
        }

        #endregion No Contact Report - Specific Functionalities

        #region Confirmed Sales Report - Specific Functionalities

        public static DataTable INGetConfirmedSalesReportCampaignGroupings()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetConfirmedSalesReportCampaignGroupings", null).Tables[0];
        }

        //public static DataSet INGetConfirmedSalesReportData(byte fkINConfirmedSalesReportCampaignGroupID, DateTime fromDate, DateTime toDate)
        //{
        //    object param1 = Database.GetParameter("@FKINConfirmedSalesReportCampaignGroupID", fkINConfirmedSalesReportCampaignGroupID);
        //    object param2 = Database.GetParameter("@FromDate", fromDate);
        //    object param3 = Database.GetParameter("@ToDate", toDate);

        //    object[] paramArray = new[] { param1, param2, param3 };

        //    return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportConfirmedSalesOLD", paramArray, 600);
        //}

        public static DataSet INGetConfirmedSalesReportData(DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@FromDate", fromDate);
            object param2 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportConfirmedSales", paramArray, 600);
        }

        #endregion Confirmed Sales Report - Specific Functionalities

        #region Bump-Up Report - Specific Functionalities

        public static DataTable INGetBumpUpReportData(long campaignID, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@CampaignID", campaignID);
            object param2 = Database.GetParameter("@StartDate", fromDate);
            object param3 = Database.GetParameter("@EndDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportBumpUp", paramArray, 1800).Tables[0];
        }

        public static DataTable INGetBumpUpReportSummaryData(string campaignIDs, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@CampaignIDs", campaignIDs);
            object param2 = Database.GetParameter("@StartDate", fromDate);
            object param3 = Database.GetParameter("@EndDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportBumpUpSummary", paramArray).Tables[0];
        }

        #endregion Bump-Up Report - Specific Functionalities

        #region Sales Report - Specific Functionalities

        public static DataTable INGetSalesReportOvertimeData(long? fkUserID, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@FKUserID", fkUserID);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSalesOvertime", paramArray).Tables[0];
        }

        public static DataSet INGetSalesReportRedeemedGiftsData(long? fkUserID, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@FKUserID", fkUserID);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSalesRedeemedGifts", paramArray);
        }

        #endregion Sales Report - Specific Functionalities

        #region Latent Leads Report - Specific Functionalities

        public static DataTable INGetLatentLeadsReportBatchAssignees(long batchID)
        {
            object param1 = Database.GetParameter("@FKINBatchID", batchID);
            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetLatentLeadsReportBatchAssignees", paramArray).Tables[0];
        }

        public static DataTable INGetLatentLeadsReportData(long batchID, long fkUserID)
        {
            object param1 = Database.GetParameter("@FKINBatchID", batchID);
            object param2 = Database.GetParameter("@FKUserID", fkUserID);
            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINLatentLeadsReport", paramArray).Tables[0];
        }

        public static bool DoesBatchContainLatentLeads(long batchID)
        {
            bool result = false;
            object param1 = Database.GetParameter("@FKINBatchID", batchID);
            object[] paramArray = new[] { param1 };

            DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINBatchHasLatentLeads", paramArray).Tables[0];

            result = (bool)dt.Rows[0][0];

            return result;
        }

        public static int IndicateBatchAsContainingLatentLeads(long batchID, bool containsLatentLeads)
        {
            object param1 = Database.GetParameter("@FKINBatchID", batchID);
            object param2 = Database.GetParameter("@ContainsLatentLeads", containsLatentLeads);
            object[] paramArray = new[] { param1, param2 };

            int affectedRecords = Database.ExecuteCommand(null, CommandType.StoredProcedure, "spINIndicateBatchAsContainingLatentLeads", paramArray);

            return affectedRecords;
        }

        #endregion Latent Leads Report - Specific Functionalities

        #region Fall-Off Report - Specific Functionalities

        public static DataSet INGetFallOffReportDatasheetData(string campaignIDs, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@FKINCampaignIDs", campaignIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportFallOffDataSheet", paramArray);
        }

        #endregion Fall-Off Report - Specific Functionalities

        #region Lead De-Allocation - Specific Functionalities

        public static bool INIsAuthorizedToDeAllocateLeads()
        {
            bool result = false;
            object param1 = Database.GetParameter("@LoggedInUserID", Embriant.Framework.Configuration.GlobalSettings.ApplicationUser.ID);
            object[] paramArray = new[] { param1 };

            DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINIsAuthorizedToDeAllocateLeads", paramArray).Tables[0];

            result = (bool)dt.Rows[0][0];

            return result;
        }

        public static bool INCanUserDeAllocateLeads()
        {
            bool result = false;
            object param1 = Database.GetParameter("@LoggedInUserID", Embriant.Framework.Configuration.GlobalSettings.ApplicationUser.ID);
            object[] paramArray = new[] { param1 };

            DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINCanUserDeAllocateLeads", paramArray).Tables[0];

            result = (bool)dt.Rows[0][0];

            return result;
        }

        public static DataSet INGetLeadDeallocationScreenLookups()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetLeadDeAllocationScreenLookups", null);
        }

        public static DataTable INGetBatchAssignees(long fkINBatchID)
        {
            object param1 = Database.GetParameter("@FKINBatchID", fkINBatchID);
            object[] paramArray = new[] { param1 };
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetBatchAssignees", paramArray).Tables[0];
        }

        public static DataTable INGetAllocationDatesByBatchIDAndUserID(long fkINBatchID, long fkUserID)
        {
            object param1 = Database.GetParameter("@FKINBatchID", fkINBatchID);
            object param2 = Database.GetParameter("@FKUserID", fkUserID);

            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetAllocationDatesByBatchIDAndUserID", paramArray).Tables[0];
        }

        public static DataSet INGetDeAllocatableLeadDetails(long fkINBatchID, long fkUserID, DateTime allocationDate)
        {
            object param1 = Database.GetParameter("@FKINBatchID", fkINBatchID);
            object param2 = Database.GetParameter("@FKUserID", fkUserID);
            object param3 = Database.GetParameter("@AllocationDate", allocationDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetDeAllocatableLeadDetails", paramArray);
        }

        public static DataSet INDeAllocateLeads(long fkINBatchID, long fkUserID, DateTime allocationDate, string fkINImportIDs, long fkINLeadDeAllocationReasonID, long fkRequestorUserID, string note, bool commit)
        {
            object param1 = Database.GetParameter("@FKINBatchID", fkINBatchID);
            object param2 = Database.GetParameter("@FKUserID", fkUserID);
            object param3 = Database.GetParameter("@AllocationDate", allocationDate);
            object param4 = Database.GetParameter("@FKINImportIDs", fkINImportIDs);
            object param5 = Database.GetParameter("@FKINLeadDeAllocationReasonID", fkINLeadDeAllocationReasonID);
            object param6 = Database.GetParameter("@FKRequestorUserID", fkRequestorUserID);
            object param7 = Database.GetParameter("@Note", note);
            object param8 = Database.GetParameter("@FKStampUserID", GlobalSettings.ApplicationUser.ID);
            object param9 = Database.GetParameter("@Commit", commit);

            object[] paramArray = new[] { param1, param2, param3, param4, param5, param6, param7, param8, param9 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINDeAllocateLeads", paramArray);
        }

        #endregion Lead De-Allocation - Specific Functionalities

        #region Hours Capturing Screen - Specific Functionalities



        #endregion Hours Capturing Screen - Specific Functionalities

        #region Note - Specific Functionalities

        //public static bool DoesLeadHaveNotes(long fkINImportID)
        //{
        //    return (INGetLeadNotes(fkINImportID).Rows.Count > 0);
        //}

        public static DataTable INGetLeadNotes(long fkINImportID)
        {
            object param1 = Database.GetParameter("@FKINImportID", fkINImportID);
            object param2 = Database.GetParameter("@LoggedInUserID", GlobalSettings.ApplicationUser.ID);

            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetLeadNotesByImportID", paramArray).Tables[0];
        }

        public static DataTable INGetLatestNote(long fkINImportID)
        {
            object param1 = Database.GetParameter("@FKINImportID", fkINImportID);
            object[] paramArray = new[] { param1 };
            DataTable dtLatestNote = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetLatestNoteByImportID", paramArray).Tables[0];

            return dtLatestNote;
        }

        public static DataTable INGetLeadNoteByImportIDAndSequence(long fkINImportID, int sequence)
        {
            object param1 = Database.GetParameter("@FKINImportID", fkINImportID);
            object param2 = Database.GetParameter("@Sequence", sequence);
            object[] paramArray = new[] { param1, param2 };

            DataTable dtNote = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetLeadNoteByImportIDAndSequence", paramArray).Tables[0];

            return dtNote;
        }

        #endregion Note - Specific Functionalities

        #region Batch Analysis Report - Specific Functionalities

        public static DataSet INGetBatchAnalysisReportData(long fkINCampaignID, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@CampaignID", fkINCampaignID);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportIncomingBatchAnalysis", paramArray);
        }

        #endregion Batch Analysis Report - Specific Functionalities

        #region Renewal Lead Copying - Specific Functionalities

        public static DataSet INGetRenewalLeadCopyScreenLookups()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetRenewalLeadCopyScreenLookups", null);
        }

        public static string INGetRenewalReferenceNumber(long fkINOriginalCampaignID, long fkINDestinationCampaignID, string originalReferenceNumber)
        {
            string newReferenceNumber = String.Empty;

            object param1 = Database.GetParameter("@FKINOriginalCampaignID", fkINOriginalCampaignID);
            object param2 = Database.GetParameter("@FKINDestinationCampaignID", fkINDestinationCampaignID);
            object param3 = Database.GetParameter("@OriginalReferenceNumber", originalReferenceNumber);
            object[] paramArray = new[] { param1, param2, param3 };

            DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetRenewalReferenceNumber", paramArray).Tables[0];

            newReferenceNumber = dt.Rows[0][0].ToString();

            return newReferenceNumber;
        }

        #endregion Renewal Lead Copying - Specific Functionalities

        #region Batch Report - Specific Functionalities

        public static DataTable INGetBatchReportTypes()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetBatchReportTypes", null).Tables[0];
        }

        public static DataTable INGetBatchReportCampaignsOrCampaignTypesByReportType(byte reportType)
        {
            object param1 = Database.GetParameter("@INReportType", reportType);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetBatchReportCampaignsOrCampaignTypesByReportType", paramArray).Tables[0];
        }

        //public static DataSet INGetBatchReportData(string fkINCampaignIDs, DateTime fromDate, DateTime toDate, byte reportTypeID, bool includeLeadsCopiedToExtension, bool includeCompletedBatches)
        public static DataSet INGetBatchReportData(string fkINCampaignIDs, int year, int month, byte reportTypeID, bool includeLeadsCopiedToExtension, bool includeCompletedBatches, bool onlyBatchesReceived91DaysAgoAndAfter, bool combineUL)
        {
            object param1 = Database.GetParameter("@FKINCampaignIDs", fkINCampaignIDs);
            object param2 = Database.GetParameter("@Year", year);
            object param3 = Database.GetParameter("@Month", month);
            object param4 = Database.GetParameter("@ReportType", reportTypeID);
            object param5 = Database.GetParameter("@IncludeLeadsCopiedToExtension", includeLeadsCopiedToExtension);
            object param6 = Database.GetParameter("@IncludeCompleted", includeCompletedBatches);
            object param7 = Database.GetParameter("@LoggedInUserID", GlobalSettings.ApplicationUser.ID);
            object param8 = Database.GetParameter("@OnlyBatchesReceived91DaysAgoAndAfter", onlyBatchesReceived91DaysAgoAndAfter);
            object param9 = Database.GetParameter("@CombineUL", combineUL);

            object[] paramArray = new[] { param1, param2, param3, param4, param5, param6, param7, param8, param9};

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportBatch", paramArray, 1200);
        }

        

        //public static DataSet INGetBatchReportGiftSheetData(string fkINCampaignIDs, int year, int month, byte reportTypeID, bool includeLeadsCopiedToExtension, bool includeCompletedBatches)
        //{
        //    object param1 = Database.GetParameter("@FKINCampaignIDs", fkINCampaignIDs);
        //    object param2 = Database.GetParameter("@Year", year);
        //    object param3 = Database.GetParameter("@Month", month);
        //    object param4 = Database.GetParameter("@ReportType", reportTypeID);
        //    object param5 = Database.GetParameter("@IncludeLeadsCopiedToExtension", includeLeadsCopiedToExtension);
        //    object param6 = Database.GetParameter("@IncludeCompleted", includeCompletedBatches);
        //    object param7 = Database.GetParameter("@LoggedInUserID", GlobalSettings.ApplicationUser.ID);

        //    object[] paramArray = new[] { param1, param2, param3, param4, param5, param6, param7 };

        //    return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportBatchGifts", paramArray, 600);
        //}

        #endregion Batch Report - Specific Functionalities

        #region Bump-Up Potential Report - Specific Functionalities

        public static DataSet INGetBumpUpPotentialReportLookupsAndConfigs()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetBumpUpPotentialReportLookupsAndConfigs", null);
        }

        public static DataSet INGetBumpUpPotentialReportData(long fkINCampaignID, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@FKINCampaignID", fkINCampaignID);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportBumpUpPotential", paramArray, 600);
        }

        public static DataSet INGetBumpUpPotentialReportCampaignGroups(string fkINCampaignID)
        {
            object param1 = Database.GetParameter("@FKINCampaignIDs", fkINCampaignID);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportBumpUpCampaignGroups", paramArray, 600);
        }

        #endregion Bump-Up Potential Report - Specific Functionalities

        #region Diary Report - Specific Functionalities

        public static DataSet INGetDiaryReportData(string fkINCampaignIDs, DateTime fromDate, DateTime toDate)
        {
            //SqlParameter[] parameters = new SqlParameter[3];
            //parameters[0] = new SqlParameter("@CampaignID", campaignID);
            //parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
            //parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

            //DataSet dsReducedPremiumReport = Methods.ExecuteStoredProcedure("spINReportDiary", parameters);

            object param1 = Database.GetParameter("@FKINCampaignIDs", fkINCampaignIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportDiary", paramArray, 600);
        }

        #endregion Diary Report - Specific Functionalities

        #region Permission Lead Report - Specific Function
        public static DataSet INGetPermissionLeadReportData(int? fkINCampaignIDs, DateTime fromDate, DateTime toDate)
        {
            //SqlParameter[] parameters = new SqlParameter[3];
            //parameters[0] = new SqlParameter("@CampaignID", campaignID);
            //parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
            //parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

            //DataSet dsReducedPremiumReport = Methods.ExecuteStoredProcedure("spINReportDiary", parameters);

            object param1 = Database.GetParameter("@FKINCampaignIDs", fkINCampaignIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportPermissionLead", paramArray, 600);
        }

        public static DataSet INGetPermissionLeadMaccMillReportData(int? fkINCampaignIDs, DateTime fromDate, DateTime toDate)
        {
            //SqlParameter[] parameters = new SqlParameter[3];
            //parameters[0] = new SqlParameter("@CampaignID", campaignID);
            //parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
            //parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

            //DataSet dsReducedPremiumReport = Methods.ExecuteStoredProcedure("spINReportDiary", parameters);

            object param1 = Database.GetParameter("@FKINCampaignIDs", fkINCampaignIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportPermissionLeadMaccMill", paramArray, 600);
        }

        public static DataSet INGetPermissionLeadMaccReportData(int? fkINCampaignIDs, DateTime fromDate, DateTime toDate)
        {
            //SqlParameter[] parameters = new SqlParameter[3];
            //parameters[0] = new SqlParameter("@CampaignID", campaignID);
            //parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
            //parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

            //DataSet dsReducedPremiumReport = Methods.ExecuteStoredProcedure("spINReportDiary", parameters);

            object param1 = Database.GetParameter("@FKINCampaignIDs", fkINCampaignIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportPermissionLeadMacc", paramArray, 600);
        }
        #endregion

        #region LeadAllocationBatchReport
        public static DataSet INGetLeadAllocationBatch(long CampaignID, DateTime fromDate, DateTime toDate, int combinedbool, int tempperm)
        {
            object param1 = Database.GetParameter("@CampaignId", CampaignID);
            object param2 = Database.GetParameter("@_endDate", fromDate);
            object param3 = Database.GetParameter("@_startDate", toDate);
            object param4 = Database.GetParameter("@Combined", combinedbool);
            object param5 = Database.GetParameter("@StaffType", tempperm);


            object[] paramArray = new[] { param1, param2, param3, param4, param5 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportLeadAllocationBatchSummary", paramArray, 600);
        }

        public static DataSet INGetLeadAllocationBatchSummary(long CampaignID, DateTime fromDate, DateTime toDate, int combinedbool)
        {
            object param1 = Database.GetParameter("@CampaignId", CampaignID);
            object param2 = Database.GetParameter("@_endDate", fromDate);
            object param3 = Database.GetParameter("@_startDate", toDate);
            object param4 = Database.GetParameter("@Combined", combinedbool);


            object[] paramArray = new[] { param1, param2, param3, param4 };


            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportLeadAllocationBatchSummary", paramArray, 600);
        }
        #endregion

        #region Debi-Check Report
        public static DataSet INGetDebiCheckPL(DateTime fromDate, DateTime toDate)
        {
            //SqlParameter[] parameters = new SqlParameter[3];
            //parameters[0] = new SqlParameter("@CampaignID", campaignID);
            //parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
            //parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

            //DataSet dsReducedPremiumReport = Methods.ExecuteStoredProcedure("spINReportDiary", parameters);

            object param1 = Database.GetParameter("@DateFrom", fromDate);
            object param2 = Database.GetParameter("@DateTo", toDate);

            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spReportDebiCheckPL", paramArray, 600);
        }

        public static DataSet INGetDebiCheckTrackingAccepted(DateTime fromDate, DateTime toDate)
        {

            object param1 = Database.GetParameter("@DateFrom", fromDate);
            object param2 = Database.GetParameter("@DateTo", toDate);


            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spReportDebiCheckTrackingAccepted", paramArray, 600);
        }


        public static DataSet INGetDebiCheckLookupPL(string refNo)
        {
            //SqlParameter[] parameters = new SqlParameter[3];
            //parameters[0] = new SqlParameter("@CampaignID", campaignID);
            //parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
            //parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

            //DataSet dsReducedPremiumReport = Methods.ExecuteStoredProcedure("spINReportDiary", parameters);

            object param1 = Database.GetParameter("@ReferenceNumber", refNo);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spGetMandateLookupPL", paramArray, 600);
        }

        public static DataSet INGetDebiCheckPLConsolidated(DateTime fromDate, DateTime toDate, string baseupgrade)
        {
            //SqlParameter[] parameters = new SqlParameter[3];
            //parameters[0] = new SqlParameter("@CampaignID", campaignID);
            //parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
            //parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

            //DataSet dsReducedPremiumReport = Methods.ExecuteStoredProcedure("spINReportDiary", parameters);

            object param1 = Database.GetParameter("@DateFrom", fromDate);
            object param2 = Database.GetParameter("@DateTo", toDate);
            object param3 = Database.GetParameter("@BaseUpgradeBool", baseupgrade);



            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spReportDebiCheckPLCustom", paramArray, 600);
        }

        public static DataSet INGetDebiChecKTrackingTSR(DateTime fromDate, DateTime toDate, long? userID)
        {
            //SqlParameter[] parameters = new SqlParameter[3];
            //parameters[0] = new SqlParameter("@CampaignID", campaignID);
            //parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
            //parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

            //DataSet dsReducedPremiumReport = Methods.ExecuteStoredProcedure("spINReportDiary", parameters);

            object param1 = Database.GetParameter("@DateFrom", toDate);
            object param2 = Database.GetParameter("@DateTo", fromDate);
            object param3 = Database.GetParameter("@FKUserID", userID);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spReportDebiCheckTrackingByTSR", paramArray, 600);
        }
        public static DataSet INGetDebiChecKTrackingTSRUpgrades(DateTime fromDate, DateTime toDate, long? userID)
        {
            //SqlParameter[] parameters = new SqlParameter[3];
            //parameters[0] = new SqlParameter("@CampaignID", campaignID);
            //parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
            //parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

            //DataSet dsReducedPremiumReport = Methods.ExecuteStoredProcedure("spINReportDiary", parameters);

            object param1 = Database.GetParameter("@DateFrom", toDate);
            object param2 = Database.GetParameter("@DateTo", fromDate);
            object param3 = Database.GetParameter("@FKUserID", userID);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spReportDebiCheckTrackingByTSRUpgrades", paramArray, 600);
        }
        public static DataSet INGetDebiChecKTrackingTSRAgents(DateTime fromDate, DateTime toDate)
        {
            //SqlParameter[] parameters = new SqlParameter[3];
            //parameters[0] = new SqlParameter("@CampaignID", campaignID);
            //parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
            //parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

            //DataSet dsReducedPremiumReport = Methods.ExecuteStoredProcedure("spINReportDiary", parameters);

            object param1 = Database.GetParameter("@DateFrom", toDate);
            object param2 = Database.GetParameter("@DateTo", fromDate);

            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spGetDebiCheckTrackingByTSR", paramArray, 600);
        }

        public static DataSet INGetDebiChecKTrackingTSRAgentsUpgrades(DateTime fromDate, DateTime toDate)
        {
            //SqlParameter[] parameters = new SqlParameter[3];
            //parameters[0] = new SqlParameter("@CampaignID", campaignID);
            //parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
            //parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

            //DataSet dsReducedPremiumReport = Methods.ExecuteStoredProcedure("spINReportDiary", parameters);

            object param1 = Database.GetParameter("@DateFrom", toDate);
            object param2 = Database.GetParameter("@DateTo", fromDate);

            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spGetDebiCheckTrackingByTSRUpgrades", paramArray, 600);
        }

        public static DataSet INGetDebiChecKTrackingTSRAgentsTeam(string fkuserid)
        {
            //SqlParameter[] parameters = new SqlParameter[3];
            //parameters[0] = new SqlParameter("@CampaignID", campaignID);
            //parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
            //parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

            //DataSet dsReducedPremiumReport = Methods.ExecuteStoredProcedure("spINReportDiary", parameters);

            object param1 = Database.GetParameter("@FKUserID", fkuserid);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spGetDebiCheckTrackingByTSRTeam", paramArray, 600);
        }

        public static DataSet INReportDebiChecKTrackingCampaignsUpgrades(DateTime fromDate, DateTime toDate, string campaignID)
        {
            //SqlParameter[] parameters = new SqlParameter[3];
            //parameters[0] = new SqlParameter("@CampaignID", campaignID);
            //parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
            //parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

            //DataSet dsReducedPremiumReport = Methods.ExecuteStoredProcedure("spINReportDiary", parameters);

            object param1 = Database.GetParameter("@DateFrom", toDate);
            object param2 = Database.GetParameter("@DateTo", fromDate);
            object param3 = Database.GetParameter("@FKUserID", campaignID);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spReportDebiCheckTrackingUpgrades", paramArray, 200000);
        }
        #endregion

        #region DebiCheck Configuration
        public static DataSet INGetPLServerConnectionTest(string refno)
        {
            object param1 = Database.GetParameter("@RefNo", refno);



            object[] paramArray = new[] { param1};

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "PLServerConnectionTest", paramArray, 600);
        }
        #endregion

        #region DebiCheck Bulk Send
        public static DataSet INGetDebiCheckBulkNoResponses(DateTime fromDate, DateTime toDate)
        {
            //SqlParameter[] parameters = new SqlParameter[3];
            //parameters[0] = new SqlParameter("@CampaignID", campaignID);
            //parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
            //parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

            //DataSet dsReducedPremiumReport = Methods.ExecuteStoredProcedure("spINReportDiary", parameters);

            object param1 = Database.GetParameter("@DateFrom", fromDate);
            object param2 = Database.GetParameter("@DateTo", toDate);


            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spGetBulkNoResponses", paramArray, 600);
        }
        #endregion

        #region DebiCheck Tracking Report
        public static DataSet INGetDebiCheckTracking(DateTime fromDate, DateTime toDate)
        {
            //SqlParameter[] parameters = new SqlParameter[3];
            //parameters[0] = new SqlParameter("@CampaignID", campaignID);
            //parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
            //parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

            //DataSet dsReducedPremiumReport = Methods.ExecuteStoredProcedure("spINReportDiary", parameters);

            object param1 = Database.GetParameter("@DateFrom", fromDate);
            object param2 = Database.GetParameter("@DateTo", toDate);


            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spReportDebiCheckTracking", paramArray, 600);
        }
        #endregion

        #region GetMandateInfo
        public static DataSet INGetMandateInfo(string RefNo)
        {
            //SqlParameter[] parameters = new SqlParameter[3];
            //parameters[0] = new SqlParameter("@CampaignID", campaignID);
            //parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
            //parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

            //DataSet dsReducedPremiumReport = Methods.ExecuteStoredProcedure("spINReportDiary", parameters);

            object param1 = Database.GetParameter("@RefNo", @RefNo);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spGetMandateInfo", paramArray, 600);
        }
        #endregion

        #region Status Loading Screen - Specific Functionalities

        public static DataSet INGetStatusLoadingScreenLookups()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetStatusLoadingScreenLookups", null);
        }

        #endregion Status Loading Screen - Specific Functionalities

        #region Status Loading Summary Report - Specific Functionalities

        public static DataSet INGetStatusLoadingSummaryReportData(string fkINCampaignIDs, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@FKINCampaignIDs", fkINCampaignIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportStatusLoadingSummary", paramArray, 600);
        }

        #endregion Status Loading Summary Report - Specific Functionalities

        #region Daily Sales Report - Specific Functionalities

        public static DataSet INGetDailySalesReportLookups()
        {
            object param1 = Database.GetParameter("@LoggedInUserID", GlobalSettings.ApplicationUser.ID);

            object[] paramArray = new[] { param1};

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetDailySalesReportLookups", paramArray, 600);
        }

        #endregion Daily Sales Report - Specific Functionalities

        #region Capture TSR Targets Screen Functionalities

        public static DataSet INGetCaptureTSRTargetsScreenLookups()
        {
            object param1 = Database.GetParameter("@UserID", GlobalSettings.ApplicationUser.ID);
            object[] paramArray = new[] { param1 };
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spCaptureTSRTargetsLookupData", paramArray, 600);
        }

        #endregion Capture TSR Targets Screen Functionalities

        #region Elite Confirmation Progress Report - Specific Functionalities

        public static DataSet INGetEliteConfirmationProgressReport(/*string fkINCampaignIDs,*/ DateTime fromDate, DateTime toDate)
        {
            //object param1 = Database.GetParameter("@FKINCampaignIDs", fkINCampaignIDs);
            object param1 = Database.GetParameter("@FromDate", fromDate);
            object param2 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportEliteConfirmationProgress", paramArray, 600);
        }

        #endregion Elite Confirmation Progress Report - Specific Functionalities

        #region Redeemed Gifts Export - Specific Functionalities

        public static DataTable INGetRedeemedGiftsExportLookups()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetRedeemedGiftsExportLookups", null).Tables[0];
        }

        public static DataSet INGetRedeemedGiftsExportData(string fkINCampaignIDs, DateTime date)
        {
            object param1 = Database.GetParameter("@FKINCampaignIDs", fkINCampaignIDs);
            object param2 = Database.GetParameter("@Date", date);

            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportRedeemedGiftsExport", paramArray, 600);
        }

        #endregion Redeemed Gifts Export - Specific Functionalities

        #region Agent Activity Report - Specific Functionalities

        public static DataSet INReportAgentActivity(string fkUserIDs, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@FKUserIDs", fkUserIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportAgentActivity", paramArray);
        }

        public static DataSet INReportCallMonitoringActivity(string fkUserIDs, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@FKUserIDs", fkUserIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportCallMonitoringActivity", paramArray);
        }

        #endregion Agent Activity Report - Specific Functionalities

        #region Delete Single Lead - Specific Functionalities

        public static DataTable INGetDeleteSingleLeadScreenLookups()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetDeleteSingleLeadScreenLookups", null).Tables[0];
        }

        public static DataSet spDeleteSingleLead(string importIDs, long stampUserID)
        {
            object param1 = Database.GetParameter("@ImportIDs", importIDs);
            object param2 = Database.GetParameter("@StampUserID", stampUserID);

            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spDeleteSingleLead", paramArray);
        }
        #endregion

        #region Decline Report - Specific Functionalitites
        //The INGetDeclineScreenLookups and INReportDeclines methods are derived from the INGetPODDiariesScreenLookups and INReportPODDiaries methods respectively
        public static DataSet INGetDeclineScreenLookups()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetDeclineScreenLookups", null);
        }

        public static DataSet INReportDeclines(long fkINCampaignID, string fkINBatchIDs, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@FKINCampaignID", fkINCampaignID);
            object param2 = Database.GetParameter("@FKINBatchIDs", fkINBatchIDs);
            object param3 = Database.GetParameter("@FromDate", fromDate);
            object param4 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3, param4};

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportDecline", paramArray);
        }
        #endregion

        #region Diary To Final Status Report - Specific Functionalities
        public static DataSet INGetDiaryToFinalScreenLookups()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetDiaryToFinalScreenLookups", null);
        }

        public static DataSet INReportDiaryToFinal(long fkINCampaignID, string fkINBatchIDs, DateTime toDate)
        {
            object param1 = Database.GetParameter("@FKINCampaignID", fkINCampaignID);
            object param2 = Database.GetParameter("@FKINBatchIDs", fkINBatchIDs);
            object param3 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportDiaryToFinalStatus", paramArray);
        }
        #endregion

        #region POD Diaries Report - Specific Functionalities

        public static DataSet INGetPODDiariesScreenLookups()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetPODDiariesScreenLookups", null);
        }

        public static DataSet INReportPODDiaries(long fkINCampaignID, string fkINBatchIDs /*, DateTime fromDate, DateTime toDate*/)
        {
            object param1 = Database.GetParameter("@FKINCampaignID", fkINCampaignID);
            object param2 = Database.GetParameter("@FKINBatchIDs", fkINBatchIDs);
            //object param3 = Database.GetParameter("@FromDate", fromDate);
            //object param4 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2/*, param3, param4*/ };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportPODDiaries", paramArray);
        }

        #endregion POD Diaries Report - Specific Functionalities

        #region Confirmation Statistics Report - Specific Functionalities

        public static DataTable INGetConfirmationStatisticsReportLookups()
        {
            object param1 = Database.GetParameter("@LoggedInUserID", GlobalSettings.ApplicationUser.ID);
            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetConfirmationStatisticsReportLookups", paramArray).Tables[0];
        }

        public static DataSet INReportConfirmationStats(string fkUserIDs, DateTime fromDate, DateTime toDate, byte reportScope)
        {
            object param1 = Database.GetParameter("@AgentIDs", fkUserIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);
            object param4 = Database.GetParameter("@ReportScope",  reportScope);

            object[] paramArray = new[] { param1, param2, param3, param4 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportConfirmationStats", paramArray, 600);
        }

        #endregion Confirmation Statistics Report - Specific Functionalities

        #region Reduced Premium Report - Specific Functionalities

        public static DataSet INGetPremiumBreakdownReportScreenLookups()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetPremiumBreakdownReportScreenLookups", null);
        }

        public static DataSet INReportPremiumBreakdown(string fkINBatchIDs, DateTime fromDate, DateTime toDate, byte staffType)
        {
            object param1 = Database.GetParameter("@BatchIDList", fkINBatchIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);
            object param4 = Database.GetParameter("@StaffType", staffType);

            object[] paramArray = new[] { param1, param2, param3, param4 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportPremiumBreakdown", paramArray);
        }

        #endregion Reduced Premium Report - Specific Functionalities 

        #region Redeemed Gifts Screen - Specific Functionalities

        public static DataSet INGetRedeemGiftScreenLookups(long fkINImport)
        {
            object param1 = Database.GetParameter("@FKINImportID", fkINImport);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetRedeemGiftScreenLookups", paramArray);
        }

        #endregion Redeemed Gifts Screen - Specific Functionalities

        #region Confirmed Sales Per Agent Report - Specific Functionalities

        public static DataTable INGetConfirmationAgents()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetConfirmationAgents", null).Tables[0];
        }

        public static DataSet INReportConfirmedSalesPerAgent(string userIDs, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@AgentIDs", userIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportConfirmedSalesPerAgent", paramArray, 600);
        }

        #endregion

        #region Hours Report - Specific Functionalities

        public static DataSet INReportHours(byte hoursReportType, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@ReportType", hoursReportType);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportHours", paramArray, 600);
        }

        #endregion Hours Report - Specific Functionalities

        #region Call Monitoring Bump-Ups - Specific Functionalities

        public static DataSet INReportCallMonitoringBumpUps(DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@FromDate", fromDate);
            object param2 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportCallMonitoringBumpUps", paramArray, 600);
        }

        #endregion Call Monitoring Bump-Ups - Specific Functionalities

        #region Call Monitoring Report - Specific Functionalities

        public static DataSet INGetCallMonitoringExtraDetails(long? fkinimportid)
        {
            object param1 = Database.GetParameter("@FKINImport", fkinimportid);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spGetCMExtraDetails", paramArray, 600);
        }

        public static DataSet INReportCallMonitoring(DateTime fromDate, DateTime toDate, byte staffType)
        {
            object param1 = Database.GetParameter("@FromDate", fromDate);
            object param2 = Database.GetParameter("@ToDate", toDate);
            object param3 = Database.GetParameter("@StaffType", staffType);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportCallMonitoring", paramArray, 600);
        }

        public static DataTable INGetReportCallMonitoringQueryScreenLookups()
        {

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetCallMonitoringQueryScreenLookups", null, 600).Tables[0];
        }

        #endregion Call Monitoring Report - Specific Functionalities

        #region Call Monitoring Screen Functionalities

        public static DataSet INGetCallMonitoringScreenLookups()
        {
            //object param1 = Database.GetParameter("@FKINImport", fkINImport);
            object param1 = Database.GetParameter("@LoggedInUserID", GlobalSettings.ApplicationUser.ID);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetCallMonitoringScreenLookups", paramArray, 600);
        }

        public static DataTable INGetCallMonitoringScreenDefaults()
        {
            //object param1 = Database.GetParameter("@FKINImport", fkINImport);
            object param1 = Database.GetParameter("@LoggedInUserID", GlobalSettings.ApplicationUser.ID);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetCallMonitoringScreenDefaults", paramArray, 600).Tables[0];
        }

        public static DataTable INGetCallMonitoringStandardNotes()
        {

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "INGetCallMonitoringStandardNotes", null, 600).Tables[0];
        }

        public static DataTable GetCMSelectedNotes(long? ImportID)
        {
            object param1 = Database.GetParameter("@ImportID", ImportID);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "GetCMSelectedNotes", paramArray, 600).Tables[0];
        }

        #endregion Call Monitoring Screen Functionalities

        #region Invalid Accounts Report - Specific Functionalities

        public static DataSet INGetInvalidAccountsReportData(string fkINCampaignIDs, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@FKINCampaignIDs", fkINCampaignIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportInvalidAccounts", paramArray, 600);
        }
        #endregion Invalid Accounts Report - Specific Functionalities

        #region Top Ten Report - Specific Functionalities
        public static DataSet INReportTopTen(DateTime reportDate)
        {
            object param1 = Database.GetParameter("@ReportDate", reportDate);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportTopTen", paramArray, 600);
        }
        #endregion Top Ten Report - Specific Functionalities

        #region Carried Forward Report - Specific Functionalities

        public static DataSet INReportCarriedForward(string fkUserIDs, DateTime fromDate, DateTime toDate, byte reportScope)
        {
            //object param1 = Database.GetParameter("@FromDate", fromDate);
            //object param2 = Database.GetParameter("@ToDate", toDate);

            //object[] paramArray = new[] { param1, param2 };

            //return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportCarriedForward", paramArray, 600);

            object param1 = Database.GetParameter("@AgentIDs", fkUserIDs);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);
            object param4 = Database.GetParameter("@ReportScope", reportScope);

            object[] paramArray = new[] { param1, param2, param3, param4 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportCarriedForward", paramArray, 600);
        }

        #endregion Carried Forward Report - Specific Functionalities

        #region Call Monitoring Tracking Report - Specific Functionalities

        public static DataTable INGetCallMonitoringTrackingReportLookups()
        {
            //object param1 = Database.GetParameter("@FromDate", fromDate);
            //object param2 = Database.GetParameter("@ToDate", toDate);

            //object[] paramArray = new[] { param1, param2 };

            //return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportCarriedForward", paramArray, 600);

            object param1 = Database.GetParameter("@LoggedInUserID", GlobalSettings.ApplicationUser.ID);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetCallMonitoringTrackingReportLookups", paramArray, 600).Tables[0];
        }

        #endregion

        #region Bump-Up Stats Report - Specific Functionalitiess

        public static DataSet INReportBumpUpStatistics(DateTime fromDate, DateTime toDate, long mode, string yearSelected)
        {
            object param1 = Database.GetParameter("@FromDate", fromDate);
            object param2 = Database.GetParameter("@ToDate", toDate);
            object param3 = Database.GetParameter("@Mode", mode);
            object param4 = Database.GetParameter("@SelectedYear", yearSelected);

            object[] paramArray = new[] { param1, param2, param3, param4 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportBumpUpStatistics", paramArray, 600);
        }

        #endregion Bump-Up Stats Report - Specific Functionalities 

        #region Turnover Report - Specific Functionalities

        public static DataSet INReportTurnover(string campaignIDs, string fkUserIDs, DateTime fromDate, DateTime toDate, bool includeBumpUps, bool includeElevationTeam, lkpINTurnoverCompanyMode? company, byte staffType/*, byte campaignType*/, bool allSalesAgents, string QAIDs)
        {


            object param1 = Database.GetParameter("@CampaignIDList", campaignIDs);
            object param2 = Database.GetParameter("@SalesAgentIDList", fkUserIDs);
            object param3 = Database.GetParameter("@FromDate", fromDate);
            object param4 = Database.GetParameter("@ToDate", toDate);
            object param5 = Database.GetParameter("@IncludeBumpUps", includeBumpUps);
            object param6 = Database.GetParameter("@IncludeElevation", includeElevationTeam);
            object param7 = Database.GetParameter("@Company", (byte)company);
            object param8 = Database.GetParameter("@StaffType", staffType);
            //object param6 = Database.GetParameter("@CampaignType", campaignType);
            object param9 = Database.GetParameter("@AllSalesAgents", allSalesAgents);
            object param10 = Database.GetParameter("@QAIDList", QAIDs);

            object[] paramArray = new[] { param1, param2, param3, param4, param5, param6, param7, param8, param9, param10 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportTurnover", paramArray, 600);
        }
        public static DataSet INReportTurnoverElevation(string campaignIDs, string fkUserIDs, DateTime fromDate, DateTime toDate, bool includeBumpUps, bool includeElevationTeam, lkpINTurnoverCompanyMode? company, byte staffType/*, byte campaignType*/, bool allSalesAgents, string QAIDs)
        {


            object param1 = Database.GetParameter("@CampaignIDList", campaignIDs);
            object param2 = Database.GetParameter("@SalesAgentIDList", fkUserIDs);
            object param3 = Database.GetParameter("@FromDate", fromDate);
            object param4 = Database.GetParameter("@ToDate", toDate);
            object param5 = Database.GetParameter("@IncludeBumpUps", includeBumpUps);
            object param6 = Database.GetParameter("@IncludeElevation", includeElevationTeam);
            object param7 = Database.GetParameter("@Company", (byte)company);
            object param8 = Database.GetParameter("@StaffType", staffType);
            //object param6 = Database.GetParameter("@CampaignType", campaignType);
            object param9 = Database.GetParameter("@AllSalesAgents", allSalesAgents);
            object param10 = Database.GetParameter("@QAIDList", QAIDs);

            object[] paramArray = new[] { param1, param2, param3, param4, param5, param6, param7, param8, param9, param10 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportTurnoverElevation", paramArray, 600);
        }


        public static DataSet INGetTurnoverScreenLookups()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetTurnoverScreenLookups", null, 600);
        }

        public static DataSet INGetTurnoverScreenLookupsSalesCoaches()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetTurnoverScreenLookupsSalesCoach", null, 600);
        }

        public static DataSet INGetTurnoverAgents(lkpINTurnoverCompanyMode? company, byte staffType, bool includeAdmin/*, byte campaignType*/)
        {


            object param1 = Database.GetParameter("@StaffType", staffType);
            object param2 = Database.GetParameter("@CompanyType", (byte)company);
            object param3 = Database.GetParameter("@IncludeAdmin", includeAdmin);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spGetSalesAgents3", paramArray, 600);
        }

        public static DataSet INReportTurnover(string campaignIDs, string fkUserIDs, DateTime fromDate, DateTime toDate, bool includeBumpUps, lkpINTurnoverCompanyMode? company, byte staffType/*, byte campaignType*/, bool allSalesAgents, string QAIDs)
        {

            
            object param1 = Database.GetParameter("@CampaignIDList", campaignIDs);
            object param2 = Database.GetParameter("@SalesAgentIDList", fkUserIDs);
            object param3 = Database.GetParameter("@FromDate", fromDate);
            object param4 = Database.GetParameter("@ToDate", toDate);
            object param5 = Database.GetParameter("@IncludeBumpUps", includeBumpUps);
            object param6 = Database.GetParameter("@Company", (byte)company);
            object param7 = Database.GetParameter("@StaffType", staffType);
            //object param6 = Database.GetParameter("@CampaignType", campaignType);
            object param8 = Database.GetParameter("@AllSalesAgents", allSalesAgents);
            object param9 = Database.GetParameter("@QAIDList", QAIDs);

            object[] paramArray = new[] { param1, param2, param3, param4, param5, param6, param7, param8, param9 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportTurnover", paramArray, 600);
        }
        #endregion Turnover Report - Specific Functionalities

        #region Call Monitoring Sort - Specific Functionalities
        public static DataSet INGetCallMonitoringSortSummaryData()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetCallMonitoringSortSummaryData", null);
        }

        public static DataSet INGetDateOfSaleAssignedSalesData(DateTime dateOfSale, long campaignGroupType, long activity, long workStatusEmployed)
        {
            object param1 = Database.GetParameter("@DateOfSale", dateOfSale);
            object param2 = Database.GetParameter("@CampaignGroupType", campaignGroupType);
            object param3 = Database.GetParameter("@Activity", activity);
            object param4 = Database.GetParameter("@WorkStatusEmployed", workStatusEmployed);

            object[] paramArray = new[] { param1, param2, param3, param4 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetDateOfSaleAssignedSalesData", paramArray);
        }

        public static DataTable INGetUnassignedCallMonitoringAllocationsByDateOfSale(DateTime dateOfSale, long campaignGroupType)
        {
            object param1 = Database.GetParameter("@DateOfSale", dateOfSale);
            object param2 = Database.GetParameter("@CampaignGroupType", campaignGroupType);

            object[] paramArray = new[] { param1, param2 };


            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetUnassignedCallMonitoringAllocationsByDateOfSale", paramArray).Tables[0];
           
        }

        public static DataSet INGetSalesAssignedToCMAgent(long agentID)
        {
            object param1 = Database.GetParameter("@UserID", agentID);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetSalesAssignedToCMAgent", paramArray);
        }

        public static DataSet INGetSalesAssignedToDCAgent(long agentID)
        {
            object param1 = Database.GetParameter("@UserID", agentID);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetSalesAssignedToDCAgent", paramArray);
        }

        public static DataTable GetCMAllocationsNotWorkedOn(DateTime dateOfSale, long campaignGroupType, long cmUserID)
        {
            object param1 = Database.GetParameter("@DateOfSale", dateOfSale);
            object param2 = Database.GetParameter("@CampaignGroupType", campaignGroupType);
            object param3 = Database.GetParameter("@CMUserID", cmUserID);

            object[] paramArray = new[] { param1, param2, param3 };


            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spGetCMAllocationsNotWorkedOn", paramArray).Tables[0];

        }
        #endregion Call Monitoring Sort - Specific Functionalities

        #region Permission Question Report  - Specific Functionalities

        public static DataSet INReportPermissionQuestion(DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@FromDate", fromDate);
            object param2 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportPermissionQuestion", paramArray, 600);
        }

        #endregion Permission Question Report  - Specific Functionalities

        #region Bump-Up Sort - Specific Functionalities
        public static DataSet INGetBumpUpSortSummaryData()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetBumpUpSortSummaryData", null, 1800);
        }

        public static DataSet INGetLeadsAvailable()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spReportBumpUpLeadAvailable", null, 1800);
        }

        public static DataSet INGetDateOfSaleAssignedPossibleBumpUpsData(DateTime dateOfSale, string campaignIDs, long activity, long workStatusEmployed)
        {
            object param1 = Database.GetParameter("@DateOfSale", dateOfSale);
            object param2 = Database.GetParameter("@CampaignIDs", campaignIDs);
            object param3 = Database.GetParameter("@Activity", activity);
            object param4 = Database.GetParameter("@WorkStatusEmployed", workStatusEmployed);

            object[] paramArray = new[] { param1, param2, param3, param4 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetDateOfSaleAssignedPossibleBumpUpsData", paramArray);
        }

        public static DataTable INGetUnassignedPossibleBumpUpAllocationsByDateOfSale(DateTime dateOfSale, string campaignIDs)
        {
            object param1 = Database.GetParameter("@DateOfSale", dateOfSale);
            object param2 = Database.GetParameter("@CampaignIDs", campaignIDs);

            object[] paramArray = new[] { param1, param2 };


            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetUnassignedPossibleBumpUpAllocationsByDateOfSale", paramArray).Tables[0];

        }

        //Stored Proc to load Possible BumpUp Allocations into bump up agent's inbox
        public static DataSet INGetPossibleBumpUpsAssignedToBUAgent(long agentID)
        {
            object param1 = Database.GetParameter("@UserID", agentID);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetPossibleBumpUpsAssignedToBUAgent", paramArray);
        }
        public static DataSet INGetBatchExportDebiCheckRefNo(DateTime fromDate, long campaignID)
        {
            object param1 = Database.GetParameter("@DateOfSale", fromDate);
            object param2 = Database.GetParameter("@CampaignID", campaignID);
            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetDebiCheckRefNo", paramArray);

        }


        public static DataTable GetBUAllocationsNotWorkedOn(DateTime dateOfSale, string campaignIDs, long bUUserID)
        {
            object param1 = Database.GetParameter("@DateOfSale", dateOfSale);
            object param2 = Database.GetParameter("@CampaignIDs", campaignIDs);
            object param3 = Database.GetParameter("@BUUserID", bUUserID);

            object[] paramArray = new[] { param1, param2, param3 };


            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spGetBUAllocationsNotWorkedOn", paramArray).Tables[0];

        }

        #endregion Bump-Up Sort - Specific Functionalities

        #region Sales Tracking Report - Specific Functionalities

        public static DataTable INGetSalesTrackingReportLookups()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetReportSalesTrackingScreenLookups", null, 600).Tables[0];
        }

        public static DataSet INGetSalesTrackingReportData(long fkINCampaignID, DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@CampaignID", fkINCampaignID);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2, param3 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSalesTracking", paramArray, 600);
        }

        #endregion Sales Tracking Report - Specific Functionalities

        #region Base Sales and Contact Tracking Report - Specific Functionalities

        public static DataTable INGetBaseSalesContactTrackingReportLookups()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetReportBaseSalesContactTrackingScreenLookups", null, 600).Tables[0];
        }

        //overloaded method for if they ever want to use this report on the batch report again.
        public static DataSet INGetConversionReportData(string fkINCampaignIDs, bool IsSalesConversion, bool IsContactsConversion)
        {
            object param1 = Database.GetParameter("@CampaignIDs", fkINCampaignIDs);
            object param2 = Database.GetParameter("@IsSalesConversion", IsSalesConversion);
            object param3 = Database.GetParameter("@IsContactsConversion", IsContactsConversion);
            object param4 = Database.GetParameter("@FromDate", DBNull.Value);
            object param5 = Database.GetParameter("@ToDate", DBNull.Value);



            object[] paramArray = new[] { param1, param2, param3, param4, param5};

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSalesContactsConversionPerBatch", paramArray, 1200);
        }

        public static DataSet INGetConversionReportData(string fkINCampaignIDs, bool IsSalesConversion, bool IsContactsConversion, DateTime fromDate, DateTime toDate, int weeks, bool isCampaignTypeIDs)
        {
            object param1 = Database.GetParameter("@CampaignIDs", fkINCampaignIDs);
            object param2 = Database.GetParameter("@IsSalesConversion", IsSalesConversion);
            object param3 = Database.GetParameter("@IsContactsConversion", IsContactsConversion);
            object param4 = Database.GetParameter("@FromDate", fromDate);
            object param5 = Database.GetParameter("@ToDate", toDate);
            object param6 = Database.GetParameter("@WeeksNumber", weeks);
            object param7 = Database.GetParameter("@IsCampaignTypeIDs", isCampaignTypeIDs);



            object[] paramArray = new[] { param1, param2, param3, param4, param5, param6, param7 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportSalesContactsConversionPerBatch", paramArray, 1800);
        }

        public static DataTable INGetBaseSalesContactReportCampaignsOrCampaignTypesByReportType(byte reportType)
        {
            object param1 = Database.GetParameter("@INReportType", reportType);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetBaseSalesContactsReportCampaignsOrCampaignTypesByReportType", paramArray).Tables[0];
        }

        #endregion Sales Tracking Report - Specific Functionalities

        public static DataSet INGetCallMonitoringStatsReport(DateTime fromDate, DateTime toDate)
        {

            object param1 = Database.GetParameter("@FromDate", fromDate);
            object param2 = Database.GetParameter("@ToDate", toDate);
            object[] paramArray = new[] { param1, param2 };
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportHoursConfirmation", paramArray, 600);
        }

            #region Leads Available Report - Specific Functionalities

            public static DataSet INGetReportLeadsAvailableData (string campaignTypeIDs, string campaignGroupIDs)
        {
            object param1 = Database.GetParameter("@CampaignTypes", campaignTypeIDs);
            object param2 = Database.GetParameter("@CampaignGroups", campaignGroupIDs);

            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportLeadsAvailable", paramArray, 600);
        }

        #endregion Leads Available Report - Specific Functionalities

        #region Upgrade Lead Premium Report - Specific Functionalities
        public static DataSet ReportUpgradeLeadPremium(DateTime fromDate, DateTime toDate)
        {
            object param1 = Database.GetParameter("@FromDate", fromDate);
            object param2 = Database.GetParameter("@ToDate", toDate);

            object[] paramArray = new[] { param1, param2 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spReportUpgradeLeadPremium", paramArray, 600);
        }
        #endregion Upgrade Lead Premium Report - Specific Functionalities

        #region Call Transfer Report
        public static DataSet INGetReportCallTransfer( int? fkINCampaignIDs, DateTime fromDate, DateTime toDate, string isupgrade)
        {
            object param1 = Database.GetParameter("@FKUserID", fkINCampaignIDs);
            object param2 = Database.GetParameter("@DateFrom", fromDate);
            object param3 = Database.GetParameter("@DateTo", toDate);
            object param4 = Database.GetParameter("@IsUpgrade", isupgrade);


            object[] paramArray = new[] { param1, param2, param3, param4 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spReportDCCallTransferStats", paramArray, 600);
        }

        public static DataSet INGetReportCallTransferAgents( DateTime fromDate, DateTime toDate, string isupgrade)
        {
            object param2 = Database.GetParameter("@DateFrom", fromDate);
            object param3 = Database.GetParameter("@DateTo", toDate);
            object param4 = Database.GetParameter("@IsUpgrade", isupgrade);

            object[] paramArray = new[] { param2, param3, param4 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetTransferAgentsIDs", paramArray, 600);
        }
        #endregion

        #region DC Transfer Sales
        public static DataSet INGetReportDCTransferSales(string selectedagents, DateTime fromDate, DateTime toDate, string type)
        {
            object param1 = Database.GetParameter("@FKUserID", selectedagents);
            object param2 = Database.GetParameter("@FromDate", fromDate);
            object param3 = Database.GetParameter("@ToDate", toDate);
            object param4 = Database.GetParameter("@Type", type);

            object[] paramArray = new[] { param1, param2, param3, param4 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINReportDebiCheckSpecialist", paramArray, 600);
        }
        #endregion

    }
}
