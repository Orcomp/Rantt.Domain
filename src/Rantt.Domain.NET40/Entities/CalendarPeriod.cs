//-----------------------------------------------------------------------
// <copyright file="CalendarPeriod.cs" company="Orcomp">
//     Copyright (c) 2013 Orcomp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Rantt.Domain.Entities
{
    using System;
    using System.Collections.Generic;

    using CsvHelper.Configuration;
    using Exceptions;

    /// <summary>
    /// CsvCalendarPeriod class.
    /// </summary>
    public class CalendarPeriod : ICalendarPeriod
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarPeriod"/> class.
        /// </summary>
        public CalendarPeriod()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarPeriod"/> class.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        public CalendarPeriod(Dictionary<string, object> values)
        {
            ReadFixedFields(values);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarPeriod"/> class.
        /// </summary>
        /// <param name="fieldReader">
        /// The field reader.
        /// </param>
        public CalendarPeriod(dynamic fieldReader)
        {
            ReadFixedFields(fieldReader);
        }

        /// <summary>
        /// Gets or sets resource name.
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// Gets or sets the calendar state
        /// </summary>
        public string CalendarState { get; set; }

        /// <summary>
        /// Gets or sets start time.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets end time.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets the period start time.
        /// </summary>
        public DateTime PeriodStartTime
        {
            get
            {
                return this.StartTime;
            }
        }

        /// <summary>
        /// Gets the period end time.
        /// </summary>
        public DateTime PeriodEndTime
        {
            get
            {
                return this.EndTime;
            }
        }

        /// <summary>
        /// Reads fields from given source.
        /// </summary>
        /// <param name="fieldReader">source of fields.</param>
        private void ReadFixedFields(dynamic fieldReader)
        {
            this.StartTime = ReadDateTimeField(fieldReader, FixedFieldNames.StartTime);
            this.EndTime = ReadDateTimeField(fieldReader, FixedFieldNames.EndTime);
            this.Resource = fieldReader.GetField(FixedFieldNames.Resource);
            this.CalendarState = fieldReader.GetField("CalendarState");
        }

        /// <summary>
        /// Reads fields from given source.
        /// </summary>
        /// <param name="values">source of fields.</param>
        private void ReadFixedFields(Dictionary<string, object> values)
        {
            this.StartTime = (DateTime)this.ConvertRawValue(values[FixedFieldNames.StartTime]);
            this.EndTime = (DateTime)this.ConvertRawValue(values[FixedFieldNames.EndTime]);
            this.Resource = (string)values[FixedFieldNames.Resource];
            this.CalendarState = values["CalendarState"] == null ? null : values["CalendarState"].ToString();
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
            return Convert.ToDateTime(value);
        }

        private DateTime ReadDateTimeField(dynamic fieldReader, string fieldName)
        {
            try
            {
                return fieldReader.GetField<DateTime>(fieldName);
            }
            catch (CsvHelper.TypeConversion.CsvTypeConverterException ex)
            {
                string message = string.Format("Unable to convert {0} value of {1} field to correct type. Consider changing project culture if value is correct.",
                    fieldReader.GetField(fieldName) == null ? string.Empty : fieldReader.GetField(fieldName).ToString(),
                    fieldName);

                throw new TypeConversionException(message, ex);
            }
        }

    }
}
