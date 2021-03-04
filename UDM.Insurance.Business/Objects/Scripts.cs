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
    public partial class Scripts : ObjectBase<long>
    {
        #region Members
        private long? _fkscripttypeid = null;
        private long? _fkcampaignid = null;
        private long? _fkcampaigntypeid = null;
        private long? _fkcampaigngroupid = null;
        private long? _fkcampaigntypegroupid = null;
        private long? _fkcampaigngrouptypeid = null;
        private long? _fklanguageid = null;
        private string _description = null;
        private byte[] _document = null;
        private DateTime? _date = null;
        private bool? _isactive = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the Scripts class.
        /// </summary>
        public Scripts()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the Scripts class.
        /// </summary>
        public Scripts(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKScriptTypeID
        {
            get
            {
                Fill();
                return _fkscripttypeid;
            }
            set 
            {
                Fill();
                if (value != _fkscripttypeid)
                {
                    _fkscripttypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKCampaignID
        {
            get
            {
                Fill();
                return _fkcampaignid;
            }
            set 
            {
                Fill();
                if (value != _fkcampaignid)
                {
                    _fkcampaignid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKCampaignTypeID
        {
            get
            {
                Fill();
                return _fkcampaigntypeid;
            }
            set 
            {
                Fill();
                if (value != _fkcampaigntypeid)
                {
                    _fkcampaigntypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKCampaignGroupID
        {
            get
            {
                Fill();
                return _fkcampaigngroupid;
            }
            set 
            {
                Fill();
                if (value != _fkcampaigngroupid)
                {
                    _fkcampaigngroupid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKCampaignTypeGroupID
        {
            get
            {
                Fill();
                return _fkcampaigntypegroupid;
            }
            set 
            {
                Fill();
                if (value != _fkcampaigntypegroupid)
                {
                    _fkcampaigntypegroupid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKCampaignGroupTypeID
        {
            get
            {
                Fill();
                return _fkcampaigngrouptypeid;
            }
            set 
            {
                Fill();
                if (value != _fkcampaigngrouptypeid)
                {
                    _fkcampaigngrouptypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKLanguageID
        {
            get
            {
                Fill();
                return _fklanguageid;
            }
            set 
            {
                Fill();
                if (value != _fklanguageid)
                {
                    _fklanguageid = value;
                    _hasChanged = true;
                }
            }
        }

        public string Description
        {
            get
            {
                Fill();
                return _description;
            }
            set 
            {
                Fill();
                if (value != _description)
                {
                    _description = value;
                    _hasChanged = true;
                }
            }
        }

        public byte[] Document
        {
            get
            {
                Fill();
                return _document;
            }
            set 
            {
                Fill();
                if (value != _document)
                {
                    _document = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? Date
        {
            get
            {
                Fill();
                return _date;
            }
            set 
            {
                Fill();
                if (value != _date)
                {
                    _date = value;
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
        /// Fills an Scripts object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                ScriptsMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an Scripts object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the Scripts object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = ScriptsMapper.Save(this);
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
        /// Deletes an Scripts object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the Scripts object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && ScriptsMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the Scripts.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<scripts>");
            xml.Append("<fkscripttypeid>" + FKScriptTypeID.ToString() + "</fkscripttypeid>");
            xml.Append("<fkcampaignid>" + FKCampaignID.ToString() + "</fkcampaignid>");
            xml.Append("<fkcampaigntypeid>" + FKCampaignTypeID.ToString() + "</fkcampaigntypeid>");
            xml.Append("<fkcampaigngroupid>" + FKCampaignGroupID.ToString() + "</fkcampaigngroupid>");
            xml.Append("<fkcampaigntypegroupid>" + FKCampaignTypeGroupID.ToString() + "</fkcampaigntypegroupid>");
            xml.Append("<fkcampaigngrouptypeid>" + FKCampaignGroupTypeID.ToString() + "</fkcampaigngrouptypeid>");
            xml.Append("<fklanguageid>" + FKLanguageID.ToString() + "</fklanguageid>");
            xml.Append("<description>" + Description.ToString() + "</description>");
            xml.Append("<document>" + Document.ToString() + "</document>");
            xml.Append("<date>" + Date.Value.ToString("dd MMM yyyy HH:mm:ss") + "</date>");
            xml.Append("<isactive>" + IsActive.ToString() + "</isactive>");
            xml.Append("</scripts>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the Scripts object from a list of parameters.
        /// </summary>
        /// <param name="fkscripttypeid"></param>
        /// <param name="fkcampaignid"></param>
        /// <param name="fkcampaigntypeid"></param>
        /// <param name="fkcampaigngroupid"></param>
        /// <param name="fkcampaigntypegroupid"></param>
        /// <param name="fkcampaigngrouptypeid"></param>
        /// <param name="fklanguageid"></param>
        /// <param name="description"></param>
        /// <param name="document"></param>
        /// <param name="date"></param>
        /// <param name="isactive"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkscripttypeid, long? fkcampaignid, long? fkcampaigntypeid, long? fkcampaigngroupid, long? fkcampaigntypegroupid, long? fkcampaigngrouptypeid, long? fklanguageid, string description, byte[] document, DateTime? date, bool? isactive)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKScriptTypeID = fkscripttypeid;
                this.FKCampaignID = fkcampaignid;
                this.FKCampaignTypeID = fkcampaigntypeid;
                this.FKCampaignGroupID = fkcampaigngroupid;
                this.FKCampaignTypeGroupID = fkcampaigntypegroupid;
                this.FKCampaignGroupTypeID = fkcampaigngrouptypeid;
                this.FKLanguageID = fklanguageid;
                this.Description = description;
                this.Document = document;
                this.Date = date;
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
        /// Deletes a(n) Scripts's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the Scripts history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return ScriptsMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an Scripts object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the Scripts object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return ScriptsMapper.UnDelete(this);
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
    /// A collection of the Scripts object.
    /// </summary>
    public partial class ScriptsCollection : ObjectCollection<Scripts>
    { 
    }
    #endregion
}
