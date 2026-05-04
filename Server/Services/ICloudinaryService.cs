namespace Server.Services;

public interface ICloudinaryService
{
    Task<List<string>> UploadMultipleAsync(List<IFormFile> files);
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

    public async Task<List<string>> UploadMultipleAsync(List<IFormFile> files)
    {
        if (files == null || files.Count == 0)
            throw new ArgumentException("No files provided");

        var cloudName = _config["Cloudinary:CloudName"];
        var uploadPreset = _config["Cloudinary:UploadPreset"];

        if (string.IsNullOrWhiteSpace(cloudName))
            throw new Exception("Cloudinary CloudName is missing");

        if (string.IsNullOrWhiteSpace(uploadPreset))
            throw new Exception("Cloudinary UploadPreset is missing");

        var url = $"https://api.cloudinary.com/v1_1/{cloudName}/image/upload";

        var resultList = new List<string>();

        foreach (var file in files)
        {
            if (file == null || file.Length == 0)
                continue;

            using var form = new MultipartFormDataContent();

            // ✅ preset (must be first for Cloudinary unsigned upload)
            var presetContent = new StringContent(uploadPreset);
            presetContent.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "\"upload_preset\""
                };
            form.Add(presetContent);

            // ✅ file
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

            // request
            var response = await _http.PostAsync(url, form);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Cloudinary upload failed: {result}");

            using var json = System.Text.Json.JsonDocument.Parse(result);

            if (json.RootElement.TryGetProperty("secure_url", out var urlElement))
            {
                var secureUrl = urlElement.GetString();
                if (!string.IsNullOrWhiteSpace(secureUrl))
                    resultList.Add(secureUrl);
            }
        }

        return resultList;
    }
}

