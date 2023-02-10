

namespace Intus.Data.Mapping
{
    public class WindowMap : IEntityTypeConfiguration<Window>
    {
        public void Configure(EntityTypeBuilder<Window> builder)
        {
            builder.ToTable(DatabaseTableName.Window);

            builder.HasKey(w => w.Id);
            builder.Property(w => w.Name).IsRequired();
            builder.Property(p => p.CreateDate).IsRequired();
        }
    }
}
