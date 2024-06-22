namespace DB.Dto
{
    public class RoleDto
    {
        public long RoleId { get; set; }
        public string RoleType { get; set; }
        public string RoleName { get; set; }
        public string Remark { get; set; }
        public byte IsDel { get; set; }

        public long? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
