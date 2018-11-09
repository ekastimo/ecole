using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace App.Areas.Doc.ViewModels
{
    public class UploadRequest
    {
        public Guid? RefrenceId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Details { get; set; }
        
        [Required]
        public IFormFile File { get; set; }
        public bool IsPrimary { get; set; }
    }
}
