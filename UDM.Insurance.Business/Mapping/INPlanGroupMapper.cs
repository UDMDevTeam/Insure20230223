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
    /// Contains methods to fill, save and delete inplangroup objects.
    /// </summary>
    public partial class INPlanGroupMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inplangroup object from the database.
        /// </summary>
        /// <param name="inplangroup">The id of the inplangroup object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inplangroup object.</param>
        /// <returns>True if the inplangroup object was deleted successfully, else false.</returns>
        internal static bool Delete(INPlanGroup inplangroup)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inplangroup.ConnectionName, INPlanGroupQueries.Delete(inplangroup, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INPlanGroup object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inplangroup object from the database.
        /// </summary>
        /// <param name="inplangroup">The id of the inplangroup object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inplangroup history.</param>
        /// <returns>True if the inplangroup history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INPlanGroup inplangroup)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inplangroup.ConnectionName, INPlanGroupQueries.DeleteHistory(inplangroup, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INPlanGroup history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inplangroup object from the database.
        /// </summary>
        /// <param name="inplangroup">The id of the inplangroup object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inplangroup object.</param>
        /// <returns>True if the inplangroup object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INPlanGroup inplangroup)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inplangroup.ConnectionName, INPlanGroupQueries.UnDelete(inplangroup, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INPlanGroup object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inplangroup object from the data reader.
        /// </summary>
        /// <param name="inplangroup">The inplangroup object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INPlanGroup inplangroup, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inplangroup.IsLoaded = true;
                    inplangroup.Description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : (string)null;
                    inplangroup.FKINCampaignTypeID = reader["FKINCampaignTypeID"] != DBNull.Value ? (long)reader["FKINCampaignTypeID"] : (long?)null;
                    inplangroup.FKINCampaignGroupID = reader["FKINCampaignGroupID"] != DBNull.Value ? (long)reader["FKINCampaignGroupID"] : (long?)null;
                    inplangroup.Date = reader["Date"] != DBNull.Value ? (DateTime)reader["Date"] : (DateTime?)null;
                    inplangroup.PolicyFee = reader["PolicyFee"] != DBNull.Value ? (decimal)reader["PolicyFee"] : (decimal?)null;
                    inplangroup.IsActive = reader["IsActive"] != DBNull.Value ? (bool)reader["IsActive"] : (bool?)null;
                    inplangroup.StampDate = (DateTime)reader["StampDate"];
                    inplangroup.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INPlanGroup does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPlanGroup object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inplangroup object from the database.
        /// </summary>
        /// <param name="inplangroup">The inplangroup to fill.</param>
        internal static void Fill(INPlanGroup inplangroup)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inplangroup.ConnectionName, INPlanGroupQueries.Fill(inplangroup, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inplangroup, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPlanGroup object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inplangroup object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INPlanGroup inplangroup)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inplangroup.ConnectionName, INPlanGroupQueries.FillData(inplangroup, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INPlanGroup object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inplangroup object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inplangroup">The inplangroup to fill from history.</param>
        internal static void FillHistory(INPlanGroup inplangroup, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inplangroup.ConnectionName, INPlanGroupQueries.FillHistory(inplangroup, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inplangroup, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPlanGroup object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inplangroup objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INPlanGroupCollection List(bool showDeleted, string connectionName)
        {
            INPlanGroupCollection collection = new INPlanGroupCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INPlanGroupQueries.ListDeleted() : INPlanGroupQueries.List(), null);
                while (reader.Read())
                {
                    INPlanGroup inplangroup = new INPlanGroup((long)reader["ID"]);
                    inplangroup.ConnectionName = connectionName;
                    collection.Add(inplangroup);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INPlanGroup objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inplangroup objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INPlanGroupQueries.ListDeleted() : INPlanGroupQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INPlanGroup objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inplangroup object from the database.
        /// </summary>
        /// <param name="inplangroup">The inplangroup to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INPlanGroup inplangroup)
        {
            INPlanGroupCollection collection = new INPlanGroupCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inplangroup.ConnectionName, INPlanGroupQueries.ListHistory(inplangroup, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INPlanGroup in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inplangroup object to the database.
        /// </summary>
        /// <param name="inplangroup">The INPlanGroup object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inplangroup was saved successfull, otherwise, false.</returns>
        internal static bool Save(INPlanGroup inplangroup)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inplangroup.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inplangroup.ConnectionName, INPlanGroupQueries.Save(inplangroup, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inplangroup.ConnectionName, INPlanGroupQueries.Save(inplangroup, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inplangroup.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inplangroup.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INPlanGroup object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inplangroup objects in the database.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <param name="fkincampaigntypeid">The fkincampaigntypeid search criteria.</param>
        /// <param name="fkincampaigngroupid">The fkincampaigngroupid search criteria.</param>
        /// <param name="date">The date search criteria.</param>
        /// <param name="policyfee">The policyfee search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INPlanGroupCollection Search(string description, long? fkincampaigntypeid, long? fkincampaigngroupid, DateTime? date, decimal? policyfee, bool? isactive, string connectionName)
        {
            INPlanGroupCollection collection = new INPlanGroupCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INPlanGroupQueries.Search(description, fkincampaigntypeid, fkincampaigngroupid, date, policyfee, isactive), null);
                while (reader.Read())
                {
                    INPlanGroup inplangroup = new INPlanGroup((long)reader["ID"]);
                    inplangroup.ConnectionName = connectionName;
                    collection.Add(inplangroup);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPlanGroup objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inplangroup objects in the database.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <param name="fkincampaigntypeid">The fkincampaigntypeid search criteria.</param>
        /// <param name="fkincampaigngroupid">The fkincampaigngroupid search criteria.</param>
        /// <param name="date">The date search criteria.</param>
        /// <param name="policyfee">The policyfee search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(string description, long? fkincampaigntypeid, long? fkincampaigngroupid, DateTime? date, decimal? policyfee, bool? isactive, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INPlanGroupQueries.Search(description, fkincampaigntypeid, fkincampaigngroupid, date, policyfee, isactive), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPlanGroup objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inplangroup objects in the database.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <param name="fkincampaigntypeid">The fkincampaigntypeid search criteria.</param>
        /// <param name="fkincampaigngroupid">The fkincampaigngroupid search criteria.</param>
        /// <param name="date">The date search criteria.</param>
        /// <param name="policyfee">The policyfee search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INPlanGroup SearchOne(string description, long? fkincampaigntypeid, long? fkincampaigngroupid, DateTime? date, decimal? policyfee, bool? isactive, string connectionName)
        {
            INPlanGroup inplangroup = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INPlanGroupQueries.Search(description, fkincampaigntypeid, fkincampaigngroupid, date, policyfee, isactive), null);
                if (reader.Read())
                {
                    inplangroup = new INPlanGroup((long)reader["ID"]);
                    inplangroup.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPlanGroup objects in the database", ex);
            }
            return inplangroup;
        }
        #endregion
    }
}
