using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ObjectMapper
{
    // Cache property info to improve reflection performance
    private static readonly Dictionary<Type, PropertyInfo[]> SourcePropertyCache = new();
    private static readonly Dictionary<Type, PropertyInfo[]> DestinationPropertyCache = new();

    // Track circular references to avoid infinite recursion
    private static readonly Dictionary<object, object> ReferenceMap = new();

    // ==== Single Object Mapping ==== //

    /// <summary>
    /// Maps a source object to a new or existing destination object (optional).
    /// </summary>
    public static TDestination MapTo<TDestination>(
        this object source,
        TDestination destination = null)
        where TDestination : class, new()
    {
        if (source == null) return null;

        ReferenceMap.Clear(); // Reset tracking for a new mapping operation
        return (TDestination)MapInternal(source, typeof(TDestination), destination);
    }

    // ==== Collection Mapping (IEnumerable<T> Support) ==== //

    /// <summary>
    /// Maps a collection to a new or existing IEnumerable<TDestination> (optional).
    /// </summary>
    public static IEnumerable<TDestination> MapTo<TSource, TDestination>(
        this IEnumerable<TSource> sources,
        IEnumerable<TDestination> destinationCollection = null)
        where TDestination : class, new()
    {
        if (sources == null) return null;

        ReferenceMap.Clear();

        // If destination is provided, use it; otherwise, create a new List<TDestination>
        var resultCollection = destinationCollection?.ToList() ?? new List<TDestination>();

        foreach (var source in sources)
        {
            var mappedItem = source.MapTo<TDestination>();
            resultCollection.Add(mappedItem);
        }

        // Return the same collection type if possible (e.g., if input was an array, return an array)
        return destinationCollection?.GetType().IsArray == true
            ? resultCollection.ToArray()
            : (IEnumerable<TDestination>)resultCollection;
    }

    // ==== Core Mapping Logic ==== //

    private static object MapInternal(
        object source,
        Type destinationType,
        object existingDestination = null)
    {
        if (source == null) return null;

        // Handle circular references
        if (ReferenceMap.TryGetValue(source, out var existingMappedObject))
            return existingMappedObject;

        // Create or reuse destination
        var destination = existingDestination ?? Activator.CreateInstance(destinationType);
        ReferenceMap[source] = destination;

        // Get cached properties
        var sourceProps = GetSourceProperties(source.GetType());
        var destProps = GetDestinationProperties(destinationType);

        foreach (var sourceProp in sourceProps)
        {
            var destProp = destProps.FirstOrDefault(p => p.Name == sourceProp.Name);
            if (destProp == null || !destProp.CanWrite || !sourceProp.CanRead)
                continue;

            var sourceValue = sourceProp.GetValue(source);
            if (sourceValue == null)
            {
                destProp.SetValue(destination, null);
                continue;
            }

            // Handle collections (excluding strings)
            if (typeof(IEnumerable).IsAssignableFrom(sourceProp.PropertyType) &&
                sourceProp.PropertyType != typeof(string))
            {
                var sourceCollection = (IEnumerable)sourceValue;
                var destCollection = MapCollection(sourceCollection, destProp.PropertyType);
                destProp.SetValue(destination, destCollection);
            }
            else if (IsSimpleType(sourceProp.PropertyType))
            {
                // Directly assign simple types
                destProp.SetValue(destination, sourceValue);
            }
            else
            {
                // Recursively map complex objects
                var mappedValue = MapInternal(sourceValue, destProp.PropertyType);
                destProp.SetValue(destination, mappedValue);
            }
        }

        return destination;
    }

    private static object MapCollection(IEnumerable sourceCollection, Type destinationCollectionType)
    {
        if (sourceCollection == null) return null;

        // Get element type (supports List<T>, arrays, etc.)
        Type destElementType = destinationCollectionType.GetGenericArguments().FirstOrDefault()
                            ?? destinationCollectionType.GetElementType();

        if (destElementType == null) return null;

        // Create a List<T> to store mapped items
        var listType = typeof(List<>).MakeGenericType(destElementType);
        var destinationList = (IList)Activator.CreateInstance(listType);

        foreach (var item in sourceCollection)
        {
            if (item == null)
            {
                destinationList.Add(null);
                continue;
            }

            var mappedItem = MapInternal(item, destElementType);
            destinationList.Add(mappedItem);
        }

        // Convert to target collection type
        if (destinationCollectionType.IsArray)
        {
            var array = Array.CreateInstance(destElementType, destinationList.Count);
            destinationList.CopyTo(array, 0);
            return array;
        }
        if (destinationCollectionType.IsAssignableFrom(listType))
        {
            return destinationList;
        }

        // Try to create the destination collection type (e.g., HashSet<T>)
        if (Activator.CreateInstance(destinationCollectionType) is IList collection)
        {
            foreach (var item in destinationList) collection.Add(item);
            return collection;
        }

        return destinationList;
    }

    // ==== Helpers ==== //

    private static bool IsSimpleType(Type type)
    {
        return type.IsPrimitive ||
               type == typeof(string) ||
               type == typeof(decimal) ||
               type == typeof(DateTime) ||
               type == typeof(DateTimeOffset) ||
               type == typeof(TimeSpan) ||
               type == typeof(Guid) ||
               Nullable.GetUnderlyingType(type) != null && IsSimpleType(Nullable.GetUnderlyingType(type));
    }

    private static PropertyInfo[] GetSourceProperties(Type type)
    {
        if (!SourcePropertyCache.TryGetValue(type, out var properties))
        {
            properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            SourcePropertyCache[type] = properties;
        }
        return properties;
    }

    private static PropertyInfo[] GetDestinationProperties(Type type)
    {
        if (!DestinationPropertyCache.TryGetValue(type, out var properties))
        {
            properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                           .Where(p => p.CanWrite)
                           .ToArray();
            DestinationPropertyCache[type] = properties;
        }
        return properties;
    }
}