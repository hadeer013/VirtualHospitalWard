using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VHM.DAL.Data;
using VHW.BLL.Interfaces.BackupAndRestore;
using VHW.BLL.Interfaces;
using VHW.BLL.Repositories.AmbulanceRepository;
using VHW.BLL.Repositories.HangFireRepository;
using VHW.BLL.Repositories;
using VHW.BLL.Token;

namespace VHM_APi_.Extentions
{
    public static class AddScopedSevices
    {
        public static void AddScopedServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(ITokenService), typeof(TokenServices));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IPatientRepository), typeof(PatientRepository));
            services.AddScoped(typeof(IUserRepository<>), typeof(UserRepository<>));
            services.AddScoped(typeof(IConsultationTask), typeof(ConsultationTaskRepository));
            services.AddScoped(typeof(ISavedRepository), typeof(SavedRepository));
            services.AddScoped(typeof(IDoctorRepository), typeof(DoctorRepository));
            services.AddScoped(typeof(IReportRepository), typeof(ReportRepository));
            services.AddScoped(typeof(IPrescriptionRepository), typeof(PrescriptionRepository));
            services.AddScoped(typeof(IFeedBackRepository), typeof(FeedBackRepository));
            services.AddScoped(typeof(ISupportMessageRepository), typeof(SupportMessagesRepository));
            services.AddScoped(typeof(IChatRepository), typeof(ChatRepository));
            services.AddScoped(typeof(IBackupAndResRepository), typeof(BackupAndResRepository));
            services.AddScoped(typeof(IBackRestoreServiceRepository), typeof(BackupAndRestoreServicesRepository));
            services.AddScoped(typeof(IAmbulanceRepository), typeof(AmbulanceCallRepository));
            services.AddScoped(typeof(IUserWithConnectionIdsRepository), typeof(UserWithConnectionIdsRepository));
            services.AddScoped(typeof(IReadingRepository), typeof(ReadingRepository));
        }
    }
}
