﻿using IdentityModel.Client;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Backend.Identity;
using MindNote.Shared.Web.Configuration;
using System;
using System.Collections.Generic;

namespace Test.Server.Identities
{
    public class MockIdentityWebApplicationFactory : WebApplicationFactory<MindNote.Backend.Identity.Startup>
    {
        public const string ClientId = "test";
        public const string ClientSecret = "secret";
        private readonly List<TestUser> users;

        public MockIdentityWebApplicationFactory(params TestUser[] users)
        {
            this.users = new List<TestUser>(users);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Mock DB and IdentityServices

                services.AddDbContext<MindNote.Backend.Identity.Data.ApplicationDbContext>(optionsAction: options =>
                {
                    options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                });

                LinkedServerConfiguration server = new LinkedServerConfiguration
                {
                    Api = "",
                    Host = "",
                    Client = "",
                    Identity = ""
                };

                Startup.ConfigureIdentityServices(server, null, services, idServer =>
                {
                    idServer.AddInMemoryClients(new[]{
                        new Client
                        {
                            ClientId=ClientId,
                            ClientSecrets = new [] { new Secret(ClientSecret.Sha256()) },
                            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                            AllowedScopes = { SampleConfig.APIScope }
                        }})
                    .AddTestUsers(users);
                });

                Startup.ConfigureFinalServices(null, services);
            });

            builder.Configure(app =>
            {
                Startup.ConfigureApp(null, app, null);
            });
        }

        public string GetBearerToken(string username, string password, string scope)
        {
            using (System.Net.Http.HttpClient client = CreateClient())
            {
                DiscoveryResponse disco = client.GetDiscoveryDocumentAsync(Utils.ServerConfiguration.Identity).Result;
                Assert.IsFalse(disco.IsError);

                TokenResponse tokenResponse = client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = MockIdentityWebApplicationFactory.ClientId,
                    ClientSecret = MockIdentityWebApplicationFactory.ClientSecret,

                    UserName = username,
                    Password = password,
                    Scope = scope,
                }).Result;

                Assert.IsFalse(tokenResponse.IsError);
                return tokenResponse.Json["access_token"].ToString();
            }
        }
    }
}
