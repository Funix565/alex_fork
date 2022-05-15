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
    public partial class ConnectForm : Form
    {
        private Form1 parent;
        public ConnectForm()
        {
            InitializeComponent();
        }

        public void setParent(Form1 parent)
        {
            this.parent = parent; 
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string host, db, user, pass;
            /* int port = Int32.Parse(textBox2.Text);
             host = textBox1.Text;
             db = textBox3.Text;
             user = textBox4.Text;
             pass = textBox5.Text;*/
            int port = 5432;
            host = "localhost";
            db = "csharp";
            user = "postgres";
            pass = "123";
            parent.Connect(host, port, db, user, pass);
            parent.GetProductForm().updateProviders(); 
            parent.FillDataGridView1ByProviders();
            this.Visible = false; 
        }
    }
}
