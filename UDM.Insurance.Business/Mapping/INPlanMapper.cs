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
    /// Contains methods to fill, save and delete inplan objects.
    /// </summary>
    public partial class INPlanMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inplan object from the database.
        /// </summary>
        /// <param name="inplan">The id of the inplan object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inplan object.</param>
        /// <returns>True if the inplan object was deleted successfully, else false.</returns>
        internal static bool Delete(INPlan inplan)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inplan.ConnectionName, INPlanQueries.Delete(inplan, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INPlan object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inplan object from the database.
        /// </summary>
        /// <param name="inplan">The id of the inplan object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inplan history.</param>
        /// <returns>True if the inplan history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INPlan inplan)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inplan.ConnectionName, INPlanQueries.DeleteHistory(inplan, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INPlan history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inplan object from the database.
        /// </summary>
        /// <param name="inplan">The id of the inplan object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inplan object.</param>
        /// <returns>True if the inplan object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INPlan inplan)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inplan.ConnectionName, INPlanQueries.UnDelete(inplan, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INPlan object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inplan object from the data reader.
        /// </summary>
        /// <param name="inplan">The inplan object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INPlan inplan, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inplan.IsLoaded = true;
                    inplan.FKINPlanGroupID = reader["FKINPlanGroupID"] != DBNull.Value ? (long)reader["FKINPlanGroupID"] : (long?)null;
                    inplan.PlanCode = reader["PlanCode"] != DBNull.Value ? (string)reader["PlanCode"] : (string)null;
                    inplan.AgeMin = reader["AgeMin"] != DBNull.Value ? (short)reader["AgeMin"] : (short?)null;
                    inplan.AgeMax = reader["AgeMax"] != DBNull.Value ? (short)reader["AgeMax"] : (short?)null;
                    inplan.IGFreeCover = reader["IGFreeCover"] != DBNull.Value ? (decimal)reader["IGFreeCover"] : (decimal?)null;
                    inplan.IsActive = reader["IsActive"] != DBNull.Value ? (bool)reader["IsActive"] : (bool?)null;
                    inplan.StampDate = (DateTime)reader["StampDate"];
                    inplan.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INPlan does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPlan object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inplan object from the database.
        /// </summary>
        /// <param name="inplan">The inplan to fill.</param>
        internal static void Fill(INPlan inplan)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inplan.ConnectionName, INPlanQueries.Fill(inplan, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inplan, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPlan object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inplan object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INPlan inplan)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inplan.ConnectionName, INPlanQueries.FillData(inplan, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INPlan object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inplan object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inplan">The inplan to fill from history.</param>
        internal static void FillHistory(INPlan inplan, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inplan.ConnectionName, INPlanQueries.FillHistory(inplan, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inplan, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPlan object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inplan objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INPlanCollection List(bool showDeleted, string connectionName)
        {
            INPlanCollection collection = new INPlanCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INPlanQueries.ListDeleted() : INPlanQueries.List(), null);
                while (reader.Read())
                {
                    INPlan inplan = new INPlan((long)reader["ID"]);
                    inplan.ConnectionName = connectionName;
                    collection.Add(inplan);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INPlan objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inplan objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INPlanQueries.ListDeleted() : INPlanQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INPlan objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inplan object from the database.
        /// </summary>
        /// <param name="inplan">The inplan to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INPlan inplan)
        {
            INPlanCollection collection = new INPlanCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inplan.ConnectionName, INPlanQueries.ListHistory(inplan, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INPlan in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inplan object to the database.
        /// </summary>
        /// <param name="inplan">The INPlan object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inplan was saved successfull, otherwise, false.</returns>
        internal static bool Save(INPlan inplan)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inplan.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inplan.ConnectionName, INPlanQueries.Save(inplan, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inplan.ConnectionName, INPlanQueries.Save(inplan, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inplan.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inplan.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INPlan object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inplan objects in the database.
        /// </summary>
        /// <param name="fkinplangroupid">The fkinplangroupid search criteria.</param>
        /// <param name="plancode">The plancode search criteria.</param>
        /// <param name="agemin">The agemin search criteria.</param>
        /// <param name="agemax">The agemax search criteria.</param>
        /// <param name="igfreecover">The igfreecover search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INPlanCollection Search(long? fkinplangroupid, string plancode, short? agemin, short? agemax, decimal? igfreecover, bool? isactive, string connectionName)
        {
            INPlanCollection collection = new INPlanCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INPlanQueries.Search(fkinplangroupid, plancode, agemin, agemax, igfreecover, isactive), null);
                while (reader.Read())
                {
                    INPlan inplan = new INPlan((long)reader["ID"]);
                    inplan.ConnectionName = connectionName;
                    collection.Add(inplan);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPlan objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inplan objects in the database.
        /// </summary>
        /// <param name="fkinplangroupid">The fkinplangroupid search criteria.</param>
        /// <param name="plancode">The plancode search criteria.</param>
        /// <param name="agemin">The agemin search criteria.</param>
        /// <param name="agemax">The agemax search criteria.</param>
        /// <param name="igfreecover">The igfreecover search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinplangroupid, string plancode, short? agemin, short? agemax, decimal? igfreecover, bool? isactive, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INPlanQueries.Search(fkinplangroupid, plancode, agemin, agemax, igfreecover, isactive), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPlan objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inplan objects in the database.
        /// </summary>
        /// <param name="fkinplangroupid">The fkinplangroupid search criteria.</param>
        /// <param name="plancode">The plancode search criteria.</param>
        /// <param name="agemin">The agemin search criteria.</param>
        /// <param name="agemax">The agemax search criteria.</param>
        /// <param name="igfreecover">The igfreecover search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INPlan SearchOne(long? fkinplangroupid, string plancode, short? agemin, short? agemax, decimal? igfreecover, bool? isactive, string connectionName)
        {
            INPlan inplan = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INPlanQueries.Search(fkinplangroupid, plancode, agemin, agemax, igfreecover, isactive), null);
                if (reader.Read())
                {
                    inplan = new INPlan((long)reader["ID"]);
                    inplan.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPlan objects in the database", ex);
            }
            return inplan;
        }
        #endregion
    }
}
