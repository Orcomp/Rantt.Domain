// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Project.cs" company="Orcomp">
//   Copyright Orcomp
// </copyright>
// <summary>
//   The project.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Xml.Serialization;

    using CsvHelper;

    using Rantt.Domain.Configuration;
    using Rantt.Domain.Configuration.CsvHelperConfiguration;

    /// <summary>
    /// The project.
    /// </summary>
    /// <typeparam name="T">
    /// the type parameter
    /// </typeparam>
    #if (!SILVERLIGHT)
    [Serializable]
    #endif
    public class Project<T> where T : IComparable<T>
    {
        private string sourceFilePath = string.Empty;

        private string sourceColorsFilePath = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Project{T}"/> class.
        /// </summary>
        public Project()
        {
            this.Id = Guid.NewGuid().ToString();
            this.StartDate = new DateTime(1753, 1, 1);
            this.EndDate = new DateTime(9999, 12, 31);
            this.Culture = Thread.CurrentThread.CurrentCulture.Name;
            this.FirstDayOfWeek = DayOfWeek.Monday;
            this.ShortTimeFormat = ShortTimeFormat.TwelveHours;
            this.StartupSettings = new StartupSettings { StartViewIntervalType = StartViewIntervalType.FitToScreen, Duration = 1, DurationUnitOfTime = UnitOfTime.Day };
            this.DefaultColorsOverridings = new Dictionary<KeyValuePair<string, string>, string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Project{T}"/> class.
        /// </summary>
        /// <param name="initializeFirstDataSet">
        /// The initialize first data set.
        /// </param>
        public Project(bool initializeFirstDataSet) : this()
        {
            if (initializeFirstDataSet)
            {
                this.DataSetDescriptors = new List<DataSetDescriptor>
                                              {
                                                  new DataSetDescriptor
                                                      {
                                                          OperationDataSource = new DataSource
                                                                  {
                                                                      IntervalEntityType = IntervalEntityType.Operation, 
                                                                      ConnectionString = string.Empty,
                                                                      TableName = "Operations", 
                                                                      DefaultAttribute = "Resource",
                                                                      StartDate = this.StartDate,
                                                                      WhereStart = this.StartDate, 
                                                                      WhereEnd = this.EndDate
                                                                  },
                                                          OperationRelationshipDataSource = new DataSource
                                                                  {
                                                                      IntervalEntityType = IntervalEntityType.Relationship,
                                                                      ConnectionString = string.Empty,
                                                                      TableName = "OperationRelationships",
                                                                      WhereStart = this.StartDate,
                                                                      WhereEnd = DateTime.MaxValue, 
                                                                      StartDate = this.StartDate,
                                                                      IsEmpty = true // while project manager doesn't support operation relationship pages default value is null
                                                                  },
                                                          CalendarPeriodDataSource = new DataSource
                                                                  {
                                                                      IntervalEntityType = IntervalEntityType.CalendarPeriod,
                                                                      ConnectionString = string.Empty,
                                                                      TableName = "CalendarPeriods",
                                                                      WhereStart = this.StartDate, 
                                                                      WhereEnd = this.EndDate,
                                                                      StartDate = this.StartDate,
                                                                      IsEmpty = true // by default no calendar periods
                                                                  }
                                                      }
                                              };
            }
        }

        #region Public Properties

        /// <summary>
        /// Gets or sets the unique project identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        public int Configuration { get; set; }

        /// <summary>
        /// Gets or sets the data set descriptor.
        /// </summary>
        public List<DataSetDescriptor> DataSetDescriptors { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether local configuration.
        /// </summary>
        public bool LocalConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the first week day.
        /// </summary>
        public DayOfWeek FirstDayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets default short time format.
        /// </summary>
        public ShortTimeFormat ShortTimeFormat { get; set; }

        public StartupSettings StartupSettings { get; set; }

        public AutoUpdateSettings AutoUpdateSettings { get; set; }

        /// <summary>
        /// Gets or sets operation attribute colors system shouls use if there is nothing else defined.
        /// </summary>
        #if (!SILVERLIGHT)
        [XmlIgnore]
        #endif
        public Dictionary<KeyValuePair<string, string>, string> DefaultColorsOverridings { get; set; } 

        #endregion

        /// <summary>
        /// Gets overriden color for given attribute value
        /// </summary>
        /// <param name="attrName"></param>
        /// <param name="attrvalue"></param>
        /// <returns></returns>
        public string GetOverridenColor(string attrName, object attrvalue)
        {
            KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(attrName, attrvalue == null ? string.Empty : attrvalue.ToString());
            return DefaultColorsOverridings.ContainsKey(kvp) ? DefaultColorsOverridings[kvp] : null;
        }
       
        #if (!SILVERLIGHT)
        /// <summary>
        /// The load.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object Load(string filePath)
        {
            XmlSerializer xmlSerializer = GetSerializer<T>();

            using (StreamReader sr = new StreamReader(filePath))
            {
                Project<T> project = (Project<T>)xmlSerializer.Deserialize(sr);
                project.MakeAllPathsAbsolute(filePath);
                
                project.sourceFilePath = filePath;

                project.sourceColorsFilePath = Path.Combine(Path.GetDirectoryName(filePath), "Colors.csv");

                project.UpdateColorOverridings();

                return project;
            }
        }

        /// <summary>
        /// Updates project colors.
        /// </summary>
        public void UpdateColorOverridings()
        {
            this.DefaultColorsOverridings.Clear();

            if (File.Exists(this.sourceColorsFilePath))
            {
                using (StreamReader streamReader = new StreamReader(new FileStream(this.sourceColorsFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                {
                    using (CsvReader csvReader = new CsvReader(streamReader))
                    {
                        csvReader.Configuration.RegisterClassMap<AttributeValueOverrideColorClassMap>();
                        csvReader.Configuration.TrimFields = true;

                        IEnumerable<AttributeValueOverrideColor> records =
                            csvReader.GetRecords<AttributeValueOverrideColor>();

                        foreach (AttributeValueOverrideColor overrideColor in records)
                        {
                            KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(overrideColor.AttributeName,
                                overrideColor.AttributeValue);

                            if (!this.DefaultColorsOverridings.ContainsKey(kvp))
                            {
                                this.DefaultColorsOverridings.Add(kvp, overrideColor.Color);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The make all paths relative.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        public void MakeAllPathsRelative(string filePath)
        {
            Uri basepath = new Uri(filePath);
            foreach (DataSetDescriptor dataSetDescriptor in this.DataSetDescriptors)
            {
                if (!string.IsNullOrEmpty(dataSetDescriptor.OperationDataSource.FilePath))
                {
                    Uri fileUri = new Uri(dataSetDescriptor.OperationDataSource.FilePath);
                    dataSetDescriptor.OperationDataSource.FilePath = UrlHelper.Decode(basepath.MakeRelativeUri(fileUri).ToString());
                }

                if (!string.IsNullOrEmpty(dataSetDescriptor.CalendarPeriodDataSource.FilePath))
                {
                    Uri fileUri = new Uri(dataSetDescriptor.CalendarPeriodDataSource.FilePath);
                    dataSetDescriptor.CalendarPeriodDataSource.FilePath = UrlHelper.Decode(basepath.MakeRelativeUri(fileUri).ToString());
                }
            }
        }

        /// <summary>
        /// Clears data fields which is not needed
        /// </summary>
        public void RemoveNotNeededData()
        {
            foreach (DataSetDescriptor dataSetDescriptor in this.DataSetDescriptors)
            {
                dataSetDescriptor.RemoveNotNeededData();
            }
        }

        /// <summary>
        /// Gets unescaped file path from base path and file path.
        /// </summary>
        /// <param name="basepath">Base path</param>
        /// <param name="filePath">File path</param>
        /// <returns>Unescaped file path</returns>
        private static string GetAbsolutePath(Uri basepath, string filePath)
        {
            Uri fileUri = new Uri(basepath, filePath);
            return UrlHelper.Decode(fileUri.AbsolutePath);
        }

        /// <summary>
        /// The make all paths absolute.
        /// </summary>
        /// <param name="filePath">
        /// The base file path.
        /// </param>
        public void MakeAllPathsAbsolute(string filePath, bool silentException = false)
        {
            Uri basepath = new Uri(filePath);
            foreach (DataSetDescriptor dataSetDescriptor in this.DataSetDescriptors)
            {
                if (!string.IsNullOrEmpty(dataSetDescriptor.OperationDataSource.FilePath))
                {
                    Uri fileUri = new Uri(basepath, dataSetDescriptor.OperationDataSource.FilePath);
                    dataSetDescriptor.OperationDataSource.FilePath = Uri.UnescapeDataString(fileUri.AbsolutePath.Replace("/", @"\"));
                }
                else
                {
                    if (!silentException && dataSetDescriptor.OperationDataSource.DataSourceType == DataSourceType.Csv)
                    {
                        throw new Exception("Invalid dataset descriptor. Operation data source must be defined.");
                    }
                }

                if (!string.IsNullOrEmpty(dataSetDescriptor.OperationRelationshipDataSource.FilePath))
                {
                    Uri fileUri = new Uri(basepath, dataSetDescriptor.OperationRelationshipDataSource.FilePath);
                    dataSetDescriptor.OperationRelationshipDataSource.FilePath = Uri.UnescapeDataString(fileUri.AbsolutePath.Replace("/", @"\"));
                }

                if (!string.IsNullOrEmpty(dataSetDescriptor.CalendarPeriodDataSource.FilePath))
                {
                    Uri fileUri = new Uri(basepath, dataSetDescriptor.CalendarPeriodDataSource.FilePath);
                    dataSetDescriptor.CalendarPeriodDataSource.FilePath = Uri.UnescapeDataString(fileUri.AbsolutePath.Replace("/", @"\"));
                }
            }
        }

        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        /// <param name="makeRelativePaths">
        /// The make relative paths.
        /// </param>
        /// <param name="removeNotNeededData">
        /// The remove not needed data.
        /// </param>
        public void Save(string filePath, bool makeRelativePaths = true, bool removeNotNeededData = true)
        {
            XmlSerializer xmlSerializer = GetSerializer<T>();

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                if (removeNotNeededData)
                {
                    this.RemoveNotNeededData();
                }

                if (makeRelativePaths)
                {
                    this.MakeAllPathsRelative(filePath);
                }

                xmlSerializer.Serialize(sw, this);

                if (makeRelativePaths)
                {
                    // In scenario we deliberately save non validated project we do not need exception thrown.
                    this.MakeAllPathsAbsolute(filePath, true);
                }
            }
        }

        /// <summary>
        /// The get serializer.
        /// </summary>
        /// <typeparam name="T">
        /// Project type
        /// </typeparam>
        /// <returns>
        /// The <see cref="XmlSerializer"/>.
        /// </returns>
        #pragma warning disable 693
        private static XmlSerializer GetSerializer<T>()
        #pragma warning restore 693
        {
            Type genericType = typeof(Project<>);
            Type instanceType = genericType.MakeGenericType(new[] { typeof(T) });
            return new XmlSerializer(
                instanceType,
                new[] { typeof(DataSetDescriptor), typeof(List<DataSetDescriptor>), typeof(DateRepresentation), typeof(DataSource) });
        }
        #endif
    }

    /// <summary>
    /// URL encoding class.  Note: use at your own risk.
    /// Written by: Ian Hopkins (http://www.lucidhelix.com)
    /// Date: 2008-Dec-23
    /// (Ported to C# by t3rse (http://www.t3rse.com))
    /// </summary>
    public class UrlHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Encode(string str)
        {
            var charClass = String.Format("0-9a-zA-Z{0}", Regex.Escape("-_.!~*'()"));
            return Regex.Replace(str,
                String.Format("[^{0}]", charClass),
                new MatchEvaluator(EncodeEvaluator));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public static string EncodeEvaluator(Match match)
        {
            return (match.Value == " ") ? "+" : String.Format("%{0:X2}", Convert.ToInt32(match.Value[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public static string DecodeEvaluator(Match match)
        {
            return Convert.ToChar(int.Parse(match.Value.Substring(1), System.Globalization.NumberStyles.HexNumber)).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Decode(string str)
        {
            return Regex.Replace(str.Replace('+', ' '), "%[0-9a-zA-Z][0-9a-zA-Z]", new MatchEvaluator(DecodeEvaluator));
        }
    }
}