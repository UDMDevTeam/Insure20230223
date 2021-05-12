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
    /// Contains methods to fill, save and delete iNImportContactTracing objects.
    /// </summary>
    public partial class INImportContactTracingMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) iNImportContactTracing object from the database.
        /// </summary>
        /// <param name="iNImportContactTracing">The id of the iNImportContactTracing object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the iNImportContactTracing object.</param>
        /// <returns>True if the iNImportContactTracing object was deleted successfully, else false.</returns>
        internal static bool Delete(INImportContactTracing iNImportContactTracing)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(iNImportContactTracing.ConnectionName, INImportContactTracingQueries.Delete(iNImportContactTracing, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INImportContactTracing object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) iNImportContactTracing object from the database.
        /// </summary>
        /// <param name="iNImportContactTracing">The id of the iNImportContactTracing object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the iNImportContactTracing history.</param>
        /// <returns>True if the iNImportContactTracing history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INImportContactTracing iNImportContactTracing)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(iNImportContactTracing.ConnectionName, INImportContactTracingQueries.DeleteHistory(iNImportContactTracing, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INImportContactTracing history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) iNImportContactTracing object from the database.
        /// </summary>
        /// <param name="iNImportContactTracing">The id of the iNImportContactTracing object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the iNImportContactTracing object.</param>
        /// <returns>True if the iNImportContactTracing object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INImportContactTracing iNImportContactTracing)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(iNImportContactTracing.ConnectionName, INImportContactTracingQueries.UnDelete(iNImportContactTracing, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INImportContactTracing object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the iNImportContactTracing object from the data reader.
        /// </summary>
        /// <param name="iNImportContactTracing">The iNImportContactTracing object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INImportContactTracing iNImportContactTracing, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    iNImportContactTracing.IsLoaded = true;
                    iNImportContactTracing.FKINImportID = reader["FKINImportID"] != DBNull.Value ? (long)reader["FKINImportID"] : (long?)null;
                    iNImportContactTracing.ContactTraceOne = reader["Contact1"] != DBNull.Value ? (string)reader["Contact1"] : (string)null;
                    iNImportContactTracing.ContactTraceTwo = reader["Contact2"] != DBNull.Value ? (string)reader["Contact2"] : (string)null;
                    iNImportContactTracing.ContactTraceThree = reader["Contact3"] != DBNull.Value ? (string)reader["Contact3"] : (string)null;
                    iNImportContactTracing.ContactTraceFour = reader["Contact4"] != DBNull.Value ? (string)reader["Contact4"] : (string)null;
                    iNImportContactTracing.ContactTraceFive = reader["Contact5"] != DBNull.Value ? (string)reader["Contact5"] : (string)null;
                    iNImportContactTracing.ContactTraceSix = reader["Contact6"] != DBNull.Value ? (string)reader["Contact6"] : (string)null;
                    iNImportContactTracing.StampDate = (DateTime)reader["StampDate"];
                    iNImportContactTracing.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INImportContactTracing does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportContactTracing object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an iNImportContactTracing object from the database.
        /// </summary>
        /// <param name="iNImportContactTracing">The iNImportContactTracing to fill.</param>
        internal static void Fill(INImportContactTracing iNImportContactTracing)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(iNImportContactTracing.ConnectionName, INImportContactTracingQueries.Fill(iNImportContactTracing, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(iNImportContactTracing, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportContactTracing object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a iNImportContactTracing object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INImportContactTracing iNImportContactTracing)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(iNImportContactTracing.ConnectionName, INImportContactTracingQueries.FillData(iNImportContactTracing, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INImportContactTracing object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an iNImportContactTracing object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="iNImportContactTracing">The iNImportContactTracing to fill from history.</param>
        internal static void FillHistory(INImportContactTracing iNImportContactTracing, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(iNImportContactTracing.ConnectionName, INImportContactTracingQueries.FillHistory(iNImportContactTracing, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(iNImportContactTracing, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportContactTracing object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the iNImportContactTracing objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INImportContactTracingCollection List(bool showDeleted, string connectionName)
        {
            INImportContactTracingCollection collection = new INImportContactTracingCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INImportContactTracingQueries.ListDeleted() : INImportContactTracingQueries.List(), null);
                while (reader.Read())
                {
                    INImportContactTracing iNImportContactTracing = new INImportContactTracing((long)reader["ID"]);
                    iNImportContactTracing.ConnectionName = connectionName;
                    collection.Add(iNImportContactTracing);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportContactTracing objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the iNImportContactTracing objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INImportContactTracingQueries.ListDeleted() : INImportContactTracingQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportContactTracing objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) iNImportContactTracing object from the database.
        /// </summary>
        /// <param name="iNImportContactTracing">The iNImportContactTracing to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INImportContactTracing iNImportContactTracing)
        {
            INImportContactTracingCollection collection = new INImportContactTracingCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(iNImportContactTracing.ConnectionName, INImportContactTracingQueries.ListHistory(iNImportContactTracing, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INImportContactTracing in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an iNImportContactTracing object to the database.
        /// </summary>
        /// <param name="iNImportContactTracing">The INImportContactTracing object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the iNImportContactTracing was saved successfull, otherwise, false.</returns>
        internal static bool Save(INImportContactTracing iNImportContactTracing)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (iNImportContactTracing.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(iNImportContactTracing.ConnectionName, INImportContactTracingQueries.Save(iNImportContactTracing, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(iNImportContactTracing.ConnectionName, INImportContactTracingQueries.Save(iNImportContactTracing, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            iNImportContactTracing.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= iNImportContactTracing.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INImportContactTracing object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for iNImportContactTracing objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkinrelationshipid">The fkinrelationshipid search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="telcontact">The telcontact search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportContactTracingCollection Search(long? fkinimportid, string contact1, string contact2, string contact3, string contact4, string contact5, string contact6, string connectionName)
        {
            INImportContactTracingCollection collection = new INImportContactTracingCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportContactTracingQueries.Search(fkinimportid, contact1, contact2, contact3, contact4, contact5, contact6), null);
                while (reader.Read())
                {
                    INImportContactTracing iNImportContactTracing = new INImportContactTracing((long)reader["ID"]);
                    iNImportContactTracing.ConnectionName = connectionName;
                    collection.Add(iNImportContactTracing);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportContactTracing objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for iNImportContactTracing objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkinrelationshipid">The fkinrelationshipid search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="telcontact">The telcontact search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinimportid, string contact1, string contact2, string contact3, string contact4, string contact5, string contact6, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INImportContactTracingQueries.Search(fkinimportid, contact1, contact2, contact3, contact4, contact5, contact6), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportContactTracing objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 iNImportContactTracing objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkinrelationshipid">The fkinrelationshipid search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="telcontact">The telcontact search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportContactTracing SearchOne(long? fkinimportid, string contact1, string contact2, string contact3, string contact4, string contact5, string contact6, string connectionName)
        {
            INImportContactTracing iNImportContactTracing = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportContactTracingQueries.Search(fkinimportid, contact1, contact2, contact3, contact4, contact5, contact6), null);
                if (reader.Read())
                {
                    iNImportContactTracing = new INImportContactTracing((long)reader["ID"]);
                    iNImportContactTracing.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportContactTracing objects in the database", ex);
            }
            return iNImportContactTracing;
        }
        #endregion
    }
}
