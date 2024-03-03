#if DEBUG
using Microsoft.AspNetCore.Mvc;

namespace BetterBeatSaber.Server.Controllers;

[Route("download")]
[Controller]
public sealed class DownloadController : ControllerBase {

    [HttpGet]
    public ActionResult Download() {
        var fileStream = System.IO.File.OpenRead(Path.Combine(Environment.CurrentDirectory, "Public", "BetterBeatSaber.Online.dll"));
        return File(fileStream, "application/octet-stream", "BetterBeatSaber.Online.dll");
    }

}
#endif