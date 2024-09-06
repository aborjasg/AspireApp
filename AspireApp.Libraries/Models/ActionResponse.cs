using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireApp.Libraries.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionResponse
    {
        public int Id { get; set; } = 0;
        public string Type { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
