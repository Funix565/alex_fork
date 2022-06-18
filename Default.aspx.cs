using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate;
using WindowsFormsApp1.dao;
using WindowsFormsApp1.domain;

namespace CS_lab5
{
    public partial class _Default : Page
    {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Prerender(object sender, EventArgs e)
    {
      ISession session = (ISession)Session["hbmsession"];
      AbsDAOFactory dao = new FactoryDAO(session);
      IProviderDAO providerDAO = dao.GetProviderDAO();
      List<Provider> providers = providerDAO.GetAll();
      GridView1.DataSource = providers;
      GridView1.DataBind();
    }

    protected void ibInsert_Click(object sender, EventArgs e)
    {
      string s1 = ((TextBox)GridView1.FooterRow.FindControl("MyFooterTextBox1")).Text;
      string s2 = ((TextBox)GridView1.FooterRow.FindControl("MyFooterTextBox2")).Text;
      
      Provider p = new Provider();
      p.Name = s1;
      p.Country = s2; 

      ISession session = (ISession)Session["hbmsession"];
      AbsDAOFactory dao = new FactoryDAO(session);
      IProviderDAO providerDAO = dao.GetProviderDAO();
      providerDAO.SaveOrUpdate(p);
      Response.Redirect(HttpContext.Current.Request.Url.ToString());
    }
    
     // add first row to grid view 
    protected void ibInsertInEmpty_Click(object sender, EventArgs e)
    {
      var parent = ((Control)sender).Parent;
      var nameTextBox = parent.FindControl("emptyProviderNameTextBox") as TextBox;
      var countryTextBox = parent.FindControl("emptyProviderCountryTextBox") as TextBox;

      Provider p = new Provider();
      p.Name = nameTextBox.Text;
      p.Country = countryTextBox.Text;

      ISession session = (ISession)Session["hbmsession"];
      AbsDAOFactory dao = new FactoryDAO(session);
      IProviderDAO providerDAO = dao.GetProviderDAO();
      providerDAO.SaveOrUpdate(p);
      Response.Redirect(HttpContext.Current.Request.Url.ToString());
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
      int index = e.RowIndex;
      GridViewRow row = GridView1.Rows[index];
      
      // get provider's name 
      string key = ((Label)(row.Cells[0].FindControl("myLabel1"))).Text;
      

      ISession hbmSession = (ISession)Session["hbmsession"];
      AbsDAOFactory dao = new FactoryDAO(hbmSession);
      IProviderDAO providerDAO = dao.GetProviderDAO();
      Provider p = providerDAO.GetProviderByName(key); 
      //Удаление группы
      if (p != null)
      {
        providerDAO.Delete(p);
      }
      Response.Redirect(HttpContext.Current.Request.Url.ToString());
    }

    // row into editing mode 
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
      
      int index = e.NewEditIndex;
      GridViewRow row = GridView1.Rows[index];
      
      string oldProviderName = ((Label)(row.Cells[0].FindControl("myLabel1"))).Text;
     
      ViewState["oldProviderName"] = oldProviderName;
      GridView1.EditIndex = index;
      GridView1.ShowFooter = false;
      GridView1.DataBind();
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
      GridView1.EditIndex = -1;
      GridView1.ShowFooter = true;
      GridView1.DataBind();
    }

    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
      int index = e.RowIndex;
      GridViewRow row = GridView1.Rows[index];
      string newProviderName = ((TextBox)(row.Cells[0].FindControl("myTextBox1"))).Text;
      string newProviderCountry = ((TextBox)(row.Cells[1].FindControl("myTextBox2"))).Text;
      string oldProviderName = (string)ViewState["oldProviderName"];
      
      ISession hbmSession = (ISession)Session["hbmsession"];
      AbsDAOFactory dao = new FactoryDAO(hbmSession);
      IProviderDAO providerDAO = dao.GetProviderDAO();
      Provider p = providerDAO.GetProviderByName(oldProviderName);
      
      p.Name= newProviderName;
      p.Country = newProviderCountry;
      providerDAO.SaveOrUpdate(p);
      GridView1.EditIndex = -1;
      GridView1.ShowFooter = true;
      GridView1.DataBind();
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
      if (e.CommandName == "Select")
      {
        int index = Convert.ToInt32(e.CommandArgument);
        GridViewRow row = GridView1.Rows[index];
        string providerName = ((Label)(row.Cells[0].FindControl("myLabel1"))).Text;
        Session["keyProviderName"] = providerName;
        Response.Redirect("ProductsForm.aspx");
      }
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
      GridView1.PageIndex = e.NewPageIndex;
      GridView1.EditIndex = -1;
      GridView1.ShowFooter = true;
      GridView1.DataBind();
    }

  }
}