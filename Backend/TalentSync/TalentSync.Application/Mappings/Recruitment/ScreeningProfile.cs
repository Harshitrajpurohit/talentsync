using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Domain.Entities.Recruitment;

namespace TalentSync.Application.Mappings.Recruitment
{
    public class ScreeningProfile : Profile
    {
        public ScreeningProfile() {

            CreateMap<Screening, ScreeningResponseDto>()
                    .ForMember(dest => dest.CandidateName,
                        opt => opt.MapFrom(src => src.Application.Candidate != null
                            ? src.Application.Candidate.Name
                            : null))
                    .ForMember(dest => dest.JobTitle,
                        opt => opt.MapFrom(src => src.Application.Job != null
                            ? src.Application.Job.Title
                            : null))
                    .ForMember(dest => dest.ScreenedName,
                        opt => opt.MapFrom(src => src.ScreenedBy != null
                            ? src.ScreenedBy.Name
                            : null));

            CreateMap<CreateScreeningRequestDto, Screening>();

            CreateMap<UpdateScreeningRequestDto, Screening>();
        }
    }
}
