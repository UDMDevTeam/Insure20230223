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
    /// Contains methods to fill, save and delete inimportextra objects.
    /// </summary>
    public partial class INImportExtraMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inimportextra object from the database.
        /// </summary>
        /// <param name="inimportextra">The id of the inimportextra object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inimportextra object.</param>
        /// <returns>True if the inimportextra object was deleted successfully, else false.</returns>
        internal static bool Delete(INImportExtra inimportextra)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportextra.ConnectionName, INImportExtraQueries.Delete(inimportextra, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INImportExtra object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inimportextra object from the database.
        /// </summary>
        /// <param name="inimportextra">The id of the inimportextra object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inimportextra history.</param>
        /// <returns>True if the inimportextra history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INImportExtra inimportextra)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportextra.ConnectionName, INImportExtraQueries.DeleteHistory(inimportextra, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INImportExtra history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inimportextra object from the database.
        /// </summary>
        /// <param name="inimportextra">The id of the inimportextra object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inimportextra object.</param>
        /// <returns>True if the inimportextra object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INImportExtra inimportextra)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportextra.ConnectionName, INImportExtraQueries.UnDelete(inimportextra, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INImportExtra object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inimportextra object from the data reader.
        /// </summary>
        /// <param name="inimportextra">The inimportextra object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INImportExtra inimportextra, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inimportextra.IsLoaded = true;
                    inimportextra.FKINImportID = reader["FKINImportID"] != DBNull.Value ? (long)reader["FKINImportID"] : (long?)null;
                    inimportextra.Extension1 = reader["Extension1"] != DBNull.Value ? (string)reader["Extension1"] : (string)null;
                    inimportextra.Extension2 = reader["Extension2"] != DBNull.Value ? (string)reader["Extension2"] : (string)null;
                    inimportextra.NotPossibleBumpUp = reader["NotPossibleBumpUp"] != DBNull.Value ? (bool)reader["NotPossibleBumpUp"] : (bool?)null;
                    inimportextra.FKCMCallRefUserID = reader["FKCMCallRefUserID"] != DBNull.Value ? (long)reader["FKCMCallRefUserID"] : (long?)null;
                    inimportextra.EMailRequested = reader["EMailRequested"] != DBNull.Value ? (bool)reader["EMailRequested"] : (bool?)null;
                    inimportextra.CustomerCareRequested = reader["CustomerCareRequested"] != DBNull.Value ? (bool)reader["CustomerCareRequested"] : (bool?)null;
                    inimportextra.IsGoldenLead = reader["IsGoldenLead"] != DBNull.Value ? (bool)reader["IsGoldenLead"] : (bool?)null;
                    inimportextra.StampDate = (DateTime)reader["StampDate"];
                    inimportextra.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INImportExtra does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportExtra object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportextra object from the database.
        /// </summary>
        /// <param name="inimportextra">The inimportextra to fill.</param>
        internal static void Fill(INImportExtra inimportextra)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimportextra.ConnectionName, INImportExtraQueries.Fill(inimportextra, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimportextra, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportExtra object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inimportextra object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INImportExtra inimportextra)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimportextra.ConnectionName, INImportExtraQueries.FillData(inimportextra, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INImportExtra object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportextra object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inimportextra">The inimportextra to fill from history.</param>
        internal static void FillHistory(INImportExtra inimportextra, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimportextra.ConnectionName, INImportExtraQueries.FillHistory(inimportextra, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimportextra, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportExtra object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimportextra objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INImportExtraCollection List(bool showDeleted, string connectionName)
        {
            INImportExtraCollection collection = new INImportExtraCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INImportExtraQueries.ListDeleted() : INImportExtraQueries.List(), null);
                while (reader.Read())
                {
                    INImportExtra inimportextra = new INImportExtra((long)reader["ID"]);
                    inimportextra.ConnectionName = connectionName;
                    collection.Add(inimportextra);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportExtra objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inimportextra objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INImportExtraQueries.ListDeleted() : INImportExtraQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportExtra objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inimportextra object from the database.
        /// </summary>
        /// <param name="inimportextra">The inimportextra to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INImportExtra inimportextra)
        {
            INImportExtraCollection collection = new INImportExtraCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimportextra.ConnectionName, INImportExtraQueries.ListHistory(inimportextra, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INImportExtra in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inimportextra object to the database.
        /// </summary>
        /// <param name="inimportextra">The INImportExtra object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inimportextra was saved successfull, otherwise, false.</returns>
        internal static bool Save(INImportExtra inimportextra)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inimportextra.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inimportextra.ConnectionName, INImportExtraQueries.Save(inimportextra, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inimportextra.ConnectionName, INImportExtraQueries.Save(inimportextra, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inimportextra.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inimportextra.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INImportExtra object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimportextra objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="extension1">The extension1 search criteria.</param>
        /// <param name="extension2">The extension2 search criteria.</param>
        /// <param name="notpossiblebumpup">The notpossiblebumpup search criteria.</param>
        /// <param name="fkcmcallrefuserid">The fkcmcallrefuserid search criteria.</param>
        /// <param name="emailrequested">The emailrequested search criteria.</param>
        /// <param name="customercarerequested">The customercarerequested search criteria.</param>
        /// <param name="isgoldenlead">The isgoldenlead search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportExtraCollection Search(long? fkinimportid, string extension1, string extension2, bool? notpossiblebumpup, long? fkcmcallrefuserid, bool? emailrequested, bool? customercarerequested, bool? isgoldenlead, string connectionName)
        {
            INImportExtraCollection collection = new INImportExtraCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportExtraQueries.Search(fkinimportid, extension1, extension2, notpossiblebumpup, fkcmcallrefuserid, emailrequested, customercarerequested, isgoldenlead), null);
                while (reader.Read())
                {
                    INImportExtra inimportextra = new INImportExtra((long)reader["ID"]);
                    inimportextra.ConnectionName = connectionName;
                    collection.Add(inimportextra);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportExtra objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inimportextra objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="extension1">The extension1 search criteria.</param>
        /// <param name="extension2">The extension2 search criteria.</param>
        /// <param name="notpossiblebumpup">The notpossiblebumpup search criteria.</param>
        /// <param name="fkcmcallrefuserid">The fkcmcallrefuserid search criteria.</param>
        /// <param name="emailrequested">The emailrequested search criteria.</param>
        /// <param name="customercarerequested">The customercarerequested search criteria.</param>
        /// <param name="isgoldenlead">The isgoldenlead search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinimportid, string extension1, string extension2, bool? notpossiblebumpup, long? fkcmcallrefuserid, bool? emailrequested, bool? customercarerequested, bool? isgoldenlead, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INImportExtraQueries.Search(fkinimportid, extension1, extension2, notpossiblebumpup, fkcmcallrefuserid, emailrequested, customercarerequested, isgoldenlead), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportExtra objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inimportextra objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="extension1">The extension1 search criteria.</param>
        /// <param name="extension2">The extension2 search criteria.</param>
        /// <param name="notpossiblebumpup">The notpossiblebumpup search criteria.</param>
        /// <param name="fkcmcallrefuserid">The fkcmcallrefuserid search criteria.</param>
        /// <param name="emailrequested">The emailrequested search criteria.</param>
        /// <param name="customercarerequested">The customercarerequested search criteria.</param>
        /// <param name="isgoldenlead">The isgoldenlead search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportExtra SearchOne(long? fkinimportid, string extension1, string extension2, bool? notpossiblebumpup, long? fkcmcallrefuserid, bool? emailrequested, bool? customercarerequested, bool? isgoldenlead, string connectionName)
        {
            INImportExtra inimportextra = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportExtraQueries.Search(fkinimportid, extension1, extension2, notpossiblebumpup, fkcmcallrefuserid, emailrequested, customercarerequested, isgoldenlead), null);
                if (reader.Read())
                {
                    inimportextra = new INImportExtra((long)reader["ID"]);
                    inimportextra.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportExtra objects in the database", ex);
            }
            return inimportextra;
        }
        #endregion
    }
}
