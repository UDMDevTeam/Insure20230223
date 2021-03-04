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
    /// Contains methods to fill, save and delete indefaultoption objects.
    /// </summary>
    public partial class INDefaultOptionMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) indefaultoption object from the database.
        /// </summary>
        /// <param name="indefaultoption">The id of the indefaultoption object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the indefaultoption object.</param>
        /// <returns>True if the indefaultoption object was deleted successfully, else false.</returns>
        internal static bool Delete(INDefaultOption indefaultoption)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(indefaultoption.ConnectionName, INDefaultOptionQueries.Delete(indefaultoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INDefaultOption object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) indefaultoption object from the database.
        /// </summary>
        /// <param name="indefaultoption">The id of the indefaultoption object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the indefaultoption history.</param>
        /// <returns>True if the indefaultoption history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INDefaultOption indefaultoption)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(indefaultoption.ConnectionName, INDefaultOptionQueries.DeleteHistory(indefaultoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INDefaultOption history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) indefaultoption object from the database.
        /// </summary>
        /// <param name="indefaultoption">The id of the indefaultoption object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the indefaultoption object.</param>
        /// <returns>True if the indefaultoption object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INDefaultOption indefaultoption)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(indefaultoption.ConnectionName, INDefaultOptionQueries.UnDelete(indefaultoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INDefaultOption object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the indefaultoption object from the data reader.
        /// </summary>
        /// <param name="indefaultoption">The indefaultoption object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INDefaultOption indefaultoption, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    indefaultoption.IsLoaded = true;
                    indefaultoption.FKINCampaign = reader["FKINCampaign"] != DBNull.Value ? (long)reader["FKINCampaign"] : (long?)null;
                    indefaultoption.FKINPlanID = reader["FKINPlanID"] != DBNull.Value ? (long)reader["FKINPlanID"] : (long?)null;
                    indefaultoption.FKHigherOptionID = reader["FKHigherOptionID"] != DBNull.Value ? (long)reader["FKHigherOptionID"] : (long?)null;
                    indefaultoption.FKLowerOptionID = reader["FKLowerOptionID"] != DBNull.Value ? (long)reader["FKLowerOptionID"] : (long?)null;
                    indefaultoption.IsActive = reader["IsActive"] != DBNull.Value ? (bool)reader["IsActive"] : (bool?)null;
                    indefaultoption.StampDate = (DateTime)reader["StampDate"];
                    indefaultoption.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INDefaultOption does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INDefaultOption object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an indefaultoption object from the database.
        /// </summary>
        /// <param name="indefaultoption">The indefaultoption to fill.</param>
        internal static void Fill(INDefaultOption indefaultoption)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(indefaultoption.ConnectionName, INDefaultOptionQueries.Fill(indefaultoption, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(indefaultoption, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INDefaultOption object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a indefaultoption object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INDefaultOption indefaultoption)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(indefaultoption.ConnectionName, INDefaultOptionQueries.FillData(indefaultoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INDefaultOption object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an indefaultoption object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="indefaultoption">The indefaultoption to fill from history.</param>
        internal static void FillHistory(INDefaultOption indefaultoption, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(indefaultoption.ConnectionName, INDefaultOptionQueries.FillHistory(indefaultoption, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(indefaultoption, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INDefaultOption object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the indefaultoption objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INDefaultOptionCollection List(bool showDeleted, string connectionName)
        {
            INDefaultOptionCollection collection = new INDefaultOptionCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INDefaultOptionQueries.ListDeleted() : INDefaultOptionQueries.List(), null);
                while (reader.Read())
                {
                    INDefaultOption indefaultoption = new INDefaultOption((long)reader["ID"]);
                    indefaultoption.ConnectionName = connectionName;
                    collection.Add(indefaultoption);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INDefaultOption objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the indefaultoption objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INDefaultOptionQueries.ListDeleted() : INDefaultOptionQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INDefaultOption objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) indefaultoption object from the database.
        /// </summary>
        /// <param name="indefaultoption">The indefaultoption to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INDefaultOption indefaultoption)
        {
            INDefaultOptionCollection collection = new INDefaultOptionCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(indefaultoption.ConnectionName, INDefaultOptionQueries.ListHistory(indefaultoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INDefaultOption in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an indefaultoption object to the database.
        /// </summary>
        /// <param name="indefaultoption">The INDefaultOption object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the indefaultoption was saved successfull, otherwise, false.</returns>
        internal static bool Save(INDefaultOption indefaultoption)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (indefaultoption.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(indefaultoption.ConnectionName, INDefaultOptionQueries.Save(indefaultoption, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(indefaultoption.ConnectionName, INDefaultOptionQueries.Save(indefaultoption, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            indefaultoption.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= indefaultoption.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INDefaultOption object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for indefaultoption objects in the database.
        /// </summary>
        /// <param name="fkincampaign">The fkincampaign search criteria.</param>
        /// <param name="fkinplanid">The fkinplanid search criteria.</param>
        /// <param name="fkhigheroptionid">The fkhigheroptionid search criteria.</param>
        /// <param name="fkloweroptionid">The fkloweroptionid search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INDefaultOptionCollection Search(long? fkincampaign, long? fkinplanid, long? fkhigheroptionid, long? fkloweroptionid, bool? isactive, string connectionName)
        {
            INDefaultOptionCollection collection = new INDefaultOptionCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INDefaultOptionQueries.Search(fkincampaign, fkinplanid, fkhigheroptionid, fkloweroptionid, isactive), null);
                while (reader.Read())
                {
                    INDefaultOption indefaultoption = new INDefaultOption((long)reader["ID"]);
                    indefaultoption.ConnectionName = connectionName;
                    collection.Add(indefaultoption);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INDefaultOption objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for indefaultoption objects in the database.
        /// </summary>
        /// <param name="fkincampaign">The fkincampaign search criteria.</param>
        /// <param name="fkinplanid">The fkinplanid search criteria.</param>
        /// <param name="fkhigheroptionid">The fkhigheroptionid search criteria.</param>
        /// <param name="fkloweroptionid">The fkloweroptionid search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkincampaign, long? fkinplanid, long? fkhigheroptionid, long? fkloweroptionid, bool? isactive, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INDefaultOptionQueries.Search(fkincampaign, fkinplanid, fkhigheroptionid, fkloweroptionid, isactive), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INDefaultOption objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 indefaultoption objects in the database.
        /// </summary>
        /// <param name="fkincampaign">The fkincampaign search criteria.</param>
        /// <param name="fkinplanid">The fkinplanid search criteria.</param>
        /// <param name="fkhigheroptionid">The fkhigheroptionid search criteria.</param>
        /// <param name="fkloweroptionid">The fkloweroptionid search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INDefaultOption SearchOne(long? fkincampaign, long? fkinplanid, long? fkhigheroptionid, long? fkloweroptionid, bool? isactive, string connectionName)
        {
            INDefaultOption indefaultoption = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INDefaultOptionQueries.Search(fkincampaign, fkinplanid, fkhigheroptionid, fkloweroptionid, isactive), null);
                if (reader.Read())
                {
                    indefaultoption = new INDefaultOption((long)reader["ID"]);
                    indefaultoption.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INDefaultOption objects in the database", ex);
            }
            return indefaultoption;
        }
        #endregion
    }
}
