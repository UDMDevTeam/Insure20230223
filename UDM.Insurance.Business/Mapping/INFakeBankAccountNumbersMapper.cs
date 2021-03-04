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
    /// Contains methods to fill, save and delete infakebankaccountnumbers objects.
    /// </summary>
    public partial class INFakeBankAccountNumbersMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) infakebankaccountnumbers object from the database.
        /// </summary>
        /// <param name="infakebankaccountnumbers">The id of the infakebankaccountnumbers object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the infakebankaccountnumbers object.</param>
        /// <returns>True if the infakebankaccountnumbers object was deleted successfully, else false.</returns>
        internal static bool Delete(INFakeBankAccountNumbers infakebankaccountnumbers)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(infakebankaccountnumbers.ConnectionName, INFakeBankAccountNumbersQueries.Delete(infakebankaccountnumbers, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INFakeBankAccountNumbers object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) infakebankaccountnumbers object from the database.
        /// </summary>
        /// <param name="infakebankaccountnumbers">The id of the infakebankaccountnumbers object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the infakebankaccountnumbers history.</param>
        /// <returns>True if the infakebankaccountnumbers history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INFakeBankAccountNumbers infakebankaccountnumbers)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(infakebankaccountnumbers.ConnectionName, INFakeBankAccountNumbersQueries.DeleteHistory(infakebankaccountnumbers, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INFakeBankAccountNumbers history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) infakebankaccountnumbers object from the database.
        /// </summary>
        /// <param name="infakebankaccountnumbers">The id of the infakebankaccountnumbers object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the infakebankaccountnumbers object.</param>
        /// <returns>True if the infakebankaccountnumbers object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INFakeBankAccountNumbers infakebankaccountnumbers)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(infakebankaccountnumbers.ConnectionName, INFakeBankAccountNumbersQueries.UnDelete(infakebankaccountnumbers, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INFakeBankAccountNumbers object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the infakebankaccountnumbers object from the data reader.
        /// </summary>
        /// <param name="infakebankaccountnumbers">The infakebankaccountnumbers object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INFakeBankAccountNumbers infakebankaccountnumbers, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    infakebankaccountnumbers.IsLoaded = true;
                    infakebankaccountnumbers.FKBankID = reader["FKBankID"] != DBNull.Value ? (long)reader["FKBankID"] : (long?)null;
                    infakebankaccountnumbers.AccNo = reader["AccNo"] != DBNull.Value ? (string)reader["AccNo"] : (string)null;
                    infakebankaccountnumbers.StampDate = (DateTime)reader["StampDate"];
                    infakebankaccountnumbers.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INFakeBankAccountNumbers does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INFakeBankAccountNumbers object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an infakebankaccountnumbers object from the database.
        /// </summary>
        /// <param name="infakebankaccountnumbers">The infakebankaccountnumbers to fill.</param>
        internal static void Fill(INFakeBankAccountNumbers infakebankaccountnumbers)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(infakebankaccountnumbers.ConnectionName, INFakeBankAccountNumbersQueries.Fill(infakebankaccountnumbers, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(infakebankaccountnumbers, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INFakeBankAccountNumbers object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a infakebankaccountnumbers object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INFakeBankAccountNumbers infakebankaccountnumbers)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(infakebankaccountnumbers.ConnectionName, INFakeBankAccountNumbersQueries.FillData(infakebankaccountnumbers, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INFakeBankAccountNumbers object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an infakebankaccountnumbers object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="infakebankaccountnumbers">The infakebankaccountnumbers to fill from history.</param>
        internal static void FillHistory(INFakeBankAccountNumbers infakebankaccountnumbers, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(infakebankaccountnumbers.ConnectionName, INFakeBankAccountNumbersQueries.FillHistory(infakebankaccountnumbers, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(infakebankaccountnumbers, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INFakeBankAccountNumbers object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the infakebankaccountnumbers objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INFakeBankAccountNumbersCollection List(bool showDeleted, string connectionName)
        {
            INFakeBankAccountNumbersCollection collection = new INFakeBankAccountNumbersCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INFakeBankAccountNumbersQueries.ListDeleted() : INFakeBankAccountNumbersQueries.List(), null);
                while (reader.Read())
                {
                    INFakeBankAccountNumbers infakebankaccountnumbers = new INFakeBankAccountNumbers((long)reader["ID"]);
                    infakebankaccountnumbers.ConnectionName = connectionName;
                    collection.Add(infakebankaccountnumbers);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INFakeBankAccountNumbers objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the infakebankaccountnumbers objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INFakeBankAccountNumbersQueries.ListDeleted() : INFakeBankAccountNumbersQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INFakeBankAccountNumbers objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) infakebankaccountnumbers object from the database.
        /// </summary>
        /// <param name="infakebankaccountnumbers">The infakebankaccountnumbers to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INFakeBankAccountNumbers infakebankaccountnumbers)
        {
            INFakeBankAccountNumbersCollection collection = new INFakeBankAccountNumbersCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(infakebankaccountnumbers.ConnectionName, INFakeBankAccountNumbersQueries.ListHistory(infakebankaccountnumbers, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INFakeBankAccountNumbers in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an infakebankaccountnumbers object to the database.
        /// </summary>
        /// <param name="infakebankaccountnumbers">The INFakeBankAccountNumbers object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the infakebankaccountnumbers was saved successfull, otherwise, false.</returns>
        internal static bool Save(INFakeBankAccountNumbers infakebankaccountnumbers)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (infakebankaccountnumbers.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(infakebankaccountnumbers.ConnectionName, INFakeBankAccountNumbersQueries.Save(infakebankaccountnumbers, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(infakebankaccountnumbers.ConnectionName, INFakeBankAccountNumbersQueries.Save(infakebankaccountnumbers, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            infakebankaccountnumbers.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= infakebankaccountnumbers.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INFakeBankAccountNumbers object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for infakebankaccountnumbers objects in the database.
        /// </summary>
        /// <param name="fkbankid">The fkbankid search criteria.</param>
        /// <param name="accno">The accno search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INFakeBankAccountNumbersCollection Search(long? fkbankid, string accno, string connectionName)
        {
            INFakeBankAccountNumbersCollection collection = new INFakeBankAccountNumbersCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INFakeBankAccountNumbersQueries.Search(fkbankid, accno), null);
                while (reader.Read())
                {
                    INFakeBankAccountNumbers infakebankaccountnumbers = new INFakeBankAccountNumbers((long)reader["ID"]);
                    infakebankaccountnumbers.ConnectionName = connectionName;
                    collection.Add(infakebankaccountnumbers);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INFakeBankAccountNumbers objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for infakebankaccountnumbers objects in the database.
        /// </summary>
        /// <param name="fkbankid">The fkbankid search criteria.</param>
        /// <param name="accno">The accno search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkbankid, string accno, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INFakeBankAccountNumbersQueries.Search(fkbankid, accno), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INFakeBankAccountNumbers objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 infakebankaccountnumbers objects in the database.
        /// </summary>
        /// <param name="fkbankid">The fkbankid search criteria.</param>
        /// <param name="accno">The accno search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INFakeBankAccountNumbers SearchOne(long? fkbankid, string accno, string connectionName)
        {
            INFakeBankAccountNumbers infakebankaccountnumbers = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INFakeBankAccountNumbersQueries.Search(fkbankid, accno), null);
                if (reader.Read())
                {
                    infakebankaccountnumbers = new INFakeBankAccountNumbers((long)reader["ID"]);
                    infakebankaccountnumbers.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INFakeBankAccountNumbers objects in the database", ex);
            }
            return infakebankaccountnumbers;
        }
        #endregion
    }
}
