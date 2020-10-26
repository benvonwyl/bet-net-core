using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace API_BET.Model
{
    public class IdentifierModel
    {
        [Required]
        public long Id { get; set; }
        
    }
}
