using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagements.Models
{
    public class Patient
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Only alphabets and spaces are allowed.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
      //  [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [Display(Name = "Date of Registration")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        // Add for Image.
      // public string ResumeImage { get; set; }
      public string? ResumeImages  { get; set; }

    }
}
