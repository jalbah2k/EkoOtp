using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Controls_Blog_Blog : System.Web.UI.UserControl
{
    //SearchDocuments1.delegateHidePages += new SearchPages(HideDelegateFunction);
        
    protected string param;

    public Controls_Blog_Blog()
    {
        param = "";
    }

    public Controls_Blog_Blog(string a)
    {
        param = a;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.BlogList1.delegateLoadContent = new LoadContent(LoadContentDelegateFunction);
    }

    protected void LoadContentDelegateFunction(int id)
    {
        this.BlogContent1.BindData(id);
    }
}