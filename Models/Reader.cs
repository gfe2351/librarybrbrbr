using System;
using System.ComponentModel.DataAnnotations;

namespace NationalLibraryApp.Models
{
    public class Reader
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите ФИО")]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = "Укажите дату рождения")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Укажите место учёбы/работы")]
        public string Institution { get; set; } = "";

        public InstitutionType InstitutionType { get; set; }

        [Required(ErrorMessage = "Укажите ИИН")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "ИИН должен состоять из 12 цифр")]
        public string IIN { get; set; } = "";

        [Required(ErrorMessage = "Укажите почту")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Укажите номер телефона")]
        [Phone(ErrorMessage = "Некорректный номер телефона")]
        public string Phone { get; set; } = "";

        public Gender Gender { get; set; }
    }
}
