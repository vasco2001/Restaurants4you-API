using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Restaurant4you_API.Models {
    /// <summary>
    /// Descreve os pratos
    /// </summary>
    public class Plate {

        /// <summary>
        /// PK para o Prato
        /// </summary>
        public int Id { get; set; }

        // <summary>
        /// Nome do prato
        /// </summary>
        [StringLength(32, ErrorMessage = "O {0} não pode ter mais do que {1} carateres.")]
        [Display(Name = "Nome")]
        [RegularExpression("[A-ZÂÓÍa-záéíóúàèìòùâêîôûãõäëïöüñç '-]+", ErrorMessage = "Só pode escrever letras no {0}")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        public String Name { get; set; }

        /// <summary>
        /// Descrição do Prato
        /// </summary>
        [StringLength(256, ErrorMessage = "O {0} não pode ter mais do que {1} carateres.")]
        [Display(Name = "Descrição")]
        [RegularExpression("[A-ZÂÓÍa-záéíóúàèìòùâêîôûãõäëïöüñç '-]+", ErrorMessage = "Só pode escrever letras no {0}")]
        public String Description { get; set; }

        /// <summary>
        /// FK para referenciar o restaurante ao qual o rato pertence
        /// </summary>
        [ForeignKey(nameof(Restaurant))]
        public int RestaurantFK { get; set; }
        public Restaurants Restaurant { get; set; }
    }
}
