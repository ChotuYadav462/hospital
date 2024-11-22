using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagements.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a patient.")]
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        [Required(ErrorMessage = "Please select a doctor.")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        [Required(ErrorMessage = "Please enter a date.")]
        [DataType(DataType.Date)]
        [Display(Name = "Appointment Date")]
        [FutureDate(ErrorMessage = "Back date appointments are not accepted.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please enter a disease.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Only alphabets and spaces are allowed.")]
        public string Disease { get; set; }

        [Required(ErrorMessage = "Please enter a phone number.")]

        public string PhoneNumber { get; set; }

        public bool IsChecked { get; set; }
    }

    // Custom validation attribute for future date validation
    // Custom validation attribute for future date validation
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime date;
            if (value is DateTime)
            {
                date = (DateTime)value;
            }
            else if (value is string)
            {
                if (DateTime.TryParse((string)value, out date))
                {
                    return date.Date >= DateTime.Today;
                }
                return false;
            }
            else
            {
                return false;
            }

            return date.Date >= DateTime.Today;
        }
    }
}
