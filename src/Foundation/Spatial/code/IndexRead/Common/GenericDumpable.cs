// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericDumpable.cs" company="Aceik">
//   
// </copyright>
// <summary>
//   Defines the GenericDumpable type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using Sitecore.ContentSearch.Linq.Common;

namespace Aceik.Foundation.CloudSpatialSearch.IndexRead.Common
{
    public class GenericDumpable : IDumpable
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        protected object Value { get; set; }

        public GenericDumpable(object value)
        {
            this.Value = value;
        }

        public virtual void WriteTo(TextWriter writer)
        {
            IDumpable dumpable = this.Value as IDumpable;
            if (dumpable != null)
                dumpable.WriteTo(writer);
            else
                writer.WriteLine(this.Value);
        }
    }
}