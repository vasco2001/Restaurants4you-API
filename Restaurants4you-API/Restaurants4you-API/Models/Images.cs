using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant4you_API.Models
{
    /// <summary>
    /// Descreve os dados das imagens
    /// </summary>
    public class Images
    {
        public Images() { }

        /// <summary>
        /// PK da tabela dos utilizadores
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Caminho das imagens
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// FK para referenciar os restaurantes registados
        /// </summary>
        [ForeignKey(nameof(Restaurant))]
        public int RestaurantFK { get; set; }

        public Restaurants Restaurant { get; set; }
    }
}
