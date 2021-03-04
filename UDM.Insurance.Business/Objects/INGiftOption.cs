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
    public partial class INGiftOption : ObjectBase<long>
    {
        #region Members
        private string _gift = null;
        private DateTime? _activestartdate = null;
        private DateTime? _activeenddate = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INGiftOption class.
        /// </summary>
        public INGiftOption()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INGiftOption class.
        /// </summary>
        public INGiftOption(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public string Gift
        {
            get
            {
                Fill();
                return _gift;
            }
            set 
            {
                Fill();
                if (value != _gift)
                {
                    _gift = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? ActiveStartDate
        {
            get
            {
                Fill();
                return _activestartdate;
            }
            set 
            {
                Fill();
                if (value != _activestartdate)
                {
                    _activestartdate = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? ActiveEndDate
        {
            get
            {
                Fill();
                return _activeenddate;
            }
            set 
            {
                Fill();
                if (value != _activeenddate)
                {
                    _activeenddate = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INGiftOption object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INGiftOptionMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INGiftOption object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INGiftOption object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INGiftOptionMapper.Save(this);
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
        /// Deletes an INGiftOption object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INGiftOption object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INGiftOptionMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INGiftOption.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<ingiftoption>");
            xml.Append("<gift>" + Gift.ToString() + "</gift>");
            xml.Append("<activestartdate>" + ActiveStartDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</activestartdate>");
            xml.Append("<activeenddate>" + ActiveEndDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</activeenddate>");
            xml.Append("</ingiftoption>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INGiftOption object from a list of parameters.
        /// </summary>
        /// <param name="gift"></param>
        /// <param name="activestartdate"></param>
        /// <param name="activeenddate"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(string gift, DateTime? activestartdate, DateTime? activeenddate)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.Gift = gift;
                this.ActiveStartDate = activestartdate;
                this.ActiveEndDate = activeenddate;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INGiftOption's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INGiftOption history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INGiftOptionMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INGiftOption object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INGiftOption object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INGiftOptionMapper.UnDelete(this);
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
    /// A collection of the INGiftOption object.
    /// </summary>
    public partial class INGiftOptionCollection : ObjectCollection<INGiftOption>
    { 
    }
    #endregion
}
