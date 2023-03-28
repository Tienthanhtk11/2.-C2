using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management;

namespace App.Common
{
    public class GsmDevice
    {
        public string PhoneNumber { get; set; }
        public string Telco { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DefaultValue(false)]
        public bool IsDeviceFound { get; set; }
        public GsmDevice() { }

        #region Liệt kê toàn bộ thiết bị GSM Modem được cắm vào máy tính
        public static List<GsmDevice> GetCOMPortsInfo()
        {
            //List<GsmDevice> gsmCom = new List<GsmDevice>();
            //ConnectionOptions options = new ConnectionOptions();
            //options.Impersonation = ImpersonationLevel.Impersonate;
            //options.EnablePrivileges = true; 

            //string conString = $@"\\{Environment.MachineName}\root\cimv2";

            //ManagementScope scope = new ManagementScope(conString, options);
            ////ManagementScope scope = new ManagementScope("\\\\.\\ROOT\\cimv2");
            //scope.Connect();
            ////Lấy toàn bộ cổng COM hiện có trên máy tính
            //ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_POTSModem");//Win32_POTSModem");
            //ManagementObjectSearcher search = new ManagementObjectSearcher(scope, query);
            //ManagementObjectCollection collection = search.Get();
            //foreach (ManagementObject obj in collection)
            //{
            //    {
            //        string portName = obj["AttachedTo"].ToString();
            //        string portDescription = obj["Description"].ToString();
            //        if (portName != "")
            //        {
            //            GsmDevice com = new GsmDevice();
            //            com.Name = portName;
            //            com.Description = portDescription;
            //            gsmCom.Add(com);
            //        }
            //    }
            //}
            //return gsmCom;
            List<GsmDevice> comPortInfoList = new List<GsmDevice>();

            System.Management.ConnectionOptions options = ProcessConnection.ProcessConnectionOptions();
            ManagementScope connectionScope = ProcessConnection.ConnectionScope(Environment.MachineName, options, @"\root\CIMV2");

            ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0");
            ManagementObjectSearcher comPortSearcher = new ManagementObjectSearcher(connectionScope, objectQuery);

            using (comPortSearcher)
            {
                string caption = null;
                foreach (ManagementObject obj in comPortSearcher.Get())
                {
                    if (obj != null)
                    {
                        object captionObj = obj["Caption"];
                        if (captionObj != null)
                        {
                            caption = captionObj.ToString();
                            if (caption.Contains("(COM"))
                            {
                                GsmDevice comPortInfo = new GsmDevice()
                                {
                                    Name = caption.Substring(caption.LastIndexOf("(COM")).Replace("(", string.Empty).Replace(")", string.Empty),
                                    Description = caption
                                };
                                comPortInfoList.Add(comPortInfo);
                            }
                        }
                    }
                }
            }
            return comPortInfoList;
        }
        #endregion

        #region Tìm kiếm thiết bị trả về cho thuộc tính IsDeviceFound
        public bool Searh()
        {
            IEnumerator en = GetCOMPortsInfo().GetEnumerator();
            GsmDevice com = en.MoveNext() ? (GsmDevice)en.Current : null;
            if (com == null)
            {
                IsDeviceFound = false;
            }
            else
            {
                IsDeviceFound = true;
            }
            return IsDeviceFound;
        }
        #endregion*/

        internal class ProcessConnection
        {
            public static System.Management.ConnectionOptions ProcessConnectionOptions()
            {
                System.Management.ConnectionOptions options = new ConnectionOptions()
                {
                    Impersonation = ImpersonationLevel.Impersonate,
                    Authentication = System.Management.AuthenticationLevel.Default,
                    EnablePrivileges = true
                };
                return options;
            }

            public static ManagementScope ConnectionScope(string machineName, System.Management.ConnectionOptions options, string path)
            {
                ManagementScope connectScope = new ManagementScope();
                connectScope.Path = new ManagementPath(@"\\" + machineName + path);
                connectScope.Options = options;
                connectScope.Connect();
                return connectScope;
            }
        }
    }
}
