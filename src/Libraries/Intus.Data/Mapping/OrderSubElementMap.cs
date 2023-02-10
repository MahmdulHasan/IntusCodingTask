using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intus.Data.Mapping
{
    public class OrderSubElementMap : IEntityTypeConfiguration<OrderSubElement>
    {
        public void Configure(EntityTypeBuilder<OrderSubElement> builder)
        {
            builder.ToTable(DatabaseTableName.OrderSubElement);

            builder.HasKey(ose => ose.Id);
            builder.Property(p => p.CreateDate).IsRequired();

            builder.HasOne(ose => ose.Element)
                .WithMany(e => e.OrderSubElements)
                .HasForeignKey(ose => ose.ElementId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
