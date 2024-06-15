using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DB.Models;

public class SysUser
{
    [Key]
    public long UserId { get; set; }

    [StringLength(50)]
    public string UserName { get; set; }

    [StringLength(50)]
    public string UserNickname { get; set; }

    [StringLength(32)]
    public string Pwd { get; set; }

    public string Sort { get; set; }

    [StringLength(50)]
    public string Email { get; set; }

    [StringLength(20)]
    public string Tel { get; set; }

    [StringLength(12)]
    public string Mobile { get; set; }

    public byte Sex { get; set; }

    public long RoleId { get; set; }
    public long DeptId { get; set; }

    [StringLength(15)]
    public string LoginIp { get; set; }

    public DateTime LoginDate { get; set; }

    public int LoginTime { get; set; }

    public string HeadImg { get; set; }

    public byte IsEabled { get; set; }

    [Required]
    public byte IsDel { get; set; }

    [StringLength(255)]
    public string Remark { get; set; }

    public long CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public long ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
    [ForeignKey("DeptId")]
    public virtual SysDept Dept { get; set; }
    [ForeignKey("RoleId")]
    public virtual SysRole Role { get; set; }

    [ForeignKey("CreateBy")]
    public virtual SysUser CreateByUser { get; set; }

    [ForeignKey("ModifiedBy")]
    public virtual SysUser ModifiedByUser { get; set; }


    //Sys
    public virtual ICollection<SysUser> CreateUsers { get; set; } = new List<SysUser>();
    public virtual ICollection<SysUser> ModifiedUsers { get; set; } = new List<SysUser>();
    public virtual ICollection<SysDept> CreateDepts { get; set; } = new List<SysDept>();
    public virtual ICollection<SysDept> ModifiedDepts { get; set; } = new List<SysDept>();
    public virtual ICollection<SysDict> CreateDicts { get; set; } = new List<SysDict>();
    public virtual ICollection<SysDict> ModifiedDicts { get; set; } = new List<SysDict>();
    public virtual ICollection<SysLog> CreateLogs { get; set; } = new List<SysLog>();
    public virtual ICollection<SysLog> ModifiedLogs { get; set; } = new List<SysLog>();
    public virtual ICollection<SysRole> CreateRoles { get; set; } = new LinkedList<SysRole>();
    public virtual ICollection<SysRole> ModifiedRoles { get; set; } = new LinkedList<SysRole>();
    public virtual ICollection<SysMenu> CreateMenus { get; set; } = new LinkedList<SysMenu>();
    public virtual ICollection<SysMenu> ModifiedMenus { get; set; } = new LinkedList<SysMenu>();
    public virtual ICollection<SysRolemenu> CreateRolemenus { get; set; } = new LinkedList<SysRolemenu>();
    public virtual ICollection<SysRolemenu> ModifiedRolemenus { get; set; } = new LinkedList<SysRolemenu>();
    public virtual ICollection<SysSerialnum> CreateSerialnums { get; set; } = new List<SysSerialnum>();
    public virtual ICollection<SysSerialnum> ModifiedSerialnums { get; set; } = new List<SysSerialnum>();
    public virtual ICollection<SysIdentity> Identities { get; set; } = new List<SysIdentity>();

    //Wms
    public virtual ICollection<WmsCarrier> CreateCarriers { get; set; } = new List<WmsCarrier>();
    public virtual ICollection<WmsCarrier> ModifiedCarriers { get; set; } = new List<WmsCarrier>();
    public virtual ICollection<WmsCustomer> CreateCustomers { get; set; } = new List<WmsCustomer>();
    public virtual ICollection<WmsCustomer> ModifiedCustomers { get; set; } = new List<WmsCustomer>();
    public virtual ICollection<WmsDelivery> CreateDeliveries { get; set; } = new List<WmsDelivery>();
    public virtual ICollection<WmsDelivery> ModifiedDeliveries { get; set; } = new List<WmsDelivery>();
    public virtual ICollection<WmsInventory> CreateInventories { get; set; } = new List<WmsInventory>();
    public virtual ICollection<WmsInventory> ModifiedInventories { get; set; } = new List<WmsInventory>();
    public virtual ICollection<WmsInventorymove> CreateInventorymoves { get; set; } = new List<WmsInventorymove>();
    public virtual ICollection<WmsInventorymove> ModifiedInventorymoves { get; set; } = new List<WmsInventorymove>();
    public virtual ICollection<WmsInventoryrecord> CreateInventoryrecords { get; set; } = new List<WmsInventoryrecord>();
    public virtual ICollection<WmsInventoryrecord> ModifiedInventoryrecords { get; set; } = new List<WmsInventoryrecord>();
    public virtual ICollection<WmsInvmovedetail> AuditinInvmovedetails { get; set; } = new List<WmsInvmovedetail>();
    public virtual ICollection<WmsInvmovedetail> CreateInvmovedetails { get; set; } = new List<WmsInvmovedetail>();
    public virtual ICollection<WmsInvmovedetail> ModifiedInvmovedetails { get; set; } = new List<WmsInvmovedetail>();
    public virtual ICollection<WmsMaterial> CreateMaterials { get; set; } = new List<WmsMaterial>();
    public virtual ICollection<WmsMaterial> ModifiedMaterials { get; set; } = new List<WmsMaterial>();
    public virtual ICollection<WmsReservoirarea> CreateReservorirareas { get; set; } = new List<WmsReservoirarea>();
    public virtual ICollection<WmsReservoirarea> ModifiedReservorirareas { get; set; } = new List<WmsReservoirarea>();
    public virtual ICollection<WmsStockin> CreateStockins { get; set; } = new List<WmsStockin>();
    public virtual ICollection<WmsStockin> ModifiedStockins { get; set; } = new List<WmsStockin>();
    public virtual ICollection<WmsStockindetail> AuditinStockindetails { get; set; } = new List<WmsStockindetail>();
    public virtual ICollection<WmsStockindetail> CreateStockindetails { get; set; } = new List<WmsStockindetail>();
    public virtual ICollection<WmsStockindetail> ModifiedStockindetails { get; set; } = new List<WmsStockindetail>();
    public virtual ICollection<WmsStockout> CreateStockouts { get; set; } = new List<WmsStockout>();
    public virtual ICollection<WmsStockout> ModifiedStockouts { get; set; } = new List<WmsStockout>();
    public virtual ICollection<WmsStockoutdetail> AuditinStockoutdetails { get; set; } = new List<WmsStockoutdetail>();
    public virtual ICollection<WmsStockoutdetail> CreateStockoutdetails { get; set; } = new List<WmsStockoutdetail>();
    public virtual ICollection<WmsStockoutdetail> ModifiedStockoutdetails { get; set; } = new List<WmsStockoutdetail>();
    public virtual ICollection<WmsStoragerack> CreateStorageracks { get; set; } = new List<WmsStoragerack>();
    public virtual ICollection<WmsStoragerack> ModifiedStorageracks { get; set; } = new List<WmsStoragerack>();
    public virtual ICollection<WmsSupplier> CreateSuppliers { get; set; } = new List<WmsSupplier>();
    public virtual ICollection<WmsSupplier> ModifiedSuppliers { get; set; } = new List<WmsSupplier>();
    public virtual ICollection<WmsWarehouse> CreateWarehouses { get; set; } = new List<WmsWarehouse>();
    public virtual ICollection<WmsWarehouse> ModifiedWarehouses { get; set; } = new List<WmsWarehouse>();

}

