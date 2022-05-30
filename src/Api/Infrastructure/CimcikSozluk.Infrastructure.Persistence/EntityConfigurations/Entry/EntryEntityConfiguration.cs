using CimcikSozluk.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CimcikSozluk.Infrastructure.Persistence.EntityConfigurations.Entry;

public class EntryEntityConfiguration : BaseEntityConfiguration<Api.Domain.Models.Entry>
{
    public override void Configure(EntityTypeBuilder<Api.Domain.Models.Entry> builder)
    {
        base.Configure(builder);
        builder.ToTable("entry", CimcikSozlukContext.DEFAULT_SCHEMA);

        builder.HasOne(i => i.CreatedBy).WithMany(i => i.Entries).HasForeignKey(i => i.CreatedById);
    }
}