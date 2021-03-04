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
    /// Contains methods to fill, save and delete inleadbook objects.
    /// </summary>
    public partial class INLeadBookMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inleadbook object from the database.
        /// </summary>
        /// <param name="inleadbook">The id of the inleadbook object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inleadbook object.</param>
        /// <returns>True if the inleadbook object was deleted successfully, else false.</returns>
        internal static bool Delete(INLeadBook inleadbook)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inleadbook.ConnectionName, INLeadBookQueries.Delete(inleadbook, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INLeadBook object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inleadbook object from the database.
        /// </summary>
        /// <param name="inleadbook">The id of the inleadbook object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inleadbook history.</param>
        /// <returns>True if the inleadbook history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INLeadBook inleadbook)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inleadbook.ConnectionName, INLeadBookQueries.DeleteHistory(inleadbook, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INLeadBook history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inleadbook object from the database.
        /// </summary>
        /// <param name="inleadbook">The id of the inleadbook object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inleadbook object.</param>
        /// <returns>True if the inleadbook object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INLeadBook inleadbook)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inleadbook.ConnectionName, INLeadBookQueries.UnDelete(inleadbook, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INLeadBook object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inleadbook object from the data reader.
        /// </summary>
        /// <param name="inleadbook">The inleadbook object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INLeadBook inleadbook, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inleadbook.IsLoaded = true;
                    inleadbook.FKUserID = reader["FKUserID"] != DBNull.Value ? (long)reader["FKUserID"] : (long?)null;
                    inleadbook.FKINBatchID = reader["FKINBatchID"] != DBNull.Value ? (long)reader["FKINBatchID"] : (long?)null;
                    inleadbook.Description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : (string)null;
                    inleadbook.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INLeadBook does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INLeadBook object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inleadbook object from the database.
        /// </summary>
        /// <param name="inleadbook">The inleadbook to fill.</param>
        internal static void Fill(INLeadBook inleadbook)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inleadbook.ConnectionName, INLeadBookQueries.Fill(inleadbook, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inleadbook, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INLeadBook object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inleadbook object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INLeadBook inleadbook)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inleadbook.ConnectionName, INLeadBookQueries.FillData(inleadbook, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INLeadBook object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inleadbook object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inleadbook">The inleadbook to fill from history.</param>
        internal static void FillHistory(INLeadBook inleadbook, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inleadbook.ConnectionName, INLeadBookQueries.FillHistory(inleadbook, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inleadbook, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INLeadBook object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inleadbook objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INLeadBookCollection List(bool showDeleted, string connectionName)
        {
            INLeadBookCollection collection = new INLeadBookCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INLeadBookQueries.ListDeleted() : INLeadBookQueries.List(), null);
                while (reader.Read())
                {
                    INLeadBook inleadbook = new INLeadBook((long)reader["ID"]);
                    inleadbook.ConnectionName = connectionName;
                    collection.Add(inleadbook);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INLeadBook objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inleadbook objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INLeadBookQueries.ListDeleted() : INLeadBookQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INLeadBook objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inleadbook object from the database.
        /// </summary>
        /// <param name="inleadbook">The inleadbook to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INLeadBook inleadbook)
        {
            INLeadBookCollection collection = new INLeadBookCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inleadbook.ConnectionName, INLeadBookQueries.ListHistory(inleadbook, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INLeadBook in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inleadbook object to the database.
        /// </summary>
        /// <param name="inleadbook">The INLeadBook object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inleadbook was saved successfull, otherwise, false.</returns>
        internal static bool Save(INLeadBook inleadbook)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inleadbook.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inleadbook.ConnectionName, INLeadBookQueries.Save(inleadbook, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inleadbook.ConnectionName, INLeadBookQueries.Save(inleadbook, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inleadbook.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inleadbook.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INLeadBook object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inleadbook objects in the database.
        /// </summary>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fkinbatchid">The fkinbatchid search criteria.</param>
        /// <param name="description">The description search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INLeadBookCollection Search(long? fkuserid, long? fkinbatchid, string description, string connectionName)
        {
            INLeadBookCollection collection = new INLeadBookCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INLeadBookQueries.Search(fkuserid, fkinbatchid, description), null);
                while (reader.Read())
                {
                    INLeadBook inleadbook = new INLeadBook((long)reader["ID"]);
                    inleadbook.ConnectionName = connectionName;
                    collection.Add(inleadbook);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INLeadBook objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inleadbook objects in the database.
        /// </summary>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fkinbatchid">The fkinbatchid search criteria.</param>
        /// <param name="description">The description search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkuserid, long? fkinbatchid, string description, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INLeadBookQueries.Search(fkuserid, fkinbatchid, description), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INLeadBook objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inleadbook objects in the database.
        /// </summary>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fkinbatchid">The fkinbatchid search criteria.</param>
        /// <param name="description">The description search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INLeadBook SearchOne(long? fkuserid, long? fkinbatchid, string description, string connectionName)
        {
            INLeadBook inleadbook = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INLeadBookQueries.Search(fkuserid, fkinbatchid, description), null);
                if (reader.Read())
                {
                    inleadbook = new INLeadBook((long)reader["ID"]);
                    inleadbook.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INLeadBook objects in the database", ex);
            }
            return inleadbook;
        }
        #endregion
    }
}
