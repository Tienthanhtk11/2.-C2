using Microsoft.EntityFrameworkCore;
using Point_of_Sale.Controllers;

namespace Point_of_Sale.Entities
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
        public virtual DbSet<Admin_User> Admin_User { set; get; }
        public virtual DbSet<Admin_Group> Admin_Group { set; get; }
        public virtual DbSet<Admin_Role> Admin_Role { set; get; }
        public virtual DbSet<Admin_Role_Detail> Admin_Role_Detail { set; get; }
        public virtual DbSet<Admin_Group_User> Admin_Group_User { set; get; }
        public virtual DbSet<Admin_Role_Group> Admin_Role_Group { set; get; }
        public virtual DbSet<Admin_Role_Group_Detail> Admin_Role_Group_Detail { set; get; }
        public virtual DbSet<Admin_User_Warehouse> Admin_User_Warehouse { set; get; }
        public virtual DbSet<Product> Product { set; get; }
        public virtual DbSet<Product_Partner> Product_Partner { set; get; }
        public virtual DbSet<Product_Combo> Product_Combo { set; get; }
        public virtual DbSet<Product_Warehouse> Product_Warehouse { set; get; }
        public virtual DbSet<Product_Warehouse_History> Product_Warehouse_History { set; get; }
        public virtual DbSet<Product_Warehouse_Price_History> Product_Warehouse_Price_History { set; get; }
        public virtual DbSet<Category_Product> Category_Product { set; get; }
        public virtual DbSet<Category_Unit> Category_Unit { set; get; }
        public virtual DbSet<Category_Packing> Category_Packing { set; get; }
        public virtual DbSet<Category_District> Category_District { set; get; }
        public virtual DbSet<Category_Province> Category_Province { set; get; }
        public virtual DbSet<Category_Ward> Category_Ward { set; get; }
        public virtual DbSet<Partner> Partner { set; get; }
        public virtual DbSet<Voucher> Voucher { set; get; }
        public virtual DbSet<Order> Order { set; get; }
        public virtual DbSet<Order_Detail> Order_Detail { set; get; }
        public virtual DbSet<Sales_Session> Sales_Session { set; get; }
        public virtual DbSet<Customer> Customer { set; get; }
        public virtual DbSet<Warehouse> Warehouse { set; get; }
        public virtual DbSet<Warehouse_Request> Warehouse_Request { set; get; }
        public virtual DbSet<Warehouse_Request_Product> Warehouse_Request_Product { set; get; }
        public virtual DbSet<Warehouse_Receipt> Warehouse_Receipt { set; get; }
        public virtual DbSet<Warehouse_Receipt_Product> Warehouse_Receipt_Product { set; get; }
        public virtual DbSet<Warehouse_Export> Warehouse_Export { set; get; }
        public virtual DbSet<Warehouse_Export_Product> Warehouse_Export_Product { set; get; }
        public virtual DbSet<Purchase> Purchase { set; get; }
        public virtual DbSet<Purchase_Product> Purchase_Product { set; get; }
        public virtual DbSet<BankAccount> BankAccount { set; get; }
        public virtual DbSet<Refund> Refund { set; get; }
        public virtual DbSet<Refund_Order> Refund_Order { set; get; }
        public virtual DbSet<Refund_Order_Product> Refund_Order_Product { set; get; }
        public virtual DbSet<POS_File> POS_File { set; get; }
        public virtual DbSet<Warehouse_Inventory> Warehouse_Inventory { set; get; }
        public virtual DbSet<Warehouse_Inventory_Product> Warehouse_Inventory_Product { set; get; }
        public virtual DbSet<Promotion> Promotion { set; get; }
        public virtual DbSet<Promotion_Product> Promotion_Product { set; get; }
        public virtual DbSet<Promotion_Product_Includes> Promotion_Product_Includes { set; get; }
        public virtual DbSet<Promotion_Product_Item> Promotion_Product_Item { set; get; }
        public virtual DbSet<Promotion_Schedule> Promotion_Schedule { set; get; }
        public virtual DbSet<Promotion_Schedule_Time> Promotion_Schedule_Time { set; get; }
        public virtual DbSet<Promotion_Warehouse> Promotion_Warehouse { set; get; }
        public virtual DbSet<Category_Stalls> Category_Stalls { set; get; }
        public virtual DbSet<Category_Group> Category_Group { set; get; }
        public virtual DbSet<Combo> Combo { set; get; }
        public virtual DbSet<Customer_Member_Config> Customer_Member_Config { set; get; }
        public virtual DbSet<Customer_Point_History> Customer_Point_History { set; get; }

    }
}
