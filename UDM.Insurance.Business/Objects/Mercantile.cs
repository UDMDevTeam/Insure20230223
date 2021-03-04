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
    public partial class Mercantile : ObjectBase<long>
    {
        #region Members
        private long? _fksystemid = null;
        private long? _fkimportid = null;
        private long? _fkbankid = null;
        private long? _fkbankbranchid = null;
        private string _accountnumber = null;
        private Byte? _accnumcheckstatus = null;
        private string _accnumcheckmsg = null;
        private string _accnumcheckmsgfull = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the Mercantile class.
        /// </summary>
        public Mercantile()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the Mercantile class.
        /// </summary>
        public Mercantile(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKSystemID
        {
            get
            {
                Fill();
                return _fksystemid;
            }
            set 
            {
                Fill();
                if (value != _fksystemid)
                {
                    _fksystemid = value;
                    _hasChanged = true;
                }
            }
        }

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

        public long? FKBankBranchID
        {
            get
            {
                Fill();
                return _fkbankbranchid;
            }
            set 
            {
                Fill();
                if (value != _fkbankbranchid)
                {
                    _fkbankbranchid = value;
                    _hasChanged = true;
                }
            }
        }

        public string AccountNumber
        {
            get
            {
                Fill();
                return _accountnumber;
            }
            set 
            {
                Fill();
                if (value != _accountnumber)
                {
                    _accountnumber = value;
                    _hasChanged = true;
                }
            }
        }

        public Byte? AccNumCheckStatus
        {
            get
            {
                Fill();
                return _accnumcheckstatus;
            }
            set 
            {
                Fill();
                if (value != _accnumcheckstatus)
                {
                    _accnumcheckstatus = value;
                    _hasChanged = true;
                }
            }
        }

        public string AccNumCheckMsg
        {
            get
            {
                Fill();
                return _accnumcheckmsg;
            }
            set 
            {
                Fill();
                if (value != _accnumcheckmsg)
                {
                    _accnumcheckmsg = value;
                    _hasChanged = true;
                }
            }
        }

        public string AccNumCheckMsgFull
        {
            get
            {
                Fill();
                return _accnumcheckmsgfull;
            }
            set 
            {
                Fill();
                if (value != _accnumcheckmsgfull)
                {
                    _accnumcheckmsgfull = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an Mercantile object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                MercantileMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an Mercantile object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the Mercantile object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = MercantileMapper.Save(this);
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
        /// Deletes an Mercantile object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the Mercantile object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && MercantileMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the Mercantile.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<mercantile>");
            xml.Append("<fksystemid>" + FKSystemID.ToString() + "</fksystemid>");
            xml.Append("<fkimportid>" + FKImportID.ToString() + "</fkimportid>");
            xml.Append("<fkbankid>" + FKBankID.ToString() + "</fkbankid>");
            xml.Append("<fkbankbranchid>" + FKBankBranchID.ToString() + "</fkbankbranchid>");
            xml.Append("<accountnumber>" + AccountNumber.ToString() + "</accountnumber>");
            xml.Append("<accnumcheckstatus>" + AccNumCheckStatus.ToString() + "</accnumcheckstatus>");
            xml.Append("<accnumcheckmsg>" + AccNumCheckMsg.ToString() + "</accnumcheckmsg>");
            xml.Append("<accnumcheckmsgfull>" + AccNumCheckMsgFull.ToString() + "</accnumcheckmsgfull>");
            xml.Append("</mercantile>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the Mercantile object from a list of parameters.
        /// </summary>
        /// <param name="fksystemid"></param>
        /// <param name="fkimportid"></param>
        /// <param name="fkbankid"></param>
        /// <param name="fkbankbranchid"></param>
        /// <param name="accountnumber"></param>
        /// <param name="accnumcheckstatus"></param>
        /// <param name="accnumcheckmsg"></param>
        /// <param name="accnumcheckmsgfull"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fksystemid, long? fkimportid, long? fkbankid, long? fkbankbranchid, string accountnumber, Byte? accnumcheckstatus, string accnumcheckmsg, string accnumcheckmsgfull)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKSystemID = fksystemid;
                this.FKImportID = fkimportid;
                this.FKBankID = fkbankid;
                this.FKBankBranchID = fkbankbranchid;
                this.AccountNumber = accountnumber;
                this.AccNumCheckStatus = accnumcheckstatus;
                this.AccNumCheckMsg = accnumcheckmsg;
                this.AccNumCheckMsgFull = accnumcheckmsgfull;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) Mercantile's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the Mercantile history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return MercantileMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an Mercantile object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the Mercantile object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return MercantileMapper.UnDelete(this);
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
    /// A collection of the Mercantile object.
    /// </summary>
    public partial class MercantileCollection : ObjectCollection<Mercantile>
    { 
    }
    #endregion
}
