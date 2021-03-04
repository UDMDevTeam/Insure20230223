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
    public partial class INMoneyBack : ObjectBase<long>
    {
        #region Members
        private long? _fkinpolicytypeid = null;
        private long? _fkinoptionid = null;
        private short? _agemin = null;
        private short? _agemax = null;
        private decimal? _value = null;
        private bool? _isactive = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INMoneyBack class.
        /// </summary>
        public INMoneyBack()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INMoneyBack class.
        /// </summary>
        public INMoneyBack(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKINPolicyTypeID
        {
            get
            {
                Fill();
                return _fkinpolicytypeid;
            }
            set 
            {
                Fill();
                if (value != _fkinpolicytypeid)
                {
                    _fkinpolicytypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINOptionID
        {
            get
            {
                Fill();
                return _fkinoptionid;
            }
            set 
            {
                Fill();
                if (value != _fkinoptionid)
                {
                    _fkinoptionid = value;
                    _hasChanged = true;
                }
            }
        }

        public short? AgeMin
        {
            get
            {
                Fill();
                return _agemin;
            }
            set 
            {
                Fill();
                if (value != _agemin)
                {
                    _agemin = value;
                    _hasChanged = true;
                }
            }
        }

        public short? AgeMax
        {
            get
            {
                Fill();
                return _agemax;
            }
            set 
            {
                Fill();
                if (value != _agemax)
                {
                    _agemax = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? Value
        {
            get
            {
                Fill();
                return _value;
            }
            set 
            {
                Fill();
                if (value != _value)
                {
                    _value = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsActive
        {
            get
            {
                Fill();
                return _isactive;
            }
            set 
            {
                Fill();
                if (value != _isactive)
                {
                    _isactive = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INMoneyBack object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INMoneyBackMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INMoneyBack object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INMoneyBack object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INMoneyBackMapper.Save(this);
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
        /// Deletes an INMoneyBack object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INMoneyBack object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INMoneyBackMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INMoneyBack.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inmoneyback>");
            xml.Append("<fkinpolicytypeid>" + FKINPolicyTypeID.ToString() + "</fkinpolicytypeid>");
            xml.Append("<fkinoptionid>" + FKINOptionID.ToString() + "</fkinoptionid>");
            xml.Append("<agemin>" + AgeMin.ToString() + "</agemin>");
            xml.Append("<agemax>" + AgeMax.ToString() + "</agemax>");
            xml.Append("<value>" + Value.ToString() + "</value>");
            xml.Append("<isactive>" + IsActive.ToString() + "</isactive>");
            xml.Append("</inmoneyback>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INMoneyBack object from a list of parameters.
        /// </summary>
        /// <param name="fkinpolicytypeid"></param>
        /// <param name="fkinoptionid"></param>
        /// <param name="agemin"></param>
        /// <param name="agemax"></param>
        /// <param name="value"></param>
        /// <param name="isactive"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinpolicytypeid, long? fkinoptionid, short? agemin, short? agemax, decimal? value, bool? isactive)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINPolicyTypeID = fkinpolicytypeid;
                this.FKINOptionID = fkinoptionid;
                this.AgeMin = agemin;
                this.AgeMax = agemax;
                this.Value = value;
                this.IsActive = isactive;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INMoneyBack's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INMoneyBack history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INMoneyBackMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INMoneyBack object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INMoneyBack object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INMoneyBackMapper.UnDelete(this);
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
    /// A collection of the INMoneyBack object.
    /// </summary>
    public partial class INMoneyBackCollection : ObjectCollection<INMoneyBack>
    { 
    }
    #endregion
}
