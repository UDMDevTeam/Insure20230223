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
    /// Contains methods to fill, save and delete inchild objects.
    /// </summary>
    public partial class INChildMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inchild object from the database.
        /// </summary>
        /// <param name="inchild">The id of the inchild object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inchild object.</param>
        /// <returns>True if the inchild object was deleted successfully, else false.</returns>
        internal static bool Delete(INChild inchild)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inchild.ConnectionName, INChildQueries.Delete(inchild, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INChild object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inchild object from the database.
        /// </summary>
        /// <param name="inchild">The id of the inchild object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inchild history.</param>
        /// <returns>True if the inchild history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INChild inchild)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inchild.ConnectionName, INChildQueries.DeleteHistory(inchild, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INChild history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inchild object from the database.
        /// </summary>
        /// <param name="inchild">The id of the inchild object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inchild object.</param>
        /// <returns>True if the inchild object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INChild inchild)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inchild.ConnectionName, INChildQueries.UnDelete(inchild, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INChild object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inchild object from the data reader.
        /// </summary>
        /// <param name="inchild">The inchild object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INChild inchild, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inchild.IsLoaded = true;
                    inchild.FirstName = reader["FirstName"] != DBNull.Value ? (string)reader["FirstName"] : (string)null;
                    inchild.DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime)reader["DateOfBirth"] : (DateTime?)null;
                    inchild.ToDeleteID = reader["ToDeleteID"] != DBNull.Value ? (long)reader["ToDeleteID"] : (long?)null;
                    inchild.StampDate = (DateTime)reader["StampDate"];
                    inchild.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INChild does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INChild object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inchild object from the database.
        /// </summary>
        /// <param name="inchild">The inchild to fill.</param>
        internal static void Fill(INChild inchild)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inchild.ConnectionName, INChildQueries.Fill(inchild, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inchild, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INChild object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inchild object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INChild inchild)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inchild.ConnectionName, INChildQueries.FillData(inchild, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INChild object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inchild object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inchild">The inchild to fill from history.</param>
        internal static void FillHistory(INChild inchild, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inchild.ConnectionName, INChildQueries.FillHistory(inchild, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inchild, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INChild object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inchild objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INChildCollection List(bool showDeleted, string connectionName)
        {
            INChildCollection collection = new INChildCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INChildQueries.ListDeleted() : INChildQueries.List(), null);
                while (reader.Read())
                {
                    INChild inchild = new INChild((long)reader["ID"]);
                    inchild.ConnectionName = connectionName;
                    collection.Add(inchild);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INChild objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inchild objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INChildQueries.ListDeleted() : INChildQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INChild objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inchild object from the database.
        /// </summary>
        /// <param name="inchild">The inchild to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INChild inchild)
        {
            INChildCollection collection = new INChildCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inchild.ConnectionName, INChildQueries.ListHistory(inchild, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INChild in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inchild object to the database.
        /// </summary>
        /// <param name="inchild">The INChild object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inchild was saved successfull, otherwise, false.</returns>
        internal static bool Save(INChild inchild)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inchild.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inchild.ConnectionName, INChildQueries.Save(inchild, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inchild.ConnectionName, INChildQueries.Save(inchild, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inchild.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inchild.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INChild object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inchild objects in the database.
        /// </summary>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="dateofbirth">The dateofbirth search criteria.</param>
        /// <param name="todeleteid">The todeleteid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INChildCollection Search(string firstname, DateTime? dateofbirth, long? todeleteid, string connectionName)
        {
            INChildCollection collection = new INChildCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INChildQueries.Search(firstname, dateofbirth, todeleteid), null);
                while (reader.Read())
                {
                    INChild inchild = new INChild((long)reader["ID"]);
                    inchild.ConnectionName = connectionName;
                    collection.Add(inchild);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INChild objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inchild objects in the database.
        /// </summary>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="dateofbirth">The dateofbirth search criteria.</param>
        /// <param name="todeleteid">The todeleteid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(string firstname, DateTime? dateofbirth, long? todeleteid, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INChildQueries.Search(firstname, dateofbirth, todeleteid), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INChild objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inchild objects in the database.
        /// </summary>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="dateofbirth">The dateofbirth search criteria.</param>
        /// <param name="todeleteid">The todeleteid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INChild SearchOne(string firstname, DateTime? dateofbirth, long? todeleteid, string connectionName)
        {
            INChild inchild = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INChildQueries.Search(firstname, dateofbirth, todeleteid), null);
                if (reader.Read())
                {
                    inchild = new INChild((long)reader["ID"]);
                    inchild.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INChild objects in the database", ex);
            }
            return inchild;
        }
        #endregion
    }
}
