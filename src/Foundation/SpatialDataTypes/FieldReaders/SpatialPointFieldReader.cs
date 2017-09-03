// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpatialPointFieldReader.cs" company="Aceik">
//   
// </copyright>
// <summary>
//   Defines the SpatialPointFieldReader type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Foundation.Spatial.DataTypes.FieldReaders
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.FieldReaders;
    using Sitecore.Data.Fields;

    public class SpatialPointFieldReader : FieldReader
    {
        public override object GetFieldValue(IIndexableDataField indexableField)
        {
            if (!(indexableField is SitecoreItemDataField))
                return indexableField.Value;
            var field = (Field)(indexableField as SitecoreItemDataField);
            if (!string.IsNullOrEmpty(field.Value))
                return new SpatialPoint(field.Value);
            
            return null;
        }
    }
}