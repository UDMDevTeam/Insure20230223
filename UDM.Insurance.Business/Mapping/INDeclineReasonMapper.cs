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
    /// Contains methods to fill, save and delete indeclinereason objects.
    /// </summary>
    public partial class INDeclineReasonMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) indeclinereason object from the database.
        /// </summary>
        /// <param name="indeclinereason">The id of the indeclinereason object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the indeclinereason object.</param>
        /// <returns>True if the indeclinereason object was deleted successfully, else false.</returns>
        internal static bool Delete(INDeclineReason indeclinereason)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(indeclinereason.ConnectionName, INDeclineReasonQueries.Delete(indeclinereason, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INDeclineReason object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) indeclinereason object from the database.
        /// </summary>
        /// <param name="indeclinereason">The id of the indeclinereason object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the indeclinereason history.</param>
        /// <returns>True if the indeclinereason history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INDeclineReason indeclinereason)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(indeclinereason.ConnectionName, INDeclineReasonQueries.DeleteHistory(indeclinereason, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INDeclineReason history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) indeclinereason object from the database.
        /// </summary>
        /// <param name="indeclinereason">The id of the indeclinereason object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the indeclinereason object.</param>
        /// <returns>True if the indeclinereason object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INDeclineReason indeclinereason)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(indeclinereason.ConnectionName, INDeclineReasonQueries.UnDelete(indeclinereason, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INDeclineReason object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the indeclinereason object from the data reader.
        /// </summary>
        /// <param name="indeclinereason">The indeclinereason object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INDeclineReason indeclinereason, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    indeclinereason.IsLoaded = true;
                    indeclinereason.Code = reader["Code"] != DBNull.Value ? (string)reader["Code"] : (string)null;
                    indeclinereason.Description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : (string)null;
                    indeclinereason.CodeNumber = reader["CodeNumber"] != DBNull.Value ? (string)reader["CodeNumber"] : (string)null;
                    indeclinereason.StampDate = (DateTime)reader["StampDate"];
                    indeclinereason.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INDeclineReason does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INDeclineReason object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an indeclinereason object from the database.
        /// </summary>
        /// <param name="indeclinereason">The indeclinereason to fill.</param>
        internal static void Fill(INDeclineReason indeclinereason)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(indeclinereason.ConnectionName, INDeclineReasonQueries.Fill(indeclinereason, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(indeclinereason, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INDeclineReason object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a indeclinereason object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INDeclineReason indeclinereason)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(indeclinereason.ConnectionName, INDeclineReasonQueries.FillData(indeclinereason, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INDeclineReason object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an indeclinereason object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="indeclinereason">The indeclinereason to fill from history.</param>
        internal static void FillHistory(INDeclineReason indeclinereason, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(indeclinereason.ConnectionName, INDeclineReasonQueries.FillHistory(indeclinereason, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(indeclinereason, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INDeclineReason object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the indeclinereason objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INDeclineReasonCollection List(bool showDeleted, string connectionName)
        {
            INDeclineReasonCollection collection = new INDeclineReasonCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INDeclineReasonQueries.ListDeleted() : INDeclineReasonQueries.List(), null);
                while (reader.Read())
                {
                    INDeclineReason indeclinereason = new INDeclineReason((long)reader["ID"]);
                    indeclinereason.ConnectionName = connectionName;
                    collection.Add(indeclinereason);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INDeclineReason objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the indeclinereason objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INDeclineReasonQueries.ListDeleted() : INDeclineReasonQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INDeclineReason objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) indeclinereason object from the database.
        /// </summary>
        /// <param name="indeclinereason">The indeclinereason to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INDeclineReason indeclinereason)
        {
            INDeclineReasonCollection collection = new INDeclineReasonCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(indeclinereason.ConnectionName, INDeclineReasonQueries.ListHistory(indeclinereason, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INDeclineReason in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an indeclinereason object to the database.
        /// </summary>
        /// <param name="indeclinereason">The INDeclineReason object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the indeclinereason was saved successfull, otherwise, false.</returns>
        internal static bool Save(INDeclineReason indeclinereason)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (indeclinereason.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(indeclinereason.ConnectionName, INDeclineReasonQueries.Save(indeclinereason, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(indeclinereason.ConnectionName, INDeclineReasonQueries.Save(indeclinereason, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            indeclinereason.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= indeclinereason.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INDeclineReason object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for indeclinereason objects in the database.
        /// </summary>
        /// <param name="code">The code search criteria.</param>
        /// <param name="description">The description search criteria.</param>
        /// <param name="codenumber">The codenumber search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INDeclineReasonCollection Search(string code, string description, string codenumber, string connectionName)
        {
            INDeclineReasonCollection collection = new INDeclineReasonCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INDeclineReasonQueries.Search(code, description, codenumber), null);
                while (reader.Read())
                {
                    INDeclineReason indeclinereason = new INDeclineReason((long)reader["ID"]);
                    indeclinereason.ConnectionName = connectionName;
                    collection.Add(indeclinereason);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INDeclineReason objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for indeclinereason objects in the database.
        /// </summary>
        /// <param name="code">The code search criteria.</param>
        /// <param name="description">The description search criteria.</param>
        /// <param name="codenumber">The codenumber search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(string code, string description, string codenumber, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INDeclineReasonQueries.Search(code, description, codenumber), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INDeclineReason objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 indeclinereason objects in the database.
        /// </summary>
        /// <param name="code">The code search criteria.</param>
        /// <param name="description">The description search criteria.</param>
        /// <param name="codenumber">The codenumber search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INDeclineReason SearchOne(string code, string description, string codenumber, string connectionName)
        {
            INDeclineReason indeclinereason = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INDeclineReasonQueries.Search(code, description, codenumber), null);
                if (reader.Read())
                {
                    indeclinereason = new INDeclineReason((long)reader["ID"]);
                    indeclinereason.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INDeclineReason objects in the database", ex);
            }
            return indeclinereason;
        }
        #endregion
    }
}
