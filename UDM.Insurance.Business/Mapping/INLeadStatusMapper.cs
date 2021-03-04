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
    /// Contains methods to fill, save and delete inleadstatus objects.
    /// </summary>
    public partial class INLeadStatusMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inleadstatus object from the database.
        /// </summary>
        /// <param name="inleadstatus">The id of the inleadstatus object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inleadstatus object.</param>
        /// <returns>True if the inleadstatus object was deleted successfully, else false.</returns>
        internal static bool Delete(INLeadStatus inleadstatus)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inleadstatus.ConnectionName, INLeadStatusQueries.Delete(inleadstatus, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INLeadStatus object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inleadstatus object from the database.
        /// </summary>
        /// <param name="inleadstatus">The id of the inleadstatus object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inleadstatus history.</param>
        /// <returns>True if the inleadstatus history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INLeadStatus inleadstatus)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inleadstatus.ConnectionName, INLeadStatusQueries.DeleteHistory(inleadstatus, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INLeadStatus history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inleadstatus object from the database.
        /// </summary>
        /// <param name="inleadstatus">The id of the inleadstatus object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inleadstatus object.</param>
        /// <returns>True if the inleadstatus object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INLeadStatus inleadstatus)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inleadstatus.ConnectionName, INLeadStatusQueries.UnDelete(inleadstatus, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INLeadStatus object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inleadstatus object from the data reader.
        /// </summary>
        /// <param name="inleadstatus">The inleadstatus object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INLeadStatus inleadstatus, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inleadstatus.IsLoaded = true;
                    inleadstatus.Description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : (string)null;
                    inleadstatus.IsActive = reader["IsActive"] != DBNull.Value ? (bool)reader["IsActive"] : (bool?)null;
                    inleadstatus.CodeNumber = reader["CodeNumber"] != DBNull.Value ? (string)reader["CodeNumber"] : (string)null;
                    inleadstatus.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INLeadStatus does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INLeadStatus object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inleadstatus object from the database.
        /// </summary>
        /// <param name="inleadstatus">The inleadstatus to fill.</param>
        internal static void Fill(INLeadStatus inleadstatus)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inleadstatus.ConnectionName, INLeadStatusQueries.Fill(inleadstatus, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inleadstatus, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INLeadStatus object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inleadstatus object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INLeadStatus inleadstatus)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inleadstatus.ConnectionName, INLeadStatusQueries.FillData(inleadstatus, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INLeadStatus object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inleadstatus object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inleadstatus">The inleadstatus to fill from history.</param>
        internal static void FillHistory(INLeadStatus inleadstatus, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inleadstatus.ConnectionName, INLeadStatusQueries.FillHistory(inleadstatus, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inleadstatus, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INLeadStatus object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inleadstatus objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INLeadStatusCollection List(bool showDeleted, string connectionName)
        {
            INLeadStatusCollection collection = new INLeadStatusCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INLeadStatusQueries.ListDeleted() : INLeadStatusQueries.List(), null);
                while (reader.Read())
                {
                    INLeadStatus inleadstatus = new INLeadStatus((long)reader["ID"]);
                    inleadstatus.ConnectionName = connectionName;
                    collection.Add(inleadstatus);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INLeadStatus objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inleadstatus objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INLeadStatusQueries.ListDeleted() : INLeadStatusQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INLeadStatus objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inleadstatus object from the database.
        /// </summary>
        /// <param name="inleadstatus">The inleadstatus to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INLeadStatus inleadstatus)
        {
            INLeadStatusCollection collection = new INLeadStatusCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inleadstatus.ConnectionName, INLeadStatusQueries.ListHistory(inleadstatus, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INLeadStatus in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inleadstatus object to the database.
        /// </summary>
        /// <param name="inleadstatus">The INLeadStatus object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inleadstatus was saved successfull, otherwise, false.</returns>
        internal static bool Save(INLeadStatus inleadstatus)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inleadstatus.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inleadstatus.ConnectionName, INLeadStatusQueries.Save(inleadstatus, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inleadstatus.ConnectionName, INLeadStatusQueries.Save(inleadstatus, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inleadstatus.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inleadstatus.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INLeadStatus object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inleadstatus objects in the database.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="codenumber">The codenumber search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INLeadStatusCollection Search(string description, bool? isactive, string codenumber, string connectionName)
        {
            INLeadStatusCollection collection = new INLeadStatusCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INLeadStatusQueries.Search(description, isactive, codenumber), null);
                while (reader.Read())
                {
                    INLeadStatus inleadstatus = new INLeadStatus((long)reader["ID"]);
                    inleadstatus.ConnectionName = connectionName;
                    collection.Add(inleadstatus);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INLeadStatus objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inleadstatus objects in the database.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="codenumber">The codenumber search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(string description, bool? isactive, string codenumber, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INLeadStatusQueries.Search(description, isactive, codenumber), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INLeadStatus objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inleadstatus objects in the database.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="codenumber">The codenumber search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INLeadStatus SearchOne(string description, bool? isactive, string codenumber, string connectionName)
        {
            INLeadStatus inleadstatus = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INLeadStatusQueries.Search(description, isactive, codenumber), null);
                if (reader.Read())
                {
                    inleadstatus = new INLeadStatus((long)reader["ID"]);
                    inleadstatus.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INLeadStatus objects in the database", ex);
            }
            return inleadstatus;
        }
        #endregion
    }
}
