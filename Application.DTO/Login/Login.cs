using System.ComponentModel.DataAnnotations;

namespace Application.DTO
{
    public class Login
    {
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "Ingresar el Usuario")]
        [StringLength(30, ErrorMessage = "Máximo de carácteres permitidos")]

        public string Username { get; set; }
        public string Usuario { get; set; }


        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Ingresar la contraseña")]
        [StringLength(30, ErrorMessage = "Máximo de carácteres permitidos")]
        [DataType(DataType.Password)]

        public string Password { get; set; }

    }
}
