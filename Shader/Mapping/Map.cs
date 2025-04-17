namespace Shader.Mapping
{
    public static class Map
    {
        public static TDestination ToDTO<TSource, TDestination>(this TSource source)
            where TDestination : new()
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
        public static IEnumerable<TDestination> ToDTO<TSource, TDestination>(this IEnumerable<TSource> source)
            where TDestination : new()
        {
            List<TDestination> destination = [];
            foreach(var item in source)
            {
                var dest = new TDestination();
                foreach (var prop in typeof(TSource).GetProperties())
                {
                    var destProp = typeof(TDestination).GetProperty(prop.Name);
                    if (destProp != null && destProp.CanWrite && destProp.CanRead)
                    {
                        destProp.SetValue(dest, prop.GetValue(item));
                    }
                }
                destination.Add(dest);
            }
            return destination;
        }
        public static TSource ToEntity<TSource, TDestination>(this TDestination destination, TSource? source = null)
            where TSource : class, new()
            where TDestination : class, new()
        {
            source ??= new TSource(); 

            foreach (var destProp in typeof(TDestination).GetProperties())
            {
                var srcProp = typeof(TSource).GetProperty(destProp.Name);
                if (srcProp != null && srcProp.CanWrite && destProp.CanRead)
                {
                    srcProp.SetValue(source, destProp.GetValue(destination));
                }
            }
            return source;
        }
    }
}
