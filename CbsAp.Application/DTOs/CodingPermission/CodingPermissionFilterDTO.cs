
namespace CbsAp.Application.DTOs.CodingPermission
{
    public class CodingPermissionFilterDTO
    {
        public long EntityProfileID { get; set; }

        public string Category { get; set; } = string.Empty;

        public string NameCode { get; set; } = string.Empty;

        public bool IsAssigned { get; set; } = false;

        public bool IsUnassigned { get; set; } = false;
    }
}