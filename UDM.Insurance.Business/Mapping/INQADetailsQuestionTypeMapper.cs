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
    /// Contains methods to fill, save and delete inqadetailsquestiontype objects.
    /// </summary>
    public partial class INQADetailsQuestionTypeMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inqadetailsquestiontype object from the database.
        /// </summary>
        /// <param name="inqadetailsquestiontype">The id of the inqadetailsquestiontype object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inqadetailsquestiontype object.</param>
        /// <returns>True if the inqadetailsquestiontype object was deleted successfully, else false.</returns>
        internal static bool Delete(INQADetailsQuestionType inqadetailsquestiontype)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inqadetailsquestiontype.ConnectionName, INQADetailsQuestionTypeQueries.Delete(inqadetailsquestiontype, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INQADetailsQuestionType object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inqadetailsquestiontype object from the database.
        /// </summary>
        /// <param name="inqadetailsquestiontype">The id of the inqadetailsquestiontype object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inqadetailsquestiontype history.</param>
        /// <returns>True if the inqadetailsquestiontype history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INQADetailsQuestionType inqadetailsquestiontype)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inqadetailsquestiontype.ConnectionName, INQADetailsQuestionTypeQueries.DeleteHistory(inqadetailsquestiontype, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INQADetailsQuestionType history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inqadetailsquestiontype object from the database.
        /// </summary>
        /// <param name="inqadetailsquestiontype">The id of the inqadetailsquestiontype object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inqadetailsquestiontype object.</param>
        /// <returns>True if the inqadetailsquestiontype object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INQADetailsQuestionType inqadetailsquestiontype)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inqadetailsquestiontype.ConnectionName, INQADetailsQuestionTypeQueries.UnDelete(inqadetailsquestiontype, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INQADetailsQuestionType object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inqadetailsquestiontype object from the data reader.
        /// </summary>
        /// <param name="inqadetailsquestiontype">The inqadetailsquestiontype object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INQADetailsQuestionType inqadetailsquestiontype, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inqadetailsquestiontype.IsLoaded = true;
                    inqadetailsquestiontype.Description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : (string)null;
                    inqadetailsquestiontype.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INQADetailsQuestionType does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INQADetailsQuestionType object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inqadetailsquestiontype object from the database.
        /// </summary>
        /// <param name="inqadetailsquestiontype">The inqadetailsquestiontype to fill.</param>
        internal static void Fill(INQADetailsQuestionType inqadetailsquestiontype)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inqadetailsquestiontype.ConnectionName, INQADetailsQuestionTypeQueries.Fill(inqadetailsquestiontype, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inqadetailsquestiontype, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INQADetailsQuestionType object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inqadetailsquestiontype object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INQADetailsQuestionType inqadetailsquestiontype)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inqadetailsquestiontype.ConnectionName, INQADetailsQuestionTypeQueries.FillData(inqadetailsquestiontype, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INQADetailsQuestionType object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inqadetailsquestiontype object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inqadetailsquestiontype">The inqadetailsquestiontype to fill from history.</param>
        internal static void FillHistory(INQADetailsQuestionType inqadetailsquestiontype, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inqadetailsquestiontype.ConnectionName, INQADetailsQuestionTypeQueries.FillHistory(inqadetailsquestiontype, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inqadetailsquestiontype, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INQADetailsQuestionType object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inqadetailsquestiontype objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INQADetailsQuestionTypeCollection List(bool showDeleted, string connectionName)
        {
            INQADetailsQuestionTypeCollection collection = new INQADetailsQuestionTypeCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INQADetailsQuestionTypeQueries.ListDeleted() : INQADetailsQuestionTypeQueries.List(), null);
                while (reader.Read())
                {
                    INQADetailsQuestionType inqadetailsquestiontype = new INQADetailsQuestionType((long)reader["ID"]);
                    inqadetailsquestiontype.ConnectionName = connectionName;
                    collection.Add(inqadetailsquestiontype);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INQADetailsQuestionType objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inqadetailsquestiontype objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INQADetailsQuestionTypeQueries.ListDeleted() : INQADetailsQuestionTypeQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INQADetailsQuestionType objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inqadetailsquestiontype object from the database.
        /// </summary>
        /// <param name="inqadetailsquestiontype">The inqadetailsquestiontype to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INQADetailsQuestionType inqadetailsquestiontype)
        {
            INQADetailsQuestionTypeCollection collection = new INQADetailsQuestionTypeCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inqadetailsquestiontype.ConnectionName, INQADetailsQuestionTypeQueries.ListHistory(inqadetailsquestiontype, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INQADetailsQuestionType in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inqadetailsquestiontype object to the database.
        /// </summary>
        /// <param name="inqadetailsquestiontype">The INQADetailsQuestionType object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inqadetailsquestiontype was saved successfull, otherwise, false.</returns>
        internal static bool Save(INQADetailsQuestionType inqadetailsquestiontype)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inqadetailsquestiontype.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inqadetailsquestiontype.ConnectionName, INQADetailsQuestionTypeQueries.Save(inqadetailsquestiontype, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inqadetailsquestiontype.ConnectionName, INQADetailsQuestionTypeQueries.Save(inqadetailsquestiontype, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inqadetailsquestiontype.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inqadetailsquestiontype.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INQADetailsQuestionType object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inqadetailsquestiontype objects in the database.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INQADetailsQuestionTypeCollection Search(string description, string connectionName)
        {
            INQADetailsQuestionTypeCollection collection = new INQADetailsQuestionTypeCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INQADetailsQuestionTypeQueries.Search(description), null);
                while (reader.Read())
                {
                    INQADetailsQuestionType inqadetailsquestiontype = new INQADetailsQuestionType((long)reader["ID"]);
                    inqadetailsquestiontype.ConnectionName = connectionName;
                    collection.Add(inqadetailsquestiontype);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INQADetailsQuestionType objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inqadetailsquestiontype objects in the database.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(string description, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INQADetailsQuestionTypeQueries.Search(description), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INQADetailsQuestionType objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inqadetailsquestiontype objects in the database.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INQADetailsQuestionType SearchOne(string description, string connectionName)
        {
            INQADetailsQuestionType inqadetailsquestiontype = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INQADetailsQuestionTypeQueries.Search(description), null);
                if (reader.Read())
                {
                    inqadetailsquestiontype = new INQADetailsQuestionType((long)reader["ID"]);
                    inqadetailsquestiontype.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INQADetailsQuestionType objects in the database", ex);
            }
            return inqadetailsquestiontype;
        }
        #endregion
    }
}
