using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Domain.Entities.Recruitment;

namespace TalentSync.Application.Mappings.Recruitment
{
    public class ResumeProfile : Profile
    {
        public ResumeProfile()
        {
            CreateMap<CloudinaryUploadResultDto, Resume>();
            CreateMap<Resume, ResumeResponseDto>();

            CreateMap<Resume, ResumeWithDetailsResponseDto>()
                .ForMember(dest => dest.CandidateName, opt => opt.MapFrom(src => src.Candidate.Name))
                .ForMember(dest => dest.CandidateEmail, opt => opt.MapFrom(src => src.Candidate.Email));
        }
    }
}
