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
using UDM.Insurance.Business.Objects;
using static UDM.Insurance.Business.Objects.CancerQuestion;

namespace UDM.Insurance.Business.Mapping
{
    /// <summary>
    /// Contains methods to fill, save and delete cancerquestion objects.
    /// </summary>
    class CancerQuestionMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) cancerquestion object from the database.
        /// </summary>
        /// <param name="cancerquestion">The id of the cancerquestion object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the cancerquestion object.</param>
        /// <returns>True if the cancerquestion object was deleted successfully, else false.</returns>
        internal static bool Delete(CancerQuestion cancerquestion)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(cancerquestion.ConnectionName, CancerQuestionQueries.Delete(cancerquestion, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a CancerQuestion object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) cancerquestion object from the database.
        /// </summary>
        /// <param name="cancerquestion">The id of the cancerquestion object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the cancerquestion history.</param>
        /// <returns>True if the cancerquestion history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(CancerQuestion cancerquestion)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(cancerquestion.ConnectionName, CancerQuestionQueries.DeleteHistory(cancerquestion, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete CancerQuestion history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) cancerquestion object from the database.
        /// </summary>
        /// <param name="cancerquestion">The id of the cancerquestion object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the cancerquestion object.</param>
        /// <returns>True if the cancerquestion object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(CancerQuestion cancerquestion)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(cancerquestion.ConnectionName, CancerQuestionQueries.UnDelete(cancerquestion, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a CancerQuestion object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the cancerquestion object from the data reader.
        /// </summary>
        /// <param name="cancerquestion">The cancerquestion object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(CancerQuestion cancerquestion, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    cancerquestion.IsLoaded = true;
                    cancerquestion.QuestionOne = reader["QuestionOne"] != DBNull.Value ? (bool)reader["QuestionOne"] : (bool?)null;
                    cancerquestion.QuestionTwo = reader["QuestionTwo"] != DBNull.Value ? (bool)reader["QuestionTwo"] : (bool?)null;
                    cancerquestion.FKINImportID = reader["FKINImportID"] != DBNull.Value ? (long)reader["FKINImportID"] : (long?)null;
                    cancerquestion.FKINCampaignID = reader["FKINCampaignID"] != DBNull.Value ? (long)reader["FKINCampaignID"] : (long?)null;
                    cancerquestion.StampDate = (DateTime)reader["StampDate"];
                    cancerquestion.HasChanged = false;
                }
                else
                {
                    throw new MapperException("CancerQuestion does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a CancerQuestion object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an cancerquestion object from the database.
        /// </summary>
        /// <param name="cancerquestion">The cancerquestion to fill.</param>
        internal static void Fill(CancerQuestion cancerquestion)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(cancerquestion.ConnectionName, CancerQuestionQueries.Fill(cancerquestion, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(cancerquestion, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a CancerQuestion object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a cancerquestion object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(CancerQuestion cancerquestion)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(cancerquestion.ConnectionName, CancerQuestionQueries.FillData(cancerquestion, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a CancerQuestion object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an cancerquestion object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="cancerquestion">The cancerquestion to fill from history.</param>
        internal static void FillHistory(CancerQuestion cancerquestion, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(cancerquestion.ConnectionName, CancerQuestionQueries.FillHistory(cancerquestion, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(cancerquestion, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a CancerQuestion object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the cancerquestion objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static CancerQuestionCollection List(bool showDeleted, string connectionName)
        {
            CancerQuestionCollection collection = new CancerQuestionCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? CancerQuestionQueries.ListDeleted() : CancerQuestionQueries.List(), null);
                while (reader.Read())
                {
                    CancerQuestion cancerquestion = new CancerQuestion((long)reader["ID"]);
                    cancerquestion.ConnectionName = connectionName;
                    collection.Add(cancerquestion);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list CancerQuestion objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the cancerquestion objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? CancerQuestionQueries.ListDeleted() : CancerQuestionQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list CancerQuestion objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) cancerquestion object from the database.
        /// </summary>
        /// <param name="cancerquestion">The cancerquestion to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(CancerQuestion cancerquestion)
        {
            CancerQuestionCollection collection = new CancerQuestionCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(cancerquestion.ConnectionName, CancerQuestionQueries.ListHistory(cancerquestion, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) CancerQuestion in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an cancerquestion object to the database.
        /// </summary>
        /// <param name="cancerquestion">The CancerQuestion object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the cancerquestion was saved successfull, otherwise, false.</returns>
        internal static bool Save(CancerQuestion cancerquestion)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (cancerquestion.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(cancerquestion.ConnectionName, CancerQuestionQueries.Save(cancerquestion, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(cancerquestion.ConnectionName, CancerQuestionQueries.Save(cancerquestion, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            cancerquestion.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= cancerquestion.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a CancerQuestion object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for cancerquestion objects in the database.
        /// </summary>
        /// <param name="questionone">The questionone search criteria.</param>
        /// <param name="questiontwo">The questiontwo search criteria.</param>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static CancerQuestionCollection Search(bool? questionone, bool? questiontwo, long? fkinimportid, long? fkincampaignid, string connectionName)
        {
            CancerQuestionCollection collection = new CancerQuestionCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, CancerQuestionQueries.Search(questionone, questiontwo, fkinimportid, fkincampaignid), null);
                while (reader.Read())
                {
                    CancerQuestion cancerquestion = new CancerQuestion((long)reader["ID"]);
                    cancerquestion.ConnectionName = connectionName;
                    collection.Add(cancerquestion);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for CancerQuestion objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for cancerquestion objects in the database.
        /// </summary>
        /// <param name="questionone">The questionone search criteria.</param>
        /// <param name="questiontwo">The questiontwo search criteria.</param>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(bool? questionone, bool? questiontwo, long? fkinimportid, long? fkincampaignid, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, CancerQuestionQueries.Search(questionone, questiontwo, fkinimportid, fkincampaignid), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for CancerQuestion objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 cancerquestion objects in the database.
        /// </summary>
        /// <param name="questionone">The questionone search criteria.</param>
        /// <param name="questiontwo">The questiontwo search criteria.</param>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static CancerQuestion SearchOne(bool? questionone, bool? questiontwo, long? fkinimportid, long? fkincampaignid, string connectionName)
        {
            CancerQuestion cancerquestion = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, CancerQuestionQueries.Search(questionone, questiontwo, fkinimportid, fkincampaignid), null);
                if (reader.Read())
                {
                    cancerquestion = new CancerQuestion((long)reader["ID"]);
                    cancerquestion.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for CancerQuestion objects in the database", ex);
            }
            return cancerquestion;
        }
        #endregion
    }
}
