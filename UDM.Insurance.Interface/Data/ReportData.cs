using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UDM.Insurance.Business;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Data
{
    public class ReportData : ObservableObject
    {
        #region Public Properties

        #region Call Monitoring Query Specific Properties



        private lkpINCampTSRReportMode? _callMonitoringReportMode = lkpINCampTSRReportMode.ByTSR;

        public lkpINCampTSRReportMode? CallMonitoringQueryMode
        {
            get { return _callMonitoringReportMode; }
            set { SetProperty(ref _callMonitoringReportMode, value, () => CallMonitoringQueryMode); }
        }

        private lkpINTurnoverCompanyMode? _callMonitoringCompanyMode = lkpINTurnoverCompanyMode.Insurance;

        public lkpINTurnoverCompanyMode? CallMonitoringQueryCompanyMode
        {
            get { return _callMonitoringCompanyMode; }
            set { SetProperty(ref _callMonitoringCompanyMode, value, () => CallMonitoringQueryCompanyMode); }
        }


        #endregion

        #region Turnover Specific Properties

        private lkpINCampTSRReportMode? _turnoverReportMode = lkpINCampTSRReportMode.ByCampaign;

        public lkpINCampTSRReportMode? TurnoverReportMode
        {
            get { return _turnoverReportMode; }
            set { SetProperty(ref _turnoverReportMode, value, () => TurnoverReportMode); }
        }

        private lkpINTurnoverCompanyMode? _turnoverCompanyMode = lkpINTurnoverCompanyMode.Insurance;

        public lkpINTurnoverCompanyMode? TurnoverCompanyMode
        {
            get { return _turnoverCompanyMode; }
            set { SetProperty(ref _turnoverCompanyMode, value, () => TurnoverCompanyMode); }
        }

        private bool _includeBumpups/* = false*/;
        private bool _includeElevationTeam/* = false*/;

        public bool IncludeBumpups
        {
            get { return _includeBumpups; }
            set { SetProperty(ref _includeBumpups, value, () => IncludeBumpups); }
        }

        public bool IncludeElevationTeam
        {
            get { return _includeElevationTeam; }
            set { SetProperty(ref _includeElevationTeam, value, () => IncludeElevationTeam); }
        }

        #endregion Turnover Specific Properties

        #region Batch Export Specific Properties
        private lkpINBatchType? _batchType = lkpINBatchType.Normal;

        public lkpINBatchType? BatchType
        {
            get { return _batchType; }
            set { SetProperty(ref _batchType, value, () => BatchType); }
        }
        #endregion Batch Export Specific Properties

        #region Batch Report Specific Properties

        private bool _combineUL/* = false*/;

        public bool CombineUL
        {
            get { return _combineUL; }
            set { SetProperty(ref _combineUL, value, () => CombineUL); }
        }

        private bool _salesConversionPerBatch;

        public bool SalesConversionPerBatch
        {
            get
            {
                if (_salesConversionPerBatch)
                {
                    ContactsConversionPerBatch = false;
                }

                return _salesConversionPerBatch;
            }
            set {
                
                SetProperty(ref _salesConversionPerBatch, value, () => SalesConversionPerBatch);                
            }
        }

        private bool _contactsConversionPerBatch;

        public bool ContactsConversionPerBatch
        {
            get
            {
                if (_contactsConversionPerBatch)
                {
                    SalesConversionPerBatch = false;
                }

                return _contactsConversionPerBatch;
            }
            set {

                SetProperty(ref _contactsConversionPerBatch, value, () => ContactsConversionPerBatch);                
            }
        }

        private bool _includeAdmin/* = false*/;

        public bool IncludeAdmin
        {
            get { return _includeAdmin; }
            set { SetProperty(ref _includeAdmin, value, () => IncludeAdmin); }
        }

        #endregion Batch Report Specific Properties

        #region Base Sales And Contacts Tracking Report Specific Properties

            private lkpBaseSalesContactsWeeks? _weeks = lkpBaseSalesContactsWeeks.Weeks13;

            public lkpBaseSalesContactsWeeks? Weeks
            {
                get { return _weeks; }
                set { SetProperty(ref _weeks, value, () => Weeks); }
            }

        #endregion Base Sales And Contacts Tracking Report Specific Properties

        #region STL Report Specific Properties
        private lkpINSTLBatchType? _stlBatchType = lkpINSTLBatchType.Combined;

            public lkpINSTLBatchType? STLBatchType
            {
                get { return _stlBatchType; }
                set { SetProperty(ref _stlBatchType, value, () => STLBatchType); }
            }

        private int? _stlOption;

        public int? STLOption
        {
            get { return _stlOption; }
            set { SetProperty(ref _stlOption, value, () => STLOption); }
        }
        #endregion STL Report Specific Properties

        #region Salary Stats Report Specific Properties

        private bool _includeInactiveAgents/* = false*/;

        public bool IncludeInactiveAgents
        {
            get { return _includeInactiveAgents; }
            set { SetProperty(ref _includeInactiveAgents, value, () => IncludeInactiveAgents); }
        }

        #endregion Salary Stats Report Specific Properties

        #endregion
    }
}
