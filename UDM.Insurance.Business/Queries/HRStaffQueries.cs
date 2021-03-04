using System;
using System.Text;

using UDM.HR.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.HR.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to hrstaff objects.
    /// </summary>
    internal abstract partial class HRStaffQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) hrstaff from the database.
        /// </summary>
        /// <param name="hrstaff">The hrstaff object to delete.</param>
        /// <returns>A query that can be used to delete the hrstaff from the database.</returns>
        internal static string Delete(HRStaff hrstaff, ref object[] parameters)
        {
            string query = string.Empty;
            if (hrstaff != null)
            {
                query = "INSERT INTO [zHstHRStaff] ([ID], [EmployeeNo], [TempStartDate], [EmploymentStartDate], [EmploymentEndDate], [FKCompanyID], [FKStaffTypeID], [FKWorkStatusID], [FKJobTitleID], [FKUserID], [FKRecruitmentConsultantID], [RecruitmentSource], [FKTitleID], [Initials], [FirstName], [PreferredName], [Surname], [FKGenderID], [FKRaceID], [IDNo], [DOB], [FKCitizenID], [FKReligionID], [TelHome], [TelCell], [TelOther], [EMail], [RegisteredTaxPayer], [TaxRefNo], [IRP5Number], [FKMedicalAidID], [CriminalOffence], [EmploymentDismissed], [DisciplinaryGuilty], [WorkedFinServicesIndustry], [WorkedFinServicesIndustryFrom], [WorkedFinServicesIndustryTo], [RegLevel1ExamDate], [Note], [FKAutomationStatusID], [AutomationDate], [StampDate], [StampUserID]) SELECT [ID], [EmployeeNo], [TempStartDate], [EmploymentStartDate], [EmploymentEndDate], [FKCompanyID], [FKStaffTypeID], [FKWorkStatusID], [FKJobTitleID], [FKUserID], [FKRecruitmentConsultantID], [RecruitmentSource], [FKTitleID], [Initials], [FirstName], [PreferredName], [Surname], [FKGenderID], [FKRaceID], [IDNo], [DOB], [FKCitizenID], [FKReligionID], [TelHome], [TelCell], [TelOther], [EMail], [RegisteredTaxPayer], [TaxRefNo], [IRP5Number], [FKMedicalAidID], [CriminalOffence], [EmploymentDismissed], [DisciplinaryGuilty], [WorkedFinServicesIndustry], [WorkedFinServicesIndustryFrom], [WorkedFinServicesIndustryTo], [RegLevel1ExamDate], [Note], [FKAutomationStatusID], [AutomationDate], [StampDate], [StampUserID] FROM [HRStaff] WHERE [HRStaff].[ID] = @ID; ";
                query += "DELETE FROM [HRStaff] WHERE [HRStaff].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", hrstaff.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) hrstaff from the database.
        /// </summary>
        /// <param name="hrstaff">The hrstaff object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the hrstaff from the database.</returns>
        internal static string DeleteHistory(HRStaff hrstaff, ref object[] parameters)
        {
            string query = string.Empty;
            if (hrstaff != null)
            {
                query = "DELETE FROM [zHstHRStaff] WHERE [zHstHRStaff].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", hrstaff.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) hrstaff from the database.
        /// </summary>
        /// <param name="hrstaff">The hrstaff object to undelete.</param>
        /// <returns>A query that can be used to undelete the hrstaff from the database.</returns>
        internal static string UnDelete(HRStaff hrstaff, ref object[] parameters)
        {
            string query = string.Empty;
            if (hrstaff != null)
            {
                query = "INSERT INTO [HRStaff] ([ID], [EmployeeNo], [TempStartDate], [EmploymentStartDate], [EmploymentEndDate], [FKCompanyID], [FKStaffTypeID], [FKWorkStatusID], [FKJobTitleID], [FKUserID], [FKRecruitmentConsultantID], [RecruitmentSource], [FKTitleID], [Initials], [FirstName], [PreferredName], [Surname], [FKGenderID], [FKRaceID], [IDNo], [DOB], [FKCitizenID], [FKReligionID], [TelHome], [TelCell], [TelOther], [EMail], [RegisteredTaxPayer], [TaxRefNo], [IRP5Number], [FKMedicalAidID], [CriminalOffence], [EmploymentDismissed], [DisciplinaryGuilty], [WorkedFinServicesIndustry], [WorkedFinServicesIndustryFrom], [WorkedFinServicesIndustryTo], [RegLevel1ExamDate], [Note], [FKAutomationStatusID], [AutomationDate], [StampDate], [StampUserID]) SELECT [ID], [EmployeeNo], [TempStartDate], [EmploymentStartDate], [EmploymentEndDate], [FKCompanyID], [FKStaffTypeID], [FKWorkStatusID], [FKJobTitleID], [FKUserID], [FKRecruitmentConsultantID], [RecruitmentSource], [FKTitleID], [Initials], [FirstName], [PreferredName], [Surname], [FKGenderID], [FKRaceID], [IDNo], [DOB], [FKCitizenID], [FKReligionID], [TelHome], [TelCell], [TelOther], [EMail], [RegisteredTaxPayer], [TaxRefNo], [IRP5Number], [FKMedicalAidID], [CriminalOffence], [EmploymentDismissed], [DisciplinaryGuilty], [WorkedFinServicesIndustry], [WorkedFinServicesIndustryFrom], [WorkedFinServicesIndustryTo], [RegLevel1ExamDate], [Note], [FKAutomationStatusID], [AutomationDate], [StampDate], [StampUserID] FROM [zHstHRStaff] WHERE [zHstHRStaff].[ID] = @ID AND [zHstHRStaff].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstHRStaff] WHERE [zHstHRStaff].[ID] = @ID) AND (SELECT COUNT(ID) FROM [HRStaff] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstHRStaff] WHERE [zHstHRStaff].[ID] = @ID AND [zHstHRStaff].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstHRStaff] WHERE [zHstHRStaff].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [HRStaff] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", hrstaff.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an hrstaff object.
        /// </summary>
        /// <param name="hrstaff">The hrstaff object to fill.</param>
        /// <returns>A query that can be used to fill the hrstaff object.</returns>
        internal static string Fill(HRStaff hrstaff, ref object[] parameters)
        {
            string query = string.Empty;
            if (hrstaff != null)
            {
                query = "SELECT [ID], [EmployeeNo], [TempStartDate], [EmploymentStartDate], [EmploymentEndDate], [FKCompanyID], [FKStaffTypeID], [FKWorkStatusID], [FKJobTitleID], [FKUserID], [FKRecruitmentConsultantID], [RecruitmentSource], [FKTitleID], [Initials], [FirstName], [PreferredName], [Surname], [FKGenderID], [FKRaceID], [IDNo], [DOB], [FKCitizenID], [FKReligionID], [TelHome], [TelCell], [TelOther], [EMail], [RegisteredTaxPayer], [TaxRefNo], [IRP5Number], [FKMedicalAidID], [CriminalOffence], [EmploymentDismissed], [DisciplinaryGuilty], [WorkedFinServicesIndustry], [WorkedFinServicesIndustryFrom], [WorkedFinServicesIndustryTo], [RegLevel1ExamDate], [Note], [FKAutomationStatusID], [AutomationDate], [StampDate], [StampUserID] FROM [HRStaff] WHERE [HRStaff].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", hrstaff.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  hrstaff data.
        /// </summary>
        /// <param name="hrstaff">The hrstaff to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  hrstaff data.</returns>
        internal static string FillData(HRStaff hrstaff, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (hrstaff != null)
            {
            query.Append("SELECT [HRStaff].[ID], [HRStaff].[EmployeeNo], [HRStaff].[TempStartDate], [HRStaff].[EmploymentStartDate], [HRStaff].[EmploymentEndDate], [HRStaff].[FKCompanyID], [HRStaff].[FKStaffTypeID], [HRStaff].[FKWorkStatusID], [HRStaff].[FKJobTitleID], [HRStaff].[FKUserID], [HRStaff].[FKRecruitmentConsultantID], [HRStaff].[RecruitmentSource], [HRStaff].[FKTitleID], [HRStaff].[Initials], [HRStaff].[FirstName], [HRStaff].[PreferredName], [HRStaff].[Surname], [HRStaff].[FKGenderID], [HRStaff].[FKRaceID], [HRStaff].[IDNo], [HRStaff].[DOB], [HRStaff].[FKCitizenID], [HRStaff].[FKReligionID], [HRStaff].[TelHome], [HRStaff].[TelCell], [HRStaff].[TelOther], [HRStaff].[EMail], [HRStaff].[RegisteredTaxPayer], [HRStaff].[TaxRefNo], [HRStaff].[IRP5Number], [HRStaff].[FKMedicalAidID], [HRStaff].[CriminalOffence], [HRStaff].[EmploymentDismissed], [HRStaff].[DisciplinaryGuilty], [HRStaff].[WorkedFinServicesIndustry], [HRStaff].[WorkedFinServicesIndustryFrom], [HRStaff].[WorkedFinServicesIndustryTo], [HRStaff].[RegLevel1ExamDate], [HRStaff].[Note], [HRStaff].[FKAutomationStatusID], [HRStaff].[AutomationDate], [HRStaff].[StampDate], [HRStaff].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [HRStaff].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [HRStaff] ");
                query.Append(" WHERE [HRStaff].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", hrstaff.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an hrstaff object from history.
        /// </summary>
        /// <param name="hrstaff">The hrstaff object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the hrstaff object from history.</returns>
        internal static string FillHistory(HRStaff hrstaff, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (hrstaff != null)
            {
                query = "SELECT [ID], [EmployeeNo], [TempStartDate], [EmploymentStartDate], [EmploymentEndDate], [FKCompanyID], [FKStaffTypeID], [FKWorkStatusID], [FKJobTitleID], [FKUserID], [FKRecruitmentConsultantID], [RecruitmentSource], [FKTitleID], [Initials], [FirstName], [PreferredName], [Surname], [FKGenderID], [FKRaceID], [IDNo], [DOB], [FKCitizenID], [FKReligionID], [TelHome], [TelCell], [TelOther], [EMail], [RegisteredTaxPayer], [TaxRefNo], [IRP5Number], [FKMedicalAidID], [CriminalOffence], [EmploymentDismissed], [DisciplinaryGuilty], [WorkedFinServicesIndustry], [WorkedFinServicesIndustryFrom], [WorkedFinServicesIndustryTo], [RegLevel1ExamDate], [Note], [FKAutomationStatusID], [AutomationDate], [StampDate], [StampUserID] FROM [zHstHRStaff] WHERE [zHstHRStaff].[ID] = @ID AND [zHstHRStaff].[StampUserID] = @StampUserID AND [zHstHRStaff].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", hrstaff.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the hrstaffs in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the hrstaffs in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [HRStaff].[ID], [HRStaff].[EmployeeNo], [HRStaff].[TempStartDate], [HRStaff].[EmploymentStartDate], [HRStaff].[EmploymentEndDate], [HRStaff].[FKCompanyID], [HRStaff].[FKStaffTypeID], [HRStaff].[FKWorkStatusID], [HRStaff].[FKJobTitleID], [HRStaff].[FKUserID], [HRStaff].[FKRecruitmentConsultantID], [HRStaff].[RecruitmentSource], [HRStaff].[FKTitleID], [HRStaff].[Initials], [HRStaff].[FirstName], [HRStaff].[PreferredName], [HRStaff].[Surname], [HRStaff].[FKGenderID], [HRStaff].[FKRaceID], [HRStaff].[IDNo], [HRStaff].[DOB], [HRStaff].[FKCitizenID], [HRStaff].[FKReligionID], [HRStaff].[TelHome], [HRStaff].[TelCell], [HRStaff].[TelOther], [HRStaff].[EMail], [HRStaff].[RegisteredTaxPayer], [HRStaff].[TaxRefNo], [HRStaff].[IRP5Number], [HRStaff].[FKMedicalAidID], [HRStaff].[CriminalOffence], [HRStaff].[EmploymentDismissed], [HRStaff].[DisciplinaryGuilty], [HRStaff].[WorkedFinServicesIndustry], [HRStaff].[WorkedFinServicesIndustryFrom], [HRStaff].[WorkedFinServicesIndustryTo], [HRStaff].[RegLevel1ExamDate], [HRStaff].[Note], [HRStaff].[FKAutomationStatusID], [HRStaff].[AutomationDate], [HRStaff].[StampDate], [HRStaff].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [HRStaff].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [HRStaff] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted hrstaffs in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted hrstaffs in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstHRStaff].[ID], [zHstHRStaff].[EmployeeNo], [zHstHRStaff].[TempStartDate], [zHstHRStaff].[EmploymentStartDate], [zHstHRStaff].[EmploymentEndDate], [zHstHRStaff].[FKCompanyID], [zHstHRStaff].[FKStaffTypeID], [zHstHRStaff].[FKWorkStatusID], [zHstHRStaff].[FKJobTitleID], [zHstHRStaff].[FKUserID], [zHstHRStaff].[FKRecruitmentConsultantID], [zHstHRStaff].[RecruitmentSource], [zHstHRStaff].[FKTitleID], [zHstHRStaff].[Initials], [zHstHRStaff].[FirstName], [zHstHRStaff].[PreferredName], [zHstHRStaff].[Surname], [zHstHRStaff].[FKGenderID], [zHstHRStaff].[FKRaceID], [zHstHRStaff].[IDNo], [zHstHRStaff].[DOB], [zHstHRStaff].[FKCitizenID], [zHstHRStaff].[FKReligionID], [zHstHRStaff].[TelHome], [zHstHRStaff].[TelCell], [zHstHRStaff].[TelOther], [zHstHRStaff].[EMail], [zHstHRStaff].[RegisteredTaxPayer], [zHstHRStaff].[TaxRefNo], [zHstHRStaff].[IRP5Number], [zHstHRStaff].[FKMedicalAidID], [zHstHRStaff].[CriminalOffence], [zHstHRStaff].[EmploymentDismissed], [zHstHRStaff].[DisciplinaryGuilty], [zHstHRStaff].[WorkedFinServicesIndustry], [zHstHRStaff].[WorkedFinServicesIndustryFrom], [zHstHRStaff].[WorkedFinServicesIndustryTo], [zHstHRStaff].[RegLevel1ExamDate], [zHstHRStaff].[Note], [zHstHRStaff].[FKAutomationStatusID], [zHstHRStaff].[AutomationDate], [zHstHRStaff].[StampDate], [zHstHRStaff].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstHRStaff].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstHRStaff] ");
            query.Append("INNER JOIN (SELECT [zHstHRStaff].[ID], MAX([zHstHRStaff].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstHRStaff] ");
            query.Append("WHERE [zHstHRStaff].[ID] NOT IN (SELECT [HRStaff].[ID] FROM [HRStaff]) ");
            query.Append("GROUP BY [zHstHRStaff].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstHRStaff].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstHRStaff].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) hrstaff in the database.
        /// </summary>
        /// <param name="hrstaff">The hrstaff object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) hrstaff in the database.</returns>
        public static string ListHistory(HRStaff hrstaff, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (hrstaff != null)
            {
            query.Append("SELECT [zHstHRStaff].[ID], [zHstHRStaff].[EmployeeNo], [zHstHRStaff].[TempStartDate], [zHstHRStaff].[EmploymentStartDate], [zHstHRStaff].[EmploymentEndDate], [zHstHRStaff].[FKCompanyID], [zHstHRStaff].[FKStaffTypeID], [zHstHRStaff].[FKWorkStatusID], [zHstHRStaff].[FKJobTitleID], [zHstHRStaff].[FKUserID], [zHstHRStaff].[FKRecruitmentConsultantID], [zHstHRStaff].[RecruitmentSource], [zHstHRStaff].[FKTitleID], [zHstHRStaff].[Initials], [zHstHRStaff].[FirstName], [zHstHRStaff].[PreferredName], [zHstHRStaff].[Surname], [zHstHRStaff].[FKGenderID], [zHstHRStaff].[FKRaceID], [zHstHRStaff].[IDNo], [zHstHRStaff].[DOB], [zHstHRStaff].[FKCitizenID], [zHstHRStaff].[FKReligionID], [zHstHRStaff].[TelHome], [zHstHRStaff].[TelCell], [zHstHRStaff].[TelOther], [zHstHRStaff].[EMail], [zHstHRStaff].[RegisteredTaxPayer], [zHstHRStaff].[TaxRefNo], [zHstHRStaff].[IRP5Number], [zHstHRStaff].[FKMedicalAidID], [zHstHRStaff].[CriminalOffence], [zHstHRStaff].[EmploymentDismissed], [zHstHRStaff].[DisciplinaryGuilty], [zHstHRStaff].[WorkedFinServicesIndustry], [zHstHRStaff].[WorkedFinServicesIndustryFrom], [zHstHRStaff].[WorkedFinServicesIndustryTo], [zHstHRStaff].[RegLevel1ExamDate], [zHstHRStaff].[Note], [zHstHRStaff].[FKAutomationStatusID], [zHstHRStaff].[AutomationDate], [zHstHRStaff].[StampDate], [zHstHRStaff].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstHRStaff].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstHRStaff] ");
                query.Append(" WHERE [zHstHRStaff].[ID] = @ID");
                query.Append(" ORDER BY [zHstHRStaff].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", hrstaff.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) hrstaff to the database.
        /// </summary>
        /// <param name="hrstaff">The hrstaff to save.</param>
        /// <returns>A query that can be used to save the hrstaff to the database.</returns>
        internal static string Save(HRStaff hrstaff, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (hrstaff != null)
            {
                if (hrstaff.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstHRStaff] ([ID], [EmployeeNo], [TempStartDate], [EmploymentStartDate], [EmploymentEndDate], [FKCompanyID], [FKStaffTypeID], [FKWorkStatusID], [FKJobTitleID], [FKUserID], [FKRecruitmentConsultantID], [RecruitmentSource], [FKTitleID], [Initials], [FirstName], [PreferredName], [Surname], [FKGenderID], [FKRaceID], [IDNo], [DOB], [FKCitizenID], [FKReligionID], [TelHome], [TelCell], [TelOther], [EMail], [RegisteredTaxPayer], [TaxRefNo], [IRP5Number], [FKMedicalAidID], [CriminalOffence], [EmploymentDismissed], [DisciplinaryGuilty], [WorkedFinServicesIndustry], [WorkedFinServicesIndustryFrom], [WorkedFinServicesIndustryTo], [RegLevel1ExamDate], [Note], [FKAutomationStatusID], [AutomationDate], [StampDate], [StampUserID]) SELECT [ID], [EmployeeNo], [TempStartDate], [EmploymentStartDate], [EmploymentEndDate], [FKCompanyID], [FKStaffTypeID], [FKWorkStatusID], [FKJobTitleID], [FKUserID], [FKRecruitmentConsultantID], [RecruitmentSource], [FKTitleID], [Initials], [FirstName], [PreferredName], [Surname], [FKGenderID], [FKRaceID], [IDNo], [DOB], [FKCitizenID], [FKReligionID], [TelHome], [TelCell], [TelOther], [EMail], [RegisteredTaxPayer], [TaxRefNo], [IRP5Number], [FKMedicalAidID], [CriminalOffence], [EmploymentDismissed], [DisciplinaryGuilty], [WorkedFinServicesIndustry], [WorkedFinServicesIndustryFrom], [WorkedFinServicesIndustryTo], [RegLevel1ExamDate], [Note], [FKAutomationStatusID], [AutomationDate], [StampDate], [StampUserID] FROM [HRStaff] WHERE [HRStaff].[ID] = @ID; ");
                    query.Append("UPDATE [HRStaff]");
                    parameters = new object[41];
                    query.Append(" SET [EmployeeNo] = @EmployeeNo");
                    parameters[0] = Database.GetParameter("@EmployeeNo", string.IsNullOrEmpty(hrstaff.EmployeeNo) ? DBNull.Value : (object)hrstaff.EmployeeNo);
                    query.Append(", [TempStartDate] = @TempStartDate");
                    parameters[1] = Database.GetParameter("@TempStartDate", hrstaff.TempStartDate.HasValue ? (object)hrstaff.TempStartDate.Value : DBNull.Value);
                    query.Append(", [EmploymentStartDate] = @EmploymentStartDate");
                    parameters[2] = Database.GetParameter("@EmploymentStartDate", hrstaff.EmploymentStartDate.HasValue ? (object)hrstaff.EmploymentStartDate.Value : DBNull.Value);
                    query.Append(", [EmploymentEndDate] = @EmploymentEndDate");
                    parameters[3] = Database.GetParameter("@EmploymentEndDate", hrstaff.EmploymentEndDate.HasValue ? (object)hrstaff.EmploymentEndDate.Value : DBNull.Value);
                    query.Append(", [FKCompanyID] = @FKCompanyID");
                    parameters[4] = Database.GetParameter("@FKCompanyID", hrstaff.FKCompanyID.HasValue ? (object)hrstaff.FKCompanyID.Value : DBNull.Value);
                    query.Append(", [FKStaffTypeID] = @FKStaffTypeID");
                    parameters[5] = Database.GetParameter("@FKStaffTypeID", hrstaff.FKStaffTypeID.HasValue ? (object)hrstaff.FKStaffTypeID.Value : DBNull.Value);
                    query.Append(", [FKWorkStatusID] = @FKWorkStatusID");
                    parameters[6] = Database.GetParameter("@FKWorkStatusID", hrstaff.FKWorkStatusID.HasValue ? (object)hrstaff.FKWorkStatusID.Value : DBNull.Value);
                    query.Append(", [FKJobTitleID] = @FKJobTitleID");
                    parameters[7] = Database.GetParameter("@FKJobTitleID", hrstaff.FKJobTitleID.HasValue ? (object)hrstaff.FKJobTitleID.Value : DBNull.Value);
                    query.Append(", [FKUserID] = @FKUserID");
                    parameters[8] = Database.GetParameter("@FKUserID", hrstaff.FKUserID.HasValue ? (object)hrstaff.FKUserID.Value : DBNull.Value);
                    query.Append(", [FKRecruitmentConsultantID] = @FKRecruitmentConsultantID");
                    parameters[9] = Database.GetParameter("@FKRecruitmentConsultantID", hrstaff.FKRecruitmentConsultantID.HasValue ? (object)hrstaff.FKRecruitmentConsultantID.Value : DBNull.Value);
                    query.Append(", [RecruitmentSource] = @RecruitmentSource");
                    parameters[10] = Database.GetParameter("@RecruitmentSource", string.IsNullOrEmpty(hrstaff.RecruitmentSource) ? DBNull.Value : (object)hrstaff.RecruitmentSource);
                    query.Append(", [FKTitleID] = @FKTitleID");
                    parameters[11] = Database.GetParameter("@FKTitleID", hrstaff.FKTitleID.HasValue ? (object)hrstaff.FKTitleID.Value : DBNull.Value);
                    query.Append(", [Initials] = @Initials");
                    parameters[12] = Database.GetParameter("@Initials", string.IsNullOrEmpty(hrstaff.Initials) ? DBNull.Value : (object)hrstaff.Initials);
                    query.Append(", [FirstName] = @FirstName");
                    parameters[13] = Database.GetParameter("@FirstName", string.IsNullOrEmpty(hrstaff.FirstName) ? DBNull.Value : (object)hrstaff.FirstName);
                    query.Append(", [PreferredName] = @PreferredName");
                    parameters[14] = Database.GetParameter("@PreferredName", string.IsNullOrEmpty(hrstaff.PreferredName) ? DBNull.Value : (object)hrstaff.PreferredName);
                    query.Append(", [Surname] = @Surname");
                    parameters[15] = Database.GetParameter("@Surname", string.IsNullOrEmpty(hrstaff.Surname) ? DBNull.Value : (object)hrstaff.Surname);
                    query.Append(", [FKGenderID] = @FKGenderID");
                    parameters[16] = Database.GetParameter("@FKGenderID", hrstaff.FKGenderID.HasValue ? (object)hrstaff.FKGenderID.Value : DBNull.Value);
                    query.Append(", [FKRaceID] = @FKRaceID");
                    parameters[17] = Database.GetParameter("@FKRaceID", hrstaff.FKRaceID.HasValue ? (object)hrstaff.FKRaceID.Value : DBNull.Value);
                    query.Append(", [IDNo] = @IDNo");
                    parameters[18] = Database.GetParameter("@IDNo", string.IsNullOrEmpty(hrstaff.IDNo) ? DBNull.Value : (object)hrstaff.IDNo);
                    query.Append(", [DOB] = @DOB");
                    parameters[19] = Database.GetParameter("@DOB", hrstaff.DOB.HasValue ? (object)hrstaff.DOB.Value : DBNull.Value);
                    query.Append(", [FKCitizenID] = @FKCitizenID");
                    parameters[20] = Database.GetParameter("@FKCitizenID", hrstaff.FKCitizenID.HasValue ? (object)hrstaff.FKCitizenID.Value : DBNull.Value);
                    query.Append(", [FKReligionID] = @FKReligionID");
                    parameters[21] = Database.GetParameter("@FKReligionID", hrstaff.FKReligionID.HasValue ? (object)hrstaff.FKReligionID.Value : DBNull.Value);
                    query.Append(", [TelHome] = @TelHome");
                    parameters[22] = Database.GetParameter("@TelHome", string.IsNullOrEmpty(hrstaff.TelHome) ? DBNull.Value : (object)hrstaff.TelHome);
                    query.Append(", [TelCell] = @TelCell");
                    parameters[23] = Database.GetParameter("@TelCell", string.IsNullOrEmpty(hrstaff.TelCell) ? DBNull.Value : (object)hrstaff.TelCell);
                    query.Append(", [TelOther] = @TelOther");
                    parameters[24] = Database.GetParameter("@TelOther", string.IsNullOrEmpty(hrstaff.TelOther) ? DBNull.Value : (object)hrstaff.TelOther);
                    query.Append(", [EMail] = @EMail");
                    parameters[25] = Database.GetParameter("@EMail", string.IsNullOrEmpty(hrstaff.EMail) ? DBNull.Value : (object)hrstaff.EMail);
                    query.Append(", [RegisteredTaxPayer] = @RegisteredTaxPayer");
                    parameters[26] = Database.GetParameter("@RegisteredTaxPayer", hrstaff.RegisteredTaxPayer.HasValue ? (object)hrstaff.RegisteredTaxPayer.Value : DBNull.Value);
                    query.Append(", [TaxRefNo] = @TaxRefNo");
                    parameters[27] = Database.GetParameter("@TaxRefNo", string.IsNullOrEmpty(hrstaff.TaxRefNo) ? DBNull.Value : (object)hrstaff.TaxRefNo);
                    query.Append(", [IRP5Number] = @IRP5Number");
                    parameters[28] = Database.GetParameter("@IRP5Number", string.IsNullOrEmpty(hrstaff.IRP5Number) ? DBNull.Value : (object)hrstaff.IRP5Number);
                    query.Append(", [FKMedicalAidID] = @FKMedicalAidID");
                    parameters[29] = Database.GetParameter("@FKMedicalAidID", hrstaff.FKMedicalAidID.HasValue ? (object)hrstaff.FKMedicalAidID.Value : DBNull.Value);
                    query.Append(", [CriminalOffence] = @CriminalOffence");
                    parameters[30] = Database.GetParameter("@CriminalOffence", hrstaff.CriminalOffence.HasValue ? (object)hrstaff.CriminalOffence.Value : DBNull.Value);
                    query.Append(", [EmploymentDismissed] = @EmploymentDismissed");
                    parameters[31] = Database.GetParameter("@EmploymentDismissed", hrstaff.EmploymentDismissed.HasValue ? (object)hrstaff.EmploymentDismissed.Value : DBNull.Value);
                    query.Append(", [DisciplinaryGuilty] = @DisciplinaryGuilty");
                    parameters[32] = Database.GetParameter("@DisciplinaryGuilty", hrstaff.DisciplinaryGuilty.HasValue ? (object)hrstaff.DisciplinaryGuilty.Value : DBNull.Value);
                    query.Append(", [WorkedFinServicesIndustry] = @WorkedFinServicesIndustry");
                    parameters[33] = Database.GetParameter("@WorkedFinServicesIndustry", hrstaff.WorkedFinServicesIndustry.HasValue ? (object)hrstaff.WorkedFinServicesIndustry.Value : DBNull.Value);
                    query.Append(", [WorkedFinServicesIndustryFrom] = @WorkedFinServicesIndustryFrom");
                    parameters[34] = Database.GetParameter("@WorkedFinServicesIndustryFrom", hrstaff.WorkedFinServicesIndustryFrom.HasValue ? (object)hrstaff.WorkedFinServicesIndustryFrom.Value : DBNull.Value);
                    query.Append(", [WorkedFinServicesIndustryTo] = @WorkedFinServicesIndustryTo");
                    parameters[35] = Database.GetParameter("@WorkedFinServicesIndustryTo", hrstaff.WorkedFinServicesIndustryTo.HasValue ? (object)hrstaff.WorkedFinServicesIndustryTo.Value : DBNull.Value);
                    query.Append(", [RegLevel1ExamDate] = @RegLevel1ExamDate");
                    parameters[36] = Database.GetParameter("@RegLevel1ExamDate", hrstaff.RegLevel1ExamDate.HasValue ? (object)hrstaff.RegLevel1ExamDate.Value : DBNull.Value);
                    query.Append(", [Note] = @Note");
                    parameters[37] = Database.GetParameter("@Note", string.IsNullOrEmpty(hrstaff.Note) ? DBNull.Value : (object)hrstaff.Note);
                    query.Append(", [FKAutomationStatusID] = @FKAutomationStatusID");
                    parameters[38] = Database.GetParameter("@FKAutomationStatusID", hrstaff.FKAutomationStatusID.HasValue ? (object)hrstaff.FKAutomationStatusID.Value : DBNull.Value);
                    query.Append(", [AutomationDate] = @AutomationDate");
                    parameters[39] = Database.GetParameter("@AutomationDate", hrstaff.AutomationDate.HasValue ? (object)hrstaff.AutomationDate.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [HRStaff].[ID] = @ID"); 
                    parameters[40] = Database.GetParameter("@ID", hrstaff.ID);
                }
                else
                {
                    query.Append("INSERT INTO [HRStaff] ([EmployeeNo], [TempStartDate], [EmploymentStartDate], [EmploymentEndDate], [FKCompanyID], [FKStaffTypeID], [FKWorkStatusID], [FKJobTitleID], [FKUserID], [FKRecruitmentConsultantID], [RecruitmentSource], [FKTitleID], [Initials], [FirstName], [PreferredName], [Surname], [FKGenderID], [FKRaceID], [IDNo], [DOB], [FKCitizenID], [FKReligionID], [TelHome], [TelCell], [TelOther], [EMail], [RegisteredTaxPayer], [TaxRefNo], [IRP5Number], [FKMedicalAidID], [CriminalOffence], [EmploymentDismissed], [DisciplinaryGuilty], [WorkedFinServicesIndustry], [WorkedFinServicesIndustryFrom], [WorkedFinServicesIndustryTo], [RegLevel1ExamDate], [Note], [FKAutomationStatusID], [AutomationDate], [StampDate], [StampUserID]) VALUES(@EmployeeNo, @TempStartDate, @EmploymentStartDate, @EmploymentEndDate, @FKCompanyID, @FKStaffTypeID, @FKWorkStatusID, @FKJobTitleID, @FKUserID, @FKRecruitmentConsultantID, @RecruitmentSource, @FKTitleID, @Initials, @FirstName, @PreferredName, @Surname, @FKGenderID, @FKRaceID, @IDNo, @DOB, @FKCitizenID, @FKReligionID, @TelHome, @TelCell, @TelOther, @EMail, @RegisteredTaxPayer, @TaxRefNo, @IRP5Number, @FKMedicalAidID, @CriminalOffence, @EmploymentDismissed, @DisciplinaryGuilty, @WorkedFinServicesIndustry, @WorkedFinServicesIndustryFrom, @WorkedFinServicesIndustryTo, @RegLevel1ExamDate, @Note, @FKAutomationStatusID, @AutomationDate, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[40];
                    parameters[0] = Database.GetParameter("@EmployeeNo", string.IsNullOrEmpty(hrstaff.EmployeeNo) ? DBNull.Value : (object)hrstaff.EmployeeNo);
                    parameters[1] = Database.GetParameter("@TempStartDate", hrstaff.TempStartDate.HasValue ? (object)hrstaff.TempStartDate.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@EmploymentStartDate", hrstaff.EmploymentStartDate.HasValue ? (object)hrstaff.EmploymentStartDate.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@EmploymentEndDate", hrstaff.EmploymentEndDate.HasValue ? (object)hrstaff.EmploymentEndDate.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@FKCompanyID", hrstaff.FKCompanyID.HasValue ? (object)hrstaff.FKCompanyID.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@FKStaffTypeID", hrstaff.FKStaffTypeID.HasValue ? (object)hrstaff.FKStaffTypeID.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@FKWorkStatusID", hrstaff.FKWorkStatusID.HasValue ? (object)hrstaff.FKWorkStatusID.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@FKJobTitleID", hrstaff.FKJobTitleID.HasValue ? (object)hrstaff.FKJobTitleID.Value : DBNull.Value);
                    parameters[8] = Database.GetParameter("@FKUserID", hrstaff.FKUserID.HasValue ? (object)hrstaff.FKUserID.Value : DBNull.Value);
                    parameters[9] = Database.GetParameter("@FKRecruitmentConsultantID", hrstaff.FKRecruitmentConsultantID.HasValue ? (object)hrstaff.FKRecruitmentConsultantID.Value : DBNull.Value);
                    parameters[10] = Database.GetParameter("@RecruitmentSource", string.IsNullOrEmpty(hrstaff.RecruitmentSource) ? DBNull.Value : (object)hrstaff.RecruitmentSource);
                    parameters[11] = Database.GetParameter("@FKTitleID", hrstaff.FKTitleID.HasValue ? (object)hrstaff.FKTitleID.Value : DBNull.Value);
                    parameters[12] = Database.GetParameter("@Initials", string.IsNullOrEmpty(hrstaff.Initials) ? DBNull.Value : (object)hrstaff.Initials);
                    parameters[13] = Database.GetParameter("@FirstName", string.IsNullOrEmpty(hrstaff.FirstName) ? DBNull.Value : (object)hrstaff.FirstName);
                    parameters[14] = Database.GetParameter("@PreferredName", string.IsNullOrEmpty(hrstaff.PreferredName) ? DBNull.Value : (object)hrstaff.PreferredName);
                    parameters[15] = Database.GetParameter("@Surname", string.IsNullOrEmpty(hrstaff.Surname) ? DBNull.Value : (object)hrstaff.Surname);
                    parameters[16] = Database.GetParameter("@FKGenderID", hrstaff.FKGenderID.HasValue ? (object)hrstaff.FKGenderID.Value : DBNull.Value);
                    parameters[17] = Database.GetParameter("@FKRaceID", hrstaff.FKRaceID.HasValue ? (object)hrstaff.FKRaceID.Value : DBNull.Value);
                    parameters[18] = Database.GetParameter("@IDNo", string.IsNullOrEmpty(hrstaff.IDNo) ? DBNull.Value : (object)hrstaff.IDNo);
                    parameters[19] = Database.GetParameter("@DOB", hrstaff.DOB.HasValue ? (object)hrstaff.DOB.Value : DBNull.Value);
                    parameters[20] = Database.GetParameter("@FKCitizenID", hrstaff.FKCitizenID.HasValue ? (object)hrstaff.FKCitizenID.Value : DBNull.Value);
                    parameters[21] = Database.GetParameter("@FKReligionID", hrstaff.FKReligionID.HasValue ? (object)hrstaff.FKReligionID.Value : DBNull.Value);
                    parameters[22] = Database.GetParameter("@TelHome", string.IsNullOrEmpty(hrstaff.TelHome) ? DBNull.Value : (object)hrstaff.TelHome);
                    parameters[23] = Database.GetParameter("@TelCell", string.IsNullOrEmpty(hrstaff.TelCell) ? DBNull.Value : (object)hrstaff.TelCell);
                    parameters[24] = Database.GetParameter("@TelOther", string.IsNullOrEmpty(hrstaff.TelOther) ? DBNull.Value : (object)hrstaff.TelOther);
                    parameters[25] = Database.GetParameter("@EMail", string.IsNullOrEmpty(hrstaff.EMail) ? DBNull.Value : (object)hrstaff.EMail);
                    parameters[26] = Database.GetParameter("@RegisteredTaxPayer", hrstaff.RegisteredTaxPayer.HasValue ? (object)hrstaff.RegisteredTaxPayer.Value : DBNull.Value);
                    parameters[27] = Database.GetParameter("@TaxRefNo", string.IsNullOrEmpty(hrstaff.TaxRefNo) ? DBNull.Value : (object)hrstaff.TaxRefNo);
                    parameters[28] = Database.GetParameter("@IRP5Number", string.IsNullOrEmpty(hrstaff.IRP5Number) ? DBNull.Value : (object)hrstaff.IRP5Number);
                    parameters[29] = Database.GetParameter("@FKMedicalAidID", hrstaff.FKMedicalAidID.HasValue ? (object)hrstaff.FKMedicalAidID.Value : DBNull.Value);
                    parameters[30] = Database.GetParameter("@CriminalOffence", hrstaff.CriminalOffence.HasValue ? (object)hrstaff.CriminalOffence.Value : DBNull.Value);
                    parameters[31] = Database.GetParameter("@EmploymentDismissed", hrstaff.EmploymentDismissed.HasValue ? (object)hrstaff.EmploymentDismissed.Value : DBNull.Value);
                    parameters[32] = Database.GetParameter("@DisciplinaryGuilty", hrstaff.DisciplinaryGuilty.HasValue ? (object)hrstaff.DisciplinaryGuilty.Value : DBNull.Value);
                    parameters[33] = Database.GetParameter("@WorkedFinServicesIndustry", hrstaff.WorkedFinServicesIndustry.HasValue ? (object)hrstaff.WorkedFinServicesIndustry.Value : DBNull.Value);
                    parameters[34] = Database.GetParameter("@WorkedFinServicesIndustryFrom", hrstaff.WorkedFinServicesIndustryFrom.HasValue ? (object)hrstaff.WorkedFinServicesIndustryFrom.Value : DBNull.Value);
                    parameters[35] = Database.GetParameter("@WorkedFinServicesIndustryTo", hrstaff.WorkedFinServicesIndustryTo.HasValue ? (object)hrstaff.WorkedFinServicesIndustryTo.Value : DBNull.Value);
                    parameters[36] = Database.GetParameter("@RegLevel1ExamDate", hrstaff.RegLevel1ExamDate.HasValue ? (object)hrstaff.RegLevel1ExamDate.Value : DBNull.Value);
                    parameters[37] = Database.GetParameter("@Note", string.IsNullOrEmpty(hrstaff.Note) ? DBNull.Value : (object)hrstaff.Note);
                    parameters[38] = Database.GetParameter("@FKAutomationStatusID", hrstaff.FKAutomationStatusID.HasValue ? (object)hrstaff.FKAutomationStatusID.Value : DBNull.Value);
                    parameters[39] = Database.GetParameter("@AutomationDate", hrstaff.AutomationDate.HasValue ? (object)hrstaff.AutomationDate.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for hrstaffs that match the search criteria.
        /// </summary>
        /// <param name="employeeno">The employeeno search criteria.</param>
        /// <param name="tempstartdate">The tempstartdate search criteria.</param>
        /// <param name="employmentstartdate">The employmentstartdate search criteria.</param>
        /// <param name="employmentenddate">The employmentenddate search criteria.</param>
        /// <param name="fkcompanyid">The fkcompanyid search criteria.</param>
        /// <param name="fkstafftypeid">The fkstafftypeid search criteria.</param>
        /// <param name="fkworkstatusid">The fkworkstatusid search criteria.</param>
        /// <param name="fkjobtitleid">The fkjobtitleid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fkrecruitmentconsultantid">The fkrecruitmentconsultantid search criteria.</param>
        /// <param name="recruitmentsource">The recruitmentsource search criteria.</param>
        /// <param name="fktitleid">The fktitleid search criteria.</param>
        /// <param name="initials">The initials search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="preferredname">The preferredname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="fkgenderid">The fkgenderid search criteria.</param>
        /// <param name="fkraceid">The fkraceid search criteria.</param>
        /// <param name="idno">The idno search criteria.</param>
        /// <param name="dob">The dob search criteria.</param>
        /// <param name="fkcitizenid">The fkcitizenid search criteria.</param>
        /// <param name="fkreligionid">The fkreligionid search criteria.</param>
        /// <param name="telhome">The telhome search criteria.</param>
        /// <param name="telcell">The telcell search criteria.</param>
        /// <param name="telother">The telother search criteria.</param>
        /// <param name="email">The email search criteria.</param>
        /// <param name="registeredtaxpayer">The registeredtaxpayer search criteria.</param>
        /// <param name="taxrefno">The taxrefno search criteria.</param>
        /// <param name="irp5number">The irp5number search criteria.</param>
        /// <param name="fkmedicalaidid">The fkmedicalaidid search criteria.</param>
        /// <param name="criminaloffence">The criminaloffence search criteria.</param>
        /// <param name="employmentdismissed">The employmentdismissed search criteria.</param>
        /// <param name="disciplinaryguilty">The disciplinaryguilty search criteria.</param>
        /// <param name="workedfinservicesindustry">The workedfinservicesindustry search criteria.</param>
        /// <param name="workedfinservicesindustryfrom">The workedfinservicesindustryfrom search criteria.</param>
        /// <param name="workedfinservicesindustryto">The workedfinservicesindustryto search criteria.</param>
        /// <param name="reglevel1examdate">The reglevel1examdate search criteria.</param>
        /// <param name="note">The note search criteria.</param>
        /// <param name="fkautomationstatusid">The fkautomationstatusid search criteria.</param>
        /// <param name="automationdate">The automationdate search criteria.</param>
        /// <returns>A query that can be used to search for hrstaffs based on the search criteria.</returns>
        internal static string Search(string employeeno, DateTime? tempstartdate, DateTime? employmentstartdate, DateTime? employmentenddate, long? fkcompanyid, long? fkstafftypeid, long? fkworkstatusid, long? fkjobtitleid, long? fkuserid, long? fkrecruitmentconsultantid, string recruitmentsource, long? fktitleid, string initials, string firstname, string preferredname, string surname, long? fkgenderid, long? fkraceid, string idno, DateTime? dob, long? fkcitizenid, long? fkreligionid, string telhome, string telcell, string telother, string email, bool? registeredtaxpayer, string taxrefno, string irp5number, long? fkmedicalaidid, bool? criminaloffence, bool? employmentdismissed, bool? disciplinaryguilty, bool? workedfinservicesindustry, DateTime? workedfinservicesindustryfrom, DateTime? workedfinservicesindustryto, DateTime? reglevel1examdate, string note, long? fkautomationstatusid, DateTime? automationdate)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (employeeno != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[EmployeeNo] LIKE '" + employeeno.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (tempstartdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[TempStartDate] = '" + tempstartdate.Value.ToString(Database.DateFormat) + "'");
            }
            if (employmentstartdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[EmploymentStartDate] = '" + employmentstartdate.Value.ToString(Database.DateFormat) + "'");
            }
            if (employmentenddate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[EmploymentEndDate] = '" + employmentenddate.Value.ToString(Database.DateFormat) + "'");
            }
            if (fkcompanyid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[FKCompanyID] = " + fkcompanyid + "");
            }
            if (fkstafftypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[FKStaffTypeID] = " + fkstafftypeid + "");
            }
            if (fkworkstatusid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[FKWorkStatusID] = " + fkworkstatusid + "");
            }
            if (fkjobtitleid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[FKJobTitleID] = " + fkjobtitleid + "");
            }
            if (fkuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[FKUserID] = " + fkuserid + "");
            }
            if (fkrecruitmentconsultantid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[FKRecruitmentConsultantID] = " + fkrecruitmentconsultantid + "");
            }
            if (recruitmentsource != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[RecruitmentSource] LIKE '" + recruitmentsource.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fktitleid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[FKTitleID] = " + fktitleid + "");
            }
            if (initials != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[Initials] LIKE '" + initials.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (firstname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[FirstName] LIKE '" + firstname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (preferredname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[PreferredName] LIKE '" + preferredname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (surname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[Surname] LIKE '" + surname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkgenderid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[FKGenderID] = " + fkgenderid + "");
            }
            if (fkraceid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[FKRaceID] = " + fkraceid + "");
            }
            if (idno != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[IDNo] LIKE '" + idno.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (dob != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[DOB] = '" + dob.Value.ToString(Database.DateFormat) + "'");
            }
            if (fkcitizenid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[FKCitizenID] = " + fkcitizenid + "");
            }
            if (fkreligionid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[FKReligionID] = " + fkreligionid + "");
            }
            if (telhome != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[TelHome] LIKE '" + telhome.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (telcell != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[TelCell] LIKE '" + telcell.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (telother != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[TelOther] LIKE '" + telother.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (email != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[EMail] LIKE '" + email.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (registeredtaxpayer != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[RegisteredTaxPayer] = " + ((bool)registeredtaxpayer ? "1" : "0"));
            }
            if (taxrefno != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[TaxRefNo] LIKE '" + taxrefno.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (irp5number != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[IRP5Number] LIKE '" + irp5number.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkmedicalaidid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[FKMedicalAidID] = " + fkmedicalaidid + "");
            }
            if (criminaloffence != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[CriminalOffence] = " + ((bool)criminaloffence ? "1" : "0"));
            }
            if (employmentdismissed != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[EmploymentDismissed] = " + ((bool)employmentdismissed ? "1" : "0"));
            }
            if (disciplinaryguilty != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[DisciplinaryGuilty] = " + ((bool)disciplinaryguilty ? "1" : "0"));
            }
            if (workedfinservicesindustry != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[WorkedFinServicesIndustry] = " + ((bool)workedfinservicesindustry ? "1" : "0"));
            }
            if (workedfinservicesindustryfrom != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[WorkedFinServicesIndustryFrom] = '" + workedfinservicesindustryfrom.Value.ToString(Database.DateFormat) + "'");
            }
            if (workedfinservicesindustryto != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[WorkedFinServicesIndustryTo] = '" + workedfinservicesindustryto.Value.ToString(Database.DateFormat) + "'");
            }
            if (reglevel1examdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[RegLevel1ExamDate] = '" + reglevel1examdate.Value.ToString(Database.DateFormat) + "'");
            }
            if (note != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[Note] LIKE '" + note.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkautomationstatusid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[FKAutomationStatusID] = " + fkautomationstatusid + "");
            }
            if (automationdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[HRStaff].[AutomationDate] = '" + automationdate.Value.ToString(Database.DateFormat) + "'");
            }
            query.Append("SELECT [HRStaff].[ID], [HRStaff].[EmployeeNo], [HRStaff].[TempStartDate], [HRStaff].[EmploymentStartDate], [HRStaff].[EmploymentEndDate], [HRStaff].[FKCompanyID], [HRStaff].[FKStaffTypeID], [HRStaff].[FKWorkStatusID], [HRStaff].[FKJobTitleID], [HRStaff].[FKUserID], [HRStaff].[FKRecruitmentConsultantID], [HRStaff].[RecruitmentSource], [HRStaff].[FKTitleID], [HRStaff].[Initials], [HRStaff].[FirstName], [HRStaff].[PreferredName], [HRStaff].[Surname], [HRStaff].[FKGenderID], [HRStaff].[FKRaceID], [HRStaff].[IDNo], [HRStaff].[DOB], [HRStaff].[FKCitizenID], [HRStaff].[FKReligionID], [HRStaff].[TelHome], [HRStaff].[TelCell], [HRStaff].[TelOther], [HRStaff].[EMail], [HRStaff].[RegisteredTaxPayer], [HRStaff].[TaxRefNo], [HRStaff].[IRP5Number], [HRStaff].[FKMedicalAidID], [HRStaff].[CriminalOffence], [HRStaff].[EmploymentDismissed], [HRStaff].[DisciplinaryGuilty], [HRStaff].[WorkedFinServicesIndustry], [HRStaff].[WorkedFinServicesIndustryFrom], [HRStaff].[WorkedFinServicesIndustryTo], [HRStaff].[RegLevel1ExamDate], [HRStaff].[Note], [HRStaff].[FKAutomationStatusID], [HRStaff].[AutomationDate], [HRStaff].[StampDate], [HRStaff].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [HRStaff].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [HRStaff] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
