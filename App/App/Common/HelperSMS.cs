using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace App
{
    public class HelperSMS
    {
        SMS_LIB.Telco ctelco = new SMS_LIB.Telco();
        SMS_LIB.SMS_Bussiness bussiness = new SMS_LIB.SMS_Bussiness();
        public HelperSMS()
        {

        }
        public string GetTelco(string phone)
        {
            phone = ctelco.Format_Phone(phone);
            string telco = GetTelcoNew(phone);
            return telco;
        }
        public string GetTelcoNew(string phone)
        {
            if (phone.StartsWith("0"))
            {
                phone = "84" + phone.Remove(0, 1);
            }

            if (!phone.ToString().Trim().StartsWith("84"))
            {
                phone = "84" + phone.ToString();
            }
            string result = "UNSUPPORT";
            string text = phone;
            string text2 = text;
            if (text2 != null)
            {
                string text3 = text2;
                if (text3.StartsWith("8488") || text3.StartsWith("8491") || text3.StartsWith("8494") || text3.StartsWith("8481") || text3.StartsWith("8482") || text3.StartsWith("8483") || text3.StartsWith("8484") || text3.StartsWith("8485") || text3.StartsWith("8483") || text3.StartsWith("8484") || text3.StartsWith("8485"))
                {
                    result = "VINAPHONE";
                }
                else
                {
                    string text4 = text2;
                    if (text4.StartsWith("8486") || text4.StartsWith("8497") || text4.StartsWith("8496") || text4.StartsWith("8498") || text4.StartsWith("8432") || text4.StartsWith("8433") || text4.StartsWith("8434") || text4.StartsWith("8435") || text4.StartsWith("8436") || text4.StartsWith("8437") || text4.StartsWith("8438") || text4.StartsWith("8439"))
                    {
                        result = "VIETTEL";
                    }
                    else
                    {
                        string text5 = text2;
                        if (text5.StartsWith("8490") || (text5.StartsWith("8493") || text5.StartsWith("8489")) || text5.StartsWith("8470") || text5.StartsWith("8479") || text5.StartsWith("8477") || text5.StartsWith("8476") || text5.StartsWith("8478"))
                        {
                            result = "MOBIFONE";
                        }
                        else
                        {
                            string text6 = text2;
                            if (text6.StartsWith("8492") || text6.StartsWith("8456") || text6.StartsWith("8458"))
                                result = "VIETNAMMOBILE";
                            else
                            {
                                string text7 = text2;
                                if (text7.StartsWith("8499")|| text7.StartsWith("8459"))
                                {
                                    result = "GMOBILE";
                                }
                                else
                                {
                                    string text8 = text2;
                                    if (text8.StartsWith("8487"))
                                    {
                                        result = "ITEL";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public int CountSms(string message)
        {
            int nLengthSms = bussiness.lengthSms(message);
            return bussiness.CountSMS(nLengthSms);
        }
        public string FormatPhone(string phone)
        {
            return ctelco.Format_Phone(phone);
        }
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
