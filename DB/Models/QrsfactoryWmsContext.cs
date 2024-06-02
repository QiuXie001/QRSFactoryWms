using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace DB.Models;

public partial class QrsfactoryWmsContext : DbContext 
{
    public QrsfactoryWmsContext()
    {
    }

    public QrsfactoryWmsContext(DbContextOptions<QrsfactoryWmsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SysDept> SysDepts { get; set; }

    public virtual DbSet<SysDict> SysDicts { get; set; }

    public virtual DbSet<SysLog> SysLogs { get; set; }
    public virtual DbSet<SysIdentity> SysIdentity { get; set; }

    public virtual DbSet<SysMenu> SysMenu { get; set; }

    public virtual DbSet<SysRole> SysRoles { get; set; }

    public virtual DbSet<SysRoleMenu> SysRolemenus { get; set; }

    public virtual DbSet<SysSerialNum> SysSerialnums { get; set; }

    public virtual DbSet<SysUser> SysUsers { get; set; }

    public virtual DbSet<WmsDelivery> WmsDeliveries { get; set; }

    public virtual DbSet<WmsInventory> WmsInventories { get; set; }

    public virtual DbSet<WmsInventorymove> WmsInventorymoves { get; set; }

    public virtual DbSet<WmsInventoryrecord> WmsInventoryrecords { get; set; }

    public virtual DbSet<WmsInvmovedetail> WmsInvmovedetails { get; set; }

    public virtual DbSet<WmsMaterial> WmsMaterials { get; set; }

    public virtual DbSet<WmsReservoirarea> WmsReservoirareas { get; set; }

    public virtual DbSet<WmsStockin> WmsStockins { get; set; }

    public virtual DbSet<WmsStockindetail> WmsStockindetails { get; set; }

    public virtual DbSet<WmsStockout> WmsStockouts { get; set; }

    public virtual DbSet<WmsStockoutdetail> WmsStockoutdetails { get; set; }

    public virtual DbSet<WmsStoragerack> WmsStorageracks { get; set; }

    public virtual DbSet<WmsWarehouse> WmsWarehouses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("data source=110.40.171.227;user id=sa;password=!8432CE8AF88D31A4E85FC68607DDECF5;database=QRSFactoryWms;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SysDept>(entity =>
        {
            entity.HasKey(e => e.DeptId).HasName("PK__sys_dept__014881AEB4436F97");

            modelBuilder.Entity<SysDept>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateDepts) // 假设 SysUser 类有一个名为 CreatedDepts 的集合属性
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<SysDept>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedDepts) // 假设 SysUser 类有一个名为 ModifiedDepts 的集合属性
        .HasForeignKey(d => d.ModifiedBy);

            entity.ToTable("sys_dept");

            entity.Property(e => e.DeptId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DeptName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DeptNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Remark)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SysDict>(entity =>
        {
            entity.HasKey(e => e.DictId).HasName("PK__sys_dict__9882F3F0042501EC");

            entity.ToTable("sys_dict");

            entity.Property(e => e.DictId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DictName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DictNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DictType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Remark)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SysLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__sys_log__5E5486487BCA3D3A");

            entity.ToTable("sys_log");

            entity.Property(e => e.LogId).ValueGeneratedNever();
            entity.Property(e => e.Browser)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.LogIp)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.LogType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Url)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SysMenu>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PK__sys_menu__C99ED2309A83DC78");

            entity.ToTable("sys_menu_wms");

            entity
                .HasOne(rm => rm.CreateByUser)
                .WithMany(u => u.CreateMenus)
                .HasForeignKey(rm => rm.CreateBy);

            entity
                .HasOne(rm => rm.ModifiedByUser)
                .WithMany(u => u.ModifiedMenus)
                .HasForeignKey(rm => rm.ModifiedBy);

            entity.Property(e => e.MenuId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.MenuIcon)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MenuName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MenuType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MenuUrl)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Remark)
                .HasMaxLength(255)
                .IsUnicode(false);
        });
        
        modelBuilder.Entity<SysRole>(entity =>
        {
            entity.HasKey(r => r.RoleId);
            entity.ToTable("sys_role");

            entity
                .HasOne(rm => rm.CreateByUser)
                .WithMany(u => u.CreateRoles)
                .HasForeignKey(rm => rm.CreateBy);

            entity
                .HasOne(rm => rm.ModifiedByUser)
                .WithMany(u => u.ModifiedRoles)
                .HasForeignKey(rm => rm.ModifiedBy);

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Remark)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleType)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SysRoleMenu>(entity =>
        {
            entity.HasKey(e => e.RoleMenuId).HasName("PK__sys_role__F86287B6C7F29634");

            entity.ToTable("sys_rolemenu");


            entity
                .HasOne(rm => rm.Menu)
                .WithMany(m => m.RoleMenus) 
                .HasForeignKey(rm => rm.MenuId);

            entity
                .HasOne(rm => rm.Role)
                .WithMany(r => r.RoleMenus) 
                .HasForeignKey(rm => rm.RoleId);

            entity
                .HasOne(rm => rm.CreateByUser)
                .WithMany(u => u.CreateRoleMenus) 
                .HasForeignKey(rm => rm.CreateBy);

            entity
                .HasOne(rm => rm.ModifiedByUser)
                .WithMany(u => u.ModifiedRoleMenus) 
                .HasForeignKey(rm => rm.ModifiedBy);


            entity.Property(e => e.RoleMenuId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<SysSerialNum>(entity =>
        {
            entity.HasKey(e => e.SerialNumberId).HasName("PK__sys_seri__BA34B056B95E0E90");

            entity.ToTable("sys_serialnum");

            entity.Property(e => e.SerialNumberId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Prefix)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Remark)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SerialNumber)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.TableName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SysIdentity>(entity =>
        {
            entity.ToTable("sys_identity");
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Token)
                .HasColumnType("VARCHAR(255)");

            entity.Property(p => p.GeneratedTime)
                .HasColumnType("DATETIME");

            entity.Property(p => p.ExpirationTime)
                .HasColumnType("DATETIME");

            entity.Property(p => p.UserId)
                .HasColumnType("BIGINT");

            entity.HasOne(p => p.User)
                .WithMany(p => p.Identities)
                .HasForeignKey(p => p.UserId);

        });
            modelBuilder.Entity<SysUser>(entity =>
        {
            entity
        .HasOne(u => u.Role)
        .WithMany(r => r.Users) // 假设 SysRole 类有一个名为 Users 的集合属性
        .HasForeignKey(u => u.RoleId);

            entity
        .HasOne(s => s.CreateByUser)
        .WithMany()
        .HasForeignKey(s => s.CreateBy)
        .OnDelete(DeleteBehavior.Restrict);

            entity
        .HasOne(s => s.ModifiedByUser)
        .WithMany()
        .HasForeignKey(s => s.ModifiedBy)
        .OnDelete(DeleteBehavior.Restrict);

            entity.HasKey(e => e.UserId).HasName("PK__sys_user__1788CC4CA4A660F0");

            entity.ToTable("sys_user");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HeadImg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LoginDate).HasColumnType("datetime");
            entity.Property(e => e.LoginIp)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Pwd)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.Remark)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Sort)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Tel)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserNickname)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<WmsDelivery>(entity =>
        {
            entity.HasKey(e => e.DeliveryId).HasName("PK__wms_deli__626D8FCE25F89DD0");

            entity.ToTable("wms_delivery");

            entity.Property(e => e.DeliveryId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Remark).HasMaxLength(255);
            entity.Property(e => e.TrackingNo).HasMaxLength(50);
        });

        modelBuilder.Entity<WmsInventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("PK__wms_inve__F5FDE6B3B72ABCDB");

            entity.ToTable("wms_inventory");

            entity.Property(e => e.InventoryId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Qty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Remark).HasMaxLength(255);
        });

        modelBuilder.Entity<WmsInventorymove>(entity =>
        {
            entity.HasKey(e => e.InventorymoveId).HasName("PK__wms_inve__A1254B8DE9BF5023");

            entity.ToTable("wms_inventorymove");

            entity.Property(e => e.InventorymoveId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.InventorymoveNo).HasMaxLength(30);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Remark).HasMaxLength(255);
        });

        modelBuilder.Entity<WmsInventoryrecord>(entity =>
        {
            entity.HasKey(e => e.InventoryrecordId).HasName("PK__wms_inve__E7FFA5BBC5050555");

            entity.ToTable("wms_inventoryrecord");

            entity.Property(e => e.InventoryrecordId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Qty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Remark).HasMaxLength(255);
        });

        modelBuilder.Entity<WmsInvmovedetail>(entity =>
        {
            entity.HasKey(e => e.MoveDetailId).HasName("PK__wms_invm__7A0D92B1ADB659DB");

            entity.ToTable("wms_invmovedetail");

            entity.Property(e => e.MoveDetailId).ValueGeneratedNever();
            entity.Property(e => e.ActQty).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.AuditinTime).HasColumnType("datetime");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PlanQty).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Remark).HasMaxLength(255);
        });

        modelBuilder.Entity<WmsMaterial>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("PK__wms_mate__C50610F77CDB3140");

            entity.ToTable("wms_material");

            entity.Property(e => e.MaterialId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ExpiryDate).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.MaterialName).HasMaxLength(60);
            entity.Property(e => e.MaterialNo).HasMaxLength(20);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Qty).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Remark).HasMaxLength(255);
        });

        modelBuilder.Entity<WmsReservoirarea>(entity =>
        {
            entity.HasKey(e => e.ReservoirAreaId).HasName("PK__wms_rese__0CB017225BB1C64C");

            entity.ToTable("wms_reservoirarea");

            entity.Property(e => e.ReservoirAreaId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Remark).HasMaxLength(255);
            entity.Property(e => e.ReservoirAreaName).HasMaxLength(60);
            entity.Property(e => e.ReservoirAreaNo).HasMaxLength(20);
        });

        modelBuilder.Entity<WmsStockin>(entity =>
        {
            entity.HasKey(e => e.StockInId).HasName("PK__wms_stoc__794DA66C4C6A2224");

            entity.ToTable("wms_stockin");

            entity.Property(e => e.StockInId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.OrderNo).HasMaxLength(32);
            entity.Property(e => e.Remark).HasMaxLength(255);
            entity.Property(e => e.StockInNo).HasMaxLength(32);
        });

        modelBuilder.Entity<WmsStockindetail>(entity =>
        {
            entity.HasKey(e => e.StockInDetailId).HasName("PK__wms_stoc__EEDA1013E1B7D908");

            entity.ToTable("wms_stockindetail");

            entity.Property(e => e.StockInDetailId).ValueGeneratedNever();
            entity.Property(e => e.ActInQty).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.AuditinTime).HasColumnType("datetime");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PlanInQty).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Remark).HasMaxLength(255);
        });

        modelBuilder.Entity<WmsStockout>(entity =>
        {
            entity.HasKey(e => e.StockOutId).HasName("PK__wms_stoc__C5308D7A76CDE9E0");

            entity.ToTable("wms_stockout");

            entity.Property(e => e.StockOutId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.OrderNo).HasMaxLength(50);
            entity.Property(e => e.Remark).HasMaxLength(255);
            entity.Property(e => e.StockOutNo).HasMaxLength(22);
        });

        modelBuilder.Entity<WmsStockoutdetail>(entity =>
        {
            entity.HasKey(e => e.StockOutDetailId).HasName("PK__wms_stoc__EB248E9F733D0330");

            entity.ToTable("wms_stockoutdetail");

            entity.Property(e => e.StockOutDetailId).ValueGeneratedNever();
            entity.Property(e => e.ActOutQty).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.AuditinTime).HasColumnType("datetime");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PlanOutQty).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Remark).HasMaxLength(255);
        });

        modelBuilder.Entity<WmsStoragerack>(entity =>
        {
            entity.HasKey(e => e.StorageRackId).HasName("PK__wms_stor__243CF6DE580F4A87");

            entity.ToTable("wms_storagerack");

            entity.Property(e => e.StorageRackId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Remark).HasMaxLength(255);
            entity.Property(e => e.StorageRackName).HasMaxLength(60);
            entity.Property(e => e.StorageRackNo).HasMaxLength(20);
        });

        modelBuilder.Entity<WmsWarehouse>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("PK__wms_ware__2608AFF9054CEB83");

            entity.ToTable("wms_warehouse");

            entity.Property(e => e.WarehouseId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Remark).HasMaxLength(255);
            entity.Property(e => e.WarehouseName).HasMaxLength(50);
            entity.Property(e => e.WarehouseNo).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
