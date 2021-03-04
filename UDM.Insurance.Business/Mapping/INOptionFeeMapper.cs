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
    /// Contains methods to fill, save and delete inoptionfee objects.
    /// </summary>
    public partial class INOptionFeeMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inoptionfee object from the database.
        /// </summary>
        /// <param name="inoptionfee">The id of the inoptionfee object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inoptionfee object.</param>
        /// <returns>True if the inoptionfee object was deleted successfully, else false.</returns>
        internal static bool Delete(INOptionFee inoptionfee)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inoptionfee.ConnectionName, INOptionFeeQueries.Delete(inoptionfee, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INOptionFee object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inoptionfee object from the database.
        /// </summary>
        /// <param name="inoptionfee">The id of the inoptionfee object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inoptionfee history.</param>
        /// <returns>True if the inoptionfee history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INOptionFee inoptionfee)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inoptionfee.ConnectionName, INOptionFeeQueries.DeleteHistory(inoptionfee, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INOptionFee history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inoptionfee object from the database.
        /// </summary>
        /// <param name="inoptionfee">The id of the inoptionfee object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inoptionfee object.</param>
        /// <returns>True if the inoptionfee object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INOptionFee inoptionfee)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inoptionfee.ConnectionName, INOptionFeeQueries.UnDelete(inoptionfee, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INOptionFee object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inoptionfee object from the data reader.
        /// </summary>
        /// <param name="inoptionfee">The inoptionfee object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INOptionFee inoptionfee, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inoptionfee.IsLoaded = true;
                    inoptionfee.FKINOptionID = reader["FKINOptionID"] != DBNull.Value ? (long)reader["FKINOptionID"] : 0;
                    inoptionfee.FKINFeeID = reader["FKINFeeID"] != DBNull.Value ? (long)reader["FKINFeeID"] : 0;
                    inoptionfee.Date = reader["Date"] != DBNull.Value ? (DateTime)reader["Date"] : GlobalSettings.EmptyDate;
                    inoptionfee.IsActive = reader["IsActive"] != DBNull.Value ? (bool)reader["IsActive"] : false;
                    inoptionfee.StampDate = (DateTime)reader["StampDate"];
                    inoptionfee.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INOptionFee does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INOptionFee object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inoptionfee object from the database.
        /// </summary>
        /// <param name="inoptionfee">The inoptionfee to fill.</param>
        internal static void Fill(INOptionFee inoptionfee)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inoptionfee.ConnectionName, INOptionFeeQueries.Fill(inoptionfee, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inoptionfee, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INOptionFee object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inoptionfee object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INOptionFee inoptionfee)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inoptionfee.ConnectionName, INOptionFeeQueries.FillData(inoptionfee, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INOptionFee object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inoptionfee object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inoptionfee">The inoptionfee to fill from history.</param>
        internal static void FillHistory(INOptionFee inoptionfee, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inoptionfee.ConnectionName, INOptionFeeQueries.FillHistory(inoptionfee, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inoptionfee, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INOptionFee object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inoptionfee objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INOptionFeeCollection List(bool showDeleted, string connectionName)
        {
            INOptionFeeCollection collection = new INOptionFeeCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INOptionFeeQueries.ListDeleted() : INOptionFeeQueries.List(), null);
                while (reader.Read())
                {
                    INOptionFee inoptionfee = new INOptionFee((long)reader["ID"]);
                    inoptionfee.ConnectionName = connectionName;
                    collection.Add(inoptionfee);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INOptionFee objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inoptionfee objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INOptionFeeQueries.ListDeleted() : INOptionFeeQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INOptionFee objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inoptionfee object from the database.
        /// </summary>
        /// <param name="inoptionfee">The inoptionfee to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INOptionFee inoptionfee)
        {
            INOptionFeeCollection collection = new INOptionFeeCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inoptionfee.ConnectionName, INOptionFeeQueries.ListHistory(inoptionfee, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INOptionFee in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inoptionfee object to the database.
        /// </summary>
        /// <param name="inoptionfee">The INOptionFee object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inoptionfee was saved successfull, otherwise, false.</returns>
        internal static bool Save(INOptionFee inoptionfee)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inoptionfee.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inoptionfee.ConnectionName, INOptionFeeQueries.Save(inoptionfee, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inoptionfee.ConnectionName, INOptionFeeQueries.Save(inoptionfee, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inoptionfee.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inoptionfee.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INOptionFee object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inoptionfee objects in the database.
        /// </summary>
        /// <param name="fkinoptionid">The fkinoptionid search criteria.</param>
        /// <param name="fkinfeeid">The fkinfeeid search criteria.</param>
        /// <param name="date">The date search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INOptionFeeCollection Search(long? fkinoptionid, long? fkinfeeid, DateTime? date, bool? isactive, string connectionName)
        {
            INOptionFeeCollection collection = new INOptionFeeCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INOptionFeeQueries.Search(fkinoptionid, fkinfeeid, date, isactive), null);
                while (reader.Read())
                {
                    INOptionFee inoptionfee = new INOptionFee((long)reader["ID"]);
                    inoptionfee.ConnectionName = connectionName;
                    collection.Add(inoptionfee);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INOptionFee objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inoptionfee objects in the database.
        /// </summary>
        /// <param name="fkinoptionid">The fkinoptionid search criteria.</param>
        /// <param name="fkinfeeid">The fkinfeeid search criteria.</param>
        /// <param name="date">The date search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinoptionid, long? fkinfeeid, DateTime? date, bool? isactive, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INOptionFeeQueries.Search(fkinoptionid, fkinfeeid, date, isactive), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INOptionFee objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inoptionfee objects in the database.
        /// </summary>
        /// <param name="fkinoptionid">The fkinoptionid search criteria.</param>
        /// <param name="fkinfeeid">The fkinfeeid search criteria.</param>
        /// <param name="date">The date search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INOptionFee SearchOne(long? fkinoptionid, long? fkinfeeid, DateTime? date, bool? isactive, string connectionName)
        {
            INOptionFee inoptionfee = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INOptionFeeQueries.Search(fkinoptionid, fkinfeeid, date, isactive), null);
                if (reader.Read())
                {
                    inoptionfee = new INOptionFee((long)reader["ID"]);
                    inoptionfee.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INOptionFee objects in the database", ex);
            }
            return inoptionfee;
        }
        #endregion
    }
}
