using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class CreateSelectionDecisionDto
    {
        [Required]
        public Guid ApplicationId { get; set; }
        [Required]
        public SelectionDecision Decision { get; set; }
        [Required]
        [MaxLength(2000)]
        public string? Notes { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Department { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Position { get; set; }
    }
}
