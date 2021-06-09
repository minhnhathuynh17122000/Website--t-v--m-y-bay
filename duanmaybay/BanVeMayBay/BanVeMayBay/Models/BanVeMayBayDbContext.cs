namespace BanVeMayBay.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BanVeMayBayDbContext : DbContext
    {
        public BanVeMayBayDbContext()
            : base("name=ChuoiKn")
        {
        }
        public virtual DbSet<menu> menus { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<order> orders { get; set; }
        public virtual DbSet<ordersdetail> ordersdetails { get; set; }

        public virtual DbSet<ticket> tickets { get; set; }
        public virtual DbSet<post> Posts { get; set; }
        public virtual DbSet<role> roles { get; set; }
        public virtual DbSet<topic> Topics { get; set; }

      
    }
}
