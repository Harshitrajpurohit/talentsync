using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Notifications;
using TalentSync.Domain.Entities.Notifications;

namespace TalentSync.Application.Mappings.Notifications
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile() {

            CreateMap<CreateNotificationDto, Notification>();
            CreateMap<Notification, NotificationRealtimeDto>();
            CreateMap<Notification, NotificationResponseDto>();
        }
    }
}
