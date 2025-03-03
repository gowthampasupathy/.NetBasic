using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data
{
    
    public class Credential
    {
        [Key]
        public int Id {get;set;}
        public String name {get; set;}
        public String password {get; set;}
    }
}