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
        BlobContainerClient containerClient;

        public ImagesService(String keys)
        {
            this.client = new BlobServiceClient(keys);
            this.containerClient = this.client.GetBlobContainerClient("imageneslicoreria");
        }

        public async Task UploadImageAsync(String nombre, Stream stream)
        {
            await
                this.containerClient.UploadBlobAsync(nombre, stream);

        }

        public async Task DeleteImageAsync(String nombre)
        {
            try
            {
                await this.containerClient.DeleteBlobAsync(nombre);
            } catch(RequestFailedException e) { };
            
        }

    }
}
