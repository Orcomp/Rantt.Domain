namespace Rantt.Domain.Mru
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Microsoft.Win32;

    public class MruRepository : IMruRepository
    {
        private const string SubKeyName = @"Software\Rantt\MRU";

        public void RegisterUsage(MostRecentProject mostRecentProject)
        {
            RegistryKey ranttMru = Registry.CurrentUser.CreateSubKey(SubKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (ranttMru == null)
            {
                return; // TODO: do we need to throw exception here?
            }

            RegistryKey projectKey = ranttMru.CreateSubKey(mostRecentProject.Id, RegistryKeyPermissionCheck.ReadWriteSubTree);
            
            if (projectKey == null)
            {
                return; // TODO: do we need to throw exception here?
            }

            projectKey.SetValue("Name", mostRecentProject.Name);
            projectKey.SetValue("FilePath", mostRecentProject.FilePath);
            double seconds = (mostRecentProject.LastAccessed - DateTime.MinValue).TotalSeconds;
            projectKey.SetValue("LastAccessed", seconds);
        }

        public IEnumerable<MostRecentProject> GetAllProjects()
        {
            var result = new List<MostRecentProject>();

            RegistryKey ranttMru = Registry.CurrentUser.CreateSubKey(SubKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (ranttMru == null)
            {
                return null; // TODO: do we need to throw exception here?
            }

            foreach (string subKeyName in ranttMru.GetSubKeyNames())
            {
                MostRecentProject project = new MostRecentProject { Id = subKeyName };
                RegistryKey projectKey = ranttMru.OpenSubKey(subKeyName);
                try
                {
                    project.Name = (string)projectKey.GetValue("Name");
                    project.FilePath = (string)projectKey.GetValue("FilePath");
                    double seconds = double.Parse(projectKey.GetValue("LastAccessed").ToString());
                    project.LastAccessed = DateTime.MinValue.AddSeconds(seconds);
                    result.Add(project);
                }
                catch (Exception exception)
                {
                    // do nothing
                }
            }

            return result.OrderByDescending(mrp => mrp.LastAccessed);
        }

        public void CleanRepository(int top)
        {
            var allProjects = this.GetAllProjects().ToList();
            if (allProjects.Count <= top)
            {
                return;
            }

            var ranttMru = Registry.CurrentUser.CreateSubKey(SubKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (ranttMru == null)
            {
                return; // TODO: do we need to throw exception here?
            }

            var dateTime = allProjects[top - 1].LastAccessed;
            for (int i = top; i < allProjects.Count; i++)
            {
                ranttMru.DeleteSubKeyTree(allProjects[i].Id, false);
            }
        }

        public void RemoveRecentProject(MostRecentProject project)
        {
            RegistryKey ranttMru = Registry.CurrentUser.CreateSubKey(SubKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (ranttMru == null)
            {
                return; // TODO: do we need to throw exception here?
            }

            ranttMru.DeleteSubKey(project.Id);
        }
    }
}
