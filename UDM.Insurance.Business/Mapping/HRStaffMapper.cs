using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Collections;

using UDM.HR.Business;
using UDM.HR.Business.Queries;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using Embriant.Framework.Exceptions;
using Embriant.Framework;
using Embriant.Framework.Validation;

namespace UDM.HR.Business.Mapping
{
    /// <summary>
    /// Contains methods to fill, save and delete hrstaff objects.
    /// </summary>
    public partial class HRStaffMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) hrstaff object from the database.
        /// </summary>
        /// <param name="hrstaff">The id of the hrstaff object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the hrstaff object.</param>
        /// <returns>True if the hrstaff object was deleted successfully, else false.</returns>
        internal static bool Delete(HRStaff hrstaff)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(hrstaff.ConnectionName, HRStaffQueries.Delete(hrstaff, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a HRStaff object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) hrstaff object from the database.
        /// </summary>
        /// <param name="hrstaff">The id of the hrstaff object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the hrstaff history.</param>
        /// <returns>True if the hrstaff history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(HRStaff hrstaff)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(hrstaff.ConnectionName, HRStaffQueries.DeleteHistory(hrstaff, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete HRStaff history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) hrstaff object from the database.
        /// </summary>
        /// <param name="hrstaff">The id of the hrstaff object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the hrstaff object.</param>
        /// <returns>True if the hrstaff object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(HRStaff hrstaff)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(hrstaff.ConnectionName, HRStaffQueries.UnDelete(hrstaff, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a HRStaff object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the hrstaff object from the data reader.
        /// </summary>
        /// <param name="hrstaff">The hrstaff object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(HRStaff hrstaff, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    hrstaff.IsLoaded = true;
                    hrstaff.EmployeeNo = reader["EmployeeNo"] != DBNull.Value ? (string)reader["EmployeeNo"] : (string)null;
                    hrstaff.TempStartDate = reader["TempStartDate"] != DBNull.Value ? (DateTime)reader["TempStartDate"] : (DateTime?)null;
                    hrstaff.EmploymentStartDate = reader["EmploymentStartDate"] != DBNull.Value ? (DateTime)reader["EmploymentStartDate"] : (DateTime?)null;
                    hrstaff.EmploymentEndDate = reader["EmploymentEndDate"] != DBNull.Value ? (DateTime)reader["EmploymentEndDate"] : (DateTime?)null;
                    hrstaff.FKCompanyID = reader["FKCompanyID"] != DBNull.Value ? (long)reader["FKCompanyID"] : (long?)null;
                    hrstaff.FKStaffTypeID = reader["FKStaffTypeID"] != DBNull.Value ? (long)reader["FKStaffTypeID"] : (long?)null;
                    hrstaff.FKWorkStatusID = reader["FKWorkStatusID"] != DBNull.Value ? (long)reader["FKWorkStatusID"] : (long?)null;
                    hrstaff.FKJobTitleID = reader["FKJobTitleID"] != DBNull.Value ? (long)reader["FKJobTitleID"] : (long?)null;
                    hrstaff.FKUserID = reader["FKUserID"] != DBNull.Value ? (long)reader["FKUserID"] : (long?)null;
                    hrstaff.FKRecruitmentConsultantID = reader["FKRecruitmentConsultantID"] != DBNull.Value ? (long)reader["FKRecruitmentConsultantID"] : (long?)null;
                    hrstaff.RecruitmentSource = reader["RecruitmentSource"] != DBNull.Value ? (string)reader["RecruitmentSource"] : (string)null;
                    hrstaff.FKTitleID = reader["FKTitleID"] != DBNull.Value ? (long)reader["FKTitleID"] : (long?)null;
                    hrstaff.Initials = reader["Initials"] != DBNull.Value ? (string)reader["Initials"] : (string)null;
                    hrstaff.FirstName = reader["FirstName"] != DBNull.Value ? (string)reader["FirstName"] : (string)null;
                    hrstaff.PreferredName = reader["PreferredName"] != DBNull.Value ? (string)reader["PreferredName"] : (string)null;
                    hrstaff.Surname = reader["Surname"] != DBNull.Value ? (string)reader["Surname"] : (string)null;
                    hrstaff.FKGenderID = reader["FKGenderID"] != DBNull.Value ? (long)reader["FKGenderID"] : (long?)null;
                    hrstaff.FKRaceID = reader["FKRaceID"] != DBNull.Value ? (long)reader["FKRaceID"] : (long?)null;
                    hrstaff.IDNo = reader["IDNo"] != DBNull.Value ? (string)reader["IDNo"] : (string)null;
                    hrstaff.DOB = reader["DOB"] != DBNull.Value ? (DateTime)reader["DOB"] : (DateTime?)null;
                    hrstaff.FKCitizenID = reader["FKCitizenID"] != DBNull.Value ? (long)reader["FKCitizenID"] : (long?)null;
                    hrstaff.FKReligionID = reader["FKReligionID"] != DBNull.Value ? (long)reader["FKReligionID"] : (long?)null;
                    hrstaff.TelHome = reader["TelHome"] != DBNull.Value ? (string)reader["TelHome"] : (string)null;
                    hrstaff.TelCell = reader["TelCell"] != DBNull.Value ? (string)reader["TelCell"] : (string)null;
                    hrstaff.TelOther = reader["TelOther"] != DBNull.Value ? (string)reader["TelOther"] : (string)null;
                    hrstaff.EMail = reader["EMail"] != DBNull.Value ? (string)reader["EMail"] : (string)null;
                    hrstaff.RegisteredTaxPayer = reader["RegisteredTaxPayer"] != DBNull.Value ? (bool)reader["RegisteredTaxPayer"] : (bool?)null;
                    hrstaff.TaxRefNo = reader["TaxRefNo"] != DBNull.Value ? (string)reader["TaxRefNo"] : (string)null;
                    hrstaff.IRP5Number = reader["IRP5Number"] != DBNull.Value ? (string)reader["IRP5Number"] : (string)null;
                    hrstaff.FKMedicalAidID = reader["FKMedicalAidID"] != DBNull.Value ? (long)reader["FKMedicalAidID"] : (long?)null;
                    hrstaff.CriminalOffence = reader["CriminalOffence"] != DBNull.Value ? (bool)reader["CriminalOffence"] : (bool?)null;
                    hrstaff.EmploymentDismissed = reader["EmploymentDismissed"] != DBNull.Value ? (bool)reader["EmploymentDismissed"] : (bool?)null;
                    hrstaff.DisciplinaryGuilty = reader["DisciplinaryGuilty"] != DBNull.Value ? (bool)reader["DisciplinaryGuilty"] : (bool?)null;
                    hrstaff.WorkedFinServicesIndustry = reader["WorkedFinServicesIndustry"] != DBNull.Value ? (bool)reader["WorkedFinServicesIndustry"] : (bool?)null;
                    hrstaff.WorkedFinServicesIndustryFrom = reader["WorkedFinServicesIndustryFrom"] != DBNull.Value ? (DateTime)reader["WorkedFinServicesIndustryFrom"] : (DateTime?)null;
                    hrstaff.WorkedFinServicesIndustryTo = reader["WorkedFinServicesIndustryTo"] != DBNull.Value ? (DateTime)reader["WorkedFinServicesIndustryTo"] : (DateTime?)null;
                    hrstaff.RegLevel1ExamDate = reader["RegLevel1ExamDate"] != DBNull.Value ? (DateTime)reader["RegLevel1ExamDate"] : (DateTime?)null;
                    hrstaff.Note = reader["Note"] != DBNull.Value ? (string)reader["Note"] : (string)null;
                    hrstaff.FKAutomationStatusID = reader["FKAutomationStatusID"] != DBNull.Value ? (long)reader["FKAutomationStatusID"] : (long?)null;
                    hrstaff.AutomationDate = reader["AutomationDate"] != DBNull.Value ? (DateTime)reader["AutomationDate"] : (DateTime?)null;
                    hrstaff.StampDate = (DateTime)reader["StampDate"];
                    hrstaff.HasChanged = false;
                }
                else
                {
                    throw new MapperException("HRStaff does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a HRStaff object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an hrstaff object from the database.
        /// </summary>
        /// <param name="hrstaff">The hrstaff to fill.</param>
        internal static void Fill(HRStaff hrstaff)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(hrstaff.ConnectionName, HRStaffQueries.Fill(hrstaff, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(hrstaff, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a HRStaff object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a hrstaff object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(HRStaff hrstaff)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(hrstaff.ConnectionName, HRStaffQueries.FillData(hrstaff, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a HRStaff object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an hrstaff object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="hrstaff">The hrstaff to fill from history.</param>
        internal static void FillHistory(HRStaff hrstaff, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(hrstaff.ConnectionName, HRStaffQueries.FillHistory(hrstaff, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(hrstaff, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a HRStaff object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the hrstaff objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static HRStaffCollection List(bool showDeleted, string connectionName)
        {
            HRStaffCollection collection = new HRStaffCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? HRStaffQueries.ListDeleted() : HRStaffQueries.List(), null);
                while (reader.Read())
                {
                    HRStaff hrstaff = new HRStaff((long)reader["ID"]);
                    hrstaff.ConnectionName = connectionName;
                    collection.Add(hrstaff);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list HRStaff objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the hrstaff objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? HRStaffQueries.ListDeleted() : HRStaffQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list HRStaff objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) hrstaff object from the database.
        /// </summary>
        /// <param name="hrstaff">The hrstaff to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(HRStaff hrstaff)
        {
            HRStaffCollection collection = new HRStaffCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(hrstaff.ConnectionName, HRStaffQueries.ListHistory(hrstaff, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) HRStaff in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an hrstaff object to the database.
        /// </summary>
        /// <param name="hrstaff">The HRStaff object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the hrstaff was saved successfull, otherwise, false.</returns>
        internal static bool Save(HRStaff hrstaff)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (hrstaff.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(hrstaff.ConnectionName, HRStaffQueries.Save(hrstaff, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(hrstaff.ConnectionName, HRStaffQueries.Save(hrstaff, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            hrstaff.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= hrstaff.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a HRStaff object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for hrstaff objects in the database.
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
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static HRStaffCollection Search(string employeeno, DateTime? tempstartdate, DateTime? employmentstartdate, DateTime? employmentenddate, long? fkcompanyid, long? fkstafftypeid, long? fkworkstatusid, long? fkjobtitleid, long? fkuserid, long? fkrecruitmentconsultantid, string recruitmentsource, long? fktitleid, string initials, string firstname, string preferredname, string surname, long? fkgenderid, long? fkraceid, string idno, DateTime? dob, long? fkcitizenid, long? fkreligionid, string telhome, string telcell, string telother, string email, bool? registeredtaxpayer, string taxrefno, string irp5number, long? fkmedicalaidid, bool? criminaloffence, bool? employmentdismissed, bool? disciplinaryguilty, bool? workedfinservicesindustry, DateTime? workedfinservicesindustryfrom, DateTime? workedfinservicesindustryto, DateTime? reglevel1examdate, string note, long? fkautomationstatusid, DateTime? automationdate, string connectionName)
        {
            HRStaffCollection collection = new HRStaffCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, HRStaffQueries.Search(employeeno, tempstartdate, employmentstartdate, employmentenddate, fkcompanyid, fkstafftypeid, fkworkstatusid, fkjobtitleid, fkuserid, fkrecruitmentconsultantid, recruitmentsource, fktitleid, initials, firstname, preferredname, surname, fkgenderid, fkraceid, idno, dob, fkcitizenid, fkreligionid, telhome, telcell, telother, email, registeredtaxpayer, taxrefno, irp5number, fkmedicalaidid, criminaloffence, employmentdismissed, disciplinaryguilty, workedfinservicesindustry, workedfinservicesindustryfrom, workedfinservicesindustryto, reglevel1examdate, note, fkautomationstatusid, automationdate), null);
                while (reader.Read())
                {
                    HRStaff hrstaff = new HRStaff((long)reader["ID"]);
                    hrstaff.ConnectionName = connectionName;
                    collection.Add(hrstaff);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for HRStaff objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for hrstaff objects in the database.
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
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(string employeeno, DateTime? tempstartdate, DateTime? employmentstartdate, DateTime? employmentenddate, long? fkcompanyid, long? fkstafftypeid, long? fkworkstatusid, long? fkjobtitleid, long? fkuserid, long? fkrecruitmentconsultantid, string recruitmentsource, long? fktitleid, string initials, string firstname, string preferredname, string surname, long? fkgenderid, long? fkraceid, string idno, DateTime? dob, long? fkcitizenid, long? fkreligionid, string telhome, string telcell, string telother, string email, bool? registeredtaxpayer, string taxrefno, string irp5number, long? fkmedicalaidid, bool? criminaloffence, bool? employmentdismissed, bool? disciplinaryguilty, bool? workedfinservicesindustry, DateTime? workedfinservicesindustryfrom, DateTime? workedfinservicesindustryto, DateTime? reglevel1examdate, string note, long? fkautomationstatusid, DateTime? automationdate, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, HRStaffQueries.Search(employeeno, tempstartdate, employmentstartdate, employmentenddate, fkcompanyid, fkstafftypeid, fkworkstatusid, fkjobtitleid, fkuserid, fkrecruitmentconsultantid, recruitmentsource, fktitleid, initials, firstname, preferredname, surname, fkgenderid, fkraceid, idno, dob, fkcitizenid, fkreligionid, telhome, telcell, telother, email, registeredtaxpayer, taxrefno, irp5number, fkmedicalaidid, criminaloffence, employmentdismissed, disciplinaryguilty, workedfinservicesindustry, workedfinservicesindustryfrom, workedfinservicesindustryto, reglevel1examdate, note, fkautomationstatusid, automationdate), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for HRStaff objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 hrstaff objects in the database.
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
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static HRStaff SearchOne(string employeeno, DateTime? tempstartdate, DateTime? employmentstartdate, DateTime? employmentenddate, long? fkcompanyid, long? fkstafftypeid, long? fkworkstatusid, long? fkjobtitleid, long? fkuserid, long? fkrecruitmentconsultantid, string recruitmentsource, long? fktitleid, string initials, string firstname, string preferredname, string surname, long? fkgenderid, long? fkraceid, string idno, DateTime? dob, long? fkcitizenid, long? fkreligionid, string telhome, string telcell, string telother, string email, bool? registeredtaxpayer, string taxrefno, string irp5number, long? fkmedicalaidid, bool? criminaloffence, bool? employmentdismissed, bool? disciplinaryguilty, bool? workedfinservicesindustry, DateTime? workedfinservicesindustryfrom, DateTime? workedfinservicesindustryto, DateTime? reglevel1examdate, string note, long? fkautomationstatusid, DateTime? automationdate, string connectionName)
        {
            HRStaff hrstaff = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, HRStaffQueries.Search(employeeno, tempstartdate, employmentstartdate, employmentenddate, fkcompanyid, fkstafftypeid, fkworkstatusid, fkjobtitleid, fkuserid, fkrecruitmentconsultantid, recruitmentsource, fktitleid, initials, firstname, preferredname, surname, fkgenderid, fkraceid, idno, dob, fkcitizenid, fkreligionid, telhome, telcell, telother, email, registeredtaxpayer, taxrefno, irp5number, fkmedicalaidid, criminaloffence, employmentdismissed, disciplinaryguilty, workedfinservicesindustry, workedfinservicesindustryfrom, workedfinservicesindustryto, reglevel1examdate, note, fkautomationstatusid, automationdate), null);
                if (reader.Read())
                {
                    hrstaff = new HRStaff((long)reader["ID"]);
                    hrstaff.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for HRStaff objects in the database", ex);
            }
            return hrstaff;
        }
        #endregion
    }
}
