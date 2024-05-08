using CodenApp.Sdk.Infrastructure.Abstraction.Bus;
using Crefaz.Documento.Application.Interfaces;
using Crefaz.Documento.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Crefaz.Documento.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PdfController : ControllerBase
{
  private readonly IConfiguration _configuration;
  private readonly IMediatorHandler _mediator;
  private readonly IVideoService _videoService;

  public PdfController(
      IVideoService videoService,
      IConfiguration configuration,
      IMediatorHandler mediator
  )
  {
    _configuration = configuration;
    _mediator = mediator;
    _videoService = videoService;
  }


  [HttpPost("upload-video")]
  public async Task<IActionResult> UploadVideo(IFormFile uploadResquest)
  {
     if (uploadResquest == null || uploadResquest.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }
    await _videoService.UploadVideo(uploadResquest);
    return Ok("Video uploaded successfully.");
  }
}