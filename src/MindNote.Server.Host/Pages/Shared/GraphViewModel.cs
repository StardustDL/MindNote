﻿using MindNote.Server.Host.Helpers;

namespace MindNote.Server.Host.Pages.Shared
{
    public class GraphViewModel
    {
        public RelationHelper.D3Graph Graph { get; set; }

        public int SelectNodeIndex { get; set; } = -1;
    }
}