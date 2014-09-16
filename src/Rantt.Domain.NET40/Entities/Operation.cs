// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Operation.cs" company="Cherry development team">
//   Copyright (c) 2008 - 2013 Cherry development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rantt.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using CsvHelper;
    using Exceptions;
    using Helpers;

    /// <summary>
    /// CsvOperation class.
    /// </summary>
    /// <typeparam name="T">
    /// Date time type parameter.
    /// </typeparam>
    public class Operation<T> : IOperation<T>
    {
        #region Fields
        /// <summary>
        /// Attributes associated with the operation.
        /// </summary>
        private readonly AttributeCollection _attributes = new AttributeCollection();

        /// <summary>
        /// The fixed headers.
        /// </summary>
        private readonly HashSet<string> _fixedHeaders = new HashSet<string> { "StartTime", "EndTime", "SetupStartTime", "TearDownStartTime", "Reference", "Resource" };
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Operation{T}"/> class.
        /// </summary>
        public Operation()
        {
            ResourceAttributeName = FixedFieldNames.Resource;
            UniqueId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Operation{T}"/> class. 
        /// Initializes a new instance of the Operation class, reading data from a
        /// currently open/active field reader that is positioned at the record that contains
        /// all details of the operation.
        /// </summary>
        /// <param name="fieldReader">
        /// Currently open/active field reader that is positioned at the record to be read.
        /// </param>
        public Operation(dynamic fieldReader)
            : this()
        {
            ReadFixedOperationFields(fieldReader);
            ReadOperationAttributes(fieldReader);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Operation{T}"/> class.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        public Operation(Dictionary<string, object> values)
            : this()
        {
            ReadFixedOperationFields(values);
            ReadOperationAttributes(values);
        }
        #endregion

        #region IOperation<T> Members
        /// <summary>
        /// Gets resource name.
        /// </summary>
        public string Resource
        {
            get { return _attributes[ResourceAttributeName]; }
        }

        /// <summary>
        /// Gets or sets setup start time.
        /// </summary>
        public T SetupStartTime { get; set; }

        /// <summary>
        /// Gets or sets start time.
        /// </summary>
        public T StartTime { get; set; }

        /// <summary>
        /// Gets or sets tear down start time.
        /// </summary>
        public T TearDownStartTime { get; set; }

        /// <summary>
        /// Gets or sets end time.
        /// </summary>
        public T EndTime { get; set; }

        /// <summary>
        /// Gets or sets Reference.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Gets the unique id.
        /// </summary>
        public string UniqueId { get; private set; }

        /// <summary>
        /// Gets or sets the resource attribute name.
        /// </summary>
        public string ResourceAttributeName { get; set; }

        /// <summary>
        /// Gets or sets attributes associated with the operation.
        /// </summary>
        public IAttributeCollection Attributes
        {
            get { return _attributes; }

            set
            {
                _attributes.Clear();

                foreach (var attributeName in value.Names)
                {
                    _attributes.Add(attributeName, value[attributeName]);
                }
            }
        }

        /// <summary>
        /// Gets or sets the intervals associated with an operation.
        /// </summary>
        public IEnumerable<Interval> Intervals { get; set; }

        /// <summary>
        /// Gets the period start time.
        /// </summary>
        public DateTime PeriodStartTime
        {
            get
            {
                if (typeof (T) == typeof (double))
                {
// ReSharper disable ImpureMethodCallOnReadonlyValueField
                    return DateTime.MinValue.AddSeconds(Convert.ToDouble(StartTime));

// ReSharper restore ImpureMethodCallOnReadonlyValueField
                }

                return Convert.ToDateTime(StartTime);
            }
        }

        /// <summary>
        /// Gets the period end time.
        /// </summary>
        public DateTime PeriodEndTime
        {
            get
            {
                if (typeof (T) == typeof (double))
                {
// ReSharper disable ImpureMethodCallOnReadonlyValueField
                    return DateTime.MinValue.AddSeconds(Convert.ToDouble(EndTime));

// ReSharper restore ImpureMethodCallOnReadonlyValueField
                }

                return Convert.ToDateTime(EndTime);
            }
        }

        /// <summary>
        /// The save to csv.
        /// </summary>
        /// <param name="csvWriter">
        /// The csv writer.
        /// </param>
        public void SaveToCsv(CsvWriter csvWriter)
        {
            csvWriter.WriteField(Reference);
            csvWriter.WriteField(Resource);
            csvWriter.WriteField(StartTime);
            csvWriter.WriteField(EndTime);
            csvWriter.WriteField(SetupStartTime);
            csvWriter.WriteField(TearDownStartTime);

            foreach (KeyValuePair<string, string> keyValuePair in _attributes)
            {
                if (!_fixedHeaders.Contains(keyValuePair.Key))
                {
                    csvWriter.WriteField(keyValuePair.Value);
                }
            }

            csvWriter.NextRecord();
        }

        /// <summary>
        /// The write csv header.
        /// </summary>
        /// <param name="csvWriter">
        /// The csv writer.
        /// </param>
        public void WriteCsvHeader(CsvWriter csvWriter)
        {
            csvWriter.WriteField("Reference");
            csvWriter.WriteField("Resource");
            csvWriter.WriteField("StartTime");
            csvWriter.WriteField("EndTime");
            csvWriter.WriteField("SetupStartTime");
            csvWriter.WriteField("TearDownStartTime");

            foreach (KeyValuePair<string, string> keyValuePair in _attributes)
            {
                if (!_fixedHeaders.Contains(keyValuePair.Key))
                {
                    csvWriter.WriteField(keyValuePair.Key);
                }
            }

            csvWriter.NextRecord();
        }
        #endregion

        #region Methods
        private T ReadField(dynamic fieldReader, string fieldName)
        {
            try
            {
                return fieldReader.GetField<T>(fieldName);
            }
            catch (CsvHelper.TypeConversion.CsvTypeConverterException ex)
            {
                string message = string.Format("Unable to convert {0} value of {1} field to correct type. Consider changing project culture if value is correct.", 
                    fieldReader.GetField(fieldName) == null ? "empty" : fieldReader.GetField(fieldName).ToString(),
                    fieldName);

                throw new TypeConversionException(message, ex);
            }
        }

        /// <summary>
        /// Reads operation fields that have fixed/expected names.
        /// </summary>
        /// <param name="fieldReader">
        /// The field reader.
        /// </param>
        private void ReadFixedOperationFields(dynamic fieldReader)
        {
            StartTime = this.ReadField(fieldReader, FixedFieldNames.StartTime);
            EndTime = this.ReadField(fieldReader, FixedFieldNames.EndTime);

            var fieldHeaders = new HashSet<string>(fieldReader.FieldHeaders);

            SetupStartTime = fieldHeaders.Contains(FixedFieldNames.SetupStartTime)
                                 ? this.ReadField(fieldReader, FixedFieldNames.SetupStartTime)
                                 : StartTime;

            TearDownStartTime = fieldHeaders.Contains(FixedFieldNames.TearDownStartTime)
                                 ? this.ReadField(fieldReader, FixedFieldNames.TearDownStartTime)
                                 : EndTime;

            Reference = fieldHeaders.Contains(FixedFieldNames.Reference)
                            ? fieldReader.GetField(FixedFieldNames.Reference)
                            : ReferenceGenerator.Instance.NextReferenceNumber;
        }

        /// <summary>
        /// The convert raw value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        private object ConvertRawValue(object value)
        {
            if (typeof (T) == typeof (double))
            {
                return Convert.ToDouble(value);
            }

            if (typeof (T) == typeof (DateTime))
            {
                return Convert.ToDateTime(value);
            }

            return (T) value;
        }

        /// <summary>
        /// The read fixed operation fields.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        private void ReadFixedOperationFields(Dictionary<string, object> values)
        {
            StartTime = (T) ConvertRawValue(values[FixedFieldNames.StartTime]);
            EndTime = (T) ConvertRawValue(values[FixedFieldNames.EndTime]);

            SetupStartTime = values.ContainsKey(FixedFieldNames.SetupStartTime)
                                 ? (T) ConvertRawValue(values[FixedFieldNames.SetupStartTime])
                                 : StartTime;

            TearDownStartTime = values.ContainsKey(FixedFieldNames.TearDownStartTime)
                                 ? (T)ConvertRawValue(values[FixedFieldNames.TearDownStartTime])
                                 : EndTime;

            Reference = values.ContainsKey(FixedFieldNames.Reference)
                            ? values[FixedFieldNames.Reference].ToString()
                            : ReferenceGenerator.Instance.NextReferenceNumber;
        }

        /// <summary>
        /// The read operation attributes.
        /// </summary>
        /// <param name="fieldReader">
        /// The field reader.
        /// </param>
        private void ReadOperationAttributes(dynamic fieldReader)
        {
            foreach (string field in fieldReader.FieldHeaders)
            {
                if (field.Trim() == string.Empty)
                {
                    continue;
                }

                _attributes.Add(field, fieldReader.GetField(field));
            }
        }

        /// <summary>
        /// The read operation attributes.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        private void ReadOperationAttributes(Dictionary<string, object> values)
        {
            foreach (string field in values.Keys)
            {
                if (field.Trim() == string.Empty)
                {
                    continue;
                }

                _attributes.Add(field, values[field] == null ? string.Empty : values[field].ToString());
            }
        }
        #endregion
    }
}