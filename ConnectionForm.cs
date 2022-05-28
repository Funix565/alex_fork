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
using WindowsFormsApp1.domain;
using WindowsFormsApp1.dao;
using System.Diagnostics;

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

        private void fillDb()
        {
            AbsDAOFactory abs = new FactoryDAO(session);
            IProductDAO productDAO = abs.GetProductDAO();
            IProviderDAO providerDAO = abs.GetProviderDAO();

            Provider p1 = new Provider();
            p1.Name = "provider1";
            p1.Country = "country1";
            Provider p2 = new Provider();
            p2.Name = "provider2";
            p2.Country = "country2";
            providerDAO.SaveOrUpdate(p1);
            providerDAO.SaveOrUpdate(p2);

            Product prod1 = new Product(), prod2 = new Product(), prod3 = new Product();
            prod1.Name = "car";
            prod1.Price = 1234;
            prod1.Provider = p1;
            prod1.Count = 10;
            p1.Products.Add(prod1);
            providerDAO.SaveOrUpdate(p1);

            prod2.Name = "car";
            prod2.Price = 800;
            prod2.Provider = p2;
            prod2.Count = 12;
            p2.Products.Add(prod2);
            productDAO.SaveOrUpdate(prod2);

            prod3.Name = "javelin";
            prod3.Price = 430;
            prod3.Count = 300;
            prod3.Provider = p1;
            p1.Products.Add(prod3);
            productDAO.SaveOrUpdate(prod3);

            IDAO<Delivery> d = new GenericDAO<Delivery>(session);      
            
             Delivery delivery1 = new Delivery(), delivery2 = new Delivery(), delivery3 = new Delivery();

             // Add delivery for car from Provider1 
             delivery1.Date = "12/12/2022";
             delivery1.quantity = 5;
             delivery1.Provider = p1;
             delivery1.Product = delivery1.Provider.Products[0];
             p1.Deliveries.Add(delivery1);
             prod1.Deliveries.Add(delivery1);
            d.SaveOrUpdate(delivery1);

             // Add delivery for car from provider2
             delivery2.Date = "10/05/2019";
             delivery2.quantity = 7;
             delivery2.Provider = p2;
             delivery2.Product = delivery2.Provider.Products[0];
             p2.Deliveries.Add(delivery2);
             prod2.Deliveries.Add(delivery2);
            d.SaveOrUpdate(delivery2);

            // Add delivery for javelin from provider1
            delivery3.Date = "03/07/2015";
             delivery3.quantity = 1;
             delivery3.Provider = p1;
             delivery3.Product = delivery3.Provider.Products[1];
             p1.Deliveries.Add(delivery3);
             prod3.Deliveries.Add(delivery3);
            d.SaveOrUpdate(delivery3);

            // Add delivery for car from provider1 (second delivery of this product from this provider)
            Delivery d4 = new Delivery();
            d4.Date = "04/05/2017";
            d4.quantity = 1;
            d4.Provider = p1;
            d4.Product = d4.Provider.Products[0];
            p1.Deliveries.Add(d4);
            prod1.Deliveries.Add(d4);
            d.SaveOrUpdate(d4);

            Debug.Assert(p1.Deliveries.Count == 3 && p2.Deliveries.Count == 1);
            Debug.Assert(prod1.Deliveries.Count == 2 && prod1.Provider == p1
                && prod2.Deliveries.Count == 1 && prod2.Provider == p2 
                && prod3.Deliveries.Count == 1 && prod3.Provider == p1);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            session = openSession("localhost", 5432, "csharp", "postgres", "123");
            this.Visible = false;
            parent.setSession(session);
            fillDb();
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
                    (c => c.Host(host).Port(port).Database(database).Username(user).Password(passwd)).ShowSql())
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
