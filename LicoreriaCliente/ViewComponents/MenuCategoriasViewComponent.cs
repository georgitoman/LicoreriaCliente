using LicoreriaCliente.Models;
using LicoreriaCliente.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicoreriaCliente.ViewComponents
{
    public class MenuCategoriasViewComponent: ViewComponent
    {
        LicoreriaService repo;

        public MenuCategoriasViewComponent(LicoreriaService repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Categoria> categorias = await this.repo.GetCategoriasAsync();
            return View(categorias);
        }
    }
}
