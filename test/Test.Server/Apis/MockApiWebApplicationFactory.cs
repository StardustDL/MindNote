﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MindNote.Frontend.SDK.Identity;
using MindNote.Data.Providers;
using MindNote.Backend.API;
using MindNote.Data.Repositories;

namespace Test.Server.Apis
{
    public class MockApiWebApplicationFactory : WebApplicationFactory<MindNote.Backend.API.Startup>
    {
        private readonly IDataRepository dataRepository;
        private readonly TestServer idServer;
        private readonly IIdentityDataGetter idData;

        public MockApiWebApplicationFactory(TestServer idServer, IDataRepository dataRepository, IIdentityDataGetter idData)
        {
            this.dataRepository = dataRepository;
            this.idServer = idServer;
            this.idData = idData;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Mock DataProvider
                services.AddSingleton(dataRepository);

                // Mock IdentityServices
                {
                    services.AddAuthorization();

                    System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

                    services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
                    {
                        options.Authority = Utils.ServerConfiguration.Identity;
                        options.RequireHttpsMetadata = false;
                        options.BackchannelHttpHandler = idServer.CreateHandler();

                        options.Audience = "api";
                    });
                }

                Startup.ConfigureDocumentServices(Utils.ServerConfiguration, services);

                Startup.ConfigureGraphQLServices(Utils.ServerConfiguration, services);

                // Mock IdentityDataGetter
                services.AddSingleton(idData);

                Startup.ConfigureFinalServices(null, services);
            });

            builder.Configure(app =>
            {
                Startup.ConfigureApp(null, app, null);
            });
        }
    }
}
