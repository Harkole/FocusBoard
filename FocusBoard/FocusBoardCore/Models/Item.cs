using System.Runtime.Serialization;

namespace FocusBoardCore.Models
{
    [DataContract]
    public class Item
    {
        [DataMember(Name = "id")]
        public string Id { get; set; } = string.Empty;
        [DataMember(Name = "author")]
        public string Author { get; set; } = string.Empty;
        [DataMember(Name = "hideAuthor")]
        public bool HideAuthor { get; set; } = false;
        [DataMember(Name = "title")]
        public string Title { get; set; } = string.Empty;
        [DataMember(Name = "description")]
        public string Description { get; set; } = string.Empty;
        [DataMember(Name = "votes")]
        public int Votes { get; set; } = 0;
    }
}
