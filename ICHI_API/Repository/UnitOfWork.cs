﻿using ICHI_API.Data;
using ICHI.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI.DataAccess.Repository
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private PcsApiContext _db;
    public ICategoryRepository Category { get; private set; }
    public IProductRepository Product { get; private set; }
    public ICustomerRepository Customer { get; private set; }
    public IEmployeeRepository Employee { get; private set; }
    public IInventoryReceiptDetailRepository InventoryReceiptDetail { get; private set; }
    public IInventoryReceiptRepository InventoryReceipt { get; private set; }
    public IProductDetailRepository ProductDetail { get; private set; }
    public IProductReturnRepository ProductReturn { get; private set; }
    public IProductReturnDetailRepository ProductReturnDetail { get; private set; }
    public IProductImagesRepository ProductImages { get; private set; }
    public IPromotionRepository Promotion { get; private set; }
    public IPromotionDetailRepository PromotionDetail { get; private set; }
    public IUserRoleRepository UserRole { get; private set; }
    public IUserRepository User { get; private set; }
    public IRoleRepository Role { get; private set; }
    public ISupplierRepository Supplier { get; private set; }
    public ITrademarkRepository Trademark { get; private set; }
    public ITransactionDetailRepository TransactionDetail { get; private set; }
    public ITrxTransactionRepository TrxTransaction { get; private set; }


    public UnitOfWork(PcsApiContext db)
    {
      _db = db;
      Category = new CategoryRepository(_db);
      Product = new ProductRepository(_db);
      Customer = new CustomerRepository(_db);
      Employee = new EmployeeRepository(_db);
      InventoryReceiptDetail = new InventoryReceiptDetailRepository(_db);
      InventoryReceipt = new InventoryReceiptRepository(_db);
      ProductDetail = new ProductDetailRepository(_db);
      ProductReturn = new ProductReturnRepository(_db);
      ProductReturnDetail = new ProductReturnDetailRepository(_db);
      ProductImages = new ProductImagesRepository(_db);
      Promotion = new PromotionRepository(_db);
      PromotionDetail = new PromotionDetailRepository(_db);
      UserRole = new UserRoleRepository(_db);
      User = new UserRepository(_db, _configuration, _httpContextAccessor);
      Role = new RoleRepository(_db);
      Supplier = new SupplierRepository(_db);
      Trademark = new TrademarkRepository(_db);
      TransactionDetail = new TransactionDetailRepository(_db);
      TrxTransaction = new TrxTransactionRepository(_db);

    }


    public void Save()
    {
      _db.SaveChanges();
    }
  }
}