using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebRole1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual List<PiSpyDevice> PiSpyDevices { get; set; }

        public int? CellularCarrierId { get; set; }
        public virtual CellularCarrier CellularCarrier { get; set; }

        public int MinutesBetweenAlerts { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // pispy devices
        public DbSet<PiSpyDevice> PiSpyDevices { get; set; }
        public DbSet<PiSpyDataLog> PiSpyDataLogs { get; set; }
        public DbSet<PiSpyVideoLog> PiSpyVideoLogs { get; set; }

        // policy engine 
        public DbSet<Policy> Policies { get; set; }
        public DbSet<NumericPolicy> NumericPolicies { get; set; }
        public DbSet<PolicyGroup> PolicyGroups { get; set; }
        public DbSet<AndPolicyGroup> AndPolicyGroups { get; set; }
        public DbSet<OrPolicyGroup> OrPolicyGroups { get; set; }
        public DbSet<XorPolicyGroup> XorPolicyGroups { get; set; }
        public DbSet<ComparisonOperator> ComparisonOperators { get; set; }
        public DbSet<NumericPolicyType> NumericPolicyTypes { get; set; }

        public DbSet<CellularCarrier> CellularCarriers { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}