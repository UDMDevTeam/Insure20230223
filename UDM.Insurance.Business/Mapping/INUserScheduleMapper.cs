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
using INUserScheduleQueries = UDM.Insurance.Business.Queries.INUserScheduleQueries;

namespace UDM.Insurance.Business.Mapping
{
    /// <summary>
    /// Contains methods to fill, save and delete inuserschedule objects.
    /// </summary>
    public partial class INUserScheduleMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inuserschedule object from the database.
        /// </summary>
        /// <param name="inuserschedule">The id of the inuserschedule object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inuserschedule object.</param>
        /// <returns>True if the inuserschedule object was deleted successfully, else false.</returns>
        internal static bool Delete(INUserSchedule inuserschedule)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inuserschedule.ConnectionName, INUserScheduleQueries.Delete(inuserschedule, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INUserSchedule object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inuserschedule object from the database.
        /// </summary>
        /// <param name="inuserschedule">The id of the inuserschedule object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inuserschedule history.</param>
        /// <returns>True if the inuserschedule history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INUserSchedule inuserschedule)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inuserschedule.ConnectionName, INUserScheduleQueries.DeleteHistory(inuserschedule, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INUserSchedule history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inuserschedule object from the database.
        /// </summary>
        /// <param name="inuserschedule">The id of the inuserschedule object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inuserschedule object.</param>
        /// <returns>True if the inuserschedule object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INUserSchedule inuserschedule)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inuserschedule.ConnectionName, INUserScheduleQueries.UnDelete(inuserschedule, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INUserSchedule object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inuserschedule object from the data reader.
        /// </summary>
        /// <param name="inuserschedule">The inuserschedule object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INUserSchedule inuserschedule, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inuserschedule.IsLoaded = true;
                    inuserschedule.FKSystemID = reader["FKSystemID"] != DBNull.Value ? (long)reader["FKSystemID"] : (long?)null;
                    inuserschedule.FKUserID = reader["FKUserID"] != DBNull.Value ? (long)reader["FKUserID"] : (long?)null;
                    inuserschedule.FKINImportID = reader["FKINImportID"] != DBNull.Value ? (long)reader["FKINImportID"] : (long?)null;
                    inuserschedule.ScheduleID = reader["ScheduleID"] != DBNull.Value ? (Guid)reader["ScheduleID"] : (Guid?)null;
                    inuserschedule.Duration = reader["Duration"] != DBNull.Value ? (TimeSpan)reader["Duration"] : (TimeSpan?)null;
                    inuserschedule.Start = reader["Start"] != DBNull.Value ? (DateTime)reader["Start"] : (DateTime?)null;
                    inuserschedule.End = reader["End"] != DBNull.Value ? (DateTime)reader["End"] : (DateTime?)null;
                    inuserschedule.Subject = reader["Subject"] != DBNull.Value ? (string)reader["Subject"] : (string)null;
                    inuserschedule.Description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : (string)null;
                    inuserschedule.Location = reader["Location"] != DBNull.Value ? (string)reader["Location"] : (string)null;
                    inuserschedule.Categories = reader["Categories"] != DBNull.Value ? (string)reader["Categories"] : (string)null;
                    inuserschedule.ReminderEnabled = reader["ReminderEnabled"] != DBNull.Value ? (bool)reader["ReminderEnabled"] : (bool?)null;
                    inuserschedule.ReminderInterval = reader["ReminderInterval"] != DBNull.Value ? (long)reader["ReminderInterval"] : (long?)null;
                    inuserschedule.StampDate = (DateTime)reader["StampDate"];
                    inuserschedule.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INUserSchedule does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INUserSchedule object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inuserschedule object from the database.
        /// </summary>
        /// <param name="inuserschedule">The inuserschedule to fill.</param>
        internal static void Fill(INUserSchedule inuserschedule)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inuserschedule.ConnectionName, INUserScheduleQueries.Fill(inuserschedule, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inuserschedule, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INUserSchedule object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inuserschedule object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INUserSchedule inuserschedule)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inuserschedule.ConnectionName, INUserScheduleQueries.FillData(inuserschedule, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INUserSchedule object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inuserschedule object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inuserschedule">The inuserschedule to fill from history.</param>
        internal static void FillHistory(INUserSchedule inuserschedule, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inuserschedule.ConnectionName, INUserScheduleQueries.FillHistory(inuserschedule, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inuserschedule, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INUserSchedule object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inuserschedule objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INUserScheduleCollection List(bool showDeleted, string connectionName)
        {
            INUserScheduleCollection collection = new INUserScheduleCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INUserScheduleQueries.ListDeleted() : INUserScheduleQueries.List(), null);
                while (reader.Read())
                {
                    INUserSchedule inuserschedule = new INUserSchedule((long)reader["ID"]);
                    inuserschedule.ConnectionName = connectionName;
                    collection.Add(inuserschedule);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INUserSchedule objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inuserschedule objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INUserScheduleQueries.ListDeleted() : INUserScheduleQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INUserSchedule objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inuserschedule object from the database.
        /// </summary>
        /// <param name="inuserschedule">The inuserschedule to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INUserSchedule inuserschedule)
        {
            INUserScheduleCollection collection = new INUserScheduleCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inuserschedule.ConnectionName, INUserScheduleQueries.ListHistory(inuserschedule, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INUserSchedule in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inuserschedule object to the database.
        /// </summary>
        /// <param name="inuserschedule">The INUserSchedule object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inuserschedule was saved successfull, otherwise, false.</returns>
        internal static bool Save(INUserSchedule inuserschedule)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inuserschedule.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inuserschedule.ConnectionName, INUserScheduleQueries.Save(inuserschedule, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inuserschedule.ConnectionName, INUserScheduleQueries.Save(inuserschedule, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inuserschedule.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inuserschedule.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INUserSchedule object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inuserschedule objects in the database.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="scheduleid">The scheduleid search criteria.</param>
        /// <param name="duration">The duration search criteria.</param>
        /// <param name="start">The start search criteria.</param>
        /// <param name="end">The end search criteria.</param>
        /// <param name="subject">The subject search criteria.</param>
        /// <param name="description">The description search criteria.</param>
        /// <param name="location">The location search criteria.</param>
        /// <param name="categories">The categories search criteria.</param>
        /// <param name="reminderenabled">The reminderenabled search criteria.</param>
        /// <param name="reminderinterval">The reminderinterval search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INUserScheduleCollection Search(long? fksystemid, long? fkuserid, long? fkinimportid, Guid? scheduleid, TimeSpan? duration, DateTime? start, DateTime? end, string subject, string description, string location, string categories, bool? reminderenabled, long? reminderinterval, string connectionName)
        {
            INUserScheduleCollection collection = new INUserScheduleCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INUserScheduleQueries.Search(fksystemid, fkuserid, fkinimportid, scheduleid, duration, start, end, subject, description, location, categories, reminderenabled, reminderinterval), null);
                while (reader.Read())
                {
                    INUserSchedule inuserschedule = new INUserSchedule((long)reader["ID"]);
                    inuserschedule.ConnectionName = connectionName;
                    collection.Add(inuserschedule);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INUserSchedule objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inuserschedule objects in the database.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="scheduleid">The scheduleid search criteria.</param>
        /// <param name="duration">The duration search criteria.</param>
        /// <param name="start">The start search criteria.</param>
        /// <param name="end">The end search criteria.</param>
        /// <param name="subject">The subject search criteria.</param>
        /// <param name="description">The description search criteria.</param>
        /// <param name="location">The location search criteria.</param>
        /// <param name="categories">The categories search criteria.</param>
        /// <param name="reminderenabled">The reminderenabled search criteria.</param>
        /// <param name="reminderinterval">The reminderinterval search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fksystemid, long? fkuserid, long? fkinimportid, Guid? scheduleid, TimeSpan? duration, DateTime? start, DateTime? end, string subject, string description, string location, string categories, bool? reminderenabled, long? reminderinterval, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INUserScheduleQueries.Search(fksystemid, fkuserid, fkinimportid, scheduleid, duration, start, end, subject, description, location, categories, reminderenabled, reminderinterval), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INUserSchedule objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inuserschedule objects in the database.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="scheduleid">The scheduleid search criteria.</param>
        /// <param name="duration">The duration search criteria.</param>
        /// <param name="start">The start search criteria.</param>
        /// <param name="end">The end search criteria.</param>
        /// <param name="subject">The subject search criteria.</param>
        /// <param name="description">The description search criteria.</param>
        /// <param name="location">The location search criteria.</param>
        /// <param name="categories">The categories search criteria.</param>
        /// <param name="reminderenabled">The reminderenabled search criteria.</param>
        /// <param name="reminderinterval">The reminderinterval search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INUserSchedule SearchOne(long? fksystemid, long? fkuserid, long? fkinimportid, Guid? scheduleid, TimeSpan? duration, DateTime? start, DateTime? end, string subject, string description, string location, string categories, bool? reminderenabled, long? reminderinterval, string connectionName)
        {
            INUserSchedule inuserschedule = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INUserScheduleQueries.Search(fksystemid, fkuserid, fkinimportid, scheduleid, duration, start, end, subject, description, location, categories, reminderenabled, reminderinterval), null);
                if (reader.Read())
                {
                    inuserschedule = new INUserSchedule((long)reader["ID"]);
                    inuserschedule.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INUserSchedule objects in the database", ex);
            }
            return inuserschedule;
        }
        #endregion
    }
}
