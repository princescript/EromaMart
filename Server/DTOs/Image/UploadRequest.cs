namespace Server.DTOs.Image;

public class UploadRequest
{
    public int ProductId { get; set; }
    public List<IFormFile> Files { get; set; } = new();
}
public class CloudinaryResponse
{
    public string Url { get; set; } =  "";
    public string PublicId { get; set; } = null!;
}

//public class UploadResponse
//{
//    public int ProductId { get; set; }
//    public List<string> Urls { get; set; } = new();
//}
public class UploadResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}