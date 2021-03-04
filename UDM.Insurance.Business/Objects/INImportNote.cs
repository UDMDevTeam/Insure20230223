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
    public partial class INImportNote : ObjectBase<long>
    {
        #region Members
        private long? _fkinimportid = null;
        private long? _fkuserid = null;
        private DateTime? _notedate = null;
        private string _note = null;
        private int? _sequence = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INImportNote class.
        /// </summary>
        public INImportNote()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INImportNote class.
        /// </summary>
        public INImportNote(long id)
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

        public long? FKUserID
        {
            get
            {
                Fill();
                return _fkuserid;
            }
            set 
            {
                Fill();
                if (value != _fkuserid)
                {
                    _fkuserid = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? NoteDate
        {
            get
            {
                Fill();
                return _notedate;
            }
            set 
            {
                Fill();
                if (value != _notedate)
                {
                    _notedate = value;
                    _hasChanged = true;
                }
            }
        }

        public string Note
        {
            get
            {
                Fill();
                return _note;
            }
            set 
            {
                Fill();
                if (value != _note)
                {
                    _note = value;
                    _hasChanged = true;
                }
            }
        }

        public int? Sequence
        {
            get
            {
                Fill();
                return _sequence;
            }
            set 
            {
                Fill();
                if (value != _sequence)
                {
                    _sequence = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INImportNote object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INImportNoteMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INImportNote object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportNote object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INImportNoteMapper.Save(this);
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
        /// Deletes an INImportNote object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportNote object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INImportNoteMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INImportNote.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inimportnote>");
            xml.Append("<fkinimportid>" + FKINImportID.ToString() + "</fkinimportid>");
            xml.Append("<fkuserid>" + FKUserID.ToString() + "</fkuserid>");
            xml.Append("<notedate>" + NoteDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</notedate>");
            xml.Append("<note>" + Note.ToString() + "</note>");
            xml.Append("<sequence>" + Sequence.ToString() + "</sequence>");
            xml.Append("</inimportnote>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INImportNote object from a list of parameters.
        /// </summary>
        /// <param name="fkinimportid"></param>
        /// <param name="fkuserid"></param>
        /// <param name="notedate"></param>
        /// <param name="note"></param>
        /// <param name="sequence"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinimportid, long? fkuserid, DateTime? notedate, string note, int? sequence)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINImportID = fkinimportid;
                this.FKUserID = fkuserid;
                this.NoteDate = notedate;
                this.Note = note;
                this.Sequence = sequence;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INImportNote's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportNote history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportNoteMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INImportNote object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportNote object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportNoteMapper.UnDelete(this);
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
    /// A collection of the INImportNote object.
    /// </summary>
    public partial class INImportNoteCollection : ObjectCollection<INImportNote>
    { 
    }
    #endregion
}
