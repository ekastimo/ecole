using Core.Models;

namespace App.Areas.Documents.ViewModels
{
    public class DocumentViewModel:ViewModel
    {
        public string Description { get; set; }
        public string OriginalFileName { get; set; }
        public string FileName { get; set; }
        public double SizeInMbs { get; set; }
        public string ContentType { get; set; }
    }
}
