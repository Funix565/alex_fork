using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;

namespace lab6
{
    public interface IDAO<T>
    {
        void SaveOrUpdate(T item);

        T GetById(long id);

        List<T> GetAll();

        void Delete(T item); 

    }

    public interface IProviderDAO : IDAO<Provider>
    {
        Provider GetProviderByName(string name);

        IList<Product> getAllProductsOfProvider(string provider_name);

        void RemoveProviderByName(string name);
    }

    public interface IProductDAO : IDAO<Product>
    {
        Product GetProductByProviderAndProductName(string provider_name, string product_name); 
    }

    abstract public class AbsDAOFactory
    {
        public abstract IProviderDAO GetProviderDAO();

        public abstract IProductDAO GetProductDAO();

    }

    public class GenericDAO<T>:IDAO<T>
    {
        protected ISession session;

        public GenericDAO() { }
        public GenericDAO(ISession session)
        {
            this.session = session;
        }

        public void SaveOrUpdate(T item)
        {
            ITransaction transaction = session.BeginTransaction();
            session.SaveOrUpdate(item);
            transaction.Commit();
        }

        public T GetById(long id)
        {
            return session.Get<T>(id);
        }

        public List<T> GetAll()
        {
            return new List<T>(session.CreateCriteria(typeof(T)).List<T>());
        }

        public void Delete(T item)
        {
            ITransaction transaction = session.BeginTransaction();
            session.Delete(item);
            transaction.Commit();
        }

    }
   
    public class ProviderDAO : GenericDAO<Provider>, IProviderDAO
    {
        public ProviderDAO(ISession session) : base(session) { }

        public IList<Product> getAllProductsOfProvider(string provider_name)
        {
            var prod_lst = session.CreateSQLQuery("select product.* from product join provider on " +
                "provider.id = product.provider_id where provider.name = '" + provider_name + "'").AddEntity("Product", typeof(Product)).List<Product>();
            return prod_lst;
        }

        public Provider GetProviderByName(string name)
        {
            Provider p = new Provider();
            p.Name = name;
            ICriteria criteria = session.CreateCriteria(typeof(Provider))
                .Add(Example.Create(p));
            IList<Provider> lst = criteria.List<Provider>();
            if (lst.Count > 0)
              return lst[0];
            return null;
        }

        public void RemoveProviderByName(string name)
        {
            Provider p = GetProviderByName(name);
            if (p != null)
                Delete(p);
        }
    }

    public class ProductDAO:GenericDAO<Product>, IProductDAO
    {
        public ProductDAO(ISession session) : base (session)
        { }

        public Product GetProductByProviderAndProductName(string provider_name, string product_name)
        {
            var lst = session.CreateSQLQuery("select product.* from product inner join provider " +
                "on provider.name = '" + provider_name + "' and product.name = '" + product_name + "'").
                AddEntity("Product", typeof(Product)).List<Product>();
            if (lst.Count > 0)
                return lst[0];
            return null; 
        }
    }


    public class FactoryDAO: AbsDAOFactory
    {
        protected ISession session = null; 

        public FactoryDAO(ISession session)
        {
            this.session = session;
        }
        public override IProviderDAO GetProviderDAO()
        {
            return new ProviderDAO(session);
        }

        public override IProductDAO GetProductDAO()
        {
            return new ProductDAO(session);
        }
    }

}
