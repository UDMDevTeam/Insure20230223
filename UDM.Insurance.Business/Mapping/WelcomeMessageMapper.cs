using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using Embriant.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDM.Insurance.Business.Queries;

namespace UDM.Insurance.Business.Mapping
{
    public partial class WelcomeMessageMapper
    {
        #region Delete

        internal static bool Delete(WelcomMessage welcomeMessage)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(welcomeMessage.ConnectionName, WelcomeMessageQueries.Delete(welcomeMessage, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a WelcomeMessage object from the database", ex);
            }
            return affectedRows > 0;
        }

        internal static bool DeleteHistory(WelcomMessage welcomeMessage)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(welcomeMessage.ConnectionName, WelcomeMessageQueries.DeleteHistory(welcomeMessage, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete WelcomeMessage history from the database", ex);
            }
            return affectedRows > 0;
        }

        internal static bool UnDelete(WelcomMessage welcomeMessage)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(welcomeMessage.ConnectionName, WelcomeMessageQueries.UnDelete(welcomeMessage, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw  new MapperException("Failed to undelete a WelcomeMessage object from the database", ex);
            }
            return affectedRows > 0;
        }

        #endregion

        #region Fill

        private static void Fill(WelcomMessage welcomeMessage, IDataReader reader)
        {
            try
            {
                if (reader.Read()) // Ensure there is data to read
                {
                    welcomeMessage.IsLoaded = true;
                    welcomeMessage.ID = reader["ID"] != DBNull.Value ? (long)reader["ID"] : 0;
                    welcomeMessage.TextMessage = reader["TextMessage"] != DBNull.Value ? (string)reader["TextMessage"] : null;
                    welcomeMessage.StampDate = reader["StampDate"] != DBNull.Value ? (DateTime)reader["StampDate"] : DateTime.MinValue;
                    welcomeMessage.StampUserID = reader["StampUserID"] != DBNull.Value ? (int)reader["StampUserID"] : 0;
                    welcomeMessage.IsActive = reader["IsActive"] != DBNull.Value ? (bool)reader["IsActive"] : false;
                    welcomeMessage.PDFName = reader["PDFName"] != DBNull.Value ? (string)reader["PDFName"] : null;
                    welcomeMessage.HasChanged = false; // Assuming HasChanged flags if any property was modified
                }
                else
                {
                    throw new MapperException("WelcomeMessage does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw  new MapperException("Failed to populate a WelcomeMessage object from the database", ex);
            }
        }

        internal static void Fill(WelcomMessage welcomeMessage)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(welcomeMessage.ConnectionName, WelcomeMessageQueries.Fill(welcomeMessage, ref parameters), parameters);
                if (reader != null && reader.Read())
                {
                    Fill(welcomeMessage, reader);
                }
                reader?.Close();
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a WelcomeMessage object from the database", ex);
            }
        }

        public static DataSet FillData(WelcomMessage welcomeMessage)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(welcomeMessage.ConnectionName, WelcomeMessageQueries.FillData(welcomeMessage, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a WelcomeMessage object from the database", ex);
            }
        }

        internal static void FillHistory(WelcomMessage welcomeMessage, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(welcomeMessage.ConnectionName, WelcomeMessageQueries.FillHistory(welcomeMessage, stampUserID, stampDate, ref parameters), parameters);
                if (reader != null && reader.Read())
                {
                    Fill(welcomeMessage, reader);
                }
                reader?.Close();
            }
            catch (Exception ex)
            {
                throw  new MapperException("Failed to populate a WelcomeMessage object from the database", ex);
            }
        }

        #endregion

        #region List

        public static WelcomMessageCollection List(bool showDeleted, string connectionName)
        {
            WelcomMessageCollection collection = new WelcomMessageCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? WelcomeMessageQueries.ListDeleted() : WelcomeMessageQueries.List(), null);
                while (reader.Read())
                {
                    WelcomMessage welcomeMessage = new WelcomMessage()
                    {
                        ID = (long)reader["ID"],
                        TextMessage = reader["TextMessage"].ToString(),
                        StampDate = Convert.ToDateTime(reader["StampDate"]),
                        StampUserID = Convert.ToInt32(reader["StampUserID"]),
                        IsActive = Convert.ToBoolean(reader["IsActive"]),
                        PDFName = reader["PDFName"].ToString(),
                        ConnectionName = connectionName
                    };
                    collection.Add(welcomeMessage);
                }
                reader?.Close();
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list WelcomeMessage objects in the database", ex);
            }
            return collection;
        }

        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? WelcomeMessageQueries.ListDeleted() : WelcomeMessageQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list WelcomeMessage objects in the database", ex);
            }
        }

        public static DataSet ListHistory(WelcomMessage welcomeMessage)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(welcomeMessage.ConnectionName, WelcomeMessageQueries.ListHistory(welcomeMessage, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw  new MapperException("Failed to list history for a WelcomeMessage in the database", ex);
            }
        }

        #endregion

        #region Save
        public static bool UpdateIsActive(WelcomMessage welcomeMessage,long welcomeMessageId, bool isActive)
        {
            try
            {
                // Prepare the SQL query and parameters
                string query = "UPDATE [WelcomeMessage] SET [IsActive] = @IsActive WHERE [ID] = @ID;";
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@IsActive", isActive),
            new SqlParameter("@ID", welcomeMessageId)
                };

                // Execute the command
                int affectedRows = Database.ExecuteCommand(welcomeMessage.ConnectionName, query, parameters);

                // Check if the command was successful
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new MapperException("Failed to update IsActive status for WelcomeMessage.", ex);
            }
        }

        public static bool Save(WelcomMessage welcomeMessage)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (welcomeMessage.IsLoaded)
                {
                    // Assuming IsLoaded means the object has an ID and exists in the DB
                    int affectedRows = Database.ExecuteCommand(welcomeMessage.ConnectionName, WelcomeMessageQueries.Save(welcomeMessage, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    // New object scenario
                    object newId = Database.ExecuteScalar(welcomeMessage.ConnectionName, WelcomeMessageQueries.Save(welcomeMessage, ref parameters), parameters);
                    if (newId != null)
                    {
                        welcomeMessage.ID = Convert.ToInt64(newId);
                        result = welcomeMessage.ID > 0;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a WelcomeMessage object to the database", ex);
            }
            return result;
        }

        #endregion

        #region Search

        public static WelcomMessageCollection Search(string textMessage, bool isActive, string connectionName)
        {
            WelcomMessageCollection collection = new WelcomMessageCollection();
            try
            {
                object[] parameters = new object[]
                {
                    new SqlParameter("@TextMessage", $"%{textMessage}%"),
                    new SqlParameter("@IsActive", isActive)
                };

                IDataReader reader = Database.ExecuteReader(connectionName, WelcomeMessageQueries.Search(textMessage, isActive, ref parameters), parameters);
                while (reader != null && reader.Read())
                {
                    WelcomMessage welcomeMessage = new WelcomMessage()
                    {
                        ID = (long)reader["ID"],
                        TextMessage = reader["TextMessage"].ToString(),
                        StampDate = Convert.ToDateTime(reader["StampDate"]),
                        StampUserID = Convert.ToInt32(reader["StampUserID"]),
                        IsActive = Convert.ToBoolean(reader["IsActive"]),
                        PDFName = reader["PDFName"].ToString(),
                        ConnectionName = connectionName
                    };
                    collection.Add(welcomeMessage);
                }
                reader?.Close();
            }
            catch (Exception ex)
            {
                throw new MapperException($"Failed to search for WelcomeMessage objects in the database: {ex.Message}", ex);
            }
            return collection;
        }

        #endregion
    }
}
