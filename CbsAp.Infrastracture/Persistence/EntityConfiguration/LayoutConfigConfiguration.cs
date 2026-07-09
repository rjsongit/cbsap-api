using CbsAp.Domain.Entities.DimensionSetup;
using CbsAp.Domain.Entities.LayoutConfigs;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration
{
    public class LayoutConfigConfiguration : IEntityTypeConfiguration<LayoutConfig>
    {
        public void Configure(EntityTypeBuilder<LayoutConfig> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(LayoutConfig), "CBSAP");

            builder.HasKey(x => x.LayoutConfigId)
                   .HasAnnotation("SqlServer:Identity", "1, 1");

            builder.Property(x => x.Username)
                   .HasMaxLength(150)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(x => x.LayoutValue)
                   .HasColumnType("int");

        }
    }
}
