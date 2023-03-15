using AutoMapper;
using Point_of_Sale.Controllers;
using Point_of_Sale.Entities;
using Point_of_Sale.Model;
using Point_of_Sale.Model.Customer;
using Point_of_Sale.Model.Inventory;
using Point_of_Sale.Model.Product;
using Point_of_Sale.Model.Promotion;
using Point_of_Sale.Model.Purchase;
using Point_of_Sale.Model.Receipt;
using Point_of_Sale.Model.Refund;
using Point_of_Sale.Model.Request;
using Point_of_Sale.Model.SaleSession;
using Point_of_Sale.Model.User;
using Point_of_Sale.Model.Warehouse;

namespace Point_of_Sale.Mapper
{
    public class AddAutoMapper : Profile
    {
        public AddAutoMapper()
        {
            CreateMap<Admin_User, UserCreateModel>();
            CreateMap<UserCreateModel, Admin_User>();
            CreateMap<Admin_User, UserModifyModel>();
            CreateMap<UserModifyModel, Admin_User>();
            CreateMap<Admin_User, UserModel>();

            CreateMap<Product, ProductModel>();
            CreateMap<ProductModel, Product>();


            CreateMap<Voucher, VoucherModel>();
            CreateMap<VoucherModel, Voucher>();
            
            CreateMap<Order, OrderModel>();
            CreateMap<OrderModel, Order>();

            CreateMap<PartnerModel, Partner>();
            CreateMap<Partner, PartnerModel>();


            CreateMap<Request_ProductModel, Warehouse_Request_Product>();
            CreateMap<Warehouse_Request_Product, Request_ProductModel>();


            CreateMap<RequestModel, Warehouse_Request>();
            CreateMap<Warehouse_Request, RequestModel>();


            CreateMap<Receipt_ProductModel, Warehouse_Receipt_Product>();
            CreateMap<Warehouse_Receipt_Product, Receipt_ProductModel>();


            CreateMap<ReceiptModel, Warehouse_Receipt>();
            CreateMap<Warehouse_Receipt, ReceiptModel>();

            CreateMap<CustomerModel, Customer>();
            CreateMap<Customer, CustomerModel>();

            CreateMap<Sales_SessionModel, Sales_Session>();
            CreateMap<Sales_Session, Sales_SessionModel>();

            CreateMap<Warehouse_Export, Warehouse_Export_Model>();
            CreateMap<Warehouse_Export_Model, Warehouse_Export>();


            CreateMap<Purchase, PurchaseModel>();
            CreateMap<PurchaseModel, Purchase>();


            CreateMap<PurchaseProductModel, Purchase_Product>();
            CreateMap<Purchase_Product, PurchaseProductModel>();

            CreateMap<RefundModel, Refund>();
            CreateMap<Refund, RefundModel>();
            CreateMap<RefundOrderModel, Refund_Order>();
            CreateMap<Refund_Order, RefundOrderModel>();
            CreateMap<RefundProduct, Refund_Order_Product>();
            CreateMap<Refund_Order_Product, RefundProduct>();

            CreateMap<Warehouse_Inventory_Model, Warehouse_Inventory>();
            CreateMap<Warehouse_Inventory, Warehouse_Inventory_Model>();
            CreateMap<Warehouse_Inventory_Product_Model, Warehouse_Inventory_Product>();
            CreateMap<Warehouse_Inventory_Product, Warehouse_Inventory_Product_Model>();

            CreateMap<Product_Warehouse, Product_Warehouse>();

            CreateMap<Promotion, PromotionModel>();
            CreateMap<PromotionModel, Promotion>();

            CreateMap<Warehouse_Model, Warehouse>();
            CreateMap<Warehouse, Warehouse_Model>();

            CreateMap<Product_Partner_Model, Product_Partner>();
            CreateMap<Product_Partner, Product_Partner_Model>(); 
            
            CreateMap<Product_Combo, Product_Combo_Model>();
            CreateMap<Product_Combo_Model, Product_Combo>(); 

            CreateMap<Combo_Model, Combo>();
            CreateMap<Combo, Combo_Model>();
        }
    }
}
