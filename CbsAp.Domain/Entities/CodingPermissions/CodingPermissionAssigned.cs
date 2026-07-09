using CbsAp.Domain.Common;

namespace CbsAp.Domain.Entities.CodingPermissions
{
    public class CodingPermissionAssigned : BaseAuditableEntity
    {
        public long ID { get; set; }

        public string? NameCode { get; set; }

        public long? EntityProfileID { get; set; }

        public long? RoleID { get; set; }

        public string? Category { get; set; }

        public bool IsAssigned { get; set; }
    }
}
