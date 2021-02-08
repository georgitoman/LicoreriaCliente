using Licoreria.Models;
using Licoreria.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.ViewComponents
{
    public class MenuCategoriasViewComponent: ViewComponent
    {
        RepositoryLicoreria repo;

        public MenuCategoriasViewComponent(RepositoryLicoreria repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Categoria> categorias = this.repo.GetCategorias();
            return View(categorias);
        }
    }
}
