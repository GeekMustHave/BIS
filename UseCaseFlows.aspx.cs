using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

/// <summary>
/// Description: Use case Flow Module for a selected Package.
/// History:
/// -------------------------------------------------------------------------------
/// Date:               Name                Description
/// --------------------------------------------------------------------------------
/// 7/20/2015           JHRS                Intial Creation
/// </summary>
public partial class UseCaseFlows : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //See if the user has selected a package. If not display error message to select a pacakge first.
        if (!Page.IsPostBack)
        {
            string prevUrl = string.Empty;
            try
            {
                prevUrl = Request.UrlReferrer.ToString();
            }
            catch (Exception)
            {
                ////User forced the URI in the Browser.  ** It should be redirected from UseCases.asps page.
                Response.Redirect("~/UseCases.aspx", true);
            }

            if ((Session["svSelectedPackage"] == null) || (Session["svSelectedUseCase"] == null))
            {
                lblPackageName.Text = "Package: None";
                lblUseCaseName.Text = "UseCase: None";
                mvUseCases.SetActiveView(vwNoSelect);
            }
            else
            {
                //User selected a package. Get the PAckage name and bind the UseCases to the grid.
                string PrjPkgName = Requirement.GetProjectPackageName(Session["svSelectedPackage"].ToString());
                if (string.IsNullOrEmpty(PrjPkgName))
                {
                    lblError.Text = "Error: No data found for this Project. Cannot View Use Case Flows.!";
                }
                else
                {
                    lblPackageName.Text = "Package: " + PrjPkgName;
                    DataTable dtUCDeetails = UseCase.GetUseCaseDetailView(Session["svSelectedUseCase"].ToString()).Tables[0];
                    if (dtUCDeetails.Rows.Count == 0)
                    {
                        lblError.Text = "No data found for Details";
                        return;
                    }
                    else
                    {
                        lblUseCaseName.Text = "UseCase: " + dtUCDeetails.Rows[0]["Name"].ToString().Trim();
                        mvUseCases.SetActiveView(vwDefault);
                        BindFlowsDropDown();
                        BindStepsListGrid();
                        BindFlowsGrid();
                    }
                }
            }

        }
    }

    #region " button/Postback Events"
    protected void ddlFlows_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindStepsListGrid();
        UpdatePanelUCFlows.Update();
    }
    protected void btnAddNewFlow_Click(object sender, EventArgs e)
    {
        PrepareForAddNewFlow();
    }
    protected void btnCreateUpdateStepClose_Click(object sender, EventArgs e)
    {
        mpCreateUpdateStep.Hide();
    }
    protected void btnCreateUpdateStep_Click(object sender, EventArgs e)
    {
        //Button Click event for Create and update a Requiremnt.
        try
        {
            Button btn = (Button)sender;
            switch (btn.CommandName)
            {
                case "CreateStep":
                    //Create requirement. and rebind the grid to update the created one.
                    try
                    {
                        string actorSystemDoesJson = ConvertToJson(Server.HtmlDecode(htmlEditorCreateUpdateStepActorDoes.Text),
                                                        Server.HtmlDecode(htmlEditorCreateUpdateStepSystemDoes.Text));
                        UseCase.AddUCStep(hfCurentStepId.Value, actorSystemDoesJson, ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);
                        // Hide the PopUp and update the grid.
                        mpCreateUpdateStep.Hide();
                        upCreateUpdateStep.Update();
                        BindStepsListGrid();
                        BindFlowsGrid();
                        UpdatePanelUCFlows.Update();
                    }
                    catch (Exception Ex)
                    {
                        lblErrorCreateUpdateStep.Text = Ex.Message.ToString();
                        mpCreateUpdateStep.Show();
                        upCreateUpdateStep.Update();
                    }
                    break;
                case "UpdateStep":
                    //USe sp to update requirement  and rebind the grid to update the Updated REquirement.
                    try
                    {
                        string actorSystemDoesJsonUpdate = ConvertToJson(Server.HtmlDecode(htmlEditorCreateUpdateStepActorDoes.Text),
                                                        Server.HtmlDecode(htmlEditorCreateUpdateStepSystemDoes.Text));
                        UseCase.UpdateUCStep(hfCurentStepId.Value, actorSystemDoesJsonUpdate);
                        mpCreateUpdateStep.Hide();
                        upCreateUpdateStep.Update();
                        BindStepsListGrid();
                        UpdatePanelUCFlows.Update();
                    }
                    catch (Exception ex)
                    {
                        lblErrorCreateUpdateStep.Text = ex.Message.ToString();
                        mpCreateUpdateStep.Show();
                        upCreateUpdateStep.Update();
                    }
                    break;
            }
            lblError.Text = "";
            // To supress re creating/updating Req on F5.
            //Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }
        catch (Exception ex)
        {
            lblErrorCreateUpdateStep.Text = ex.Message.ToString();
            lblErrorCreateUpdateStep.ForeColor = System.Drawing.Color.Red;
        }
    }

    //Flow Create Update Close Buttons
    protected void btnCreateUpdateFlowClose_Click(object sender, EventArgs e)
    {
        BindFlowsGrid();
        UpdatePanelUCFlows.Update();
        mpCreateUpdateFlow.Hide();
    }
    protected void btnCreateUpdateFlow_Click(object sender, EventArgs e)
    {
        //Button Click event for Create and update a Flow.
        try
        {
            Button btn = (Button)sender;
            switch (btn.CommandName)
            {
                case "CreateFlow":
                    //Create requirement. and rebind the grid to update the created one.
                    try
                    {
                        UseCase.AddUCFlow(Session["svSelectedUseCase"].ToString(), ddlCreateUpdateFlowEntryPoint.SelectedValue,
                                            txtCreateUpdateFlowName.Text.Trim(), ddlCreateUpdateFlowType.SelectedValue, ddlCreateUpdateFlowJoin.SelectedValue,
                                            ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);
                        // Hide the PopUp and update the grid.
                        mpCreateUpdateFlow.Hide();
                        upCreateUpdateFlow.Update();
                        BindFlowsDropDown();
                        BindFlowsGrid();
                        UpdatePanelUCFlows.Update();
                    }
                    catch (Exception Ex)
                    {
                        lblErrorCreateUpdateFlow.Text = "Error While Adding Flow: " + Ex.Message.ToString();
                        mpCreateUpdateFlow.Show();
                        upCreateUpdateFlow.Update();
                    }
                    break;
                case "UpdateFlow":
                    //USe sp to update requirement  and rebind the grid to update the Updated REquirement.
                    try
                    {
                        //Upda the Flow HERe

                        UseCase.UpdateUCFlow(hfCurrentFlowID.Value, ddlCreateUpdateFlowEntryPoint.SelectedValue,
                                           txtCreateUpdateFlowName.Text.Trim(), ddlCreateUpdateFlowType.SelectedValue, ddlCreateUpdateFlowJoin.SelectedValue,
                                           ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);

                        // Hide the PopUp and update the grid.
                        mpCreateUpdateFlow.Hide();
                        upCreateUpdateFlow.Update();
                        BindFlowsDropDown();
                        BindFlowsGrid();
                        UpdatePanelUCFlows.Update();
                    }
                    catch (Exception ex)
                    {
                        lblErrorCreateUpdateFlow.Text = "Error While Adding Flow: " + ex.Message.ToString();
                        mpCreateUpdateFlow.Show();
                        upCreateUpdateFlow.Update();
                    }
                    break;
            }
            lblError.Text = "";
            // To supress re creating/updating Req on F5.
            //Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }
        catch (Exception ex)
        {
            lblErrorCreateUpdateStep.Text = ex.Message.ToString();
            lblErrorCreateUpdateStep.ForeColor = System.Drawing.Color.Red;
        }
    }

    //When USer selects alternative flow then they will see a list of all the steps in the flow, 
    //if the user select Exception flow the only choice is 'end'
    protected void ddlCreateUpdateFlowType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCreateUpdateFlowType.SelectedValue.ToString().Equals("Exception"))
        {
            BindFlowJoinWhileException();
        }
        else
        {
            BindFlowJoinCombo();
        }
    }
    #endregion

    #region "Grid Events"
    protected void GridStepsForFlow_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //Row command only if it's not a page index change event
        if (!e.CommandName.ToLower().Contains("page"))
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument) - GridStepsForFlow.PageSize * GridStepsForFlow.PageIndex;
            string eguid = GridStepsForFlow.DataKeys[rowIndex]["Step_GUID"].ToString();
            hfCurentStepId.Value = eguid;
            upCreateUpdateStep.Update();
            // Code for link clicks on Steps grid.        
            if (e.CommandName == "AddStep")
            {
                //Open popup to add new Step.
                tblStepAddEdit.Visible = true;
                lblCreateUpdateStepId.Text = (Convert.ToInt32(GridStepsForFlow.DataKeys[rowIndex]["StepNumber"].ToString()) + 1).ToString();
                lblCreateUpdateStepFlowName.Text = ddlFlows.SelectedItem.Text.ToString();
                htmlEditorCreateUpdateStepActorDoes.Text = string.Empty;
                htmlEditorCreateUpdateStepSystemDoes.Text = string.Empty;
                btnCreateUpdateStep.Visible = true;
                btnCreateUpdateStep.CommandName = "CreateStep";
                btnCreateUpdateStepClose.Text = "Cancel";
                lblErrorCreateUpdateStep.Text = string.Empty;
                upCreateUpdateStep.Update();
                mpCreateUpdateStep.Show();
            }
            else if (e.CommandName == "EditStep")
            {
                //Open popup to EDIT  Step.
                tblStepAddEdit.Visible = true;
                lblCreateUpdateStepId.Text = GridStepsForFlow.DataKeys[rowIndex]["StepNumber"].ToString();

                string actorSystemDoesJson = GridStepsForFlow.DataKeys[rowIndex]["ActorSystemDoes"].ToString();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ActorSystemDoes));
                MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(actorSystemDoesJson));
                ActorSystemDoes result = serializer.ReadObject(ms) as ActorSystemDoes;
                htmlEditorCreateUpdateStepActorDoes.Text = result.ActorDoes;
                htmlEditorCreateUpdateStepSystemDoes.Text = result.SystemDoes;

                lblCreateUpdateStepFlowName.Text = ddlFlows.SelectedItem.Text.ToString();
                btnCreateUpdateStep.Visible = true;
                btnCreateUpdateStep.CommandName = "UpdateStep";
                btnCreateUpdateStepClose.Text = "Cancel";
                lblErrorCreateUpdateStep.Text = string.Empty;
                upCreateUpdateStep.Update();
                mpCreateUpdateStep.Show();
            }
            else if (e.CommandName == "DeleteStep")
            {
                if (UseCase.VerifyDeleteUCStep(eguid))
                {
                    //Delete the Step and rebind the steps grid
                    UseCase.DeleteUCStep(eguid);
                    BindStepsListGrid();
                    BindFlowsGrid();
                    UpdatePanelUCFlows.Update();
                }
                else
                {
                    mpDeleteStep.Show();
                }
            }
            else if (e.CommandName == "ViewUpdateBusinessRule")
            {
                string stepnumber = GridStepsForFlow.DataKeys[rowIndex]["StepNumber"].ToString();

                string actorSystemDoesJson = GridStepsForFlow.DataKeys[rowIndex]["ActorSystemDoes"].ToString();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ActorSystemDoes));
                MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(actorSystemDoesJson));
                ActorSystemDoes result = serializer.ReadObject(ms) as ActorSystemDoes;
                hfRuleListCurStepId.Value = eguid;
                upBusinessRuleList.Update();
                if (!Convert.ToBoolean(Convert.ToInt32(GridStepsForFlow.DataKeys[rowIndex]["BR_Flag"].ToString())))
                {
                    //no Business Rules are present  - Open Add New BR Popup
                    //Bind the Step DEtails on the popup
                    lblBusinessRuleAddEditStep.Text = stepnumber;
                    lblBusinessRuleAddEditActorDoes.Text = result.ActorDoes;
                    lblBusinessRuleAddEditSystemDoes.Text = result.SystemDoes;

                    lblBusinessRuleAddEditFlow.Text = ddlFlows.SelectedItem.Text.ToString();
                    BindBusinessRulesAddEditRules();
                    txtBusinessRuleAddEditRuleName.Text = string.Empty;
                    htmlEditorBusinessRuleAddEditNote.Text = string.Empty;
                    btnBusinessRuleAddEditAddRule.CommandName = "AddBusinessRule";
                    lblBusinessRuleAddEditError.Text = string.Empty;
                    tdBusinessruleEdit.Visible = false;
                    trBusinessruleAdd.Visible = true;
                    upBusinessRuleAddEdit.Update();
                    mpBusinessRuleAddEdit.Show();
                }
                else
                {
                    // Open edit Business Rules PopUp.
                    //Bind the Step DEtails on the popup
                    lblBusinessRuleListStep.Text = stepnumber;
                    lblBusinessRuleListActorDoes.Text = result.ActorDoes;
                    lblBusinessRuleListSystemDoes.Text = result.SystemDoes;

                    lblBusinessRuleListFlow.Text = ddlFlows.SelectedItem.Text.ToString();
                    BindGridBusinessRules(eguid);
                    upBusinessRuleList.Update();
                    mpBusinessRuleList.Show();
                }
            }
            else if (e.CommandName == "ViewUpdateErrorMessage")
            {
                string stepnumber = GridStepsForFlow.DataKeys[rowIndex]["StepNumber"].ToString();

                string actorSystemDoesJson = GridStepsForFlow.DataKeys[rowIndex]["ActorSystemDoes"].ToString();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ActorSystemDoes));
                MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(actorSystemDoesJson));
                ActorSystemDoes result = serializer.ReadObject(ms) as ActorSystemDoes;
                //hfMessageListCurStepId.Value = eguid;
                upErrorMessageList.Update();
                if (!Convert.ToBoolean(Convert.ToInt32(GridStepsForFlow.DataKeys[rowIndex]["EM_Flag"].ToString())))
                {
                    //no Error Messages are present  - Open Add New EM Popup
                    //Bind the Step DEtails on the popup
                    lblErrorMessageAddEditStep.Text = stepnumber;
                    lblErrorMessageAddEditActorDoes.Text = result.ActorDoes;
                    lblErrorMessageAddEditSystemDoes.Text = result.SystemDoes;

                    lblErrorMessageAddEditFlow.Text = ddlFlows.SelectedItem.Text.ToString();
                    BindErrorMessagesAddEditMessages();
                    txtErrorMessageAddEditMessageName.Text = string.Empty;
                    htmlEditorErrorMessageAddEditNote.Text = string.Empty;
                    btnErrorMessageAddEditAddMessage.CommandName = "AddErrorMessage";
                    lblErrorMessageAddEditError.Text = string.Empty;
                    tdErrorMessageEdit.Visible = false;
                    trErrorMessageAdd.Visible = true;
                    upErrorMessageAddEdit.Update();
                    mpErrorMessageAddEdit.Show();
                }
                else
                {
                    // Open edit Error Messages PopUp.
                    //Bind the Step DEtails on the popup
                    lblErrorMessageListStep.Text = stepnumber;
                    lblErrorMessageListActorDoes.Text = result.ActorDoes;
                    lblErrorMessageListSystemDoes.Text = result.SystemDoes;

                    lblErrorMessageListFlow.Text = ddlFlows.SelectedItem.Text.ToString();
                    BindGridErrorMessages(eguid);
                    upErrorMessageList.Update();
                    mpErrorMessageList.Show();
                }
            }
        }
    }

    private void BindBusinessRulesAddEditRules()
    {
        DataSet dsRules = Package.GetPackageBusinessRules(Session["svSelectedPackage"].ToString());
        ddlBusinessRuleAddEditRules.Items.Clear();
        ddlBusinessRuleAddEditRules.DataSource = dsRules;
        ddlBusinessRuleAddEditRules.DataValueField = "BR_GUID";
        ddlBusinessRuleAddEditRules.DataTextField = "BR_Name";
        ddlBusinessRuleAddEditRules.DataBind();
        if (dsRules.Tables[0].Rows.Count == 0)
        {
            ddlBusinessRuleAddEditRules.Items.Insert(0, new ListItem("-- No rules found --", "None"));
        }
        else
        {
            ddlBusinessRuleAddEditRules.Items.Insert(0, new ListItem("-- Select a Business Rule from below", "None"));
        }
        ddlBusinessRuleAddEditRules.SelectedIndex = 0;
    }

    protected void GridStepsForFlow_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //Move to the new page
        GridStepsForFlow.PageIndex = e.NewPageIndex;
        GridStepsForFlow.SelectedIndex = -1;
        BindStepsListGrid();
        UpdatePanelUCFlows.Update();
    }

    protected void GridStepsForFlow_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string group = GridStepsForFlow.DataKeys[e.Row.RowIndex].Values["ActorSystemDoes"].ToString();
            Label lblGridStepsActorDoes = (Label)e.Row.FindControl("lblGridStepsActorDoes");
            Label lblGridStepsSystemDoes = (Label)e.Row.FindControl("lblGridStepsSystemDoes");

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ActorSystemDoes));
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(group));
            ActorSystemDoes result = serializer.ReadObject(ms) as ActorSystemDoes;
            lblGridStepsActorDoes.Text = result.ActorDoes;
            lblGridStepsSystemDoes.Text = result.SystemDoes;

            // Link buttons to do async postback
            LinkButton lnkGridStepsEditStep = e.Row.FindControl("lnkGridStepsEditStep") as LinkButton;
            LinkButton lnkGridStepsAddStep = e.Row.FindControl("lnkGridStepsAddStep") as LinkButton;
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(lnkGridStepsEditStep);
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(lnkGridStepsAddStep);
            LinkButton lnkGridStepsDeleteStep = e.Row.FindControl("lnkGridStepsDeleteStep") as LinkButton;
            LinkButton lnkGridStepsBusinessRule = e.Row.FindControl("lnkGridStepsBusinessRule") as LinkButton;
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(lnkGridStepsDeleteStep);
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(lnkGridStepsBusinessRule);

        }
    }

    protected void GridFlows_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //Move to the new page
        GridFlows.PageIndex = e.NewPageIndex;
        GridFlows.SelectedIndex = -1;
        BindFlowsGrid();
        UpdatePanelUCFlows.Update();
    }
    protected void GridFlows_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //Row command only if it's not a page index change event
        if (!e.CommandName.ToLower().Contains("page"))
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument) - GridFlows.PageSize * GridFlows.PageIndex;
            string eflowguid = GridFlows.DataKeys[rowIndex]["Flow_GUID"].ToString();
            // Code for link clicks on Steps grid.        
            if (e.CommandName == "EditFlow")
            {
                hfCurrentFlowID.Value = eflowguid;

                //Open popup to add new Step.
                trCreateUpdateFlowHidden.Visible = true;
                lblErrorCreateUpdateFlow.Text = "";
                BindFlowEntryCombo();
                ddlCreateUpdateFlowEntryPoint.SelectedValue = GridFlows.DataKeys[rowIndex]["EntryPoint_GUID"].ToString();
                txtCreateUpdateFlowName.Text = GridFlows.DataKeys[rowIndex]["FlowName"].ToString();
                chkCreateUpdateFlowHidden.Checked = ((GridFlows.DataKeys[rowIndex]["HiddenFlag"].ToString() == "Hide") ? false : true);
                ddlCreateUpdateFlowType.SelectedValue = GridFlows.DataKeys[rowIndex]["FlowType"].ToString();
                BindFlowJoinCombo();
                if (GridFlows.DataKeys[rowIndex]["FlowType"].ToString().Equals("Exception"))
                {
                    BindFlowJoinWhileException();
                }
                ddlCreateUpdateFlowJoin.SelectedValue = GridFlows.DataKeys[rowIndex]["JoinPoint_GUID"].ToString();

                btnCreateUpdateFlow.CommandName = "UpdateFlow";
                upCreateUpdateFlow.Update();
                mpCreateUpdateFlow.Show();
            }
            else if (e.CommandName == "HideUnhideFlow")
            {
                UseCase.HideUnhideUCFlow(eflowguid, ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);
                BindFlowsGrid();
                UpdatePanelUCFlows.Update();
            }
        }
    }
    protected void GridFlows_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkBtnHideUnHide = e.Row.FindControl("lnkGridFlowsHideUnHideFlow") as LinkButton;
            LinkButton lnkBtnEditFlow = e.Row.FindControl("lnkGridFlowsEdit") as LinkButton;
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(lnkBtnHideUnHide);
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(lnkBtnEditFlow);
        }
    }
    #endregion

    #region "Support functtions"
    private string ConvertToJson(string actorDoesNotes, string systemDoesNotes)
    {
        string ret = "";
        ActorSystemDoes obj = new ActorSystemDoes()
        {
            ActorDoes = actorDoesNotes,
            SystemDoes = systemDoesNotes
        };
        //var json = new JavaScriptSerializer().Serialize(obj);
        //return json.ToString();
        DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(ActorSystemDoes));
        MemoryStream ms = new MemoryStream();
        js.WriteObject(ms, obj);

        ms.Position = 0;
        StreamReader sr = new StreamReader(ms);
        ret = sr.ReadToEnd();
        sr.Close();
        ms.Close();
        return ret;
    }
    private void BindStepsListGrid()
    {
        DataSet ds = UseCase.GetUCStepsList(ddlFlows.SelectedValue.ToString());
        GridStepsForFlow.DataSource = ds;
        GridStepsForFlow.DataBind();
        hfTotalStepsCount.Value = ds.Tables[0].Rows.Count.ToString();
    }

    private void BindFlowsGrid()
    {
        GridFlows.DataSource = UseCase.GetUCFlowsList(Session["svSelectedUseCase"].ToString());
        GridFlows.DataBind();
    }
    private void BindGridBusinessRules(string eguid)
    {
        GridBusinessRules.DataSource = UseCase.GetUCStepBusinessRulesList(eguid);
        GridBusinessRules.DataBind();
    }
    private void BindFlowsDropDown()
    {
        ddlFlows.Items.Clear();
        ddlFlows.DataSource = UseCase.GetUCFlowsCombo(Session["svSelectedUseCase"].ToString());
        ddlFlows.DataValueField = "Flow_GUID";
        ddlFlows.DataTextField = "Flow_Name";
        ddlFlows.DataBind();
        ddlFlows.SelectedIndex = 0;
    }

    /// <summary>
    /// Private Class to Stringify the Json values of ActorSystemDoes.
    /// </summary>
    [DataContract]
    private class ActorSystemDoes
    {
        [DataMember(Name = "Actor Does")]
        public string ActorDoes { get; set; }

        [DataMember(Name = "System Does")]
        public string SystemDoes { get; set; }
    }

    //Steps to prepare for creating 
    private void PrepareForAddNewFlow()
    {
        trCreateUpdateFlowHidden.Visible = false;
        txtCreateUpdateFlowName.Text = "";
        chkCreateUpdateFlowHidden.Checked = false;
        BindFlowEntryCombo();
        BindFlowJoinCombo();
        ddlCreateUpdateFlowType.SelectedIndex = 0;
        btnCreateUpdateFlow.CommandName = "CreateFlow";
        lblErrorCreateUpdateFlow.Text = string.Empty;
        upCreateUpdateFlow.Update();
        mpCreateUpdateFlow.Show();
    }
    private void BindFlowEntryCombo()
    {
        ddlCreateUpdateFlowEntryPoint.Items.Clear();
        ddlCreateUpdateFlowEntryPoint.DataSource = UseCase.GetUCFlowEntryCombo(ddlFlows.SelectedValue);
        ddlCreateUpdateFlowEntryPoint.DataValueField = "EntryStep_GUID";
        ddlCreateUpdateFlowEntryPoint.DataTextField = "EntryPoint";
        ddlCreateUpdateFlowEntryPoint.DataBind();
        ddlCreateUpdateFlowEntryPoint.SelectedIndex = 0;
    }
    private void BindFlowJoinCombo()
    {
        ddlCreateUpdateFlowJoin.Items.Clear();
        ddlCreateUpdateFlowJoin.DataSource = UseCase.GetUCFlowJoinCombo(ddlFlows.SelectedValue);
        ddlCreateUpdateFlowJoin.DataValueField = "JoinStep_GUID";
        ddlCreateUpdateFlowJoin.DataTextField = "EntryPoint";
        ddlCreateUpdateFlowJoin.DataBind();
        ddlCreateUpdateFlowJoin.Items.Add(new ListItem("End", "End"));
    }
    private void BindFlowJoinWhileException()
    {
        ddlCreateUpdateFlowJoin.Items.Clear();
        ddlCreateUpdateFlowJoin.Items.Add(new ListItem("End", "End"));
    }
    #endregion

    #region "Business Rules Supporting code"
    
    protected void btnBusinessRuleListAddRule_Click(object sender, EventArgs e)
    {
        //Clear and reload AddUpdate Business rule popup.
        //Bind the Step DEtails on the popup
        DataTable dtStepDetails = UseCase.GetUCStepDetailView(hfRuleListCurStepId.Value).Tables[0];
        string actorSystem = dtStepDetails.Rows[0]["ActorSystem"].ToString().Trim();
        string actorSystemDoesJson = actorSystem;
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ActorSystemDoes));
        MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(actorSystemDoesJson));
        ActorSystemDoes result = serializer.ReadObject(ms) as ActorSystemDoes;

        lblBusinessRuleAddEditStep.Text = dtStepDetails.Rows[0]["StepNumber"].ToString().Trim();
        lblBusinessRuleAddEditActorDoes.Text = result.ActorDoes;
        lblBusinessRuleAddEditSystemDoes.Text = result.SystemDoes;

        lblBusinessRuleAddEditFlow.Text = ddlFlows.SelectedItem.Text.ToString();
        BindBusinessRulesAddEditRules();
        txtBusinessRuleAddEditRuleName.Text = string.Empty;
        htmlEditorBusinessRuleAddEditNote.Text = string.Empty;
        btnBusinessRuleAddEditAddRule.CommandName = "AddBusinessRule";
        lblBusinessRuleAddEditError.Text = string.Empty;
        tdBusinessruleEdit.Visible = false;
        trBusinessruleAdd.Visible = true;
        upBusinessRuleAddEdit.Update();
        mpBusinessRuleAddEdit.Show();
        mpBusinessRuleList.Hide();
    }
    protected void btnBusinessRuleListClose_Click(object sender, EventArgs e)
    {
        BindStepsListGrid();
        UpdatePanelUCFlows.Update();
        mpBusinessRuleList.Hide();
    }
    protected void GridBusinessRules_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //Row command only if it's not a page index change event
        if (!e.CommandName.ToLower().Contains("page"))
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument) - GridBusinessRules.PageSize * GridBusinessRules.PageIndex;
            int brObjPKId = Convert.ToInt32(GridBusinessRules.DataKeys[rowIndex]["BR_ObjectPK"].ToString());
            // Code for deleting business rule       
            if (e.CommandName == "DeleteBusinessRule")
            {
                UseCase.DeleteUCStepBusinessRule(brObjPKId, ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);

                BindGridBusinessRules(hfRuleListCurStepId.Value);
                upBusinessRuleList.Update();
                mpBusinessRuleList.Show();
            }
            else if (e.CommandName == "EditBusinessRule")
            {
                DataTable dtStepDetails = UseCase.GetUCStepDetailView(hfRuleListCurStepId.Value).Tables[0];
                string actorSystem = dtStepDetails.Rows[0]["ActorSystem"].ToString().Trim();
                string actorSystemDoesJson = actorSystem;
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ActorSystemDoes));
                MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(actorSystemDoesJson));
                ActorSystemDoes result = serializer.ReadObject(ms) as ActorSystemDoes;

                lblBusinessRuleAddEditStep.Text = dtStepDetails.Rows[0]["StepNumber"].ToString().Trim();
                lblBusinessRuleAddEditActorDoes.Text = result.ActorDoes;
                lblBusinessRuleAddEditSystemDoes.Text = result.SystemDoes;

                lblBusinessRuleAddEditFlow.Text = ddlFlows.SelectedItem.Text.ToString();
                lblBusinessRulesAddEditRuleId.Text = GridBusinessRules.DataKeys[rowIndex]["UV_ID"].ToString();
                txtBusinessRuleAddEditRuleName.Text = GridBusinessRules.DataKeys[rowIndex]["BR_ShortName"].ToString();
                htmlEditorBusinessRuleAddEditNote.Text = GridBusinessRules.DataKeys[rowIndex]["BR_Note"].ToString();
                
                //Set the Br Guid for updating
                hfBusinessRuleAddEditBrGUID.Value = GridBusinessRules.DataKeys[rowIndex]["BR_GUID"].ToString();

                btnBusinessRuleAddEditAddRule.CommandName = "UpdateBusinessRule";
                lblBusinessRuleAddEditError.Text = string.Empty;
                tdBusinessruleEdit.Visible = true;
                trBusinessruleAdd.Visible = false;
                mpBusinessRuleList.Hide();
                upBusinessRuleAddEdit.Update();
                mpBusinessRuleAddEdit.Show();
            }
        }

    }
    protected void GridBusinessRules_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Do this so that the Links in the grid wont do a full post back- eliminates the refresh resubmit issue
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbDelete = e.Row.FindControl("lnkGridBusinessRuleDeleteRule") as LinkButton;
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(lbDelete);
            LinkButton lbEditRule = e.Row.FindControl("lnkGridBusinessRuleEditRule") as LinkButton;
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(lbEditRule);
        }
    }
    protected void GridBusinessRules_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridBusinessRules.PageIndex = e.NewPageIndex;
        GridBusinessRules.SelectedIndex = -1;
        BindGridBusinessRules(hfRuleListCurStepId.Value);
        upBusinessRuleList.Update();
    }
    protected void ddlBusinessRuleAddEditRules_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBusinessRuleAddEditRules.SelectedIndex == 0)
        {
            txtBusinessRuleAddEditRuleName.Text = string.Empty;
            htmlEditorBusinessRuleAddEditNote.Text = string.Empty;
        }
        else
        {
            DataTable dtBRDetails = UseCase.GetBusinessRuleDetailView(ddlBusinessRuleAddEditRules.SelectedValue).Tables[0];
            if (dtBRDetails.Rows.Count == 0)
            {
                lblBusinessRuleAddEditError.Text = "No data found for Details";
                txtBusinessRuleAddEditRuleName.Text = string.Empty;
                htmlEditorBusinessRuleAddEditNote.Text = string.Empty;
            }
            else
            {
                txtBusinessRuleAddEditRuleName.Text = dtBRDetails.Rows[0]["Br_Name"].ToString().Trim();
                htmlEditorBusinessRuleAddEditNote.Text = dtBRDetails.Rows[0]["BR_Note"].ToString().Trim();
            }
        }
    }
    protected void btnBusinessRuleAddEditClose_Click(object sender, EventArgs e)
    {
        mpBusinessRuleAddEdit.Hide();
    }
    protected void btnBusinessRuleAddEditAddRule_Click(object sender, EventArgs e)
    {
        try
        {
            Button btn = (Button)sender;
            switch (btn.CommandName)
            {
                case "AddBusinessRule":
                    //Create requirement. and rebind the grid to update the created one.
                    try
                    {
                        if (hfRuleListCurStepId.Value == "" || Session["svSelectedPackage"] == null) throw new Exception("Cannot find the key, Please reload the page");
                        UseCase.AddUCStepBusinessRule(Session["svSelectedPackage"].ToString(), hfRuleListCurStepId.Value, txtBusinessRuleAddEditRuleName.Text,
                                                        htmlEditorBusinessRuleAddEditNote.Text.Trim(), ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);
                        BindStepsListGrid();
                        UpdatePanelUCFlows.Update();
                        mpBusinessRuleAddEdit.Hide();                        
                    }
                    catch (Exception Ex)
                    {
                        lblBusinessRuleAddEditError.Text = "Error While Adding Business rule: " + Ex.Message.ToString();
                        mpBusinessRuleAddEdit.Show();
                        upBusinessRuleAddEdit.Update();
                    }
                    break;
                case "UpdateBusinessRule":                    
                    try
                    {
                        if (hfBusinessRuleAddEditBrGUID.Value == "") throw new Exception("Cannot find the key, Please reload the page");
                        //Update Business rule.
                        UseCase.UpdateUCStepBusinessRule(hfBusinessRuleAddEditBrGUID.Value, txtBusinessRuleAddEditRuleName.Text,
                                                       htmlEditorBusinessRuleAddEditNote.Text.Trim(), ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);
                        // Hide the PopUp and update the grid.
                        BindStepsListGrid();
                        UpdatePanelUCFlows.Update();
                        mpBusinessRuleAddEdit.Hide(); 
                    }
                    catch (Exception ex)
                    {
                        lblBusinessRuleAddEditError.Text = "Error While Updating Busines Rule: " + ex.Message.ToString();
                        mpBusinessRuleAddEdit.Show();
                        upBusinessRuleAddEdit.Update();
                    }
                    break;
            }
            lblBusinessRuleAddEditError.Text = "";
        }
        catch (Exception Ex)
        {
            lblBusinessRuleAddEditError.Text = "Error while Add/Update business rule: " + Ex.Message.ToString();
            mpBusinessRuleAddEdit.Show();
            upBusinessRuleAddEdit.Update();
        }
    }
    #endregion

#region "Error Messages Supporting code"
    
    protected void btnErrorMessageListAddMessage_Click(object sender, EventArgs e)
    {
        //Clear and reload AddUpdate Error Message popup.
        //Bind the Step DEtails on the popup
        DataTable dtStepDetails = UseCase.GetUCStepDetailView(hfCurentStepId.Value).Tables[0];
        string actorSystem = dtStepDetails.Rows[0]["ActorSystem"].ToString().Trim();
        string actorSystemDoesJson = actorSystem;
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ActorSystemDoes));
        MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(actorSystemDoesJson));
        ActorSystemDoes result = serializer.ReadObject(ms) as ActorSystemDoes;

        lblErrorMessageAddEditStep.Text = dtStepDetails.Rows[0]["StepNumber"].ToString().Trim();
        lblErrorMessageAddEditActorDoes.Text = result.ActorDoes;
        lblErrorMessageAddEditSystemDoes.Text = result.SystemDoes;

        lblErrorMessageAddEditFlow.Text = ddlFlows.SelectedItem.Text.ToString();
        BindErrorMessagesAddEditMessages();
        txtErrorMessageAddEditMessageName.Text = string.Empty;
        htmlEditorErrorMessageAddEditNote.Text = string.Empty;
        btnErrorMessageAddEditAddMessage.CommandName = "AddErrorMessage";
        lblErrorMessageAddEditError.Text = string.Empty;
        tdErrorMessageEdit.Visible = false;
        trErrorMessageAdd.Visible = true;
        upErrorMessageAddEdit.Update();
        mpErrorMessageAddEdit.Show();
        mpErrorMessageList.Hide();
    }
    protected void btnErrorMessageListClose_Click(object sender, EventArgs e)
    {
        BindStepsListGrid();
        UpdatePanelUCFlows.Update();
        mpErrorMessageList.Hide();
    }
    protected void GridErrorMessages_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //Row command only if it's not a page index change event
        if (!e.CommandName.ToLower().Contains("page"))
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument) - GridErrorMessages.PageSize * GridErrorMessages.PageIndex;
            int emObjPKId = Convert.ToInt32(GridErrorMessages.DataKeys[rowIndex]["EM_ObjectPK"].ToString());
            // Code for deleting Error Message       
            if (e.CommandName == "DeleteErrorMessage")
            {
                UseCase.DeleteUCStepErrorMessage(emObjPKId, ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);

                BindGridErrorMessages(hfCurentStepId.Value);
                upErrorMessageList.Update();
                mpErrorMessageList.Show();
            }
            else if (e.CommandName == "EditErrorMessage")
            {
                DataTable dtStepDetails = UseCase.GetUCStepDetailView(hfCurentStepId.Value).Tables[0];
                string actorSystem = dtStepDetails.Rows[0]["ActorSystem"].ToString().Trim();
                string actorSystemDoesJson = actorSystem;
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ActorSystemDoes));
                MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(actorSystemDoesJson));
                ActorSystemDoes result = serializer.ReadObject(ms) as ActorSystemDoes;

                lblErrorMessageAddEditStep.Text = dtStepDetails.Rows[0]["StepNumber"].ToString().Trim();
                lblErrorMessageAddEditActorDoes.Text = result.ActorDoes;
                lblErrorMessageAddEditSystemDoes.Text = result.SystemDoes;

                lblErrorMessageAddEditFlow.Text = ddlFlows.SelectedItem.Text.ToString();
                lblErrorMessagesAddEditMessageId.Text = GridErrorMessages.DataKeys[rowIndex]["UV_ID"].ToString();
                txtErrorMessageAddEditMessageName.Text = GridErrorMessages.DataKeys[rowIndex]["EM_ShortName"].ToString();
                htmlEditorErrorMessageAddEditNote.Text = GridErrorMessages.DataKeys[rowIndex]["EM_Note"].ToString();
                
                //Set the em Guid for updating
                hfErrorMessageAddEditEMGUID.Value = GridErrorMessages.DataKeys[rowIndex]["EM_GUID"].ToString();

                btnErrorMessageAddEditAddMessage.CommandName = "UpdateErrorMessage";
                lblErrorMessageAddEditError.Text = string.Empty;
                tdErrorMessageEdit.Visible = true;
                trErrorMessageAdd.Visible = false;
                mpErrorMessageList.Hide();
                upErrorMessageAddEdit.Update();
                mpErrorMessageAddEdit.Show();
            }
        }

    }
    protected void GridErrorMessages_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Do this so that the Links in the grid wont do a full post back- eliminates the refresh resubmit issue
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbDelete = e.Row.FindControl("lnkGridErrorMessageDeleteMessage") as LinkButton;
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(lbDelete);
            LinkButton lbEditMessage = e.Row.FindControl("lnkGridErrorMessageEditMessage") as LinkButton;
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(lbEditMessage);
        }
    }
    protected void GridErrorMessages_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridErrorMessages.PageIndex = e.NewPageIndex;
        GridErrorMessages.SelectedIndex = -1;
        BindGridErrorMessages(hfCurentStepId.Value);
        upErrorMessageList.Update();
    }
    protected void ddlErrorMessageAddEditMessages_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlErrorMessageAddEditMessages.SelectedIndex == 0)
        {
            txtErrorMessageAddEditMessageName.Text = string.Empty;
            htmlEditorErrorMessageAddEditNote.Text = string.Empty;
        }
        else
        {
            DataTable dtEMDetails = UseCase.GetErrorMessageDetailView(ddlErrorMessageAddEditMessages.SelectedValue).Tables[0];
            if (dtEMDetails.Rows.Count == 0)
            {
                lblErrorMessageAddEditError.Text = "No data found for Details";
                txtErrorMessageAddEditMessageName.Text = string.Empty;
                htmlEditorErrorMessageAddEditNote.Text = string.Empty;
            }
            else
            {
                txtErrorMessageAddEditMessageName.Text = dtEMDetails.Rows[0]["EM_Name"].ToString().Trim();
                htmlEditorErrorMessageAddEditNote.Text = dtEMDetails.Rows[0]["EM_Note"].ToString().Trim();
            }
        }
    }
    protected void btnErrorMessageAddEditClose_Click(object sender, EventArgs e)
    {
        mpErrorMessageAddEdit.Hide();
    }
    protected void btnErrorMessageAddEditAddMessage_Click(object sender, EventArgs e)
    {
        try
        {
            Button btn = (Button)sender;
            switch (btn.CommandName)
            {
                case "AddErrorMessage":
                    //Create requirement. and rebind the grid to update the created one.
                    try
                    {
                        if (hfCurentStepId.Value == "" || Session["svSelectedPackage"] == null) throw new Exception("Cannot find the key, Please reload the page");
                        UseCase.AddUCStepErrorMessage(Session["svSelectedPackage"].ToString(), hfCurentStepId.Value, txtErrorMessageAddEditMessageName.Text,
                                                        htmlEditorErrorMessageAddEditNote.Text.Trim(), ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);
                        BindStepsListGrid();
                        UpdatePanelUCFlows.Update();
                        mpErrorMessageAddEdit.Hide();                        
                    }
                    catch (Exception Ex)
                    {
                        lblErrorMessageAddEditError.Text = "Error While Adding Error Message: " + Ex.Message.ToString();
                        mpErrorMessageAddEdit.Show();
                        upErrorMessageAddEdit.Update();
                    }
                    break;
                case "UpdateErrorMessage":                    
                    try
                    {
                        if (hfErrorMessageAddEditEMGUID.Value == "") throw new Exception("Cannot find the key, Please reload the page");
                        //Update Error Message.
                        UseCase.UpdateUCStepErrorMessage(hfErrorMessageAddEditEMGUID.Value, txtErrorMessageAddEditMessageName.Text,
                                                       htmlEditorErrorMessageAddEditNote.Text.Trim(), ((CurrentUser)CurrentUser.GetUserDetails()).User_GUID);
                        // Hide the PopUp and update the grid.
                        BindStepsListGrid();
                        UpdatePanelUCFlows.Update();
                        mpErrorMessageAddEdit.Hide(); 
                    }
                    catch (Exception ex)
                    {
                        lblErrorMessageAddEditError.Text = "Error While Updating Busines Message: " + ex.Message.ToString();
                        mpErrorMessageAddEdit.Show();
                        upErrorMessageAddEdit.Update();
                    }
                    break;
            }
            lblErrorMessageAddEditError.Text = "";
        }
        catch (Exception Ex)
        {
            lblErrorMessageAddEditError.Text = "Error while Add/Update Error Message: " + Ex.Message.ToString();
            mpErrorMessageAddEdit.Show();
            upErrorMessageAddEdit.Update();
        }
    }
    private void BindErrorMessagesAddEditMessages()
    {
        DataSet dsMessages = Package.GetPackageErrorMessages(Session["svSelectedPackage"].ToString());
        ddlErrorMessageAddEditMessages.Items.Clear();
        ddlErrorMessageAddEditMessages.DataSource = dsMessages;
        ddlErrorMessageAddEditMessages.DataValueField = "EM_GUID";
        ddlErrorMessageAddEditMessages.DataTextField = "EM_Name";
        ddlErrorMessageAddEditMessages.DataBind();
        if (dsMessages.Tables[0].Rows.Count == 0)
        {
            ddlErrorMessageAddEditMessages.Items.Insert(0, new ListItem("-- No Messages found --", "None"));
        }
        else
        {
            ddlErrorMessageAddEditMessages.Items.Insert(0, new ListItem("-- Select a Business Message from below", "None"));
        }
        ddlErrorMessageAddEditMessages.SelectedIndex = 0;
    }
    private void BindGridErrorMessages(string eguid)
    {
        GridErrorMessages.DataSource = UseCase.GetUCStepErrorMessagesList(eguid);
        GridErrorMessages.DataBind();
    }
    #endregion
}
