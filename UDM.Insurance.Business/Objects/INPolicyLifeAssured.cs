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
    public partial class INPolicyLifeAssured : ObjectBase<long>
    {
        #region Members
        private long? _fkinpolicyid = null;
        private long? _fkinlifeassuredid = null;
        private int? _lifeassuredrank = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INPolicyLifeAssured class.
        /// </summary>
        public INPolicyLifeAssured()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INPolicyLifeAssured class.
        /// </summary>
        public INPolicyLifeAssured(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKINPolicyID
        {
            get
            {
                Fill();
                return _fkinpolicyid;
            }
            set 
            {
                Fill();
                if (value != _fkinpolicyid)
                {
                    _fkinpolicyid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINLifeAssuredID
        {
            get
            {
                Fill();
                return _fkinlifeassuredid;
            }
            set 
            {
                Fill();
                if (value != _fkinlifeassuredid)
                {
                    _fkinlifeassuredid = value;
                    _hasChanged = true;
                }
            }
        }

        public int? LifeAssuredRank
        {
            get
            {
                Fill();
                return _lifeassuredrank;
            }
            set 
            {
                Fill();
                if (value != _lifeassuredrank)
                {
                    _lifeassuredrank = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INPolicyLifeAssured object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INPolicyLifeAssuredMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INPolicyLifeAssured object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicyLifeAssured object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INPolicyLifeAssuredMapper.Save(this);
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
        /// Deletes an INPolicyLifeAssured object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicyLifeAssured object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INPolicyLifeAssuredMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INPolicyLifeAssured.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inpolicylifeassured>");
            xml.Append("<fkinpolicyid>" + FKINPolicyID.ToString() + "</fkinpolicyid>");
            xml.Append("<fkinlifeassuredid>" + FKINLifeAssuredID.ToString() + "</fkinlifeassuredid>");
            xml.Append("<lifeassuredrank>" + LifeAssuredRank.ToString() + "</lifeassuredrank>");
            xml.Append("</inpolicylifeassured>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INPolicyLifeAssured object from a list of parameters.
        /// </summary>
        /// <param name="fkinpolicyid"></param>
        /// <param name="fkinlifeassuredid"></param>
        /// <param name="lifeassuredrank"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinpolicyid, long? fkinlifeassuredid, int? lifeassuredrank)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINPolicyID = fkinpolicyid;
                this.FKINLifeAssuredID = fkinlifeassuredid;
                this.LifeAssuredRank = lifeassuredrank;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INPolicyLifeAssured's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicyLifeAssured history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INPolicyLifeAssuredMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INPolicyLifeAssured object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicyLifeAssured object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INPolicyLifeAssuredMapper.UnDelete(this);
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
    /// A collection of the INPolicyLifeAssured object.
    /// </summary>
    public partial class INPolicyLifeAssuredCollection : ObjectCollection<INPolicyLifeAssured>
    { 
    }
    #endregion
}
