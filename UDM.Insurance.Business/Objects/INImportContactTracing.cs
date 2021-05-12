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
    public partial class INImportContactTracing : ObjectBase<long>
    {
        #region Members
        private long? _fkinimportid = null;
        private string _contactTraceOne = null;
        private string _contactTraceTwo = null;
        private string _contactTraceThree = null;
        private string _contactTraceFour = null;
        private string _contactTraceFive = null;
        private string _contactTraceSix = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INImportContactTracing class.
        /// </summary>
        public INImportContactTracing()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INImportContactTracing class.
        /// </summary>
        public INImportContactTracing(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKINImportID
        {
            get
            {
                Fill();
                return _fkinimportid;
            }
            set
            {
                Fill();
                if (value != _fkinimportid)
                {
                    _fkinimportid = value;
                    _hasChanged = true;
                }
            }
        }

        public string ContactTraceOne
        {
            get
            {
                Fill();
                return _contactTraceOne;
            }
            set
            {
                Fill();
                if (value != _contactTraceOne)
                {
                    _contactTraceOne = value;
                    _hasChanged = true;
                }
            }
        }


        public string ContactTraceTwo
        {
            get
            {
                Fill();
                return _contactTraceTwo;
            }
            set
            {
                Fill();
                if (value != _contactTraceTwo)
                {
                    _contactTraceTwo = value;
                    _hasChanged = true;
                }
            }
        }

        public string ContactTraceThree
        {
            get
            {
                Fill();
                return _contactTraceOne;
            }
            set
            {
                Fill();
                if (value != _contactTraceOne)
                {
                    _contactTraceOne = value;
                    _hasChanged = true;
                }
            }
        }

        public string ContactTraceFour
        {
            get
            {
                Fill();
                return _contactTraceFour;
            }
            set
            {
                Fill();
                if (value != _contactTraceFour)
                {
                    _contactTraceFour = value;
                    _hasChanged = true;
                }
            }
        }

        public string ContactTraceFive
        {
            get
            {
                Fill();
                return _contactTraceFive;
            }
            set
            {
                Fill();
                if (value != _contactTraceFive)
                {
                    _contactTraceFive = value;
                    _hasChanged = true;
                }
            }
        }

        public string ContactTraceSix
        {
            get
            {
                Fill();
                return _contactTraceSix;
            }
            set
            {
                Fill();
                if (value != _contactTraceSix)
                {
                    _contactTraceSix = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INImportContactTracing object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INImportContactTracingMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INImportContactTracing object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportContactTracing object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INImportContactTracingMapper.Save(this);
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
        /// Deletes an INImportContactTracing object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportContactTracing object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INImportContactTracingMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INImportContactTracing.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<INImportContactTracing>");
            xml.Append("<fkinimportid>" + FKINImportID.ToString() + "</fkinimportid>");
            xml.Append("<contact1>" + ContactTraceOne.ToString() + "</contact1>");
            xml.Append("<contact2>" + ContactTraceTwo.ToString() + "</contact2>");
            xml.Append("<contact3>" + ContactTraceThree.ToString() + "</contact3>");
            xml.Append("<contact4>" + ContactTraceFour.ToString() + "</contact4>");
            xml.Append("<contact5>" + ContactTraceFive.ToString() + "</contact5>");
            xml.Append("<contact6>" + ContactTraceSix.ToString() + "</contact6>");
            xml.Append("</INImportContactTracing>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INImportContactTracing object from a list of parameters.
        /// </summary>
        /// <param name="fkinimportid"></param>
        /// <param name="fkinrelationshipid"></param>
        /// <param name="firstname"></param>
        /// <param name="surname"></param>
        /// <param name="telcontact"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinimportid, string contactTraceOne, string contactTraceTwo, string contactTraceThree, string contactTraceFour, string contactTraceFive, string contactTraceSix)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINImportID = fkinimportid;
                this.ContactTraceOne = contactTraceOne;
                this.ContactTraceTwo = contactTraceTwo;
                this.ContactTraceThree = contactTraceThree;
                this.ContactTraceFour = contactTraceFour;
                this.ContactTraceFive = contactTraceFive;
                this.ContactTraceSix = contactTraceSix;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INImportContactTracing's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportContactTracing history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportContactTracingMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INImportContactTracing object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportContactTracing object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportContactTracingMapper.UnDelete(this);
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
    /// A collection of the INImportContactTracing object.
    /// </summary>
    public partial class INImportContactTracingCollection : ObjectCollection<INImportContactTracing>
    {
    }
    #endregion
}
