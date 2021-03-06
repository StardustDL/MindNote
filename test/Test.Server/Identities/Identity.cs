using IdentityServer4.Test;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Backend.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test.Server.Identities
{
    [TestClass]
    public class Identity
    {
        [DataTestMethod]
        [DataRow("/Index")]
        [DataRow("/Privacy")]
        [DataRow("/Error")]
        [DataRow("/Identity/Account/Error")]
        [DataRow("/Identity/Account/Logout")]
        [DataRow("/.well-known/openid-configuration")]
        public void Urls(string url)
        {
            TestUser user = Utils.DefaultUser;
            using (MockIdentityWebApplicationFactory id = new MockIdentityWebApplicationFactory(user))
            {
                using (System.Net.Http.HttpClient client = id.CreateClient())
                {
                    System.Net.Http.HttpResponseMessage response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        [TestMethod]
        public void Token()
        {
            TestUser user = Utils.DefaultUser;
            using (MockIdentityWebApplicationFactory id = new MockIdentityWebApplicationFactory(user))
            {
                string token = id.GetBearerToken(user.Username, user.Password, SampleConfig.APIScope);
            }
        }
    }
}
