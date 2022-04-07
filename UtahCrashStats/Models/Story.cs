using System;
using System.ComponentModel.DataAnnotations;

namespace UtahCrashStats.Models
{
    public class Story
    {
        [Key]
        [Required]
        public int STORY_ID { get; set; }
        [Required]
        public DateTime UPLOAD_TIME { get; set; }
        [Required]
        public DateTime STORY_TIME { get; set; }
        [Required]
        public string STORY_CONTENT { get; set; }
        public string STORY_AUTHOR { get; set; }
        [Required]
        public string STORY_RELATION { get; set; }
        public int CRASH_ID { get; set; }
    }
}
