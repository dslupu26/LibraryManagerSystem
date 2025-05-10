using System.ComponentModel.DataAnnotations;

namespace Common.DomainModels
{
    public abstract class DomainModelBase : IDomainModel
    {
        /// <summary>Gets or sets the id.</summary>
        [Key]
        public int Id { get; set; }

        /// <summary>Gets or sets the created on.</summary>
        [Required]
        public DateTime CreatedOn { get; set; }

        /// <summary>Gets or sets the created by.</summary>
        [Required]
        [MaxLength(25)]
        public string CreatedBy { get; set; } 

        /// <summary>Gets or sets the modified by.</summary>
        [MaxLength(25)]
        public string ModifiedBy { get; set; }

        /// <summary>Gets or sets the modified on.</summary>
        public DateTime? ModifiedOn { get; set; }

        /// <summary>Gets or sets the timestamp.</summary>
        [Timestamp]
        public byte[] TS { get; set; }        
    }
}
