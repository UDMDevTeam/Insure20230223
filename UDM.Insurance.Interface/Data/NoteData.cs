using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Embriant.Framework.Configuration;
using UDM.Insurance.Business;
using UDM.WPF.Library;
using UDM.WPF.Models;

namespace UDM.Insurance.Interface.Data
{

    public class NoteData : ObservableObject
    {

        #region Private Members

        private long? _id;
        private long _fkINImportID;
        private long _fkUserID;
        private DateTime _noteDate;
        private string _note;
        private int _noteCount;
        private int _sequence;
        private int _previousSequence;
        private int _nextSequence;
        private int _maxSequence;
        private int _minSequence;
        private string _navigatorText;
        //private bool _hasNotes;
        private bool _canBeModified;

        #endregion Private Members

        #region Public Members

        public long? ID
        {
            get
            {
                return _id;
            }

            set
            {
                SetProperty(ref _id, value, () => ID);
            }
        }

        public long FKINImportID
        {
            get
            {
                return _fkINImportID;
            }

            set
            {
                {
                    SetProperty(ref _fkINImportID, value, () => FKINImportID);
                }
            }
        }

        public long FKUserID
        {
            get
            {
                return _fkUserID;
            }

            set
            {
                {
                    SetProperty(ref _fkUserID, value, () => FKUserID);
                }
            }
        }

        public DateTime NoteDate
        {
            get
            {
                return _noteDate;
            }

            set
            {
                {
                    SetProperty(ref _noteDate, value, () => NoteDate);
                }
            }
        }

        public string Note
        {
            get
            {
                return _note;
            }

            set
            {
                {
                    SetProperty(ref _note, value, () => Note);
                }
            }
        }

        public int Sequence
        {
            get
            {
                return _sequence;
            }

            set
            {
                {
                    SetProperty(ref _sequence, value, () => Sequence);
                }
            }
        }

        public int PreviousSequence
        {
            get
            {
                return _previousSequence;
            }

            set
            {
                {
                    SetProperty(ref _previousSequence, value, () => PreviousSequence);
                }
            }
        }

        public int NextSequence
        {
            get
            {
                return _nextSequence;
            }

            set
            {
                {
                    SetProperty(ref _nextSequence, value, () => NextSequence);
                }
            }
        }

        public int MaxSequence
        {
            get
            {
                return _maxSequence;
            }

            set
            {
                {
                    SetProperty(ref _maxSequence, value, () => MaxSequence);
                }
            }
        }

        public int MinSequence
        {
            get
            {
                return _minSequence;
            }

            set
            {
                {
                    SetProperty(ref _minSequence, value, () => MinSequence);
                }
            }
        }

        //public bool HasNotes
        //{
        //    get
        //    {
        //        return _hasNotes;
        //    }
        //    set
        //    {
        //        {
        //            SetProperty(ref _hasNotes, value, () => HasNotes);
        //        }
        //    }
        //}

        public string NavigatorText
        {
            get
            {
                return _navigatorText;
            }

            set
            {
                {
                    SetProperty(ref _navigatorText, value, () => NavigatorText);
                }
            }
        }

        public int NoteCount
        {
            get
            {
                return _noteCount;
            }

            set
            {
                {
                    SetProperty(ref _noteCount, value, () => NoteCount);
                }
            }
        }

        public bool CanBeModified
        {
            get
            {
                return _canBeModified;
            }

            set
            {
                SetProperty(ref _canBeModified, value, () => CanBeModified);
            }
        }

        #endregion Public Members

        #region Constructor

        public NoteData()
        {
            //dtTitles = Methods.GetTableData("SELECT [ID], [Description] FROM lkpINTitle");
        }

        #endregion Constructor

        #region Public Methods


        #endregion Public Methods

        #region Private Methods


        #endregion Private Methods

    }

}
