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
    /// Contains methods to fill, save and delete inqadetailsquestion objects.
    /// </summary>
    public partial class INQADetailsQuestionMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inqadetailsquestion object from the database.
        /// </summary>
        /// <param name="inqadetailsquestion">The id of the inqadetailsquestion object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inqadetailsquestion object.</param>
        /// <returns>True if the inqadetailsquestion object was deleted successfully, else false.</returns>
        internal static bool Delete(INQADetailsQuestion inqadetailsquestion)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inqadetailsquestion.ConnectionName, INQADetailsQuestionQueries.Delete(inqadetailsquestion, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INQADetailsQuestion object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inqadetailsquestion object from the database.
        /// </summary>
        /// <param name="inqadetailsquestion">The id of the inqadetailsquestion object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inqadetailsquestion history.</param>
        /// <returns>True if the inqadetailsquestion history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INQADetailsQuestion inqadetailsquestion)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inqadetailsquestion.ConnectionName, INQADetailsQuestionQueries.DeleteHistory(inqadetailsquestion, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INQADetailsQuestion history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inqadetailsquestion object from the database.
        /// </summary>
        /// <param name="inqadetailsquestion">The id of the inqadetailsquestion object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inqadetailsquestion object.</param>
        /// <returns>True if the inqadetailsquestion object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INQADetailsQuestion inqadetailsquestion)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inqadetailsquestion.ConnectionName, INQADetailsQuestionQueries.UnDelete(inqadetailsquestion, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INQADetailsQuestion object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inqadetailsquestion object from the data reader.
        /// </summary>
        /// <param name="inqadetailsquestion">The inqadetailsquestion object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INQADetailsQuestion inqadetailsquestion, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inqadetailsquestion.IsLoaded = true;
                    inqadetailsquestion.Question = reader["Question"] != DBNull.Value ? (string)reader["Question"] : (string)null;
                    inqadetailsquestion.FKQuestionTypeID = reader["FKQuestionTypeID"] != DBNull.Value ? (long)reader["FKQuestionTypeID"] : (long?)null;
                    inqadetailsquestion.FKAnswerTypeID = reader["FKAnswerTypeID"] != DBNull.Value ? (long)reader["FKAnswerTypeID"] : (long?)null;
                    inqadetailsquestion.FKCampaignTypeID = reader["FKCampaignTypeID"] != DBNull.Value ? (long)reader["FKCampaignTypeID"] : (long?)null;
                    inqadetailsquestion.FKCampaignGroupID = reader["FKCampaignGroupID"] != DBNull.Value ? (long)reader["FKCampaignGroupID"] : (long?)null;
                    inqadetailsquestion.FKCampaignTypeGroupID = reader["FKCampaignTypeGroupID"] != DBNull.Value ? (long)reader["FKCampaignTypeGroupID"] : (long?)null;
                    inqadetailsquestion.FKCampaignGroupTypeID = reader["FKCampaignGroupTypeID"] != DBNull.Value ? (long)reader["FKCampaignGroupTypeID"] : (long?)null;
                    inqadetailsquestion.FKCampaignID = reader["FKCampaignID"] != DBNull.Value ? (long)reader["FKCampaignID"] : (long?)null;
                    inqadetailsquestion.Weight = reader["Weight"] != DBNull.Value ? (int)reader["Weight"] : (int?)null;
                    inqadetailsquestion.Rank = reader["Rank"] != DBNull.Value ? (int)reader["Rank"] : (int?)null;
                    inqadetailsquestion.IsActive = reader["IsActive"] != DBNull.Value ? (bool)reader["IsActive"] : (bool?)null;
                    inqadetailsquestion.StampDate = (DateTime)reader["StampDate"];
                    inqadetailsquestion.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INQADetailsQuestion does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INQADetailsQuestion object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inqadetailsquestion object from the database.
        /// </summary>
        /// <param name="inqadetailsquestion">The inqadetailsquestion to fill.</param>
        internal static void Fill(INQADetailsQuestion inqadetailsquestion)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inqadetailsquestion.ConnectionName, INQADetailsQuestionQueries.Fill(inqadetailsquestion, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inqadetailsquestion, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INQADetailsQuestion object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inqadetailsquestion object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INQADetailsQuestion inqadetailsquestion)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inqadetailsquestion.ConnectionName, INQADetailsQuestionQueries.FillData(inqadetailsquestion, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INQADetailsQuestion object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inqadetailsquestion object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inqadetailsquestion">The inqadetailsquestion to fill from history.</param>
        internal static void FillHistory(INQADetailsQuestion inqadetailsquestion, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inqadetailsquestion.ConnectionName, INQADetailsQuestionQueries.FillHistory(inqadetailsquestion, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inqadetailsquestion, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INQADetailsQuestion object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inqadetailsquestion objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INQADetailsQuestionCollection List(bool showDeleted, string connectionName)
        {
            INQADetailsQuestionCollection collection = new INQADetailsQuestionCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INQADetailsQuestionQueries.ListDeleted() : INQADetailsQuestionQueries.List(), null);
                while (reader.Read())
                {
                    INQADetailsQuestion inqadetailsquestion = new INQADetailsQuestion((long)reader["ID"]);
                    inqadetailsquestion.ConnectionName = connectionName;
                    collection.Add(inqadetailsquestion);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INQADetailsQuestion objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inqadetailsquestion objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INQADetailsQuestionQueries.ListDeleted() : INQADetailsQuestionQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INQADetailsQuestion objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inqadetailsquestion object from the database.
        /// </summary>
        /// <param name="inqadetailsquestion">The inqadetailsquestion to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INQADetailsQuestion inqadetailsquestion)
        {
            INQADetailsQuestionCollection collection = new INQADetailsQuestionCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inqadetailsquestion.ConnectionName, INQADetailsQuestionQueries.ListHistory(inqadetailsquestion, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INQADetailsQuestion in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inqadetailsquestion object to the database.
        /// </summary>
        /// <param name="inqadetailsquestion">The INQADetailsQuestion object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inqadetailsquestion was saved successfull, otherwise, false.</returns>
        internal static bool Save(INQADetailsQuestion inqadetailsquestion)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inqadetailsquestion.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inqadetailsquestion.ConnectionName, INQADetailsQuestionQueries.Save(inqadetailsquestion, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inqadetailsquestion.ConnectionName, INQADetailsQuestionQueries.Save(inqadetailsquestion, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inqadetailsquestion.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inqadetailsquestion.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INQADetailsQuestion object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inqadetailsquestion objects in the database.
        /// </summary>
        /// <param name="question">The question search criteria.</param>
        /// <param name="fkquestiontypeid">The fkquestiontypeid search criteria.</param>
        /// <param name="fkanswertypeid">The fkanswertypeid search criteria.</param>
        /// <param name="fkcampaigntypeid">The fkcampaigntypeid search criteria.</param>
        /// <param name="fkcampaigngroupid">The fkcampaigngroupid search criteria.</param>
        /// <param name="fkcampaigntypegroupid">The fkcampaigntypegroupid search criteria.</param>
        /// <param name="fkcampaigngrouptypeid">The fkcampaigngrouptypeid search criteria.</param>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="weight">The weight search criteria.</param>
        /// <param name="rank">The rank search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INQADetailsQuestionCollection Search(string question, long? fkquestiontypeid, long? fkanswertypeid, long? fkcampaigntypeid, long? fkcampaigngroupid, long? fkcampaigntypegroupid, long? fkcampaigngrouptypeid, long? fkcampaignid, int? weight, int? rank, bool? isactive, string connectionName)
        {
            INQADetailsQuestionCollection collection = new INQADetailsQuestionCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INQADetailsQuestionQueries.Search(question, fkquestiontypeid, fkanswertypeid, fkcampaigntypeid, fkcampaigngroupid, fkcampaigntypegroupid, fkcampaigngrouptypeid, fkcampaignid, weight, rank, isactive), null);
                while (reader.Read())
                {
                    INQADetailsQuestion inqadetailsquestion = new INQADetailsQuestion((long)reader["ID"]);
                    inqadetailsquestion.ConnectionName = connectionName;
                    collection.Add(inqadetailsquestion);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INQADetailsQuestion objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inqadetailsquestion objects in the database.
        /// </summary>
        /// <param name="question">The question search criteria.</param>
        /// <param name="fkquestiontypeid">The fkquestiontypeid search criteria.</param>
        /// <param name="fkanswertypeid">The fkanswertypeid search criteria.</param>
        /// <param name="fkcampaigntypeid">The fkcampaigntypeid search criteria.</param>
        /// <param name="fkcampaigngroupid">The fkcampaigngroupid search criteria.</param>
        /// <param name="fkcampaigntypegroupid">The fkcampaigntypegroupid search criteria.</param>
        /// <param name="fkcampaigngrouptypeid">The fkcampaigngrouptypeid search criteria.</param>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="weight">The weight search criteria.</param>
        /// <param name="rank">The rank search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(string question, long? fkquestiontypeid, long? fkanswertypeid, long? fkcampaigntypeid, long? fkcampaigngroupid, long? fkcampaigntypegroupid, long? fkcampaigngrouptypeid, long? fkcampaignid, int? weight, int? rank, bool? isactive, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INQADetailsQuestionQueries.Search(question, fkquestiontypeid, fkanswertypeid, fkcampaigntypeid, fkcampaigngroupid, fkcampaigntypegroupid, fkcampaigngrouptypeid, fkcampaignid, weight, rank, isactive), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INQADetailsQuestion objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inqadetailsquestion objects in the database.
        /// </summary>
        /// <param name="question">The question search criteria.</param>
        /// <param name="fkquestiontypeid">The fkquestiontypeid search criteria.</param>
        /// <param name="fkanswertypeid">The fkanswertypeid search criteria.</param>
        /// <param name="fkcampaigntypeid">The fkcampaigntypeid search criteria.</param>
        /// <param name="fkcampaigngroupid">The fkcampaigngroupid search criteria.</param>
        /// <param name="fkcampaigntypegroupid">The fkcampaigntypegroupid search criteria.</param>
        /// <param name="fkcampaigngrouptypeid">The fkcampaigngrouptypeid search criteria.</param>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="weight">The weight search criteria.</param>
        /// <param name="rank">The rank search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INQADetailsQuestion SearchOne(string question, long? fkquestiontypeid, long? fkanswertypeid, long? fkcampaigntypeid, long? fkcampaigngroupid, long? fkcampaigntypegroupid, long? fkcampaigngrouptypeid, long? fkcampaignid, int? weight, int? rank, bool? isactive, string connectionName)
        {
            INQADetailsQuestion inqadetailsquestion = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INQADetailsQuestionQueries.Search(question, fkquestiontypeid, fkanswertypeid, fkcampaigntypeid, fkcampaigngroupid, fkcampaigntypegroupid, fkcampaigngrouptypeid, fkcampaignid, weight, rank, isactive), null);
                if (reader.Read())
                {
                    inqadetailsquestion = new INQADetailsQuestion((long)reader["ID"]);
                    inqadetailsquestion.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INQADetailsQuestion objects in the database", ex);
            }
            return inqadetailsquestion;
        }
        #endregion
    }
}
