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
    /// Contains methods to fill, save and delete calldata objects.
    /// </summary>
    public partial class INIDConfirmedMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) calldata object from the database.
        /// </summary>
        /// <param name="calldata">The id of the calldata object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the calldata object.</param>
        /// <returns>True if the calldata object was deleted successfully, else false.</returns>
        internal static bool Delete(INIDConfirmed calldata)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(calldata.ConnectionName, INIDConfirmedQueries.Delete(calldata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INIDConfirmed object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) calldata object from the database.
        /// </summary>
        /// <param name="calldata">The id of the calldata object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the calldata history.</param>
        /// <returns>True if the calldata history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INIDConfirmed calldata)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(calldata.ConnectionName, INIDConfirmedQueries.DeleteHistory(calldata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INIDConfirmed history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) calldata object from the database.
        /// </summary>
        /// <param name="calldata">The id of the calldata object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the calldata object.</param>
        /// <returns>True if the calldata object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INIDConfirmed calldata)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(calldata.ConnectionName, INIDConfirmedQueries.UnDelete(calldata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INIDConfirmed object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the calldata object from the data reader.
        /// </summary>
        /// <param name="calldata">The calldata object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INIDConfirmed calldata, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    calldata.IsLoaded = true;
                    calldata.FKImportID = reader["FKImportID"] != DBNull.Value ? (long)reader["FKImportID"] : (long?)null;
                    calldata.Confirmed = reader["Confirmed"] != DBNull.Value ? (string)reader["Confirmed"] : (string)null;
                    calldata.StampDate = (DateTime)reader["StampDate"];
                    calldata.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INIDConfirmed does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INIDConfirmed object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an calldata object from the database.
        /// </summary>
        /// <param name="calldata">The calldata to fill.</param>
        internal static void Fill(INIDConfirmed calldata)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(calldata.ConnectionName, INIDConfirmedQueries.Fill(calldata, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(calldata, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INIDConfirmed object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a calldata object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INIDConfirmed calldata)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(calldata.ConnectionName, INIDConfirmedQueries.FillData(calldata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INIDConfirmed object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an calldata object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="calldata">The calldata to fill from history.</param>
        internal static void FillHistory(INIDConfirmed calldata, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(calldata.ConnectionName, INIDConfirmedQueries.FillHistory(calldata, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(calldata, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INIDConfirmed object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the calldata objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INIDConfirmedCollection List(bool showDeleted, string connectionName)
        {
            INIDConfirmedCollection collection = new INIDConfirmedCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INIDConfirmedQueries.ListDeleted() : INIDConfirmedQueries.List(), null);
                while (reader.Read())
                {
                    INIDConfirmed calldata = new INIDConfirmed((long)reader["ID"]);
                    calldata.ConnectionName = connectionName;
                    collection.Add(calldata);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INIDConfirmed objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the calldata objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INIDConfirmedQueries.ListDeleted() : INIDConfirmedQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INIDConfirmed objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) calldata object from the database.
        /// </summary>
        /// <param name="calldata">The calldata to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INIDConfirmed calldata)
        {
            INIDConfirmedCollection collection = new INIDConfirmedCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(calldata.ConnectionName, INIDConfirmedQueries.ListHistory(calldata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INIDConfirmed in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an calldata object to the database.
        /// </summary>
        /// <param name="calldata">The CallData object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the calldata was saved successfull, otherwise, false.</returns>
        internal static bool Save(INIDConfirmed calldata)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (calldata.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(calldata.ConnectionName, INIDConfirmedQueries.Save(calldata, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(calldata.ConnectionName, INIDConfirmedQueries.Save(calldata, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            calldata.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= calldata.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INIDConfirmed object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for calldata objects in the database.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="number">The number search criteria.</param>
        /// <param name="extension">The extension search criteria.</param>
        /// <param name="recref">The recref search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INIDConfirmedCollection Search(long? fkimportid, string confirmed, string connectionName)
        {
            INIDConfirmedCollection collection = new INIDConfirmedCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INIDConfirmedQueries.Search(fkimportid, confirmed), null);
                while (reader.Read())
                {
                    INIDConfirmed calldata = new INIDConfirmed((long)reader["ID"]);
                    calldata.ConnectionName = connectionName;
                    collection.Add(calldata);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INIDConfirmed objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for calldata objects in the database.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="number">The number search criteria.</param>
        /// <param name="extension">The extension search criteria.</param>
        /// <param name="recref">The recref search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkimportid, string confirmed, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INIDConfirmedQueries.Search(fkimportid, confirmed), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INIDConfirmed objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 calldata objects in the database.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="number">The number search criteria.</param>
        /// <param name="extension">The extension search criteria.</param>
        /// <param name="recref">The recref search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INIDConfirmed SearchOne(long? fkimportid, string confirmed, string connectionName)
        {
            INIDConfirmed calldata = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INIDConfirmedQueries.Search(fkimportid, confirmed), null);
                if (reader.Read())
                {
                    calldata = new INIDConfirmed((long)reader["ID"]);
                    calldata.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INIDConfirmed objects in the database", ex);
            }
            return calldata;
        }
        #endregion
    }
}
