using GroupDelivery.Application.Abstractions;
using GroupDelivery.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GroupDelivery.Web.Controllers.Api
{
    [Authorize]
    [ApiController]
    [Route("api/merchant/stores")]
    public class MerchantStoreImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IStoreService _storeService;
        private readonly ICurrentStoreContextService _currentStoreContextService;

        public MerchantStoreImageController(
            IWebHostEnvironment env,
            IStoreService storeService,
            ICurrentStoreContextService currentStoreContextService)
        {
            _env = env;
            _storeService = storeService;
            _currentStoreContextService = currentStoreContextService;
        }

        [HttpPost("{storeId}/cover-image")]
        public async Task<IActionResult> UploadCoverImage(int storeId, IFormFile file)
        {
            return await UploadImage(storeId, file, ImageType.Cover);
        }

        [HttpPost("{storeId}/menu-image")]
        public async Task<IActionResult> UploadMenuImage(int storeId, IFormFile file)
        {
            return await UploadImage(storeId, file, ImageType.Menu);
        }

        private async Task<IActionResult> UploadImage(int storeId, IFormFile file, ImageType type)
        {
            try
            {
                var currentStoreId = _currentStoreContextService.GetCurrentStoreId(User);
                if (currentStoreId != storeId)
                    return Forbid();

                _storeService.ValidateImage(file?.FileName, file?.Length ?? 0);

                var imageUrl = await SaveStoreImage(storeId, file, type.ToString().ToLower());

                if (type == ImageType.Cover)
                    await _storeService.UpdateCoverImageAsync(storeId, imageUrl);
                else
                    await _storeService.UpdateMenuImageAsync(storeId, imageUrl);

                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<string> SaveStoreImage(int storeId, IFormFile file, string prefix)
        {
            var folder = Path.Combine(_env.WebRootPath, "uploads", "stores", storeId.ToString());
            Directory.CreateDirectory(folder);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{prefix}_{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/stores/{storeId}/{fileName}";
        }

        private enum ImageType
        {
            Cover,
            Menu
        }
    }
}
