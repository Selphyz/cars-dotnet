using AutoMapper;
using Contracts;
using SearchService.Models;

namespace SearchService.RequestHelpers;

public class MappingProfiles : Profile {
    protected MappingProfiles() {
        CreateMap<AuctionCreated, Item>();
    }
}