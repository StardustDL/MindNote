﻿using GraphQL.Client.Http;
using GraphQL.Common.Request;
using GraphQL.Common.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Frontend.SDK.API
{
    public class GraphQLClient : IGraphQLClient
    {
        GraphQLHttpClient innerClient;

        public GraphQLClient(HttpClient httpClient, IGraphQLClientOptions options)
        {
            Client = httpClient;
            Options = options;
            innerClient = Client.AsGraphQLClient(new GraphQLHttpClientOptions
            {
                EndPoint = new System.Uri("http://localhost"),
                JsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                    Converters = new List<JsonConverter>
                    {
                        new Newtonsoft.Json.Converters.StringEnumConverter()
                    }
                }
            });
        }

        public HttpClient Client { get; }

        public IGraphQLClientOptions Options { get; }

        public async Task<GraphQLResponse> Query(GraphQLRequest request)
        {
            innerClient.EndPoint = await Options.GetEndpoint();
            var token = await Options.GetToken();
            if (token != null) Client.SetBearerToken(token);
            var response = await innerClient.SendQueryAsync(request);
            if (response.Errors != null)
            {
                throw new Exception(JsonConvert.SerializeObject(new { request.Query, request.OperationName, request.Variables, response.Errors, response.Data }));
            }
            return response;
        }

        public async Task<GraphQLResponse> Mutation(GraphQLRequest request)
        {
            innerClient.EndPoint = await Options.GetEndpoint();
            var token = await Options.GetToken();
            if (token != null) Client.SetBearerToken(token);
            var response = await innerClient.SendMutationAsync(request);
            if (response.Errors != null)
            {
                throw new Exception(JsonConvert.SerializeObject(new { request.Query, request.OperationName, request.Variables, response.Errors, response.Data }));
            }
            return response;
        }
    }
}
