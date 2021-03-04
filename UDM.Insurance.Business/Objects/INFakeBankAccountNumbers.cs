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
    public partial class INFakeBankAccountNumbers : ObjectBase<long>
    {
        #region Members
        private long? _fkbankid = null;
        private string _accno = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INFakeBankAccountNumbers class.
        /// </summary>
        public INFakeBankAccountNumbers()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INFakeBankAccountNumbers class.
        /// </summary>
        public INFakeBankAccountNumbers(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKBankID
        {
            get
            {
                Fill();
                return _fkbankid;
            }
            set 
            {
                Fill();
                if (value != _fkbankid)
                {
                    _fkbankid = value;
                    _hasChanged = true;
                }
            }
        }

        public string AccNo
        {
            get
            {
                Fill();
                return _accno;
            }
            set 
            {
                Fill();
                if (value != _accno)
                {
                    _accno = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INFakeBankAccountNumbers object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INFakeBankAccountNumbersMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INFakeBankAccountNumbers object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INFakeBankAccountNumbers object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INFakeBankAccountNumbersMapper.Save(this);
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
        /// Deletes an INFakeBankAccountNumbers object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INFakeBankAccountNumbers object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INFakeBankAccountNumbersMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INFakeBankAccountNumbers.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<infakebankaccountnumbers>");
            xml.Append("<fkbankid>" + FKBankID.ToString() + "</fkbankid>");
            xml.Append("<accno>" + AccNo.ToString() + "</accno>");
            xml.Append("</infakebankaccountnumbers>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INFakeBankAccountNumbers object from a list of parameters.
        /// </summary>
        /// <param name="fkbankid"></param>
        /// <param name="accno"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkbankid, string accno)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKBankID = fkbankid;
                this.AccNo = accno;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INFakeBankAccountNumbers's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INFakeBankAccountNumbers history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INFakeBankAccountNumbersMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INFakeBankAccountNumbers object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INFakeBankAccountNumbers object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INFakeBankAccountNumbersMapper.UnDelete(this);
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
    /// A collection of the INFakeBankAccountNumbers object.
    /// </summary>
    public partial class INFakeBankAccountNumbersCollection : ObjectCollection<INFakeBankAccountNumbers>
    { 
    }
    #endregion
}
