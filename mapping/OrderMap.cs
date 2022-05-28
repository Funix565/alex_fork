using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using WindowsFormsApp1.domain;

namespace WindowsFormsApp1.mapping
{
    public class OrderMap : ClassMap<Delivery>
    {
        public OrderMap()
        {
            Table("delivery");
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.Date);
            Map(x => x.quantity);
            References(x => x.Provider, "provider_id");
            References(x => x.Product, "product_id");
        }
    }
}
