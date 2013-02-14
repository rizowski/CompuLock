using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Database.Enviroment;
using Database.Models;

namespace Service.Profile
{
    class ComputerManager
    {
        public ComputerManager()
        {
            
        }

        public static List<Principal> GetUsers()
        {
            SecurityIdentifier builtinAdminSid = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);

            PrincipalContext ctx = new PrincipalContext(ContextType.Machine);

            GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, builtinAdminSid.Value);

            if (group != null)
                return new List<Principal>(group.Members);
            return null;
        }

        public static Computer GetComputer()
        {
            return new Computer{CreatedAt = DateTime.Now, Enviroment = OS.StringName, UpdatedAt = DateTime.Now};
        }


    }
}
