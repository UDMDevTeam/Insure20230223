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
    /// Contains methods to fill, save and delete inpolicyoption objects.
    /// </summary>
    public partial class INPolicyOptionMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inpolicyoption object from the database.
        /// </summary>
        /// <param name="inpolicyoption">The id of the inpolicyoption object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inpolicyoption object.</param>
        /// <returns>True if the inpolicyoption object was deleted successfully, else false.</returns>
        internal static bool Delete(INPolicyOption inpolicyoption)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inpolicyoption.ConnectionName, INPolicyOptionQueries.Delete(inpolicyoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INPolicyOption object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inpolicyoption object from the database.
        /// </summary>
        /// <param name="inpolicyoption">The id of the inpolicyoption object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inpolicyoption history.</param>
        /// <returns>True if the inpolicyoption history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INPolicyOption inpolicyoption)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inpolicyoption.ConnectionName, INPolicyOptionQueries.DeleteHistory(inpolicyoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INPolicyOption history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inpolicyoption object from the database.
        /// </summary>
        /// <param name="inpolicyoption">The id of the inpolicyoption object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inpolicyoption object.</param>
        /// <returns>True if the inpolicyoption object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INPolicyOption inpolicyoption)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inpolicyoption.ConnectionName, INPolicyOptionQueries.UnDelete(inpolicyoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INPolicyOption object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inpolicyoption object from the data reader.
        /// </summary>
        /// <param name="inpolicyoption">The inpolicyoption object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INPolicyOption inpolicyoption, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inpolicyoption.IsLoaded = true;
                    inpolicyoption.FKPolicyID = reader["FKPolicyID"] != DBNull.Value ? (long)reader["FKPolicyID"] : (long?)null;
                    inpolicyoption.FKOptionID = reader["FKOptionID"] != DBNull.Value ? (long)reader["FKOptionID"] : (long?)null;
                    inpolicyoption.IsLA2Selected = reader["IsLA2Selected"] != DBNull.Value ? (Byte)reader["IsLA2Selected"] : (Byte?)null;
                    inpolicyoption.IsChildSelected = reader["IsChildSelected"] != DBNull.Value ? (Byte)reader["IsChildSelected"] : (Byte?)null;
                    inpolicyoption.StampDate = (DateTime)reader["StampDate"];
                    inpolicyoption.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INPolicyOption does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPolicyOption object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inpolicyoption object from the database.
        /// </summary>
        /// <param name="inpolicyoption">The inpolicyoption to fill.</param>
        internal static void Fill(INPolicyOption inpolicyoption)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inpolicyoption.ConnectionName, INPolicyOptionQueries.Fill(inpolicyoption, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inpolicyoption, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPolicyOption object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inpolicyoption object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INPolicyOption inpolicyoption)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inpolicyoption.ConnectionName, INPolicyOptionQueries.FillData(inpolicyoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INPolicyOption object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inpolicyoption object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inpolicyoption">The inpolicyoption to fill from history.</param>
        internal static void FillHistory(INPolicyOption inpolicyoption, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inpolicyoption.ConnectionName, INPolicyOptionQueries.FillHistory(inpolicyoption, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inpolicyoption, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPolicyOption object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inpolicyoption objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INPolicyOptionCollection List(bool showDeleted, string connectionName)
        {
            INPolicyOptionCollection collection = new INPolicyOptionCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INPolicyOptionQueries.ListDeleted() : INPolicyOptionQueries.List(), null);
                while (reader.Read())
                {
                    INPolicyOption inpolicyoption = new INPolicyOption((long)reader["ID"]);
                    inpolicyoption.ConnectionName = connectionName;
                    collection.Add(inpolicyoption);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INPolicyOption objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inpolicyoption objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INPolicyOptionQueries.ListDeleted() : INPolicyOptionQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INPolicyOption objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inpolicyoption object from the database.
        /// </summary>
        /// <param name="inpolicyoption">The inpolicyoption to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INPolicyOption inpolicyoption)
        {
            INPolicyOptionCollection collection = new INPolicyOptionCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inpolicyoption.ConnectionName, INPolicyOptionQueries.ListHistory(inpolicyoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INPolicyOption in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inpolicyoption object to the database.
        /// </summary>
        /// <param name="inpolicyoption">The INPolicyOption object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inpolicyoption was saved successfull, otherwise, false.</returns>
        internal static bool Save(INPolicyOption inpolicyoption)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inpolicyoption.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inpolicyoption.ConnectionName, INPolicyOptionQueries.Save(inpolicyoption, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inpolicyoption.ConnectionName, INPolicyOptionQueries.Save(inpolicyoption, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inpolicyoption.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inpolicyoption.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INPolicyOption object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inpolicyoption objects in the database.
        /// </summary>
        /// <param name="fkpolicyid">The fkpolicyid search criteria.</param>
        /// <param name="fkoptionid">The fkoptionid search criteria.</param>
        /// <param name="isla2selected">The isla2selected search criteria.</param>
        /// <param name="ischildselected">The ischildselected search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INPolicyOptionCollection Search(long? fkpolicyid, long? fkoptionid, Byte? isla2selected, Byte? ischildselected, string connectionName)
        {
            INPolicyOptionCollection collection = new INPolicyOptionCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INPolicyOptionQueries.Search(fkpolicyid, fkoptionid, isla2selected, ischildselected), null);
                while (reader.Read())
                {
                    INPolicyOption inpolicyoption = new INPolicyOption((long)reader["ID"]);
                    inpolicyoption.ConnectionName = connectionName;
                    collection.Add(inpolicyoption);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPolicyOption objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inpolicyoption objects in the database.
        /// </summary>
        /// <param name="fkpolicyid">The fkpolicyid search criteria.</param>
        /// <param name="fkoptionid">The fkoptionid search criteria.</param>
        /// <param name="isla2selected">The isla2selected search criteria.</param>
        /// <param name="ischildselected">The ischildselected search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkpolicyid, long? fkoptionid, Byte? isla2selected, Byte? ischildselected, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INPolicyOptionQueries.Search(fkpolicyid, fkoptionid, isla2selected, ischildselected), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPolicyOption objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inpolicyoption objects in the database.
        /// </summary>
        /// <param name="fkpolicyid">The fkpolicyid search criteria.</param>
        /// <param name="fkoptionid">The fkoptionid search criteria.</param>
        /// <param name="isla2selected">The isla2selected search criteria.</param>
        /// <param name="ischildselected">The ischildselected search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INPolicyOption SearchOne(long? fkpolicyid, long? fkoptionid, Byte? isla2selected, Byte? ischildselected, string connectionName)
        {
            INPolicyOption inpolicyoption = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INPolicyOptionQueries.Search(fkpolicyid, fkoptionid, isla2selected, ischildselected), null);
                if (reader.Read())
                {
                    inpolicyoption = new INPolicyOption((long)reader["ID"]);
                    inpolicyoption.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPolicyOption objects in the database", ex);
            }
            return inpolicyoption;
        }
        #endregion
    }
}
