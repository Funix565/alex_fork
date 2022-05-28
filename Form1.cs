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
using WindowsFormsApp1.domain;
using WindowsFormsApp1.dao;
using WindowsFormsApp1.mapping;
using NHibernate.Cfg;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private ISession session;
        private ConnectionForm connectionForm = null;
        private ProviderForm providerForm = null;
        private ProductForm productForm = null;
        public Form1()
        {
            InitializeComponent();
            Configuration cfg = new Configuration().SetProperty("hibernate.show_sql", "true");

        }

        // -------------------- Methods related with connection ------------------------------//
        public void setSession(ISession session)
        {
            this.session = session;
        }

        private ConnectionForm GetConnectionForm()
        {
            if (connectionForm == null)
            {
                connectionForm = new ConnectionForm();
            }
            connectionForm.setPerent(this);
            return connectionForm;
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetConnectionForm().Visible = true;
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (session != null)
            {
                session.Close();
                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
            }   
        }

        // -------------------- End of methods related with connection ------------------------------//

        // -------------------- Methods related with Provider table ------------------------------//

        public void fillDataGridView1()
        {
            dataGridView1.Rows.Clear();
            AbsDAOFactory factory = new FactoryDAO(session);
            IProviderDAO providerDAO = factory.GetProviderDAO();
            List<Provider> providerList = providerDAO.GetAll();
            DataGridViewRow row;
            DataGridViewTextBoxCell cell1, cell2;
            foreach (Provider p in providerList)
            {
                row = new DataGridViewRow();
                cell1 = new DataGridViewTextBoxCell();
                cell2 = new DataGridViewTextBoxCell();
                cell1.ValueType = typeof(string);
                cell1.Value = p.Name;
                cell2.ValueType = typeof(string);
                cell2.Value = p.Country;
                row.Cells.Add(cell1);
                row.Cells.Add(cell2);
                dataGridView1.Rows.Add(row);
            }
        }

        private ProviderForm GetProviderForm()
        {
            if (providerForm == null)
            {
                providerForm = new ProviderForm();
            }
            providerForm.setSession(session);
            providerForm.setPerent(this);
            return providerForm;
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProviderForm f = GetProviderForm();
            f.Visible = true;
            f.setTextBox1Text("");
            f.setTextBox2Text("");
            f.setButton1Visible(true);
            f.setButton2Visible(false);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProviderForm f = GetProviderForm();
            int selected_row = dataGridView1.SelectedCells[0].RowIndex;
            string provider_name = (string)dataGridView1.Rows[selected_row].Cells[0].Value;
            AbsDAOFactory dao = new FactoryDAO(session);
            IProviderDAO providerDAO = dao.GetProviderDAO();            
            Provider p = providerDAO.GetProviderByName(provider_name);
            f.setKey(provider_name);
            f.setTextBox1Text(p.Name);
            f.setTextBox2Text(p.Country);
            f.setButton1Visible(false);
            f.setButton2Visible(true);
            f.Visible = true; 
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selected_row = dataGridView1.SelectedCells[0].RowIndex;
            string provider_name = (string)dataGridView1.Rows[selected_row].Cells[0].Value;
            AbsDAOFactory dao = new FactoryDAO(session);
            IProviderDAO providerDAO = dao.GetProviderDAO();
            DialogResult dr = MessageBox.Show("Remove provider " + provider_name + "?", "", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    providerDAO.RemoveProviderByName(provider_name);
                    this.fillDataGridView1();
                    this.dataGridView2.Rows.Clear(); 
                }
                catch (Exception)
                {
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRow = dataGridView1.SelectedCells[0].RowIndex;
            string key = (string)dataGridView1.Rows[selectedRow].Cells[0].Value;
            fillDataGridView2(key);
        }

        // -------------------- End of methods related with Provider table ------------------------------//


        // -------------------- Methods related with Product table ------------------------------//

        public void fillDataGridView2(string provider_name)
        {
            dataGridView2.Rows.Clear();
            AbsDAOFactory dao = new FactoryDAO(session);
            IProviderDAO providerDAO = dao.GetProviderDAO();
            IList<Product> prodList = providerDAO.getAllProductsOfProvider(provider_name);
            DataGridViewRow row;
            DataGridViewTextBoxCell cell1, cell2, cell3; 
            foreach (Product p in prodList)
            {
                row = new DataGridViewRow();
                cell1 = new DataGridViewTextBoxCell();
                cell2 = new DataGridViewTextBoxCell();
                cell3 = new DataGridViewTextBoxCell();
                cell1.ValueType = typeof(string);
                cell1.Value = p.Name;
                cell2.ValueType = typeof(int);
                cell2.Value = p.Count;
                cell3.ValueType = typeof(double);
                cell3.Value = p.Price;
                row.Cells.Add(cell1);
                row.Cells.Add(cell2);
                row.Cells.Add(cell3);
                dataGridView2.Rows.Add(row);
            }
        }

        private ProductForm GetProductForm()
        {
            if (productForm == null)
            {
                productForm = new ProductForm();
            }
            productForm.setSession(session);
            productForm.setPerent(this);
            return productForm;
        }

        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int selectedRow = dataGridView1.SelectedCells[0].RowIndex;
            string provider_name = (string)dataGridView1.Rows[selectedRow].Cells[0].Value;
            ProductForm f = GetProductForm();
            f.setTextBox1Text("");
            f.setTextBox2Text("");
            f.setTextBox3Text("");
            f.setButton1Visible(true);
            f.setButton2Visible(false);
            f.setProviderName(provider_name);
            f.Visible = true; 
        }

        private Product GetSelectedProduct()
        {
            int selectedRow = dataGridView1.SelectedCells[0].RowIndex;
            string provider_name = (string)dataGridView1.Rows[selectedRow].Cells[0].Value;
            selectedRow = dataGridView2.SelectedCells[0].RowIndex;
            string product_name = (string)dataGridView2.Rows[selectedRow].Cells[0].Value;
            AbsDAOFactory factory = new FactoryDAO(session);
            return factory.GetProductDAO().GetProductByProviderAndProductName(provider_name, product_name);
        }

        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Product p = GetSelectedProduct();
            GetProductForm().setTextBox1Text(p.Name);
            GetProductForm().setTextBox2Text(p.Count.ToString());
            GetProductForm().setTextBox3Text(p.Price.ToString());
            GetProductForm().setProductName(p.Name);
            GetProductForm().setProviderName(p.Provider.Name);
            GetProductForm().setButton1Visible(false);
            GetProductForm().setButton2Visible(true);
            GetProductForm().Visible = true; 
        }

        private void removeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Product p = GetSelectedProduct();
            string provider_name = p.Provider.Name;
            DialogResult dr = MessageBox.Show("Remove product " + p.Name + "?", "", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    p.Provider.Products.Remove(p);
                    new FactoryDAO(session).GetProductDAO().Delete(p);
                    fillDataGridView2(provider_name);
                }
                catch (Exception)
                {
                }
            }
        }

        // -------------------- End of methods related with Product table ------------------------------//

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.dataGridView1.Columns["ProviderNameColumn"].Width = (this.dataGridView1.Width / 2) - 1;
            this.dataGridView1.Columns["ProviderCountryColumn"].Width = (this.dataGridView1.Width / 2) - 1;
            this.dataGridView2.Columns["ProductNameColumn"].Width = (this.groupBox2.Width / 2) - 1;
            this.dataGridView2.Columns["ProductCountColumn"].Width = (this.groupBox2.Width / 4) - 1;
            this.dataGridView2.Columns["ProductPriceColumn"].Width = (this.groupBox2.Width / 4) - 1;
            this.dataGridView3.Columns["OrderDateColumn"].Width = (this.groupBox3.Width / 2) - 1;
            this.dataGridView3.Columns["OrderQuantityColumn"].Width = (this.groupBox3.Width / 2) - 1;
        }

        public void fillDataGridView3(string provider, string product)
        {
            dataGridView3.Rows.Clear();
            AbsDAOFactory dao = new FactoryDAO(session);
            IProviderDAO providerDAO = dao.GetProviderDAO();
            IList<Delivery> deliveries = providerDAO.GetProviderByName(provider).Deliveries.Where(x => x.Product.Name == product).ToList(); 
            DataGridViewRow row;
            DataGridViewTextBoxCell cell1, cell2;
            foreach (Delivery o in deliveries)
            {
                row = new DataGridViewRow();
                cell1 = new DataGridViewTextBoxCell();
                cell2 = new DataGridViewTextBoxCell();
                cell1.ValueType = typeof(string);
                cell1.Value = o.Date;
                cell2.ValueType = typeof(int);
                cell2.Value = o.quantity;
                row.Cells.Add(cell1);
                row.Cells.Add(cell2);
                dataGridView3.Rows.Add(row);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRow = dataGridView1.SelectedCells[0].RowIndex;
            string provider = (string)dataGridView1.Rows[selectedRow].Cells[0].Value;
            selectedRow = dataGridView2.SelectedCells[0].RowIndex;
            string product = (string)dataGridView2.Rows[selectedRow].Cells[0].Value;
            fillDataGridView3(provider, product);
        }
    }
}
