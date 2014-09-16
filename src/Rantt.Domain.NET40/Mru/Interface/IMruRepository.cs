namespace Rantt.Domain.Mru
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IMruRepository
    {
        void RegisterUsage(MostRecentProject mostRecentProject);

        IEnumerable<MostRecentProject> GetAllProjects();

        void CleanRepository(int top);

        void RemoveRecentProject(MostRecentProject project);
    }
}
