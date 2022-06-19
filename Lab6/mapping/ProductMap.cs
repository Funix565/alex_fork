using FluentNHibernate.Mapping;

namespace lab6
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
