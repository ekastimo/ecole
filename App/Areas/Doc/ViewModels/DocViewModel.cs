using Core.Models;

namespace App.Areas.Doc.ViewModels
{
    public class DocViewModel:ViewModel
    {
        public string Description { get; set; }
        public string OriginalFileName { get; set; }
        public string FileName { get; set; }
        public double SizeInMbs { get; set; }
        public string ContentType { get; set; }
    }
}
