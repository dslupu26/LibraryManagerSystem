namespace Common.DomainModels
{
    public interface IDomainModel
    {
        /// <summary>Gets or sets the unique identifier.</summary>
        int Id { get; set; }

        /// <summary> Gets or sets the created on. </summary>        
        DateTime CreatedOn { get; set; }

        /// <summary> Gets or sets the created by. </summary>        
        string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>        
        string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified on.
        /// </summary>
        DateTime? ModifiedOn { get; set; }

        /// <summary>Gets or sets the timestamp.</summary>                
        byte[] TS { get; set; }
    }
}
