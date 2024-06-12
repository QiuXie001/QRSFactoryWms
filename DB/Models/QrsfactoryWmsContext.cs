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

    public virtual DbSet<SysIdentity> SysIdentities { get; set; }

    public virtual DbSet<SysLog> SysLogs { get; set; }

    public virtual DbSet<SysMenu> SysMenu { get; set; }

    public virtual DbSet<SysRole> SysRoles { get; set; }

    public virtual DbSet<SysRolemenu> SysRolemenus { get; set; }

    public virtual DbSet<SysSerialnum> SysSerialnums { get; set; }

    public virtual DbSet<SysUser> SysUsers { get; set; }

    public virtual DbSet<WmsCarrier> WmsCarriers { get; set; }

    public virtual DbSet<WmsCustomer> WmsCustomers { get; set; }

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

    public virtual DbSet<WmsSupplier> WmsSuppliers { get; set; }

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

            modelBuilder.Entity<SysDict>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateDicts) 
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<SysDict>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedDicts) 
        .HasForeignKey(d => d.ModifiedBy);

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

        modelBuilder.Entity<SysIdentity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sys_Iden__3214EC070A4E4347");

            modelBuilder.Entity<SysIdentity>()
        .HasOne(d => d.User)
        .WithMany(u => u.Identities)
        .HasForeignKey(d => d.UserId);

            entity.ToTable("Sys_Identity");

            entity.Property(e => e.ExpirationTime).HasColumnType("datetime");
            entity.Property(e => e.GeneratedTime).HasColumnType("datetime");
            entity.Property(e => e.LoginIp)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SysLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__sys_log__5E5486487BCA3D3A");

            modelBuilder.Entity<SysLog>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateLogs)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<SysLog>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedLogs)
        .HasForeignKey(d => d.ModifiedBy);

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

            modelBuilder.Entity<SysMenu>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateMenus)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<SysMenu>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedMenus)
        .HasForeignKey(d => d.ModifiedBy);

            entity.ToTable("sys_menu_wms");

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
            entity.HasKey(e => e.RoleId);

            modelBuilder.Entity<SysRole>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateRoles)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<SysRole>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedRoles)
        .HasForeignKey(d => d.ModifiedBy);

            entity.ToTable("sys_role");

            entity.Property(e => e.RoleId).ValueGeneratedNever();
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

        modelBuilder.Entity<SysRolemenu>(entity =>
        {
            entity.HasKey(e => e.RoleMenuId).HasName("PK__sys_role__F86287B6C7F29634");

            modelBuilder.Entity<SysRolemenu>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateRolemenus)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<SysRolemenu>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedRolemenus)
        .HasForeignKey(d => d.ModifiedBy);

            modelBuilder.Entity<SysRolemenu>()
        .HasOne(d => d.Role)
        .WithMany(u => u.Rolemenus)
        .HasForeignKey(d => d.RoleId);

            modelBuilder.Entity<SysRolemenu>()
        .HasOne(d => d.Menu)
        .WithMany(u => u.Rolemenus)
        .HasForeignKey(d => d.MenuId);

            entity.ToTable("sys_rolemenu");

            entity.Property(e => e.RoleMenuId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<SysSerialnum>(entity =>
        {
            entity.HasKey(e => e.SerialNumberId).HasName("PK__sys_seri__BA34B056B95E0E90");

            modelBuilder.Entity<SysSerialnum>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateSerialnums)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<SysSerialnum>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedSerialnums)
        .HasForeignKey(d => d.ModifiedBy);

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

        modelBuilder.Entity<SysUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__sys_user__1788CC4CA4A660F0");

            modelBuilder.Entity<SysUser>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateUsers)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<SysUser>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedUsers)
        .HasForeignKey(d => d.ModifiedBy);

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

        modelBuilder.Entity<WmsCarrier>(entity =>
        {
            entity.HasKey(e => e.CarrierId).HasName("PK__wms_carr__CB820559D5F1A410");

            modelBuilder.Entity<WmsCarrier>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateCarriers)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<WmsCarrier>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedCarriers)
        .HasForeignKey(d => d.ModifiedBy);

            entity.ToTable("wms_carrier");

            entity.Property(e => e.CarrierId).ValueGeneratedNever();
            entity.Property(e => e.Address)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.CarrierLevel)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CarrierName)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.CarrierNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CarrierPerson)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Remark)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Tel)
                .HasMaxLength(13)
                .IsUnicode(false);
        });

        modelBuilder.Entity<WmsCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__wms_cust__A4AE64D8688F83E1");

            modelBuilder.Entity<WmsCustomer>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateCustomers)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<WmsCustomer>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedCustomers)
        .HasForeignKey(d => d.ModifiedBy);

            entity.ToTable("wms_customer");

            entity.Property(e => e.CustomerId).ValueGeneratedNever();
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerLevel)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.CustomerNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CustomerPerson)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Remark)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Tel)
                .HasMaxLength(13)
                .IsUnicode(false);
        });

        modelBuilder.Entity<WmsDelivery>(entity =>
        {
            entity.HasKey(e => e.DeliveryId).HasName("PK__wms_deli__626D8FCE25F89DD0");

            modelBuilder.Entity<WmsDelivery>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateDeliveries)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<WmsDelivery>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedDeliveries)
        .HasForeignKey(d => d.ModifiedBy);

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

            modelBuilder.Entity<WmsInventory>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateInventories)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<WmsInventory>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedInventories)
        .HasForeignKey(d => d.ModifiedBy);

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

            modelBuilder.Entity<WmsInventorymove>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateInventorymoves)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<WmsInventorymove>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedInventorymoves)
        .HasForeignKey(d => d.ModifiedBy);

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

            modelBuilder.Entity<WmsInventoryrecord>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateInventoryrecords)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<WmsInventoryrecord>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedInventoryrecords)
        .HasForeignKey(d => d.ModifiedBy);

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

            modelBuilder.Entity<WmsInvmovedetail>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateInvmovedetails)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<WmsInvmovedetail>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedInvmovedetails)
        .HasForeignKey(d => d.ModifiedBy);

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

            modelBuilder.Entity<WmsMaterial>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateMaterials)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<WmsMaterial>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedMaterials)
        .HasForeignKey(d => d.ModifiedBy);

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

            modelBuilder.Entity<WmsReservoirarea>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateReservorirareas)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<WmsReservoirarea>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedReservorirareas)
        .HasForeignKey(d => d.ModifiedBy);

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

            modelBuilder.Entity<WmsStockin>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateStockins)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<WmsStockin>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedStockins)
        .HasForeignKey(d => d.ModifiedBy);

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

            modelBuilder.Entity<WmsStockindetail>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateStockindetails)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<WmsStockindetail>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedStockindetails)
        .HasForeignKey(d => d.ModifiedBy);

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

            modelBuilder.Entity<WmsStockout>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateStockouts)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<WmsStockout>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedStockouts)
        .HasForeignKey(d => d.ModifiedBy);

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

            modelBuilder.Entity<WmsStockoutdetail>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateStockoutdetails)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<WmsStockoutdetail>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedStockoutdetails)
        .HasForeignKey(d => d.ModifiedBy);

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

            modelBuilder.Entity<WmsStoragerack>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateStorageracks)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<WmsStoragerack>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedStorageracks)
        .HasForeignKey(d => d.ModifiedBy);

            entity.ToTable("wms_storagerack");

            entity.Property(e => e.StorageRackId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Remark).HasMaxLength(255);
            entity.Property(e => e.StorageRackName).HasMaxLength(60);
            entity.Property(e => e.StorageRackNo).HasMaxLength(20);
        });

        modelBuilder.Entity<WmsSupplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__wms_supp__4BE666B49E2B0439");

            modelBuilder.Entity<WmsSupplier>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateSuppliers)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<WmsSupplier>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedSuppliers)
        .HasForeignKey(d => d.ModifiedBy);

            entity.ToTable("wms_supplier");

            entity.Property(e => e.SupplierId).ValueGeneratedNever();
            entity.Property(e => e.Address)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Remark)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SupplierLevel)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.SupplierName)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.SupplierNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SupplierPerson)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Tel)
                .HasMaxLength(13)
                .IsUnicode(false);
        });

        modelBuilder.Entity<WmsWarehouse>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("PK__wms_ware__2608AFF9054CEB83");

            modelBuilder.Entity<WmsWarehouse>()
        .HasOne(d => d.CreateByUser)
        .WithMany(u => u.CreateWarehouses)
        .HasForeignKey(d => d.CreateBy);

            modelBuilder.Entity<WmsWarehouse>()
        .HasOne(d => d.ModifiedByUser)
        .WithMany(u => u.ModifiedWarehouses)
        .HasForeignKey(d => d.ModifiedBy);

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
