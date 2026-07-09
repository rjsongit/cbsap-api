using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.Invoicing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Domain.Entities.LayoutConfigs
{
    public class LayoutConfig : BaseAuditableEntity
    {
        public long LayoutConfigId { get; set; }
        public string? Username { get; set; }
        public int LayoutValue { get; set; }
    }
}
