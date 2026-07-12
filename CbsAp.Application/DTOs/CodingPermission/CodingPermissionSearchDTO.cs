namespace CbsAp.Application.DTOs.CodingPermission
{
    public class CodingPermissionSearchDTO
    {
        public long EntityProfileID { get; set; }

        public long RoleID { get; set; }

        public string Category { get; set; } = string.Empty;

        #region Paging

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string? SortField { get; set; }

        public int? SortOrder { get; set; }

        #endregion
    }
}
