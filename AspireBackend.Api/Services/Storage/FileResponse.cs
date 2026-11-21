namespace AspireBackend.Api.Services.Storage;

public record FileResponse(Stream stream, string ContentType);