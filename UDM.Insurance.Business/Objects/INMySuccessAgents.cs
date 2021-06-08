using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

using UDM.Insurance.Business.Mapping;
using Embriant.Framework.Configuration;
using Embriant.Framework;
using Embriant.Framework.Validation;

namespace UDM.Insurance.Business
{
    public partial class INMySuccessAgents : ObjectBase<long>
    {
        #region Members
        private int? _fkcampaignid = null;
        private int? _userid = null;
        private string _call1 = null;
        private string _call2 = null;
        private string _call3 = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the CallData class.
        /// </summary>
        public INMySuccessAgents()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the CallData class.
        /// </summary>
        public INMySuccessAgents(int id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public int? FKCampaignID
        {
            get
            {
                Fill();
                return _fkcampaignid;
            }
            set 
            {
                Fill();
                if (value != _fkcampaignid)
                {
                    _fkcampaignid = value;
                    _hasChanged = true;
                }
            }
        }

        public int? UserID
        {
            get
            {
                Fill();
                return _userid;
            }
            set 
            {
                Fill();
                if (value != _userid)
                {
                    _userid = value;
                    _hasChanged = true;
                }
            }
        }

        public string Call1
        {
            get
            {
                Fill();
                return _call1;
            }
            set 
            {
                Fill();
                if (value != _call1)
                {
                    _call1 = value;
                    _hasChanged = true;
                }
            }
        }

        public string Call2
        {
            get
            {
                Fill();
                return _call2;
            }
            set 
            {
                Fill();
                if (value != _call2)
                {
                    _call2 = value;
                    _hasChanged = true;
                }
            }
        }

        public string Call3
        {
            get
            {
                Fill();
                return _call3;
            }
            set
            {
                Fill();
                if (value != _call3)
                {
                    _call3 = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an CallData object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INMySuccessAgentsMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an CallData object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CallData object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INMySuccessAgentsMapper.Save(this);
                    result &= _isLoaded;
                }
                _hasChanged = !result;
                return result && AfterSave(validationResult);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes an CallData object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CallData object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INMySuccessAgentsMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the CallData.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inmysuccessagents>");
            xml.Append("<fkcampaignid>" + FKCampaignID.ToString() + "</fkcampaignid>");
            xml.Append("<userid>" + UserID.ToString() + "</userid>");
            xml.Append("<call1>" + Call1.ToString() + "</call1>");
            xml.Append("<call2>" + Call2.ToString() + "</call2>");
            xml.Append("<call3>" + Call3.ToString() + "</call3>");
            xml.Append("</inmysuccessagents>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the CallData object from a list of parameters.
        /// </summary>
        /// <param name="fkimportid"></param>
        /// <param name="number"></param>
        /// <param name="extension"></param>
        /// <param name="recref"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(int? fkcampaignid, int? userid, string call1, string call2, string call3)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKCampaignID = fkcampaignid;
                this.UserID = userid;
                this.Call1 = call1;
                this.Call2 = call2;
                this.Call3 = call3;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) CallData's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CallData history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INMySuccessAgentsMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an CallData object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CallData object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INMySuccessAgentsMapper.UnDelete(this);
            }
            else
            {
                return false;
            }
        }
        #endregion
    }

    #region Collection
    /// <summary>
    /// A collection of the CallData object.
    /// </summary>
    public partial class INMySuccessAgentsCollection : ObjectCollection<INMySuccessAgents>
    { 
    }
    #endregion
}
