using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Domain.Entities.Recruitment;

namespace TalentSync.Application.Mappings.Recruitment
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile() {
            CreateMap<CreateApplicationDto, ApplicationEntity>();
            CreateMap<ApplicationEntity, ApplicationResponseDto>();
            CreateMap<ApplicationEntity, ApplicationWithDetailsResponseDto>()
                .ForMember(dest => dest.CandidateName, opt => opt.MapFrom(src => src.Candidate != null ? src.Candidate.Name : null))
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job != null ? src.Job.Title : null));

        }
    }
}
