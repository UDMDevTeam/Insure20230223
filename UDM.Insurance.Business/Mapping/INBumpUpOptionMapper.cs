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
    /// Contains methods to fill, save and delete inbumpupoption objects.
    /// </summary>
    public partial class INBumpUpOptionMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inbumpupoption object from the database.
        /// </summary>
        /// <param name="inbumpupoption">The id of the inbumpupoption object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inbumpupoption object.</param>
        /// <returns>True if the inbumpupoption object was deleted successfully, else false.</returns>
        internal static bool Delete(INBumpUpOption inbumpupoption)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inbumpupoption.ConnectionName, INBumpUpOptionQueries.Delete(inbumpupoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INBumpUpOption object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inbumpupoption object from the database.
        /// </summary>
        /// <param name="inbumpupoption">The id of the inbumpupoption object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inbumpupoption history.</param>
        /// <returns>True if the inbumpupoption history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INBumpUpOption inbumpupoption)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inbumpupoption.ConnectionName, INBumpUpOptionQueries.DeleteHistory(inbumpupoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INBumpUpOption history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inbumpupoption object from the database.
        /// </summary>
        /// <param name="inbumpupoption">The id of the inbumpupoption object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inbumpupoption object.</param>
        /// <returns>True if the inbumpupoption object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INBumpUpOption inbumpupoption)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inbumpupoption.ConnectionName, INBumpUpOptionQueries.UnDelete(inbumpupoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INBumpUpOption object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inbumpupoption object from the data reader.
        /// </summary>
        /// <param name="inbumpupoption">The inbumpupoption object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INBumpUpOption inbumpupoption, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inbumpupoption.IsLoaded = true;
                    inbumpupoption.Description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : (string)null;
                    inbumpupoption.ImportCode = reader["ImportCode"] != DBNull.Value ? (string)reader["ImportCode"] : (string)null;
                    inbumpupoption.FKINCampaignTypeID = reader["FKINCampaignTypeID"] != DBNull.Value ? (long)reader["FKINCampaignTypeID"] : (long?)null;
                    inbumpupoption.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INBumpUpOption does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INBumpUpOption object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inbumpupoption object from the database.
        /// </summary>
        /// <param name="inbumpupoption">The inbumpupoption to fill.</param>
        internal static void Fill(INBumpUpOption inbumpupoption)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inbumpupoption.ConnectionName, INBumpUpOptionQueries.Fill(inbumpupoption, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inbumpupoption, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INBumpUpOption object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inbumpupoption object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INBumpUpOption inbumpupoption)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inbumpupoption.ConnectionName, INBumpUpOptionQueries.FillData(inbumpupoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INBumpUpOption object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inbumpupoption object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inbumpupoption">The inbumpupoption to fill from history.</param>
        internal static void FillHistory(INBumpUpOption inbumpupoption, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inbumpupoption.ConnectionName, INBumpUpOptionQueries.FillHistory(inbumpupoption, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inbumpupoption, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INBumpUpOption object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inbumpupoption objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INBumpUpOptionCollection List(bool showDeleted, string connectionName)
        {
            INBumpUpOptionCollection collection = new INBumpUpOptionCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INBumpUpOptionQueries.ListDeleted() : INBumpUpOptionQueries.List(), null);
                while (reader.Read())
                {
                    INBumpUpOption inbumpupoption = new INBumpUpOption((long)reader["ID"]);
                    inbumpupoption.ConnectionName = connectionName;
                    collection.Add(inbumpupoption);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INBumpUpOption objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inbumpupoption objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INBumpUpOptionQueries.ListDeleted() : INBumpUpOptionQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INBumpUpOption objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inbumpupoption object from the database.
        /// </summary>
        /// <param name="inbumpupoption">The inbumpupoption to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INBumpUpOption inbumpupoption)
        {
            INBumpUpOptionCollection collection = new INBumpUpOptionCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inbumpupoption.ConnectionName, INBumpUpOptionQueries.ListHistory(inbumpupoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INBumpUpOption in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inbumpupoption object to the database.
        /// </summary>
        /// <param name="inbumpupoption">The INBumpUpOption object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inbumpupoption was saved successfull, otherwise, false.</returns>
        internal static bool Save(INBumpUpOption inbumpupoption)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inbumpupoption.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inbumpupoption.ConnectionName, INBumpUpOptionQueries.Save(inbumpupoption, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inbumpupoption.ConnectionName, INBumpUpOptionQueries.Save(inbumpupoption, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inbumpupoption.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inbumpupoption.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INBumpUpOption object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inbumpupoption objects in the database.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <param name="importcode">The importcode search criteria.</param>
        /// <param name="fkincampaigntypeid">The fkincampaigntypeid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INBumpUpOptionCollection Search(string description, string importcode, long? fkincampaigntypeid, string connectionName)
        {
            INBumpUpOptionCollection collection = new INBumpUpOptionCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INBumpUpOptionQueries.Search(description, importcode, fkincampaigntypeid), null);
                while (reader.Read())
                {
                    INBumpUpOption inbumpupoption = new INBumpUpOption((long)reader["ID"]);
                    inbumpupoption.ConnectionName = connectionName;
                    collection.Add(inbumpupoption);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INBumpUpOption objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inbumpupoption objects in the database.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <param name="importcode">The importcode search criteria.</param>
        /// <param name="fkincampaigntypeid">The fkincampaigntypeid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(string description, string importcode, long? fkincampaigntypeid, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INBumpUpOptionQueries.Search(description, importcode, fkincampaigntypeid), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INBumpUpOption objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inbumpupoption objects in the database.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <param name="importcode">The importcode search criteria.</param>
        /// <param name="fkincampaigntypeid">The fkincampaigntypeid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INBumpUpOption SearchOne(string description, string importcode, long? fkincampaigntypeid, string connectionName)
        {
            INBumpUpOption inbumpupoption = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INBumpUpOptionQueries.Search(description, importcode, fkincampaigntypeid), null);
                if (reader.Read())
                {
                    inbumpupoption = new INBumpUpOption((long)reader["ID"]);
                    inbumpupoption.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INBumpUpOption objects in the database", ex);
            }
            return inbumpupoption;
        }
        #endregion
    }
}
