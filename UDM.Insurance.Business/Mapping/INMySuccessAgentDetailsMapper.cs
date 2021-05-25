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
    /// Contains methods to fill, save and delete closure objects.
    /// </summary>
    public partial class INMySuccessAgentDetailsMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) closure object from the database.
        /// </summary>
        /// <param name="document">The id of the closure object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the closure object.</param>
        /// <returns>True if the closure object was deleted successfully, else false.</returns>
        internal static bool Delete(INMySuccessAgentDetails iNMySuccessAgentDetails)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(iNMySuccessAgentDetails.ConnectionName, INMySuccessAgentDetailsQueries.Delete(iNMySuccessAgentDetails, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a Document object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) closure object from the database.
        /// </summary>
        /// <param name="document">The id of the closure object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the closure history.</param>
        /// <returns>True if the closure history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INMySuccessAgentDetails iNMySuccessAgentDetails)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(iNMySuccessAgentDetails.ConnectionName, INMySuccessAgentDetailsQueries.DeleteHistory(iNMySuccessAgentDetails, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete Document history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) closure object from the database.
        /// </summary>
        /// <param name="document">The id of the closure object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the closure object.</param>
        /// <returns>True if the closure object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INMySuccessAgentDetails iNMySuccessAgentDetails)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(iNMySuccessAgentDetails.ConnectionName, INMySuccessAgentDetailsQueries.UnDelete(iNMySuccessAgentDetails, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a Document object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the closure object from the data reader.
        /// </summary>
        /// <param name="document">The closure object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INMySuccessAgentDetails iNMySuccessAgentDetails, IDataReader reader)
        {
            try
            {

                if (reader["ID"] != DBNull.Value)
                {

                    if (iNMySuccessAgentDetails.DocumentID == 1)
                    {
                        iNMySuccessAgentDetails.IsLoaded = true;
                        iNMySuccessAgentDetails.FKCampaignID = reader["FKCampaignID"] != DBNull.Value ? (long)reader["FKCampaignID"] : (long?)null;
                        iNMySuccessAgentDetails.Document = reader["Document"] != DBNull.Value ? (byte[])reader["Document"] : (byte[])null;
                        iNMySuccessAgentDetails.StampDate = (DateTime)reader["StampDate"];
                        iNMySuccessAgentDetails.HasChanged = false;
                    }
                    else if (iNMySuccessAgentDetails.DocumentID == 2)
                    {
                        iNMySuccessAgentDetails.IsLoaded = true;
                        iNMySuccessAgentDetails.FKCampaignID = reader["FKCampaignID"] != DBNull.Value ? (long)reader["FKCampaignID"] : (long?)null;
                        iNMySuccessAgentDetails.Document = reader["Document"] != DBNull.Value ? (byte[])reader["Document"] : (byte[])null;
                        iNMySuccessAgentDetails.StampDate = (DateTime)reader["StampDate"];
                        iNMySuccessAgentDetails.HasChanged = false;
                    }
                    else if (iNMySuccessAgentDetails.DocumentID == 3)
                    {
                        iNMySuccessAgentDetails.IsLoaded = true;
                        iNMySuccessAgentDetails.FKCampaignID = reader["FKCampaignID"] != DBNull.Value ? (long)reader["FKCampaignID"] : (long?)null;
                        iNMySuccessAgentDetails.Document = reader["Document"] != DBNull.Value ? (byte[])reader["Document"] : (byte[])null;
                        iNMySuccessAgentDetails.StampDate = (DateTime)reader["StampDate"];
                        iNMySuccessAgentDetails.HasChanged = false;
                    }
                    else if (iNMySuccessAgentDetails.DocumentID == 4)
                    {
                        iNMySuccessAgentDetails.IsLoaded = true;
                        iNMySuccessAgentDetails.FKCampaignID = reader["FKCampaignID"] != DBNull.Value ? (long)reader["FKCampaignID"] : (long?)null;
                        iNMySuccessAgentDetails.Document = reader["Document"] != DBNull.Value ? (byte[])reader["Document"] : (byte[])null;
                        iNMySuccessAgentDetails.StampDate = (DateTime)reader["StampDate"];
                        iNMySuccessAgentDetails.HasChanged = false;
                    }
                    else if (iNMySuccessAgentDetails.DocumentID == 5)
                    {
                        iNMySuccessAgentDetails.IsLoaded = true;
                        iNMySuccessAgentDetails.FKCampaignID = reader["FKCampaignID"] != DBNull.Value ? (long)reader["FKCampaignID"] : (long?)null;
                        iNMySuccessAgentDetails.Document = reader["Document"] != DBNull.Value ? (byte[])reader["Document"] : (byte[])null;
                        iNMySuccessAgentDetails.StampDate = (DateTime)reader["StampDate"];
                        iNMySuccessAgentDetails.HasChanged = false;
                    }
                    else if (iNMySuccessAgentDetails.DocumentID == 6)
                    {
                        iNMySuccessAgentDetails.IsLoaded = true;
                        iNMySuccessAgentDetails.FKCampaignID = reader["FKCampaignID"] != DBNull.Value ? (long)reader["FKCampaignID"] : (long?)null;
                        iNMySuccessAgentDetails.Document = reader["Document"] != DBNull.Value ? (byte[])reader["Document"] : (byte[])null;
                        iNMySuccessAgentDetails.StampDate = (DateTime)reader["StampDate"];
                        iNMySuccessAgentDetails.HasChanged = false;
                    }
                }
                else
                {
                    throw new MapperException("Document does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a Document object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an closure object from the database.
        /// </summary>
        /// <param name="document">The closure to fill.</param>
        internal static void Fill(INMySuccessAgentDetails iNMySuccessAgentDetails)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(iNMySuccessAgentDetails.ConnectionName, INMySuccessAgentDetailsQueries.Fill(iNMySuccessAgentDetails, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(iNMySuccessAgentDetails, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a Document object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a closure object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INMySuccessAgentDetails iNMySuccessAgentDetails)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(iNMySuccessAgentDetails.ConnectionName, INMySuccessAgentDetailsQueries.FillData(iNMySuccessAgentDetails, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a Document object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an closure object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="iNMySuccessAgentDetails">The closure to fill from history.</param>
        internal static void FillHistory(INMySuccessAgentDetails iNMySuccessAgentDetails, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(iNMySuccessAgentDetails.ConnectionName, INMySuccessAgentDetailsQueries.FillHistory(iNMySuccessAgentDetails, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(iNMySuccessAgentDetails, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a Document object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the closure objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INMySuccessAgentDetailsCollection List(bool showDeleted, string connectionName)
        {
            INMySuccessAgentDetailsCollection collection = new INMySuccessAgentDetailsCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INMySuccessAgentDetailsQueries.ListDeleted() : INMySuccessAgentDetailsQueries.List(), null);
                while (reader.Read())
                {
                    INMySuccessAgentDetails iNMySuccessAgentDetails = new INMySuccessAgentDetails((long)reader["ID"]);
                    iNMySuccessAgentDetails.ConnectionName = connectionName;
                    collection.Add(iNMySuccessAgentDetails);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list Document objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the closure objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INMySuccessAgentDetailsQueries.ListDeleted() : INMySuccessAgentDetailsQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list Document objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) closure object from the database.
        /// </summary>
        /// <param name="document">The closure to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INMySuccessAgentDetails iNMySuccessAgentDetails)
        {
            INMySuccessAgentDetailsCollection collection = new INMySuccessAgentDetailsCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(iNMySuccessAgentDetails.ConnectionName, INMySuccessAgentDetailsQueries.ListHistory(iNMySuccessAgentDetails, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) Document in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an closure object to the database.
        /// </summary>
        /// <param name="document">The Closure object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the closure was saved successfull, otherwise, false.</returns>
        internal static bool Save(INMySuccessAgentDetails iNMySuccessAgentDetails)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                //string strSQL = "Select FKCampaignID FROM INMySuccessCampaignDetails"; 

                if (iNMySuccessAgentDetails.ID != 0)
                {
                    iNMySuccessAgentDetails.IsLoaded = true;
                }

                if (iNMySuccessAgentDetails.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(iNMySuccessAgentDetails.ConnectionName, INMySuccessAgentDetailsQueries.Save(iNMySuccessAgentDetails, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(iNMySuccessAgentDetails.ConnectionName, INMySuccessAgentDetailsQueries.Save(iNMySuccessAgentDetails, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            iNMySuccessAgentDetails.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= iNMySuccessAgentDetails.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a Document object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for closure objects in the database.
        /// </summary>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="document">The document search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INMySuccessAgentDetailsCollection Search(long? fkcampaignid, byte[] document, string connectionName)
        {
            INMySuccessAgentDetailsCollection collection = new INMySuccessAgentDetailsCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INMySuccessAgentDetailsQueries.Search(fkcampaignid, document), null);
                while (reader.Read())
                {
                    INMySuccessAgentDetails iNMySuccessAgentDetails = new INMySuccessAgentDetails((long)reader["ID"]);
                    iNMySuccessAgentDetails.ConnectionName = connectionName;
                    collection.Add(iNMySuccessAgentDetails);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for Document objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for closure objects in the database.
        /// </summary>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="documentid">The fklanguageid search criteria.</param>
        /// <param name="document">The document search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkcampaignid, byte[] document, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INMySuccessAgentDetailsQueries.Search(fkcampaignid, document), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for Document objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 closure objects in the database.
        /// </summary>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="document">The document search criteria.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INMySuccessAgentDetails SearchOne(long? fkcampaignid, byte[] document, string connectionName)
        {
            INMySuccessAgentDetails iNMySuccessAgentDetails = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INMySuccessAgentDetailsQueries.Search(fkcampaignid, document), null);
                if (reader.Read())
                {
                    iNMySuccessAgentDetails = new INMySuccessAgentDetails((long)reader["ID"]);
                    iNMySuccessAgentDetails.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for Document objects in the database", ex);
            }
            return iNMySuccessAgentDetails;
        }
        #endregion
    }
}