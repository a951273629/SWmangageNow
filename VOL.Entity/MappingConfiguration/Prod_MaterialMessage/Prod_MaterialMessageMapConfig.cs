using VOL.Entity.MappingConfiguration;
using VOL.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VOL.Entity.MappingConfiguration
{
    public class Prod_MaterialMessageMapConfig : EntityMappingConfiguration<Prod_MaterialMessage>
    {
        public override void Map(EntityTypeBuilder<Prod_MaterialMessage>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

