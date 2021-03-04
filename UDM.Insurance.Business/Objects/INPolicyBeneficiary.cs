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
    public partial class INPolicyBeneficiary : ObjectBase<long>
    {
        #region Members
        private long? _fkinpolicyid = null;
        private long? _fkinbeneficiaryid = null;
        private int? _beneficiaryrank = null;
        private Double? _beneficiarypercentage = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INPolicyBeneficiary class.
        /// </summary>
        public INPolicyBeneficiary()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INPolicyBeneficiary class.
        /// </summary>
        public INPolicyBeneficiary(long id)
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

        public long? FKINBeneficiaryID
        {
            get
            {
                Fill();
                return _fkinbeneficiaryid;
            }
            set 
            {
                Fill();
                if (value != _fkinbeneficiaryid)
                {
                    _fkinbeneficiaryid = value;
                    _hasChanged = true;
                }
            }
        }

        public int? BeneficiaryRank
        {
            get
            {
                Fill();
                return _beneficiaryrank;
            }
            set 
            {
                Fill();
                if (value != _beneficiaryrank)
                {
                    _beneficiaryrank = value;
                    _hasChanged = true;
                }
            }
        }

        public Double? BeneficiaryPercentage
        {
            get
            {
                Fill();
                return _beneficiarypercentage;
            }
            set 
            {
                Fill();
                if (value != _beneficiarypercentage)
                {
                    _beneficiarypercentage = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INPolicyBeneficiary object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INPolicyBeneficiaryMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INPolicyBeneficiary object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicyBeneficiary object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INPolicyBeneficiaryMapper.Save(this);
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
        /// Deletes an INPolicyBeneficiary object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicyBeneficiary object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INPolicyBeneficiaryMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INPolicyBeneficiary.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inpolicybeneficiary>");
            xml.Append("<fkinpolicyid>" + FKINPolicyID.ToString() + "</fkinpolicyid>");
            xml.Append("<fkinbeneficiaryid>" + FKINBeneficiaryID.ToString() + "</fkinbeneficiaryid>");
            xml.Append("<beneficiaryrank>" + BeneficiaryRank.ToString() + "</beneficiaryrank>");
            xml.Append("<beneficiarypercentage>" + BeneficiaryPercentage.ToString() + "</beneficiarypercentage>");
            xml.Append("</inpolicybeneficiary>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INPolicyBeneficiary object from a list of parameters.
        /// </summary>
        /// <param name="fkinpolicyid"></param>
        /// <param name="fkinbeneficiaryid"></param>
        /// <param name="beneficiaryrank"></param>
        /// <param name="beneficiarypercentage"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinpolicyid, long? fkinbeneficiaryid, int? beneficiaryrank, Double? beneficiarypercentage)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINPolicyID = fkinpolicyid;
                this.FKINBeneficiaryID = fkinbeneficiaryid;
                this.BeneficiaryRank = beneficiaryrank;
                this.BeneficiaryPercentage = beneficiarypercentage;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INPolicyBeneficiary's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicyBeneficiary history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INPolicyBeneficiaryMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INPolicyBeneficiary object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicyBeneficiary object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INPolicyBeneficiaryMapper.UnDelete(this);
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
    /// A collection of the INPolicyBeneficiary object.
    /// </summary>
    public partial class INPolicyBeneficiaryCollection : ObjectCollection<INPolicyBeneficiary>
    { 
    }
    #endregion
}
