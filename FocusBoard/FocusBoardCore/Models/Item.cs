using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FocusBoardCore.Models
{
    [DataContract]
    public class Item
    {
        [DataMember(Name = "id")]
        public string Id { get; set; } = string.Empty;

        [DataMember(Name = "author")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Author is required")]
        public string Author { get; set; } = string.Empty;

        [DataMember(Name = "hideAuthor")]
        public bool HideAuthor { get; set; } = false;

        [DataMember(Name = "title")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [DataMember(Name = "description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Description must not be blank")]
        public string Description { get; set; } = string.Empty;

        [DataMember(Name = "votes")]
        public int Votes { get; set; } = 0;
    }
}
