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
    /// Contains methods to fill, save and delete inusertypeleadstatus objects.
    /// </summary>
    public partial class INUserTypeLeadStatusMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inusertypeleadstatus object from the database.
        /// </summary>
        /// <param name="inusertypeleadstatus">The id of the inusertypeleadstatus object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inusertypeleadstatus object.</param>
        /// <returns>True if the inusertypeleadstatus object was deleted successfully, else false.</returns>
        internal static bool Delete(INUserTypeLeadStatus inusertypeleadstatus)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inusertypeleadstatus.ConnectionName, INUserTypeLeadStatusQueries.Delete(inusertypeleadstatus, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INUserTypeLeadStatus object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inusertypeleadstatus object from the database.
        /// </summary>
        /// <param name="inusertypeleadstatus">The id of the inusertypeleadstatus object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inusertypeleadstatus history.</param>
        /// <returns>True if the inusertypeleadstatus history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INUserTypeLeadStatus inusertypeleadstatus)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inusertypeleadstatus.ConnectionName, INUserTypeLeadStatusQueries.DeleteHistory(inusertypeleadstatus, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INUserTypeLeadStatus history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inusertypeleadstatus object from the database.
        /// </summary>
        /// <param name="inusertypeleadstatus">The id of the inusertypeleadstatus object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inusertypeleadstatus object.</param>
        /// <returns>True if the inusertypeleadstatus object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INUserTypeLeadStatus inusertypeleadstatus)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inusertypeleadstatus.ConnectionName, INUserTypeLeadStatusQueries.UnDelete(inusertypeleadstatus, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INUserTypeLeadStatus object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inusertypeleadstatus object from the data reader.
        /// </summary>
        /// <param name="inusertypeleadstatus">The inusertypeleadstatus object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INUserTypeLeadStatus inusertypeleadstatus, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inusertypeleadstatus.IsLoaded = true;
                    inusertypeleadstatus.FKUserTypeID = reader["FKUserTypeID"] != DBNull.Value ? (long)reader["FKUserTypeID"] : 0;
                    inusertypeleadstatus.FKLeadStatusID = reader["FKLeadStatusID"] != DBNull.Value ? (long)reader["FKLeadStatusID"] : 0;
                    inusertypeleadstatus.StampDate = (DateTime)reader["StampDate"];
                    inusertypeleadstatus.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INUserTypeLeadStatus does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INUserTypeLeadStatus object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inusertypeleadstatus object from the database.
        /// </summary>
        /// <param name="inusertypeleadstatus">The inusertypeleadstatus to fill.</param>
        internal static void Fill(INUserTypeLeadStatus inusertypeleadstatus)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inusertypeleadstatus.ConnectionName, INUserTypeLeadStatusQueries.Fill(inusertypeleadstatus, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inusertypeleadstatus, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INUserTypeLeadStatus object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inusertypeleadstatus object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INUserTypeLeadStatus inusertypeleadstatus)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inusertypeleadstatus.ConnectionName, INUserTypeLeadStatusQueries.FillData(inusertypeleadstatus, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INUserTypeLeadStatus object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inusertypeleadstatus object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inusertypeleadstatus">The inusertypeleadstatus to fill from history.</param>
        internal static void FillHistory(INUserTypeLeadStatus inusertypeleadstatus, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inusertypeleadstatus.ConnectionName, INUserTypeLeadStatusQueries.FillHistory(inusertypeleadstatus, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inusertypeleadstatus, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INUserTypeLeadStatus object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inusertypeleadstatus objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INUserTypeLeadStatusCollection List(bool showDeleted, string connectionName)
        {
            INUserTypeLeadStatusCollection collection = new INUserTypeLeadStatusCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INUserTypeLeadStatusQueries.ListDeleted() : INUserTypeLeadStatusQueries.List(), null);
                while (reader.Read())
                {
                    INUserTypeLeadStatus inusertypeleadstatus = new INUserTypeLeadStatus((long)reader["ID"]);
                    inusertypeleadstatus.ConnectionName = connectionName;
                    collection.Add(inusertypeleadstatus);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INUserTypeLeadStatus objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inusertypeleadstatus objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INUserTypeLeadStatusQueries.ListDeleted() : INUserTypeLeadStatusQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INUserTypeLeadStatus objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inusertypeleadstatus object from the database.
        /// </summary>
        /// <param name="inusertypeleadstatus">The inusertypeleadstatus to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INUserTypeLeadStatus inusertypeleadstatus)
        {
            INUserTypeLeadStatusCollection collection = new INUserTypeLeadStatusCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inusertypeleadstatus.ConnectionName, INUserTypeLeadStatusQueries.ListHistory(inusertypeleadstatus, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INUserTypeLeadStatus in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inusertypeleadstatus object to the database.
        /// </summary>
        /// <param name="inusertypeleadstatus">The INUserTypeLeadStatus object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inusertypeleadstatus was saved successfull, otherwise, false.</returns>
        internal static bool Save(INUserTypeLeadStatus inusertypeleadstatus)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inusertypeleadstatus.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inusertypeleadstatus.ConnectionName, INUserTypeLeadStatusQueries.Save(inusertypeleadstatus, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inusertypeleadstatus.ConnectionName, INUserTypeLeadStatusQueries.Save(inusertypeleadstatus, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inusertypeleadstatus.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inusertypeleadstatus.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INUserTypeLeadStatus object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inusertypeleadstatus objects in the database.
        /// </summary>
        /// <param name="fkusertypeid">The fkusertypeid search criteria.</param>
        /// <param name="fkleadstatusid">The fkleadstatusid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INUserTypeLeadStatusCollection Search(long? fkusertypeid, long? fkleadstatusid, string connectionName)
        {
            INUserTypeLeadStatusCollection collection = new INUserTypeLeadStatusCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INUserTypeLeadStatusQueries.Search(fkusertypeid, fkleadstatusid), null);
                while (reader.Read())
                {
                    INUserTypeLeadStatus inusertypeleadstatus = new INUserTypeLeadStatus((long)reader["ID"]);
                    inusertypeleadstatus.ConnectionName = connectionName;
                    collection.Add(inusertypeleadstatus);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INUserTypeLeadStatus objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inusertypeleadstatus objects in the database.
        /// </summary>
        /// <param name="fkusertypeid">The fkusertypeid search criteria.</param>
        /// <param name="fkleadstatusid">The fkleadstatusid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkusertypeid, long? fkleadstatusid, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INUserTypeLeadStatusQueries.Search(fkusertypeid, fkleadstatusid), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INUserTypeLeadStatus objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inusertypeleadstatus objects in the database.
        /// </summary>
        /// <param name="fkusertypeid">The fkusertypeid search criteria.</param>
        /// <param name="fkleadstatusid">The fkleadstatusid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INUserTypeLeadStatus SearchOne(long? fkusertypeid, long? fkleadstatusid, string connectionName)
        {
            INUserTypeLeadStatus inusertypeleadstatus = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INUserTypeLeadStatusQueries.Search(fkusertypeid, fkleadstatusid), null);
                if (reader.Read())
                {
                    inusertypeleadstatus = new INUserTypeLeadStatus((long)reader["ID"]);
                    inusertypeleadstatus.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INUserTypeLeadStatus objects in the database", ex);
            }
            return inusertypeleadstatus;
        }
        #endregion
    }
}
