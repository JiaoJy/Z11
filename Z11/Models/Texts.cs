namespace Z11.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Texts
    {
        public int Id { get; set; }

        public int? userId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}
