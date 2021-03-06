﻿using Microsoft.AspNetCore.Mvc;
using System;

namespace MindNote.Frontend.Client.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : Controller
    {
        [HttpGet("[action]")]
        public string IdentityServer()
        {
            return Utils.Linked.Identity;
        }

        [HttpGet("[action]")]
        public string ApiServer()
        {
            return Utils.Linked.Api;
        }

        [HttpGet("[action]")]
        public string FrontendClient()
        {
            return Utils.Linked.Client;
        }
    }
}
