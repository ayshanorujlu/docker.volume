using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using mytestprojectasp.Models;
using System.Diagnostics;

namespace mytestprojectasp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFileProvider _fileProvider;
        public HomeController(IFileProvider fileProvider)
        {
            _fileProvider=fileProvider;
        }

        public IActionResult Index()
        {

            var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            var videoExtensions = new[] { ".mp4", ".mov", ".avi", ".mkv" };

            var files = _fileProvider.GetDirectoryContents("data");


            var images = files.Where(x => imageExtensions.Contains(Path.GetExtension(x.Name).ToLower()))
                              .Select(x => x.Name);
            var videos = files.Where(x => videoExtensions.Contains(Path.GetExtension(x.Name).ToLower()))
                              .Select(x => x.Name);


            var model = new FileViewModel
            {
                Images = images,
                Videos = videos
            };


            return View(model);
        }

        public IActionResult ImageSave()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImageSave(IFormFile formFile)
        {
            if(formFile!=null && formFile.Length>0)
            {
                var fileName=Guid.NewGuid().ToString()+Path.GetExtension(formFile.FileName);
                var path=Path.Combine(Directory.GetCurrentDirectory(),"data", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
            }
            return RedirectToAction("Index");
        }



        [HttpPost]
        public async Task<IActionResult> VideoSave(IFormFile videoFile)
        {
            if (videoFile!=null && videoFile.Length>0)
            {
                var allowedExtensions = new[] { ".mp4", ".mov", ".avi", ".mkv" };
                var extension = Path.GetExtension(videoFile.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    return View();
                }


                var fileName = Guid.NewGuid().ToString()+Path.GetExtension(videoFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "data", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await videoFile.CopyToAsync(stream);
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string name)
        {
            var file=_fileProvider.GetDirectoryContents("data").First(x=>x.Name==name);
            System.IO.File.Delete(file.PhysicalPath);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteVideo(string name)
        {
            var file = _fileProvider.GetDirectoryContents("data").First(x => x.Name==name);
            System.IO.File.Delete(file.PhysicalPath);
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
