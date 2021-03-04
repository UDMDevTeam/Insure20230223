REM @echo off

set target=C:\Target\Insurance\

set codegen=..\udmcore\Embriant.Toolset\bin\Release\Embriant.Toolset.exe /tool:codegenerator /sourcedatabase:UDMInsure /targetfolder:%target% /namespace:UDM.Insurance.Business /frameworknamespace:Embriant

rd %target% /s /q

REM %codegen% /sourcetablename:Closure
REM %codegen% /sourcetablename:PostalCode
REM %codegen% /sourcetablename:User
REM %codegen% /sourcetablename:UserSystem

REM %codegen% /sourcetablename:INBankDetails

REM %codegen% /sourcetablename:INBatch

REM %codegen% /sourcetablename:INBeneficiary
REM %codegen% /sourcetablename:INBumpUpOption
REM %codegen% /sourcetablename:INCampaign
REM %codegen% /sourcetablename:INChild
REM %codegen% /sourcetablename:INDeclineReason

REM %codegen% /sourcetablename:INImport

REM %codegen% /sourcetablename:INDiaryReason

REM %codegen% /sourcetablename:INImportHeader
REM %codegen% /sourcetablename:INLead
REM %codegen% /sourcetablename:INLeadBook
REM %codegen% /sourcetablename:INLeadBookImport
REM %codegen% /sourcetablename:INLeadStatus

REM %codegen% /sourcetablename:INLifeAssured

REM %codegen% /sourcetablename:INMoneyBack
REM %codegen% /sourcetablename:INOption
REM %codegen% /sourcetablename:INPlan
REM %codegen% /sourcetablename:INPlanGroup
REM %codegen% /sourcetablename:INPolicy
REM %codegen% /sourcetablename:INPolicyBeneficiary
REM %codegen% /sourcetablename:INPolicyChild
REM %codegen% /sourcetablename:INPolicyLifeAssured
REM %codegen% /sourcetablename:INUserSchedule
REM %codegen% /sourcetablename:UserHours
REM %codegen% /sourcetablename:INImportedPolicyData

REM %codegen% /sourcetablename:TSRTargets
REM %codegen% /sourcetablename:TSRCampaignTargetDefaults

REM %codegen% /sourcetablename:INImportSchedules

REM %codegen% /sourcetablename:INFee
REM %codegen% /sourcetablename:INOptionFee

REM %codegen% /sourcetablename:INDefaultOption

REM %codegen% /sourcetablename:INImportOther

REM %codegen% /sourcetablename:INCampaignGroupSet
REM %codegen% /sourcetablename:INCampaignTypeSet

REM %codegen% /sourcetablename:InfoLog1

REM %codegen% /sourcetablename:INUserTypeLeadStatus

REM %codegen% /sourcetablename:INGiftOption
REM %codegen% /sourcetablename:INGiftRedeem

REM %codegen% /sourcetablename:Scripts

REM %codegen% /sourcetablename:CallData

REM %codegen% /sourcetablename:INFakeBankAccountNumbers

REM %codegen% /sourcetablename:INImportCallMonitoring

REM %codegen% /sourcetablename:CallMonitoringAllocation

REM %codegen% /sourcetablename:INImportExtra

REM %codegen% /sourcetablename:INNextOfKin

REM %codegen% /sourcetablename:PossibleBumpUpAllocation

REM %codegen% /sourcetablename:INImportMining

REM %codegen% /sourcetablename:ImportCMStandardNotes

REM %codegen% /sourcetablename:ReportTrackUpgradeBasePoliciesData

REM %codegen% /sourcetablename:SystemInfo

REM %codegen% /sourcetablename:Mercantile

REM %codegen% /sourcetablename:CallMonitoringUnallocation

REM %codegen% /sourcetablename:SMS

REM %codegen% /sourcetablename:INQADetailsQuestion
REM %codegen% /sourcetablename:INQADetailsQuestionType
REM %codegen% /sourcetablename:INQADetailsAnswerType
REM %codegen% /sourcetablename:INQADetailsINImport
REM %codegen% /sourcetablename:INQADetailsQuestionINImport

%codegen% /sourcetablename:INOptionExtra

pause

