using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => typeof(IMap).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type) as IMap;
                instance?.Mapping(this);
            }
        }
    }
}
