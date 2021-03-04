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
    public partial class INBankDetails : ObjectBase<long>
    {
        #region Members
        private long? _fkpaymentmethodid = null;
        private string _accountholder = null;
        private long? _fkbankid = null;
        private long? _fkbankbranchid = null;
        private string _accountno = null;
        private long? _fkaccounttypeid = null;
        private short? _debitday = null;
        private long? _fkahtitleid = null;
        private string _ahinitials = null;
        private string _ahfirstname = null;
        private string _ahsurname = null;
        private string _ahidno = null;
        private string _ahtelhome = null;
        private string _ahtelcell = null;
        private string _ahtelwork = null;
        private long? _todeleteid = null;
        private long? _fksigningpowerid = null;
        private Byte? _accnumcheckstatus = null;
        private string _accnumcheckmsg = null;
        private string _accnumcheckmsgfull = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INBankDetails class.
        /// </summary>
        public INBankDetails()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INBankDetails class.
        /// </summary>
        public INBankDetails(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKPaymentMethodID
        {
            get
            {
                Fill();
                return _fkpaymentmethodid;
            }
            set 
            {
                Fill();
                if (value != _fkpaymentmethodid)
                {
                    _fkpaymentmethodid = value;
                    _hasChanged = true;
                }
            }
        }

        public string AccountHolder
        {
            get
            {
                Fill();
                return _accountholder;
            }
            set 
            {
                Fill();
                if (value != _accountholder)
                {
                    _accountholder = value;
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

        public string AccountNo
        {
            get
            {
                Fill();
                return _accountno;
            }
            set 
            {
                Fill();
                if (value != _accountno)
                {
                    _accountno = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKAccountTypeID
        {
            get
            {
                Fill();
                return _fkaccounttypeid;
            }
            set 
            {
                Fill();
                if (value != _fkaccounttypeid)
                {
                    _fkaccounttypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public short? DebitDay
        {
            get
            {
                Fill();
                return _debitday;
            }
            set 
            {
                Fill();
                if (value != _debitday)
                {
                    _debitday = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKAHTitleID
        {
            get
            {
                Fill();
                return _fkahtitleid;
            }
            set 
            {
                Fill();
                if (value != _fkahtitleid)
                {
                    _fkahtitleid = value;
                    _hasChanged = true;
                }
            }
        }

        public string AHInitials
        {
            get
            {
                Fill();
                return _ahinitials;
            }
            set 
            {
                Fill();
                if (value != _ahinitials)
                {
                    _ahinitials = value;
                    _hasChanged = true;
                }
            }
        }

        public string AHFirstName
        {
            get
            {
                Fill();
                return _ahfirstname;
            }
            set 
            {
                Fill();
                if (value != _ahfirstname)
                {
                    _ahfirstname = value;
                    _hasChanged = true;
                }
            }
        }

        public string AHSurname
        {
            get
            {
                Fill();
                return _ahsurname;
            }
            set 
            {
                Fill();
                if (value != _ahsurname)
                {
                    _ahsurname = value;
                    _hasChanged = true;
                }
            }
        }

        public string AHIDNo
        {
            get
            {
                Fill();
                return _ahidno;
            }
            set 
            {
                Fill();
                if (value != _ahidno)
                {
                    _ahidno = value;
                    _hasChanged = true;
                }
            }
        }

        public string AHTelHome
        {
            get
            {
                Fill();
                return _ahtelhome;
            }
            set 
            {
                Fill();
                if (value != _ahtelhome)
                {
                    _ahtelhome = value;
                    _hasChanged = true;
                }
            }
        }

        public string AHTelCell
        {
            get
            {
                Fill();
                return _ahtelcell;
            }
            set 
            {
                Fill();
                if (value != _ahtelcell)
                {
                    _ahtelcell = value;
                    _hasChanged = true;
                }
            }
        }

        public string AHTelWork
        {
            get
            {
                Fill();
                return _ahtelwork;
            }
            set 
            {
                Fill();
                if (value != _ahtelwork)
                {
                    _ahtelwork = value;
                    _hasChanged = true;
                }
            }
        }

        public long? ToDeleteID
        {
            get
            {
                Fill();
                return _todeleteid;
            }
            set 
            {
                Fill();
                if (value != _todeleteid)
                {
                    _todeleteid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKSigningPowerID
        {
            get
            {
                Fill();
                return _fksigningpowerid;
            }
            set 
            {
                Fill();
                if (value != _fksigningpowerid)
                {
                    _fksigningpowerid = value;
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
        /// Fills an INBankDetails object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INBankDetailsMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INBankDetails object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INBankDetails object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INBankDetailsMapper.Save(this);
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
        /// Deletes an INBankDetails object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INBankDetails object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INBankDetailsMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INBankDetails.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inbankdetails>");
            xml.Append("<fkpaymentmethodid>" + FKPaymentMethodID.ToString() + "</fkpaymentmethodid>");
            xml.Append("<accountholder>" + AccountHolder.ToString() + "</accountholder>");
            xml.Append("<fkbankid>" + FKBankID.ToString() + "</fkbankid>");
            xml.Append("<fkbankbranchid>" + FKBankBranchID.ToString() + "</fkbankbranchid>");
            xml.Append("<accountno>" + AccountNo.ToString() + "</accountno>");
            xml.Append("<fkaccounttypeid>" + FKAccountTypeID.ToString() + "</fkaccounttypeid>");
            xml.Append("<debitday>" + DebitDay.ToString() + "</debitday>");
            xml.Append("<fkahtitleid>" + FKAHTitleID.ToString() + "</fkahtitleid>");
            xml.Append("<ahinitials>" + AHInitials.ToString() + "</ahinitials>");
            xml.Append("<ahfirstname>" + AHFirstName.ToString() + "</ahfirstname>");
            xml.Append("<ahsurname>" + AHSurname.ToString() + "</ahsurname>");
            xml.Append("<ahidno>" + AHIDNo.ToString() + "</ahidno>");
            xml.Append("<ahtelhome>" + AHTelHome.ToString() + "</ahtelhome>");
            xml.Append("<ahtelcell>" + AHTelCell.ToString() + "</ahtelcell>");
            xml.Append("<ahtelwork>" + AHTelWork.ToString() + "</ahtelwork>");
            xml.Append("<todeleteid>" + ToDeleteID.ToString() + "</todeleteid>");
            xml.Append("<fksigningpowerid>" + FKSigningPowerID.ToString() + "</fksigningpowerid>");
            xml.Append("<accnumcheckstatus>" + AccNumCheckStatus.ToString() + "</accnumcheckstatus>");
            xml.Append("<accnumcheckmsg>" + AccNumCheckMsg.ToString() + "</accnumcheckmsg>");
            xml.Append("<accnumcheckmsgfull>" + AccNumCheckMsgFull.ToString() + "</accnumcheckmsgfull>");
            xml.Append("</inbankdetails>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INBankDetails object from a list of parameters.
        /// </summary>
        /// <param name="fkpaymentmethodid"></param>
        /// <param name="accountholder"></param>
        /// <param name="fkbankid"></param>
        /// <param name="fkbankbranchid"></param>
        /// <param name="accountno"></param>
        /// <param name="fkaccounttypeid"></param>
        /// <param name="debitday"></param>
        /// <param name="fkahtitleid"></param>
        /// <param name="ahinitials"></param>
        /// <param name="ahfirstname"></param>
        /// <param name="ahsurname"></param>
        /// <param name="ahidno"></param>
        /// <param name="ahtelhome"></param>
        /// <param name="ahtelcell"></param>
        /// <param name="ahtelwork"></param>
        /// <param name="todeleteid"></param>
        /// <param name="fksigningpowerid"></param>
        /// <param name="accnumcheckstatus"></param>
        /// <param name="accnumcheckmsg"></param>
        /// <param name="accnumcheckmsgfull"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkpaymentmethodid, string accountholder, long? fkbankid, long? fkbankbranchid, string accountno, long? fkaccounttypeid, short? debitday, long? fkahtitleid, string ahinitials, string ahfirstname, string ahsurname, string ahidno, string ahtelhome, string ahtelcell, string ahtelwork, long? todeleteid, long? fksigningpowerid, Byte? accnumcheckstatus, string accnumcheckmsg, string accnumcheckmsgfull)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKPaymentMethodID = fkpaymentmethodid;
                this.AccountHolder = accountholder;
                this.FKBankID = fkbankid;
                this.FKBankBranchID = fkbankbranchid;
                this.AccountNo = accountno;
                this.FKAccountTypeID = fkaccounttypeid;
                this.DebitDay = debitday;
                this.FKAHTitleID = fkahtitleid;
                this.AHInitials = ahinitials;
                this.AHFirstName = ahfirstname;
                this.AHSurname = ahsurname;
                this.AHIDNo = ahidno;
                this.AHTelHome = ahtelhome;
                this.AHTelCell = ahtelcell;
                this.AHTelWork = ahtelwork;
                this.ToDeleteID = todeleteid;
                this.FKSigningPowerID = fksigningpowerid;
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
        /// Deletes a(n) INBankDetails's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INBankDetails history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INBankDetailsMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INBankDetails object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INBankDetails object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INBankDetailsMapper.UnDelete(this);
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
    /// A collection of the INBankDetails object.
    /// </summary>
    public partial class INBankDetailsCollection : ObjectCollection<INBankDetails>
    { 
    }
    #endregion
}
