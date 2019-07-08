using Armoniasoft.Clients.DTOs;
using AutoMapper;
using DefaultNamespace;

namespace Armoniasoft.Clients.MappingProfiles
{
    public class ClientMappingProfile : Profile
    {
        public ClientMappingProfile()
        {
            CreateMap<Client, ClientDto>();                
        }
    }
}