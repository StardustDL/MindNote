﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MindNote.Frontend.SDK.API;
using MindNote.Frontend.SDK.Identity;
using MindNote.Data;
using MindNote.Data.Providers.Queries;
using MindNote.Frontend.Server.Helpers;
using MindNote.Frontend.Server.Pages.Notes;
using MindNote.Frontend.Server.Pages.Shared;
using MindNote.Frontend.Server.Pages.Shared.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Frontend.Server.Pages.Categories
{
    public class ViewModel : PageModel
    {
        private readonly ICategoriesClient client;
        private readonly INotesClient nodesClient;
        private readonly IIdentityDataGetter idData;
        private readonly IRelationsClient relationsClient;
        private readonly IUsersClient usersClient;

        public ViewModel(ICategoriesClient client, INotesClient nodesClient, IRelationsClient relationsClient, IUsersClient usersClient, IIdentityDataGetter idData)
        {
            this.client = client;
            this.nodesClient = nodesClient;
            this.idData = idData;
            this.relationsClient = relationsClient;
            this.usersClient = usersClient;
        }

        public CategoriesViewModel Data { get; set; }

        public IList<Note> Notes { get; set; }

        public PagingSettings Paging { get; set; }

        public string Token { get; set; }

        public User Onwer { get; set; }

        [BindProperty]
        public CategoriesPostModel PostData { get; set; }

        public GraphViewModel Graph { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, int? pageIndex)
        {
            string token = await idData.GetAccessToken(HttpContext);
            Token = token;

            try
            {
                Data = new CategoriesViewModel { Data = await client.Get(token, id) };
                if (Data.Data.UserId != idData.GetClaimId(User))
                {
                    Onwer = await usersClient.Get(token, Data.Data.UserId);
                }

                Paging = new PagingSettings
                {
                    ItemCountPerPage = 8,
                    RouteData = new Dictionary<string, string>
                    {
                        ["id"] = Data.Data.Id.ToString(),
                    }
                };
                IEnumerable<Note> notes;
                {
                    int count = (await nodesClient.Query(token, categoryId: Data.Data.Id, targets: NoteTargets.Count)).Count();
                    Paging.MaximumIndex = (count / Paging.ItemCountPerPage) + (count % Paging.ItemCountPerPage > 0 ? 1 : 0);
                    if (!pageIndex.HasValue) pageIndex = 1;
                    Paging.CurrentIndex = pageIndex.Value;
                    int offset = (Paging.CurrentIndex - 1) * Paging.ItemCountPerPage;

                    notes = await nodesClient.Query(token, categoryId: Data.Data.Id, offset: offset, count: Paging.ItemCountPerPage);
                    Notes = notes.ToList();
                }
                /*{
                    Dictionary<int, Relation> rs = new Dictionary<int, Relation>();
                    foreach (Note v in notes)
                    {
                        foreach (Relation r in await relationsClient.GetAdjacents(token, v.Id))
                        {
                            if (rs.ContainsKey(r.Id))
                            {
                                continue;
                            }

                            rs.Add(r.Id, r);
                        }
                    }
                    Graph = new GraphViewModel
                    {
                        Graph = await RelationHelper.GenerateGraph(nodesClient, client, rs.Values, token)
                    };
                }*/
            }
            catch
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string token = await idData.GetAccessToken(HttpContext);
            try
            {
                await client.Delete(token, PostData.Data.Id);
                return RedirectToPage("./Index");
            }
            catch
            {
                return NotFound();
            }
        }
    }
}