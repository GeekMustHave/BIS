using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

/// <summary>
/// Summary description for UseCase
/// </summary>
public class UseCase
{
    public static DataSet GetUCRequirements(string ucGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spUC_ReqCombo");
        db.AddInParameter(cmd, "sUC_GUID", DbType.String, ucGuid);
        return db.ExecuteDataSet(cmd);
    }

    /// <summary>
    ///  Function to Get all the Use Cases For the Package.
    /// </summary>
    /// <param name="eaGuid"> Package Guid</param>
    /// <param name="reqGuid">to get only that belongs to a certian requiremnt</param>
    /// <param name="searchText">search text to search on use cases</param>
    /// <param name="showhidden">to show the hidden uses cases</param>
    /// <returns></returns>
    public static DataSet GetUseCaseList(string eaGuid, string reqGuid = "", string searchText = "", bool showhidden = true)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spUCList");
        db.AddInParameter(cmd, "sEA_GUID", DbType.String, eaGuid);

        if (!(string.IsNullOrEmpty(searchText)))
            db.AddInParameter(cmd, "sSearchText", DbType.String, searchText);
        if (!(string.IsNullOrEmpty(reqGuid)))
            db.AddInParameter(cmd, "spkgREQ_ID", DbType.String, reqGuid);
        db.AddInParameter(cmd, "bHidden", DbType.Boolean, showhidden);
        return db.ExecuteDataSet(cmd);
    }

    public static void AddUcReqRelation(string ucGuid, string reqguid, string createdbyUId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spReq_UCAdd");
        db.AddInParameter(cmd, "sReq_GUID", DbType.String, reqguid);
        db.AddInParameter(cmd, "sUC_GUID", DbType.String, ucGuid);
        db.AddInParameter(cmd, "sUser_GUID", DbType.String, createdbyUId);
        db.ExecuteNonQuery(cmd);
    }

    public static void DeleteUcReqRelation(string ucGuid, string reqguid, string createdbyUId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spReq_UCDelete");
        db.AddInParameter(cmd, "sReq_GUID", DbType.String, reqguid);
        db.AddInParameter(cmd, "sUC_GUID", DbType.String, ucGuid);
        db.AddInParameter(cmd, "sUser_GUID", DbType.String, createdbyUId);
        db.ExecuteNonQuery(cmd);
    }

    public static void CreateUseCase(string ucPkgGuid, string ucName, string notes, string createdbyUId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spUC_New");
        db.AddInParameter(cmd, "sPackage_GUID", DbType.String, ucPkgGuid);
        db.AddInParameter(cmd, "sName", DbType.String, ucName);
        db.AddInParameter(cmd, "sNote", DbType.String, notes);
        db.AddInParameter(cmd, "sCreatedBy", DbType.String, createdbyUId);
        db.ExecuteNonQuery(cmd);
    }
    public static void UpdateUseCase(string ucGuid, string ucName, string notes, string statuguid, string createdbyUId, bool hidden)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spUC_Update");
        db.AddInParameter(cmd, "sUC_GUID", DbType.String, ucGuid);
        db.AddInParameter(cmd, "sName", DbType.String, ucName);
        db.AddInParameter(cmd, "sNote", DbType.String, notes);
        db.AddInParameter(cmd, "sStatus_GUID", DbType.String, statuguid);
        db.AddInParameter(cmd, "sCreatedBy", DbType.String, createdbyUId);
        db.AddInParameter(cmd, "bHidden", DbType.Boolean, hidden);
        db.ExecuteNonQuery(cmd);
    }
    public static DataSet GetUseCaseDetailView(string eaGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spUCDetailView");
        db.AddInParameter(cmd, "sUC_GUID", DbType.String, eaGuid);
        return db.ExecuteDataSet(cmd);
    }
    public static DataSet GetUseCaseHistory(string eaGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spUCHistory");
        db.AddInParameter(cmd, "ea_guid", DbType.String, eaGuid);
        return db.ExecuteDataSet(cmd);
    }
    public static DataSet GetUseRequirements(string eaGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spUC_ReqList");
        db.AddInParameter(cmd, "sUC_GUID", DbType.String, eaGuid);
        return db.ExecuteDataSet(cmd);
    }

    public static DataSet GetUseCaseHistoryDetails(string eguid, int versionNum)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spUCHistoryDetail");
        db.AddInParameter(cmd, "ea_guid", DbType.String, eguid);
        db.AddInParameter(cmd, "lVersion", DbType.Int32, versionNum);
        return db.ExecuteDataSet(cmd);
    }

    /// <summary>
    /// Fucntion to get al the Steps for a flow.
    /// </summary>
    /// <param name="flowguid"></param>
    /// <returns>dataset of steps</returns>
    public static DataSet GetUCStepsList(string flowguid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spUC_StepList");
        db.AddInParameter(cmd, "sFlow_GUID", DbType.String, flowguid);
        return db.ExecuteDataSet(cmd);
    }

    /// <summary>
    /// fucntion to get flows for a use case
    /// </summary>
    /// <param name="ucGuid"> use case guid</param>
    /// <returns>data set of flows.</returns>
    public static DataSet GetUCFlowsCombo(string ucGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spFlowCombo");
        db.AddInParameter(cmd, "sUC_GUID", DbType.String, ucGuid);
        return db.ExecuteDataSet(cmd);
    }

    /// <summary>
    /// funciton to get entry points for a flow
    /// </summary>
    /// <param name="flowGuid">flow guid</param>
    /// <returns>dataset of entry points</returns>
    public static DataSet GetUCFlowEntryCombo(string flowGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spFlowEntryCombo");
        db.AddInParameter(cmd, "sFlow_GUID", DbType.String, flowGuid);
        return db.ExecuteDataSet(cmd);
    }

    /// <summary>
    /// function to get joins for a flow
    /// </summary>
    /// <param name="flowGuid">flow guid</param>
    /// <returns>dataset of entrry points</returns>
    public static DataSet GetUCFlowJoinCombo(string flowGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spFlowJoinCombo");
        db.AddInParameter(cmd, "sFlow_GUID", DbType.String, flowGuid);
        return db.ExecuteDataSet(cmd);
    }
    /// <summary>
    /// Fucntion to get Flows Details for a use case.
    /// </summary>
    /// <param name="ucGuid">Use case guid</param>
    /// <returns>data set of flows with details</returns>
    public static DataSet GetUCFlowsList(string ucGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spUC_FlowList");
        db.AddInParameter(cmd, "sUC_GUID", DbType.String, ucGuid);
        return db.ExecuteDataSet(cmd);
    }
    /// <summary>
    /// Function to Get Busniess Rules For an Use Case
    /// </summary>
    /// <param name="ucStepGuid">Step Guid</param>
    /// <returns>data set of business rules</returns>
    public static DataSet GetUCStepBusinessRulesList(string ucStepGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spBusinessRuleList");
        db.AddInParameter(cmd, "@sStep_GUID", DbType.String, ucStepGuid);
        return db.ExecuteDataSet(cmd);
    }
    /// <summary>
    /// Sub to Add New Step After the selected Step for a Use cAse
    /// </summary>
    /// <param name="stepGuid">Current Step Guid</param>
    /// <param name="actorSystemDoes">Acotr and System does as Json</param>
    /// <param name="createdbyUId">Created By USerID</param>
    public static void AddUCStep(string stepGuid, string actorSystemDoes, string createdbyUId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spStepAdd");
        db.AddInParameter(cmd, "sSelectedStep_GUID", DbType.String, stepGuid);
        db.AddInParameter(cmd, "sActorSystem", DbType.String, actorSystemDoes);
        db.AddInParameter(cmd, "sUser_GUID", DbType.String, createdbyUId);

        db.ExecuteNonQuery(cmd);
    }
    /// <summary>
    /// Sub to Update a step for a use case
    /// </summary>
    /// <param name="stepGuid">step guid to be updated</param>
    /// <param name="actorSystemDoes">acto system does json string</param>
    public static void UpdateUCStep(string stepGuid, string actorSystemDoes)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spStepEdit");
        db.AddInParameter(cmd, "sStep_GUID", DbType.String, stepGuid);
        db.AddInParameter(cmd, "sActorSystem", DbType.String, actorSystemDoes);
        db.ExecuteNonQuery(cmd);
    }
    /// <summary>
    /// Fucntion to verify if the step can be deleted or not. 
    /// </summary>
    /// <param name="stepGuid">step guid to be deleted</param>
    /// <returns>true if step can be deleted</returns>
    public static bool VerifyDeleteUCStep(string stepGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spStepDeleteVerify");
        db.AddInParameter(cmd, "sStepToBeDeleted_GUID", DbType.String, stepGuid);
        DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
        return (dt.Rows.Count == 0 ? true : false);
    }
    /// <summary>
    /// Fucntion to delete the step for a use case
    /// </summary>
    /// <param name="stepGuid"> step guid to delete</param>
    public static void DeleteUCStep(string stepGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spStepDelete");
        db.AddInParameter(cmd, "sStep_GUID", DbType.String, stepGuid);
        db.ExecuteNonQuery(cmd);
    }

    /// <summary>
    /// Funtion to add a new Flow to the Use CAse
    /// </summary>
    /// <param name="ucGuid">current USe case id</param>
    /// <param name="entrypointGuid">entry point guid</param>
    /// <param name="flowName">flow name</param>
    /// <param name="flowType">flow type </param>
    /// <param name="joinPointGuid">flow join guid</param>
    /// <param name="createdbyUId">created by user guid</param>
    public static void AddUCFlow(string ucGuid, string entrypointGuid, string flowName, string flowType, string joinPointGuid, string createdbyUId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spFlowAdd");
        db.AddInParameter(cmd, "sUC_GUID", DbType.String, ucGuid);
        db.AddInParameter(cmd, "sEntryPoint_GUID", DbType.String, entrypointGuid);
        db.AddInParameter(cmd, "sFlowName", DbType.String, flowName);
        db.AddInParameter(cmd, "sFlowType", DbType.String, flowType);
        db.AddInParameter(cmd, "sJoinPoint_GUID", DbType.String, joinPointGuid);
        db.AddInParameter(cmd, "sUser_GUID", DbType.String, createdbyUId);

        db.ExecuteNonQuery(cmd);
    }

    /// <summary>
    /// Fiunction to update an existing usecase flow
    /// </summary>
    /// <param name="flowGuid">flow guid</param>
    /// <param name="entrypointGuid"> entry point guid</param>
    /// <param name="flowName">name of the flow</param>
    /// <param name="flowNote">notes for the flow</param>
    /// <param name="flowType">type of flow</param>
    /// <param name="joinPointGuid">joint point guid</param>
    /// <param name="createdbyUId">last updated by guid</param>
    public static void UpdateUCFlow(string flowGuid, string entrypointGuid, string flowName, string flowType, string joinPointGuid, string createdbyUId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spFlowUpdate");
        db.AddInParameter(cmd, "sFlow_GUID", DbType.String, flowGuid);
        db.AddInParameter(cmd, "sEntryPoint_GUID", DbType.String, entrypointGuid);
        db.AddInParameter(cmd, "sFlowName", DbType.String, flowName);        
        db.AddInParameter(cmd, "sFlowType", DbType.String, flowType);
        db.AddInParameter(cmd, "sJoinPoint_GUID", DbType.String, joinPointGuid);
        db.AddInParameter(cmd, "sUser_GUID", DbType.String, createdbyUId);

        db.ExecuteNonQuery(cmd);
    }

    /// <summary>
    /// Fucntion to Hide unhide a Use Case Flow
    /// </summary>    
    /// <param name="flowGuid">flow guid</param>
    /// <param name="createdbyUId">created by guid</param>
    public static void HideUnhideUCFlow(string flowGuid, string createdbyUId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spFlowHideToggle");
        db.AddInParameter(cmd, "sFlow_GUID", DbType.String, flowGuid);
        db.AddInParameter(cmd, "sUser_GUID", DbType.String, createdbyUId);
        db.ExecuteNonQuery(cmd);
    }
    /// <summary>
    /// Function to delete a business rule from a step
    /// </summary>
    /// <param name="iBRobjPK">Object Pk of Business Rule</param>
    /// <param name="createdbyUId">Updated By user id</param>
    public static void DeleteUCStepBusinessRule(int iBRobjPK, string createdbyUId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spBusinessRuleDelete");
        db.AddInParameter(cmd, "@sBR_ObjPK", DbType.Int64, iBRobjPK);        
        db.AddInParameter(cmd, "sUser_GUID", DbType.String, createdbyUId);
        db.ExecuteNonQuery(cmd);
    }
    /// <summary>
    /// Get business rule Details 
    /// </summary>
    /// <param name="BR_eGuid">Business rule guid</param>
    /// <returns>DS with Business rule Details</returns>
    public static DataSet GetBusinessRuleDetailView(string BR_eGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spBusinessRuleDetail");
        db.AddInParameter(cmd, "sBR_guid", DbType.String, BR_eGuid);
        return db.ExecuteDataSet(cmd);
    }    
    /// <summary>
    /// Fucntion to return Step details
    /// </summary>
    /// <param name="sStep_eGuid">step guid</param>
    /// <returns>step details</returns>
    public static DataSet GetUCStepDetailView(string sStep_eGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spStepRead");
        db.AddInParameter(cmd, "sStep_GUID", DbType.String, sStep_eGuid);
        return db.ExecuteDataSet(cmd);
    }

    /// <summary>
    /// Sub to Update Exsisting Business rule
    /// </summary>
    /// <param name="brGuid">businee rule guid</param>
    /// <param name="brName">name of the br</param>
    /// <param name="brNote">note for the business rule</param>
    /// <param name="createdbyUId">updated by user's guid</param>
    public static void UpdateUCStepBusinessRule(string brGuid, string brName, string brNote, string createdbyUId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spBusinessRuleUpdate");
        db.AddInParameter(cmd, "sBR_GUID", DbType.String, brGuid);
        db.AddInParameter(cmd, "sBR_Name", DbType.String, brName);
        db.AddInParameter(cmd, "sBR_Note", DbType.String, brNote);        
        db.AddInParameter(cmd, "sUser_GUID", DbType.String, createdbyUId);

        db.ExecuteNonQuery(cmd);
    }
    /// <summary>
    /// Sub To Add a new Business rule to a step
    /// </summary>
    /// <param name="sPackageGUID">package the step belog to </param>
    /// <param name="sucStepGUID">step the business rule needs to be added to </param>
    /// <param name="brName">name of Busines  rule</param>
    /// <param name="brNote">Notes for Business rule</param>    
    /// <param name="createdbyUId">created by user guid</param>
    public static void AddUCStepBusinessRule(string sPackageGUID, string sucStepGUID, string brName, string brNote, string createdbyUId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spBusinessRuleAdd");
        db.AddInParameter(cmd, "sPackage_GUID ", DbType.String, sPackageGUID);
        db.AddInParameter(cmd, "sucStep_GUID", DbType.String, sucStepGUID);
        db.AddInParameter(cmd, "sBR_Name", DbType.String, brName);
        db.AddInParameter(cmd, "sBR_Note", DbType.String, brNote);
        
        db.AddInParameter(cmd, "sUser_GUID", DbType.String, createdbyUId);

        db.ExecuteNonQuery(cmd);
    }

    #region "Error Message Helper Code"
    /// <summary>
    /// Function to delete a Error Message from a step
    /// </summary>
    /// <param name="iBRobjPK">Object Pk of Error Message</param>
    /// <param name="createdbyUId">Updated By user id</param>
    public static void DeleteUCStepErrorMessage(int iBRobjPK, string createdbyUId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spErrorMessageDelete");
        db.AddInParameter(cmd, "@sEM_ObjPK", DbType.Int64, iBRobjPK);
        db.AddInParameter(cmd, "sUser_GUID", DbType.String, createdbyUId);
        db.ExecuteNonQuery(cmd);
    }
    /// <summary>
    /// Get Error Message Details 
    /// </summary>
    /// <param name="EM_eGuid">Error Message guid</param>
    /// <returns>DS with Error Message Details</returns>
    public static DataSet GetErrorMessageDetailView(string EM_eGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spErrorMessageDetail");
        db.AddInParameter(cmd, "sEM_guid", DbType.String, EM_eGuid);
        return db.ExecuteDataSet(cmd);
    }
    /// <summary>
    /// Sub to Update Exsisting Error Message
    /// </summary>
    /// <param name="brGuid">Error Message guid</param>
    /// <param name="brName">name of the br</param>
    /// <param name="brNote">note for the Error Message</param>
    /// <param name="createdbyUId">updated by user's guid</param>
    public static void UpdateUCStepErrorMessage(string brGuid, string brName, string brNote, string createdbyUId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spErrorMessageUpdate");
        db.AddInParameter(cmd, "sEM_GUID", DbType.String, brGuid);
        db.AddInParameter(cmd, "sEM_Name", DbType.String, brName);
        db.AddInParameter(cmd, "sEM_Note", DbType.String, brNote);
        db.AddInParameter(cmd, "sUser_GUID", DbType.String, createdbyUId);

        db.ExecuteNonQuery(cmd);
    }
    /// <summary>
    /// Sub To Add a new Error Message to a step
    /// </summary>
    /// <param name="sPackageGUID">package the step belog to </param>
    /// <param name="sucStepGUID">step the Error Message needs to be added to </param>
    /// <param name="brName">name of Error  Message</param>
    /// <param name="brNote">Notes for Error Message</param>    
    /// <param name="createdbyUId">created by user guid</param>
    public static void AddUCStepErrorMessage(string sPackageGUID, string sucStepGUID, string brName, string brNote, string createdbyUId)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spErrorMessageAdd");
        db.AddInParameter(cmd, "sPackage_GUID ", DbType.String, sPackageGUID);
        db.AddInParameter(cmd, "sucStep_GUID", DbType.String, sucStepGUID);
        db.AddInParameter(cmd, "sEM_Name", DbType.String, brName);
        db.AddInParameter(cmd, "sEM_Note", DbType.String, brNote);

        db.AddInParameter(cmd, "sUser_GUID", DbType.String, createdbyUId);

        db.ExecuteNonQuery(cmd);
    }
    /// <summary>
    /// Function to Get Error Messages For an Use Case
    /// </summary>
    /// <param name="ucStepGuid">Step Guid</param>
    /// <returns>data set of Error Messages</returns>
    public static DataSet GetUCStepErrorMessagesList(string ucStepGuid)
    {
        Database db = DatabaseFactory.CreateDatabase("DefaultConnString");
        DbCommand cmd = db.GetStoredProcCommand("spErrorMessageList");
        db.AddInParameter(cmd, "@sStep_GUID", DbType.String, ucStepGuid);
        return db.ExecuteDataSet(cmd);
    }
    #endregion
}