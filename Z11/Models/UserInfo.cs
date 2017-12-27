namespace Z11.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserInfo")]
    public partial class UserInfo
    {
        public int id { get; set; }

        public int? userId { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        [StringLength(11)]
        public string telPhone { get; set; }

        [StringLength(30)]
        public string email { get; set; }

        [StringLength(30)]
        public string address { get; set; }

        [StringLength(30)]
        public string job { get; set; }

        public string introduction { get; set; }

        public string background { get; set; }

        public string workExperience { get; set; }

        public string skill { get; set; }

        public string skillvalue { get; set; }

        public string hobies { get; set; }

        public string imgUrl { get; set; }
    }
}
