using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Restaurant4you_API.Models {

    /// <summary>
    /// Descreve os dados dos restaurantes
    /// </summary>
    public class Restaurants {

        /// <summary>
        /// Construtor da classe
        /// </summary>
        public Restaurants() {
            //inicializar a lista das imagens
            Images = new HashSet<Images>();

        }

        /// <summary>
        /// PK da tabela dos restaurantes
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do Restaurante
        /// </summary>
        [StringLength(32, ErrorMessage = "O {0} não pode ter mais do que {1} carateres.")]
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        public String Name { get; set; }

        /// <summary>
        /// Descrição do Restaurante
        /// </summary>
        [StringLength(256, ErrorMessage = "O {0} não pode ter mais do que {1} carateres.")]
        [Display(Name = "Descrição")]
        public String Description { get; set; }

        /// <summary>
        /// Localização do Restaurante
        /// </summary>
        [Display(Name = "Localização")]
        [Required(ErrorMessage ="O {0} é de preenchimento obrigatório")]
        public String Localization { get; set; }

        /// <summary>
        /// Contacto do Restaurante
        /// </summary>
        [Display(Name = "Contacto")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [RegularExpression("[2,8,7,9]{1}[0-9]{8}", ErrorMessage = "Insira um {0} válido, coloque no formato 9xxxxxxxx.")]
        public String  Contact { get; set; }

        /// <summary>
        /// Email do Restaurante
        /// </summary>
        [Display(Name = "Email")]
        [EmailAddress]
        public String Email { get; set; }

        /// <summary>
        /// Horário do Restaurante
        /// </summary>
        [Display(Name = "Horário")]
        public String Time { get; set; }

        /// <summary>
        /// Latitude do restaurante
        /// </summary>
        public Double Latitude { get; set; }

        /// <summary>
        /// Longitude do restaurante
        /// </summary>
        public Double Longitude { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserFK { get; set; }

        public User User { get; set; }

        /// <summary>
        /// Lista de imagens
        /// </summary>
        public ICollection<Images> Images { get; set; }




    }
}
