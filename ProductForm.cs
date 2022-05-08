using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS_lab3
{
    public partial class ProductForm : Form
    {
        private Form1 parent = null;
        private int row;
        private List<Tuple<string, int>> providers = new List<Tuple<string, int>>();
        private string providerBeforeEditing; 

        public ProductForm()
        {
            InitializeComponent();
        }

        public void setParent(Form1 parent)
        {
            this.parent = parent; 
        }

        public void setRow(int row)
        {
            this.row = row; 
        }

        public void updateProviders()
        {
            DataRowCollection rows = parent.GetDataSet().Tables["Providers"].Rows;
            providers.Clear(); 
            foreach (DataRow dr in rows)
            {
                providers.Add(new Tuple<string, int>(dr.ItemArray[1].ToString(), (int)dr.ItemArray[0]));
            }
        }

        private void initializeCombobox(bool editMode, string editValue)
        {
            comboBox1.Items.Clear(); 
            foreach (Tuple<string, int> provider in providers)
            {
                comboBox1.Items.Add(provider.Item1);
            }
            if (!editMode)
            {
                comboBox1.SelectedItem = providers[0].Item1;
            }
            else
            {
                comboBox1.SelectedItem = editValue;
            }
        }

        public void setFields(string name, string count, string price, string provider)
        {
            textBox1.Text = name;
            textBox2.Text = count;
            textBox3.Text = price;
            // when editing 
            if (name != "")
            {
                initializeCombobox(true, provider); 
            } 
            else
            {
                initializeCombobox(false, ""); 
            }
        }

        public string getProviderName(int id)
        {
            foreach (var provider in providers)
            {
                if ((int)provider.Item2 == id)
                {
                    return provider.Item1.ToString();
                }
            }
            return "";
        }

        private int getSelectedProviderId()
        {
            foreach (var provider in providers)
            {
                if (provider.Item1.ToString() == comboBox1.SelectedItem.ToString())
                {
                    return (int)provider.Item2;
                }
            }
            return -1; 
        }

        public void enableEditButton(bool val)
        {
            prod_f_edit_button.Enabled = val;
        }

        public void enableAddButton(bool val)
        {
            prod_f_add_button.Enabled = val;
        }

        private void prod_f_add_button_Click(object sender, EventArgs e)
        {
            parent.AddProduct(textBox1.Text, Int32.Parse(textBox2.Text),
                Double.Parse(textBox3.Text),  getSelectedProviderId()); 
            parent.FillDataGridView2ByProducts(comboBox1.SelectedItem.ToString());
            this.Visible = false; 
        }

        public void setProviderBeforeEditing(string name)
        {
            providerBeforeEditing = name; 
        }

        private void prod_f_edit_button_Click(object sender, EventArgs e)
        {
            // if edited provider, than should add this product to another table 
            if (comboBox1.SelectedItem.ToString() != providerBeforeEditing)
            {
                parent.GetDataSet().Tables["Products"].Rows[row].Delete();
                parent.AddProduct(textBox1.Text, Int32.Parse(textBox2.Text),
                            Double.Parse(textBox3.Text), getSelectedProviderId());
            }
            else
            {
                parent.EditProduct(row, textBox1.Text, Int32.Parse(textBox2.Text),
                    Double.Parse(textBox3.Text), getSelectedProviderId());
            }
            parent.FillDataGridView2ByProducts(comboBox1.SelectedItem.ToString());
            this.Visible = false;
        }
    }
}
