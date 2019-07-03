﻿using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using MindNote.Data.Providers;
using MindNote.Server.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Test.Server.Apis;
using Test.Server.Identities;

namespace Test.Server.SDKs
{
    [TestClass]
    public class SDK
    {
        [TestMethod]
        public void Nodes()
        {
            Utils.UseApiEnvironment((_, api, token) =>
            {
                using (var baseClient = api.CreateClient())
                {
                    var client = new NodesClient(baseClient);

                    Assert.IsFalse(client.GetAll(token).Result.Any());
                    Assert.IsNull(client.Get(token, 0).Result);
                    client.Clear(token).Wait();
                    {
                        Node node = new Node { Name = "name" };
                        int id = client.Create(token, node).Result.Value;
                        Assert.AreEqual(node.Name, client.Query(token, id, null, null, null).Result.First().Name);
                        node.Content = "content";
                        Assert.IsTrue(client.Update(token, id, node).Result.HasValue);
                        Assert.IsTrue(client.Delete(token, id).Result.HasValue);
                    }
                }
            });
        }

        [TestMethod]
        public void Tags()
        {
            Utils.UseApiEnvironment((_, api, token) =>
            {
                using (var baseClient = api.CreateClient())
                {
                    var client = new TagsClient(baseClient);

                    Assert.IsFalse(client.GetAll(token).Result.Any());
                    Assert.IsNull(client.Get(token, 0).Result);
                    client.Clear(token).Wait();
                    {
                        Tag tag = new Tag { Name = "tag", Color = "black" };
                        int id = client.Create(token, tag).Result.Value;
                        Assert.AreEqual(tag.Name, client.Query(token, id, null, null).Result.First().Name);
                        Assert.AreEqual(tag.Color, client.GetByName(token, tag.Name).Result.Color);
                        tag.Color = "white";
                        Assert.IsTrue(client.Update(token, id, tag).Result.HasValue);
                        Assert.IsTrue(client.Delete(token, id).Result.HasValue);
                    }
                }
            });
        }

        [TestMethod]
        public void Relations()
        {
            MindNote.Data.Providers.InMemory.DataProvider provider = new MindNote.Data.Providers.InMemory.DataProvider();
            int a = provider.NodesProvider.Create(new MindNote.Data.Node { Name = "node1" }, Utils.DefaultUser.SubjectId).Result.Value;
            int b = provider.NodesProvider.Create(new MindNote.Data.Node { Name = "node2" }, Utils.DefaultUser.SubjectId).Result.Value;
            Utils.UseApiEnvironment((_, api, token) =>
            {
                using (var baseClient = api.CreateClient())
                {
                    var client = new RelationsClient(baseClient);

                    Assert.IsFalse(client.GetAll(token).Result.Any());
                    Assert.IsNull(client.Get(token, 0).Result);
                    client.Clear(token).Wait();
                    {
                        Relation rel = new Relation { From = a, To = b };
                        int id = client.Create(token, rel).Result.Value;
                        Assert.AreEqual(rel.From, client.Query(token, id, null, null).Result.First().From);
                        Assert.AreEqual(1, client.GetAdjacents(token, b).Result.Count());
                        rel.To = a;
                        Assert.IsTrue(client.Update(token, id, rel).Result.HasValue);
                        Assert.AreEqual(0, client.GetAdjacents(token, b).Result.Count());
                        Assert.IsTrue(client.Delete(token, id).Result.HasValue);
                        Assert.IsTrue(client.ClearAdjacents(token, a).Result.HasValue);
                    }
                }
            }, provider);
        }
    }
}