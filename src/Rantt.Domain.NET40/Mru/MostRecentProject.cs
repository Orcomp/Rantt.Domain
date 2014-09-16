

namespace Rantt.Domain.Mru
{
    using System;

    /// <summary>
    /// The structure to save data about recently opened project
    /// </summary>
    public class MostRecentProject
    {
        /// <summary>
        /// Gets or sets project's unique id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets project's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets file path to a project.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets datetime when project has been accessed last.
        /// </summary>
        public DateTime LastAccessed { get; set; }
    }
}
