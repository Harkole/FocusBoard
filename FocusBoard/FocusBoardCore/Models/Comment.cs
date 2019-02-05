using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FocusBoardCore.Models
{
    [DataContract]
    public class Comment
    {
        [DataMember(Name = "id")]
        public string Id { get; set; } = string.Empty;

        [DataMember(Name = "authorId")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Author Identity must be provided")]
        public string AuthorId { get; set; } = string.Empty;

        [DataMember(Name = "parentId")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Parent Identity must be provided")]
        public string ParentId { get; set; } = string.Empty;

        [DataMember(Name = "comment")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Comments must have a text value")]
        public string Value { get; set; } = string.Empty;

        [DataMember(Name = "votes")]
        public int Votes { get; set; } = 0;
    }
}
