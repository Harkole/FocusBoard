using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FocusBoardCore.Models
{
    [DataContract]
    public class Actor
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Address is required")]
        public string Email { get; set; } = string.Empty;

        [DataMember(Name = "alias")]
        public string Alias { get; set; } = string.Empty;

        [DataMember(Name = "hidden")]
        public bool Hidden { get; set; } = false;
    }
}
