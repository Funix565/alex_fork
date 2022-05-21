using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.dao;
using WindowsFormsApp1.domain;

namespace WindowsFormsApp1
{
    public partial class ProductForm : Form
    {
        private ISession session;
        private Form1 parent;
        private string provider_name;
        private string product_name;
        public ProductForm()
        {
            InitializeComponent();
        }

        public void setSession(ISession session)
        {
            this.session = session;
        }
        public void setPerent(Form1 parent)
        {
            this.parent = parent;
        }
        public void setProviderName(string provider_name)
        {
            this.provider_name = provider_name;
        }

        public void setProductName(string product_name)
        {
            this.product_name = product_name;
        }

        public void setTextBox1Text(string text)
        {
            this.textBox1.Text = text;
        }
        public void setTextBox2Text(string text)
        {
            this.textBox2.Text = text;
        }
        public void setTextBox3Text(string text)
        {
            this.textBox3.Text = text;
        }
        public void setButton1Visible(bool visible)
        {
            this.button1.Visible = visible;
        }
        public void setButton2Visible(bool visible)
        {
            this.button2.Visible = visible;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            AbsDAOFactory factory = new FactoryDAO(session);
            IProviderDAO providerDAO = factory.GetProviderDAO();
            Provider p = providerDAO.GetProviderByName(provider_name);
            Product prod = new Product();
            prod.Name = textBox1.Text;
            prod.Count = Int32.Parse(textBox2.Text);
            prod.Price = Double.Parse(textBox3.Text);
            p.Products.Add(prod);
            prod.Provider = p;
            providerDAO.SaveOrUpdate(p);
            parent.fillDataGridView2(provider_name);
            this.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AbsDAOFactory dao = new FactoryDAO(session);
            IProductDAO prodDAO = dao.GetProductDAO();
            Product prod = prodDAO.GetProductByProviderAndProductName(provider_name, product_name);
            prod.Name = textBox1.Text;
            prod.Count = Int32.Parse(textBox2.Text);
            prod.Price = Double.Parse(textBox3.Text);
            prodDAO.SaveOrUpdate(prod);
            this.Visible = false;
            parent.fillDataGridView2(provider_name);
        }
    }
}
