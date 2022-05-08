using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace CS_lab3
{
    public partial class Form1 : Form
    {
        private NpgsqlConnection connection = null;
        private DataSet dataSet = null;
        private NpgsqlDataAdapter providerDataAdapter = null;
        private NpgsqlDataAdapter productDataAdapter = null;
        private ConnectForm connectForm = null;
        private ProviderForm providerForm = null;
        private ProductForm productForm = null;

        public Form1()
        {
            InitializeComponent();
        }

        public void FillDataGridView1ByProviders()
        {
            GetDataSet().Tables["Providers"].Clear();
            providerDataAdapter = new NpgsqlDataAdapter("SELECT * FROM provider", connection);
            new NpgsqlCommandBuilder(providerDataAdapter);
            providerDataAdapter.Fill(GetDataSet(), "Providers");
            dataGridView1.DataSource = GetDataSet().Tables["Providers"];
        }

        public void FillDataGridView2ByProducts(string provider_name)
        {
            GetDataSet().Tables["Products"].Clear();
            productDataAdapter = new NpgsqlDataAdapter("SELECT product.id, product.name, product.count, " +
                "product.price, product.provider_id " + "FROM product, provider " + 
                "WHERE product.provider_id = provider.id and provider.name = '" + provider_name + "'", connection);
            new NpgsqlCommandBuilder(productDataAdapter);
            productDataAdapter.Fill(dataSet, "Products");
            dataGridView2.DataSource = GetDataSet().Tables["Products"];
        }

        public NpgsqlConnection Connect(string host, int port, string database, string user, string pass)
        {
            NpgsqlConnectionStringBuilder stringBuilder = new NpgsqlConnectionStringBuilder();
            stringBuilder.Host = host;
            stringBuilder.Port = port;
            stringBuilder.Username = user;
            stringBuilder.Password = pass;
            stringBuilder.Database = database;
            stringBuilder.Timeout = 30;
            try
            {
                connection = new NpgsqlConnection(stringBuilder.ConnectionString);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to connect!", "", MessageBoxButtons.OK);
            }
            connection.Open();
            return connection;
        }

        public DataSet GetDataSet()
        {
            if (dataSet == null)
            {
                dataSet = new DataSet();
                dataSet.Tables.Add("Providers");
                dataSet.Tables.Add("Products");
            }
            return dataSet; 
        }

        //-------------- Methods related with ConnectionForm ----------------------------//
        public void setConnection(NpgsqlConnection connection)
        {
            this.connection = connection;
        }
        public ConnectForm GetConnectForm()
        {
            if (connectForm == null)
            {
                connectForm = new ConnectForm();
                connectForm.setParent(this);
            }
            return connectForm; 
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetConnectForm().Visible = true; 
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (connection != null)
              connection.Close();
        }
        //--------------- End of Connection form methods ---------------------------------//


        //-------------- Methods related with ProviderForm ----------------------------//
        private ProviderForm GetProviderForm()
        {
            if (providerForm == null)
            {
                providerForm = new ProviderForm();
                providerForm.setParent(this); 
            }
            return providerForm;
        }

        public void AddProvider(string name, string country)
        {
            GetDataSet().Tables["Providers"].Rows.Add(0, name, country);
            providerDataAdapter.Update(GetDataSet(), "Providers");
            GetProductForm().updateProviders();
        }

        public void EditProvider(int row, string name, string country)
        {
            GetDataSet().Tables["Providers"].Rows[row]["name"] = name;
            GetDataSet().Tables["Providers"].Rows[row]["country"] = country;
            providerDataAdapter.Update(GetDataSet(), "Providers");
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetProviderForm().Visible = true;
            GetProviderForm().setFieldValues("", "");
            GetProviderForm().enableAddButton(true);
            GetProviderForm().enableEditButton(false);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.SelectedCells[0].RowIndex;
            string name = (string)GetDataSet().Tables["Providers"].Rows[row].ItemArray[1];
            DialogResult d = MessageBox.Show("Remove provider " + name + "?", "", MessageBoxButtons.YesNo);
            if (d == DialogResult.Yes)
            {
                GetDataSet().Tables["Providers"].Rows[row].Delete();
                providerDataAdapter.Update(GetDataSet(), "Providers");
                FillDataGridView1ByProviders();
                GetProductForm().updateProviders();
                GetDataSet().Tables["Products"].Clear();
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.SelectedCells[0].RowIndex;
            string name = (string)GetDataSet().Tables["Providers"].Rows[row].ItemArray[1];
            string country = (string)GetDataSet().Tables["Providers"].Rows[row].ItemArray[2];
            GetProviderForm().Visible = true;
            GetProviderForm().setFieldValues(name, country);
            GetProviderForm().setRow(row);
            GetProviderForm().enableAddButton(false);
            GetProviderForm().enableEditButton(true);
        }

        // handler for click on provider. shows all provider's products 
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = dataGridView1.SelectedCells[0].RowIndex;
            FillDataGridView2ByProducts(dataGridView1.Rows[row].Cells[1].Value.ToString());
        }

        //--------------- End of ProviderForm methods ---------------------------------//

        //-------------- Methods related with ProductForm ----------------------------//

        public ProductForm GetProductForm()
        {
            if (productForm == null)
            {
                productForm = new ProductForm();
                productForm.setParent(this); 
            }
            return productForm;
        }

        public void AddProduct(string name, int count, double price, int provider_id)
        {
            GetDataSet().Tables["Products"].Rows.Add(0, name, count, price, provider_id);
            productDataAdapter.Update(GetDataSet(), "Products");
        }

        public void EditProduct(int row, string name, int count, double price, int provider_id)
        {
            // row always 0 
            GetDataSet().Tables["Products"].Rows[row]["name"] = name;
            GetDataSet().Tables["Products"].Rows[row]["count"] = count;
            GetDataSet().Tables["Products"].Rows[row]["price"] = price;
            GetDataSet().Tables["Products"].Rows[row]["provider_id"] = provider_id;
            productDataAdapter.Update(GetDataSet(), "Products");
        }

        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GetProductForm().Visible = true;
            GetProductForm().updateProviders();
            GetProductForm().setFields("", "", "", "");
            GetProductForm().enableAddButton(true);
            GetProductForm().enableEditButton(false);
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int row = dataGridView2.SelectedCells[0].RowIndex;
            string name = (string)GetDataSet().Tables["Products"].Rows[row].ItemArray[1];
            DialogResult d = MessageBox.Show("Remove product " + name + "?", "", MessageBoxButtons.YesNo);
            if (d == DialogResult.Yes)
            {
                GetDataSet().Tables["Products"].Rows[row].Delete();
                productDataAdapter.Update(GetDataSet(), "Products");
            }
        }

        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GetProductForm().updateProviders();
            GetProductForm().Visible = true;
            int row = dataGridView2.SelectedCells[0].RowIndex;
            string name = GetDataSet().Tables["Products"].Rows[row]["name"].ToString();
            int count = (int)GetDataSet().Tables["Products"].Rows[row]["count"];
            double price = (double)GetDataSet().Tables["Products"].Rows[row]["price"];
            string provider_name = GetProductForm().getProviderName((int)GetDataSet().Tables["Products"].Rows[row]["provider_id"]);
            GetProductForm().setProviderBeforeEditing(provider_name);
            GetProductForm().setFields(name, count.ToString(), price.ToString(), provider_name);
            GetProductForm().enableAddButton(false);
            GetProductForm().enableEditButton(true);
        }

        //--------------- End of ProductForm methods ---------------------------------//
    }
}
