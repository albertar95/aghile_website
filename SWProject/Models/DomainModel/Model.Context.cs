﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SwpEntities : DbContext
    {
        public SwpEntities()
            : base("name=SwpEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Img_DataFiles> Img_DataFiles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<LocalVariable> LocalVariables { get; set; }
        public virtual DbSet<Paper_DataFiles> Paper_DataFiles { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Media_DataFiles> Media_DataFiles { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<tmp_PagesStatLogs> tmp_PagesStatLogs { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Doubt> Doubts { get; set; }
        public virtual DbSet<File_DataFiles> File_DataFiles { get; set; }
    }
}
