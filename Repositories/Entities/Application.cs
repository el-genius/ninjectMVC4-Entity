using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NowOnline.AppHarbor.Repositories
{
    public class Application
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual Team Team { get; set; }
        public string Name { get; set; }
        public string BitBucketName { get; set; }

        // navigation properties
        public virtual ICollection<Tag> Tags { get; set; }

        [ForeignKey("Team")]
        public int TeamId { get; set; }
    }
}
