using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDbContext context;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment , IHttpContextAccessor httpContextAccessor ,NZWalksDbContext context )
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;
        }
        public async Task<Image> Upload(Image image)
        {
            // that will be the local path file 
            var localFilePath =Path.Combine(webHostEnvironment.ContentRootPath,"Images",
                $"{image.FileName}{image.FileExtension}");

            //Upload Image to Local path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // https://localhost:{portNumber}/images/image.extension
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            // Add Image to the image table in DB
            await context.Images.AddAsync(image);
            await context.SaveChangesAsync();

            return image;

        }
    }
}
