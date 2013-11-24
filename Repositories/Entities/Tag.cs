using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NowOnline.AppHarbor.Repositories
{
    public class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual Application Application { get; set; }
        public string Name { get; set; }
        public string Commit { get; set; }
        public DateTime Created { get; set; }

        // navigation properties
        [ForeignKey("Application")]
        public int ApplicationId { get; set; }
    }
}
