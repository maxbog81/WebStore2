using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Domain.ViewModels.Identity
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "Имя пользователя является обязательным")]
        [MaxLength(256, ErrorMessage = "Максимальная длина имени пользователя 256 символов")]
        [Remote("IsNameFree", "Account", ErrorMessage = "Пользователь с таким именем уже существует.")]
        [Display(Name = "Имя пользователя")]
        [RegularExpression(@"[A-Za-z][A-Za-z0-9_]{3,}", ErrorMessage = "Некорректное имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Не введён пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтвердите ввод пароля")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите ввод пароля")]
        [Compare(nameof(Password), ErrorMessage = "Пароль и подтверждение не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
