﻿using System.Text;

namespace MindNote.Data.Providers
{
    public interface IDataProvider
    {
        INodesProvider NodesProvider { get; }

        IRelationsProvider RelationsProvider { get; }
    }
}
