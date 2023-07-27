namespace VHM_APi_.Dtos
{
    public class ReadingDto
    {
        public int Id { get; set; }
        public string PatientId { get; set; }
        public string HeartRate { get; set; }
        public string Temperature { get; set; }
        public string Pressure { get; set; }
    }
}
