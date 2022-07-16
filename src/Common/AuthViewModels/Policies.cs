using Microsoft.AspNetCore.Authorization;

namespace Common.AuthViewModels;

    public static class Policies
    {
        public const string IsAdmin = "IsAdmin";
        public const string IsStaff = "IsStaff";
        public const string IsManager = "IsManager";

        public static AuthorizationPolicy IsAdminPolicy() {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                .RequireRole("Admin")
                .Build();
        }

        public static AuthorizationPolicy IsStaffPolicy() {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                .RequireRole("Staff")
                .Build();
        }

        public static AuthorizationPolicy IsManagerPolicy() {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                .RequireRole("Manager")
                .Build();
        }
    }
