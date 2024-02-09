using Embriant.Framework.Data;
using Embriant.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDM.Insurance.Business.Objects;
using UDM.Insurance.Business.Queries;

namespace UDM.Insurance.Business.Mapping
{
    public static class INTopDCAgentsMapper
    {
        public static bool InsertOrUpdate(INTopDCAgents agentData)
        {
            object[] parameters = null;
            string query = INTopDCAgentsQueries.InsertOrUpdate(agentData, ref parameters);

            // Assuming Database is a utility class for executing commands
            int affectedRows = Database.ExecuteCommand(agentData.ConnectionName, query, parameters);
            return affectedRows > 0;
        }

        public static bool Delete(INTopDCAgents agentData)
        {
            object[] parameters = null;
            string query = INTopDCAgentsQueries.Delete(agentData, ref parameters);

            int affectedRows = Database.ExecuteCommand(agentData.ConnectionName, query, parameters);
            return affectedRows > 0;
        }

        public static INTopDCAgentsCollection List(bool showDeleted, string connectionName)
        {
            INTopDCAgentsCollection collection = new INTopDCAgentsCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INTopDCAgentsQueries.ListDeleted() : INTopDCAgentsQueries.List(), null);
                while (reader.Read())
                {
                    INTopDCAgents refdata = new INTopDCAgents((long)reader["ID"]);
                    refdata.ConnectionName = connectionName;
                    collection.Add(refdata);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list TOP DC Agents objects in the database", ex);
            }
            return collection;
        }
        private static void Fill(INTopDCAgents agentData, IDataReader reader)
        {
            try
            {
                if (reader.Read()) // Assuming you want to read the first row of the result
                {
                    agentData.IsLoaded = true; // Assuming an IsLoaded property indicates the object has been populated
                    agentData.ID = (long)reader["ID"];
                    agentData.FKUserID = reader["FKUserID"] != DBNull.Value ? (int)reader["FKUserID"] : 0;
                    agentData.CampaignType = reader["CampaignType"] != DBNull.Value ? (int)reader["CampaignType"] : 0;
                    agentData.StampUserID = reader["StampUserID"] != DBNull.Value ? (long)reader["StampUserID"] : 0;
                    agentData.AcceptedRates = reader["AcceptedRates"] != DBNull.Value ? (decimal)reader["AcceptedRates"] : 0m;
                    agentData.StampDate = reader["StampDate"] != DBNull.Value ? (DateTime)reader["StampDate"] : DateTime.MinValue;


                    agentData.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INTopDCAgents does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate an INTopDCAgents object from the database", ex);
            }
        }
        internal static void Fill(INTopDCAgents agentData)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(agentData.ConnectionName, INTopDCAgentsQueries.Fill(agentData, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(agentData, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a Referral object from the database", ex);
            }
        }
    

        public static bool Save(INTopDCAgents agentData)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (agentData.IsLoaded) 
                {
                   
                    int affectedRows = Database.ExecuteCommand(agentData.ConnectionName, INTopDCAgentsQueries.InsertOrUpdate(agentData, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                  
                    using (IDataReader reader = Database.ExecuteReader(agentData.ConnectionName, INTopDCAgentsQueries.InsertOrUpdate(agentData, ref parameters), parameters))
                    {
                        if (reader != null && reader.Read())
                        {
                    
                            agentData.ID = (long)reader["NewID"];
                        }
                    }
                    result &= agentData.ID != 0;
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                throw new MapperException($"Failed to save an INTopDCAgents object to the database: {ex.Message}", ex);
            }
            return result;
        }
    }
    }
