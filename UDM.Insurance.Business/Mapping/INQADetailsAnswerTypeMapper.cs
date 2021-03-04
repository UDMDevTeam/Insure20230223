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
    /// Contains methods to fill, save and delete inqadetailsanswertype objects.
    /// </summary>
    public partial class INQADetailsAnswerTypeMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inqadetailsanswertype object from the database.
        /// </summary>
        /// <param name="inqadetailsanswertype">The id of the inqadetailsanswertype object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inqadetailsanswertype object.</param>
        /// <returns>True if the inqadetailsanswertype object was deleted successfully, else false.</returns>
        internal static bool Delete(INQADetailsAnswerType inqadetailsanswertype)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inqadetailsanswertype.ConnectionName, INQADetailsAnswerTypeQueries.Delete(inqadetailsanswertype, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INQADetailsAnswerType object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inqadetailsanswertype object from the database.
        /// </summary>
        /// <param name="inqadetailsanswertype">The id of the inqadetailsanswertype object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inqadetailsanswertype history.</param>
        /// <returns>True if the inqadetailsanswertype history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INQADetailsAnswerType inqadetailsanswertype)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inqadetailsanswertype.ConnectionName, INQADetailsAnswerTypeQueries.DeleteHistory(inqadetailsanswertype, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INQADetailsAnswerType history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inqadetailsanswertype object from the database.
        /// </summary>
        /// <param name="inqadetailsanswertype">The id of the inqadetailsanswertype object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inqadetailsanswertype object.</param>
        /// <returns>True if the inqadetailsanswertype object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INQADetailsAnswerType inqadetailsanswertype)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inqadetailsanswertype.ConnectionName, INQADetailsAnswerTypeQueries.UnDelete(inqadetailsanswertype, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INQADetailsAnswerType object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inqadetailsanswertype object from the data reader.
        /// </summary>
        /// <param name="inqadetailsanswertype">The inqadetailsanswertype object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INQADetailsAnswerType inqadetailsanswertype, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inqadetailsanswertype.IsLoaded = true;
                    inqadetailsanswertype.Description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : (string)null;
                    inqadetailsanswertype.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INQADetailsAnswerType does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INQADetailsAnswerType object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inqadetailsanswertype object from the database.
        /// </summary>
        /// <param name="inqadetailsanswertype">The inqadetailsanswertype to fill.</param>
        internal static void Fill(INQADetailsAnswerType inqadetailsanswertype)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inqadetailsanswertype.ConnectionName, INQADetailsAnswerTypeQueries.Fill(inqadetailsanswertype, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inqadetailsanswertype, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INQADetailsAnswerType object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inqadetailsanswertype object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INQADetailsAnswerType inqadetailsanswertype)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inqadetailsanswertype.ConnectionName, INQADetailsAnswerTypeQueries.FillData(inqadetailsanswertype, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INQADetailsAnswerType object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inqadetailsanswertype object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inqadetailsanswertype">The inqadetailsanswertype to fill from history.</param>
        internal static void FillHistory(INQADetailsAnswerType inqadetailsanswertype, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inqadetailsanswertype.ConnectionName, INQADetailsAnswerTypeQueries.FillHistory(inqadetailsanswertype, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inqadetailsanswertype, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INQADetailsAnswerType object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inqadetailsanswertype objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INQADetailsAnswerTypeCollection List(bool showDeleted, string connectionName)
        {
            INQADetailsAnswerTypeCollection collection = new INQADetailsAnswerTypeCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INQADetailsAnswerTypeQueries.ListDeleted() : INQADetailsAnswerTypeQueries.List(), null);
                while (reader.Read())
                {
                    INQADetailsAnswerType inqadetailsanswertype = new INQADetailsAnswerType((long)reader["ID"]);
                    inqadetailsanswertype.ConnectionName = connectionName;
                    collection.Add(inqadetailsanswertype);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INQADetailsAnswerType objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inqadetailsanswertype objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INQADetailsAnswerTypeQueries.ListDeleted() : INQADetailsAnswerTypeQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INQADetailsAnswerType objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inqadetailsanswertype object from the database.
        /// </summary>
        /// <param name="inqadetailsanswertype">The inqadetailsanswertype to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INQADetailsAnswerType inqadetailsanswertype)
        {
            INQADetailsAnswerTypeCollection collection = new INQADetailsAnswerTypeCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inqadetailsanswertype.ConnectionName, INQADetailsAnswerTypeQueries.ListHistory(inqadetailsanswertype, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INQADetailsAnswerType in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inqadetailsanswertype object to the database.
        /// </summary>
        /// <param name="inqadetailsanswertype">The INQADetailsAnswerType object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inqadetailsanswertype was saved successfull, otherwise, false.</returns>
        internal static bool Save(INQADetailsAnswerType inqadetailsanswertype)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inqadetailsanswertype.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inqadetailsanswertype.ConnectionName, INQADetailsAnswerTypeQueries.Save(inqadetailsanswertype, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inqadetailsanswertype.ConnectionName, INQADetailsAnswerTypeQueries.Save(inqadetailsanswertype, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inqadetailsanswertype.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inqadetailsanswertype.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INQADetailsAnswerType object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inqadetailsanswertype objects in the database.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INQADetailsAnswerTypeCollection Search(string description, string connectionName)
        {
            INQADetailsAnswerTypeCollection collection = new INQADetailsAnswerTypeCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INQADetailsAnswerTypeQueries.Search(description), null);
                while (reader.Read())
                {
                    INQADetailsAnswerType inqadetailsanswertype = new INQADetailsAnswerType((long)reader["ID"]);
                    inqadetailsanswertype.ConnectionName = connectionName;
                    collection.Add(inqadetailsanswertype);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INQADetailsAnswerType objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inqadetailsanswertype objects in the database.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(string description, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INQADetailsAnswerTypeQueries.Search(description), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INQADetailsAnswerType objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inqadetailsanswertype objects in the database.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INQADetailsAnswerType SearchOne(string description, string connectionName)
        {
            INQADetailsAnswerType inqadetailsanswertype = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INQADetailsAnswerTypeQueries.Search(description), null);
                if (reader.Read())
                {
                    inqadetailsanswertype = new INQADetailsAnswerType((long)reader["ID"]);
                    inqadetailsanswertype.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INQADetailsAnswerType objects in the database", ex);
            }
            return inqadetailsanswertype;
        }
        #endregion
    }
}
