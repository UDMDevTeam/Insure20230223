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
    /// Contains methods to fill, save and delete mercantile objects.
    /// </summary>
    public partial class MercantileMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) mercantile object from the database.
        /// </summary>
        /// <param name="mercantile">The id of the mercantile object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the mercantile object.</param>
        /// <returns>True if the mercantile object was deleted successfully, else false.</returns>
        internal static bool Delete(Mercantile mercantile)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(mercantile.ConnectionName, MercantileQueries.Delete(mercantile, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a Mercantile object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) mercantile object from the database.
        /// </summary>
        /// <param name="mercantile">The id of the mercantile object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the mercantile history.</param>
        /// <returns>True if the mercantile history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(Mercantile mercantile)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(mercantile.ConnectionName, MercantileQueries.DeleteHistory(mercantile, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete Mercantile history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) mercantile object from the database.
        /// </summary>
        /// <param name="mercantile">The id of the mercantile object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the mercantile object.</param>
        /// <returns>True if the mercantile object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(Mercantile mercantile)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(mercantile.ConnectionName, MercantileQueries.UnDelete(mercantile, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a Mercantile object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the mercantile object from the data reader.
        /// </summary>
        /// <param name="mercantile">The mercantile object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(Mercantile mercantile, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    mercantile.IsLoaded = true;
                    mercantile.FKSystemID = reader["FKSystemID"] != DBNull.Value ? (long)reader["FKSystemID"] : (long?)null;
                    mercantile.FKImportID = reader["FKImportID"] != DBNull.Value ? (long)reader["FKImportID"] : (long?)null;
                    mercantile.FKBankID = reader["FKBankID"] != DBNull.Value ? (long)reader["FKBankID"] : (long?)null;
                    mercantile.FKBankBranchID = reader["FKBankBranchID"] != DBNull.Value ? (long)reader["FKBankBranchID"] : (long?)null;
                    mercantile.AccountNumber = reader["AccountNumber"] != DBNull.Value ? (string)reader["AccountNumber"] : (string)null;
                    mercantile.AccNumCheckStatus = reader["AccNumCheckStatus"] != DBNull.Value ? (Byte)reader["AccNumCheckStatus"] : (Byte?)null;
                    mercantile.AccNumCheckMsg = reader["AccNumCheckMsg"] != DBNull.Value ? (string)reader["AccNumCheckMsg"] : (string)null;
                    mercantile.AccNumCheckMsgFull = reader["AccNumCheckMsgFull"] != DBNull.Value ? (string)reader["AccNumCheckMsgFull"] : (string)null;
                    mercantile.StampDate = (DateTime)reader["StampDate"];
                    mercantile.HasChanged = false;
                }
                else
                {
                    throw new MapperException("Mercantile does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a Mercantile object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an mercantile object from the database.
        /// </summary>
        /// <param name="mercantile">The mercantile to fill.</param>
        internal static void Fill(Mercantile mercantile)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(mercantile.ConnectionName, MercantileQueries.Fill(mercantile, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(mercantile, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a Mercantile object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a mercantile object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(Mercantile mercantile)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(mercantile.ConnectionName, MercantileQueries.FillData(mercantile, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a Mercantile object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an mercantile object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="mercantile">The mercantile to fill from history.</param>
        internal static void FillHistory(Mercantile mercantile, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(mercantile.ConnectionName, MercantileQueries.FillHistory(mercantile, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(mercantile, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a Mercantile object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the mercantile objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static MercantileCollection List(bool showDeleted, string connectionName)
        {
            MercantileCollection collection = new MercantileCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? MercantileQueries.ListDeleted() : MercantileQueries.List(), null);
                while (reader.Read())
                {
                    Mercantile mercantile = new Mercantile((long)reader["ID"]);
                    mercantile.ConnectionName = connectionName;
                    collection.Add(mercantile);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list Mercantile objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the mercantile objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? MercantileQueries.ListDeleted() : MercantileQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list Mercantile objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) mercantile object from the database.
        /// </summary>
        /// <param name="mercantile">The mercantile to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(Mercantile mercantile)
        {
            MercantileCollection collection = new MercantileCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(mercantile.ConnectionName, MercantileQueries.ListHistory(mercantile, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) Mercantile in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an mercantile object to the database.
        /// </summary>
        /// <param name="mercantile">The Mercantile object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the mercantile was saved successfull, otherwise, false.</returns>
        internal static bool Save(Mercantile mercantile)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (mercantile.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(mercantile.ConnectionName, MercantileQueries.Save(mercantile, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(mercantile.ConnectionName, MercantileQueries.Save(mercantile, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            mercantile.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= mercantile.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a Mercantile object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for mercantile objects in the database.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkbankid">The fkbankid search criteria.</param>
        /// <param name="fkbankbranchid">The fkbankbranchid search criteria.</param>
        /// <param name="accountnumber">The accountnumber search criteria.</param>
        /// <param name="accnumcheckstatus">The accnumcheckstatus search criteria.</param>
        /// <param name="accnumcheckmsg">The accnumcheckmsg search criteria.</param>
        /// <param name="accnumcheckmsgfull">The accnumcheckmsgfull search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static MercantileCollection Search(long? fksystemid, long? fkimportid, long? fkbankid, long? fkbankbranchid, string accountnumber, Byte? accnumcheckstatus, string accnumcheckmsg, string accnumcheckmsgfull, string connectionName)
        {
            MercantileCollection collection = new MercantileCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, MercantileQueries.Search(fksystemid, fkimportid, fkbankid, fkbankbranchid, accountnumber, accnumcheckstatus, accnumcheckmsg, accnumcheckmsgfull), null);
                while (reader.Read())
                {
                    Mercantile mercantile = new Mercantile((long)reader["ID"]);
                    mercantile.ConnectionName = connectionName;
                    collection.Add(mercantile);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for Mercantile objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for mercantile objects in the database.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkbankid">The fkbankid search criteria.</param>
        /// <param name="fkbankbranchid">The fkbankbranchid search criteria.</param>
        /// <param name="accountnumber">The accountnumber search criteria.</param>
        /// <param name="accnumcheckstatus">The accnumcheckstatus search criteria.</param>
        /// <param name="accnumcheckmsg">The accnumcheckmsg search criteria.</param>
        /// <param name="accnumcheckmsgfull">The accnumcheckmsgfull search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fksystemid, long? fkimportid, long? fkbankid, long? fkbankbranchid, string accountnumber, Byte? accnumcheckstatus, string accnumcheckmsg, string accnumcheckmsgfull, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, MercantileQueries.Search(fksystemid, fkimportid, fkbankid, fkbankbranchid, accountnumber, accnumcheckstatus, accnumcheckmsg, accnumcheckmsgfull), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for Mercantile objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 mercantile objects in the database.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkbankid">The fkbankid search criteria.</param>
        /// <param name="fkbankbranchid">The fkbankbranchid search criteria.</param>
        /// <param name="accountnumber">The accountnumber search criteria.</param>
        /// <param name="accnumcheckstatus">The accnumcheckstatus search criteria.</param>
        /// <param name="accnumcheckmsg">The accnumcheckmsg search criteria.</param>
        /// <param name="accnumcheckmsgfull">The accnumcheckmsgfull search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static Mercantile SearchOne(long? fksystemid, long? fkimportid, long? fkbankid, long? fkbankbranchid, string accountnumber, Byte? accnumcheckstatus, string accnumcheckmsg, string accnumcheckmsgfull, string connectionName)
        {
            Mercantile mercantile = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, MercantileQueries.Search(fksystemid, fkimportid, fkbankid, fkbankbranchid, accountnumber, accnumcheckstatus, accnumcheckmsg, accnumcheckmsgfull), null);
                if (reader.Read())
                {
                    mercantile = new Mercantile((long)reader["ID"]);
                    mercantile.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for Mercantile objects in the database", ex);
            }
            return mercantile;
        }
        #endregion
    }
}
