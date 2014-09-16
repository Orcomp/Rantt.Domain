//-----------------------------------------------------------------------
// <copyright file="RanttConfiguration.cs" company="Orcomp">
//     Copyright (c) 2013 Orcomp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Rantt.Domain.Configuration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;

    using System.Linq;
    using System.Text.RegularExpressions;

    using Catel.Data;

#if (!SILVERLIGHT)
    using CsvHelper;
    using CsvHelperConfiguration;
    using Domain.Configuration;
    using Ionic.Zip;
#endif

    /// <summary>
    /// The resource gantt configuration.
    /// </summary>
    public sealed class RanttConfiguration: ModelBase
    {
        private IFolderMappingService _foldersService;

        /// <summary>
        /// The resource configuration.
        /// </summary>
        public const string ResourceConfigurationFile = "ResourceConfiguration.csv";

        /// <summary>
        /// The attribute base configuration file.
        /// </summary>
        public const string AttributeBaseConfigurationFile = "AtttibutesConfiguration.csv";

        /// <summary>
        /// The attribute base configuration file.
        /// </summary>
        public const string CalendarStateConfigurationFile = "CalendarStateConfiguration.csv";

        /// <summary>
        /// The attribute base configuration file.
        /// </summary>
        public const string AttributeExtendedConfigurationFile = "{0}_AtttibuteConfiguration.csv";

        public const string DefaultWorkspacePseudoName = "Default Workspace";

        public const string WorkspaceSettingsName = "workspace.dob";

        #region Fields

        private bool autoLoad;

        /// <summary>
        /// Set of configuration objects which should be updated when configuration is saved to the database
        /// </summary>
        private bool isDirty; 

        /// <summary>
        /// The attribute configurations.
        /// </summary>
        private Dictionary<string, AttributeConfiguration> attributeConfigurations = new Dictionary<string, AttributeConfiguration>();

        /// <summary>
        /// The resource configurations.
        /// </summary>
        private Dictionary<string, ResourceConfiguration> resourceConfigurations = new Dictionary<string, ResourceConfiguration>();

        /// <summary>
        /// The random access memory resource configurations.
        /// </summary>
        private Dictionary<string, ResourceConfiguration> ramResourceConfigurations = new Dictionary<string, ResourceConfiguration>();

        /// <summary>
        /// The calendar states configuration.
        /// </summary>
        private Dictionary<string, CalendarStateConfiguration> calendarStateConfigurations = new Dictionary<string, CalendarStateConfiguration>(); 

        /// <summary>
        /// Configuration Key is identifier for having different configuration for different dataset.
        /// </summary>
        private string configurationKey = string.Empty;

        /// <summary>
        /// Folder where configuration is going to be saved.
        /// </summary>
        private string configurationFolder = string.Empty;

        /// <summary>
        /// Folder where configuration is going to be saved.
        /// </summary>
        private string baseFolder = string.Empty;

        /// <summary>
        /// List of available workspaces
        /// </summary>
        private Dictionary<string, WorkspaceConfiguration> availableWorkspaces = new Dictionary<string, WorkspaceConfiguration>();

        /// <summary>
        /// Flag to show if configuration is in initialization mode and we should ignore all update events.
        /// </summary>
        private bool isInitializing;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RanttConfiguration"/> class. 
        /// </summary>
        public RanttConfiguration(IFolderMappingService foldersService)
            : this(foldersService, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RanttConfiguration"/> class. 
        /// </summary>
        /// <param name="autoLoad">
        /// The value to indicating if configuration class makes attempt to read data from file system whenever when configuration key is changed.
        /// </param>
        public RanttConfiguration(IFolderMappingService foldersService, bool autoLoad)
        {
            CurrentWorkspace = null;
            _foldersService = foldersService;
            this.autoLoad = autoLoad;
        }

        #region Delegates

        /// <summary>
        /// The configuration change handler.
        /// </summary>
        /// <param name="configurationItem">
        /// The configuration item.
        /// </param>
        /// <param name="property">
        /// The property.
        /// </param>
        public delegate void ConfigurationChangeHandler(object configurationItem, string property);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the attribute configurations.
        /// </summary>
        public Dictionary<string, AttributeConfiguration> AttributeConfigurations
        {
            get
            {
                return this.attributeConfigurations;
            }
            set
            {
                this.attributeConfigurations = value;
            }
        }

        /// <summary>
        /// Gets or sets the resource configurations.
        /// </summary>
        public Dictionary<string, ResourceConfiguration> ResourceConfigurations
        {
            get
            {
                return this.resourceConfigurations;
            }
            set
            {
                this.resourceConfigurations = value;
            }
        }

        /// <summary>
        /// Gets or sets the resource configurations.
        /// </summary>
        public Dictionary<string, ResourceConfiguration> RamResourceConfigurations
        {
            get
            {
                return this.ramResourceConfigurations;
            }
            set
            {
                this.ramResourceConfigurations = value;
            }
        }

        /// <summary>
        /// Gets or sets calendar states configuration.
        /// </summary>
        public Dictionary<string, CalendarStateConfiguration> CalendarStateConfigurations
        {
            get
            {
                return this.calendarStateConfigurations;
            }

            set
            {
                this.calendarStateConfigurations = value;
            }
        }

        public string ProjectDistingusher { get; set; }

        /// <summary>
        /// Gets or sets configuration key. Each dataset has its own configuration database.
        /// </summary>
        public string ConfigurationKey
        {
            get
            {
                return this.configurationKey;
            }
            set
            {
                #if (!SILVERLIGHT)
                this.attributeConfigurations.Clear();
                this.resourceConfigurations.Clear();
                this.ramResourceConfigurations.Clear();
                this.calendarStateConfigurations.Clear();

                this.configurationFolder = _foldersService.GetConfigurationFolderPath() + TranslateKeyToLocalFolder(value) + @"\";
                #endif

                this.configurationKey = value;

                #if (!SILVERLIGHT)
                if (this.autoLoad)
                {
                    this.LoadConfiguration();
                }

                int ndx = this.configurationKey.IndexOf(@"\", System.StringComparison.Ordinal);
                if (ndx == -1)
                {
                    this.baseFolder = this.configurationFolder;
                    this.LoadWorkspaces();
                }
                else if (string.IsNullOrEmpty(this.baseFolder))
                {
                    string baseKey = value.Substring(0, ndx);
                    this.baseFolder = _foldersService.GetConfigurationFolderPath() + TranslateKeyToLocalFolder(baseKey) + @"\";
                }
                #endif

                this.isDirty = this.resourceConfigurations.Count == 0;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether configuration is in initialization mode and we should ignore all update events.
        /// </summary>
        public new bool IsInitializing
        {
            get { return this.isInitializing; }
            set { this.isInitializing = value; }
        }

        /// <summary>
        /// Gets a value indicating whether we have something to save into database.
        /// </summary>
        public bool AreUnsavedChanges
        {
            get { return this.isDirty; }
        }

        /// <summary>
        /// Gets folder where configuration is going to be saved.
        /// </summary>
        public string ConfigurationFolder
        {
            get
            {
                return this.configurationFolder;
            }
        }

        public WorkspaceConfiguration CurrentWorkspace { get; set; }

        public Dictionary<string, WorkspaceConfiguration> AvailableWorkspaces
        {
            get
            {
                return this.availableWorkspaces;
            }
        }

        #endregion

        #region Public Methods and Operators
        private string TranslateKeyToLocalFolder(string key)
        {
            List<string> folders = key.Split('\\').ToList();
            folders.Insert(1, this.ProjectDistingusher);
            return string.Join(@"\", folders);
        }

        public void SetWorkspace(WorkspaceConfiguration workspace, string newConfigurationKey, bool loadConfiguration)
        {
            this.CurrentWorkspace = workspace;
            #if(!SILVERLIGHT)
            this.configurationFolder = _foldersService.GetConfigurationFolderPath() + TranslateKeyToLocalFolder(newConfigurationKey) + @"\";
            #endif
            if (loadConfiguration)
            {
                bool oldAutoLoad = this.autoLoad;
                this.ConfigurationKey = newConfigurationKey;
                this.autoLoad = oldAutoLoad;
            }
            else
            {
                this.configurationKey = newConfigurationKey;
            }
        }

        /// <summary>
        /// Deletes workspace associated with given configuration key.
        /// </summary>
        /// <param name="configurationKey">
        /// The configuration key.
        /// </param>
        public void DeleteWorkspace(WorkspaceConfiguration workspace, string configurationKey)
        {
            var keys = this.AvailableWorkspaces.Where(kvp => kvp.Value == workspace).Select(kvp => kvp.Key).ToList();
            foreach (string key in keys)
            {
                this.AvailableWorkspaces.Remove(key);
            }
            
            #if (!SILVERLIGHT)
            string folderPath = _foldersService.GetConfigurationFolderPath() + TranslateKeyToLocalFolder(configurationKey) + @"\";
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }
            #endif
        }

        /// <summary>
        /// The raise configuration change event.
        /// </summary>
        /// <param name="configurationItem">
        /// The configuration item.
        /// </param>
        /// <param name="property">
        /// The property.
        /// </param>
        public void RaiseConfigurationChangeEvent(object configurationItem, string property)
        {
            if ((!isInitializing) && (!IsUpdatedObjectInRam(configurationItem)))
            {
                isDirty = true;
            }
        }

        /// <summary>
        /// Declare that current state of configuration doesn't contain any unsaved changes 
        /// </summary>
        public void Commit()
        {
            this.isDirty = false;
            this.RaisePropertyChanged(() => AreUnsavedChanges);
        }

        /// <summary>
        /// Return true if Workspace name is valid
        /// </summary>
        /// <param name="workspaceName"></param>
        /// <returns>
        /// Is workspace name valid
        /// </returns>
        public bool ValidWorkspaceName(string projectName, string workspaceName)
        {
            // Following commented lines is how InvalidCharString has been calculated. For SL it is substituted with constant
            // char[] invalidChars = Path.GetInvalidFileNameChars();
            // string s = string.Concat(invalidChars);
            const string InvalidCharString = "\"<>|\0\a\b\t\n\v\f\r:*?\\/";
            
            if (workspaceName.Any(InvalidCharString.Contains))
            {
                return false;
            }

            var futureNameOfFolder = string.Format("{0}/{1}/{2}", this.baseFolder, projectName, workspaceName);
            if ((futureNameOfFolder.Length >= 250) 
             || (workspaceName.Length >= 240)) 
            {
                // The specified path, file name, or both are too long. The fully qualified file name must be less than 260 characters
                // , and the directory name must be less than 248 characters.
                return false;
            }

            if ((Path.Combine(futureNameOfFolder, ResourceConfigurationFile).Length >= 250) ||
                (Path.Combine(futureNameOfFolder, AttributeBaseConfigurationFile).Length >= 250) ||
                (Path.Combine(futureNameOfFolder, CalendarStateConfigurationFile).Length >= 250))

            {
                return false;
            }
            foreach (string attributeName in this.attributeConfigurations.Keys)
            {
                string extAttributePath = Path.Combine(
                    futureNameOfFolder, string.Format(AttributeExtendedConfigurationFile, attributeName));

                List<AttributeExtendedConfiguration> list = this.attributeConfigurations[attributeName].GetExtendedConfiguration();
                if (list.Count > 0)
                {
                    if (extAttributePath.Length >= 250)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #if (!SILVERLIGHT)


        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        public void SaveChanges(bool useDefaultFolder = true)
        {
            string folder = useDefaultFolder ? this.baseFolder : this.ConfigurationFolder;
            if (!this.DirectoryExists(this.ConfigurationFolder))
            {
                this.CreateDirectory(this.ConfigurationFolder);
            }

            this.SaveResourceConfiguration(useDefaultFolder);
            this.SaveBaseAttributeConfiguration(useDefaultFolder);
            this.SaveExtendedAttributeConfiguration(useDefaultFolder);
            this.SaveCalendarStateConfiguration(useDefaultFolder);

            if ((!useDefaultFolder) && (this.CurrentWorkspace != null))
            {
                this.CurrentWorkspace.Save(Path.Combine(folder, WorkspaceSettingsName), SerializationMode.Xml);
            }

            this.isDirty = false;
            this.RaisePropertyChanged(() => AreUnsavedChanges);
        }

        /// <summary>
        /// The save to zip.
        /// </summary>
        /// <param name="zipFilePath">
        /// The zip file path.
        /// </param>
        public void SaveToZip(string zipFilePath)
        {
            this.SaveChanges();
            using (ZipFile zipFile = new ZipFile(zipFilePath))
            {
                this.AddFilesToZip(zipFile);
                zipFile.Save();
            }
        }

        /// <summary>
        /// The add files to zip.
        /// </summary>
        /// <param name="zipFile">
        /// The zip file.
        /// </param>
        public void AddFilesToZip(ZipFile zipFile)
        {
            zipFile.AddFile(Path.Combine(this.ConfigurationFolder, ResourceConfigurationFile), string.Empty);
            zipFile.AddFile(Path.Combine(this.ConfigurationFolder, AttributeBaseConfigurationFile), string.Empty);
            zipFile.AddFile(Path.Combine(this.ConfigurationFolder, CalendarStateConfigurationFile), string.Empty);
            foreach (string attributeName in this.attributeConfigurations.Keys)
            {
                string extAttributePath = Path.Combine(
                    this.ConfigurationFolder, string.Format(AttributeExtendedConfigurationFile, attributeName));
                if (this.FileExists(extAttributePath))
                {
                    zipFile.AddFile(extAttributePath, string.Empty);
                }
            }
        }

        /// <summary>
        /// The load from zip.
        /// </summary>
        /// <param name="zipFilePath">
        /// The zip file path.
        /// </param>
        public void LoadFromZip(string zipFilePath)
        {
            ZipFile zipFile = ZipFile.Read(zipFilePath);
            this.IsInitializing = true;
            Regex regex = new Regex(".*_AtttibuteConfiguration.csv");

            foreach (AttributeConfiguration attributeConfiguration in this.attributeConfigurations.Values)
            {
                attributeConfiguration.TurnToDefault();
            }

            foreach (ZipEntry zipEntry in zipFile)
            {
                if (zipEntry.FileName == ResourceConfigurationFile)
                {
                    this.UpdateResourceConfiguration(zipEntry);
                }

                if (zipEntry.FileName == AttributeBaseConfigurationFile)
                {
                    this.UpdateBaseAttributeConfiguration(zipEntry);
                }

                if (zipEntry.FileName == CalendarStateConfigurationFile)
                {
                    this.UpdateCalendarStateConfiguration(zipEntry);
                }

                if (regex.IsMatch(zipEntry.FileName))
                {
                    int ndx = zipEntry.FileName.LastIndexOf("_AtttibuteConfiguration.csv", StringComparison.Ordinal);
                    string attributeName = zipEntry.FileName.Substring(0, ndx);
                    this.UpdateExtendedAttributeConfiguration(attributeName, zipEntry);
                }
            }

            this.isDirty = true;
            this.IsInitializing = false;
        }

        /// <summary>
        /// Loads existing workspaces.
        /// </summary>
        public void LoadWorkspaces()
        {
            if (Directory.Exists(this.baseFolder))
            {
                this.availableWorkspaces.Clear();

                DirectoryInfo di = new DirectoryInfo(this.baseFolder);
                DirectoryInfo[] directories = di.GetDirectories();

                foreach (DirectoryInfo directoryInfo in directories)
                {
                    string filePath = this.baseFolder + directoryInfo.Name + @"\workspace.dob";
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                            {
                                WorkspaceConfiguration workspaceConfiguration = WorkspaceConfiguration.Load(stream, SerializationMode.Xml);
                                this.availableWorkspaces.Add(workspaceConfiguration.Name, workspaceConfiguration);
                            }
                        }
                        catch (Exception)
                        {
                            // nop
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The load configuration.
        /// </summary>
        private void LoadConfiguration()
        {
            this.isInitializing = true;
            this.LoadResourceConfiguration();
            this.LoadBaseAttributeConfiguration();
            this.LoadExtendedAttributeConfiguration();
            this.LoadCalendarStateConfiguration();
            this.isInitializing = false;
        }

        /// <summary>
        /// The load resource configuration.
        /// </summary>
        private void LoadResourceConfiguration()
        {
            string resConfigurationFilePath = Path.Combine(ConfigurationFolder, ResourceConfigurationFile);
            
            if (FileExists(resConfigurationFilePath))
            {
                using (StreamReader streamReader = new StreamReader(GetFileStream(resConfigurationFilePath, FileMode.Open)))
                {
                    CsvReader csvReader = new CsvReader(streamReader);
                    csvReader.Configuration.RegisterClassMap<ResourceConfigurationClassMap>();

                    IEnumerable<ResourceConfiguration> records =
                        csvReader.GetRecords<ResourceConfiguration>();

                    foreach (ResourceConfiguration resourceConfiguration in records)
                    {
                        resourceConfigurations.Add(resourceConfiguration.Name, resourceConfiguration);
                        resourceConfiguration.PropertyChanged += ConfigurationPropertyChanged;
                    }
                }
            }
        }

        /// <summary>
        /// The load calendar state configuration.
        /// </summary>
        private void LoadCalendarStateConfiguration()
        {
            string calStateConfigurationFilePath = Path.Combine(ConfigurationFolder, CalendarStateConfigurationFile);

            if (FileExists(calStateConfigurationFilePath))
            {
                using (StreamReader streamReader = new StreamReader(GetFileStream(calStateConfigurationFilePath, FileMode.Open)))
                {
                    CsvReader csvReader = new CsvReader(streamReader);
                    csvReader.Configuration.RegisterClassMap<CalendarStateConfigurationClassMap>();

                    IEnumerable<CalendarStateConfiguration> records =
                        csvReader.GetRecords<CalendarStateConfiguration>();

                    foreach (CalendarStateConfiguration calendarStateConfiguration in records)
                    {
                        calendarStateConfigurations.Add(calendarStateConfiguration.Name, calendarStateConfiguration);
                        calendarStateConfiguration.PropertyChanged += ConfigurationPropertyChanged;
                    }
                }
            }
        }        

        /// <summary>
        /// The load base attribute configuration.
        /// </summary>
        private void LoadBaseAttributeConfiguration()
        {
            string resConfigurationFilePath = Path.Combine(this.ConfigurationFolder, AttributeBaseConfigurationFile);

            if (this.FileExists(resConfigurationFilePath))
            {
                using (StreamReader streamReader = new StreamReader(this.GetFileStream(resConfigurationFilePath, FileMode.Open)))
                {
                    try
                    {
                        CsvReader csvReader = new CsvReader(streamReader);
                        csvReader.Configuration.RegisterClassMap<AttributeConfigurationClassMap>();
                        
                        IEnumerable<AttributeConfiguration> records = csvReader.GetRecords<AttributeConfiguration>();
                        foreach (AttributeConfiguration attributeConfiguration in records)
                        {
                            if (!string.IsNullOrEmpty(attributeConfiguration.Name))
                            {
                                attributeConfigurations.Add(attributeConfiguration.Name, attributeConfiguration);
                                attributeConfiguration.PropertyChanged += ConfigurationPropertyChanged;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Error reading file {0}", resConfigurationFilePath), ex);
                    }
                }
            }
        }

        /// <summary>
        /// The load extended attribute configuration.
        /// </summary>
        private void LoadExtendedAttributeConfiguration()
        {
            foreach (string attributeName in this.attributeConfigurations.Keys)
            {
                string extAttributePath = Path.Combine(this.ConfigurationFolder, string.Format(AttributeExtendedConfigurationFile, attributeName));
                if (this.FileExists(extAttributePath))
                {
                    using (StreamReader streamReader = new StreamReader(this.GetFileStream(extAttributePath, FileMode.Open)))
                    {
                        CsvReader csvReader = new CsvReader(streamReader);
                        IEnumerable<AttributeExtendedConfiguration> records =
                            csvReader.GetRecords<AttributeExtendedConfiguration>();

                        foreach (AttributeExtendedConfiguration attributeExtendedConfiguration in records)
                        {
                            this.attributeConfigurations[attributeName].SetAttributeValueColor(attributeExtendedConfiguration.Value, CustomColorConverter.StringToColor(attributeExtendedConfiguration.Color));
                            this.attributeConfigurations[attributeName].SetAttributeValueVisibility(attributeExtendedConfiguration.Value, attributeExtendedConfiguration.Visibility);
                        }
                    }
                }
            }
        }
#endif

        /// <summary>
        /// Occurs when a property of a <see cref="AttributeConfiguration"/> item changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void ConfigurationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {            
            RaiseConfigurationChangeEvent(sender, e.PropertyName);            
        }

        #if (!SILVERLIGHT)
        /// <summary>
        /// The update resource configuration.
        /// </summary>
        /// <param name="zipEntry">
        /// The zip entry.
        /// </param>
        private void UpdateResourceConfiguration(ZipEntry zipEntry)
        {
            MemoryStream ms = new MemoryStream();
            zipEntry.Extract(ms);
            ms.Position = 0;

            using (StreamReader streamReader = new StreamReader(ms))
            {
                CsvReader csvReader = new CsvReader(streamReader);
                csvReader.Configuration.RegisterClassMap<ResourceConfigurationClassMap>();

                IEnumerable<ResourceConfiguration> records =
                    csvReader.GetRecords<ResourceConfiguration>();

                foreach (ResourceConfiguration resourceConfiguration in records)
                {
                    if (this.resourceConfigurations.ContainsKey(resourceConfiguration.Name))
                    {
                        this.resourceConfigurations[resourceConfiguration.Name].IsVisible =
                            resourceConfiguration.IsVisible;

                        this.resourceConfigurations[resourceConfiguration.Name].Position =
                            resourceConfiguration.Position;
                    }
                }
            }
        }

        /// <summary>
        /// The update calendar state configuration.
        /// </summary>
        /// <param name="zipEntry">
        /// The zip entry.
        /// </param>
        private void UpdateCalendarStateConfiguration(ZipEntry zipEntry)
        {
            MemoryStream ms = new MemoryStream();
            zipEntry.Extract(ms);
            ms.Position = 0;

            using (StreamReader streamReader = new StreamReader(ms))
            {
                CsvReader csvReader = new CsvReader(streamReader);
                csvReader.Configuration.RegisterClassMap<CalendarStateConfigurationClassMap>();

                IEnumerable<CalendarStateConfiguration> records =
                    csvReader.GetRecords<CalendarStateConfiguration>();

                foreach (CalendarStateConfiguration calendarStateConfiguration in records)
                {
                    if (calendarStateConfigurations.ContainsKey(calendarStateConfiguration.Name))
                    {
                        calendarStateConfigurations[calendarStateConfiguration.Name].IsVisible =
                            calendarStateConfiguration.IsVisible;

                        calendarStateConfigurations[calendarStateConfiguration.Name].SetColor(calendarStateConfiguration.GetColor());                            
                    }
                }
            }
        }

        /// <summary>
        /// The update base attribute configuration.
        /// </summary>
        /// <param name="zipEntry">
        /// The zip entry.
        /// </param>
        private void UpdateBaseAttributeConfiguration(ZipEntry zipEntry)
        {
            MemoryStream ms = new MemoryStream();
            zipEntry.Extract(ms);
            ms.Position = 0;

            using (StreamReader streamReader = new StreamReader(ms))
            {
                CsvReader csvReader = new CsvReader(streamReader);
                csvReader.Configuration.RegisterClassMap<AttributeConfigurationClassMap>();

                IEnumerable<AttributeConfiguration> records =
                    csvReader.GetRecords<AttributeConfiguration>();

                foreach (AttributeConfiguration attributeConfiguration in records)
                {
                    if (this.attributeConfigurations.ContainsKey(attributeConfiguration.Name))
                    {
                        this.attributeConfigurations[attributeConfiguration.Name].Category =
                            attributeConfiguration.Category;

                        this.attributeConfigurations[attributeConfiguration.Name].Description =
                            attributeConfiguration.Description;

                        this.attributeConfigurations[attributeConfiguration.Name].DetailWindowPosition =
                            attributeConfiguration.DetailWindowPosition;

                        this.attributeConfigurations[attributeConfiguration.Name].EditorPosition =
                            attributeConfiguration.EditorPosition;

                        this.attributeConfigurations[attributeConfiguration.Name].IsColorAttribute =
                            attributeConfiguration.IsColorAttribute;

                        this.attributeConfigurations[attributeConfiguration.Name].IsDisabled =
                            attributeConfiguration.IsDisabled;

                        this.attributeConfigurations[attributeConfiguration.Name].IsInDetailsWindow =
                            attributeConfiguration.IsInDetailsWindow;

                        this.attributeConfigurations[attributeConfiguration.Name].IsInEditor =
                            attributeConfiguration.IsInEditor;

                        this.attributeConfigurations[attributeConfiguration.Name].IsInLabel =
                            attributeConfiguration.IsInLabel;

                        this.attributeConfigurations[attributeConfiguration.Name].IsInTooltip =
                            attributeConfiguration.IsInTooltip;

                        this.attributeConfigurations[attributeConfiguration.Name].LabelPosition =
                            attributeConfiguration.LabelPosition;

                        this.attributeConfigurations[attributeConfiguration.Name].TooltipPosition =
                            attributeConfiguration.TooltipPosition;

                        this.attributeConfigurations[attributeConfiguration.Name].Type =
                            attributeConfiguration.Type;
                    }
                }
            }
        }

        /// <summary>
        /// The update extended attribute configuration.
        /// </summary>
        /// <param name="attributeName">
        /// The attribute name.
        /// </param>
        /// <param name="zipEntry">
        /// The zip entry.
        /// </param>
        private void UpdateExtendedAttributeConfiguration(string attributeName, ZipEntry zipEntry)
        {
            if (!this.attributeConfigurations.ContainsKey(attributeName))
            {
                return;
            }

            MemoryStream ms = new MemoryStream();
            zipEntry.Extract(ms);
            ms.Position = 0;

            using (StreamReader streamReader = new StreamReader(ms))
            {
                CsvReader csvReader = new CsvReader(streamReader);
                IEnumerable<AttributeExtendedConfiguration> records =
                    csvReader.GetRecords<AttributeExtendedConfiguration>();

                foreach (AttributeExtendedConfiguration attributeExtConfiguration in records)
                {
                    this.attributeConfigurations[attributeName].SetAttributeValueColor(
                        attributeExtConfiguration.Value,
                        CustomColorConverter.StringToColor(attributeExtConfiguration.Color));

                    this.attributeConfigurations[attributeName].SetAttributeValueVisibility(
                        attributeExtConfiguration.Value,
                        attributeExtConfiguration.Visibility);
                }
            }
        }

        /// <summary>
        /// The save resource configuration.
        /// </summary>
        private void SaveResourceConfiguration(bool useDefaultFolder = true)
        {
            string folder = useDefaultFolder ? this.baseFolder : this.ConfigurationFolder;
            string resConfigurationFilePath = Path.Combine(folder, ResourceConfigurationFile);
            using (StreamWriter streamWriter = new StreamWriter(this.GetFileStream(resConfigurationFilePath, FileMode.OpenOrCreate)))
            {
                CsvWriter csvWriter = new CsvWriter(streamWriter);
                csvWriter.Configuration.RegisterClassMap<ResourceConfigurationClassMap>();

                IEnumerable configurations = this.resourceConfigurations.Select(kvp => kvp.Value);
                csvWriter.WriteRecords(configurations);
            }
        }

        /// <summary>
        /// The save calendar state configuration.
        /// </summary>
        private void SaveCalendarStateConfiguration(bool useDefaultFolder = true)
        {
            // if there is no calendar states there is no sense to save configuration of nothing
            if (!this.calendarStateConfigurations.Any())
            {
                return;
            }

            string folder = useDefaultFolder ? this.baseFolder : this.ConfigurationFolder;
            string calStateConfigurationFilePath = Path.Combine(folder, CalendarStateConfigurationFile);
            using (StreamWriter streamWriter = new StreamWriter(this.GetFileStream(calStateConfigurationFilePath, FileMode.OpenOrCreate)))
            {
                CsvWriter csvWriter = new CsvWriter(streamWriter);
                csvWriter.Configuration.RegisterClassMap<CalendarStateConfigurationClassMap>();

                IEnumerable calStateConf = this.calendarStateConfigurations.Select(kvp => kvp.Value);
                csvWriter.WriteRecords(calStateConf);
            }
        }

        /// <summary>
        /// The save base attribute configuration.
        /// </summary>
        private void SaveBaseAttributeConfiguration(bool useDefaultFolder = true)
        {
            string folder = useDefaultFolder ? this.baseFolder : this.ConfigurationFolder;
            string baseAttributePath = Path.Combine(folder, AttributeBaseConfigurationFile);
            using (StreamWriter streamWriter = new StreamWriter(this.GetFileStream(baseAttributePath, FileMode.OpenOrCreate)))
            {
                CsvWriter csvWriter = new CsvWriter(streamWriter);
                csvWriter.Configuration.RegisterClassMap<AttributeConfigurationClassMap>();

                IEnumerable records = this.AttributeConfigurations.Select(kvp => kvp.Value);
                csvWriter.WriteRecords(records);
            }
        }

        /// <summary>
        /// The save extended attribute configuration.
        /// </summary>
        private void SaveExtendedAttributeConfiguration(bool useDefaultFolder = true)
        {
            string folder = useDefaultFolder ? this.baseFolder : this.ConfigurationFolder;
            var attributes = attributeConfigurations.Keys.ToList();
            foreach (string attributeName in attributes)
            {
                string extAttributePath = Path.Combine(
                    folder, string.Format(AttributeExtendedConfigurationFile, attributeName));

                AttributeConfiguration currentAttribute = this.attributeConfigurations[attributeName];
                List<AttributeExtendedConfiguration> list = currentAttribute.GetExtendedConfiguration();
                if (list.Count > 0)
                {
                    using (StreamWriter streamWriter = new StreamWriter(this.GetFileStream(extAttributePath, FileMode.OpenOrCreate)))
                    {
                        CsvWriter csvWriter = new CsvWriter(streamWriter);
                        csvWriter.WriteRecords(list as IEnumerable);
                    }
                }
                else
                {
                    if (this.FileExists(extAttributePath))
                    {
                        this.DeleteFile(extAttributePath);
                    }
                }
            }
        }
        #endif

        /// <summary>
        /// Shows if object is in ram and should not be queued for update.
        /// </summary>
        /// <param name="configurationItem">Updated object.</param>
        /// <returns>The value indicating whether object is in ram or not.</returns>
        private bool IsUpdatedObjectInRam(object configurationItem)
        {
            if (configurationItem is ResourceConfiguration)
            {
                ResourceConfiguration resourceConfiguration = configurationItem as ResourceConfiguration;
                if (this.ramResourceConfigurations.ContainsKey(resourceConfiguration.Name))
                {
                    return this.ramResourceConfigurations[resourceConfiguration.Name] == resourceConfiguration;
                }
            }

            return false;
        }

        #if (!SILVERLIGHT)
        /// <summary>
        /// The directory exists.
        /// </summary>
        /// <param name="directory">
        /// The directory.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool DirectoryExists(string directory)
        {
            return Directory.Exists(directory);
        }

        /// <summary>
        /// The create directory.
        /// </summary>
        /// <param name="directory">
        /// The directory.
        /// </param>
        private void CreateDirectory(string directory)
        {
            Directory.CreateDirectory(directory);
        }

        /// <summary>
        /// The file exists.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool FileExists(string filename)
        {
            return File.Exists(filename);
        }

        /// <summary>
        /// The delete file.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        private void DeleteFile(string filename)
        {
            File.Delete(filename);
        }

        /// <summary>
        /// The get file stream.
        /// </summary>
        /// <param name="filepath">
        /// The file path.
        /// </param>
        /// <param name="mode">
        /// The mode.
        /// </param>
        /// <returns>
        /// The <see cref="FileStream"/>.
        /// </returns>
        private FileStream GetFileStream(string filepath, FileMode mode)
        {
            return new FileStream(filepath, mode);
        }
        #endif

        #endregion
    }
}