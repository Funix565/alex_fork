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
    public partial class ProviderForm : Form
    {
        private ISession session;
        private Form1 parent;
        private string key;
        public ProviderForm()
        {
            InitializeComponent();
        }

        public void setPerent(Form1 parent)
        {
            this.parent = parent;
        }

        public void setSession(ISession session)
        {
            this.session = session;
        }

        public void setKey(string key)
        {
            this.key = key;
        }

        public void setTextBox1Text(string text)
        {
            this.textBox1.Text = text;
        }
        public void setTextBox2Text(string text)
        {
            this.textBox2.Text = text;
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
            AbsDAOFactory dao = new FactoryDAO(session);
            IProviderDAO providerDAO = dao.GetProviderDAO();
            Provider p = new Provider();
            p.Name = textBox1.Text;
            p.Country = textBox2.Text;
            providerDAO.SaveOrUpdate(p);
            parent.fillDataGridView1();
            this.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AbsDAOFactory dao = new FactoryDAO(session);
            IProviderDAO providerDAO = dao.GetProviderDAO();
            Provider p = providerDAO.GetProviderByName(key);
            p.Name = textBox1.Text;
            p.Country = textBox2.Text;
            providerDAO.SaveOrUpdate(p);
            parent.fillDataGridView1();
            this.Visible = false;
        }
    }
}
