using AutoMapper;
using TalentSync.Application.DTOs.Dashboard;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Domain.Entities.Recruitment;

namespace TalentSync.Application.Mappings.Recruitment
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<CreateJobDto, Job>();

            CreateMap<Job, JobResponseDto>()
                .ForMember(
                    dest => dest.HrName,
                    opt => opt.MapFrom(src => src.HR.Name));

            CreateMap<Job, JobListDto>()
                .ForMember(
                    dest => dest.HrName,
                    opt => opt.MapFrom(src => src.HR.Name))
                .ForMember(
                    dest => dest.ApplicationsCount,
                    opt => opt.MapFrom(src => src.Applications.Count));

            CreateMap<UpdateJobRequestDto, Job>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) =>
                        srcMember != null));

            CreateMap<Job, CandidateJobListDto>();

            CreateMap<Job, CandidateJobDetailsDto>();

            CreateMap<Job, DashboardJobDto>();
        }
    }
}