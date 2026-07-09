
using CbsAp.Domain.Entities.CodingPermissions;
using CbsAp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.CodingPermissions
{
    public class CodingPermissionAssignedConfiguration : IEntityTypeConfiguration<CodingPermissionAssigned>
    {
        public void Configure(EntityTypeBuilder<CodingPermissionAssigned> builder)
        {
            builder.ToTable("CodingPermissionAssigned", "CBSAP");
            builder.HasKey(e => e.ID);
            builder.Property(e => e.ID).HasColumnName("ID").ValueGeneratedOnAdd();
            // map other properties as needed

            builder
                .Property(p => p.EntityProfileID);


            builder
                .Property(p => p.Category)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder
                .Property(p => p.NameCode)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder
                .Property(p => p.IsAssigned);
        }
    }
}