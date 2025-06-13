﻿using System.Security;
using GC = Backend.GlobalConstants;

namespace Backend.Base.Session.Ent
{
    /// <summary>
    /// Session containing relevant objects for the logged in user
    /// Note a user can have multiple sessions open
    /// </summary>
    /// <author>John Stewart</author>
    /// <created>April 5, 2025</created>
    /// <license>**Licence**</license>
    public class SessionEnt
    {
        public string Key { get; set; }
        public OrgEnt Org { get; set; }
        public UserEnt User { get; set; }
        public UserConfig UserConfig { get; set; }
        public int SourceApp {  get; set; }

        /// <summary>
        /// Return the user's crud value for the permission
        /// Return null if permission not loaded
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetUserPermissionCrud (int id)
        {
            var p = User.Permissions.FirstOrDefault(p => p.PermissionId == id);
            if (p == null) return null;
            return p.Crud;
        }


    }
}
