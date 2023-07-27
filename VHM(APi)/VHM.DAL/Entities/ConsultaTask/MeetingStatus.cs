using System.Runtime.Serialization;
using VHM_APi_.DAL.ConsultaTask;

public enum MeetingStatus
{
    [EnumMember(Value = "Pending")]
    Pending,
    [EnumMember(Value = "Accepted")]
    Accepted,
    [EnumMember(Value = "Cancelled")]
    Cancelled,
    [EnumMember(Value = "Scheduled")]
    Scheduled
}
