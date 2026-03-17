using AvaloniaTemplate.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AvaloniaTemplate.Services
{
    public class PropertyAccessorFactory : IPropertyAccessorFactory
    {
        private readonly Dictionary<(Type, string?), Func<object, object?>> _cache = new();

        public Func<object, object?> Create(Type type, string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return x => x;

            var key = (type, path);

            if (_cache.TryGetValue(key, out var getter))
                return getter;

            getter = BuildGetter(type, path);
            _cache[key] = getter;

            return getter;
        }

        private static Func<object, object?> BuildGetter(Type type, string path)
        {
            var param = Expression.Parameter(typeof(object), "obj");
            Expression current = Expression.Convert(param, type);

            foreach (var part in path.Split('.'))
            {
                var prop = type.GetProperty(part);
                if (prop == null)
                    return _ => null;

                current = Expression.Property(current, prop);
                type = prop.PropertyType;
            }

            var convert = Expression.Convert(current, typeof(object));

            return Expression.Lambda<Func<object, object?>>(convert, param).Compile();
        }
    }
}
