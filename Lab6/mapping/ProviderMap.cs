using FluentNHibernate.Mapping;

namespace lab6
{
    public class ProviderMap : ClassMap<Provider>
    {
        public ProviderMap()
        {
            Table("provider");
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.Name);
            Map(x => x.Country);
            HasMany(x => x.Products).Inverse().Cascade.All().KeyColumn("provider_id");
        }
       
    }

}
