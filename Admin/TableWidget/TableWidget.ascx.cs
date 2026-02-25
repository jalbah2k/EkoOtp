using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuteEditor;

public partial class TableWidget : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindTables();
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (pnlEditGrp.Visible && Table != 0)
        {
            if (rbHeaders.SelectedValue == "1")
            {
                rbHeaders.Items[0].Attributes.Add("style", "opacity:0.7");
                rbHeaders.Items[1].Attributes.Add("style", "opacity:0");
            }
            else if (rbHeaders.SelectedValue == "0")
            {
                rbHeaders.Items[0].Attributes.Add("style", "opacity:0");
                rbHeaders.Items[1].Attributes.Add("style", "opacity:0.7");
            }

        }
    }

    #region Properties
    public int Table
    {
        set { ViewState["Table"] = value; }
        get
        {
            if (ViewState["Table"] == null)
                return 0;
            else
                return Convert.ToInt32(ViewState["Table"]);
        }
    }
    int Column
    {
        set { ViewState["Column"] = value; }
        get { return Convert.ToInt32(ViewState["Column"]); }
    }
    //int Row
    //{
    //    set { ViewState["Row"] = value; }
    //    get { return Convert.ToInt32(ViewState["Row"]); }
    //}

    bool Inserting
    {
        set { ViewState["Inserting"] = value; }
        get
        {
            if (ViewState["Inserting"] == null)
                return false;
            else
                return Convert.ToBoolean(ViewState["Inserting"]);
        }
    }
    bool SingleHeader
    {
        set { ViewState["singleheader"] = value; }
        get
        {
            if (ViewState["singleheader"] == null)
                return false;
            else
                return Convert.ToBoolean(ViewState["singleheader"]);
        }
    }

    #endregion

    #region Tables
    private void BindTables()
    {
        DataSet ds = new DataSet();
        string sqlstr = @" select t.*, isnull(c.qty, 0) as qty from TableWidget.Tables t
                                left outer join (select count(id) qty, TableId from TableWidget.Columns group by TableId) c on t.id = c.TableId order by t.name";

        SqlCommand sq = new SqlCommand(sqlstr);
        ds = getDataset(sq, true);


        this.GV_Main.DataSource = ds.Tables[0];
        this.GV_Main.DataBind();
    }


    protected void GV_Main_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((DataRowView)e.Row.DataItem != null)
        {
            ImageButton LB_Delete = (ImageButton)e.Row.FindControl("btnDelete");
            ImageButton btnItems = (ImageButton)e.Row.FindControl("btnItems");
            DataRowView dr = (DataRowView)e.Row.DataItem;
            if (dr["qty"].ToString() == "0")
            {
                LB_Delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this table?');");

                btnItems.Enabled = false;
                btnItems.Attributes.Add("style", "opacity:0.3");
            }
            else
            {
                //LB_Delete.Enabled = false;
                //LB_Delete.Attributes.Add("style", "opacity:0.3");

                LB_Delete.Attributes.Add("onclick", "return confirm('This table has columns that might contain data. Are you sure you want to delete this table?');");

            }

        }
    }

    protected void GV_Main_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditGroup")
        {
            txtNameGrp.Text = "";

            SwitchView(pnlEditGrp);
            Table = Convert.ToInt32(e.CommandArgument.ToString());

            DataTable dt = new DataTable();
            string sqlstr = @" select * from TableWidget.Tables where id=@id";
            SqlCommand sq = new SqlCommand(sqlstr);
            sq.Parameters.AddWithValue("@id", Table);

            dt = getTable(sq, true);

            if (dt.Rows.Count > 0)
            {
                txtNameGrp.Text = dt.Rows[0]["name"].ToString();
                txtTitle.Text = dt.Rows[0]["title"].ToString();
            }

            BindColumns(Table);
            divColumnName.Visible = true;
            txtColumnName.Text = "";

            rbHeaders.SelectedValue = Convert.ToBoolean(dt.Rows[0]["SingleHeader"]) ? "1" : "0";
            rbHeaders.Enabled = false;
            tdRbHeaders.Attributes.Add("style", "opacity:0.7");


            btnSaveColumn.Visible = false;
            btnCancelColumn.Visible = false;
            btnAddColumn.Visible = true;
            
        }
        else if (e.CommandName == "EditRows")
        {
            Table = Convert.ToInt32(e.CommandArgument.ToString());

            ShowRows(Table);
        }
        else if (e.CommandName == "DeleteGroup")
        {
            string sqlstr = @" delete from TableWidget.Tables where id=@id delete from TableWidget.Columns where tableid=@id delete from content where control='TableWidget' and param=@id ";
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@id", e.CommandArgument.ToString()));

            ProcessRecord(sqlstr, parms, CommandType.Text);
            BindTables();
        }

    }

    protected void btnAddGallery_Click(object sender, EventArgs e)
    {
        Table = 0;
        divColumnName.Visible = false;
        ClearFields();
        SwitchView(pnlEditGrp);
        rbHeaders.ClearSelection();
        btnAddColumn.Visible = true;

        rbHeaders.Enabled = true;
        tdRbHeaders.Attributes.Add("style", "opacity:1");

        rbHeaders.Items[0].Attributes.Add("style", "opacity:1");
        rbHeaders.Items[1].Attributes.Add("style", "opacity:1");


        BindColumns(0);
    }

    protected void btnCancelGrp_Click(object sender, EventArgs e)
    {
        SwitchView(pnlGroups);
        BindTables();
    }
 
    protected void btnSaveGrp_Click(object sender, EventArgs e)
    {
        List<SqlParameter> parms = new List<SqlParameter>();
        string sql = "";

        if (Table == 0)
        {
            sql = @"declare @id int  
                    insert into TableWidget.Tables (name, title, SingleHeader) values(@name, @title, @SingleHeader); 
                    select @id = SCOPE_IDENTITY(); 
                    insert into content ([name],[control],[param],[language]) values ('Table -' + @name, 'TableWidget', @id, 1) 
                    if @SingleHeader = 0
                        insert into TableWidget.Columns (TableId, Caption, priority) values(@id, '', 0)
                    select @id";

            parms.Add(new SqlParameter("@SingleHeader", rbHeaders.SelectedValue));
        }
        else
        {
            //Update
            sql = "update TableWidget.Tables set name=@name, title=@title where id=@id; update content set name = 'Table -' + @name where control='TableWidget' and param=@id; select @id";
            parms.Add(new SqlParameter("@id", Table));
        }
        parms.Add(new SqlParameter("@name", txtNameGrp.Text));
        parms.Add(new SqlParameter("@title", txtTitle.Text));

        string id = ExecuteScalar(sql, parms, CommandType.Text).ToString();

        if (Table == 0)
        {
            rbHeaders.Enabled = false;
            tdRbHeaders.Attributes.Add("style", "opacity:0.7");

            Table = int.Parse(id);
            divColumnName.Visible = true;
            return;
        }

        ClearFields();
        SwitchView(pnlGroups);
        BindTables();
    }    
    #endregion

    #region Columns
    private void BindColumns(int gallery)
    {
        Table = gallery;
        BindColumns();

    }

    private void BindColumns()
    {
        DataSet ds = new DataSet();
        string sqlstr = @" select t.*, isnull(c.qty, 0) as qty from TableWidget.Columns t  
                                left outer join(select count(id) qty, ColumnId from TableWidget.Cells group by ColumnId) c on t.id = c.ColumnId
                                where t.TableId=@TableId and t.priority <> 0
								order by t.priority";

        SqlCommand sq = new SqlCommand(sqlstr);
        sq.Parameters.AddWithValue("@TableId", Table);
        ds = getDataset(sq, true);


        this.GV_Columns.DataSource = ds.Tables[0];
        this.GV_Columns.DataBind();

    }

    protected void rbHeaders_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindColumns();
    }

    protected void GV_Columns_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((DataRowView)e.Row.DataItem != null)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;

            ImageButton LB_Delete = (ImageButton)e.Row.FindControl("btnDelete");
            if (dr["qty"].ToString() == "0")
            {
                LB_Delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this column?');");
            }
            else
            {
                //LB_Delete.Enabled = false;
                //LB_Delete.Attributes.Add("style", "opacity:0.3");
                LB_Delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this column and its data?');");

            }


            Literal litHfId = (Literal)e.Row.FindControl("litHfId");
            litHfId.Text = String.Format("<input type='hidden' class='HfId' value='{0}' />", dr["id"].ToString());
        }
    }

    public void EditColumn(object sender, ImageClickEventArgs e)
    {

        Column = int.Parse(((ImageButton)sender).AlternateText);

        ResetBackground(GV_Columns);

        GridViewRow myrow = (GridViewRow)((ImageButton)sender).Parent.Parent;
        //for (int i = 0; i < myrow.Cells.Count; i++)
            myrow.Cells[1].BackColor = System.Drawing.Color.FromArgb(242, 247, 251);

        txtColumnName.Text = myrow.Cells[1].Text;

        btnAddColumn.Visible = false;
        btnSaveColumn.Visible = true;
        btnCancelColumn.Visible = true;

    }

    protected void GV_Columns_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DelColumn")
        {
            Column = Convert.ToInt32(e.CommandArgument.ToString());

            string sqlstr = @" delete from TableWidget.Columns where id=@id ";
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@id", Column));

            ProcessRecord(sqlstr, parms, CommandType.Text);
            BindColumns(Table);

            txtColumnName.Text = "";
            btnCancelColumn.Visible = false;
            btnSaveColumn.Visible = false;
            btnAddColumn.Visible = true;
        }
    }

    protected void btnAddColumn_Click(object sender, EventArgs e)
    {
        List<SqlParameter> parms = new List<SqlParameter>();
        string sql = @"insert into TableWidget.Columns (TableId, Caption, priority) values(@tableid, @caption, isnull((select max(priority) from TableWidget.Columns where tableid=@tableid group by TableId), 0) + 1 ) select SCOPE_IDENTITY()";

        parms.Add(new SqlParameter("@tableid",Table));
        parms.Add(new SqlParameter("@caption", txtColumnName.Text));

        string id = ExecuteScalar(sql, parms, CommandType.Text).ToString();
        BindColumns(Table);
        txtColumnName.Text = "";

    }

    protected void btnSaveColumn_Click(object sender, EventArgs e)
    {
        List<SqlParameter> parms = new List<SqlParameter>();
        string sql = @"update TableWidget.Columns set Caption=@caption where id=@id; select @id";

        parms.Add(new SqlParameter("@id", Column));
        parms.Add(new SqlParameter("@caption", txtColumnName.Text));

        string id = ExecuteScalar(sql, parms, CommandType.Text).ToString();
        BindColumns(Table);
        txtColumnName.Text = "";

        btnAddColumn.Visible = true;
        btnSaveColumn.Visible = false;
        btnCancelColumn.Visible = false;

        ResetBackground(GV_Columns);
    }



    protected void btnCancelColumn_Click(object sender, EventArgs e)
    {
        btnAddColumn.Visible = true;
        btnSaveColumn.Visible = false;
        btnCancelColumn.Visible = false;
        txtColumnName.Text = "";
        ResetBackground(GV_Columns);
        BindColumns();

    }
    #endregion

    #region Rows
    protected void ShowRows(int table)
    {
        Table = table;
        Inserting = false;

        if (BuildGridView(Table))
        {
            BindRows(Table);
            SwitchView(pnlRows);
            btAddRow.Visible = true;
        }

    }

    private bool BuildGridView(int table)
    {
        Table = table;

        //DataControlField colAction = GV_Rows.Columns[GV_Rows.Columns.Count - 1];
        GV_Rows.Columns.Clear();

        DataTable dt = new DataTable();
        string sqlstr = @" select c.*, t.SingleHeader from TableWidget.Columns c inner join TableWidget.Tables t on c.TableId=t.id where TableId=@id ";
        SqlCommand sq = new SqlCommand(sqlstr);
        sq.Parameters.AddWithValue("@id", Table);

        dt = getTable(sq, true);

        if (dt.Rows.Count > 0)
        {
            TemplateField field1 = new TemplateField();
            DataControlField col1;
            col1 = field1;
            col1.ItemStyle.CssClass = "itemrow";
            col1.HeaderStyle.BackColor = System.Drawing.Color.White;

            GV_Rows.Columns.Add(col1);

            SingleHeader = Convert.ToBoolean(dt.Rows[0]["SingleHeader"]);
            int j = 0;
            foreach (DataRow dr in dt.Rows)
            {
                TemplateField field = new TemplateField();
                DataControlField col;
                col = field;
                if (SingleHeader || j++ > 0)
                    col.HeaderText = dr["Caption"].ToString();
                else
                {
                    col.HeaderText = "";
                    col.HeaderStyle.BackColor = System.Drawing.Color.White;
                    col.ItemStyle.BackColor = System.Drawing.Color.FromArgb(67,67,67);
                }

                col.ItemStyle.CssClass = "itemrow row-itemrow";

                GV_Rows.Columns.Add(col);
            }

            TemplateField FieldAction = new TemplateField();
            DataControlField colAction;
            colAction = FieldAction;
            colAction.HeaderText = "Action";
            colAction.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            colAction.ItemStyle.CssClass = "itemrow";

            GV_Rows.Columns.Add(colAction);

            return true;
        }

        return false;
    }

    protected void BindRows(int table)
    {
        Table = table;
        BindRows();

    }
    private void BindRows(bool newrow = false)
    {
        DataSet ds = new DataSet();
        string sqlstr = @" select t.* from TableWidget.Rows t where t.TableId=@TableId order by t.RowNo; select * from TableWidget.Tables where id=@TableId";

        SqlCommand sq = new SqlCommand(sqlstr);
        sq.Parameters.AddWithValue("@TableId", Table);
        ds = getDataset(sq, true);

        GV_Rows.DataSource = ds.Tables[0];
        if (ds.Tables[0].Rows.Count == 0)
        {
            Inserting = true;
            hdfRow.Value = "0";
            btAddRow.Visible = false;
            newrow = true;
        }

        if (newrow)
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());

        GV_Rows.DataBind();

        if (ds.Tables[1].Rows.Count > 0)
        {
            litTableNameRows.Text = "Add/Edit  '" + ds.Tables[1].Rows[0]["name"].ToString() +  "' rows ";
            litTableTitleRows.Text = ds.Tables[1].Rows[0]["title"].ToString();
        }

        if (!newrow)
        {
            btnSaveRow.Attributes.Add("style", "display:none;");
            btnCancelRow.Attributes.Add("style", "display:none;");
        }
        else
        {
            btnSaveRow.Attributes.Add("style", "display:inline-block;");
            btnCancelRow.Attributes.Add("style", "display:inline-block;");

            if (!SingleHeader)
            {
                GV_Rows.Rows[GV_Rows.Rows.Count - 1].Cells[1].BackColor = System.Drawing.Color.FromArgb(67, 67, 67);
                GV_Rows.Rows[GV_Rows.Rows.Count - 1].Cells[1].CssClass = "new-row-header-cell";
            }

        }

        btnDelRow.Attributes.Add("style", "display:none;");
        btnDelRow.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this row?');");
    }

    protected void GV_Rows_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((DataRowView)e.Row.DataItem != null)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            GridViewRow myrow = e.Row;

            if (drv["id"].ToString() != "")
            {
                if (!Inserting)
                {
                    #region First column  
                    Literal mycontrol = new Literal();
                    mycontrol.ID = "litHfId";
                    mycontrol.Text = String.Format("<input type='hidden' class='HfId' value='{0}' />", drv["id"].ToString());
                    myrow.Cells[0].Controls.Add(mycontrol);

                    #endregion

                    #region Last column (Action)
                    Literal btnEdit = new Literal();
                    string img = @"<image src='/images/lemonaid/buttonsNew/pencil.png' 
                                    rowid='{0}' rownumber='{1}' id='editRow_{0}' 
                                    alt='button edit {0}' title='Edit Row' style='cursor:pointer;'
                                    onclick={2} />";
                    string action = "$('.row-itemrow').css('background-color','transparent');$('.txtbox_row').hide();$('.divrow').show();$('.divrow_{0}').hide();";
                    action += "$('.txtrow_{0}').show();$(this).parent().parent().children(\".row-itemrow\").css('background-color','#F2F7FB');$('.btnActionRow').show();";
                    action += "$('.headercell').parent().css('background-color','#434343!important');";
                    action += "$('#" + hdfRow.ClientID + "').val('{0}');";
                    action = String.Format(action, drv["id"].ToString());

                    btnEdit.Text = String.Format(img, drv["id"].ToString(), drv["rowno"].ToString(), action);
                    myrow.Cells[GV_Rows.Columns.Count - 1].Controls.Add(btnEdit);


                    Literal btnDelete = new Literal();
                    btnDelete.Text = String.Format("<image src='/images/lemonaid/buttonsNew/ex.png' rowid='{0}' rownumber='{1}' id='delRow_{0}' alt='button del {0}' title='Delete Row' style='cursor:pointer;margin-left:5px' onclick={2} />",
                        drv["id"].ToString(), drv["rowno"].ToString(), "$('#" + hdfRow.ClientID + "').val('" + drv["id"].ToString() + "');document.getElementById('" + btnDelRow.ClientID + "').click();");
                    myrow.Cells[GV_Rows.Columns.Count - 1].Controls.Add(btnDelete);
                    #endregion
                }

                #region Data columns
           
                DataSet ds = new DataSet();
               // string sqlstr = @"   select t.*, c.Caption from TableWidget.Cells t inner join [TableWidget].Columns c on t.ColumnId = c.Id where t.RowId=@rowid and c.TableId=@tableid order by c.Priority";
                string sqlstr = @"   select t.*, c.Caption from [TableWidget].Columns c left join (select * from TableWidget.Cells where RowId=@rowid) t on c.Id = t.ColumnId where c.TableId=@tableid order by c.Priority";

                SqlCommand sq = new SqlCommand(sqlstr);
                sq.Parameters.AddWithValue("@TableId", Table);
                sq.Parameters.AddWithValue("@rowid", drv["id"].ToString());
                ds = getDataset(sq, true);

                int i = 1;
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    Literal litValue = new Literal();
                    if(!SingleHeader && i == 1)
                        litValue.Text = String.Format("<div class='divrow_{0} divrow headercell'>{1}</div>", drv["id"].ToString(), dr["value"].ToString());
                    else
                        litValue.Text = String.Format("<div class='divrow_{0} divrow'>{1}</div>", drv["id"].ToString(), dr["value"].ToString());

                    myrow.Cells[i].Controls.Add(litValue);

                    Literal txtValue = new Literal();
                    if (!SingleHeader && i == 1)
                    {
                        txtValue.Text = String.Format("<input type='text' id='txtrow_{0}_{1}' name='txtrow_{0}_{1}' class='txtrow_{0} textbox txtbox_row' value='{2}' style='display:none;' placeholder='Enter Row Header' />",
                            drv["id"].ToString(), i, dr["value"].ToString());
                    }
                    else
                    {
                        txtValue.Text = String.Format("<input type='text' id='txtrow_{0}_{1}' name='txtrow_{0}_{1}' class='txtrow_{0} textbox txtbox_row' value='{2}' style='display:none;' />",
                            drv["id"].ToString(), i, dr["value"].ToString());

                    }

                    myrow.Cells[i++].Controls.Add(txtValue);
                }
                #endregion
            }
            else
            {
                for (int i = 1; i < GV_Rows.Columns.Count - 1; i++)
                {
                    Literal txtValue = new Literal();
                    if (!SingleHeader && i == 1)
                    {
                        txtValue.Text = String.Format("<input type='text' id='txtrow_{0}_{1}' name='txtrow_{0}_{1}' class='txtrow_{0} textbox' placeholder='Enter Row Header' />", 0, i);
                    }
                    else
                        txtValue.Text = String.Format("<input type='text' id='txtrow_{0}_{1}' name='txtrow_{0}_{1}' class='txtrow_{0} textbox' />", 0, i);

                    myrow.Cells[i].Controls.Add(txtValue);
                    myrow.Cells[i].BackColor = System.Drawing.Color.FromArgb(242, 247, 251);
                }
            }
        }
    }

    protected void btAddRow_Click(object sender, EventArgs e)
    {
        NewRow();
    }
    private void NewRow()
    {
        //Row = 0;
        Inserting = true;
        BuildGridView(Table);
        BindRows(true);
        hdfRow.Value = "0";
        btAddRow.Visible = false;
    }

    protected void btnCancelRow_Click(object sender, EventArgs e)
    {
        FinishRowAction();
    }

    protected void btnSaveRow_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        string sqlstr = @" SELECT id, ROW_NUMBER() OVER( ORDER BY [priority], id ) Num  from TableWidget.Columns  where tableid=@tableid order by Priority";

        SqlCommand sq = new SqlCommand(sqlstr);
        sq.Parameters.AddWithValue("@TableId", Table);
        ds = getDataset(sq, true);

        string sql = "";
        string row = hdfRow.Value;

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int num = Convert.ToInt32(dr["num"]);
            string value = Request.Form[String.Format("txtrow_{0}_{1}", hdfRow.Value, num)];

            List<SqlParameter> parms = new List<SqlParameter>();
            if (Inserting)
            {
                if (row == "0")
                {
                    sql = @"declare @rowid int 
                        insert into TableWidget.Rows (TableId, Priority) values(@tableid, isnull((select max(priority) from TableWidget.Rows where tableid=@tableid group by TableId), 0) + 1);
                        select @rowid = SCOPE_IDENTITY()
                        insert into TableWidget.Cells (RowId, ColumnId, Value) values(@rowid, @columnid, @val); 
                        select @rowid";

                    parms.Add(new SqlParameter("@tableid", Table));
                }
                else
                {
                    sql = "insert into TableWidget.Cells (RowId, ColumnId, Value) values(@rowid, @columnid, @val); select @rowid";
                    parms.Add(new SqlParameter("@rowid", row));
                }
            }
            else
            {
                sql = @"update TableWidget.Cells set Value=@val where RowId=@rowid and ColumnId=@columnid; 
                        if @@ROWCOUNT = 0
                            insert into TableWidget.Cells (RowId, ColumnId, Value) values(@rowid, @columnid, @val)
                        select @rowid";
                parms.Add(new SqlParameter("@rowid", row));
            }

            parms.Add(new SqlParameter("@columnid", dr["id"].ToString()));
            parms.Add(new SqlParameter("@val", value));

            row = ExecuteScalar(sql, parms, CommandType.Text).ToString();
        }

        FinishRowAction();
    }

    protected void btnDelRow_Click(object sender, EventArgs e)
    {
        string sqlstr = @" delete from TableWidget.Rows where id=@id ";
        List<SqlParameter> parms = new List<SqlParameter>();
        parms.Add(new SqlParameter("@id", hdfRow.Value));

        ProcessRecord(sqlstr, parms, CommandType.Text);

        FinishRowAction();
    }
    protected void btnBackTables_Click(object sender, EventArgs e)
    {
        SwitchView(pnlGroups);
    }

    #endregion

    #region Auxiliar
    private void SwitchView(Panel pnl)
    {
        pnlGroups.Visible = false;
        pnlEditGrp.Visible = false;
        pnlRows.Visible = false;

        pnl.Visible = true;

    }

    private void ClearFields()
    {
        txtNameGrp.Text = "";
        txtTitle.Text = "";
        Column = 0;
        //Row = 0;
        divColumnName.Visible = false;

    }

    private void ResetBackground(GridView gv)
    {
        for (int j = 0; j < gv.Rows.Count; j++)
        {
            GridViewRow row = gv.Rows[j];
            for (int i = 0; i < row.Cells.Count; i++)
                row.Cells[i].BackColor = System.Drawing.Color.Transparent;
        }
    }

    private void FinishRowAction()
    {
        Inserting = false;
        BuildGridView(Table);
        BindRows(false);
        btAddRow.Visible = true;
        SingleHeader = false;
    }
    #endregion

    #region dal
    protected string _connection = ConfigurationManager.AppSettings.Get("CMServer");

    //   protected string sqlUpdate = "";

    private DataTable getTable(string cmd)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.Fill(dt);
        return dt;
    }
    private DataTable getTable(SqlCommand cmd)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Connection = new SqlConnection(_connection);
        da.Fill(dt);
        return dt;
    }

    private DataTable getTable(SqlCommand cmd, bool a)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandType = CommandType.Text;
        da.SelectCommand.Connection = new SqlConnection(_connection);
        da.Fill(dt);
        return dt;
    }

    private DataSet getDataset(SqlCommand cmd, bool a)
    {
        DataSet dt = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandType = CommandType.Text;
        da.SelectCommand.Connection = new SqlConnection(_connection);
        da.Fill(dt);
        return dt;
    }

    private bool ProcessRecord(string sql, string fld, string _value, CommandType type)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(_connection));
        cmd.CommandType = type;
        cmd.Parameters.AddWithValue(fld, _value);
        cmd.Connection.Open();
        int ret = cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        return (ret > 0);
    }

    private bool ProcessRecord(string sql, List<SqlParameter> parms, CommandType type)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(_connection));
        cmd.CommandType = type;

        foreach(SqlParameter p in parms)
            cmd.Parameters.Add(p);

        cmd.Connection.Open();
        int ret = cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        return (ret > 0);
    }

    private object ExecuteScalar(string sql, List<SqlParameter> parms, CommandType type)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(_connection));
        cmd.CommandType = type;

        foreach (SqlParameter p in parms)
            cmd.Parameters.Add(p);

        cmd.Connection.Open();
        object ret = cmd.ExecuteScalar();
        cmd.Connection.Close();
        return ret;
    }

    private bool RemoveRecord(string sql, int rcrd)
    {
        return ProcessRecord(sql, "@id", rcrd.ToString(), CommandType.Text);
    }
    #endregion
 
}