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
    public partial class INImportHeader : ObjectBase<long>
    {
        #region Members
        private string _name = null;
        private string _header = null;
        private string _headeralt1 = null;
        private string _headeralt2 = null;
        private string _headeralt3 = null;
        private string _tablename = null;
        private bool? _ignoreifna = null;
        private bool? _isactive = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INImportHeader class.
        /// </summary>
        public INImportHeader()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INImportHeader class.
        /// </summary>
        public INImportHeader(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                Fill();
                return _name;
            }
            set 
            {
                Fill();
                if (value != _name)
                {
                    _name = value;
                    _hasChanged = true;
                }
            }
        }

        public string Header
        {
            get
            {
                Fill();
                return _header;
            }
            set 
            {
                Fill();
                if (value != _header)
                {
                    _header = value;
                    _hasChanged = true;
                }
            }
        }

        public string HeaderAlt1
        {
            get
            {
                Fill();
                return _headeralt1;
            }
            set 
            {
                Fill();
                if (value != _headeralt1)
                {
                    _headeralt1 = value;
                    _hasChanged = true;
                }
            }
        }

        public string HeaderAlt2
        {
            get
            {
                Fill();
                return _headeralt2;
            }
            set 
            {
                Fill();
                if (value != _headeralt2)
                {
                    _headeralt2 = value;
                    _hasChanged = true;
                }
            }
        }

        public string HeaderAlt3
        {
            get
            {
                Fill();
                return _headeralt3;
            }
            set 
            {
                Fill();
                if (value != _headeralt3)
                {
                    _headeralt3 = value;
                    _hasChanged = true;
                }
            }
        }

        public string TableName
        {
            get
            {
                Fill();
                return _tablename;
            }
            set 
            {
                Fill();
                if (value != _tablename)
                {
                    _tablename = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IgnoreIfNA
        {
            get
            {
                Fill();
                return _ignoreifna;
            }
            set 
            {
                Fill();
                if (value != _ignoreifna)
                {
                    _ignoreifna = value;
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
        /// Fills an INImportHeader object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INImportHeaderMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INImportHeader object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportHeader object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INImportHeaderMapper.Save(this);
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
        /// Deletes an INImportHeader object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportHeader object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INImportHeaderMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INImportHeader.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inimportheader>");
            xml.Append("<name>" + Name.ToString() + "</name>");
            xml.Append("<header>" + Header.ToString() + "</header>");
            xml.Append("<headeralt1>" + HeaderAlt1.ToString() + "</headeralt1>");
            xml.Append("<headeralt2>" + HeaderAlt2.ToString() + "</headeralt2>");
            xml.Append("<headeralt3>" + HeaderAlt3.ToString() + "</headeralt3>");
            xml.Append("<tablename>" + TableName.ToString() + "</tablename>");
            xml.Append("<ignoreifna>" + IgnoreIfNA.ToString() + "</ignoreifna>");
            xml.Append("<isactive>" + IsActive.ToString() + "</isactive>");
            xml.Append("</inimportheader>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INImportHeader object from a list of parameters.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="header"></param>
        /// <param name="headeralt1"></param>
        /// <param name="headeralt2"></param>
        /// <param name="headeralt3"></param>
        /// <param name="tablename"></param>
        /// <param name="ignoreifna"></param>
        /// <param name="isactive"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(string name, string header, string headeralt1, string headeralt2, string headeralt3, string tablename, bool? ignoreifna, bool? isactive)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.Name = name;
                this.Header = header;
                this.HeaderAlt1 = headeralt1;
                this.HeaderAlt2 = headeralt2;
                this.HeaderAlt3 = headeralt3;
                this.TableName = tablename;
                this.IgnoreIfNA = ignoreifna;
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
        /// Deletes a(n) INImportHeader's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportHeader history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportHeaderMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INImportHeader object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportHeader object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportHeaderMapper.UnDelete(this);
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
    /// A collection of the INImportHeader object.
    /// </summary>
    public partial class INImportHeaderCollection : ObjectCollection<INImportHeader>
    { 
    }
    #endregion
}
