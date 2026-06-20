using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class CreateJobDto
    {
        [Required]
        [MaxLength(100)]

        public string Title { get; set; }

        [Required]
        [MaxLength(100)]
        public string Department { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Requirements { get; set; }
    }
}
