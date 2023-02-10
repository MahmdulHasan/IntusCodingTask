

namespace Intus.Data.Mapping
{
    public class ElementMap : IEntityTypeConfiguration<Element>
    {
        public void Configure(EntityTypeBuilder<Element> builder)
        {
            builder.ToTable(DatabaseTableName.Element);

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Type).IsRequired();
            builder.Property(p => p.CreateDate).IsRequired();
        }
    }
}
