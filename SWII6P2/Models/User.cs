using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWII6P2.Models
{
    [Table("user")]
    public class User
    {
        [Column("id")]
        [Key]
        [Required]
        public int Id { get; set; }

        [Column("name")]
        [Display(Name = "nome")]
        public string Name { get; set; }

        [Column("password")]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Column("status")]
        public bool Status { get; set; }
    }
}