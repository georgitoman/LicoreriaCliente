using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.Helpers
{
    public class UploadService
    {
        PathProvider PathProvider;

        public UploadService(PathProvider pathProvider)
        {
            this.PathProvider = pathProvider;
        }

        public async Task<String> UploadFileAsync(IFormFile fichero, Folders folder, String filename)
        {
            String path = this.PathProvider.MapPath(filename, folder);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await fichero.CopyToAsync(stream);
            }

            return path;
        }
    }
}
