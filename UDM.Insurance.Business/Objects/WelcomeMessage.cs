using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

using UDM.Insurance.Business.Mapping;
using Embriant.Framework.Configuration;
using Embriant.Framework;
using Embriant.Framework.Validation;
using System.Security;

namespace UDM.Insurance.Business
{
    public partial class WelcomMessage : ObjectBase<long>
    {
        #region Members
        private string _textMessage = null;
        private DateTime? _stampDate = null;
        private long? _stampUserID = null;
        private bool? _isActive = null;
        private string _pdfName = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INNextOfKin class.
        /// </summary>
        public WelcomMessage()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INNextOfKin class.
        /// </summary>
        public WelcomMessage(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public string TextMessage
        {
            get { return _textMessage; }
            set
            {
                if (value != _textMessage)
                {
                    _textMessage = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? StampDate
        {
            get { return _stampDate; }
            set
            {
                if (value != _stampDate)
                {
                    _stampDate = value;
                    _hasChanged = true;
                }
            }
        }

        public long? StampUserID
        {
            get { return _stampUserID; }
            set
            {
                if (value != _stampUserID)
                {
                    _stampUserID = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsActive
        {
            get { return _isActive; }
            set
            {
                if (value != _isActive)
                {
                    _isActive = value;
                    _hasChanged = true;
                }
            }
        }

        public string PDFName
        {
            get { return _pdfName; }
            set
            {
                if (value != _pdfName)
                {
                    _pdfName = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion
        #region Override Methods
        /// <summary>
        /// Fills an INNextOfKin object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                WelcomeMessageMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INNextOfKin object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INNextOfKin object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = WelcomeMessageMapper.Save(this);
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
        /// Deletes an INNextOfKin object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INNextOfKin object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && WelcomeMessageMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INNextOfKin.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<welcomemessage>");
            xml.Append("<id>" + ID.ToString() + "</id>");
            xml.Append("<textmessage>" + SecurityElement.Escape(TextMessage) + "</textmessage>");
            xml.Append("<stampdate>" + StampDate.ToString() + "</stampdate>"); 
            xml.Append("<stampuserid>" + StampUserID.ToString() + "</stampuserid>");
            xml.Append("<isactive>" + IsActive.ToString().ToLower() + "</isactive>");
            xml.Append("<pdfname>" + SecurityElement.Escape(PDFName) + "</pdfname>");
            xml.Append("</welcomemessage>");
            return xml.ToString();
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INNextOfKin object from a list of parameters.
        /// </summary>
        /// <param name="fkinimportid"></param>
        /// <param name="fkinrelationshipid"></param>
        /// <param name="firstname"></param>
        /// <param name="surname"></param>
        /// <param name="telcontact"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(string textMessage, DateTime? stampDate, int? stampUserID, bool? isActive, string pdfName)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.TextMessage = textMessage;
                this.StampDate = stampDate;
                this.StampUserID = stampUserID;
                this.IsActive = isActive;
                this.PDFName = pdfName;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INNextOfKin's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INNextOfKin history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return WelcomeMessageMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INNextOfKin object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INNextOfKin object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return WelcomeMessageMapper.UnDelete(this);
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
    /// A collection of the INNextOfKin object.
    /// </summary>
    public partial class WelcomMessageCollection : ObjectCollection<WelcomMessage>
    {
    }
    #endregion
}
