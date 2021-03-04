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
    /// Contains methods to fill, save and delete ingiftoption objects.
    /// </summary>
    public partial class INGiftOptionMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) ingiftoption object from the database.
        /// </summary>
        /// <param name="ingiftoption">The id of the ingiftoption object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the ingiftoption object.</param>
        /// <returns>True if the ingiftoption object was deleted successfully, else false.</returns>
        internal static bool Delete(INGiftOption ingiftoption)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(ingiftoption.ConnectionName, INGiftOptionQueries.Delete(ingiftoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INGiftOption object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) ingiftoption object from the database.
        /// </summary>
        /// <param name="ingiftoption">The id of the ingiftoption object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the ingiftoption history.</param>
        /// <returns>True if the ingiftoption history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INGiftOption ingiftoption)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(ingiftoption.ConnectionName, INGiftOptionQueries.DeleteHistory(ingiftoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INGiftOption history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) ingiftoption object from the database.
        /// </summary>
        /// <param name="ingiftoption">The id of the ingiftoption object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the ingiftoption object.</param>
        /// <returns>True if the ingiftoption object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INGiftOption ingiftoption)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(ingiftoption.ConnectionName, INGiftOptionQueries.UnDelete(ingiftoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INGiftOption object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the ingiftoption object from the data reader.
        /// </summary>
        /// <param name="ingiftoption">The ingiftoption object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INGiftOption ingiftoption, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    ingiftoption.IsLoaded = true;
                    ingiftoption.Gift = reader["Gift"] != DBNull.Value ? (string)reader["Gift"] : (string)null;
                    ingiftoption.ActiveStartDate = reader["ActiveStartDate"] != DBNull.Value ? (DateTime)reader["ActiveStartDate"] : (DateTime?)null;
                    ingiftoption.ActiveEndDate = reader["ActiveEndDate"] != DBNull.Value ? (DateTime)reader["ActiveEndDate"] : (DateTime?)null;
                    ingiftoption.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INGiftOption does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INGiftOption object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an ingiftoption object from the database.
        /// </summary>
        /// <param name="ingiftoption">The ingiftoption to fill.</param>
        internal static void Fill(INGiftOption ingiftoption)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(ingiftoption.ConnectionName, INGiftOptionQueries.Fill(ingiftoption, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(ingiftoption, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INGiftOption object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a ingiftoption object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INGiftOption ingiftoption)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(ingiftoption.ConnectionName, INGiftOptionQueries.FillData(ingiftoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INGiftOption object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an ingiftoption object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="ingiftoption">The ingiftoption to fill from history.</param>
        internal static void FillHistory(INGiftOption ingiftoption, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(ingiftoption.ConnectionName, INGiftOptionQueries.FillHistory(ingiftoption, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(ingiftoption, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INGiftOption object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the ingiftoption objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INGiftOptionCollection List(bool showDeleted, string connectionName)
        {
            INGiftOptionCollection collection = new INGiftOptionCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INGiftOptionQueries.ListDeleted() : INGiftOptionQueries.List(), null);
                while (reader.Read())
                {
                    INGiftOption ingiftoption = new INGiftOption((long)reader["ID"]);
                    ingiftoption.ConnectionName = connectionName;
                    collection.Add(ingiftoption);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INGiftOption objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the ingiftoption objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INGiftOptionQueries.ListDeleted() : INGiftOptionQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INGiftOption objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) ingiftoption object from the database.
        /// </summary>
        /// <param name="ingiftoption">The ingiftoption to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INGiftOption ingiftoption)
        {
            INGiftOptionCollection collection = new INGiftOptionCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(ingiftoption.ConnectionName, INGiftOptionQueries.ListHistory(ingiftoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INGiftOption in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an ingiftoption object to the database.
        /// </summary>
        /// <param name="ingiftoption">The INGiftOption object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the ingiftoption was saved successfull, otherwise, false.</returns>
        internal static bool Save(INGiftOption ingiftoption)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (ingiftoption.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(ingiftoption.ConnectionName, INGiftOptionQueries.Save(ingiftoption, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(ingiftoption.ConnectionName, INGiftOptionQueries.Save(ingiftoption, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            ingiftoption.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= ingiftoption.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INGiftOption object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for ingiftoption objects in the database.
        /// </summary>
        /// <param name="gift">The gift search criteria.</param>
        /// <param name="activestartdate">The activestartdate search criteria.</param>
        /// <param name="activeenddate">The activeenddate search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INGiftOptionCollection Search(string gift, DateTime? activestartdate, DateTime? activeenddate, string connectionName)
        {
            INGiftOptionCollection collection = new INGiftOptionCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INGiftOptionQueries.Search(gift, activestartdate, activeenddate), null);
                while (reader.Read())
                {
                    INGiftOption ingiftoption = new INGiftOption((long)reader["ID"]);
                    ingiftoption.ConnectionName = connectionName;
                    collection.Add(ingiftoption);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INGiftOption objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for ingiftoption objects in the database.
        /// </summary>
        /// <param name="gift">The gift search criteria.</param>
        /// <param name="activestartdate">The activestartdate search criteria.</param>
        /// <param name="activeenddate">The activeenddate search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(string gift, DateTime? activestartdate, DateTime? activeenddate, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INGiftOptionQueries.Search(gift, activestartdate, activeenddate), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INGiftOption objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 ingiftoption objects in the database.
        /// </summary>
        /// <param name="gift">The gift search criteria.</param>
        /// <param name="activestartdate">The activestartdate search criteria.</param>
        /// <param name="activeenddate">The activeenddate search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INGiftOption SearchOne(string gift, DateTime? activestartdate, DateTime? activeenddate, string connectionName)
        {
            INGiftOption ingiftoption = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INGiftOptionQueries.Search(gift, activestartdate, activeenddate), null);
                if (reader.Read())
                {
                    ingiftoption = new INGiftOption((long)reader["ID"]);
                    ingiftoption.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INGiftOption objects in the database", ex);
            }
            return ingiftoption;
        }
        #endregion
    }
}
