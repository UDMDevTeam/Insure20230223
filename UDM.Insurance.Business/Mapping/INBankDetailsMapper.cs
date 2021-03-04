using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Collections;

using UDM.Insurance.Business;
using UDM.Insurance.Business.Queries;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using Embriant.Framework.Exceptions;
using Embriant.Framework;
using Embriant.Framework.Validation;

namespace UDM.Insurance.Business.Mapping
{
    /// <summary>
    /// Contains methods to fill, save and delete inbankdetails objects.
    /// </summary>
    public partial class INBankDetailsMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inbankdetails object from the database.
        /// </summary>
        /// <param name="inbankdetails">The id of the inbankdetails object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inbankdetails object.</param>
        /// <returns>True if the inbankdetails object was deleted successfully, else false.</returns>
        internal static bool Delete(INBankDetails inbankdetails)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inbankdetails.ConnectionName, INBankDetailsQueries.Delete(inbankdetails, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INBankDetails object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inbankdetails object from the database.
        /// </summary>
        /// <param name="inbankdetails">The id of the inbankdetails object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inbankdetails history.</param>
        /// <returns>True if the inbankdetails history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INBankDetails inbankdetails)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inbankdetails.ConnectionName, INBankDetailsQueries.DeleteHistory(inbankdetails, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INBankDetails history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inbankdetails object from the database.
        /// </summary>
        /// <param name="inbankdetails">The id of the inbankdetails object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inbankdetails object.</param>
        /// <returns>True if the inbankdetails object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INBankDetails inbankdetails)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inbankdetails.ConnectionName, INBankDetailsQueries.UnDelete(inbankdetails, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INBankDetails object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inbankdetails object from the data reader.
        /// </summary>
        /// <param name="inbankdetails">The inbankdetails object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INBankDetails inbankdetails, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inbankdetails.IsLoaded = true;
                    inbankdetails.FKPaymentMethodID = reader["FKPaymentMethodID"] != DBNull.Value ? (long)reader["FKPaymentMethodID"] : (long?)null;
                    inbankdetails.AccountHolder = reader["AccountHolder"] != DBNull.Value ? (string)reader["AccountHolder"] : (string)null;
                    inbankdetails.FKBankID = reader["FKBankID"] != DBNull.Value ? (long)reader["FKBankID"] : (long?)null;
                    inbankdetails.FKBankBranchID = reader["FKBankBranchID"] != DBNull.Value ? (long)reader["FKBankBranchID"] : (long?)null;
                    inbankdetails.AccountNo = reader["AccountNo"] != DBNull.Value ? (string)reader["AccountNo"] : (string)null;
                    inbankdetails.FKAccountTypeID = reader["FKAccountTypeID"] != DBNull.Value ? (long)reader["FKAccountTypeID"] : (long?)null;
                    inbankdetails.DebitDay = reader["DebitDay"] != DBNull.Value ? (short)reader["DebitDay"] : (short?)null;
                    inbankdetails.FKAHTitleID = reader["FKAHTitleID"] != DBNull.Value ? (long)reader["FKAHTitleID"] : (long?)null;
                    inbankdetails.AHInitials = reader["AHInitials"] != DBNull.Value ? (string)reader["AHInitials"] : (string)null;
                    inbankdetails.AHFirstName = reader["AHFirstName"] != DBNull.Value ? (string)reader["AHFirstName"] : (string)null;
                    inbankdetails.AHSurname = reader["AHSurname"] != DBNull.Value ? (string)reader["AHSurname"] : (string)null;
                    inbankdetails.AHIDNo = reader["AHIDNo"] != DBNull.Value ? (string)reader["AHIDNo"] : (string)null;
                    inbankdetails.AHTelHome = reader["AHTelHome"] != DBNull.Value ? (string)reader["AHTelHome"] : (string)null;
                    inbankdetails.AHTelCell = reader["AHTelCell"] != DBNull.Value ? (string)reader["AHTelCell"] : (string)null;
                    inbankdetails.AHTelWork = reader["AHTelWork"] != DBNull.Value ? (string)reader["AHTelWork"] : (string)null;
                    inbankdetails.ToDeleteID = reader["ToDeleteID"] != DBNull.Value ? (long)reader["ToDeleteID"] : (long?)null;
                    inbankdetails.FKSigningPowerID = reader["FKSigningPowerID"] != DBNull.Value ? (long)reader["FKSigningPowerID"] : (long?)null;
                    inbankdetails.AccNumCheckStatus = reader["AccNumCheckStatus"] != DBNull.Value ? (Byte)reader["AccNumCheckStatus"] : (Byte?)null;
                    inbankdetails.AccNumCheckMsg = reader["AccNumCheckMsg"] != DBNull.Value ? (string)reader["AccNumCheckMsg"] : (string)null;
                    inbankdetails.AccNumCheckMsgFull = reader["AccNumCheckMsgFull"] != DBNull.Value ? (string)reader["AccNumCheckMsgFull"] : (string)null;
                    inbankdetails.StampDate = (DateTime)reader["StampDate"];
                    inbankdetails.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INBankDetails does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INBankDetails object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inbankdetails object from the database.
        /// </summary>
        /// <param name="inbankdetails">The inbankdetails to fill.</param>
        internal static void Fill(INBankDetails inbankdetails)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inbankdetails.ConnectionName, INBankDetailsQueries.Fill(inbankdetails, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inbankdetails, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INBankDetails object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inbankdetails object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INBankDetails inbankdetails)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inbankdetails.ConnectionName, INBankDetailsQueries.FillData(inbankdetails, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INBankDetails object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inbankdetails object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inbankdetails">The inbankdetails to fill from history.</param>
        internal static void FillHistory(INBankDetails inbankdetails, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inbankdetails.ConnectionName, INBankDetailsQueries.FillHistory(inbankdetails, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inbankdetails, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INBankDetails object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inbankdetails objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INBankDetailsCollection List(bool showDeleted, string connectionName)
        {
            INBankDetailsCollection collection = new INBankDetailsCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INBankDetailsQueries.ListDeleted() : INBankDetailsQueries.List(), null);
                while (reader.Read())
                {
                    INBankDetails inbankdetails = new INBankDetails((long)reader["ID"]);
                    inbankdetails.ConnectionName = connectionName;
                    collection.Add(inbankdetails);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INBankDetails objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inbankdetails objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INBankDetailsQueries.ListDeleted() : INBankDetailsQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INBankDetails objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inbankdetails object from the database.
        /// </summary>
        /// <param name="inbankdetails">The inbankdetails to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INBankDetails inbankdetails)
        {
            INBankDetailsCollection collection = new INBankDetailsCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inbankdetails.ConnectionName, INBankDetailsQueries.ListHistory(inbankdetails, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INBankDetails in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inbankdetails object to the database.
        /// </summary>
        /// <param name="inbankdetails">The INBankDetails object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inbankdetails was saved successfull, otherwise, false.</returns>
        internal static bool Save(INBankDetails inbankdetails)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inbankdetails.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inbankdetails.ConnectionName, INBankDetailsQueries.Save(inbankdetails, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inbankdetails.ConnectionName, INBankDetailsQueries.Save(inbankdetails, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inbankdetails.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inbankdetails.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INBankDetails object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inbankdetails objects in the database.
        /// </summary>
        /// <param name="fkpaymentmethodid">The fkpaymentmethodid search criteria.</param>
        /// <param name="accountholder">The accountholder search criteria.</param>
        /// <param name="fkbankid">The fkbankid search criteria.</param>
        /// <param name="fkbankbranchid">The fkbankbranchid search criteria.</param>
        /// <param name="accountno">The accountno search criteria.</param>
        /// <param name="fkaccounttypeid">The fkaccounttypeid search criteria.</param>
        /// <param name="debitday">The debitday search criteria.</param>
        /// <param name="fkahtitleid">The fkahtitleid search criteria.</param>
        /// <param name="ahinitials">The ahinitials search criteria.</param>
        /// <param name="ahfirstname">The ahfirstname search criteria.</param>
        /// <param name="ahsurname">The ahsurname search criteria.</param>
        /// <param name="ahidno">The ahidno search criteria.</param>
        /// <param name="ahtelhome">The ahtelhome search criteria.</param>
        /// <param name="ahtelcell">The ahtelcell search criteria.</param>
        /// <param name="ahtelwork">The ahtelwork search criteria.</param>
        /// <param name="todeleteid">The todeleteid search criteria.</param>
        /// <param name="fksigningpowerid">The fksigningpowerid search criteria.</param>
        /// <param name="accnumcheckstatus">The accnumcheckstatus search criteria.</param>
        /// <param name="accnumcheckmsg">The accnumcheckmsg search criteria.</param>
        /// <param name="accnumcheckmsgfull">The accnumcheckmsgfull search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INBankDetailsCollection Search(long? fkpaymentmethodid, string accountholder, long? fkbankid, long? fkbankbranchid, string accountno, long? fkaccounttypeid, short? debitday, long? fkahtitleid, string ahinitials, string ahfirstname, string ahsurname, string ahidno, string ahtelhome, string ahtelcell, string ahtelwork, long? todeleteid, long? fksigningpowerid, Byte? accnumcheckstatus, string accnumcheckmsg, string accnumcheckmsgfull, string connectionName)
        {
            INBankDetailsCollection collection = new INBankDetailsCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INBankDetailsQueries.Search(fkpaymentmethodid, accountholder, fkbankid, fkbankbranchid, accountno, fkaccounttypeid, debitday, fkahtitleid, ahinitials, ahfirstname, ahsurname, ahidno, ahtelhome, ahtelcell, ahtelwork, todeleteid, fksigningpowerid, accnumcheckstatus, accnumcheckmsg, accnumcheckmsgfull), null);
                while (reader.Read())
                {
                    INBankDetails inbankdetails = new INBankDetails((long)reader["ID"]);
                    inbankdetails.ConnectionName = connectionName;
                    collection.Add(inbankdetails);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INBankDetails objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inbankdetails objects in the database.
        /// </summary>
        /// <param name="fkpaymentmethodid">The fkpaymentmethodid search criteria.</param>
        /// <param name="accountholder">The accountholder search criteria.</param>
        /// <param name="fkbankid">The fkbankid search criteria.</param>
        /// <param name="fkbankbranchid">The fkbankbranchid search criteria.</param>
        /// <param name="accountno">The accountno search criteria.</param>
        /// <param name="fkaccounttypeid">The fkaccounttypeid search criteria.</param>
        /// <param name="debitday">The debitday search criteria.</param>
        /// <param name="fkahtitleid">The fkahtitleid search criteria.</param>
        /// <param name="ahinitials">The ahinitials search criteria.</param>
        /// <param name="ahfirstname">The ahfirstname search criteria.</param>
        /// <param name="ahsurname">The ahsurname search criteria.</param>
        /// <param name="ahidno">The ahidno search criteria.</param>
        /// <param name="ahtelhome">The ahtelhome search criteria.</param>
        /// <param name="ahtelcell">The ahtelcell search criteria.</param>
        /// <param name="ahtelwork">The ahtelwork search criteria.</param>
        /// <param name="todeleteid">The todeleteid search criteria.</param>
        /// <param name="fksigningpowerid">The fksigningpowerid search criteria.</param>
        /// <param name="accnumcheckstatus">The accnumcheckstatus search criteria.</param>
        /// <param name="accnumcheckmsg">The accnumcheckmsg search criteria.</param>
        /// <param name="accnumcheckmsgfull">The accnumcheckmsgfull search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkpaymentmethodid, string accountholder, long? fkbankid, long? fkbankbranchid, string accountno, long? fkaccounttypeid, short? debitday, long? fkahtitleid, string ahinitials, string ahfirstname, string ahsurname, string ahidno, string ahtelhome, string ahtelcell, string ahtelwork, long? todeleteid, long? fksigningpowerid, Byte? accnumcheckstatus, string accnumcheckmsg, string accnumcheckmsgfull, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INBankDetailsQueries.Search(fkpaymentmethodid, accountholder, fkbankid, fkbankbranchid, accountno, fkaccounttypeid, debitday, fkahtitleid, ahinitials, ahfirstname, ahsurname, ahidno, ahtelhome, ahtelcell, ahtelwork, todeleteid, fksigningpowerid, accnumcheckstatus, accnumcheckmsg, accnumcheckmsgfull), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INBankDetails objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inbankdetails objects in the database.
        /// </summary>
        /// <param name="fkpaymentmethodid">The fkpaymentmethodid search criteria.</param>
        /// <param name="accountholder">The accountholder search criteria.</param>
        /// <param name="fkbankid">The fkbankid search criteria.</param>
        /// <param name="fkbankbranchid">The fkbankbranchid search criteria.</param>
        /// <param name="accountno">The accountno search criteria.</param>
        /// <param name="fkaccounttypeid">The fkaccounttypeid search criteria.</param>
        /// <param name="debitday">The debitday search criteria.</param>
        /// <param name="fkahtitleid">The fkahtitleid search criteria.</param>
        /// <param name="ahinitials">The ahinitials search criteria.</param>
        /// <param name="ahfirstname">The ahfirstname search criteria.</param>
        /// <param name="ahsurname">The ahsurname search criteria.</param>
        /// <param name="ahidno">The ahidno search criteria.</param>
        /// <param name="ahtelhome">The ahtelhome search criteria.</param>
        /// <param name="ahtelcell">The ahtelcell search criteria.</param>
        /// <param name="ahtelwork">The ahtelwork search criteria.</param>
        /// <param name="todeleteid">The todeleteid search criteria.</param>
        /// <param name="fksigningpowerid">The fksigningpowerid search criteria.</param>
        /// <param name="accnumcheckstatus">The accnumcheckstatus search criteria.</param>
        /// <param name="accnumcheckmsg">The accnumcheckmsg search criteria.</param>
        /// <param name="accnumcheckmsgfull">The accnumcheckmsgfull search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INBankDetails SearchOne(long? fkpaymentmethodid, string accountholder, long? fkbankid, long? fkbankbranchid, string accountno, long? fkaccounttypeid, short? debitday, long? fkahtitleid, string ahinitials, string ahfirstname, string ahsurname, string ahidno, string ahtelhome, string ahtelcell, string ahtelwork, long? todeleteid, long? fksigningpowerid, Byte? accnumcheckstatus, string accnumcheckmsg, string accnumcheckmsgfull, string connectionName)
        {
            INBankDetails inbankdetails = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INBankDetailsQueries.Search(fkpaymentmethodid, accountholder, fkbankid, fkbankbranchid, accountno, fkaccounttypeid, debitday, fkahtitleid, ahinitials, ahfirstname, ahsurname, ahidno, ahtelhome, ahtelcell, ahtelwork, todeleteid, fksigningpowerid, accnumcheckstatus, accnumcheckmsg, accnumcheckmsgfull), null);
                if (reader.Read())
                {
                    inbankdetails = new INBankDetails((long)reader["ID"]);
                    inbankdetails.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INBankDetails objects in the database", ex);
            }
            return inbankdetails;
        }
        #endregion
    }
}
