using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.domain
{
    public abstract class EntityBase
    {
        public virtual long Id { get; set; }
    }

    public class Provider: EntityBase
    {
        //list of products 
        private IList<Product> productList = new List<Product>();
        public virtual string Name { get; set; }
        public virtual string Country { get; set; }

        public virtual IList<Product> Products
        {
            get { return productList; }
            set { productList = value; }
        }

        // many orders
        private IList<Delivery> deliveriesList = new List<Delivery>();
        public virtual IList<Delivery> Deliveries
        {
            get { return deliveriesList; }
            set { deliveriesList = value; }
        }
    }

    public class Product : EntityBase
    { 
        public virtual string Name { get; set; }
        public virtual int Count { get; set; }
        public virtual double Price { get; set; }

        public virtual Provider Provider { get; set; }

        private IList<Delivery> deliveriesList = new List<Delivery>();
        public virtual IList<Delivery> Deliveries
        {
            get { return deliveriesList; }
            set { deliveriesList = value; }
        }
    }

    public class Delivery : EntityBase
    {
        public virtual string Date { get; set; }
        public virtual int quantity { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual Product Product { get; set; }
    }
}
