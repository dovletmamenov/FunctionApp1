using Azure.Storage.Blobs;
using FunctionApp1.Interfaces;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class AzureBlobStorage : IBlobStorage
{
    private readonly BlobContainerClient _containerClient;

    public AzureBlobStorage(string connectionString, string containerName)
    {
        _containerClient = new BlobContainerClient(connectionString, containerName);
        _containerClient.CreateIfNotExists();
    }

    public async Task StoreResponseContentAsync(DateTime requestTime, string responseContent)
    {
        string blobName = requestTime.ToString("yyyyMMddHHmmss");
        BlobClient blobClient = _containerClient.GetBlobClient(blobName);

        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(responseContent)))
        {
            await blobClient.UploadAsync(stream, overwrite: true);
        }
    }

    public async Task<string> RetrieveResponseContentAsync(DateTime requestTime)
    {
        string blobName = requestTime.ToString("yyyyMMddHHmmss"); 
        BlobClient blobClient = _containerClient.GetBlobClient(blobName);

        if (await blobClient.ExistsAsync())
        {
            var response = await blobClient.DownloadContentAsync();
            using (var stream = response.Value.Content.ToStream())
            {
                using (var streamReader = new StreamReader(stream))
                {
                    return await streamReader.ReadToEndAsync();
                }
            }
        }

        return null;
    }
}
