using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BoookStoreDatabase2.DAL.Entities.Abstracts
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
