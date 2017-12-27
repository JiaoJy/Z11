namespace Z11.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MyDb1 : DbContext
    {
        public MyDb1()
            : base("name=MyDb1")
        {
        }

        public virtual DbSet<Texts> Texts { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>()
                .Property(e => e.introduction)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.background)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.workExperience)
                .IsUnicode(false);
        }
    }
}
