using Azure;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace LicoreriaCliente.Helpers
{
    public class ImagesService
    {
        private BlobServiceClient client;
        private string containername = "imageneslicoreria";

        public ImagesService(String keys)
        {
            this.client = new BlobServiceClient(keys);
        }

        public async Task UploadImageAsync(String nombre, Stream stream)
        {
            BlobContainerClient containerClient =
                this.client.GetBlobContainerClient(this.containername);
            await
                containerClient.UploadBlobAsync(nombre, stream);

        }

        public async Task DeleteImageAsync(String nombre)
        {
            BlobContainerClient containerClient =
                this.client.GetBlobContainerClient(this.containername);
            try
            {
                await containerClient.DeleteBlobAsync(nombre);
            } catch(RequestFailedException e) { };
            
        }

    }
}
