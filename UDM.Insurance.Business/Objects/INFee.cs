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
    public partial class INFee : ObjectBase<long>
    {
        #region Members
        private decimal? _la1fee = null;
        private decimal? _la2fee = null;
        private decimal? _childfee = null;
        private decimal? _unitfee = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INFee class.
        /// </summary>
        public INFee()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INFee class.
        /// </summary>
        public INFee(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public decimal? LA1Fee
        {
            get
            {
                Fill();
                return _la1fee;
            }
            set 
            {
                Fill();
                if (value != _la1fee)
                {
                    _la1fee = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? LA2Fee
        {
            get
            {
                Fill();
                return _la2fee;
            }
            set 
            {
                Fill();
                if (value != _la2fee)
                {
                    _la2fee = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? ChildFee
        {
            get
            {
                Fill();
                return _childfee;
            }
            set 
            {
                Fill();
                if (value != _childfee)
                {
                    _childfee = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? UnitFee
        {
            get
            {
                Fill();
                return _unitfee;
            }
            set 
            {
                Fill();
                if (value != _unitfee)
                {
                    _unitfee = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INFee object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INFeeMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INFee object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INFee object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INFeeMapper.Save(this);
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
        /// Deletes an INFee object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INFee object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INFeeMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INFee.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<infee>");
            xml.Append("<la1fee>" + LA1Fee.ToString() + "</la1fee>");
            xml.Append("<la2fee>" + LA2Fee.ToString() + "</la2fee>");
            xml.Append("<childfee>" + ChildFee.ToString() + "</childfee>");
            xml.Append("<unitfee>" + UnitFee.ToString() + "</unitfee>");
            xml.Append("</infee>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INFee object from a list of parameters.
        /// </summary>
        /// <param name="la1fee"></param>
        /// <param name="la2fee"></param>
        /// <param name="childfee"></param>
        /// <param name="unitfee"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(decimal? la1fee, decimal? la2fee, decimal? childfee, decimal? unitfee)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.LA1Fee = la1fee;
                this.LA2Fee = la2fee;
                this.ChildFee = childfee;
                this.UnitFee = unitfee;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INFee's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INFee history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INFeeMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INFee object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INFee object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INFeeMapper.UnDelete(this);
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
    /// A collection of the INFee object.
    /// </summary>
    public partial class INFeeCollection : ObjectCollection<INFee>
    { 
    }
    #endregion
}
