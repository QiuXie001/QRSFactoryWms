﻿using System.Collections.Generic;
using DB.Models;

namespace DB.Dto
{
    public class PermissionMenu
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public List<PermissionMenu> Children { get; set; } = new List<PermissionMenu>();

        public static PermissionMenu Create(SysMenu permission)
        {
            PermissionMenu ret = new PermissionMenu()
            {
                Id = permission.MenuId.ToString(),
                Name = permission.MenuName,
                ParentId = permission.MenuParent.ToString(),
                Url = permission.MenuUrl,
                Icon = permission.MenuIcon,
            };
            return ret;
        }
    }
}