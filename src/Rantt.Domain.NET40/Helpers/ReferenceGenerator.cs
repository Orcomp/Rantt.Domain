// -----------------------------------------------------------------------
// <copyright file="ReferenceGenerator.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Rantt.Domain.Helpers
{
    using System.Globalization;

    /// <summary>
    /// A class for generating reference identifiers for operations when these
    /// are not provided in the input data
    /// </summary>
    public class ReferenceGenerator
    {
        /// <summary>
        /// Singleton instances
        /// </summary>
        private static ReferenceGenerator instance;

        /// <summary>
        /// The next reference number.
        /// </summary>
        private int nextReferenceNumber = 1;

        /// <summary>
        /// Prevents a default instance of the <see cref="ReferenceGenerator"/> class from being created.
        /// </summary>
        private ReferenceGenerator()
        {
            // Empty
        }

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        public static ReferenceGenerator Instance
        {
            get
            {
                return instance ?? (instance = new ReferenceGenerator());
            }
        }

        /// <summary>
        /// Gets the next reference number.
        /// </summary>
        public string NextReferenceNumber
        {
            get
            {
                return (this.nextReferenceNumber++).ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
