﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace XOG.Factories
{
    public class ApplicationUser : IdentityUser
    {
        public string UserType { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            //// Add custom user claims here
            //var identity = new List<Claim>();
            //identity.Add(new Claim("", ""));

            //userIdentity.AddClaims(identity);

            return userIdentity;
        }
    }
}