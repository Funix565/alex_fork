using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;
using NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using lab6; 

namespace Lab6Tests
{
  [TestClass()]
  public abstract class TestProductDAO : TestGenericDAO<Product>
  {
    protected IProductDAO productDAO = null;
    protected IProviderDAO providerDAO = null;
    protected Provider p = null;

    public TestProductDAO() : base()
    {
      AbsDAOFactory daoFactory = new FactoryDAO(session);
      productDAO = daoFactory.GetProductDAO();
      providerDAO = daoFactory.GetProviderDAO();
      setDAO(productDAO);
    }

    protected override void createEntities()
    {
      entity1 = new Product();
      entity1.Name = "ProdName1";
      entity1.Count = 12;
      entity1.Price = 888;

      entity2 = new Product();
      entity2.Name = "ProdName2";
      entity2.Count = 15;
      entity2.Price = 111;

      entity3 = new Product();
      entity3.Name = "ProdName3";
      entity3.Count = 24;
      entity3.Price = 222;
    }

    protected override void checkAllPropertiesDiffer(Product entityToCheck1, Product entityToCheck2)
    {
      Assert.AreNotEqual(entityToCheck1.Name, entityToCheck2.Name, "Product names must be different");
      Assert.AreNotEqual(entityToCheck1.Count, entityToCheck2.Count, "Product count must be different");
      Assert.AreNotEqual(entityToCheck1.Price, entityToCheck2.Price, "Product price must be different");
    }

    protected override void checkAllPropertiesEqual(Product entityToCheck1, Product entityToCheck2)
    {
      Assert.AreEqual(entityToCheck1.Name, entityToCheck2.Name, "Product names must be different");
      Assert.AreEqual(entityToCheck1.Count, entityToCheck2.Count, "Product count must be different");
      Assert.AreEqual(entityToCheck1.Price, entityToCheck2.Price, "Product price must be different");
    }

    [TestMethod]
    public void TestGetByIdProduct()
    {
      base.TestGetByIdGeneric();
    }
    [TestMethod]
    public void TestGetAllProducts()
    {
      base.TestGetAllGeneric();
    }
    [TestMethod]
    public void TestDeleteProduct()
    {
      base.TestDeleteGeneric();
    }

    [TestMethod]
    public void TestGetStudentByGroupFirstNameAndLastName()
    {
      p = new Provider();
      p.Name = "Provider name";
      p.Country = "Provider country";
      p.Products.Add(entity1);
      p.Products.Add(entity2);
      p.Products.Add(entity3);
      entity1.Provider = p;
      entity2.Provider = p;
      entity3.Provider = p;

      Provider savedProvider = null;
      try
      {
        providerDAO.SaveOrUpdate(p);
        savedProvider = getPersistentProvider(p);
        Assert.IsNotNull(savedProvider, "DAO method saveOrUpdate should return group if successfull");
        checkAllPropertiesEqualProvider(savedProvider, p);
        p = savedProvider;
      }
      catch (Exception)
      {
        Assert.Fail("Fail to save provider");
      }
      getProductByProviderNameAndProductName(entity1, p.Name, entity1.Name);
      getProductByProviderNameAndProductName(entity2, p.Name, entity2.Name);
      getProductByProviderNameAndProductName(entity3, p.Name, entity3.Name);
      p.Products.Remove(entity1);
      p.Products.Remove(entity2);
      p.Products.Remove(entity3);
      entity1.Provider = null;
      entity2.Provider = null;
      entity3.Provider = null;
      providerDAO.Delete(p);
    }

    protected void getProductByProviderNameAndProductName(Product prod, string providerName, string prodName)
    {
      Product foundProduct = null;
      try
      {
        foundProduct = productDAO.GetProductByProviderAndProductName(providerName, prodName);
        Assert.IsNotNull(foundProduct, "Service method should return student if successfull");
        checkAllPropertiesEqual(foundProduct, prod);
      }
      catch (Exception)
      {
        Assert.Fail("Failed to get product with provider name" + providerName + " and product name " + prodName);
      }
    }

    protected Provider getPersistentProvider(Provider nonPersistentPRovider)
    {
      ICriteria criteria = session.CreateCriteria(typeof(Provider)).Add(Example.Create(nonPersistentPRovider));
      IList<Provider> list = criteria.List<Provider>();
      Assert.IsTrue(list.Count >= 1, "Count of providers must be equal or more than 1");
      return list[0];
    }

    protected void checkAllPropertiesEqualProvider(Provider entityToCheck1, Provider entityToCheck2)
    {
      Assert.AreEqual(entityToCheck1.Name, entityToCheck2.Name, "Provider names must be equal");
      Assert.AreEqual(entityToCheck1.Country, entityToCheck2.Country, "Provider countries must be equal");
    }

  }
}
