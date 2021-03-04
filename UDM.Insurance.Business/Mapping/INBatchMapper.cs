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
    /// Contains methods to fill, save and delete inbatch objects.
    /// </summary>
    public partial class INBatchMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inbatch object from the database.
        /// </summary>
        /// <param name="inbatch">The id of the inbatch object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inbatch object.</param>
        /// <returns>True if the inbatch object was deleted successfully, else false.</returns>
        internal static bool Delete(INBatch inbatch)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inbatch.ConnectionName, INBatchQueries.Delete(inbatch, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INBatch object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inbatch object from the database.
        /// </summary>
        /// <param name="inbatch">The id of the inbatch object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inbatch history.</param>
        /// <returns>True if the inbatch history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INBatch inbatch)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inbatch.ConnectionName, INBatchQueries.DeleteHistory(inbatch, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INBatch history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inbatch object from the database.
        /// </summary>
        /// <param name="inbatch">The id of the inbatch object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inbatch object.</param>
        /// <returns>True if the inbatch object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INBatch inbatch)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inbatch.ConnectionName, INBatchQueries.UnDelete(inbatch, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INBatch object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inbatch object from the data reader.
        /// </summary>
        /// <param name="inbatch">The inbatch object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INBatch inbatch, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inbatch.IsLoaded = true;
                    inbatch.FKINCampaignID = reader["FKINCampaignID"] != DBNull.Value ? (long)reader["FKINCampaignID"] : (long?)null;
                    inbatch.Code = reader["Code"] != DBNull.Value ? (string)reader["Code"] : (string)null;
                    inbatch.UDMCode = reader["UDMCode"] != DBNull.Value ? (string)reader["UDMCode"] : (string)null;
                    inbatch.NewLeads = reader["NewLeads"] != DBNull.Value ? (int)reader["NewLeads"] : (int?)null;
                    inbatch.UpdatedLeads = reader["UpdatedLeads"] != DBNull.Value ? (int)reader["UpdatedLeads"] : (int?)null;
                    inbatch.ContainsLatentLeads = reader["ContainsLatentLeads"] != DBNull.Value ? (bool)reader["ContainsLatentLeads"] : (bool?)null;
                    inbatch.Completed = reader["Completed"] != DBNull.Value ? (bool)reader["Completed"] : (bool?)null;
                    inbatch.DateReceived = reader["DateReceived"] != DBNull.Value ? (DateTime)reader["DateReceived"] : (DateTime?)null;
                    inbatch.IsArchived = reader["IsArchived"] != DBNull.Value ? (bool)reader["IsArchived"] : (bool?)null;
                    inbatch.StampDate = (DateTime)reader["StampDate"];
                    inbatch.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INBatch does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INBatch object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inbatch object from the database.
        /// </summary>
        /// <param name="inbatch">The inbatch to fill.</param>
        internal static void Fill(INBatch inbatch)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inbatch.ConnectionName, INBatchQueries.Fill(inbatch, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inbatch, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INBatch object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inbatch object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INBatch inbatch)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inbatch.ConnectionName, INBatchQueries.FillData(inbatch, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INBatch object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inbatch object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inbatch">The inbatch to fill from history.</param>
        internal static void FillHistory(INBatch inbatch, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inbatch.ConnectionName, INBatchQueries.FillHistory(inbatch, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inbatch, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INBatch object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inbatch objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INBatchCollection List(bool showDeleted, string connectionName)
        {
            INBatchCollection collection = new INBatchCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INBatchQueries.ListDeleted() : INBatchQueries.List(), null);
                while (reader.Read())
                {
                    INBatch inbatch = new INBatch((long)reader["ID"]);
                    inbatch.ConnectionName = connectionName;
                    collection.Add(inbatch);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INBatch objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inbatch objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INBatchQueries.ListDeleted() : INBatchQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INBatch objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inbatch object from the database.
        /// </summary>
        /// <param name="inbatch">The inbatch to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INBatch inbatch)
        {
            INBatchCollection collection = new INBatchCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inbatch.ConnectionName, INBatchQueries.ListHistory(inbatch, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INBatch in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inbatch object to the database.
        /// </summary>
        /// <param name="inbatch">The INBatch object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inbatch was saved successfull, otherwise, false.</returns>
        internal static bool Save(INBatch inbatch)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inbatch.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inbatch.ConnectionName, INBatchQueries.Save(inbatch, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inbatch.ConnectionName, INBatchQueries.Save(inbatch, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inbatch.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inbatch.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INBatch object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inbatch objects in the database.
        /// </summary>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="code">The code search criteria.</param>
        /// <param name="udmcode">The udmcode search criteria.</param>
        /// <param name="newleads">The newleads search criteria.</param>
        /// <param name="updatedleads">The updatedleads search criteria.</param>
        /// <param name="containslatentleads">The containslatentleads search criteria.</param>
        /// <param name="completed">The completed search criteria.</param>
        /// <param name="datereceived">The datereceived search criteria.</param>
        /// <param name="isarchived">The isarchived search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INBatchCollection Search(long? fkincampaignid, string code, string udmcode, int? newleads, int? updatedleads, bool? containslatentleads, bool? completed, DateTime? datereceived, bool? isarchived, string connectionName)
        {
            INBatchCollection collection = new INBatchCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INBatchQueries.Search(fkincampaignid, code, udmcode, newleads, updatedleads, containslatentleads, completed, datereceived, isarchived), null);
                while (reader.Read())
                {
                    INBatch inbatch = new INBatch((long)reader["ID"]);
                    inbatch.ConnectionName = connectionName;
                    collection.Add(inbatch);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INBatch objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inbatch objects in the database.
        /// </summary>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="code">The code search criteria.</param>
        /// <param name="udmcode">The udmcode search criteria.</param>
        /// <param name="newleads">The newleads search criteria.</param>
        /// <param name="updatedleads">The updatedleads search criteria.</param>
        /// <param name="containslatentleads">The containslatentleads search criteria.</param>
        /// <param name="completed">The completed search criteria.</param>
        /// <param name="datereceived">The datereceived search criteria.</param>
        /// <param name="isarchived">The isarchived search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkincampaignid, string code, string udmcode, int? newleads, int? updatedleads, bool? containslatentleads, bool? completed, DateTime? datereceived, bool? isarchived, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INBatchQueries.Search(fkincampaignid, code, udmcode, newleads, updatedleads, containslatentleads, completed, datereceived, isarchived), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INBatch objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inbatch objects in the database.
        /// </summary>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="code">The code search criteria.</param>
        /// <param name="udmcode">The udmcode search criteria.</param>
        /// <param name="newleads">The newleads search criteria.</param>
        /// <param name="updatedleads">The updatedleads search criteria.</param>
        /// <param name="containslatentleads">The containslatentleads search criteria.</param>
        /// <param name="completed">The completed search criteria.</param>
        /// <param name="datereceived">The datereceived search criteria.</param>
        /// <param name="isarchived">The isarchived search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INBatch SearchOne(long? fkincampaignid, string code, string udmcode, int? newleads, int? updatedleads, bool? containslatentleads, bool? completed, DateTime? datereceived, bool? isarchived, string connectionName)
        {
            INBatch inbatch = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INBatchQueries.Search(fkincampaignid, code, udmcode, newleads, updatedleads, containslatentleads, completed, datereceived, isarchived), null);
                if (reader.Read())
                {
                    inbatch = new INBatch((long)reader["ID"]);
                    inbatch.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INBatch objects in the database", ex);
            }
            return inbatch;
        }
        #endregion
    }
}
