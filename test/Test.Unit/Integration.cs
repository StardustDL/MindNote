﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Test.Unit
{
    [TestClass]
    public class Integration
    {
        TestServer testServer;

        [TestInitialize]
        public void Init()
        {
            var host = MindNote.API.Program.CreateWebHostBuilder(new string[]{
                $"ConnectionString={MindNote.Data.Providers.SqlServer.DataContextFactory.LocalConnection}"
            }).UseStartup<MindNote.API.Startup>();
            testServer = new TestServer(host);
            MindNote.API.Program.InitialDatabase(testServer.Host);
        }

        [TestMethod]
        public void Nodes()
        {
            using (var client = testServer.CreateClient())
            {
                Assert.AreNotEqual("[]", client.GetStringAsync("/api/Nodes/Full").Result);
                Assert.AreNotEqual("[]", client.GetStringAsync("/api/Nodes/1/Full").Result);
                Assert.AreEqual("[]", client.GetStringAsync("/api/Nodes/1/Tags").Result);
            }
        }

        [TestMethod]
        public void Structs()
        {
            using (var client = testServer.CreateClient())
            {
                Assert.AreNotEqual("[]", client.GetStringAsync("/api/Structs/Full").Result);
                Assert.AreNotEqual("[]", client.GetStringAsync("/api/Nodes/1/Full").Result);
                Assert.AreEqual("[]", client.GetStringAsync("/api/Structs/1/Tags").Result);
            }
        }

        [TestMethod]
        public void Tags()
        {
            using (var client = testServer.CreateClient())
            {
                Assert.AreNotEqual("[]", client.GetStringAsync("/api/Tags/All").Result);
            }
        }

        [TestMethod]
        public void Admin()
        {
            using (var client = testServer.CreateClient())
            {
                client.PostAsync("/api/Admin", new ByteArrayContent(Convert.FromBase64String("WwogIHsKICAgICJpZCI6IDEsCiAgICAibmFtZSI6ICJTdHJ1Y3QgNiIsCiAgICAibm9kZXMiOiBbCiAgICAgIHsKICAgICAgICAiaWQiOiAxLAogICAgICAgICJuYW1lIjogIk5vdGUgMSIsCiAgICAgICAgImNvbnRlbnQiOiAiY29udGVudCAxIiwKICAgICAgICAiZXh0cmEiOiAie1wieFwiOjkxLFwieVwiOjQ5fSIsCiAgICAgICAgInRhZ3MiOiBbCiAgICAgICAgICB7CiAgICAgICAgICAgICJpZCI6IDEsCiAgICAgICAgICAgICJuYW1lIjogInRhZzEiLAogICAgICAgICAgICAiY29sb3IiOiAiYmxhY2siLAogICAgICAgICAgICAiZXh0cmEiOiBudWxsCiAgICAgICAgICB9CiAgICAgICAgXSwKICAgICAgICAiY3JlYXRpb25UaW1lIjogIjIwMTktMDUtMjdUMTE6MDY6MzAuMzI0OTg0NiswODowMCIsCiAgICAgICAgIm1vZGlmaWNhdGlvblRpbWUiOiAiMjAxOS0wNS0yN1QxMTowNjozMC4zMjY1NzMyKzA4OjAwIgogICAgICB9CiAgICBdLAogICAgInJlbGF0aW9ucyI6IFsKICAgICAgewogICAgICAgICJpZCI6IDYsCiAgICAgICAgImZyb20iOiAxLAogICAgICAgICJ0byI6IDEsCiAgICAgICAgImNvbG9yIjogImdyZXkiLAogICAgICAgICJleHRyYSI6IG51bGwKICAgICAgfQogICAgXSwKICAgICJ0YWdzIjogWwogICAgICB7CiAgICAgICAgImlkIjogNiwKICAgICAgICAibmFtZSI6ICJ0YWc2IiwKICAgICAgICAiY29sb3IiOiAiYmxhY2siLAogICAgICAgICJleHRyYSI6IG51bGwKICAgICAgfQogICAgXSwKICAgICJjcmVhdGlvblRpbWUiOiAiMjAxOS0wNS0yN1QxMTowNjozMC40ODQwNjUzKzA4OjAwIiwKICAgICJtb2RpZmljYXRpb25UaW1lIjogIjIwMTktMDUtMjdUMTE6MDY6MzAuNDg0MDY1NCswODowMCIsCiAgICAiZXh0cmEiOiAie1wiY29sb3JcIjpcImJsdWVcIn0iCiAgfSwKICB7CiAgICAiaWQiOiAyLAogICAgIm5hbWUiOiAiU3RydWN0IDUiLAogICAgIm5vZGVzIjogWwogICAgICB7CiAgICAgICAgImlkIjogMSwKICAgICAgICAibmFtZSI6ICJOb3RlIDEiLAogICAgICAgICJjb250ZW50IjogImNvbnRlbnQgMSIsCiAgICAgICAgImV4dHJhIjogIntcInhcIjo5MSxcInlcIjo0OX0iLAogICAgICAgICJ0YWdzIjogWwogICAgICAgICAgewogICAgICAgICAgICAiaWQiOiAxLAogICAgICAgICAgICAibmFtZSI6ICJ0YWcxIiwKICAgICAgICAgICAgImNvbG9yIjogImJsYWNrIiwKICAgICAgICAgICAgImV4dHJhIjogbnVsbAogICAgICAgICAgfQogICAgICAgIF0sCiAgICAgICAgImNyZWF0aW9uVGltZSI6ICIyMDE5LTA1LTI3VDExOjA2OjMwLjMyNDk4NDYrMDg6MDAiLAogICAgICAgICJtb2RpZmljYXRpb25UaW1lIjogIjIwMTktMDUtMjdUMTE6MDY6MzAuMzI2NTczMiswODowMCIKICAgICAgfSwKICAgICAgewogICAgICAgICJpZCI6IDIsCiAgICAgICAgIm5hbWUiOiAiTm90ZSAyIiwKICAgICAgICAiY29udGVudCI6ICJjb250ZW50IDIiLAogICAgICAgICJleHRyYSI6ICJ7XCJ4XCI6MzIsXCJ5XCI6NTh9IiwKICAgICAgICAidGFncyI6IFsKICAgICAgICAgIHsKICAgICAgICAgICAgImlkIjogMiwKICAgICAgICAgICAgIm5hbWUiOiAidGFnMiIsCiAgICAgICAgICAgICJjb2xvciI6ICJibGFjayIsCiAgICAgICAgICAgICJleHRyYSI6IG51bGwKICAgICAgICAgIH0KICAgICAgICBdLAogICAgICAgICJjcmVhdGlvblRpbWUiOiAiMjAxOS0wNS0yN1QxMTowNjozMC40Nzk3MzIrMDg6MDAiLAogICAgICAgICJtb2RpZmljYXRpb25UaW1lIjogIjIwMTktMDUtMjdUMTE6MDY6MzAuNDc5NzMzMiswODowMCIKICAgICAgfQogICAgXSwKICAgICJyZWxhdGlvbnMiOiBbCiAgICAgIHsKICAgICAgICAiaWQiOiA1LAogICAgICAgICJmcm9tIjogMSwKICAgICAgICAidG8iOiAyLAogICAgICAgICJjb2xvciI6ICJncmV5IiwKICAgICAgICAiZXh0cmEiOiBudWxsCiAgICAgIH0KICAgIF0sCiAgICAidGFncyI6IFsKICAgICAgewogICAgICAgICJpZCI6IDUsCiAgICAgICAgIm5hbWUiOiAidGFnNSIsCiAgICAgICAgImNvbG9yIjogImJsYWNrIiwKICAgICAgICAiZXh0cmEiOiBudWxsCiAgICAgIH0KICAgIF0sCiAgICAiY3JlYXRpb25UaW1lIjogIjIwMTktMDUtMjdUMTE6MDY6MzAuNDg0MDY0MSswODowMCIsCiAgICAibW9kaWZpY2F0aW9uVGltZSI6ICIyMDE5LTA1LTI3VDExOjA2OjMwLjQ4NDA2NDIrMDg6MDAiLAogICAgImV4dHJhIjogIntcImNvbG9yXCI6XCJibHVlXCJ9IgogIH0sCiAgewogICAgImlkIjogMywKICAgICJuYW1lIjogIlN0cnVjdCA0IiwKICAgICJub2RlcyI6IFsKICAgICAgewogICAgICAgICJpZCI6IDIsCiAgICAgICAgIm5hbWUiOiAiTm90ZSAyIiwKICAgICAgICAiY29udGVudCI6ICJjb250ZW50IDIiLAogICAgICAgICJleHRyYSI6ICJ7XCJ4XCI6MzIsXCJ5XCI6NTh9IiwKICAgICAgICAidGFncyI6IFsKICAgICAgICAgIHsKICAgICAgICAgICAgImlkIjogMiwKICAgICAgICAgICAgIm5hbWUiOiAidGFnMiIsCiAgICAgICAgICAgICJjb2xvciI6ICJibGFjayIsCiAgICAgICAgICAgICJleHRyYSI6IG51bGwKICAgICAgICAgIH0KICAgICAgICBdLAogICAgICAgICJjcmVhdGlvblRpbWUiOiAiMjAxOS0wNS0yN1QxMTowNjozMC40Nzk3MzIrMDg6MDAiLAogICAgICAgICJtb2RpZmljYXRpb25UaW1lIjogIjIwMTktMDUtMjdUMTE6MDY6MzAuNDc5NzMzMiswODowMCIKICAgICAgfSwKICAgICAgewogICAgICAgICJpZCI6IDMsCiAgICAgICAgIm5hbWUiOiAiTm90ZSAzIiwKICAgICAgICAiY29udGVudCI6ICJjb250ZW50IDMiLAogICAgICAgICJleHRyYSI6ICJ7XCJ4XCI6OTMsXCJ5XCI6MzV9IiwKICAgICAgICAidGFncyI6IFsKICAgICAgICAgIHsKICAgICAgICAgICAgImlkIjogMywKICAgICAgICAgICAgIm5hbWUiOiAidGFnMyIsCiAgICAgICAgICAgICJjb2xvciI6ICJibGFjayIsCiAgICAgICAgICAgICJleHRyYSI6IG51bGwKICAgICAgICAgIH0KICAgICAgICBdLAogICAgICAgICJjcmVhdGlvblRpbWUiOiAiMjAxOS0wNS0yN1QxMTowNjozMC40Nzk3NjQyKzA4OjAwIiwKICAgICAgICAibW9kaWZpY2F0aW9uVGltZSI6ICIyMDE5LTA1LTI3VDExOjA2OjMwLjQ3OTc2NDQrMDg6MDAiCiAgICAgIH0KICAgIF0sCiAgICAicmVsYXRpb25zIjogWwogICAgICB7CiAgICAgICAgImlkIjogNCwKICAgICAgICAiZnJvbSI6IDIsCiAgICAgICAgInRvIjogMywKICAgICAgICAiY29sb3IiOiAiZ3JleSIsCiAgICAgICAgImV4dHJhIjogbnVsbAogICAgICB9CiAgICBdLAogICAgInRhZ3MiOiBbCiAgICAgIHsKICAgICAgICAiaWQiOiA0LAogICAgICAgICJuYW1lIjogInRhZzQiLAogICAgICAgICJjb2xvciI6ICJibGFjayIsCiAgICAgICAgImV4dHJhIjogbnVsbAogICAgICB9CiAgICBdLAogICAgImNyZWF0aW9uVGltZSI6ICIyMDE5LTA1LTI3VDExOjA2OjMwLjQ4NDA2MzIrMDg6MDAiLAogICAgIm1vZGlmaWNhdGlvblRpbWUiOiAiMjAxOS0wNS0yN1QxMTowNjozMC40ODQwNjMzKzA4OjAwIiwKICAgICJleHRyYSI6ICJ7XCJjb2xvclwiOlwiYmx1ZVwifSIKICB9LAogIHsKICAgICJpZCI6IDQsCiAgICAibmFtZSI6ICJTdHJ1Y3QgMyIsCiAgICAibm9kZXMiOiBbCiAgICAgIHsKICAgICAgICAiaWQiOiAzLAogICAgICAgICJuYW1lIjogIk5vdGUgMyIsCiAgICAgICAgImNvbnRlbnQiOiAiY29udGVudCAzIiwKICAgICAgICAiZXh0cmEiOiAie1wieFwiOjkzLFwieVwiOjM1fSIsCiAgICAgICAgInRhZ3MiOiBbCiAgICAgICAgICB7CiAgICAgICAgICAgICJpZCI6IDMsCiAgICAgICAgICAgICJuYW1lIjogInRhZzMiLAogICAgICAgICAgICAiY29sb3IiOiAiYmxhY2siLAogICAgICAgICAgICAiZXh0cmEiOiBudWxsCiAgICAgICAgICB9CiAgICAgICAgXSwKICAgICAgICAiY3JlYXRpb25UaW1lIjogIjIwMTktMDUtMjdUMTE6MDY6MzAuNDc5NzY0MiswODowMCIsCiAgICAgICAgIm1vZGlmaWNhdGlvblRpbWUiOiAiMjAxOS0wNS0yN1QxMTowNjozMC40Nzk3NjQ0KzA4OjAwIgogICAgICB9LAogICAgICB7CiAgICAgICAgImlkIjogNCwKICAgICAgICAibmFtZSI6ICJOb3RlIDQiLAogICAgICAgICJjb250ZW50IjogImNvbnRlbnQgNCIsCiAgICAgICAgImV4dHJhIjogIntcInhcIjozMSxcInlcIjozNX0iLAogICAgICAgICJ0YWdzIjogWwogICAgICAgICAgewogICAgICAgICAgICAiaWQiOiA0LAogICAgICAgICAgICAibmFtZSI6ICJ0YWc0IiwKICAgICAgICAgICAgImNvbG9yIjogImJsYWNrIiwKICAgICAgICAgICAgImV4dHJhIjogbnVsbAogICAgICAgICAgfQogICAgICAgIF0sCiAgICAgICAgImNyZWF0aW9uVGltZSI6ICIyMDE5LTA1LTI3VDExOjA2OjMwLjQ3OTc2NjgrMDg6MDAiLAogICAgICAgICJtb2RpZmljYXRpb25UaW1lIjogIjIwMTktMDUtMjdUMTE6MDY6MzAuNDc5NzY2OSswODowMCIKICAgICAgfQogICAgXSwKICAgICJyZWxhdGlvbnMiOiBbCiAgICAgIHsKICAgICAgICAiaWQiOiAzLAogICAgICAgICJmcm9tIjogMywKICAgICAgICAidG8iOiA0LAogICAgICAgICJjb2xvciI6ICJncmV5IiwKICAgICAgICAiZXh0cmEiOiBudWxsCiAgICAgIH0KICAgIF0sCiAgICAidGFncyI6IFsKICAgICAgewogICAgICAgICJpZCI6IDMsCiAgICAgICAgIm5hbWUiOiAidGFnMyIsCiAgICAgICAgImNvbG9yIjogImJsYWNrIiwKICAgICAgICAiZXh0cmEiOiBudWxsCiAgICAgIH0KICAgIF0sCiAgICAiY3JlYXRpb25UaW1lIjogIjIwMTktMDUtMjdUMTE6MDY6MzAuNDg0MDU3OCswODowMCIsCiAgICAibW9kaWZpY2F0aW9uVGltZSI6ICIyMDE5LTA1LTI3VDExOjA2OjMwLjQ4NDA1NzkrMDg6MDAiLAogICAgImV4dHJhIjogIntcImNvbG9yXCI6XCJibHVlXCJ9IgogIH0sCiAgewogICAgImlkIjogNSwKICAgICJuYW1lIjogIlN0cnVjdCAyIiwKICAgICJub2RlcyI6IFsKICAgICAgewogICAgICAgICJpZCI6IDQsCiAgICAgICAgIm5hbWUiOiAiTm90ZSA0IiwKICAgICAgICAiY29udGVudCI6ICJjb250ZW50IDQiLAogICAgICAgICJleHRyYSI6ICJ7XCJ4XCI6MzEsXCJ5XCI6MzV9IiwKICAgICAgICAidGFncyI6IFsKICAgICAgICAgIHsKICAgICAgICAgICAgImlkIjogNCwKICAgICAgICAgICAgIm5hbWUiOiAidGFnNCIsCiAgICAgICAgICAgICJjb2xvciI6ICJibGFjayIsCiAgICAgICAgICAgICJleHRyYSI6IG51bGwKICAgICAgICAgIH0KICAgICAgICBdLAogICAgICAgICJjcmVhdGlvblRpbWUiOiAiMjAxOS0wNS0yN1QxMTowNjozMC40Nzk3NjY4KzA4OjAwIiwKICAgICAgICAibW9kaWZpY2F0aW9uVGltZSI6ICIyMDE5LTA1LTI3VDExOjA2OjMwLjQ3OTc2NjkrMDg6MDAiCiAgICAgIH0sCiAgICAgIHsKICAgICAgICAiaWQiOiA1LAogICAgICAgICJuYW1lIjogIk5vdGUgNSIsCiAgICAgICAgImNvbnRlbnQiOiAiY29udGVudCA1IiwKICAgICAgICAiZXh0cmEiOiAie1wieFwiOjI1LFwieVwiOjYzfSIsCiAgICAgICAgInRhZ3MiOiBbCiAgICAgICAgICB7CiAgICAgICAgICAgICJpZCI6IDUsCiAgICAgICAgICAgICJuYW1lIjogInRhZzUiLAogICAgICAgICAgICAiY29sb3IiOiAiYmxhY2siLAogICAgICAgICAgICAiZXh0cmEiOiBudWxsCiAgICAgICAgICB9CiAgICAgICAgXSwKICAgICAgICAiY3JlYXRpb25UaW1lIjogIjIwMTktMDUtMjdUMTE6MDY6MzAuNDc5NzY4MSswODowMCIsCiAgICAgICAgIm1vZGlmaWNhdGlvblRpbWUiOiAiMjAxOS0wNS0yN1QxMTowNjozMC40Nzk3NjgyKzA4OjAwIgogICAgICB9CiAgICBdLAogICAgInJlbGF0aW9ucyI6IFsKICAgICAgewogICAgICAgICJpZCI6IDIsCiAgICAgICAgImZyb20iOiA0LAogICAgICAgICJ0byI6IDUsCiAgICAgICAgImNvbG9yIjogImdyZXkiLAogICAgICAgICJleHRyYSI6IG51bGwKICAgICAgfQogICAgXSwKICAgICJ0YWdzIjogWwogICAgICB7CiAgICAgICAgImlkIjogMiwKICAgICAgICAibmFtZSI6ICJ0YWcyIiwKICAgICAgICAiY29sb3IiOiAiYmxhY2siLAogICAgICAgICJleHRyYSI6IG51bGwKICAgICAgfQogICAgXSwKICAgICJjcmVhdGlvblRpbWUiOiAiMjAxOS0wNS0yN1QxMTowNjozMC40ODQwNTA5KzA4OjAwIiwKICAgICJtb2RpZmljYXRpb25UaW1lIjogIjIwMTktMDUtMjdUMTE6MDY6MzAuNDg0MDUxNCswODowMCIsCiAgICAiZXh0cmEiOiAie1wiY29sb3JcIjpcImJsdWVcIn0iCiAgfSwKICB7CiAgICAiaWQiOiA2LAogICAgIm5hbWUiOiAiU3RydWN0IDEiLAogICAgIm5vZGVzIjogWwogICAgICB7CiAgICAgICAgImlkIjogNSwKICAgICAgICAibmFtZSI6ICJOb3RlIDUiLAogICAgICAgICJjb250ZW50IjogImNvbnRlbnQgNSIsCiAgICAgICAgImV4dHJhIjogIntcInhcIjoyNSxcInlcIjo2M30iLAogICAgICAgICJ0YWdzIjogWwogICAgICAgICAgewogICAgICAgICAgICAiaWQiOiA1LAogICAgICAgICAgICAibmFtZSI6ICJ0YWc1IiwKICAgICAgICAgICAgImNvbG9yIjogImJsYWNrIiwKICAgICAgICAgICAgImV4dHJhIjogbnVsbAogICAgICAgICAgfQogICAgICAgIF0sCiAgICAgICAgImNyZWF0aW9uVGltZSI6ICIyMDE5LTA1LTI3VDExOjA2OjMwLjQ3OTc2ODErMDg6MDAiLAogICAgICAgICJtb2RpZmljYXRpb25UaW1lIjogIjIwMTktMDUtMjdUMTE6MDY6MzAuNDc5NzY4MiswODowMCIKICAgICAgfSwKICAgICAgewogICAgICAgICJpZCI6IDYsCiAgICAgICAgIm5hbWUiOiAiTm90ZSA2IiwKICAgICAgICAiY29udGVudCI6ICJjb250ZW50IDYiLAogICAgICAgICJleHRyYSI6ICJ7XCJ4XCI6NTMsXCJ5XCI6MzF9IiwKICAgICAgICAidGFncyI6IFsKICAgICAgICAgIHsKICAgICAgICAgICAgImlkIjogNiwKICAgICAgICAgICAgIm5hbWUiOiAidGFnNiIsCiAgICAgICAgICAgICJjb2xvciI6ICJibGFjayIsCiAgICAgICAgICAgICJleHRyYSI6IG51bGwKICAgICAgICAgIH0KICAgICAgICBdLAogICAgICAgICJjcmVhdGlvblRpbWUiOiAiMjAxOS0wNS0yN1QxMTowNjozMC40Nzk3NzQ2KzA4OjAwIiwKICAgICAgICAibW9kaWZpY2F0aW9uVGltZSI6ICIyMDE5LTA1LTI3VDExOjA2OjMwLjQ3OTc3NDcrMDg6MDAiCiAgICAgIH0KICAgIF0sCiAgICAicmVsYXRpb25zIjogWwogICAgICB7CiAgICAgICAgImlkIjogMSwKICAgICAgICAiZnJvbSI6IDUsCiAgICAgICAgInRvIjogNiwKICAgICAgICAiY29sb3IiOiAiZ3JleSIsCiAgICAgICAgImV4dHJhIjogbnVsbAogICAgICB9CiAgICBdLAogICAgInRhZ3MiOiBbCiAgICAgIHsKICAgICAgICAiaWQiOiAxLAogICAgICAgICJuYW1lIjogInRhZzEiLAogICAgICAgICJjb2xvciI6ICJibGFjayIsCiAgICAgICAgImV4dHJhIjogbnVsbAogICAgICB9CiAgICBdLAogICAgImNyZWF0aW9uVGltZSI6ICIyMDE5LTA1LTI3VDExOjA2OjMwLjQ4MDE0NzkrMDg6MDAiLAogICAgIm1vZGlmaWNhdGlvblRpbWUiOiAiMjAxOS0wNS0yN1QxMTowNjozMC40ODAyMDI2KzA4OjAwIiwKICAgICJleHRyYSI6ICJ7XCJjb2xvclwiOlwiYmx1ZVwifSIKICB9LAogIHsKICAgICJpZCI6IDcsCiAgICAibmFtZSI6ICJQeXRob24gMTAwIERheXMiLAogICAgIm5vZGVzIjogWwogICAgICB7CiAgICAgICAgImlkIjogNywKICAgICAgICAibmFtZSI6ICJEYXkwMSAtIOWIneivhlB5dGhvbiIsCiAgICAgICAgImNvbnRlbnQiOiAiIyMgRGF5MDEgLSDliJ3or4ZQeXRob25cblxuIyMjIFB5dGhvbueugOS7i1xuXG4jIyMjIFB5dGhvbueahOWOhuWPslxuXG4xLiAxOTg55bm05Zyj6K+e6IqC77yaR3VpZG8gdm9uIFJvc3N1beW8gOWni+WGmVB5dGhvbuivreiogOeahOe8luivkeWZqOOAglxuMi4gMTk5MeW5tDLmnIjvvJrnrKzkuIDkuKpQeXRob27nvJbor5HlmajvvIjlkIzml7bkuZ/mmK/op6Pph4rlmajvvInor57nlJ/vvIzlroPmmK/nlKhD6K+t6KiA5a6e546w55qE77yI5ZCO6Z2i5Y+I5Ye6546w5LqGSmF2YeWSjEMj5a6e546w55qE54mI5pysSnl0aG9u5ZKMSXJvblB5dGhvbu+8jOS7peWPilB5UHnjgIFCcnl0aG9u44CBUHlzdG9u562J5YW25LuW5a6e546w77yJ77yM5Y+v5Lul6LCD55SoQ+ivreiogOeahOW6k+WHveaVsOOAguWcqOacgOaXqeeahOeJiOacrOS4re+8jFB5dGhvbuW3sue7j+aPkOS+m+S6huWvueKAnOexu+KAne+8jOKAnOWHveaVsOKAne+8jOKAnOW8guW4uOWkhOeQhuKAneetieaehOmAoOWdl+eahOaUr+aMge+8jOWQjOaXtuaPkOS+m+S6huKAnOWIl+ihqOKAneWSjOKAnOWtl+WFuOKAneetieaguOW/g+aVsOaNruexu+Wei++8jOWQjOaXtuaUr+aMgeS7peaooeWdl+S4uuWfuuehgOeahOaLk+Wxleezu+e7n+OAglxuMy4gMTk5NOW5tDHmnIjvvJpQeXRob24gMS4w5q2j5byP5Y+R5biD44CCXG40LiAyMDAw5bm0MTDmnIgxNuaXpe+8mlB5dGhvbiAyLjDlj5HluIPvvIzlop7liqDkuoblrp7njrDlrozmlbTnmoRb5Z6D5Zy+5Zue5pS2XShodHRwczovL3poLndpa2lwZWRpYS5vcmcvd2lraS8lRTUlOUUlODMlRTUlOUMlQkUlRTUlOUIlOUUlRTYlOTQlQjZfKCVFOCVBOCU4OCVFNyVBRSU5NyVFNiVBOSU5RiVFNyVBNyU5MSVFNSVBRCVCOCkp77yM5o+Q5L6b5LqG5a+5W1VuaWNvZGVdKGh0dHBzOi8vemgud2lraXBlZGlhLm9yZy93aWtpL1VuaWNvZGUp55qE5pSv5oyB44CC5LiO5q2k5ZCM5pe277yMUHl0aG9u55qE5pW05Liq5byA5Y+R6L+H56iL5pu05Yqg6YCP5piO77yM56S+5Yy65a+55byA5Y+R6L+b5bqm55qE5b2x5ZON6YCQ5riQ5omp5aSn77yM55Sf5oCB5ZyI5byA5aeL5oWi5oWi5b2i5oiQ44CCXG41LiAyMDA45bm0MTLmnIgz5pel77yaUHl0aG9uIDMuMOWPkeW4g++8jOWug+W5tuS4jeWujOWFqOWFvOWuueS5i+WJjeeahFB5dGhvbuS7o+egge+8jOS4jei/h+WboOS4uuebruWJjei/mOacieS4jeWwkeWFrOWPuOWcqOmhueebruWSjOi/kOe7tOS4reS9v+eUqFB5dGhvbiAyLnjniYjmnKzvvIzmiYDku6VQeXRob24gMy5455qE5b6I5aSa5paw54m55oCn5ZCO5p2l5Lmf6KKr56e75qSN5YiwUHl0aG9uIDIuNi8yLjfniYjmnKzkuK3jgIJcblxu55uu5YmN5oiR5Lus5L2/55So55qEUHl0aG9uIDMuNy5455qE54mI5pys5piv5ZyoMjAxOOW5tOWPkeW4g+eahO+8jFB5dGhvbueahOeJiOacrOWPt+WIhuS4uuS4ieaute+8jOW9ouWmgkEuQi5D44CC5YW25LitQeihqOekuuWkp+eJiOacrOWPt++8jOS4gOiIrOW9k+aVtOS9k+mHjeWGme+8jOaIluWHuueOsOS4jeWQkeWQjuWFvOWuueeahOaUueWPmOaXtu+8jOWinuWKoEHvvJtC6KGo56S65Yqf6IO95pu05paw77yM5Ye6546w5paw5Yqf6IO95pe25aKe5YqgQu+8m0PooajnpLrlsI/nmoTmlLnliqjvvIjlpoLkv67lpI3kuobmn5DkuKpCdWfvvInvvIzlj6ropoHmnInkv67mlLnlsLHlop7liqBD44CC5aaC5p6c5a+5UHl0aG9u55qE5Y6G5Y+y5oSf5YW06Laj77yM5Y+v5Lul5p+l55yL5LiA56+H5ZCN5Li6W+OAilB5dGhvbueugOWPsuOAi10oaHR0cDovL3d3dy5jbmJsb2dzLmNvbS92YW1laS9hcmNoaXZlLzIwMTMvMDIvMDYvMjg5MjYyOC5odG1sKeeahOWNmuaWh+OAglxuXG4iLAogICAgICAgICJleHRyYSI6ICJ7XCJ4XCI6NTEsXCJ5XCI6NDR9IiwKICAgICAgICAidGFncyI6IFsKICAgICAgICAgIHsKICAgICAgICAgICAgImlkIjogNywKICAgICAgICAgICAgIm5hbWUiOiAiUHl0aG9uIiwKICAgICAgICAgICAgImNvbG9yIjogImdyZWVuIiwKICAgICAgICAgICAgImV4dHJhIjogbnVsbAogICAgICAgICAgfQogICAgICAgIF0sCiAgICAgICAgImNyZWF0aW9uVGltZSI6ICIyMDE5LTA1LTI3VDExOjA2OjMwLjYxNzQ2ODIrMDg6MDAiLAogICAgICAgICJtb2RpZmljYXRpb25UaW1lIjogIjAwMDEtMDEtMDFUMDA6MDA6MDArMDA6MDAiCiAgICAgIH0sCiAgICAgIHsKICAgICAgICAiaWQiOiA4LAogICAgICAgICJuYW1lIjogIkRheTAyIC0g6K+t6KiA5YWD57SgIiwKICAgICAgICAiY29udGVudCI6ICIjIyBEYXkwMiAtIOivreiogOWFg+e0oFxuXG4jIyMjIOaMh+S7pOWSjOeoi+W6j1xuXG7orqHnrpfmnLrnmoTnoazku7bns7vnu5/pgJrluLjnlLHkupTlpKfpg6jku7bmnoTmiJDvvIzljIXmi6zvvJrov5DnrpflmajjgIHmjqfliLblmajjgIHlrZjlgqjlmajjgIHovpPlhaXorr7lpIflkozovpPlh7rorr7lpIfjgILlhbbkuK3vvIzov5DnrpflmajlkozmjqfliLblmajmlL7lnKjkuIDotbflsLHmmK/miJHku6zpgJrluLjmiYDor7TnmoTkuK3lpK7lpITnkIblmajvvIzlroPnmoTlip/og73mmK/miafooYzlkITnp43ov5DnrpflkozmjqfliLbmjIfku6Tku6Xlj4rlpITnkIborqHnrpfmnLrova/ku7bkuK3nmoTmlbDmja7jgILmiJHku6zpgJrluLjmiYDor7TnmoTnqIvluo/lrp7pmYXkuIrlsLHmmK/mjIfku6TnmoTpm4blkIjvvIzmiJHku6znqIvluo/lsLHmmK/lsIbkuIDns7vliJfnmoTmjIfku6TmjInnhafmn5Dnp43mlrnlvI/nu4Tnu4fliLDkuIDotbfvvIznhLblkI7pgJrov4fov5nkupvmjIfku6TljrvmjqfliLborqHnrpfmnLrlgZrmiJHku6zmg7PorqnlroPlgZrnmoTkuovmg4XjgILku4rlpKnmiJHku6zkvb/nlKjnmoTorqHnrpfmnLromb3nhLblmajku7blgZrlt6XotormnaXotornsr7lr4bvvIzlpITnkIbog73lipvotormnaXotorlvLrlpKfvvIzkvYbnqbblhbbmnKzotKjmnaXor7Tku43nhLblsZ7kuo5b4oCc5Yavwrfor7rkvp3mm7znu5PmnoTigJ1dKGh0dHBzOi8vemgud2lraXBlZGlhLm9yZy93aWtpLyVFNSU4NiVBRiVDMiVCNyVFOCVBRiVCQSVFNCVCQyU4QSVFNiU5QiVCQyVFNyVCQiU5MyVFNiU5RSU4NCnnmoTorqHnrpfmnLrjgILigJzlhq/Ct+ivuuS+neabvOe7k+aehOKAneacieS4pOS4quWFs+mUrueCue+8jOS4gOaYr+aMh+WHuuimgeWwhuWtmOWCqOiuvuWkh+S4juS4reWkruWkhOeQhuWZqOWIhuW8gO+8jOS6jOaYr+aPkOWHuuS6huWwhuaVsOaNruS7peS6jOi/m+WItuaWueW8j+e8lueggeOAguS6jOi/m+WItuaYr+S4gOenjeKAnOmAouS6jOi/m+S4gOKAneeahOiuoeaVsOazle+8jOi3n+aIkeS7rOS6uuexu+S9v+eUqOeahOKAnOmAouWNgei/m+S4gOKAneeahOiuoeaVsOazleayoeacieWunui0qOaAp+eahOWMuuWIq++8jOS6uuexu+WboOS4uuacieWNgeagueaJi+aMh+aJgOS7peS9v+eUqOS6huWNgei/m+WItu+8iOWboOS4uuWcqOaVsOaVsOaXtuWNgeagueaJi+aMh+eUqOWujOS5i+WQjuWwseWPquiDvei/m+S9jeS6hu+8jOW9k+eEtuWHoeS6i+mDveacieS+i+Wklu+8jOeOm+mbheS6uuWPr+iDveaYr+WboOS4uumVv+W5tOWFieedgOiEmueahOWOn+WboOaKiuiEmui2vuWktOS5n+eul+S4iuS6hu+8jOS6juaYr+S7luS7rOS9v+eUqOS6huS6jOWNgei/m+WItueahOiuoeaVsOazle+8jOWcqOi/meenjeiuoeaVsOazleeahOaMh+WvvOS4i+eOm+mbheS6uueahOWOhuazleWwseS4juaIkeS7rOW5s+W4uOS9v+eUqOeahOWOhuazleS4jeS4gOagt++8jOiAjOaMieeFp+eOm+mbheS6uueahOWOhuazle+8jDIwMTLlubTmmK/kuIrkuIDkuKrmiYDosJPnmoTigJzlpKrpmLPnuqrigJ3nmoTmnIDlkI7kuIDlubTvvIzogIwyMDEz5bm05YiZ5piv5paw55qE4oCc5aSq6Ziz57qq4oCd55qE5byA5aeL77yM5ZCO5p2l6L+Z5Lu25LqL5oOF6KKr5Lul6K655Lyg6K6555qE5pa55byP6K+v5Lyg5Li64oCdMjAxMuW5tOaYr+eOm+mbheS6uumihOiogOeahOS4lueVjOacq+aXpeKAnOi/meenjeiNkuivnueahOivtOazle+8jOS7iuWkqeaIkeS7rOWPr+S7peWkp+iDhueahOeMnOa1i++8jOeOm+mbheaWh+aYjuS5i+aJgOS7peWPkeWxlee8k+aFouS8sOiuoeS5n+S4juS9v+eUqOS6huS6jOWNgei/m+WItuacieWFs++8ieOAguWvueS6juiuoeeul+acuuadpeivtO+8jOS6jOi/m+WItuWcqOeJqeeQhuWZqOS7tuS4iuadpeivtOaYr+acgOWuueaYk+WunueOsOeahO+8iOmrmOeUteWOi+ihqOekujHvvIzkvY7nlLXljovooajnpLow77yJ77yM5LqO5piv5Zyo4oCc5Yavwrfor7rkvp3mm7znu5PmnoTigJ3nmoTorqHnrpfmnLrpg73kvb/nlKjkuobkuozov5vliLbjgILomb3nhLbmiJHku6zlubbkuI3pnIDopoHmr4/kuKrnqIvluo/lkZjpg73og73lpJ/kvb/nlKjkuozov5vliLbnmoTmgJ3nu7TmlrnlvI/mnaXlt6XkvZzvvIzkvYbmmK/kuobop6Pkuozov5vliLbku6Xlj4rlroPkuI7miJHku6znlJ/mtLvkuK3nmoTljYHov5vliLbkuYvpl7TnmoTovazmjaLlhbPns7vvvIzku6Xlj4rkuozov5vliLbkuI7lhavov5vliLblkozljYHlha3ov5vliLbnmoTovazmjaLlhbPns7vov5jmmK/mnInlv4XopoHnmoTjgILlpoLmnpzkvaDlr7nov5nkuIDngrnkuI3nhp/mgonvvIzlj6/ku6Xoh6rooYzkvb/nlKhb57u05Z+655m+56eRXShodHRwczovL3poLndpa2lwZWRpYS5vcmcvd2lraS8lRTQlQkElOEMlRTglQkYlOUIlRTUlODglQjYp5oiW6ICFW+eZvuW6pueZvuenkV0oaHR0cHM6Ly9iYWlrZS5iYWlkdS5jb20p56eR5pmu5LiA5LiL44CCXG5cbiMjIyDlj5jph4/lkoznsbvlnotcblxu5Zyo56iL5bqP6K6+6K6h5Lit77yM5Y+Y6YeP5piv5LiA56eN5a2Y5YKo5pWw5o2u55qE6L295L2T44CC6K6h566X5py65Lit55qE5Y+Y6YeP5piv5a6e6ZmF5a2Y5Zyo55qE5pWw5o2u5oiW6ICF6K+05piv5a2Y5YKo5Zmo5Lit5a2Y5YKo5pWw5o2u55qE5LiA5Z2X5YaF5a2Y56m66Ze077yM5Y+Y6YeP55qE5YC85Y+v5Lul6KKr6K+75Y+W5ZKM5L+u5pS577yM6L+Z5piv5omA5pyJ6K6h566X5ZKM5o6n5Yi255qE5Z+656GA44CC6K6h566X5py66IO95aSE55CG55qE5pWw5o2u5pyJ5b6I5aSa56eN57G75Z6L77yM6Zmk5LqG5pWw5YC85LmL5aSW6L+Y5Y+v5Lul5aSE55CG5paH5pys44CB5Zu+5b2i44CB6Z+z6aKR44CB6KeG6aKR562J5ZCE56eN5ZCE5qC355qE5pWw5o2u77yM6YKj5LmI5LiN5ZCM55qE5pWw5o2u5bCx6ZyA6KaB5a6a5LmJ5LiN5ZCM55qE5a2Y5YKo57G75Z6L44CCUHl0aG9u5Lit55qE5pWw5o2u57G75Z6L5b6I5aSa77yM6ICM5LiU5Lmf5YWB6K645oiR5Lus6Ieq5a6a5LmJ5paw55qE5pWw5o2u57G75Z6L77yI6L+Z5LiA54K55Zyo5ZCO6Z2i5Lya6K6y5Yiw77yJ77yM5oiR5Lus5YWI5LuL57uN5Yeg56eN5bi455So55qE5pWw5o2u57G75Z6L44CCXG5cbi0g5pW05Z6L77yaUHl0aG9u5Lit5Y+v5Lul5aSE55CG5Lu75oSP5aSn5bCP55qE5pW05pWw77yIUHl0aG9uIDIueOS4reaciWludOWSjGxvbmfkuKTnp43nsbvlnovnmoTmlbTmlbDvvIzkvYbov5nnp43ljLrliIblr7lQeXRob27mnaXor7TmhI/kuYnkuI3lpKfvvIzlm6DmraTlnKhQeXRob24gMy545Lit5pW05pWw5Y+q5pyJaW506L+Z5LiA56eN5LqG77yJ77yM6ICM5LiU5pSv5oyB5LqM6L+b5Yi277yI5aaCYDBiMTAwYO+8jOaNoueul+aIkOWNgei/m+WItuaYrzTvvInjgIHlhavov5vliLbvvIjlpoJgMG8xMDBg77yM5o2i566X5oiQ5Y2B6L+b5Yi25pivNjTvvInjgIHljYHov5vliLbvvIhgMTAwYO+8ieWSjOWNgeWFrei/m+WItu+8iGAweDEwMGDvvIzmjaLnrpfmiJDljYHov5vliLbmmK8yNTbvvInnmoTooajnpLrms5XjgIJcbi0g5rWu54K55Z6L77ya5rWu54K55pWw5Lmf5bCx5piv5bCP5pWw77yM5LmL5omA5Lul56ew5Li65rWu54K55pWw77yM5piv5Zug5Li65oyJ54Wn56eR5a2m6K6w5pWw5rOV6KGo56S65pe277yM5LiA5Liq5rWu54K55pWw55qE5bCP5pWw54K55L2N572u5piv5Y+v5Y+Y55qE77yM5rWu54K55pWw6Zmk5LqG5pWw5a2m5YaZ5rOV77yI5aaCYDEyMy40NTZg77yJ5LmL5aSW6L+Y5pSv5oyB56eR5a2m6K6h5pWw5rOV77yI5aaCYDEuMjM0NTZlMmDvvInjgIJcbi0g5a2X56ym5Liy5Z6L77ya5a2X56ym5Liy5piv5Lul5Y2V5byV5Y+35oiW5Y+M5byV5Y+35ous6LW35p2l55qE5Lu75oSP5paH5pys77yM5q+U5aaCYCdoZWxsbydg5ZKMYFwiaGVsbG9cImAs5a2X56ym5Liy6L+Y5pyJ5Y6f5aeL5a2X56ym5Liy6KGo56S65rOV44CB5a2X6IqC5a2X56ym5Liy6KGo56S65rOV44CBVW5pY29kZeWtl+espuS4suihqOekuuazle+8jOiAjOS4lOWPr+S7peS5puWGmeaIkOWkmuihjOeahOW9ouW8j++8iOeUqOS4ieS4quWNleW8leWPt+aIluS4ieS4quWPjOW8leWPt+W8gOWktO+8jOS4ieS4quWNleW8leWPt+aIluS4ieS4quWPjOW8leWPt+e7k+Wwvu+8ieOAglxuLSDluIPlsJTlnovvvJrluIPlsJTlgLzlj6rmnIlgVHJ1ZWDjgIFgRmFsc2Vg5Lik56eN5YC877yM6KaB5LmI5pivYFRydWVg77yM6KaB5LmI5pivYEZhbHNlYO+8jOWcqFB5dGhvbuS4re+8jOWPr+S7peebtOaOpeeUqGBUcnVlYOOAgWBGYWxzZWDooajnpLrluIPlsJTlgLzvvIjor7fms6jmhI/lpKflsI/lhpnvvInvvIzkuZ/lj6/ku6XpgJrov4fluIPlsJTov5DnrpforqHnrpflh7rmnaXvvIjkvovlpoJgMyA8IDVg5Lya5Lqn55Sf5biD5bCU5YC8YFRydWVg77yM6ICMYDIgPT0gMWDkvJrkuqfnlJ/luIPlsJTlgLxgRmFsc2Vg77yJ44CCXG4tIOWkjeaVsOWei++8muW9ouWmgmAzKzVqYO+8jOi3n+aVsOWtpuS4iueahOWkjeaVsOihqOekuuS4gOagt++8jOWUr+S4gOS4jeWQjOeahOaYr+iZmumDqOeahGBpYOaNouaIkOS6hmBqYOOAgiIsCiAgICAgICAgImV4dHJhIjogIntcInhcIjoxMyxcInlcIjo1fSIsCiAgICAgICAgInRhZ3MiOiBbCiAgICAgICAgICB7CiAgICAgICAgICAgICJpZCI6IDcsCiAgICAgICAgICAgICJuYW1lIjogIlB5dGhvbiIsCiAgICAgICAgICAgICJjb2xvciI6ICJncmVlbiIsCiAgICAgICAgICAgICJleHRyYSI6IG51bGwKICAgICAgICAgIH0KICAgICAgICBdLAogICAgICAgICJjcmVhdGlvblRpbWUiOiAiMjAxOS0wNS0yN1QxMTowNjozMC42MTc1NDQ1KzA4OjAwIiwKICAgICAgICAibW9kaWZpY2F0aW9uVGltZSI6ICIwMDAxLTAxLTAxVDAwOjAwOjAwKzAwOjAwIgogICAgICB9LAogICAgICB7CiAgICAgICAgImlkIjogOSwKICAgICAgICAibmFtZSI6ICJEYXkwMyAtIOWIhuaUr+e7k+aehCIsCiAgICAgICAgImNvbnRlbnQiOiAiIyMgRGF5MDMgLSDliIbmlK/nu5PmnoRcblxuIyMjIOWIhuaUr+e7k+aehOeahOW6lOeUqOWcuuaZr1xuXG7ov4Tku4rkuLrmraLvvIzmiJHku6zlhpnnmoRQeXRob27ku6PnoIHpg73mmK/kuIDmnaHkuIDmnaHor63lj6Xpobrluo/miafooYzvvIzov5nnp43nu5PmnoTnmoTku6PnoIHmiJHku6znp7DkuYvkuLrpobrluo/nu5PmnoTjgILnhLbogIzku4XmnInpobrluo/nu5PmnoTlubbkuI3og73op6PlhrPmiYDmnInnmoTpl67popjvvIzmr5TlpoLmiJHku6zorr7orqHkuIDkuKrmuLjmiI/vvIzmuLjmiI/nrKzkuIDlhbPnmoTpgJrlhbPmnaHku7bmmK/njqnlrrbojrflvpcxMDAw5YiG77yM6YKj5LmI5Zyo5a6M5oiQ5pys5bGA5ri45oiP5ZCO5oiR5Lus6KaB5qC55o2u546p5a625b6X5Yiw5YiG5pWw5p2l5Yaz5a6a56m256uf5piv6L+b5YWl56ys5LqM5YWz6L+Y5piv5ZGK6K+J546p5a624oCcR2FtZSBPdmVy4oCd77yM6L+Z6YeM5bCx5Lya5Lqn55Sf5Lik5Liq5YiG5pSv77yM6ICM5LiU6L+Z5Lik5Liq5YiG5pSv5Y+q5pyJ5LiA5Liq5Lya6KKr5omn6KGM77yM6L+Z5bCx5piv56iL5bqP5Lit5YiG5pSv57uT5p6E44CC57G75Ly855qE5Zy65pmv6L+Y5pyJ5b6I5aSa77yM57uZ5aSn5a625LiA5YiG6ZKf55qE5pe26Ze077yM5L2g5bqU6K+l5Y+v5Lul5oOz5Yiw6Iez5bCRNeS4quS7peS4iui/meagt+eahOS+i+WtkO+8jOi1tue0p+ivleS4gOivleOAglxuXG4jIyMgaWbor63lj6XnmoTkvb/nlKhcblxu5ZyoUHl0aG9u5Lit77yM6KaB5p6E6YCg5YiG5pSv57uT5p6E5Y+v5Lul5L2/55SoYGlmYOOAgWBlbGlmYOWSjGBlbHNlYOWFs+mUruWtl+OAguaJgOiwk+WFs+mUruWtl+WwseaYr+acieeJueauiuWQq+S5ieeahOWNleivje+8jOWDj2BpZmDlkoxgZWxzZWDlsLHmmK/kuJPpl6jnlKjkuo7mnoTpgKDliIbmlK/nu5PmnoTnmoTlhbPplK7lrZfvvIzlvojmmL7nhLbkvaDkuI3og73lpJ/kvb/nlKjlroPkvZzkuLrlj5jph4/lkI3vvIjkuovlrp7kuIrvvIznlKjkvZzlhbbku5bnmoTmoIfor4bnrKbkuZ/mmK/kuI3lj6/ku6XvvInjgILkuIvpnaLnmoTkvovlrZDkuK3mvJTnpLrkuoblpoLkvZXmnoTpgKDkuIDkuKrliIbmlK/nu5PmnoTjgIJcblxuYGBgUHl0aG9uXG5cIlwiXCJcbueUqOaIt+i6q+S7vemqjOivgVxuXG5WZXJzaW9uOiAwLjFcbkF1dGhvcjog6aqG5piKXG5cIlwiXCJcblxudXNlcm5hbWUgPSBpbnB1dCgn6K+36L6T5YWl55So5oi35ZCNOiAnKVxucGFzc3dvcmQgPSBpbnB1dCgn6K+36L6T5YWl5Y+j5LukOiAnKVxuIyDlpoLmnpzluIzmnJvovpPlhaXlj6Pku6Tml7Yg57uI56uv5Lit5rKh5pyJ5Zue5pi+IOWPr+S7peS9v+eUqGdldHBhc3PmqKHlnZfnmoRnZXRwYXNz5Ye95pWwXG4jIGltcG9ydCBnZXRwYXNzXG4jIHBhc3N3b3JkID0gZ2V0cGFzcy5nZXRwYXNzKCfor7fovpPlhaXlj6Pku6Q6ICcpXG5pZiB1c2VybmFtZSA9PSAnYWRtaW4nIGFuZCBwYXNzd29yZCA9PSAnMTIzNDU2JzpcbiAgICBwcmludCgn6Lqr5Lu96aqM6K+B5oiQ5YqfIScpXG5lbHNlOlxuICAgIHByaW50KCfouqvku73pqozor4HlpLHotKUhJylcbmBgYFxuXG7llK/kuIDpnIDopoHor7TmmI7nmoTmmK/lkoxDL0MrK+OAgUphdmHnrYnor63oqIDkuI3lkIzvvIxQeXRob27kuK3msqHmnInnlKjoirHmi6zlj7fmnaXmnoTpgKDku6PnoIHlnZfogIzmmK/kvb/nlKjkuobnvKnov5vnmoTmlrnlvI/mnaXorr7nva7ku6PnoIHnmoTlsYLmrKHnu5PmnoTvvIzlpoLmnpxgaWZg5p2h5Lu25oiQ56uL55qE5oOF5Ya15LiL6ZyA6KaB5omn6KGM5aSa5p2h6K+t5Y+l77yM5Y+q6KaB5L+d5oyB5aSa5p2h6K+t5Y+l5YW35pyJ55u45ZCM55qE57yp6L+b5bCx5Y+v5Lul5LqG77yM5o2i5Y+l6K+d6K+06L+e57ut55qE5Luj56CB5aaC5p6c5Y+I5L+d5oyB5LqG55u45ZCM55qE57yp6L+b6YKj5LmI5a6D5Lus5bGe5LqO5ZCM5LiA5Liq5Luj56CB5Z2X77yM55u45b2T5LqO5piv5LiA5Liq5omn6KGM55qE5pW05L2T44CCXG5cbuW9k+eEtuWmguaenOimgeaehOmAoOWHuuabtOWkmueahOWIhuaUr++8jOWPr+S7peS9v+eUqGBpZuKApmVsaWbigKZlbHNl4oCmYOe7k+aehO+8jOS+i+WmguS4i+mdoueahOWIhuauteWHveaVsOaxguWAvOOAglxuXG4hWyQkZih4KT1cXGJlZ2lue2Nhc2VzfSAzeC01JlxcdGV4dHsoeD4xKX1cXFxceCsyJlxcdGV4dHsoLTF9XFxsZXFcXHRleHR7eH1cXGxlcVxcdGV4dHsxKX1cXFxcNXgrMyZcXHRleHQgeyh4PC0xKX1cXGVuZHtjYXNlc30kJF0oLi9yZXMvZm9ybXVsYV8xLnBuZylcblxuYGBgUHl0aG9uXG5cIlwiXCJcbuWIhuauteWHveaVsOaxguWAvFxuXG4gICAgICAgIDN4IC0gNSAgKHggPiAxKVxuZih4KSA9ICB4ICsgMiAgICgtMSA8PSB4IDw9IDEpXG4gICAgICAgIDV4ICsgMyAgKHggPCAtMSlcblxuVmVyc2lvbjogMC4xXG5BdXRob3I6IOmqhuaYilxuXCJcIlwiXG5cbnggPSBmbG9hdChpbnB1dCgneCA9ICcpKVxuaWYgeCA+IDE6XG4gICAgeSA9IDMgKiB4IC0gNVxuZWxpZiB4ID49IC0xOlxuICAgIHkgPSB4ICsgMlxuZWxzZTpcbiAgICB5ID0gNSAqIHggKyAzXG5wcmludCgnZiglLjJmKSA9ICUuMmYnICUgKHgsIHkpKVxuYGBgIiwKICAgICAgICAiZXh0cmEiOiAie1wieFwiOjU4LFwieVwiOjI0fSIsCiAgICAgICAgInRhZ3MiOiBbCiAgICAgICAgICB7CiAgICAgICAgICAgICJpZCI6IDcsCiAgICAgICAgICAgICJuYW1lIjogIlB5dGhvbiIsCiAgICAgICAgICAgICJjb2xvciI6ICJncmVlbiIsCiAgICAgICAgICAgICJleHRyYSI6IG51bGwKICAgICAgICAgIH0KICAgICAgICBdLAogICAgICAgICJjcmVhdGlvblRpbWUiOiAiMjAxOS0wNS0yN1QxMTowNjozMC42MTc1ODMxKzA4OjAwIiwKICAgICAgICAibW9kaWZpY2F0aW9uVGltZSI6ICIwMDAxLTAxLTAxVDAwOjAwOjAwKzAwOjAwIgogICAgICB9CiAgICBdLAogICAgInJlbGF0aW9ucyI6IFsKICAgICAgewogICAgICAgICJpZCI6IDcsCiAgICAgICAgImZyb20iOiA3LAogICAgICAgICJ0byI6IDgsCiAgICAgICAgImNvbG9yIjogImdyZXkiLAogICAgICAgICJleHRyYSI6IG51bGwKICAgICAgfSwKICAgICAgewogICAgICAgICJpZCI6IDgsCiAgICAgICAgImZyb20iOiA4LAogICAgICAgICJ0byI6IDksCiAgICAgICAgImNvbG9yIjogImdyZXkiLAogICAgICAgICJleHRyYSI6IG51bGwKICAgICAgfQogICAgXSwKICAgICJ0YWdzIjogWwogICAgICB7CiAgICAgICAgImlkIjogNywKICAgICAgICAibmFtZSI6ICJQeXRob24iLAogICAgICAgICJjb2xvciI6ICJncmVlbiIsCiAgICAgICAgImV4dHJhIjogbnVsbAogICAgICB9CiAgICBdLAogICAgImNyZWF0aW9uVGltZSI6ICIyMDE5LTA1LTI3VDExOjA2OjMwLjYzOTAzOTUrMDg6MDAiLAogICAgIm1vZGlmaWNhdGlvblRpbWUiOiAiMjAxOS0wNS0yN1QxMTowNjozMC42MzkwNDEyKzA4OjAwIiwKICAgICJleHRyYSI6ICJ7XCJjb2xvclwiOlwib3JhbmdlXCJ9IgogIH0KXQ=="))).Wait();
            }
        }

        [TestMethod]
        public void Relations()
        {
            using (var client = testServer.CreateClient())
            {
                Assert.AreNotEqual("[]", client.GetStringAsync("/api/Relations/All").Result);
            }
        }
    }
}
