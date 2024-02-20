using AutoMapper;
using Azure.Core.Pipeline;
using Ecom.Core.DTOs;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace Ecom.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider _fileProvider;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext context, IFileProvider fileProvider,
            IMapper mapper) : base(context)
        {
            this._context = context;
            this._fileProvider = fileProvider;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAll(string sort, int? categoryId, int pageNumber, int pageSize)
        {
            var query = await _context.products
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync();

            //pagination
            pageNumber = (pageNumber > 0) ? pageNumber : 1;
            pageSize = (pageSize > 0) ? pageSize : 3;
            query = query.Skip((pageSize) * (pageNumber - 1)).Take(pageSize).ToList();


            //search by categoryId
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value).ToList();
            }


            //sort
            if (!string.IsNullOrEmpty(sort))
            {
                query = sort switch
                {
                    "PriceAsync" => query.OrderBy(x => x.Price).ToList(),
                    "PriceDesc" => query.OrderByDescending(x => x.Price).ToList(),
                    _ => query.OrderBy(x => x.Name).ToList(),
                };
            }
            var res = _mapper.Map<List<ProductDto>>(query);
            return res;
        }

        public async Task<bool> AddAsync(AddProductDto dto)
        {
            var source = "";
            if (dto.Image is not null)
            {
                var root = "/images/products/";
                var productName = $"{Guid.NewGuid()}" + dto.Image.FileName;
                if (!Directory.Exists("wwwroot" + root))
                {
                    Directory.CreateDirectory("wwwroot" + root);
                }
                source = root + productName;
                var picInfo = _fileProvider.GetFileInfo(source);
                var rootPath = picInfo.PhysicalPath;
                using (var fileStream = new FileStream(rootPath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(fileStream);
                }


            }
            //Create New Product
            var res = _mapper.Map<Product>(dto);
            res.ProductPicture = source;
            await _context.products.AddAsync(res);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
        {
            //var currentProduct = await _context.products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            var currentProduct = await _context.products.FindAsync(id);
            if (currentProduct is not null)
            {

                var src = "";
                if (dto.Image is not null)
                {
                    var root = "/images/products/";
                    var productName = $"{Guid.NewGuid()}" + dto.Image.FileName;
                    if (!Directory.Exists("wwwroot" + root))
                    {
                        Directory.CreateDirectory("wwwroot" + root);
                    }

                    src = root + productName;
                    var picInfo = _fileProvider.GetFileInfo(src);
                    var rootPath = picInfo.PhysicalPath;
                    using (var fileStream = new FileStream(rootPath, FileMode.Create))
                    {
                        await dto.Image.CopyToAsync(fileStream);
                    }
                }
                //remove old picture
                if (!string.IsNullOrEmpty(currentProduct.ProductPicture))
                {
                    //delete old picture
                    var picInfo = _fileProvider.GetFileInfo(currentProduct.ProductPicture);
                    var rootPath = picInfo.PhysicalPath;
                    System.IO.File.Delete(rootPath);
                }

                //update product
                var res = _mapper.Map(dto, currentProduct);
                res.ProductPicture = src;
                //res.Id = id;
                _context.products.Update(res);
                await _context.SaveChangesAsync();


                return true;

            }
            return false;
        }

        //public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
        //{
        //    var currentProduct = await _context.products.FindAsync(id);
        //    if (currentProduct is not null)
        //    {
        //        var source = "";
        //        if (dto.Image is not null)
        //        {
        //            var root = "/images/products/";
        //            var productName = $"{Guid.NewGuid()}" + dto.Image.FileName;
        //            if (!Directory.Exists("wwwroot" + root))
        //            {
        //                Directory.CreateDirectory("wwwroot" + root);
        //            }
        //            source = root + productName;
        //            var picInfo = _fileProvider.GetFileInfo(source);
        //            var rootPath = picInfo.PhysicalPath;
        //            using (var fileStream = new FileStream(rootPath, FileMode.Create))
        //            {
        //                await dto.Image.CopyToAsync(fileStream);
        //            }


        //        }

        //        //remove oldPicture
        //        if (!string.IsNullOrEmpty(currentProduct.ProductPicture))
        //        {
        //            //delete picture
        //            var pictureInfo = _fileProvider.GetFileInfo(currentProduct.ProductPicture);
        //            var rootPath = pictureInfo.PhysicalPath;
        //            System.IO.File.Delete(rootPath);
        //        }

        //        //Create New Product
        //        var res = _mapper.Map<Product>(dto);
        //        res.ProductPicture = source;
        //        _context.products.Update(res);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    return false;
        //}

        public async Task<bool> DeleteAsyncWithPicture(int id)
        {
            var currentProduct = await _context.products.FindAsync(id);
            if (currentProduct != null)
            {
                //remove oldPicture
                if (!string.IsNullOrEmpty(currentProduct.ProductPicture))
                {
                    //delete picture
                    var pictureInfo = _fileProvider.GetFileInfo(currentProduct.ProductPicture);
                    var rootPath = pictureInfo.PhysicalPath;
                    System.IO.File.Delete(rootPath);
                }

                _context.products.Remove(currentProduct);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
