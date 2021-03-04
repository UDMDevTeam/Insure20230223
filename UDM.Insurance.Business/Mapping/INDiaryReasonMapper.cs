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
    /// Contains methods to fill, save and delete indiaryreason objects.
    /// </summary>
    public partial class INDiaryReasonMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) indiaryreason object from the database.
        /// </summary>
        /// <param name="indiaryreason">The id of the indiaryreason object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the indiaryreason object.</param>
        /// <returns>True if the indiaryreason object was deleted successfully, else false.</returns>
        internal static bool Delete(INDiaryReason indiaryreason)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(indiaryreason.ConnectionName, INDiaryReasonQueries.Delete(indiaryreason, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INDiaryReason object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) indiaryreason object from the database.
        /// </summary>
        /// <param name="indiaryreason">The id of the indiaryreason object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the indiaryreason history.</param>
        /// <returns>True if the indiaryreason history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INDiaryReason indiaryreason)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(indiaryreason.ConnectionName, INDiaryReasonQueries.DeleteHistory(indiaryreason, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INDiaryReason history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) indiaryreason object from the database.
        /// </summary>
        /// <param name="indiaryreason">The id of the indiaryreason object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the indiaryreason object.</param>
        /// <returns>True if the indiaryreason object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INDiaryReason indiaryreason)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(indiaryreason.ConnectionName, INDiaryReasonQueries.UnDelete(indiaryreason, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INDiaryReason object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the indiaryreason object from the data reader.
        /// </summary>
        /// <param name="indiaryreason">The indiaryreason object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INDiaryReason indiaryreason, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    indiaryreason.IsLoaded = true;
                    indiaryreason.Code = reader["Code"] != DBNull.Value ? (string)reader["Code"] : (string)null;
                    indiaryreason.Description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : (string)null;
                    indiaryreason.CodeNumber = reader["CodeNumber"] != DBNull.Value ? (string)reader["CodeNumber"] : (string)null;
                    indiaryreason.StampDate = (DateTime)reader["StampDate"];
                    indiaryreason.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INDiaryReason does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INDiaryReason object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an indiaryreason object from the database.
        /// </summary>
        /// <param name="indiaryreason">The indiaryreason to fill.</param>
        internal static void Fill(INDiaryReason indiaryreason)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(indiaryreason.ConnectionName, INDiaryReasonQueries.Fill(indiaryreason, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(indiaryreason, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INDiaryReason object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a indiaryreason object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INDiaryReason indiaryreason)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(indiaryreason.ConnectionName, INDiaryReasonQueries.FillData(indiaryreason, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INDiaryReason object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an indiaryreason object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="indiaryreason">The indiaryreason to fill from history.</param>
        internal static void FillHistory(INDiaryReason indiaryreason, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(indiaryreason.ConnectionName, INDiaryReasonQueries.FillHistory(indiaryreason, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(indiaryreason, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INDiaryReason object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the indiaryreason objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INDiaryReasonCollection List(bool showDeleted, string connectionName)
        {
            INDiaryReasonCollection collection = new INDiaryReasonCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INDiaryReasonQueries.ListDeleted() : INDiaryReasonQueries.List(), null);
                while (reader.Read())
                {
                    INDiaryReason indiaryreason = new INDiaryReason((long)reader["ID"]);
                    indiaryreason.ConnectionName = connectionName;
                    collection.Add(indiaryreason);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INDiaryReason objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the indiaryreason objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INDiaryReasonQueries.ListDeleted() : INDiaryReasonQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INDiaryReason objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) indiaryreason object from the database.
        /// </summary>
        /// <param name="indiaryreason">The indiaryreason to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INDiaryReason indiaryreason)
        {
            INDiaryReasonCollection collection = new INDiaryReasonCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(indiaryreason.ConnectionName, INDiaryReasonQueries.ListHistory(indiaryreason, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INDiaryReason in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an indiaryreason object to the database.
        /// </summary>
        /// <param name="indiaryreason">The INDiaryReason object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the indiaryreason was saved successfull, otherwise, false.</returns>
        internal static bool Save(INDiaryReason indiaryreason)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (indiaryreason.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(indiaryreason.ConnectionName, INDiaryReasonQueries.Save(indiaryreason, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(indiaryreason.ConnectionName, INDiaryReasonQueries.Save(indiaryreason, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            indiaryreason.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= indiaryreason.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INDiaryReason object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for indiaryreason objects in the database.
        /// </summary>
        /// <param name="code">The code search criteria.</param>
        /// <param name="description">The description search criteria.</param>
        /// <param name="codenumber">The codenumber search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INDiaryReasonCollection Search(string code, string description, string codenumber, string connectionName)
        {
            INDiaryReasonCollection collection = new INDiaryReasonCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INDiaryReasonQueries.Search(code, description, codenumber), null);
                while (reader.Read())
                {
                    INDiaryReason indiaryreason = new INDiaryReason((long)reader["ID"]);
                    indiaryreason.ConnectionName = connectionName;
                    collection.Add(indiaryreason);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INDiaryReason objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for indiaryreason objects in the database.
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
                return Database.ExecuteDataSet(connectionName, INDiaryReasonQueries.Search(code, description, codenumber), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INDiaryReason objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 indiaryreason objects in the database.
        /// </summary>
        /// <param name="code">The code search criteria.</param>
        /// <param name="description">The description search criteria.</param>
        /// <param name="codenumber">The codenumber search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INDiaryReason SearchOne(string code, string description, string codenumber, string connectionName)
        {
            INDiaryReason indiaryreason = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INDiaryReasonQueries.Search(code, description, codenumber), null);
                if (reader.Read())
                {
                    indiaryreason = new INDiaryReason((long)reader["ID"]);
                    indiaryreason.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INDiaryReason objects in the database", ex);
            }
            return indiaryreason;
        }
        #endregion
    }
}
