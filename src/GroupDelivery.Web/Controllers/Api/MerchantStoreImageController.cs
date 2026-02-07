using GroupDelivery.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GroupDelivery.Web.Controllers.Api
{
    [Authorize]
    [ApiController]
    [Route("api/merchant/stores")]
    public class MerchantStoreImageController : ControllerBase
    {
        private readonly GroupDeliveryDbContext _db;
        private readonly IWebHostEnvironment _env;

        public MerchantStoreImageController(
            GroupDeliveryDbContext db,
            IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [HttpPost("{storeId}/cover-image")]
        public async Task<IActionResult> UploadCoverImage(
            int storeId,
            IFormFile file)
        {
            return await UploadImage(storeId, file, ImageType.Cover);
        }

        [HttpPost("{storeId}/menu-image")]
        public async Task<IActionResult> UploadMenuImage(
            int storeId,
            IFormFile file)
        {
            return await UploadImage(storeId, file, ImageType.Menu);
        }

        private async Task<IActionResult> UploadImage(
            int storeId,
            IFormFile file,
            ImageType type)
        {
            if (file == null || file.Length == 0)
                return BadRequest("未選擇檔案");

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var store = await _db.Stores
                .FirstOrDefaultAsync(s => s.StoreId == storeId && s.OwnerUserId == userId);

            if (store == null)
                return Forbid();

            // 儲存路徑
            var folder = Path.Combine(_env.WebRootPath, "uploads", "stores", storeId.ToString());
            Directory.CreateDirectory(folder);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{type.ToString().ToLower()}_{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = $"/uploads/stores/{storeId}/{fileName}";

            if (type == ImageType.Cover)
                store.CoverImageUrl = imageUrl;
            else
                store.MenuImageUrl = imageUrl;

            store.ModifiedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return Ok(new { imageUrl });
        }

        private enum ImageType
        {
            Cover,
            Menu
        }
    }
}
