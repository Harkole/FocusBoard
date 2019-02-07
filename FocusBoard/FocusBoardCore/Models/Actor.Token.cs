using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FocusBoardCore.Models
{
    [DataContract]
    public class ActorToken
    {
        [DataMember(Name = "access_token")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Token value is expected")]
        public string Token { get; set; } = string.Empty;

        [DataMember(Name = "expires_in")]
        [Required(ErrorMessage = "Expires in is expected")]
        public int ExpiresIn { get; set; } = 0;
    }
}
