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
    /// Contains methods to fill, save and delete inleadbookimport objects.
    /// </summary>
    public partial class INLeadBookImportMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inleadbookimport object from the database.
        /// </summary>
        /// <param name="inleadbookimport">The id of the inleadbookimport object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inleadbookimport object.</param>
        /// <returns>True if the inleadbookimport object was deleted successfully, else false.</returns>
        internal static bool Delete(INLeadBookImport inleadbookimport)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inleadbookimport.ConnectionName, INLeadBookImportQueries.Delete(inleadbookimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INLeadBookImport object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inleadbookimport object from the database.
        /// </summary>
        /// <param name="inleadbookimport">The id of the inleadbookimport object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inleadbookimport history.</param>
        /// <returns>True if the inleadbookimport history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INLeadBookImport inleadbookimport)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inleadbookimport.ConnectionName, INLeadBookImportQueries.DeleteHistory(inleadbookimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INLeadBookImport history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inleadbookimport object from the database.
        /// </summary>
        /// <param name="inleadbookimport">The id of the inleadbookimport object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inleadbookimport object.</param>
        /// <returns>True if the inleadbookimport object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INLeadBookImport inleadbookimport)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inleadbookimport.ConnectionName, INLeadBookImportQueries.UnDelete(inleadbookimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INLeadBookImport object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inleadbookimport object from the data reader.
        /// </summary>
        /// <param name="inleadbookimport">The inleadbookimport object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INLeadBookImport inleadbookimport, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inleadbookimport.IsLoaded = true;
                    inleadbookimport.FKINLeadBookID = reader["FKINLeadBookID"] != DBNull.Value ? (long)reader["FKINLeadBookID"] : (long?)null;
                    inleadbookimport.FKINImportID = reader["FKINImportID"] != DBNull.Value ? (long)reader["FKINImportID"] : (long?)null;
                    inleadbookimport.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INLeadBookImport does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INLeadBookImport object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inleadbookimport object from the database.
        /// </summary>
        /// <param name="inleadbookimport">The inleadbookimport to fill.</param>
        internal static void Fill(INLeadBookImport inleadbookimport)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inleadbookimport.ConnectionName, INLeadBookImportQueries.Fill(inleadbookimport, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inleadbookimport, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INLeadBookImport object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inleadbookimport object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INLeadBookImport inleadbookimport)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inleadbookimport.ConnectionName, INLeadBookImportQueries.FillData(inleadbookimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INLeadBookImport object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inleadbookimport object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inleadbookimport">The inleadbookimport to fill from history.</param>
        internal static void FillHistory(INLeadBookImport inleadbookimport, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inleadbookimport.ConnectionName, INLeadBookImportQueries.FillHistory(inleadbookimport, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inleadbookimport, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INLeadBookImport object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inleadbookimport objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INLeadBookImportCollection List(bool showDeleted, string connectionName)
        {
            INLeadBookImportCollection collection = new INLeadBookImportCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INLeadBookImportQueries.ListDeleted() : INLeadBookImportQueries.List(), null);
                while (reader.Read())
                {
                    INLeadBookImport inleadbookimport = new INLeadBookImport((long)reader["ID"]);
                    inleadbookimport.ConnectionName = connectionName;
                    collection.Add(inleadbookimport);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INLeadBookImport objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inleadbookimport objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INLeadBookImportQueries.ListDeleted() : INLeadBookImportQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INLeadBookImport objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inleadbookimport object from the database.
        /// </summary>
        /// <param name="inleadbookimport">The inleadbookimport to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INLeadBookImport inleadbookimport)
        {
            INLeadBookImportCollection collection = new INLeadBookImportCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inleadbookimport.ConnectionName, INLeadBookImportQueries.ListHistory(inleadbookimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INLeadBookImport in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inleadbookimport object to the database.
        /// </summary>
        /// <param name="inleadbookimport">The INLeadBookImport object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inleadbookimport was saved successfull, otherwise, false.</returns>
        internal static bool Save(INLeadBookImport inleadbookimport)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inleadbookimport.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inleadbookimport.ConnectionName, INLeadBookImportQueries.Save(inleadbookimport, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inleadbookimport.ConnectionName, INLeadBookImportQueries.Save(inleadbookimport, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inleadbookimport.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inleadbookimport.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INLeadBookImport object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inleadbookimport objects in the database.
        /// </summary>
        /// <param name="fkinleadbookid">The fkinleadbookid search criteria.</param>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INLeadBookImportCollection Search(long? fkinleadbookid, long? fkinimportid, string connectionName)
        {
            INLeadBookImportCollection collection = new INLeadBookImportCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INLeadBookImportQueries.Search(fkinleadbookid, fkinimportid), null);
                while (reader.Read())
                {
                    INLeadBookImport inleadbookimport = new INLeadBookImport((long)reader["ID"]);
                    inleadbookimport.ConnectionName = connectionName;
                    collection.Add(inleadbookimport);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INLeadBookImport objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inleadbookimport objects in the database.
        /// </summary>
        /// <param name="fkinleadbookid">The fkinleadbookid search criteria.</param>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinleadbookid, long? fkinimportid, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INLeadBookImportQueries.Search(fkinleadbookid, fkinimportid), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INLeadBookImport objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inleadbookimport objects in the database.
        /// </summary>
        /// <param name="fkinleadbookid">The fkinleadbookid search criteria.</param>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INLeadBookImport SearchOne(long? fkinleadbookid, long? fkinimportid, string connectionName)
        {
            INLeadBookImport inleadbookimport = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INLeadBookImportQueries.Search(fkinleadbookid, fkinimportid), null);
                if (reader.Read())
                {
                    inleadbookimport = new INLeadBookImport((long)reader["ID"]);
                    inleadbookimport.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INLeadBookImport objects in the database", ex);
            }
            return inleadbookimport;
        }
        #endregion
    }
}
