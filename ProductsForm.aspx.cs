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
  public partial class ProductsForm : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Prerender(object sender, EventArgs e)
    {
      string keyProvider = (string)Session["keyProviderName"];
      Label5.Text = keyProvider;
      ISession session = (ISession)Session["hbmsession"];
      AbsDAOFactory factory = new FactoryDAO(session);
      IProviderDAO providerDAO = factory.GetProviderDAO();
      IList<Product> products = providerDAO.getAllProductsOfProvider(keyProvider);
      GridView1.DataSource = products;
      GridView1.DataBind();
    }

    protected void ibInsert_Click(object sender, EventArgs e)
    {
      string keyProvider = (string)Session["keyProviderName"];
      
      string s1 = ((TextBox)GridView1.FooterRow.FindControl("MyFooterTextBox1")).Text;
      string s2 = ((TextBox)GridView1.FooterRow.FindControl("MyFooterTextBox2")).Text;
      string s3 = ((TextBox)GridView1.FooterRow.FindControl("MyFooterTextBox3")).Text;
      //Создаем сессию
      ISession session = (ISession)Session["hbmsession"];
      AbsDAOFactory factory = new FactoryDAO(session);
      IProviderDAO providerDAO = factory.GetProviderDAO();
      Provider p = providerDAO.GetProviderByName(keyProvider);

      IProductDAO productDAO = factory.GetProductDAO();

      Product prod = new Product();
      prod.Name = s1;
      prod.Price = Convert.ToDouble(s2);
      prod.Count = Convert.ToInt32(s3);
      prod.Provider = p;
      p.Products.Add(prod); 
      productDAO.SaveOrUpdate(prod);
      Response.Redirect(HttpContext.Current.Request.Url.ToString());
    }

    protected void ibInsertInEmpty_Click(object sender, EventArgs e)
    {
      string keyProvider = (string)Session["keyProviderName"];
      
      var parent = ((Control)sender).Parent;
      var nameTextBox = parent.FindControl("emptyProductNameTextBox") as TextBox;
      var priceTextBox = parent.FindControl("emptyProductPriceTextBox") as TextBox;
      var countTextBox = parent.FindControl("emptyProductQuantityTextBox") as TextBox;
      

      ISession session = (ISession)Session["hbmsession"];
      AbsDAOFactory factory = new FactoryDAO(session);
      IProviderDAO providerDAO = factory.GetProviderDAO();
      Provider p = providerDAO.GetProviderByName(keyProvider);
      IProductDAO productDAO = factory.GetProductDAO();

      Product prod = new Product();
      prod.Name = nameTextBox.Text;
      prod.Price = Convert.ToDouble(priceTextBox.Text);
      prod.Count = Convert.ToInt32(countTextBox.Text);
      prod.Provider = p;
      p.Products.Add(prod);
      productDAO.SaveOrUpdate(prod);
      Response.Redirect(HttpContext.Current.Request.Url.ToString());
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
      string keyProvider = (string)Session["keyProviderName"];
      
      int index = e.RowIndex;
      GridViewRow row = GridView1.Rows[index];
      
      string productName = ((Label)(row.Cells[0].FindControl("myLabel1"))).Text;

      ISession hbmSession = (ISession)Session["hbmsession"];
      AbsDAOFactory factory = new FactoryDAO(hbmSession);
      IProductDAO productDAO = factory.GetProductDAO();
      Product prod = productDAO.GetProductByProviderAndProductName(keyProvider, productName); 
      if (prod != null)
      {
        prod.Provider.Products.Remove(prod);
        productDAO.Delete(prod); 
      }
      Response.Redirect(HttpContext.Current.Request.Url.ToString());
    }

    // into editing mode 
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
      int index = e.NewEditIndex;
      GridViewRow row = GridView1.Rows[index];

      string oldProdName = ((Label)(row.Cells[0].FindControl("myLabel1"))).Text;

      ViewState["oldProdName"] = oldProdName;
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
      string keyProvider = (string)Session["keyProviderName"];
      int index = e.RowIndex;
      GridViewRow row = GridView1.Rows[index];
      string newProdName =  ((TextBox)(row.Cells[0].FindControl("myTextBox1"))).Text;
      string newPrice = ((TextBox)(row.Cells[1].FindControl("myTextBox2"))).Text;
      string newCount = ((TextBox)(row.Cells[2].FindControl("myTextBox3"))).Text;

      string oldFirstName = (string)ViewState["oldProdName"];
      
      
      ISession hbmSession = (ISession)Session["hbmsession"];
      AbsDAOFactory factory = new FactoryDAO(hbmSession);
      IProductDAO productDAO = factory.GetProductDAO();

      Product prod = productDAO.GetProductByProviderAndProductName(keyProvider, oldFirstName);
      prod.Name = newProdName;
      prod.Price = Convert.ToDouble(newPrice);
      prod.Count = Convert.ToInt32(newCount);

      productDAO.SaveOrUpdate(prod);
      GridView1.EditIndex = -1;
      GridView1.ShowFooter = true;
      GridView1.DataBind();
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