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
    /// Contains methods to fill, save and delete sms objects.
    /// </summary>
    public partial class SMSMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) sms object from the database.
        /// </summary>
        /// <param name="sms">The id of the sms object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the sms object.</param>
        /// <returns>True if the sms object was deleted successfully, else false.</returns>
        internal static bool Delete(SMS sms)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(sms.ConnectionName, SMSQueries.Delete(sms, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a SMS object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) sms object from the database.
        /// </summary>
        /// <param name="sms">The id of the sms object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the sms history.</param>
        /// <returns>True if the sms history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(SMS sms)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(sms.ConnectionName, SMSQueries.DeleteHistory(sms, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete SMS history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) sms object from the database.
        /// </summary>
        /// <param name="sms">The id of the sms object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the sms object.</param>
        /// <returns>True if the sms object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(SMS sms)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(sms.ConnectionName, SMSQueries.UnDelete(sms, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a SMS object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the sms object from the data reader.
        /// </summary>
        /// <param name="sms">The sms object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(SMS sms, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    sms.IsLoaded = true;
                    sms.FKSystemID = reader["FKSystemID"] != DBNull.Value ? (long)reader["FKSystemID"] : (long?)null;
                    sms.FKImportID = reader["FKImportID"] != DBNull.Value ? (long)reader["FKImportID"] : (long?)null;
                    sms.SMSID = reader["SMSID"] != DBNull.Value ? (string)reader["SMSID"] : (string)null;
                    sms.FKlkpSMSTypeID = reader["FKlkpSMSTypeID"] != DBNull.Value ? (long)reader["FKlkpSMSTypeID"] : (long?)null;
                    sms.RecipientCellNum = reader["RecipientCellNum"] != DBNull.Value ? (string)reader["RecipientCellNum"] : (string)null;
                    sms.SMSBody = reader["SMSBody"] != DBNull.Value ? (string)reader["SMSBody"] : (string)null;
                    sms.FKlkpSMSEncodingID = reader["FKlkpSMSEncodingID"] != DBNull.Value ? (long)reader["FKlkpSMSEncodingID"] : (long?)null;
                    sms.SubmissionID = reader["SubmissionID"] != DBNull.Value ? (string)reader["SubmissionID"] : (string)null;
                    sms.SubmissionDate = reader["SubmissionDate"] != DBNull.Value ? (DateTime)reader["SubmissionDate"] : (DateTime?)null;
                    sms.FKlkpSMSStatusTypeID = reader["FKlkpSMSStatusTypeID"] != DBNull.Value ? (long)reader["FKlkpSMSStatusTypeID"] : (long?)null;
                    sms.FKlkpSMSStatusSubtypeID = reader["FKlkpSMSStatusSubtypeID"] != DBNull.Value ? (long)reader["FKlkpSMSStatusSubtypeID"] : (long?)null;
                    sms.StampDate = (DateTime)reader["StampDate"];
                    sms.HasChanged = false;
                }
                else
                {
                    throw new MapperException("SMS does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a SMS object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an sms object from the database.
        /// </summary>
        /// <param name="sms">The sms to fill.</param>
        internal static void Fill(SMS sms)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(sms.ConnectionName, SMSQueries.Fill(sms, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(sms, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a SMS object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a sms object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(SMS sms)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(sms.ConnectionName, SMSQueries.FillData(sms, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a SMS object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an sms object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="sms">The sms to fill from history.</param>
        internal static void FillHistory(SMS sms, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(sms.ConnectionName, SMSQueries.FillHistory(sms, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(sms, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a SMS object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the sms objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static SMSCollection List(bool showDeleted, string connectionName)
        {
            SMSCollection collection = new SMSCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? SMSQueries.ListDeleted() : SMSQueries.List(), null);
                while (reader.Read())
                {
                    SMS sms = new SMS((long)reader["ID"]);
                    sms.ConnectionName = connectionName;
                    collection.Add(sms);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list SMS objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the sms objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? SMSQueries.ListDeleted() : SMSQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list SMS objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) sms object from the database.
        /// </summary>
        /// <param name="sms">The sms to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(SMS sms)
        {
            SMSCollection collection = new SMSCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(sms.ConnectionName, SMSQueries.ListHistory(sms, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) SMS in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an sms object to the database.
        /// </summary>
        /// <param name="sms">The SMS object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the sms was saved successfull, otherwise, false.</returns>
        internal static bool Save(SMS sms)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (sms.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(sms.ConnectionName, SMSQueries.Save(sms, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(sms.ConnectionName, SMSQueries.Save(sms, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            sms.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= sms.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a SMS object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for sms objects in the database.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="smsid">The smsid search criteria.</param>
        /// <param name="fklkpsmstypeid">The fklkpsmstypeid search criteria.</param>
        /// <param name="recipientcellnum">The recipientcellnum search criteria.</param>
        /// <param name="smsbody">The smsbody search criteria.</param>
        /// <param name="fklkpsmsencodingid">The fklkpsmsencodingid search criteria.</param>
        /// <param name="submissionid">The submissionid search criteria.</param>
        /// <param name="submissiondate">The submissiondate search criteria.</param>
        /// <param name="fklkpsmsstatustypeid">The fklkpsmsstatustypeid search criteria.</param>
        /// <param name="fklkpsmsstatussubtypeid">The fklkpsmsstatussubtypeid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static SMSCollection Search(long? fksystemid, long? fkimportid, string smsid, long? fklkpsmstypeid, string recipientcellnum, string smsbody, long? fklkpsmsencodingid, string submissionid, DateTime? submissiondate, long? fklkpsmsstatustypeid, long? fklkpsmsstatussubtypeid, string connectionName)
        {
            SMSCollection collection = new SMSCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, SMSQueries.Search(fksystemid, fkimportid, smsid, fklkpsmstypeid, recipientcellnum, smsbody, fklkpsmsencodingid, submissionid, submissiondate, fklkpsmsstatustypeid, fklkpsmsstatussubtypeid), null);
                while (reader.Read())
                {
                    SMS sms = new SMS((long)reader["ID"]);
                    sms.ConnectionName = connectionName;
                    collection.Add(sms);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for SMS objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for sms objects in the database.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="smsid">The smsid search criteria.</param>
        /// <param name="fklkpsmstypeid">The fklkpsmstypeid search criteria.</param>
        /// <param name="recipientcellnum">The recipientcellnum search criteria.</param>
        /// <param name="smsbody">The smsbody search criteria.</param>
        /// <param name="fklkpsmsencodingid">The fklkpsmsencodingid search criteria.</param>
        /// <param name="submissionid">The submissionid search criteria.</param>
        /// <param name="submissiondate">The submissiondate search criteria.</param>
        /// <param name="fklkpsmsstatustypeid">The fklkpsmsstatustypeid search criteria.</param>
        /// <param name="fklkpsmsstatussubtypeid">The fklkpsmsstatussubtypeid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fksystemid, long? fkimportid, string smsid, long? fklkpsmstypeid, string recipientcellnum, string smsbody, long? fklkpsmsencodingid, string submissionid, DateTime? submissiondate, long? fklkpsmsstatustypeid, long? fklkpsmsstatussubtypeid, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, SMSQueries.Search(fksystemid, fkimportid, smsid, fklkpsmstypeid, recipientcellnum, smsbody, fklkpsmsencodingid, submissionid, submissiondate, fklkpsmsstatustypeid, fklkpsmsstatussubtypeid), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for SMS objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 sms objects in the database.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="smsid">The smsid search criteria.</param>
        /// <param name="fklkpsmstypeid">The fklkpsmstypeid search criteria.</param>
        /// <param name="recipientcellnum">The recipientcellnum search criteria.</param>
        /// <param name="smsbody">The smsbody search criteria.</param>
        /// <param name="fklkpsmsencodingid">The fklkpsmsencodingid search criteria.</param>
        /// <param name="submissionid">The submissionid search criteria.</param>
        /// <param name="submissiondate">The submissiondate search criteria.</param>
        /// <param name="fklkpsmsstatustypeid">The fklkpsmsstatustypeid search criteria.</param>
        /// <param name="fklkpsmsstatussubtypeid">The fklkpsmsstatussubtypeid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static SMS SearchOne(long? fksystemid, long? fkimportid, string smsid, long? fklkpsmstypeid, string recipientcellnum, string smsbody, long? fklkpsmsencodingid, string submissionid, DateTime? submissiondate, long? fklkpsmsstatustypeid, long? fklkpsmsstatussubtypeid, string connectionName)
        {
            SMS sms = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, SMSQueries.Search(fksystemid, fkimportid, smsid, fklkpsmstypeid, recipientcellnum, smsbody, fklkpsmsencodingid, submissionid, submissiondate, fklkpsmsstatustypeid, fklkpsmsstatussubtypeid), null);
                if (reader.Read())
                {
                    sms = new SMS((long)reader["ID"]);
                    sms.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for SMS objects in the database", ex);
            }
            return sms;
        }
        #endregion
    }
}
