using AutoMapper;
using TalentSync.Application.DTOs.Dashboard;
using TalentSync.Domain.Entities.Recruitment;

namespace TalentSync.Application.Mappings.Dashboards
{
    public class DashboardProfile : Profile
    {
        public DashboardProfile()
        {

            CreateMap<ApplicationEntity, DashboardApplicationDto>()
                .ForMember(
                    dest => dest.JobTitle,
                    opt => opt.MapFrom(src => src.Job.Title));

            CreateMap<Interview, DashboardInterviewDto>()
                .ForMember(
                    dest => dest.JobTitle,
                    opt => opt.MapFrom(src => src.Application.Job.Title))
                .ForMember(
                    dest => dest.InterviewerName,
                    opt => opt.MapFrom(src => src.Interviewer.Name));
        }
    }
}