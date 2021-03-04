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
    /// Contains methods to fill, save and delete infee objects.
    /// </summary>
    public partial class INFeeMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) infee object from the database.
        /// </summary>
        /// <param name="infee">The id of the infee object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the infee object.</param>
        /// <returns>True if the infee object was deleted successfully, else false.</returns>
        internal static bool Delete(INFee infee)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(infee.ConnectionName, INFeeQueries.Delete(infee, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INFee object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) infee object from the database.
        /// </summary>
        /// <param name="infee">The id of the infee object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the infee history.</param>
        /// <returns>True if the infee history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INFee infee)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(infee.ConnectionName, INFeeQueries.DeleteHistory(infee, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INFee history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) infee object from the database.
        /// </summary>
        /// <param name="infee">The id of the infee object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the infee object.</param>
        /// <returns>True if the infee object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INFee infee)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(infee.ConnectionName, INFeeQueries.UnDelete(infee, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INFee object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the infee object from the data reader.
        /// </summary>
        /// <param name="infee">The infee object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INFee infee, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    infee.IsLoaded = true;
                    infee.LA1Fee = reader["LA1Fee"] != DBNull.Value ? (decimal)reader["LA1Fee"] : (decimal?)null;
                    infee.LA2Fee = reader["LA2Fee"] != DBNull.Value ? (decimal)reader["LA2Fee"] : (decimal?)null;
                    infee.ChildFee = reader["ChildFee"] != DBNull.Value ? (decimal)reader["ChildFee"] : (decimal?)null;
                    infee.UnitFee = reader["UnitFee"] != DBNull.Value ? (decimal)reader["UnitFee"] : (decimal?)null;
                    infee.StampDate = (DateTime)reader["StampDate"];
                    infee.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INFee does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INFee object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an infee object from the database.
        /// </summary>
        /// <param name="infee">The infee to fill.</param>
        internal static void Fill(INFee infee)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(infee.ConnectionName, INFeeQueries.Fill(infee, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(infee, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INFee object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a infee object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INFee infee)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(infee.ConnectionName, INFeeQueries.FillData(infee, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INFee object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an infee object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="infee">The infee to fill from history.</param>
        internal static void FillHistory(INFee infee, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(infee.ConnectionName, INFeeQueries.FillHistory(infee, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(infee, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INFee object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the infee objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INFeeCollection List(bool showDeleted, string connectionName)
        {
            INFeeCollection collection = new INFeeCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INFeeQueries.ListDeleted() : INFeeQueries.List(), null);
                while (reader.Read())
                {
                    INFee infee = new INFee((long)reader["ID"]);
                    infee.ConnectionName = connectionName;
                    collection.Add(infee);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INFee objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the infee objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INFeeQueries.ListDeleted() : INFeeQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INFee objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) infee object from the database.
        /// </summary>
        /// <param name="infee">The infee to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INFee infee)
        {
            INFeeCollection collection = new INFeeCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(infee.ConnectionName, INFeeQueries.ListHistory(infee, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INFee in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an infee object to the database.
        /// </summary>
        /// <param name="infee">The INFee object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the infee was saved successfull, otherwise, false.</returns>
        internal static bool Save(INFee infee)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (infee.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(infee.ConnectionName, INFeeQueries.Save(infee, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(infee.ConnectionName, INFeeQueries.Save(infee, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            infee.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= infee.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INFee object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for infee objects in the database.
        /// </summary>
        /// <param name="la1fee">The la1fee search criteria.</param>
        /// <param name="la2fee">The la2fee search criteria.</param>
        /// <param name="childfee">The childfee search criteria.</param>
        /// <param name="unitfee">The unitfee search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INFeeCollection Search(decimal? la1fee, decimal? la2fee, decimal? childfee, decimal? unitfee, string connectionName)
        {
            INFeeCollection collection = new INFeeCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INFeeQueries.Search(la1fee, la2fee, childfee, unitfee), null);
                while (reader.Read())
                {
                    INFee infee = new INFee((long)reader["ID"]);
                    infee.ConnectionName = connectionName;
                    collection.Add(infee);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INFee objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for infee objects in the database.
        /// </summary>
        /// <param name="la1fee">The la1fee search criteria.</param>
        /// <param name="la2fee">The la2fee search criteria.</param>
        /// <param name="childfee">The childfee search criteria.</param>
        /// <param name="unitfee">The unitfee search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(decimal? la1fee, decimal? la2fee, decimal? childfee, decimal? unitfee, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INFeeQueries.Search(la1fee, la2fee, childfee, unitfee), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INFee objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 infee objects in the database.
        /// </summary>
        /// <param name="la1fee">The la1fee search criteria.</param>
        /// <param name="la2fee">The la2fee search criteria.</param>
        /// <param name="childfee">The childfee search criteria.</param>
        /// <param name="unitfee">The unitfee search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INFee SearchOne(decimal? la1fee, decimal? la2fee, decimal? childfee, decimal? unitfee, string connectionName)
        {
            INFee infee = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INFeeQueries.Search(la1fee, la2fee, childfee, unitfee), null);
                if (reader.Read())
                {
                    infee = new INFee((long)reader["ID"]);
                    infee.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INFee objects in the database", ex);
            }
            return infee;
        }
        #endregion
    }
}
