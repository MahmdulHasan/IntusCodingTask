using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intus.Data.Mapping
{
    public class OrderWindowMap : IEntityTypeConfiguration<OrderWindow>
    {
        public void Configure(EntityTypeBuilder<OrderWindow> builder)
        {
            builder.ToTable(DatabaseTableName.OrderWindow);

            builder.HasKey(ow => ow.Id);
            builder.Property(p => p.CreateDate).IsRequired();

            builder.HasOne(ow => ow.Window)
            .WithMany(w => w.OrderWindows)
            .HasForeignKey(ow => ow.WindowId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(ow => ow.OrderSubElements)
                .WithOne(ose => ose.OrderWindow)
                .HasForeignKey(ose => ose.OrderWindowId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
