// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpatialPointValueConverter.cs" company="Aceik">
//   
// </copyright>
// <summary>
//   Defines the SpatialPointValueConverter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Foundation.Spatial.DataTypes.Converters
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class SpatialPointValueConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(SpatialPoint))
                return true;
            else
                return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;
            else
                return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if ((value is string))
                return new SpatialPoint((string)value);
            return new SpatialPoint();
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return ((SpatialPoint)value).ToString();
        }
    }
}