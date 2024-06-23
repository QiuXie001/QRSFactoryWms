namespace DB.Dto
{
    public class UserDto
    {
        public long UserId { get; set; }

        public string UserName { get; set; }

        public string UserNickname { get; set; }

        public string Pwd { get; set; }

        public string Sort { get; set; }

        public string Email { get; set; }

        public string Tel { get; set; }

        public string Mobile { get; set; }

        public byte Sex { get; set; }

        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public long DeptId { get; set; }
        public string DeptName { get; set; }

        public string LoginIp { get; set; }

        public DateTime LoginDate { get; set; }
        public string HeadImg { get; set; }

        public byte IsEabled { get; set; }
        public byte IsDel { get; set; }
        public string Remark { get; set; }
        public long CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public long ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }
}
