using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NHibernate;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace WindowsFormsApp1
{

    public partial class ConnectionForm : Form
    {
        private ISessionFactory factory;
        private ISession session;
        private Form1 parent;
        public ConnectionForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            session = openSession("localhost", 5432, "csharp", "postgres", "123");
            this.Visible = false;
            parent.setSession(session);
            parent.fillDataGridView1();
        }

        public void setPerent(Form1 parent)
        {
            this.parent = parent;
        }

        private ISession openSession(String host, int port, String database, String user, String passwd)
        {
            ISession session = null;
            Assembly mappingsAssemly = Assembly.GetExecutingAssembly();
            if (factory == null)
            {
                    
                factory = Fluently.Configure().Database(PostgreSQLConfiguration.PostgreSQL82.ConnectionString
                    (c => c.Host(host).Port(port).Database(database).Username(user).Password(passwd)))
                    .Mappings(m => m.FluentMappings.AddFromAssembly(mappingsAssemly)).ExposeConfiguration(BuildSchema)
                    .BuildSessionFactory();
            }
            session = factory.OpenSession();
            return session;
        }

        private static void BuildSchema(Configuration config)
        {
            new SchemaExport(config).Create(false, true);
        }
    }
}
