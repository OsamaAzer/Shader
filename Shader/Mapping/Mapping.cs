using System.Collections;

namespace Shader.Mapping
{
    public static class Mapping
    {
        public static TDestination MapWithNested<TSource, TDestination>(this TSource source)
                   where TDestination : class, new()
        {
            var destination = new TDestination();
            foreach (var prop in typeof(TSource).GetProperties())
            {
                var destProp = typeof(TDestination).GetProperty(prop.Name);
                if (destProp != null && destProp.CanWrite && destProp.CanRead)
                {
                    if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string))
                    {
                        var sourceList = prop.GetValue(source) as IEnumerable;
                        if (sourceList != null)
                        {
                            var destListType = destProp.PropertyType.GetGenericArguments().FirstOrDefault();
                            if (destListType != null)
                            {
                                var destList = Activator.CreateInstance(typeof(List<>).MakeGenericType(destListType)) as IList;
                                foreach (var item in sourceList)
                                {
                                    var mappedItem = item.MapWithNested(destListType);
                                    destList?.Add(mappedItem);
                                }
                                destProp.SetValue(destination, destList);
                            }
                        }
                    }
                    else
                    {
                        destProp.SetValue(destination, prop.GetValue(source));
                    }
                }
            }
            return destination;
        }

        public static IEnumerable<TDestination> MapWithNested<TSource, TDestination>(this IEnumerable<TSource> sources)
            where TDestination : class, new()
            where TSource : class, new()
        {
            var destinations = new List<TDestination>();
            foreach (var source in sources)
            {
                var dest = source.MapWithNested<TSource, TDestination>();
                destinations.Add(dest);
            }
            return destinations;
        }

        private static object MapWithNested(this object source, Type destinationType)
        {
            var destination = Activator.CreateInstance(destinationType);
            foreach (var prop in source.GetType().GetProperties())
            {
                var destProp = destinationType.GetProperty(prop.Name);
                if (destProp != null && destProp.CanWrite && destProp.CanRead)
                {
                    if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string))
                    {
                        var sourceList = prop.GetValue(source) as IEnumerable;
                        if (sourceList != null)
                        {
                            var destListType = destProp.PropertyType.GetGenericArguments().FirstOrDefault();
                            if (destListType != null)
                            {
                                var destList = Activator.CreateInstance(typeof(List<>).MakeGenericType(destListType)) as IList;
                                foreach (var item in sourceList)
                                {
                                    var mappedItem = item.MapWithNested(destListType);
                                    destList?.Add(mappedItem);
                                }
                                destProp.SetValue(destination, destList);
                            }
                        }
                    }
                    else
                    {
                        destProp.SetValue(destination, prop.GetValue(source));
                    }
                }
            }
            return destination;
        }
        public static TDestination ToDTO<TSource, TDestination>(this TSource source)
            where TDestination : class, new()
        {
            var destination = new TDestination();
            foreach (var prop in typeof(TSource).GetProperties())
            {
                var destProp = typeof(TDestination).GetProperty(prop.Name);
                if (destProp != null && destProp.CanWrite && destProp.CanRead)
                {
                    destProp.SetValue(destination, prop.GetValue(source));
                }
            }
            return destination;
        }
        public static IEnumerable<TDestination> ToDTOs<TSource, TDestination>(this IEnumerable<TSource> sources)
            where TDestination : class, new()
            where TSource : class, new()
        {
            List<TDestination> destinations = [];
            foreach(var source in sources)
            {
                var dest = new TDestination();
                foreach (var prop in typeof(TSource).GetProperties())
                {
                    var destProp = typeof(TDestination).GetProperty(prop.Name);
                    if (destProp != null && destProp.CanWrite && destProp.CanRead)
                    {
                        destProp.SetValue(dest, prop.GetValue(source));
                    }
                }
                destinations.Add(dest);
            }
            return destinations;
        }
        public static ICollection<TDestination> ToEntities<TSource, TDestination>(this ICollection<TSource> sources, ICollection<TDestination> destinations = null)
            where TDestination : class, new()
            where TSource : class, new()
        {
            destinations ??= [];
            foreach (var source in sources)
            {
                var dest = new TDestination();
                foreach (var prop in typeof(TSource).GetProperties())
                {
                    var destProp = typeof(TDestination).GetProperty(prop.Name);
                    if (destProp != null && destProp.CanWrite && destProp.CanRead)
                    {
                        destProp.SetValue(dest, prop.GetValue(source));
                    }
                }
                destinations.Add(dest);
            }
            return destinations;
        }
        public static TDestination ToEntity<TSource, TDestination>(this TSource source, TDestination? destination = null)
            where TDestination : class, new()
        {
            destination ??= new TDestination(); 

            foreach (var sourceProp in typeof(TSource).GetProperties())
            {
                var destProp = typeof(TDestination).GetProperty(sourceProp.Name);
                if (destProp != null && destProp.CanWrite && destProp.CanRead)
                {
                    destProp.SetValue(destination, sourceProp.GetValue(source));
                }
            }
            return destination;
        }
        public static TDestination Map<TSource, TDestination>(this TSource source, TDestination? destination = null)
            where TDestination : class, new()
        {
            destination ??= new TDestination(); 

            foreach (var sourceProp in typeof(TSource).GetProperties())
            {
                var destProp = typeof(TDestination).GetProperty(sourceProp.Name);
                if (destProp != null && destProp.CanWrite && destProp.CanRead)
                {
                    destProp.SetValue(destination, sourceProp.GetValue(source));
                }
            }
            return destination;
        }
        public static IEnumerable<TDestination> Map<TSource, TDestination>(this IEnumerable<TSource> sources, List<TDestination>? destinations = null)
            where TDestination : class, new()
            where TSource : class, new()
        {
            destinations ??= [];
            foreach (var source in sources)
            {
                var dest = new TDestination();
                foreach (var prop in typeof(TSource).GetProperties())
                {
                    var destProp = typeof(TDestination).GetProperty(prop.Name);
                    if (destProp != null && destProp.CanWrite && destProp.CanRead)
                    {
                        destProp.SetValue(dest, prop.GetValue(source));
                    }
                }
                destinations.Add(dest);
            }
            return destinations;
        }
    }

}
