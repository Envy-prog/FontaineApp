namespace FontaineApp.Models.ViewModels
{
    public class UploadedFileViewModel : UploadedFile
    {
        public byte[] FileData { get; set; }
        public List<UploadedFile> SystemFiles { get; set; }
    }
}
