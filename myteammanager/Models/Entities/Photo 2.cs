using System.ComponentModel.DataAnnotations.Schema;

namespace MYTEAMMANAGER.Models.Entities
{
    public class Photo
    {
        public Guid Id { get; set; }

        public string? Url { get; set; }

        public string? publicId { get; set; }

       //navigation property
        [ForeignKey(nameof(Id))]
        public TeamMember TeamMember {set; get;} = null!;
         

    }
}