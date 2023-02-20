namespace Intus.Data.Mapping
{
    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(DatabaseTableName.Order);

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.State).IsRequired();
            builder.Property(p => p.CreateDate).IsRequired();

            builder.HasMany(p => p.OrderWindows)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
