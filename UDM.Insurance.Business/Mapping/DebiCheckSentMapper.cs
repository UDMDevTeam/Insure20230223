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
    public partial class DebiCheckSentMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) sms object from the database.
        /// </summary>
        /// <param name="debichecksent">The id of the sms object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the sms object.</param>
        /// <returns>True if the sms object was deleted successfully, else false.</returns>
        internal static bool Delete(DebiCheckSent debicheckmapper)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(debicheckmapper.ConnectionName, DebiCheckSentQueries.Delete(debicheckmapper, ref parameters), parameters);
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
        internal static bool DeleteHistory(DebiCheckSent debichecksent)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(debichecksent.ConnectionName, DebiCheckSentQueries.DeleteHistory(debichecksent, ref parameters), parameters);
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
        internal static bool UnDelete(DebiCheckSent debichecksent)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(debichecksent.ConnectionName, DebiCheckSentQueries.UnDelete(debichecksent, ref parameters), parameters);
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
        private static void Fill(DebiCheckSent debichecksent, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    debichecksent.IsLoaded = true;
                    debichecksent.FKSystemID = reader["FKSystemID"] != DBNull.Value ? (long)reader["FKSystemID"] : (long?)null;
                    debichecksent.FKImportID = reader["FKImportID"] != DBNull.Value ? (long)reader["FKImportID"] : (long?)null;
                    debichecksent.SMSID = reader["SMSID"] != DBNull.Value ? (string)reader["SMSID"] : (string)null;
                    debichecksent.FKlkpSMSTypeID = reader["FKlkpSMSTypeID"] != DBNull.Value ? (long)reader["FKlkpSMSTypeID"] : (long?)null;
                    debichecksent.RecipientCellNum = reader["RecipientCellNum"] != DBNull.Value ? (string)reader["RecipientCellNum"] : (string)null;
                    debichecksent.SMSBody = reader["SMSBody"] != DBNull.Value ? (string)reader["SMSBody"] : (string)null;
                    debichecksent.FKlkpSMSEncodingID = reader["FKlkpSMSEncodingID"] != DBNull.Value ? (long)reader["FKlkpSMSEncodingID"] : (long?)null;
                    debichecksent.SubmissionID = reader["SubmissionID"] != DBNull.Value ? (string)reader["SubmissionID"] : (string)null;
                    debichecksent.SubmissionDate = reader["SubmissionDate"] != DBNull.Value ? (DateTime)reader["SubmissionDate"] : (DateTime?)null;
                    debichecksent.FKlkpSMSStatusTypeID = reader["FKlkpSMSStatusTypeID"] != DBNull.Value ? (long)reader["FKlkpSMSStatusTypeID"] : (long?)null;
                    debichecksent.FKlkpSMSStatusSubtypeID = reader["FKlkpSMSStatusSubtypeID"] != DBNull.Value ? (long)reader["FKlkpSMSStatusSubtypeID"] : (long?)null;
                    debichecksent.StampDate = (DateTime)reader["StampDate"];
                    debichecksent.HasChanged = false;
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
        internal static void Fill(DebiCheckSent debichecksent)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(debichecksent.ConnectionName, DebiCheckSentQueries.Fill(debichecksent, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(debichecksent, reader);
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
        public static DataSet FillData(DebiCheckSent debichecksent)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(debichecksent.ConnectionName, DebiCheckSentQueries.FillData(debichecksent, ref parameters), parameters);
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
        internal static void FillHistory(DebiCheckSent debichecksent, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(debichecksent.ConnectionName, DebiCheckSentQueries.FillHistory(debichecksent, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(debichecksent, reader);
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
        public static DebiCheckSentCollection List(bool showDeleted, string connectionName)
        {
            DebiCheckSentCollection collection = new DebiCheckSentCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? DebiCheckSentQueries.ListDeleted() : DebiCheckSentQueries.List(), null);
                while (reader.Read())
                {
                    DebiCheckSent debichecksent = new DebiCheckSent((long)reader["ID"]);
                    debichecksent.ConnectionName = connectionName;
                    collection.Add(debichecksent);
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
                return Database.ExecuteDataSet(connectionName, showDeleted ? DebiCheckSentQueries.ListDeleted() : DebiCheckSentQueries.List(), null);
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
        public static DataSet ListHistory(DebiCheckSent debichecksent)
        {
            SMSCollection collection = new SMSCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(debichecksent.ConnectionName, DebiCheckSentQueries.ListHistory(debichecksent, ref parameters), parameters);
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
        internal static bool Save(DebiCheckSent debichecksent)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (debichecksent.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(debichecksent.ConnectionName, DebiCheckSentQueries.Save(debichecksent, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(debichecksent.ConnectionName, DebiCheckSentQueries.Save(debichecksent, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            debichecksent.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= debichecksent.ID != 0;
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
        public static DebiCheckSentCollection Search(long? fksystemid, long? fkimportid, string smsid, long? fklkpsmstypeid, string recipientcellnum, string smsbody, long? fklkpsmsencodingid, string submissionid, DateTime? submissiondate, long? fklkpsmsstatustypeid, long? fklkpsmsstatussubtypeid, string connectionName)
        {
            DebiCheckSentCollection collection = new DebiCheckSentCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, DebiCheckSentQueries.Search(fksystemid, fkimportid, smsid, fklkpsmstypeid, recipientcellnum, smsbody, fklkpsmsencodingid, submissionid, submissiondate, fklkpsmsstatustypeid, fklkpsmsstatussubtypeid), null);
                while (reader.Read())
                {
                    DebiCheckSent debichecksent = new DebiCheckSent((long)reader["ID"]);
                    debichecksent.ConnectionName = connectionName;
                    collection.Add(debichecksent);
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
                return Database.ExecuteDataSet(connectionName, DebiCheckSentQueries.Search(fksystemid, fkimportid, smsid, fklkpsmstypeid, recipientcellnum, smsbody, fklkpsmsencodingid, submissionid, submissiondate, fklkpsmsstatustypeid, fklkpsmsstatussubtypeid), null);
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
        public static DebiCheckSent SearchOne(long? fksystemid, long? fkimportid, string smsid, long? fklkpsmstypeid, string recipientcellnum, string smsbody, long? fklkpsmsencodingid, string submissionid, DateTime? submissiondate, long? fklkpsmsstatustypeid, long? fklkpsmsstatussubtypeid, string connectionName)
        {
            DebiCheckSent debichecksent = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, DebiCheckSentQueries.Search(fksystemid, fkimportid, smsid, fklkpsmstypeid, recipientcellnum, smsbody, fklkpsmsencodingid, submissionid, submissiondate, fklkpsmsstatustypeid, fklkpsmsstatussubtypeid), null);
                if (reader.Read())
                {
                    debichecksent = new DebiCheckSent((long)reader["ID"]);
                    debichecksent.ConnectionName = connectionName;
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
            return debichecksent;
        }
        #endregion
    }
}
