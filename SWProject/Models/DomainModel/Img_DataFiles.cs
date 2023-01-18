//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SWProject.Models.DomainModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Img_DataFiles
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Img_DataFiles()
        {
            this.Posts = new HashSet<Post>();
            this.Categories = new HashSet<Category>();
            this.File_DataFiles = new HashSet<File_DataFiles>();
        }
    
        public System.Guid Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileBody { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.Guid UserId { get; set; }
        public string FileExtention { get; set; }
        public Nullable<int> Size { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> Categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<File_DataFiles> File_DataFiles { get; set; }
    }
}
