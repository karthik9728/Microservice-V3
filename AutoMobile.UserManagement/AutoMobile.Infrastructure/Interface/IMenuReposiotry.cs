using AutoMobile.Domain.InputModel;
using AutoMobile.Domain.Models;
using AutoMobile.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Infrastructure.Interface
{
    public interface IMenuReposiotry
    {
        Task<List<MenuVM>> GetMenuList();

        Task UpdateMenu(MenuUpdateInputModel menu);
    }
}
