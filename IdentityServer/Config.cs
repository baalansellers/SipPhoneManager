// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[] 
            { 
                new ApiResource("api.sip.manager", "SIP Phone Manager API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new Client[]
            { 
                new Client
                {
                    ClientId = "sip.mgr",
                    ClientName = "SIP Phone Manager",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RedirectUris =
                    {
                        "https://baalansellers.github.io/SIP-MGR-SPA/assets/login-callback.html",
                        "https://baalansellers.github.io/SIP-MGR-SPA/assets/renew-callback.html",
                        "https://localhost:4200/assets/login-callback.html",
                        "https://localhost:4200/assets/renew-callback.html"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://baalansellers.github.io/SIP-MGR-SPA/?postLogout=true",
                        "https://localhost:4200/?postLogout=true"
                    },
                    AllowedCorsOrigins =
                    {
                        "https://baalansellers.github.io",
                        "https://localhost:4200"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api.sip.manager"
                    },

                    //AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    IdentityTokenLifetime = 120,
                    AccessTokenLifetime = 120
                }
            };
        }
    }
}