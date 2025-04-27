using System.Reflection;

namespace MonthlyExpenseTracker.Helper.Mapper
{
    public static class Mapper
    {
        /// <summary>
        /// Maps a source object to a new destination object
        /// </summary>
        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            if (source == null)
                return default;

            // Create instance without requiring parameterless constructor
            TDestination destination = (TDestination)Activator.CreateInstance(typeof(TDestination));
            Map(source, destination);
            return destination;
        }

        /// <summary>
        /// Maps a source object to an existing destination object
        /// </summary>
        public static void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            if (source == null || destination == null)
                return;

            Type sourceType = typeof(TSource);
            Type destinationType = typeof(TDestination);

            // Get all the properties from source type that have public getters
            var sourceProperties = sourceType.GetProperties()
                .Where(p => p.CanRead);

            // Get all the properties from destination type that have public setters
            var destinationProperties = destinationType.GetProperties()
                .Where(p => p.CanWrite)
                .ToDictionary(p => p.Name);

            foreach (var sourceProp in sourceProperties)
            {
                // Check if destination has a property with the same name
                if (destinationProperties.TryGetValue(sourceProp.Name, out PropertyInfo destProp))
                {
                    // Get the value from source
                    object value = sourceProp.GetValue(source);

                    if (value != null)
                    {
                        // Try to set the value directly if types are compatible
                        if (destProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
                        {
                            destProp.SetValue(destination, value);
                        }
                        // Try type conversion for primitive types
                        else if (IsConvertible(sourceProp.PropertyType, destProp.PropertyType))
                        {
                            try
                            {
                                object convertedValue = Convert.ChangeType(value, destProp.PropertyType);
                                destProp.SetValue(destination, convertedValue);
                            }
                            catch (InvalidCastException)
                            {
                                // Skip properties that can't be converted
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Maps a collection of source objects to a collection of destination objects
        /// </summary>
        public static List<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> sources)
        {
            if (sources == null)
                return new List<TDestination>();

            return sources.Select(Map<TSource, TDestination>).ToList();
        }

        /// <summary>
        /// Determines if source type can be converted to destination type
        /// </summary>
        private static bool IsConvertible(Type sourceType, Type destinationType)
        {
            // Handle nullable types
            Type actualSourceType = Nullable.GetUnderlyingType(sourceType) ?? sourceType;
            Type actualDestType = Nullable.GetUnderlyingType(destinationType) ?? destinationType;

            // Check if source and destination are compatible numeric types
            if ((IsNumericType(actualSourceType) && IsNumericType(actualDestType)) ||
                (actualDestType == typeof(string)) ||
                (actualSourceType == typeof(string) && IsParseable(actualDestType)))
            {
                return true;
            }

            // Check if destination type can be created from the source type
            return destinationType.IsAssignableFrom(sourceType);
        }

        /// <summary>
        /// Determines if the type is a numeric type
        /// </summary>
        private static bool IsNumericType(Type type)
        {
            return type == typeof(byte) || type == typeof(sbyte) ||
                   type == typeof(short) || type == typeof(ushort) ||
                   type == typeof(int) || type == typeof(uint) ||
                   type == typeof(long) || type == typeof(ulong) ||
                   type == typeof(float) || type == typeof(double) ||
                   type == typeof(decimal);
        }

        /// <summary>
        /// Determines if the type can be parsed from a string
        /// </summary>
        private static bool IsParseable(Type type)
        {
            return type == typeof(Guid) || type == typeof(DateTime) ||
                   type.IsEnum || IsNumericType(type);
        }
    }
}
