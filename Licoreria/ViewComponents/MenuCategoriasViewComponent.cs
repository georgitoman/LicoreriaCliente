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
        IRepositoryLicoreria repo;

        public MenuCategoriasViewComponent(IRepositoryLicoreria repo)
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
