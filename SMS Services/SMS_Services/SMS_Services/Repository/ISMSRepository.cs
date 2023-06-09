﻿using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using SMS_Services.Common;
using SMS_Services.Entity;
using SMS_Services.Model;

namespace SMS_Services.Repository
{
    public interface ISMSRepository
    {
        #region SMS REPO
        //Task<string> InsertSMS_Bulk(List<OrderDetails> list);
        //Task<Order> InsertOrder(Order order);
        //Task<Order> UpdateOrder(Order order);
        //Task<Order> GetOrderByOrderNo(string orderNo);
        //Task<List<OrderDetails>> GetOrderDetails(string orderNo);
        Task<List<Port>> PortList();
        void SendSMSDirect(string phone_number, string message);
        void check_port();
        Task<List<Message_Receive>> GetListSMSByPhone(string phone_receive, string phone_send);
        Task<List<Message_Receive>> GetListSMSByPhone2(string phone_receive, string? phone_send);
        Task<List<Message_Receive>> GetListSMSReceive(long user_id);
        Task<List<Message_Receive>> GetListSMSReceiveAdmin();
        Task<string> Create_SMS_Receive(List<Message_Receive> model);
        #endregion
        #region Admin
        Task<Admin_User> UserGetById(long id);
        Task<Admin_User> UserCreate(Admin_User model);
        Task<Admin_User> UserModify(Admin_User model);
        Task<List<Admin_User>> UserList(string? username);
        int Authenticate(LoginModel login);
        Task<Admin_User> CheckUser(string username);
        Task<int> CheckUserExists(string username);
        #endregion
      
        #region SMS Request
        Task<bool> Request(Data_Upload data);
        #endregion
    }
}
