using System.Runtime.Serialization;

namespace FocusBoardCore.Models
{
    [DataContract]
    public class Comment
    {
        [DataMember(Name = "id")]
        public string Id { get; set; } = string.Empty;
        [DataMember(Name = "authorId")]
        public string AuthorId { get; set; } = string.Empty;
        [DataMember(Name = "parentId")]
        public string ParentId { get; set; } = string.Empty;
        [DataMember(Name = "comment")]
        public string Value { get; set; } = string.Empty;
        [DataMember(Name = "votes")]
        public int Votes { get; set; } = 0;
    }
}
