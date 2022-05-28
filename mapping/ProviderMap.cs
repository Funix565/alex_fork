using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using WindowsFormsApp1.domain;
//using FluentNHibernate.;


namespace WindowsFormsApp1.mapping
{
    public class ProviderMap : ClassMap<Provider>
    {
        public ProviderMap()
        {
            Table("provider");
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.Name);
            Map(x => x.Country);
            HasMany(x => x.Products).Inverse().Cascade.All().Table("product");
            HasMany(x => x.Deliveries).Inverse().Cascade.All().Table("delivery");
        }
       
    }

}
