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
    /// Contains methods to fill, save and delete inmoneyback objects.
    /// </summary>
    public partial class INMoneyBackMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inmoneyback object from the database.
        /// </summary>
        /// <param name="inmoneyback">The id of the inmoneyback object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inmoneyback object.</param>
        /// <returns>True if the inmoneyback object was deleted successfully, else false.</returns>
        internal static bool Delete(INMoneyBack inmoneyback)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inmoneyback.ConnectionName, INMoneyBackQueries.Delete(inmoneyback, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INMoneyBack object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inmoneyback object from the database.
        /// </summary>
        /// <param name="inmoneyback">The id of the inmoneyback object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inmoneyback history.</param>
        /// <returns>True if the inmoneyback history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INMoneyBack inmoneyback)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inmoneyback.ConnectionName, INMoneyBackQueries.DeleteHistory(inmoneyback, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INMoneyBack history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inmoneyback object from the database.
        /// </summary>
        /// <param name="inmoneyback">The id of the inmoneyback object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inmoneyback object.</param>
        /// <returns>True if the inmoneyback object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INMoneyBack inmoneyback)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inmoneyback.ConnectionName, INMoneyBackQueries.UnDelete(inmoneyback, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INMoneyBack object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inmoneyback object from the data reader.
        /// </summary>
        /// <param name="inmoneyback">The inmoneyback object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INMoneyBack inmoneyback, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inmoneyback.IsLoaded = true;
                    inmoneyback.FKINPolicyTypeID = reader["FKINPolicyTypeID"] != DBNull.Value ? (long)reader["FKINPolicyTypeID"] : (long?)null;
                    inmoneyback.FKINOptionID = reader["FKINOptionID"] != DBNull.Value ? (long)reader["FKINOptionID"] : (long?)null;
                    inmoneyback.AgeMin = reader["AgeMin"] != DBNull.Value ? (short)reader["AgeMin"] : (short?)null;
                    inmoneyback.AgeMax = reader["AgeMax"] != DBNull.Value ? (short)reader["AgeMax"] : (short?)null;
                    inmoneyback.Value = reader["Value"] != DBNull.Value ? (decimal)reader["Value"] : (decimal?)null;
                    inmoneyback.IsActive = reader["IsActive"] != DBNull.Value ? (bool)reader["IsActive"] : (bool?)null;
                    inmoneyback.StampDate = (DateTime)reader["StampDate"];
                    inmoneyback.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INMoneyBack does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INMoneyBack object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inmoneyback object from the database.
        /// </summary>
        /// <param name="inmoneyback">The inmoneyback to fill.</param>
        internal static void Fill(INMoneyBack inmoneyback)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inmoneyback.ConnectionName, INMoneyBackQueries.Fill(inmoneyback, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inmoneyback, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INMoneyBack object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inmoneyback object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INMoneyBack inmoneyback)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inmoneyback.ConnectionName, INMoneyBackQueries.FillData(inmoneyback, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INMoneyBack object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inmoneyback object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inmoneyback">The inmoneyback to fill from history.</param>
        internal static void FillHistory(INMoneyBack inmoneyback, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inmoneyback.ConnectionName, INMoneyBackQueries.FillHistory(inmoneyback, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inmoneyback, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INMoneyBack object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inmoneyback objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INMoneyBackCollection List(bool showDeleted, string connectionName)
        {
            INMoneyBackCollection collection = new INMoneyBackCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INMoneyBackQueries.ListDeleted() : INMoneyBackQueries.List(), null);
                while (reader.Read())
                {
                    INMoneyBack inmoneyback = new INMoneyBack((long)reader["ID"]);
                    inmoneyback.ConnectionName = connectionName;
                    collection.Add(inmoneyback);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INMoneyBack objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inmoneyback objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INMoneyBackQueries.ListDeleted() : INMoneyBackQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INMoneyBack objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inmoneyback object from the database.
        /// </summary>
        /// <param name="inmoneyback">The inmoneyback to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INMoneyBack inmoneyback)
        {
            INMoneyBackCollection collection = new INMoneyBackCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inmoneyback.ConnectionName, INMoneyBackQueries.ListHistory(inmoneyback, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INMoneyBack in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inmoneyback object to the database.
        /// </summary>
        /// <param name="inmoneyback">The INMoneyBack object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inmoneyback was saved successfull, otherwise, false.</returns>
        internal static bool Save(INMoneyBack inmoneyback)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inmoneyback.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inmoneyback.ConnectionName, INMoneyBackQueries.Save(inmoneyback, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inmoneyback.ConnectionName, INMoneyBackQueries.Save(inmoneyback, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inmoneyback.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inmoneyback.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INMoneyBack object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inmoneyback objects in the database.
        /// </summary>
        /// <param name="fkinpolicytypeid">The fkinpolicytypeid search criteria.</param>
        /// <param name="fkinoptionid">The fkinoptionid search criteria.</param>
        /// <param name="agemin">The agemin search criteria.</param>
        /// <param name="agemax">The agemax search criteria.</param>
        /// <param name="value">The value search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INMoneyBackCollection Search(long? fkinpolicytypeid, long? fkinoptionid, short? agemin, short? agemax, decimal? value, bool? isactive, string connectionName)
        {
            INMoneyBackCollection collection = new INMoneyBackCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INMoneyBackQueries.Search(fkinpolicytypeid, fkinoptionid, agemin, agemax, value, isactive), null);
                while (reader.Read())
                {
                    INMoneyBack inmoneyback = new INMoneyBack((long)reader["ID"]);
                    inmoneyback.ConnectionName = connectionName;
                    collection.Add(inmoneyback);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INMoneyBack objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inmoneyback objects in the database.
        /// </summary>
        /// <param name="fkinpolicytypeid">The fkinpolicytypeid search criteria.</param>
        /// <param name="fkinoptionid">The fkinoptionid search criteria.</param>
        /// <param name="agemin">The agemin search criteria.</param>
        /// <param name="agemax">The agemax search criteria.</param>
        /// <param name="value">The value search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinpolicytypeid, long? fkinoptionid, short? agemin, short? agemax, decimal? value, bool? isactive, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INMoneyBackQueries.Search(fkinpolicytypeid, fkinoptionid, agemin, agemax, value, isactive), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INMoneyBack objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inmoneyback objects in the database.
        /// </summary>
        /// <param name="fkinpolicytypeid">The fkinpolicytypeid search criteria.</param>
        /// <param name="fkinoptionid">The fkinoptionid search criteria.</param>
        /// <param name="agemin">The agemin search criteria.</param>
        /// <param name="agemax">The agemax search criteria.</param>
        /// <param name="value">The value search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INMoneyBack SearchOne(long? fkinpolicytypeid, long? fkinoptionid, short? agemin, short? agemax, decimal? value, bool? isactive, string connectionName)
        {
            INMoneyBack inmoneyback = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INMoneyBackQueries.Search(fkinpolicytypeid, fkinoptionid, agemin, agemax, value, isactive), null);
                if (reader.Read())
                {
                    inmoneyback = new INMoneyBack((long)reader["ID"]);
                    inmoneyback.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INMoneyBack objects in the database", ex);
            }
            return inmoneyback;
        }
        #endregion
    }
}
