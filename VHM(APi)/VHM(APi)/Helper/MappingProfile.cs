using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using VHM.DAL.Data.HangFireContext.HangFContextEntities;
using VHM.DAL.Entities;
using VHM.DAL.Entities.Ambulance;
using VHM.DAL.Entities.ChatEntities;
using VHM.DAL.Entities.DoctorEntities;
using VHM.DAL.Entities.PatientEntities;
using VHM.DAL.Entities.ReadingEntities;
using VHM_APi_.Dtos;
using VHM_APi_.Dtos.Ambulance;
using VHM_APi_.Dtos.Chat;
using VHM_APi_.Dtos.Doctor;
using VHM_APi_.EntityInputs;
using VHM_APi_.EntityInputs.DoctorInputs;
using VHM_APi_.Helper.Resolvers;

namespace VHM_APi_.Helper
{
    public class MappingProfile : Profile
    {


        public MappingProfile()
        {

            CreateMap<Patient, PatientDto>()
                .ForMember(d => d.ImageUrl, o => o.MapFrom<ImageUrlResolver>());

            CreateMap<Prescription, PrescriptionDto>()
                .ForMember(p => p.DoctorName, p => p.MapFrom(p => p.Doctor.UserName));

            CreateMap<PrescriptionInput, Prescription>();

            CreateMap<Report, ReportDto>()
               .ForMember(p => p.DoctorName, p => p.MapFrom(p => p.Doctor.UserName));


            CreateMap<PatientInput, Patient>();

            CreateMap<Doctor, DoctorDto>()
                .ForMember(p => p.ImageUrl, p => p.MapFrom<ImageUrlResolver>())
                .ReverseMap();
            CreateMap<Doctor, UpdateDoctorInputDto>();

            CreateMap<Staff, ApplicationUserDto>()
                .ForMember(p => p.ImageUrl, p => p.MapFrom<ImageUrlResolver>())
                .ReverseMap();
            CreateMap<Admin, ApplicationUserDto>()
                .ForMember(p => p.ImageUrl, p => p.MapFrom<ImageUrlResolver>())
                .ReverseMap();

            CreateMap<FeedBack, FeedBackDto>()
                .ForMember(r => r.PatientName, o => o.MapFrom(p => p.Patient.UserName));

            CreateMap<SupportMessages, SupportMessageDto>()
                .ForMember(r => r.PatientName, o => o.MapFrom(p => p.Patient.UserName))
                .ForMember(r => r.PatientId, O => O.MapFrom(p => p.Patient.Id));

            CreateMap<ApplicationUser, BaseUserDto>();
            CreateMap<AmbulanceCall, AmbulanceCallDto>();

            CreateMap<ApplicationUser, LatestUserWithUnreadCountDto>();
               // .ForMember(d => d.Role, o => o.MapFrom( s => (userManager.GetRolesAsync(s).Result).FirstOrDefault()));

            CreateMap<Notification,NotificationDto>();
            CreateMap<Message,MessageDto>();
            CreateMap<Readings, ReadingDto>();
        }
    }
}
