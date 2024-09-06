using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Runtime.Serialization;

namespace AspireApp.Libraries.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Key] public int Id { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public string Metadata { get; set; } = string.Empty; // Serialized
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        public ModelBase()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
