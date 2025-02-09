using Lab1.Domain.Utilities;
using System;
using System.Linq;
using System.Reflection;

public class ObjectMapperService
{
    public static TDestination Map<TSource, TDestination>(TSource source) where TDestination : new()
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        TDestination destination = new();
        Type sourceType = typeof(TSource);
        Type destinationType = typeof(TDestination);

        PropertyInfo[] sourceProperties = sourceType.GetProperties();
        PropertyInfo[] destinationProperties = destinationType.GetProperties();

        foreach (var sourceProp in sourceProperties)
        {
            var destProp = destinationProperties.FirstOrDefault(p => p.Name == sourceProp.Name && p.PropertyType == sourceProp.PropertyType);

            var mapToAttr = sourceProp.GetCustomAttribute<MapToAttribute>();
            if (mapToAttr != null)
            {
                destProp = destinationProperties.FirstOrDefault(p => p.Name == mapToAttr.TargetProperty);
            }

            if (destProp != null && destProp.CanWrite)
            {
                object value = sourceProp.GetValue(source);

                if (value != null && destProp.PropertyType != sourceProp.PropertyType)
                {
                    value = Convert.ChangeType(value, destProp.PropertyType);
                }

                destProp.SetValue(destination, value);
            }
        }

        return destination;
    }
}
