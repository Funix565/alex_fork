using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using WindowsFormsApp1.domain;

namespace WindowsFormsApp1.mapping
{
    public class ProductMap : ClassMap<Product>
    {
        ProductMap()
        {
            Table("product");
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.Name);
            Map(x => x.Count);
            Map(x => x.Price);
            References(x => x.Provider, "provider_id");
        }
    }
}
