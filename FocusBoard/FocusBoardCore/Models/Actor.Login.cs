using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FocusBoardCore.Models
{
    [DataContract]
    public class ActorLogin
    {
        [DataMember(Name = "email", IsRequired = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Must provided login email")]
        public string Email { get; set; } = string.Empty;

        [DataMember(Name = "password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Must provided password")]
        public string Password { get; set; } = string.Empty;
    }
}
