//-----------------------------------------------------------------------
// <copyright file="AttributeCollection.cs" company="Orcomp">
//     Copyright (c) 2013 Orcomp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Rantt.Domain.Entities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Domain;

    /// <summary>
    /// Contains the attributes of a single operation
    /// </summary>
    public class AttributeCollection : IAttributeCollection, IDictionary<string, string>
    {
        #region Fields

        /// <summary>
        /// The values of names attributes.
        /// </summary>
        private readonly Dictionary<string, string> attributes = new Dictionary<string, string>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCollection"/> class.
        /// </summary>
        public AttributeCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCollection"/> class.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        public AttributeCollection(IEnumerable<KeyValuePair<string, string>> collection)
        {
            foreach (KeyValuePair<string, string> kvp in collection)
            {
                this.attributes.Add(kvp.Key, kvp.Value);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count
        {
            get
            {
                return this.attributes.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is read only.
        /// </summary>
        public bool IsReadOnly { get; private set; }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        public ICollection<string> Keys
        {
            get
            {
                return this.attributes.Keys;
            }
        }

        /// <summary>
        /// Gets the attribute names.
        /// </summary>
        public IEnumerable<string> Names
        {
            get
            {
                return this.attributes.Keys;
            }
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        public ICollection<string> Values
        {
            get
            {
                return this.attributes.Values;
            }
        }

        #endregion

        #region Public Indexers

        /// <summary>
        /// Indexer for accessing values of operation named attributes.
        /// </summary>
        /// <param name="name">
        /// The attribute name.
        /// </param>
        /// <returns>
        /// The value of the attribute, as a string
        /// </returns>
        public string this[string name]
        {
            get
            {
                this.CheckAttributeExists(name);
                return this.attributes[name];
            }

            set
            {
                this.attributes[name] = value;
            }
        }

        #endregion

        #region Explicit Interface Indexers

        /// <summary>
        /// The i dictionary&lt;string,string&gt;.this.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string IDictionary<string, string>.this[string key]
        {
            get
            {
                return this.attributes[key];
            }

            set
            {
                this.attributes[key] = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public void Add(string key, string value)
        {
            this.attributes.Add(key, value);
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        public void Add(KeyValuePair<string, string> item)
        {
            this.attributes.Add(item.Key, item.Value);
        }

        /// <summary>
        /// The as collection.
        /// </summary>
        /// <returns>
        /// The <see cref="ICollection"/>.
        /// </returns>
        public IEnumerable<KeyValuePair<string, string>> AsCollection()
        {
            return this.attributes;
        }

        /// <summary>
        /// The clear.
        /// </summary>
        public void Clear()
        {
            this.attributes.Clear();
        }

        /// <summary>
        /// The contains.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented
        /// </exception>
        public bool Contains(KeyValuePair<string, string> item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The contains key.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool ContainsKey(string key)
        {
            return this.attributes.ContainsKey(key);
        }

        /// <summary>
        /// The copy to.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <param name="arrayIndex">
        /// The array index.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// Not implemented
        /// </exception>
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return this.attributes.GetEnumerator();
        }

        /// <summary>
        /// Gets the value of the specified attribute as the given type
        /// </summary>
        /// <typeparam name="T">Expected type of the value</typeparam>
        /// <param name="name">The attribute name.</param>
        /// <returns>``0.</returns>
        public T GetValue<T>(string name) where T : IConvertible
        {
            return (T)this.GetValue(name, typeof(T));
        }

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
        public object GetValue(string name, Type type)
        {
            this.CheckAttributeExists(name);
            var valueAsString = this.attributes[name];
            return Convert.ChangeType(valueAsString, type, null);
        }

        /// <summary>
        /// The has name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool HasName(string name)
        {
            return this.ContainsKey(name);
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Remove(string key)
        {
            return this.attributes.Remove(key);
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented
        /// </exception>
        public bool Remove(KeyValuePair<string, string> item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The try get value.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool TryGetValue(string key, out string value)
        {
            return this.attributes.TryGetValue(key, out value);
        }

        #endregion

        #region Explicit Interface Methods

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.attributes.GetEnumerator();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The check attribute exists.
        /// </summary>
        /// <param name="attributeName">
        /// The attribute name.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If attribute is not defined
        /// </exception>
        private void CheckAttributeExists(string attributeName)
        {
            if (!this.attributes.ContainsKey(attributeName))
            {
                throw new ArgumentException("Undefined attribute with name: " + attributeName);
            }
        }

        #endregion
    }
}