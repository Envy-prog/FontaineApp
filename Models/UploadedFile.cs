using System.ComponentModel.DataAnnotations;

namespace FontaineApp.Models
{
    public class UploadedFile
    {
        public int Id { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Display(Name = "File Path")]
        public string FilePath { get; set; }

        [Display(Name = "File Title")]
        public string Title { get; set; }

        [Display(Name = "File Description")]
        public string Description { get; set; }

        [Display(Name = "Uploaded At")]
        public DateTime UploadedAt { get; set; }
    }
}
