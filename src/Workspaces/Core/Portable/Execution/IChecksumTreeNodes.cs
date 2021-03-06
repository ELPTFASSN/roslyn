﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis.Execution
{
    /// <summary>
    /// Root checksum tree node which has ability to save additional asset
    /// </summary>
    internal interface IRootChecksumTreeNode : IChecksumTreeNode
    {
        Solution Solution { get; }

        void AddAdditionalAsset(Asset asset, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Interface for checksum tree node. we currently have 2 implementation.
    /// 
    /// one that used for hierarchical tree, one that is used to create one off asset
    /// from asset builder such as additional or global asset which is not part of
    /// hierarchical checksum tree
    /// </summary>
    internal interface IChecksumTreeNode
    {
        Serializer Serializer { get; }

        IChecksumTreeNode GetOrCreateSubTreeNode<TKey>(TKey key);

        // TResult since Task doesn't allow covariant
        Task<TResult> GetOrCreateChecksumObjectWithChildrenAsync<TKey, TValue, TResult>(
            TKey key, TValue value, string kind,
            Func<TKey, TValue, string, CancellationToken, Task<TResult>> valueGetterAsync,
            CancellationToken cancellationToken)
            where TKey : class
            where TResult : ChecksumObjectWithChildren;

        // TResult since Task doesn't allow covariant
        Task<TResult> GetOrCreateAssetAsync<TKey, TValue, TResult>(
            TKey key, TValue value, string kind,
            Func<TValue, string, CancellationToken, Task<TResult>> valueGetterAsync,
            CancellationToken cancellationToken)
            where TKey : class
            where TResult : Asset;
    }
}
