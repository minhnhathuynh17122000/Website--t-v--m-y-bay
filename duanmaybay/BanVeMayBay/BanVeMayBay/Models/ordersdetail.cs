namespace BanVeMayBay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ordersdetail")]
    public partial class ordersdetail
    {
        [Key]
        public int ID { get; set; }
        public int orderid { get; set; }
        public int ticketId { get; set; }
        public int quantity { get; set; }
    }
}
