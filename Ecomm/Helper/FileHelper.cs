using Azure.Storage.Blobs;
using System.Net;

namespace Ecomm.Helper
{
    public static class FileHelper
    {
        public static async Task<string> UploadImage(IFormFile file)
        {
            string filename = Guid.NewGuid().ToString();
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=shoppingstorages;AccountKey=BjRk+rB39HFtdSwQOfrH3Yr2Q3BfAf092Z4yZGSbO0TtqiWGa+wqkvH3C9luzrEXTtRSwGEOQW/n+AStxnpocg==;EndpointSuffix=core.windows.net";
            string containerName = "bookphotos";
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = containerClient.GetBlobClient(filename+file.FileName);
            MemoryStream ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Position = 0;
            await blobClient.UploadAsync(ms);
            return blobClient.Uri.AbsoluteUri;
        }
        public static async Task<string> UploadUrl(IFormFile file)
        {
            string filename = Guid.NewGuid().ToString();
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=shoppingstorages;AccountKey=BjRk+rB39HFtdSwQOfrH3Yr2Q3BfAf092Z4yZGSbO0TtqiWGa+wqkvH3C9luzrEXTtRSwGEOQW/n+AStxnpocg==;EndpointSuffix=core.windows.net";
            string containerName = "bookurl";
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = containerClient.GetBlobClient(filename+file.FileName);
            MemoryStream ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Position = 0;
            await blobClient.UploadAsync(ms);
            return blobClient.Uri.AbsoluteUri;
        }
    }
}