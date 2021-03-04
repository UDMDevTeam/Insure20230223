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
    /// Contains methods to fill, save and delete userhours objects.
    /// </summary>
    public partial class UserHoursMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) userhours object from the database.
        /// </summary>
        /// <param name="userhours">The id of the userhours object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the userhours object.</param>
        /// <returns>True if the userhours object was deleted successfully, else false.</returns>
        internal static bool Delete(UserHours userhours)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(userhours.ConnectionName, UserHoursQueries.Delete(userhours, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a UserHours object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) userhours object from the database.
        /// </summary>
        /// <param name="userhours">The id of the userhours object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the userhours history.</param>
        /// <returns>True if the userhours history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(UserHours userhours)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(userhours.ConnectionName, UserHoursQueries.DeleteHistory(userhours, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete UserHours history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) userhours object from the database.
        /// </summary>
        /// <param name="userhours">The id of the userhours object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the userhours object.</param>
        /// <returns>True if the userhours object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(UserHours userhours)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(userhours.ConnectionName, UserHoursQueries.UnDelete(userhours, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a UserHours object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the userhours object from the data reader.
        /// </summary>
        /// <param name="userhours">The userhours object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(UserHours userhours, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    userhours.IsLoaded = true;
                    userhours.FKUserID = reader["FKUserID"] != DBNull.Value ? (long)reader["FKUserID"] : (long?)null;
                    userhours.FKINCampaignID = reader["FKINCampaignID"] != DBNull.Value ? (long)reader["FKINCampaignID"] : (long?)null;
                    userhours.WorkingDate = reader["WorkingDate"] != DBNull.Value ? (DateTime)reader["WorkingDate"] : (DateTime?)null;
                    userhours.MorningShiftStartTime = reader["MorningShiftStartTime"] != DBNull.Value ? (TimeSpan)reader["MorningShiftStartTime"] : (TimeSpan?)null;
                    userhours.MorningShiftEndTime = reader["MorningShiftEndTime"] != DBNull.Value ? (TimeSpan)reader["MorningShiftEndTime"] : (TimeSpan?)null;
                    userhours.NormalShiftStartTime = reader["NormalShiftStartTime"] != DBNull.Value ? (TimeSpan)reader["NormalShiftStartTime"] : (TimeSpan?)null;
                    userhours.NormalShiftEndTime = reader["NormalShiftEndTime"] != DBNull.Value ? (TimeSpan)reader["NormalShiftEndTime"] : (TimeSpan?)null;
                    userhours.EveningShiftStartTime = reader["EveningShiftStartTime"] != DBNull.Value ? (TimeSpan)reader["EveningShiftStartTime"] : (TimeSpan?)null;
                    userhours.EveningShiftEndTime = reader["EveningShiftEndTime"] != DBNull.Value ? (TimeSpan)reader["EveningShiftEndTime"] : (TimeSpan?)null;
                    userhours.PublicHolidayWeekendShiftStartTime = reader["PublicHolidayWeekendShiftStartTime"] != DBNull.Value ? (TimeSpan)reader["PublicHolidayWeekendShiftStartTime"] : (TimeSpan?)null;
                    userhours.PublicHolidayWeekendShiftEndTime = reader["PublicHolidayWeekendShiftEndTime"] != DBNull.Value ? (TimeSpan)reader["PublicHolidayWeekendShiftEndTime"] : (TimeSpan?)null;
                    userhours.FKShiftTypeID = reader["FKShiftTypeID"] != DBNull.Value ? (long)reader["FKShiftTypeID"] : (long?)null;
                    userhours.IsRedeemedHours = reader["IsRedeemedHours"] != DBNull.Value ? (bool)reader["IsRedeemedHours"] : (bool?)null;
                    userhours.StampDate = (DateTime)reader["StampDate"];
                    userhours.HasChanged = false;
                }
                else
                {
                    throw new MapperException("UserHours does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a UserHours object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an userhours object from the database.
        /// </summary>
        /// <param name="userhours">The userhours to fill.</param>
        internal static void Fill(UserHours userhours)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(userhours.ConnectionName, UserHoursQueries.Fill(userhours, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(userhours, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a UserHours object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a userhours object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(UserHours userhours)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(userhours.ConnectionName, UserHoursQueries.FillData(userhours, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a UserHours object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an userhours object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="userhours">The userhours to fill from history.</param>
        internal static void FillHistory(UserHours userhours, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(userhours.ConnectionName, UserHoursQueries.FillHistory(userhours, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(userhours, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a UserHours object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the userhours objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static UserHoursCollection List(bool showDeleted, string connectionName)
        {
            UserHoursCollection collection = new UserHoursCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? UserHoursQueries.ListDeleted() : UserHoursQueries.List(), null);
                while (reader.Read())
                {
                    UserHours userhours = new UserHours((long)reader["ID"]);
                    userhours.ConnectionName = connectionName;
                    collection.Add(userhours);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list UserHours objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the userhours objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? UserHoursQueries.ListDeleted() : UserHoursQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list UserHours objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) userhours object from the database.
        /// </summary>
        /// <param name="userhours">The userhours to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(UserHours userhours)
        {
            UserHoursCollection collection = new UserHoursCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(userhours.ConnectionName, UserHoursQueries.ListHistory(userhours, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) UserHours in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an userhours object to the database.
        /// </summary>
        /// <param name="userhours">The UserHours object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the userhours was saved successfull, otherwise, false.</returns>
        internal static bool Save(UserHours userhours)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (userhours.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(userhours.ConnectionName, UserHoursQueries.Save(userhours, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(userhours.ConnectionName, UserHoursQueries.Save(userhours, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            userhours.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= userhours.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a UserHours object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for userhours objects in the database.
        /// </summary>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="workingdate">The workingdate search criteria.</param>
        /// <param name="morningshiftstarttime">The morningshiftstarttime search criteria.</param>
        /// <param name="morningshiftendtime">The morningshiftendtime search criteria.</param>
        /// <param name="normalshiftstarttime">The normalshiftstarttime search criteria.</param>
        /// <param name="normalshiftendtime">The normalshiftendtime search criteria.</param>
        /// <param name="eveningshiftstarttime">The eveningshiftstarttime search criteria.</param>
        /// <param name="eveningshiftendtime">The eveningshiftendtime search criteria.</param>
        /// <param name="publicholidayweekendshiftstarttime">The publicholidayweekendshiftstarttime search criteria.</param>
        /// <param name="publicholidayweekendshiftendtime">The publicholidayweekendshiftendtime search criteria.</param>
        /// <param name="fkshifttypeid">The fkshifttypeid search criteria.</param>
        /// <param name="isredeemedhours">The isredeemedhours search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static UserHoursCollection Search(long? fkuserid, long? fkincampaignid, DateTime? workingdate, TimeSpan? morningshiftstarttime, TimeSpan? morningshiftendtime, TimeSpan? normalshiftstarttime, TimeSpan? normalshiftendtime, TimeSpan? eveningshiftstarttime, TimeSpan? eveningshiftendtime, TimeSpan? publicholidayweekendshiftstarttime, TimeSpan? publicholidayweekendshiftendtime, long? fkshifttypeid, bool? isredeemedhours, string connectionName)
        {
            UserHoursCollection collection = new UserHoursCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, UserHoursQueries.Search(fkuserid, fkincampaignid, workingdate, morningshiftstarttime, morningshiftendtime, normalshiftstarttime, normalshiftendtime, eveningshiftstarttime, eveningshiftendtime, publicholidayweekendshiftstarttime, publicholidayweekendshiftendtime, fkshifttypeid, isredeemedhours), null);
                while (reader.Read())
                {
                    UserHours userhours = new UserHours((long)reader["ID"]);
                    userhours.ConnectionName = connectionName;
                    collection.Add(userhours);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for UserHours objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for userhours objects in the database.
        /// </summary>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="workingdate">The workingdate search criteria.</param>
        /// <param name="morningshiftstarttime">The morningshiftstarttime search criteria.</param>
        /// <param name="morningshiftendtime">The morningshiftendtime search criteria.</param>
        /// <param name="normalshiftstarttime">The normalshiftstarttime search criteria.</param>
        /// <param name="normalshiftendtime">The normalshiftendtime search criteria.</param>
        /// <param name="eveningshiftstarttime">The eveningshiftstarttime search criteria.</param>
        /// <param name="eveningshiftendtime">The eveningshiftendtime search criteria.</param>
        /// <param name="publicholidayweekendshiftstarttime">The publicholidayweekendshiftstarttime search criteria.</param>
        /// <param name="publicholidayweekendshiftendtime">The publicholidayweekendshiftendtime search criteria.</param>
        /// <param name="fkshifttypeid">The fkshifttypeid search criteria.</param>
        /// <param name="isredeemedhours">The isredeemedhours search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkuserid, long? fkincampaignid, DateTime? workingdate, TimeSpan? morningshiftstarttime, TimeSpan? morningshiftendtime, TimeSpan? normalshiftstarttime, TimeSpan? normalshiftendtime, TimeSpan? eveningshiftstarttime, TimeSpan? eveningshiftendtime, TimeSpan? publicholidayweekendshiftstarttime, TimeSpan? publicholidayweekendshiftendtime, long? fkshifttypeid, bool? isredeemedhours, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, UserHoursQueries.Search(fkuserid, fkincampaignid, workingdate, morningshiftstarttime, morningshiftendtime, normalshiftstarttime, normalshiftendtime, eveningshiftstarttime, eveningshiftendtime, publicholidayweekendshiftstarttime, publicholidayweekendshiftendtime, fkshifttypeid, isredeemedhours), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for UserHours objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 userhours objects in the database.
        /// </summary>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="workingdate">The workingdate search criteria.</param>
        /// <param name="morningshiftstarttime">The morningshiftstarttime search criteria.</param>
        /// <param name="morningshiftendtime">The morningshiftendtime search criteria.</param>
        /// <param name="normalshiftstarttime">The normalshiftstarttime search criteria.</param>
        /// <param name="normalshiftendtime">The normalshiftendtime search criteria.</param>
        /// <param name="eveningshiftstarttime">The eveningshiftstarttime search criteria.</param>
        /// <param name="eveningshiftendtime">The eveningshiftendtime search criteria.</param>
        /// <param name="publicholidayweekendshiftstarttime">The publicholidayweekendshiftstarttime search criteria.</param>
        /// <param name="publicholidayweekendshiftendtime">The publicholidayweekendshiftendtime search criteria.</param>
        /// <param name="fkshifttypeid">The fkshifttypeid search criteria.</param>
        /// <param name="isredeemedhours">The isredeemedhours search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static UserHours SearchOne(long? fkuserid, long? fkincampaignid, DateTime? workingdate, TimeSpan? morningshiftstarttime, TimeSpan? morningshiftendtime, TimeSpan? normalshiftstarttime, TimeSpan? normalshiftendtime, TimeSpan? eveningshiftstarttime, TimeSpan? eveningshiftendtime, TimeSpan? publicholidayweekendshiftstarttime, TimeSpan? publicholidayweekendshiftendtime, long? fkshifttypeid, bool? isredeemedhours, string connectionName)
        {
            UserHours userhours = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, UserHoursQueries.Search(fkuserid, fkincampaignid, workingdate, morningshiftstarttime, morningshiftendtime, normalshiftstarttime, normalshiftendtime, eveningshiftstarttime, eveningshiftendtime, publicholidayweekendshiftstarttime, publicholidayweekendshiftendtime, fkshifttypeid, isredeemedhours), null);
                if (reader.Read())
                {
                    userhours = new UserHours((long)reader["ID"]);
                    userhours.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for UserHours objects in the database", ex);
            }
            return userhours;
        }
        #endregion
    }
}
