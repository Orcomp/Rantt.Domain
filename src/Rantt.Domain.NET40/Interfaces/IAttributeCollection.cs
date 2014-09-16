// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAttributeCollection.cs" company="Cherry development team">
//   Copyright (c) 2008 - 2013 Cherry development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rantt.Domain
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface for accessing the attributes of an operation.
    /// </summary>
    public interface IAttributeCollection
    {
        #region Properties
        /// <summary>
        /// Gets the attribute names.
        /// </summary>
        IEnumerable<string> Names { get; }

        /// <summary>
        /// Indexer for accessing values of operation named attributes.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The value of the attribute, as a string.
        /// </returns>
        string this[string name] { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// ``0.
        /// </returns>
        T GetValue<T>(string name)
            where T : IConvertible;

        /// <summary>
        /// The get value.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        object GetValue(string name, Type type);

        /// <summary>
        /// The has name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool HasName(string name);

        /// <summary>
        /// Ases the collection.
        /// </summary>
        /// <returns>IEnumerable{KeyValuePair{System.StringSystem.String}}.</returns>
        IEnumerable<KeyValuePair<string, string>> AsCollection();
        #endregion
    }
}