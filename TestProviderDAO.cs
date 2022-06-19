using Lab6TestAlex.dao;
using Lab6TestAlex.domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6AlexTests
{
    [TestClass]
    public /*abstract */ class TestProviderDAO : TestGenericDAO<Provider>
    {
        protected IProviderDAO providerDAO = null;
        protected Product prod1 = null;
        protected Product prod2 = null;
        protected Product prod3 = null;

        public TestProviderDAO() : base()
        {
            AbsDAOFactory daoFactory = new FactoryDAO(session);
            providerDAO = daoFactory.GetProviderDAO();
            setDAO(providerDAO);
        }

        protected override void createEntities()
        {
            entity1 = new Provider();
            entity1.Name = "Mercedes";
            entity1.Country = "Germany";

            entity2 = new Provider();
            entity2.Name = "Lamborgini";
            entity2.Country = "Italy";

            entity3 = new Provider();
            entity3.Name = "Intel";
            entity3.Country = "USA";
        }

        protected override void checkAllPropertiesDiffer(Provider entityToCheck1, Provider entityToCheck2)
        {
            Assert.AreNotEqual(entityToCheck1.Name, entityToCheck2.Name, "Provider names must be different");
            Assert.AreNotEqual(entityToCheck1.Country, entityToCheck2.Country, "Provider countries must be different");
        }

        protected override void checkAllPropertiesEqual(Provider entityToCheck1, Provider entityToCheck2)
        {
            Assert.AreEqual(entityToCheck1.Name, entityToCheck2.Name, "Provider names must be equal");
            Assert.AreEqual(entityToCheck1.Country, entityToCheck2.Country, "Provider countries must be equal");
        }

        [TestMethod]
        public void TestGetByIdProvider()
        {
            base.TestGetByIdGeneric();
        }
        [TestMethod]
        public void TestGetAllProviders()
        {
            base.TestGetAllGeneric();
        }
        [TestMethod]
        public void TestDeleteProvider()
        {
            base.TestDeleteGeneric();
        }

        [TestMethod]
        public void TestGetProviderByName()
        {
            Provider p1 = providerDAO.GetProviderByName(entity1.Name);
            Assert.IsNotNull(p1, "Service method GetProviderByName should return group if successfull");

            Provider p2 = providerDAO.GetProviderByName(entity2.Name);
            Assert.IsNotNull(p2, "Service method GetProviderByName should return group if successfull");

            Provider p3 = providerDAO.GetProviderByName(entity3.Name);
            Assert.IsNotNull(p3, "Service method GetProviderByName should return group if successfull");

            checkAllPropertiesEqual(p1, entity1);
            checkAllPropertiesEqual(p2, entity2);
            checkAllPropertiesEqual(p3, entity3);
        }

        [TestMethod]
        public void TestGetAllStudentOfGroup()
        {
            createEntitiesForProvider();
            Assert.IsNotNull(prod1, "Please, create object for prod1");
            Assert.IsNotNull(prod2, "Please, create object for prod2");
            Assert.IsNotNull(prod3, "Please, create object for prod3");

            entity1.Products.Add(prod1);
            prod1.Provider = entity1;

            entity1.Products.Add(prod2);
            prod2.Provider = entity1;
            //entity2.Products.Add(prod2);
            //prod2.Provider = entity2;

            entity1.Products.Add(prod3);
            prod3.Provider = entity1;
            //entity3.Products.Add(prod3);
            //prod3.Provider = entity3;

            Provider savedObject = null;
            try
            {
                dao.SaveOrUpdate(entity1);
                savedObject = getPersistentObject(entity1);
                Assert.IsNotNull(savedObject, "DAO method saveOrUpdate should return entity if successfull");
                checkAllPropertiesEqual(savedObject, entity1);
                entity1 = savedObject;
            }
            catch (Exception)
            {
                Assert.Fail("Fail to save entity1");
            }
            IList<Product> productList = providerDAO.getAllProductsOfProvider(entity1.Name);
            Assert.IsNotNull(productList, "List can't be null");
            Assert.IsTrue(productList.Count == 3, "Count of products in the list must be 3");
            checkAllPropertiesEqualForProduct(productList[0], prod1);
            checkAllPropertiesEqualForProduct(productList[1], prod2);
            checkAllPropertiesEqualForProduct(productList[2], prod3);
        }

        protected void createEntitiesForProvider()
        {
            prod1 = new Product();
            prod1.Name = "AMG E63";
            prod1.Price = 150000;
            prod1.Count = 2;

            prod2 = new Product();
            prod2.Name = "URUS";
            prod2.Price = 250000;
            prod2.Count = 1;

            prod3 = new Product();
            prod3.Name = "Intel Core i7 12900k";
            prod3.Price = 1100;
            prod3.Count = 22;
        }

        protected void checkAllPropertiesEqualForProduct(Product entityToCheck1, Product entityToCheck2)
        {
            Assert.AreEqual(entityToCheck1.Name, entityToCheck2.Name, "Product names must be equal");
            Assert.AreEqual(entityToCheck1.Count, entityToCheck2.Count, "Product count must be equal");
            Assert.AreEqual(entityToCheck1.Price, entityToCheck2.Price, "Product price must be equal");
        }
    }
}
