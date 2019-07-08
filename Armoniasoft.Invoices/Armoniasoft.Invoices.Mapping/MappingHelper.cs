using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Armoniasoft.Invoices.Mapping
{
    public class MappingHelper
    {
        public static IEnumerable<To> Map<From, To>(IEnumerable<From> from)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<From, To>();
            });

            IMapper mapper = new Mapper(config);
            var dest = mapper.Map<IEnumerable<From>, IEnumerable<To>>(from, opt => opt.ConfigureMap(MemberList.Destination));
            return dest;
        }

        public static To Map<From, To>(From from)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<From, To>();
            });

            IMapper mapper = new Mapper(config);
            var dest = mapper.Map<From, To>(from, opt => opt.ConfigureMap(MemberList.Destination));
            return dest;
        }

        public static To Map<From, To>(From from, Profile profile)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile(profile);
            });

            IMapper mapper = new Mapper(config);
            var dest = mapper.Map<From, To>(from);
            return dest;
        }
    }
}
