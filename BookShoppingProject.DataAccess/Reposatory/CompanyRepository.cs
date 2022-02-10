using BookShoppingProject.DataAccess.Data;
using BookShoppingProject.DataAccess.Repository.IRepository;
using BookShoppingProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppingProject.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _context;
        public CompanyRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        public void Update(Company company)
        {
            var companyInDb = _context.Companies.FirstOrDefault(p => p.Id == company.Id);
            if (companyInDb != null)
            {
                if (company.ImageURL != null)
                    companyInDb.ImageURL = company.ImageURL;
                companyInDb.Name = company.Name;
                companyInDb.StreetAddress = company.StreetAddress;
                companyInDb.City = company.City;
                companyInDb.State = company.State;
                companyInDb.PostalCode = company.PostalCode;
                companyInDb.PhoneNumber = company.PhoneNumber;
                companyInDb.IsAuthorizedCompany = company.IsAuthorizedCompany;
               
            }
        }
    }
}
