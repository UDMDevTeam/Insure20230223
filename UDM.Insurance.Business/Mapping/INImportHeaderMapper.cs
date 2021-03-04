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
    /// Contains methods to fill, save and delete inimportheader objects.
    /// </summary>
    public partial class INImportHeaderMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inimportheader object from the database.
        /// </summary>
        /// <param name="inimportheader">The id of the inimportheader object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inimportheader object.</param>
        /// <returns>True if the inimportheader object was deleted successfully, else false.</returns>
        internal static bool Delete(INImportHeader inimportheader)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportheader.ConnectionName, INImportHeaderQueries.Delete(inimportheader, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INImportHeader object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inimportheader object from the database.
        /// </summary>
        /// <param name="inimportheader">The id of the inimportheader object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inimportheader history.</param>
        /// <returns>True if the inimportheader history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INImportHeader inimportheader)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportheader.ConnectionName, INImportHeaderQueries.DeleteHistory(inimportheader, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INImportHeader history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inimportheader object from the database.
        /// </summary>
        /// <param name="inimportheader">The id of the inimportheader object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inimportheader object.</param>
        /// <returns>True if the inimportheader object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INImportHeader inimportheader)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportheader.ConnectionName, INImportHeaderQueries.UnDelete(inimportheader, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INImportHeader object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inimportheader object from the data reader.
        /// </summary>
        /// <param name="inimportheader">The inimportheader object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INImportHeader inimportheader, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inimportheader.IsLoaded = true;
                    inimportheader.Name = reader["Name"] != DBNull.Value ? (string)reader["Name"] : (string)null;
                    inimportheader.Header = reader["Header"] != DBNull.Value ? (string)reader["Header"] : (string)null;
                    inimportheader.HeaderAlt1 = reader["HeaderAlt1"] != DBNull.Value ? (string)reader["HeaderAlt1"] : (string)null;
                    inimportheader.HeaderAlt2 = reader["HeaderAlt2"] != DBNull.Value ? (string)reader["HeaderAlt2"] : (string)null;
                    inimportheader.HeaderAlt3 = reader["HeaderAlt3"] != DBNull.Value ? (string)reader["HeaderAlt3"] : (string)null;
                    inimportheader.TableName = reader["TableName"] != DBNull.Value ? (string)reader["TableName"] : (string)null;
                    inimportheader.IgnoreIfNA = reader["IgnoreIfNA"] != DBNull.Value ? (bool)reader["IgnoreIfNA"] : (bool?)null;
                    inimportheader.IsActive = reader["IsActive"] != DBNull.Value ? (bool)reader["IsActive"] : (bool?)null;
                    inimportheader.StampDate = (DateTime)reader["StampDate"];
                    inimportheader.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INImportHeader does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportHeader object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportheader object from the database.
        /// </summary>
        /// <param name="inimportheader">The inimportheader to fill.</param>
        internal static void Fill(INImportHeader inimportheader)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimportheader.ConnectionName, INImportHeaderQueries.Fill(inimportheader, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimportheader, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportHeader object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inimportheader object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INImportHeader inimportheader)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimportheader.ConnectionName, INImportHeaderQueries.FillData(inimportheader, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INImportHeader object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportheader object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inimportheader">The inimportheader to fill from history.</param>
        internal static void FillHistory(INImportHeader inimportheader, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimportheader.ConnectionName, INImportHeaderQueries.FillHistory(inimportheader, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimportheader, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportHeader object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimportheader objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INImportHeaderCollection List(bool showDeleted, string connectionName)
        {
            INImportHeaderCollection collection = new INImportHeaderCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INImportHeaderQueries.ListDeleted() : INImportHeaderQueries.List(), null);
                while (reader.Read())
                {
                    INImportHeader inimportheader = new INImportHeader((long)reader["ID"]);
                    inimportheader.ConnectionName = connectionName;
                    collection.Add(inimportheader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportHeader objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inimportheader objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INImportHeaderQueries.ListDeleted() : INImportHeaderQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportHeader objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inimportheader object from the database.
        /// </summary>
        /// <param name="inimportheader">The inimportheader to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INImportHeader inimportheader)
        {
            INImportHeaderCollection collection = new INImportHeaderCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimportheader.ConnectionName, INImportHeaderQueries.ListHistory(inimportheader, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INImportHeader in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inimportheader object to the database.
        /// </summary>
        /// <param name="inimportheader">The INImportHeader object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inimportheader was saved successfull, otherwise, false.</returns>
        internal static bool Save(INImportHeader inimportheader)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inimportheader.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inimportheader.ConnectionName, INImportHeaderQueries.Save(inimportheader, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inimportheader.ConnectionName, INImportHeaderQueries.Save(inimportheader, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inimportheader.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inimportheader.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INImportHeader object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimportheader objects in the database.
        /// </summary>
        /// <param name="name">The name search criteria.</param>
        /// <param name="header">The header search criteria.</param>
        /// <param name="headeralt1">The headeralt1 search criteria.</param>
        /// <param name="headeralt2">The headeralt2 search criteria.</param>
        /// <param name="headeralt3">The headeralt3 search criteria.</param>
        /// <param name="tablename">The tablename search criteria.</param>
        /// <param name="ignoreifna">The ignoreifna search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportHeaderCollection Search(string name, string header, string headeralt1, string headeralt2, string headeralt3, string tablename, bool? ignoreifna, bool? isactive, string connectionName)
        {
            INImportHeaderCollection collection = new INImportHeaderCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportHeaderQueries.Search(name, header, headeralt1, headeralt2, headeralt3, tablename, ignoreifna, isactive), null);
                while (reader.Read())
                {
                    INImportHeader inimportheader = new INImportHeader((long)reader["ID"]);
                    inimportheader.ConnectionName = connectionName;
                    collection.Add(inimportheader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportHeader objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inimportheader objects in the database.
        /// </summary>
        /// <param name="name">The name search criteria.</param>
        /// <param name="header">The header search criteria.</param>
        /// <param name="headeralt1">The headeralt1 search criteria.</param>
        /// <param name="headeralt2">The headeralt2 search criteria.</param>
        /// <param name="headeralt3">The headeralt3 search criteria.</param>
        /// <param name="tablename">The tablename search criteria.</param>
        /// <param name="ignoreifna">The ignoreifna search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(string name, string header, string headeralt1, string headeralt2, string headeralt3, string tablename, bool? ignoreifna, bool? isactive, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INImportHeaderQueries.Search(name, header, headeralt1, headeralt2, headeralt3, tablename, ignoreifna, isactive), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportHeader objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inimportheader objects in the database.
        /// </summary>
        /// <param name="name">The name search criteria.</param>
        /// <param name="header">The header search criteria.</param>
        /// <param name="headeralt1">The headeralt1 search criteria.</param>
        /// <param name="headeralt2">The headeralt2 search criteria.</param>
        /// <param name="headeralt3">The headeralt3 search criteria.</param>
        /// <param name="tablename">The tablename search criteria.</param>
        /// <param name="ignoreifna">The ignoreifna search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportHeader SearchOne(string name, string header, string headeralt1, string headeralt2, string headeralt3, string tablename, bool? ignoreifna, bool? isactive, string connectionName)
        {
            INImportHeader inimportheader = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportHeaderQueries.Search(name, header, headeralt1, headeralt2, headeralt3, tablename, ignoreifna, isactive), null);
                if (reader.Read())
                {
                    inimportheader = new INImportHeader((long)reader["ID"]);
                    inimportheader.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportHeader objects in the database", ex);
            }
            return inimportheader;
        }
        #endregion
    }
}
