using AutoMapper;
using AutoMobile.Domain.InputModel;
using AutoMobile.Domain.Models;
using AutoMobile.Domain.ViewModel;
using AutoMobile.Infrastructure.Common;
using AutoMobile.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Infrastructure.Repository
{
    public class MenuReposiotry : IMenuReposiotry
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public MenuReposiotry(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<MenuVM>> GetMenuList()
        {
            var menus = await _dbContext.Menu.AsQueryable()
                .Where(x => x.IsActive == true)
                .Include(x=>x.SubMenu.Where(x=>x.IsActive == true))
                .ToListAsync();

            return _mapper.Map<List<MenuVM>>(menus);
        }

        public async Task UpdateMenu(MenuUpdateInputModel menu)
        {
            var objMenu = await _dbContext.Menu.FirstOrDefaultAsync(x => x.Id == menu.Id);

            if (objMenu != null)
            {
                objMenu.IsActive = menu.IsActive;

                _dbContext.Menu.Update(objMenu);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
