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
    public partial class CallData : ObjectBase<long>
    {
        #region Members
        private long? _fkimportid = null;
        private string _number = null;
        private string _extension = null;
        private string _recref = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the CallData class.
        /// </summary>
        public CallData()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the CallData class.
        /// </summary>
        public CallData(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKImportID
        {
            get
            {
                Fill();
                return _fkimportid;
            }
            set 
            {
                Fill();
                if (value != _fkimportid)
                {
                    _fkimportid = value;
                    _hasChanged = true;
                }
            }
        }

        public string Number
        {
            get
            {
                Fill();
                return _number;
            }
            set 
            {
                Fill();
                if (value != _number)
                {
                    _number = value;
                    _hasChanged = true;
                }
            }
        }

        public string Extension
        {
            get
            {
                Fill();
                return _extension;
            }
            set 
            {
                Fill();
                if (value != _extension)
                {
                    _extension = value;
                    _hasChanged = true;
                }
            }
        }

        public string RecRef
        {
            get
            {
                Fill();
                return _recref;
            }
            set 
            {
                Fill();
                if (value != _recref)
                {
                    _recref = value;
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
                CallDataMapper.Fill(this);
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
                    _isLoaded = CallDataMapper.Save(this);
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
                return BeforeDelete(validationResult) && CallDataMapper.Delete(this);
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
            xml.Append("<calldata>");
            xml.Append("<fkimportid>" + FKImportID.ToString() + "</fkimportid>");
            xml.Append("<number>" + Number.ToString() + "</number>");
            xml.Append("<extension>" + Extension.ToString() + "</extension>");
            xml.Append("<recref>" + RecRef.ToString() + "</recref>");
            xml.Append("</calldata>");
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
        public ValidationResult Fill(long? fkimportid, string number, string extension, string recref)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKImportID = fkimportid;
                this.Number = number;
                this.Extension = extension;
                this.RecRef = recref;
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
                return CallDataMapper.DeleteHistory(this);
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
                return CallDataMapper.UnDelete(this);
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
    public partial class CallDataCollection : ObjectCollection<CallData>
    { 
    }
    #endregion
}
