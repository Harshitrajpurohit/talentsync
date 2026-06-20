using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Domain.Entities.Recruitment;

namespace TalentSync.Application.Mappings.Recruitment
{
    public class JobProfile : Profile
    {
        public JobProfile() {
            CreateMap<CreateJobDto, Job>();
            CreateMap<Job, JobResponseDto>();
            CreateMap<Job, JobListDto>();
            CreateMap<UpdateJobRequestDto, Job>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) =>
                        srcMember != null));


        }
    }
}
