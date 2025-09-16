using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Raffle.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private readonly long MaxFileSize = 5 * 1024 * 1024; // 5MB
        private readonly string UploadPath = "wwwroot/uploads/images";

        public UploadController()
        {
            if (!Directory.Exists(UploadPath))
            {
                Directory.CreateDirectory(UploadPath);
            }
        }

        [HttpPost("image")]
        public async Task<ActionResult<string>> UploadImage(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Nenhum arquivo foi enviado");
                }

                if (file.Length > MaxFileSize)
                {
                    return BadRequest($"Arquivo muito grande. Tamanho máximo: {MaxFileSize / (1024 * 1024)}MB");
                }

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(extension))
                {
                    return BadRequest("Formato de arquivo não aceito. Use: JPG, JPEG, PNG, GIF, WEBP");
                }

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(UploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var imageUrl = $"/uploads/images/{fileName}";
                return Ok(new { url = imageUrl, filename = fileName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor: " + ex.Message);
            }
        }

        [HttpPost("images")]
        public async Task<ActionResult<List<string>>> UploadImages(List<IFormFile> files)
        {
            try
            {
                if (files == null || files.Count == 0)
                {
                    return BadRequest("Nenhum arquivo foi enviado");
                }

                if (files.Count > 10)
                {
                    return BadRequest("Máximo de 10 imagens por vez");
                }

                var uploadedUrls = new List<string>();

                foreach (var file in files)
                {
                    if (file.Length == 0) continue;

                    if (file.Length > MaxFileSize)
                    {
                        return BadRequest($"Arquivo {file.FileName} muito grande. Tamanho máximo: {MaxFileSize / (1024 * 1024)}MB");
                    }

                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!AllowedExtensions.Contains(extension))
                    {
                        return BadRequest($"Formato do arquivo {file.FileName} não aceito. Use: JPG, JPEG, PNG, GIF, WEBP");
                    }

                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(UploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    uploadedUrls.Add($"/uploads/images/{fileName}");
                }

                return Ok(uploadedUrls);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor: " + ex.Message);
            }
        }

        [HttpDelete("image")]
        public ActionResult DeleteImage([FromQuery] string filename)
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                {
                    return BadRequest("Nome do arquivo não especificado");
                }

                var filePath = Path.Combine(UploadPath, filename);
                
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return Ok(new { message = "Imagem excluída com sucesso" });
                }

                return NotFound("Arquivo não encontrado");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor: " + ex.Message);
            }
        }
    }
}