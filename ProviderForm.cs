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
    public partial class ProviderForm : Form
    {
        private Form1 parent = null;
        private int row; 
        public ProviderForm()
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
        
        public void enableAddButton(bool val)
        {
            pf_add_button.Enabled = val;
        }

        public void enableEditButton(bool val)
        {
            pf_edit_button.Enabled = val;
        }

        public void setFieldValues(string name, string country)
        {
            this.textBox1.Text = name;
            this.textBox2.Text = country; 
        }

        private void pf_add_button_Click(object sender, EventArgs e)
        {
            parent.AddProvider(textBox1.Text, textBox2.Text);
            parent.FillDataGridView1ByProviders();
            this.Visible = false; 
        }

        private void pf_edit_button_Click(object sender, EventArgs e)
        {
            parent.EditProvider(row, textBox1.Text, textBox2.Text);
            parent.FillDataGridView1ByProviders();  
            this.Visible = false; 
        }
    }
}
