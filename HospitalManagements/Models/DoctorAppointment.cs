using System.ComponentModel.DataAnnotations;

namespace HospitalManagements.Models
{
    public class DoctorAppointment
    {
        [Key]
        public int Id { get; set; }

        public Guid PatientId { get; set; }

        public int DoctorId { get; set; }

        public DateTime Date { get; set; }

        public string Disease { get; set; }

        public string PhoneNumber { get; set; }

    }
}
