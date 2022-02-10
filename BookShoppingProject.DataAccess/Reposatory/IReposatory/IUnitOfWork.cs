using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppingProject.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICompanyRepository Company { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IProductRepository Product { get; }
        ICategoryRepository Category { get;}

        ISP_CALLS SP_CALLS { get; }
        ICoverTypeRepository CoverType { get; }

        IShoppingCartRepository ShoppingCart { get; }

        IOrderHeaderRepository OrderHeader { get; }

        IOrderDetailRepository OrderDetail { get; }

        void Save();
    }
}
