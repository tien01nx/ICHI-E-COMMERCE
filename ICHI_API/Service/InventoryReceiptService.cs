﻿using ICHI.DataAccess.Repository.IRepository;
using ICHI_API.Data;
using ICHI_API.Helpers;
using ICHI_API.Model;
using ICHI_API.Service.IService;
using ICHI_CORE.Domain.MasterModel;
using ICHI_CORE.NlogConfig;
using System.Linq.Dynamic.Core;
using System.Net.WebSockets;


namespace ICHI_API.Service
{
  public class InventoryReceiptService : IInventoryReceiptService
  {
    private readonly IUnitOfWork _unitOfWork;
    private PcsApiContext _db;

    public InventoryReceiptService(IUnitOfWork unitOfWork, IConfiguration configuration, PcsApiContext pcsApiContext)
    {
      _unitOfWork = unitOfWork;
      _db = pcsApiContext;
    }

    public InventoryReceipt Create(InventoryReceiptDTO data, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        // lấy ra employeeId theo userId 
        var employee = _unitOfWork.Employee.Get(u => u.UserId == data.EmployeeId);
        InventoryReceipt model = new InventoryReceipt
        {
          SupplierId = data.SupplierId,
          EmployeeId = employee.Id,
          Notes = data.Notes
        };
        _unitOfWork.InventoryReceipt.Add(model);
        _unitOfWork.Save();
        // lấy ra danh sách productId trong product
        var product = _unitOfWork.Product.GetAll();
        foreach (var item in data.InventoryReceiptDetails)
        {
          var productItem = product.FirstOrDefault(u => u.Id == item.ProductId);
          if (productItem == null)
          {
            strMessage = "Không tìm thấy sản phẩm";
            return null;
          }

          item.InventoryReceiptId = model.Id;
          item.ProductId = item.ProductId;
          item.Total = item.Total;
          item.Price = item.Price;
          _unitOfWork.InventoryReceiptDetail.Add(item);
          // cập nhật số lượng sản phẩm
          productItem.Quantity += item.Total;
          _unitOfWork.Product.Update(productItem);
        }
        _unitOfWork.Save();
        return model;
      }
      catch (Exception ex)
      {
        strMessage = ex.Message;
        NLogger.log.Error(ex.ToString());
        return null;
      }
    }

    public bool Delete(int id, out string strMessage)
    {
      throw new NotImplementedException();
    }

    public InventoryReceiptDTO FindById(int id, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var data = _unitOfWork.InventoryReceiptDetail.GetAll(u => u.InventoryReceiptId == id, includeProperties: "InventoryReceipt,Product").ToList();
        if (data == null)
        {
          strMessage = "Không tìm thấy phiếu nhập";
          return null;
        }
        var employee = _unitOfWork.Employee.Get(u => u.Id == data.FirstOrDefault().InventoryReceipt.EmployeeId);
        var supllier = _unitOfWork.Supplier.Get(u => u.Id == data.FirstOrDefault().InventoryReceipt.SupplierId);
        InventoryReceiptDTO model = new InventoryReceiptDTO
        {
          EmployeeId = data.FirstOrDefault().InventoryReceipt.EmployeeId.ToString(),
          FullName = employee.FullName,
          SupplierId = data.FirstOrDefault().InventoryReceipt.SupplierId,
          SupplierName = supllier.SupplierName,
          Notes = data.FirstOrDefault().InventoryReceipt.Notes,
          InventoryReceiptDetails = data
        };
        return model;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }

    public Helpers.PagedResult<InventoryReceipt> GetAll(string name, int pageSize, int pageNumber, string sortDir, string sortBy, out string strMessage)
    {
      strMessage = string.Empty;
      try
      {
        var query = _unitOfWork.InventoryReceipt.GetAll(includeProperties: "Supplier,Employee").AsQueryable();

        if (!string.IsNullOrEmpty(name))
        {
          query = query.Where(e => e.Supplier.SupplierName.Contains(name));
        }
        //var orderBy = $"{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}";
        var orderBy = sortBy switch
        {
          "FullName" => $"Employee.{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}",
          "SupplierName" => $"Supplier.{sortBy} {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}",
          _ => $"Id {(sortDir.ToLower() == "asc" ? "ascending" : "descending")}"
        };

        query = query.OrderBy(orderBy);
        var pagedResult = Helpers.PagedResult<InventoryReceipt>.CreatePagedResult(query, pageNumber, pageSize);
        return pagedResult;
      }
      catch (Exception ex)
      {
        NLogger.log.Error(ex.ToString());
        strMessage = ex.ToString();
        return null;
      }
    }
  }
}