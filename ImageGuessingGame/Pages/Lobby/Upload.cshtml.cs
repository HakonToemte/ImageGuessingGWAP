using System;
using System.IO;
using System.Web;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ImageGuessingGame.GameContext.GameHandling;
using ImageGuessingGame.GameContext;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImageGuessingGame.Pages
{
    public class UploadModel : PageModel
    {
        private readonly ILoginUserProvider _loginUserProvider;
        public string LoggedUser;
        [BindProperty]
        public IFormFile ImageFile{get;set;}

        public UploadModel(ILoginUserProvider loginProvider)
        {
            _loginUserProvider = loginProvider;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("_Name") != null)
            {
                LoggedUser = HttpContext.Session.GetString("_Name");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ImageFile == null){
                Console.WriteLine("NULL");
            }
            if (HttpContext.Session.GetString("_Name") != null)
            {
                LoggedUser = HttpContext.Session.GetString("_Name");
            }
            var path_for_fullimage= (Path.Combine($"wwwroot/uploads", ImageFile.FileName));
            var path_for_slices= "data";
            var directory = new DirectoryInfo(path_for_slices).CreateSubdirectory(Path.GetRandomFileName());

            using (Stream fileStream = new FileStream(path_for_fullimage, FileMode.Create)) {
                    await ImageFile.CopyToAsync(fileStream);
                };
            var imageProcessor = new ImageProcessor();
            imageProcessor.AutomaticSliceVoronoi(path_for_fullimage, directory.FullName);
            //imageProcessor.AutomaticSlice(path_for_fullimage, directory.FullName);
            string label = Request.Form["label"];
            AddLabel(label, directory.Name);
            return Page();
        }
        public static void AddLabel(string label, string directory){
            using(StreamWriter w = System.IO.File.AppendText("data/image_mapping.csv"))
            {
                w.WriteLine($"{directory} {label.Replace(" ","")}");
            }
            using(StreamWriter sw = System.IO.File.AppendText("data/label_mapping.csv"))
            {
                sw.WriteLine($"{label.Replace(" ","")} {label}");
            }
        }
    }
}
