using Server.DTOs.Image;
namespace Server.Services;

public interface ICloudinaryService
{
    Task<List<CloudinaryResponse>> UploadMultipleAsync(List<IFormFile> files);
    Task<bool> DeleteAsync(string publicId);

}

public class CloudinaryService : ICloudinaryService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public CloudinaryService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }
    public async Task<List<CloudinaryResponse>> UploadMultipleAsync(List<IFormFile> files)
    {
        //if (files == null || files.Count == 0)
        //    throw new ArgumentException("No files provided");

        var cloudName = _config["Cloudinary:CloudName"];
        var uploadPreset = _config["Cloudinary:UploadPreset"];

        if (string.IsNullOrWhiteSpace(cloudName))
            throw new Exception("Cloudinary CloudName is missing");

        if (string.IsNullOrWhiteSpace(uploadPreset))
            throw new Exception("Cloudinary UploadPreset is missing");

        var url = $"https://api.cloudinary.com/v1_1/{cloudName}/image/upload";

        var resultList = new List<CloudinaryResponse>();

        foreach (var file in files)
        {
            if (file == null || file.Length == 0)
                continue;

            using var form = new MultipartFormDataContent();

            // preset
            var presetContent = new StringContent(uploadPreset);
            presetContent.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "\"upload_preset\""
                };
            form.Add(presetContent);

            // file
            await using var stream = file.OpenReadStream();
            var fileContent = new StreamContent(stream);

            fileContent.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue(
                    file.ContentType ?? "application/octet-stream"
                );

            fileContent.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "\"file\"",
                    FileName = $"\"{file.FileName}\""
                };

            form.Add(fileContent);

            var response = await _http.PostAsync(url, form);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Cloudinary upload failed: {result}");

            using var json = System.Text.Json.JsonDocument.Parse(result);

            var root = json.RootElement;

            var secureUrl = root.GetProperty("secure_url").GetString();
            var publicId = root.GetProperty("public_id").GetString(); 

            if (!string.IsNullOrWhiteSpace(secureUrl) &&
                !string.IsNullOrWhiteSpace(publicId))
            {
                resultList.Add(new CloudinaryResponse
                {
                    Url = secureUrl!,
                    PublicId = publicId! 
                });
            }
        }

        return resultList;
    }
    public async Task<bool> DeleteAsync(string publicId)
    {
        var cloudName = _config["Cloudinary:CloudName"];

        if (string.IsNullOrWhiteSpace(cloudName))
            throw new Exception("Cloudinary CloudName is missing");

        var url = $"https://api.cloudinary.com/v1_1/{cloudName}/image/destroy";

        using var form = new MultipartFormDataContent();

        form.Add(new StringContent(publicId), "public_id");

        var response = await _http.PostAsync(url, form);
        var result = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return false;

        using var json = System.Text.Json.JsonDocument.Parse(result);

        if (json.RootElement.TryGetProperty("result", out var res))
        {
            return res.GetString() == "ok";
        }

        return false;
    }
}

