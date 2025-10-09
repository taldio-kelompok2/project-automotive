using Microsoft.AspNetCore.Authorization;

namespace AutomotiveApp.Infrastructure.Implementation.Utils
{
    /// <summary>
    /// Authorization Policies Configuration
    /// Mendefinisikan policy-policy untuk authorization
    /// 
    /// Policy Types:
    /// 1. Role-Based: Berdasarkan role user
    /// 2. Claims-Based: Berdasarkan custom claims
    /// 3. Requirement-Based: Berdasarkan custom requirements
    /// </summary>
    public static class AuthorizationPolicies
    {
        // Policy names sebagai constants
        public const string RequireAdminRole = "RequireAdminRole";
        public const string RequireBuyerRole = "RequireBuyerRole";

        /// <summary>
        /// Configure all authorization policies
        /// Dipanggil saat application startup
        /// </summary>
        public static void ConfigurePolicies(AuthorizationOptions options)
        {
            // ===== ROLE-BASED POLICIES =====

            // Admin policy: Butuh role Admin atau SuperAdmin
            options.AddPolicy(RequireAdminRole, policy =>
                policy.RequireRole("Admin"));

            // User policy: Semua role bisa akses
            options.AddPolicy(RequireBuyerRole, policy =>
                policy.RequireRole("Buyer", "Admin"));
        }
    }
}
