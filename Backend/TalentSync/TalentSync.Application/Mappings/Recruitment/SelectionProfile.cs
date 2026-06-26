using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Domain.Entities.Recruitment;

namespace TalentSync.Application.Mappings.Recruitment
{
    public class SelectionProfile : Profile
    {
        public SelectionProfile() {

            CreateMap<CreateSelectionDecisionDto, Selection>();
            CreateMap<Selection, SelectionResponseDto>();

            CreateMap<Selection, SelectionWithDetailsResponseDto>()
                .ForMember(dest => dest.CandidateName, opt => opt.MapFrom(src => src.Application.Candidate != null ? src.Application.Candidate.Name : null))
                .ForMember(dest => dest.CandidateEmail, opt => opt.MapFrom(src => src.Application.Candidate != null ? src.Application.Candidate.Email : null))
                .ForMember(dest => dest.CandidateId, opt => opt.MapFrom(src => src.Application.Candidate != null ? src.Application.Candidate.Id : Guid.Empty))
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Application.Job != null ? src.Application.Job.Title : null))
                .ForMember(dest => dest.JobId, opt => opt.MapFrom(src => src.Application.Job != null ? src.Application.Job.Id : Guid.Empty));

            
        }
    }
}
