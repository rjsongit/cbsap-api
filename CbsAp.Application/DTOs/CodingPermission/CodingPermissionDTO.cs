namespace CbsAp.Application.DTOs.CodingPermission
{
    public class CodingPermissionDTO
    {
        public long? ID { get; set; }

        public long? EntityProfileID { get; set; }

        public long? RoleID { get; set; }

        public string? Category { get; set; }

        public string? NameCode { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }

        public bool OriginallyAssigned { get; set; }

        public bool Checked { get; set; }
    }
}
