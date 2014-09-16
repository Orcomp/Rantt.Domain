//-----------------------------------------------------------------------
// <copyright file="Relationship.cs" company="Orcomp">
//     Copyright (c) 2013 Orcomp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using CsvHelper;

namespace Rantt.Domain.Entities
{
    using System.Collections.Generic;
    using Domain;

    /// <summary>
    /// Represents an operations relationship.
    /// </summary>
    public class Relationship : IRelationship
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Relationship"/> class.
        /// </summary>
        public Relationship()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Relationship"/> class.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        public Relationship(Dictionary<string, object> values)
        {
            this.From = (string)values["From"];
            this.To = (string)values["To"];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Relationship"/> class.
        /// </summary>
        /// <param name="fieldReader">
        /// The field reader.
        /// </param>
        public Relationship(dynamic fieldReader)
        {
            this.From = fieldReader.GetField("From");
            this.To = fieldReader.GetField("To");
        }

        #region IRelationship Members

        /// <summary>
        /// Gets or sets <c>from</c> operation.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets <c>to</c> operation.
        /// </summary>
        public string To { get; set; }

        #endregion
    }
}
