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
    /// Contains methods to fill, save and delete inpolicychild objects.
    /// </summary>
    public partial class INPolicyChildMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inpolicychild object from the database.
        /// </summary>
        /// <param name="inpolicychild">The id of the inpolicychild object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inpolicychild object.</param>
        /// <returns>True if the inpolicychild object was deleted successfully, else false.</returns>
        internal static bool Delete(INPolicyChild inpolicychild)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inpolicychild.ConnectionName, INPolicyChildQueries.Delete(inpolicychild, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INPolicyChild object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inpolicychild object from the database.
        /// </summary>
        /// <param name="inpolicychild">The id of the inpolicychild object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inpolicychild history.</param>
        /// <returns>True if the inpolicychild history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INPolicyChild inpolicychild)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inpolicychild.ConnectionName, INPolicyChildQueries.DeleteHistory(inpolicychild, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INPolicyChild history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inpolicychild object from the database.
        /// </summary>
        /// <param name="inpolicychild">The id of the inpolicychild object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inpolicychild object.</param>
        /// <returns>True if the inpolicychild object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INPolicyChild inpolicychild)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inpolicychild.ConnectionName, INPolicyChildQueries.UnDelete(inpolicychild, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INPolicyChild object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inpolicychild object from the data reader.
        /// </summary>
        /// <param name="inpolicychild">The inpolicychild object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INPolicyChild inpolicychild, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inpolicychild.IsLoaded = true;
                    inpolicychild.FKINPolicyID = reader["FKINPolicyID"] != DBNull.Value ? (long)reader["FKINPolicyID"] : (long?)null;
                    inpolicychild.FKINChildID = reader["FKINChildID"] != DBNull.Value ? (long)reader["FKINChildID"] : (long?)null;
                    inpolicychild.ChildRank = reader["ChildRank"] != DBNull.Value ? (int)reader["ChildRank"] : (int?)null;
                    inpolicychild.StampDate = (DateTime)reader["StampDate"];
                    inpolicychild.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INPolicyChild does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPolicyChild object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inpolicychild object from the database.
        /// </summary>
        /// <param name="inpolicychild">The inpolicychild to fill.</param>
        internal static void Fill(INPolicyChild inpolicychild)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inpolicychild.ConnectionName, INPolicyChildQueries.Fill(inpolicychild, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inpolicychild, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPolicyChild object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inpolicychild object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INPolicyChild inpolicychild)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inpolicychild.ConnectionName, INPolicyChildQueries.FillData(inpolicychild, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INPolicyChild object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inpolicychild object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inpolicychild">The inpolicychild to fill from history.</param>
        internal static void FillHistory(INPolicyChild inpolicychild, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inpolicychild.ConnectionName, INPolicyChildQueries.FillHistory(inpolicychild, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inpolicychild, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPolicyChild object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inpolicychild objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INPolicyChildCollection List(bool showDeleted, string connectionName)
        {
            INPolicyChildCollection collection = new INPolicyChildCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INPolicyChildQueries.ListDeleted() : INPolicyChildQueries.List(), null);
                while (reader.Read())
                {
                    INPolicyChild inpolicychild = new INPolicyChild((long)reader["ID"]);
                    inpolicychild.ConnectionName = connectionName;
                    collection.Add(inpolicychild);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INPolicyChild objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inpolicychild objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INPolicyChildQueries.ListDeleted() : INPolicyChildQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INPolicyChild objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inpolicychild object from the database.
        /// </summary>
        /// <param name="inpolicychild">The inpolicychild to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INPolicyChild inpolicychild)
        {
            INPolicyChildCollection collection = new INPolicyChildCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inpolicychild.ConnectionName, INPolicyChildQueries.ListHistory(inpolicychild, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INPolicyChild in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inpolicychild object to the database.
        /// </summary>
        /// <param name="inpolicychild">The INPolicyChild object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inpolicychild was saved successfull, otherwise, false.</returns>
        internal static bool Save(INPolicyChild inpolicychild)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inpolicychild.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inpolicychild.ConnectionName, INPolicyChildQueries.Save(inpolicychild, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inpolicychild.ConnectionName, INPolicyChildQueries.Save(inpolicychild, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inpolicychild.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inpolicychild.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INPolicyChild object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inpolicychild objects in the database.
        /// </summary>
        /// <param name="fkinpolicyid">The fkinpolicyid search criteria.</param>
        /// <param name="fkinchildid">The fkinchildid search criteria.</param>
        /// <param name="childrank">The childrank search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INPolicyChildCollection Search(long? fkinpolicyid, long? fkinchildid, int? childrank, string connectionName)
        {
            INPolicyChildCollection collection = new INPolicyChildCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INPolicyChildQueries.Search(fkinpolicyid, fkinchildid, childrank), null);
                while (reader.Read())
                {
                    INPolicyChild inpolicychild = new INPolicyChild((long)reader["ID"]);
                    inpolicychild.ConnectionName = connectionName;
                    collection.Add(inpolicychild);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPolicyChild objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inpolicychild objects in the database.
        /// </summary>
        /// <param name="fkinpolicyid">The fkinpolicyid search criteria.</param>
        /// <param name="fkinchildid">The fkinchildid search criteria.</param>
        /// <param name="childrank">The childrank search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinpolicyid, long? fkinchildid, int? childrank, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INPolicyChildQueries.Search(fkinpolicyid, fkinchildid, childrank), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPolicyChild objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inpolicychild objects in the database.
        /// </summary>
        /// <param name="fkinpolicyid">The fkinpolicyid search criteria.</param>
        /// <param name="fkinchildid">The fkinchildid search criteria.</param>
        /// <param name="childrank">The childrank search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INPolicyChild SearchOne(long? fkinpolicyid, long? fkinchildid, int? childrank, string connectionName)
        {
            INPolicyChild inpolicychild = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INPolicyChildQueries.Search(fkinpolicyid, fkinchildid, childrank), null);
                if (reader.Read())
                {
                    inpolicychild = new INPolicyChild((long)reader["ID"]);
                    inpolicychild.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPolicyChild objects in the database", ex);
            }
            return inpolicychild;
        }
        #endregion
    }
}
