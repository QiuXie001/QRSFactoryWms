using DB.Models;
using Repository;
using IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using Qiu.Core.Dto;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;

namespace Services
{
    //public class Sys_RoleService : BaseService<SysRole>, ISys_RoleService
    //{
        //private readonly ISys_RoleResponsitory _responsitory;
        //private readonly ISys_RoleMenuService _rolemenuService;
        //private readonly ISys_MenuService _menuService;
        //private readonly QrsfactoryWmsContext _dbContext;
        //public Sys_RoleService(
        //    ISys_MenuService menuServices, 
        //    ISys_RoleMenuService rolemenuService,
        //    QrsfactoryWmsContext dbContext, 
        //    ISys_RoleResponsitory responsitory
        //    ) : base(responsitory)
        //{
        //    _responsitory = responsitory;
        //    _dbContext = dbContext;
        //    _rolemenuService = rolemenuService;
        //    _menuService = menuServices;
        //}

        //public string PageList(Bootstrap.BootstrapParams bootstrap)
        //{
        //    int totalNumber = 0;
        //    if (bootstrap.offset != 0)
        //    {
        //        bootstrap.offset = bootstrap.offset / bootstrap.limit + 1;
        //    }
        //    var query = _dbContext.Queryable<SysRole, SysUser, SysUser>
        //        ((s, c, u) => new object[] {
        //           JoinType.Left,s.CreateBy==c.UserId,
        //           JoinType.Left,s.ModifiedBy==u.UserId
        //         })
        //         .Select((s, c, u) => new
        //         {
        //             RoleId = s.RoleId.ToString(),
        //             s.RoleType,
        //             s.RoleName,
        //             s.IsDel,
        //             s.Remark,
        //             CName = c.UserNickname,
        //             s.CreateDate,
        //             UName = u.UserNickname,
        //             s.ModifiedDate
        //         }).MergeTable().Where((s) => s.IsDel == 1);
        //    if (!bootstrap.search.IsEmpty())
        //    {
        //        query.Where((s) => s.RoleName.Contains(bootstrap.search));
        //    }
        //    if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty())
        //    {
        //        query.Where(s => s.CreateDate > bootstrap.datemin.ToDateTimeB() && s.CreateDate <= bootstrap.datemax.ToDateTimeE());
        //    }
        //    if (bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase))
        //    {
        //        query.OrderBy($"MergeTable.{bootstrap.sort} desc");
        //    }
        //    else
        //    {
        //        query.OrderBy($"MergeTable.{bootstrap.sort} asc");
        //    }
        //    var list = query.ToPageList(bootstrap.offset, bootstrap.limit, ref totalNumber);
        //    return Bootstrap.GridData(list, totalNumber).JilToJson();
        //}

        //public List<PermissionMenu> GetMenu()
        //{
        //    var permissionMenus = _menuService.QueryableToList(c => c.IsDel == 1 && c.MenuType == "menu" && c.Status == 1);
        //    var parentPermissions = permissionMenus.Where(a => a.MenuParent == -1).ToList();
        //    var ret = new List<PermissionMenu>();
        //    parentPermissions.ForEach(p =>
        //    {
        //        PermissionMenu permissionMenu = PermissionMenu.Create(p);
        //        permissionMenu.Children = permissionMenus
        //        .Where(c => c.MenuParent == p.MenuId)
        //        .Select(m => new PermissionMenu()
        //        {
        //            Id = m.MenuId.ToString(),
        //            Name = m.MenuName,
        //            Icon = m.MenuIcon,
        //            Url = m.MenuUrl,
        //            ParentId = m.MenuParent.ToString()
        //        }).ToList();
        //        ret.Add(permissionMenu);
        //    });
        //    return ret;
        //}

        //public List<PermissionMenu> GetMenu(long roleId, string menuType = "menu")
        //{
        //    var listAll = _dbContext.Queryable<SysRoleMenu, SysMenu>
        //        ((s, c) => new object[] {
        //           JoinType.Left,s.MenuId==c.MenuId,
        //         })
        //         .Select((s, c) => new
        //         {
        //             RoleId = s.RoleId.ToString(),
        //             c.IsDel,
        //             c.MenuName,
        //             c.MenuUrl,
        //             c.MenuType,
        //             c.MenuIcon,
        //             c.MenuParent,
        //             c.Status,
        //             c.MenuId,
        //             c.Sort
        //         }).MergeTable().Where((s) => s.IsDel == 1 && s.MenuType == menuType && s.Status == 1 && s.RoleId == roleId.ToString()).OrderBy(s => s.Sort).ToList();
        //    var listParent = listAll.Where(s => s.MenuParent == -1).ToList();
        //    List<PermissionMenu> ret = new List<PermissionMenu>();
        //    listParent.ForEach(p =>
        //    {
        //        PermissionMenu permissionMenu = PermissionMenu.Create(new Sys_menu
        //        {
        //            MenuId = p.MenuId,
        //            Status = p.Status,
        //            MenuIcon = p.MenuIcon,
        //            MenuName = p.MenuName,
        //            MenuParent = p.MenuParent,
        //            MenuType = p.MenuType,
        //            MenuUrl = p.MenuUrl,
        //        });
        //        permissionMenu.Children = listAll
        //        .Where(c => c.MenuParent == p.MenuId)
        //        .Select(m => new PermissionMenu()
        //        {
        //            Id = m.MenuId.ToString(),
        //            Name = m.MenuName,
        //            Icon = m.MenuIcon,
        //            Url = m.MenuUrl,
        //            ParentId = m.MenuParent.ToString()
        //        }).ToList();
        //        ret.Add(permissionMenu);
        //    });
        //    return ret;
        //}

        //public Task<bool> Insert(SysRole role, long userId, string[] menuId)
        //{
        //    return _dbContext.Ado.UseTran(() =>
        //   {
        //       role.RoleId = PubId.SnowflakeId;
        //       role.CreateBy = userId;
        //       role.RoleType = "#";
        //       var roleId = _repository.InsertReturnEntity(role);
        //       if (!roleId.IsNullT())
        //       {
        //           var list = new List<SysRoleMenu>();
        //           foreach (var item in menuId)
        //           {
        //               list.Add(new SysRoleMenu
        //               {
        //                   CreateBy = userId,
        //                   MenuId = item.ToInt64(),
        //                   RoleId = roleId.RoleId,
        //                   RoleMenuId = PubId.SnowflakeId
        //               });
        //           }
        //           _rolemenuServices.Insert(list);
        //       }
        //   });
        //}

        //public DbResult<bool> Update(SysRole role, long userId, string[] menuId)
        //{
        //    var list = _rolemenuServices.QueryableToList(c => c.RoleId == role.RoleId);
        //    string idsu = "";  //数据库中的Id;
        //    list.ForEach((m) =>
        //    {
        //        idsu += m.MenuId + ",";
        //    });
        //    var arr = idsu.TrimEnd(',').ToSplit(',');
        //    //menuId 页面上的菜单Id;
        //    role.ModifiedBy = userId;
        //    role.ModifiedDate = DateTimeExt.DateTime;
        //    //role.RoleType = "#";
        //    string[] pageId = arr.Union(menuId).Except(menuId).ToArray(); //delete
        //    string[] dataId = menuId.Union(arr).Except(arr).ToArray();  //insert
        //    return _responsitory.UseTran(() =>
        //    {
        //        _repository.Update(role);
        //        List<long> array = new List<long>();
        //        if (pageId.Any())
        //        {
        //            foreach (var item in pageId)
        //            {
        //                array.Add(list.Where(c => c.RoleId == role.RoleId && c.MenuId == item.ToInt64()).SingleOrDefault().RoleMenuId);
        //            }
        //            _rolemenuServices.Delete(array.ToArray());
        //        }
        //        if (dataId.Any())
        //        {
        //            var roleList = new List<Sys_rolemenu>();
        //            foreach (var item in dataId)
        //            {
        //                roleList.Add(new Sys_rolemenu
        //                {
        //                    CreateBy = userId,
        //                    MenuId = item.ToInt64(),
        //                    RoleId = role.RoleId,
        //                    RoleMenuId = PubId.SnowflakeId
        //                });
        //            }
        //            _rolemenuServices.Insert(roleList);
        //        }
        //    });

        //}
    //}
}