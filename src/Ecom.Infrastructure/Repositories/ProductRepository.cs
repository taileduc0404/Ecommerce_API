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
            var currentProduct = await _context.products.FindAsync(id);
            if (currentProduct is not null)
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

                //remove oldPicture
                if (!string.IsNullOrEmpty(currentProduct.ProductPicture))
                {
                    //delete picture
                    var pictureInfo = _fileProvider.GetFileInfo(currentProduct.ProductPicture);
                    var rootPath = pictureInfo.PhysicalPath;
                    System.IO.File.Delete(rootPath);
                }

                //Create New Product
                var res = _mapper.Map<Product>(dto);
                res.ProductPicture = source;
                _context.products.Update(res);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

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
