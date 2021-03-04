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
    /// Contains methods to fill, save and delete inqadetailsinimport objects.
    /// </summary>
    public partial class INQADetailsINImportMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inqadetailsinimport object from the database.
        /// </summary>
        /// <param name="inqadetailsinimport">The id of the inqadetailsinimport object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inqadetailsinimport object.</param>
        /// <returns>True if the inqadetailsinimport object was deleted successfully, else false.</returns>
        internal static bool Delete(INQADetailsINImport inqadetailsinimport)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inqadetailsinimport.ConnectionName, INQADetailsINImportQueries.Delete(inqadetailsinimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INQADetailsINImport object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inqadetailsinimport object from the database.
        /// </summary>
        /// <param name="inqadetailsinimport">The id of the inqadetailsinimport object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inqadetailsinimport history.</param>
        /// <returns>True if the inqadetailsinimport history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INQADetailsINImport inqadetailsinimport)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inqadetailsinimport.ConnectionName, INQADetailsINImportQueries.DeleteHistory(inqadetailsinimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INQADetailsINImport history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inqadetailsinimport object from the database.
        /// </summary>
        /// <param name="inqadetailsinimport">The id of the inqadetailsinimport object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inqadetailsinimport object.</param>
        /// <returns>True if the inqadetailsinimport object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INQADetailsINImport inqadetailsinimport)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inqadetailsinimport.ConnectionName, INQADetailsINImportQueries.UnDelete(inqadetailsinimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INQADetailsINImport object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inqadetailsinimport object from the data reader.
        /// </summary>
        /// <param name="inqadetailsinimport">The inqadetailsinimport object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INQADetailsINImport inqadetailsinimport, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inqadetailsinimport.IsLoaded = true;
                    inqadetailsinimport.FKImportID = reader["FKImportID"] != DBNull.Value ? (long)reader["FKImportID"] : (long?)null;
                    inqadetailsinimport.FKAssessorID = reader["FKAssessorID"] != DBNull.Value ? (long)reader["FKAssessorID"] : (long?)null;
                    inqadetailsinimport.StampDate = (DateTime)reader["StampDate"];
                    inqadetailsinimport.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INQADetailsINImport does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INQADetailsINImport object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inqadetailsinimport object from the database.
        /// </summary>
        /// <param name="inqadetailsinimport">The inqadetailsinimport to fill.</param>
        internal static void Fill(INQADetailsINImport inqadetailsinimport)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inqadetailsinimport.ConnectionName, INQADetailsINImportQueries.Fill(inqadetailsinimport, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inqadetailsinimport, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INQADetailsINImport object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inqadetailsinimport object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INQADetailsINImport inqadetailsinimport)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inqadetailsinimport.ConnectionName, INQADetailsINImportQueries.FillData(inqadetailsinimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INQADetailsINImport object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inqadetailsinimport object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inqadetailsinimport">The inqadetailsinimport to fill from history.</param>
        internal static void FillHistory(INQADetailsINImport inqadetailsinimport, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inqadetailsinimport.ConnectionName, INQADetailsINImportQueries.FillHistory(inqadetailsinimport, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inqadetailsinimport, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INQADetailsINImport object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inqadetailsinimport objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INQADetailsINImportCollection List(bool showDeleted, string connectionName)
        {
            INQADetailsINImportCollection collection = new INQADetailsINImportCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INQADetailsINImportQueries.ListDeleted() : INQADetailsINImportQueries.List(), null);
                while (reader.Read())
                {
                    INQADetailsINImport inqadetailsinimport = new INQADetailsINImport((long)reader["ID"]);
                    inqadetailsinimport.ConnectionName = connectionName;
                    collection.Add(inqadetailsinimport);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INQADetailsINImport objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inqadetailsinimport objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INQADetailsINImportQueries.ListDeleted() : INQADetailsINImportQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INQADetailsINImport objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inqadetailsinimport object from the database.
        /// </summary>
        /// <param name="inqadetailsinimport">The inqadetailsinimport to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INQADetailsINImport inqadetailsinimport)
        {
            INQADetailsINImportCollection collection = new INQADetailsINImportCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inqadetailsinimport.ConnectionName, INQADetailsINImportQueries.ListHistory(inqadetailsinimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INQADetailsINImport in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inqadetailsinimport object to the database.
        /// </summary>
        /// <param name="inqadetailsinimport">The INQADetailsINImport object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inqadetailsinimport was saved successfull, otherwise, false.</returns>
        internal static bool Save(INQADetailsINImport inqadetailsinimport)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inqadetailsinimport.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inqadetailsinimport.ConnectionName, INQADetailsINImportQueries.Save(inqadetailsinimport, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inqadetailsinimport.ConnectionName, INQADetailsINImportQueries.Save(inqadetailsinimport, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inqadetailsinimport.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inqadetailsinimport.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INQADetailsINImport object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inqadetailsinimport objects in the database.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkassessorid">The fkassessorid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INQADetailsINImportCollection Search(long? fkimportid, long? fkassessorid, string connectionName)
        {
            INQADetailsINImportCollection collection = new INQADetailsINImportCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INQADetailsINImportQueries.Search(fkimportid, fkassessorid), null);
                while (reader.Read())
                {
                    INQADetailsINImport inqadetailsinimport = new INQADetailsINImport((long)reader["ID"]);
                    inqadetailsinimport.ConnectionName = connectionName;
                    collection.Add(inqadetailsinimport);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INQADetailsINImport objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inqadetailsinimport objects in the database.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkassessorid">The fkassessorid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkimportid, long? fkassessorid, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INQADetailsINImportQueries.Search(fkimportid, fkassessorid), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INQADetailsINImport objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inqadetailsinimport objects in the database.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkassessorid">The fkassessorid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INQADetailsINImport SearchOne(long? fkimportid, long? fkassessorid, string connectionName)
        {
            INQADetailsINImport inqadetailsinimport = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INQADetailsINImportQueries.Search(fkimportid, fkassessorid), null);
                if (reader.Read())
                {
                    inqadetailsinimport = new INQADetailsINImport((long)reader["ID"]);
                    inqadetailsinimport.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INQADetailsINImport objects in the database", ex);
            }
            return inqadetailsinimport;
        }
        #endregion
    }
}
