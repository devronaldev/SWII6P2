using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWII6P2.Models
{
    [Table("product")]
    public class Product
    {
        [Key]
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        [Display(Name = "nome")]
        public string Name { get; set; }

        [Required]
        [Column("price")]
        [Display(Name = "preço")]
        public double Price { get; set; }

        [Required]
        [Column("status")]
        public bool Status { get; set; }

        [Required]
        [Column("recorderId")]
        [Display(Name = "Id Registrador")]
        public int RecorderId { get; set; }

        [Column("lastUpdaterId")]
        [Display(Name = "Último a atualizar")]
        public int LastUpdaterId { get; set; }
    }
}
