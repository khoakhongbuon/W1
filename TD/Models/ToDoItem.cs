using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using TD.Areas.Identity.Data;
namespace TD.Models
{
    public class ToDoItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public bool IsCompleted { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual TDUser User { get; set; }
    }
}
