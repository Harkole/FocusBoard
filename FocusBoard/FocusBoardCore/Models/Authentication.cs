using System.Runtime.Serialization;

namespace FocusBoardCore.Models
{
    [DataContract]
    public class Authentication
    {
        [DataMember(Name = "id")]
        public string PrimaryId { get; set; } = string.Empty;

        [DataMember(Name = "groupId")]
        public string PrimaryGroupId { get; set; } = string.Empty;

        [DataMember(Name = "roleId")]
        public string RoleId { get; set; } = string.Empty;
        
        [DataMember(Name = "alias")]
        public string Alias { get; set; } = string.Empty;

        [DataMember(Name = "email")]
        public string Email { get; set; } = string.Empty;

        [DataMember(Name = "hidden")]
        public bool Hidden { get; set; } = false;
    }
}
