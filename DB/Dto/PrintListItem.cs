namespace DB.Dto
{
    public class PrintListItem
    {
        public class Inventorymove
        {
            public long InventorymoveId { get; set; }
            public long MoveDetailId { get; set; }
            public string MaterialNo { get; set; }
            public string MaterialName { get; set; }
            public byte? Status { get; set; }
            public decimal? PlanQty { get; set; }
            public decimal? ActQty { get; set; }
            public string? Remark { get; set; }
            public DateTime? AuditinTime { get; set; }
            public string AName { get; set; }
            public string CName { get; set; }
            public string UName { get; set; }
            public DateTime? CreateDate { get; set; }
            public DateTime? ModifiedDate { get; set; }
        }

        public class StockIn
        {
            public long StockInId { get; set; }
            public long StockInDetailId { get; set; }
            public string MaterialNo { get; set; }
            public string MaterialName { get; set; }
            public byte? Status { get; set; }
            public decimal? PlanInQty { get; set; }
            public decimal? ActInQty { get; set; }
            public string? Remark { get; set; }
            public DateTime? AuditinTime { get; set; }
            public string AName { get; set; }
            public string CName { get; set; }
            public string UName { get; set; }
            public DateTime? CreateDate { get; set; }
            public DateTime? ModifiedDate { get; set; }
        }
        public class StockOut
        {
            public long StockOutId { get; set; }
            public long StockOutDetailId { get; set; }
            public string MaterialNo { get; set; }
            public string MaterialName { get; set; }
            public byte? Status { get; set; }
            public decimal? PlanInQty { get; set; }
            public decimal? ActInQty { get; set; }
            public string? Remark { get; set; }
            public DateTime? AuditinTime { get; set; }
            public string AName { get; set; }
            public string CName { get; set; }
            public string UName { get; set; }
            public DateTime? CreateDate { get; set; }
            public DateTime? ModifiedDate { get; set; }
        }
    }
}
