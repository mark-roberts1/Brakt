using System;
using System.Reflection;

namespace Brakt.Rest.Database
{
    /// <summary>
    /// Represents an expection that occurred during the mapping of an object.
    /// </summary>
    public class MapException : Exception
    {
        /// <summary>
        /// Contructs an instance of <see cref="MapException"/>
        /// </summary>
        /// <param name="property">The property that failed to be mapped</param>
        /// <param name="value">The value that failed to map to the property</param>
        /// <param name="ex">The exception that was thrown during mapping</param>
        public MapException(PropertyInfo property, object value, Exception ex) 
            : base($"Error mapping {value} to {property.DeclaringType.Name}.{property.Name} {ex.Message}", ex)
        {
        }
    }
}
