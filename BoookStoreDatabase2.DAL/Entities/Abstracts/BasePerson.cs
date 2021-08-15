using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BoookStoreDatabase2.DAL.Entities.Abstracts
{
    public abstract class BasePerson : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        public DateTime DOB { get; set; }
    }
}
