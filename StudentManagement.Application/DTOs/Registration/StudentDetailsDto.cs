using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Application.DTOs.Registration
{
    public class StudentDetailsDto
    {
        public int StudentId { get; set; }

        public string? StudentName { get; set; }

        public string? Gender { get; set; }

        public string? MobileNumber { get; set; }

        public string EmailAddress { get; set; } = string.Empty;

        public string? Password { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? ProfilePhoto { get; set; }

        public string? Qualification { get; set; }

        public int? Flag { get; set; }

        public string? ParentName { get; set; }

        public string? ParentNumber { get; set; }

        public string? StudentCode { get; set; }

        public string? LastName { get; set; }

        public string? WhatsAppNumber { get; set; }

        public string? LocalAddress { get; set; }

        public string? PermanentAddress { get; set; }

        public string PermanentIdentificationNumber { get; set; }
            = string.Empty;

        [Column("adhar_card_number")]
        public string? AadharCardNumber { get; set; }

        [Column("adhar_card_photo")]
        public string? AadharCardPhoto { get; set; }

        public int BranchId { get; set; }

        public DateTime? InsertedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public DateTime? RestoredAt { get; set; }
    }
}