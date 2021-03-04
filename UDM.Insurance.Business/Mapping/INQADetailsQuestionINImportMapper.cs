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
    /// Contains methods to fill, save and delete inqadetailsquestioninimport objects.
    /// </summary>
    public partial class INQADetailsQuestionINImportMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inqadetailsquestioninimport object from the database.
        /// </summary>
        /// <param name="inqadetailsquestioninimport">The id of the inqadetailsquestioninimport object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inqadetailsquestioninimport object.</param>
        /// <returns>True if the inqadetailsquestioninimport object was deleted successfully, else false.</returns>
        internal static bool Delete(INQADetailsQuestionINImport inqadetailsquestioninimport)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inqadetailsquestioninimport.ConnectionName, INQADetailsQuestionINImportQueries.Delete(inqadetailsquestioninimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INQADetailsQuestionINImport object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inqadetailsquestioninimport object from the database.
        /// </summary>
        /// <param name="inqadetailsquestioninimport">The id of the inqadetailsquestioninimport object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inqadetailsquestioninimport history.</param>
        /// <returns>True if the inqadetailsquestioninimport history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INQADetailsQuestionINImport inqadetailsquestioninimport)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inqadetailsquestioninimport.ConnectionName, INQADetailsQuestionINImportQueries.DeleteHistory(inqadetailsquestioninimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INQADetailsQuestionINImport history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inqadetailsquestioninimport object from the database.
        /// </summary>
        /// <param name="inqadetailsquestioninimport">The id of the inqadetailsquestioninimport object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inqadetailsquestioninimport object.</param>
        /// <returns>True if the inqadetailsquestioninimport object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INQADetailsQuestionINImport inqadetailsquestioninimport)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inqadetailsquestioninimport.ConnectionName, INQADetailsQuestionINImportQueries.UnDelete(inqadetailsquestioninimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INQADetailsQuestionINImport object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inqadetailsquestioninimport object from the data reader.
        /// </summary>
        /// <param name="inqadetailsquestioninimport">The inqadetailsquestioninimport object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INQADetailsQuestionINImport inqadetailsquestioninimport, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inqadetailsquestioninimport.IsLoaded = true;
                    inqadetailsquestioninimport.FKImportID = reader["FKImportID"] != DBNull.Value ? (long)reader["FKImportID"] : (long?)null;
                    inqadetailsquestioninimport.FKQuestionID = reader["FKQuestionID"] != DBNull.Value ? (long)reader["FKQuestionID"] : (long?)null;
                    inqadetailsquestioninimport.AnswerInt = reader["AnswerInt"] != DBNull.Value ? (long)reader["AnswerInt"] : (long?)null;
                    inqadetailsquestioninimport.AnswerDateTime = reader["AnswerDateTime"] != DBNull.Value ? (DateTime)reader["AnswerDateTime"] : (DateTime?)null;
                    inqadetailsquestioninimport.AnswerText = reader["AnswerText"] != DBNull.Value ? (string)reader["AnswerText"] : (string)null;
                    inqadetailsquestioninimport.StampDate = (DateTime)reader["StampDate"];
                    inqadetailsquestioninimport.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INQADetailsQuestionINImport does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INQADetailsQuestionINImport object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inqadetailsquestioninimport object from the database.
        /// </summary>
        /// <param name="inqadetailsquestioninimport">The inqadetailsquestioninimport to fill.</param>
        internal static void Fill(INQADetailsQuestionINImport inqadetailsquestioninimport)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inqadetailsquestioninimport.ConnectionName, INQADetailsQuestionINImportQueries.Fill(inqadetailsquestioninimport, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inqadetailsquestioninimport, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INQADetailsQuestionINImport object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inqadetailsquestioninimport object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INQADetailsQuestionINImport inqadetailsquestioninimport)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inqadetailsquestioninimport.ConnectionName, INQADetailsQuestionINImportQueries.FillData(inqadetailsquestioninimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INQADetailsQuestionINImport object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inqadetailsquestioninimport object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inqadetailsquestioninimport">The inqadetailsquestioninimport to fill from history.</param>
        internal static void FillHistory(INQADetailsQuestionINImport inqadetailsquestioninimport, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inqadetailsquestioninimport.ConnectionName, INQADetailsQuestionINImportQueries.FillHistory(inqadetailsquestioninimport, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inqadetailsquestioninimport, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INQADetailsQuestionINImport object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inqadetailsquestioninimport objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INQADetailsQuestionINImportCollection List(bool showDeleted, string connectionName)
        {
            INQADetailsQuestionINImportCollection collection = new INQADetailsQuestionINImportCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INQADetailsQuestionINImportQueries.ListDeleted() : INQADetailsQuestionINImportQueries.List(), null);
                while (reader.Read())
                {
                    INQADetailsQuestionINImport inqadetailsquestioninimport = new INQADetailsQuestionINImport((long)reader["ID"]);
                    inqadetailsquestioninimport.ConnectionName = connectionName;
                    collection.Add(inqadetailsquestioninimport);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INQADetailsQuestionINImport objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inqadetailsquestioninimport objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INQADetailsQuestionINImportQueries.ListDeleted() : INQADetailsQuestionINImportQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INQADetailsQuestionINImport objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inqadetailsquestioninimport object from the database.
        /// </summary>
        /// <param name="inqadetailsquestioninimport">The inqadetailsquestioninimport to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INQADetailsQuestionINImport inqadetailsquestioninimport)
        {
            INQADetailsQuestionINImportCollection collection = new INQADetailsQuestionINImportCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inqadetailsquestioninimport.ConnectionName, INQADetailsQuestionINImportQueries.ListHistory(inqadetailsquestioninimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INQADetailsQuestionINImport in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inqadetailsquestioninimport object to the database.
        /// </summary>
        /// <param name="inqadetailsquestioninimport">The INQADetailsQuestionINImport object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inqadetailsquestioninimport was saved successfull, otherwise, false.</returns>
        internal static bool Save(INQADetailsQuestionINImport inqadetailsquestioninimport)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inqadetailsquestioninimport.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inqadetailsquestioninimport.ConnectionName, INQADetailsQuestionINImportQueries.Save(inqadetailsquestioninimport, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inqadetailsquestioninimport.ConnectionName, INQADetailsQuestionINImportQueries.Save(inqadetailsquestioninimport, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inqadetailsquestioninimport.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inqadetailsquestioninimport.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INQADetailsQuestionINImport object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inqadetailsquestioninimport objects in the database.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkquestionid">The fkquestionid search criteria.</param>
        /// <param name="answerint">The answerint search criteria.</param>
        /// <param name="answerdatetime">The answerdatetime search criteria.</param>
        /// <param name="answertext">The answertext search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INQADetailsQuestionINImportCollection Search(long? fkimportid, long? fkquestionid, long? answerint, DateTime? answerdatetime, string answertext, string connectionName)
        {
            INQADetailsQuestionINImportCollection collection = new INQADetailsQuestionINImportCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INQADetailsQuestionINImportQueries.Search(fkimportid, fkquestionid, answerint, answerdatetime, answertext), null);
                while (reader.Read())
                {
                    INQADetailsQuestionINImport inqadetailsquestioninimport = new INQADetailsQuestionINImport((long)reader["ID"]);
                    inqadetailsquestioninimport.ConnectionName = connectionName;
                    collection.Add(inqadetailsquestioninimport);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INQADetailsQuestionINImport objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inqadetailsquestioninimport objects in the database.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkquestionid">The fkquestionid search criteria.</param>
        /// <param name="answerint">The answerint search criteria.</param>
        /// <param name="answerdatetime">The answerdatetime search criteria.</param>
        /// <param name="answertext">The answertext search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkimportid, long? fkquestionid, long? answerint, DateTime? answerdatetime, string answertext, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INQADetailsQuestionINImportQueries.Search(fkimportid, fkquestionid, answerint, answerdatetime, answertext), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INQADetailsQuestionINImport objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inqadetailsquestioninimport objects in the database.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkquestionid">The fkquestionid search criteria.</param>
        /// <param name="answerint">The answerint search criteria.</param>
        /// <param name="answerdatetime">The answerdatetime search criteria.</param>
        /// <param name="answertext">The answertext search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INQADetailsQuestionINImport SearchOne(long? fkimportid, long? fkquestionid, long? answerint, DateTime? answerdatetime, string answertext, string connectionName)
        {
            INQADetailsQuestionINImport inqadetailsquestioninimport = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INQADetailsQuestionINImportQueries.Search(fkimportid, fkquestionid, answerint, answerdatetime, answertext), null);
                if (reader.Read())
                {
                    inqadetailsquestioninimport = new INQADetailsQuestionINImport((long)reader["ID"]);
                    inqadetailsquestioninimport.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INQADetailsQuestionINImport objects in the database", ex);
            }
            return inqadetailsquestioninimport;
        }
        #endregion
    }
}
