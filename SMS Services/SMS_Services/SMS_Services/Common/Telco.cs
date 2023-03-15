namespace SMS_Services.Common
{
    public class Telco
    {
        public Telco()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public string GetTelco(string phone)
        {
            if (phone.StartsWith("0"))
            {
                phone = "84" + phone.Remove(0, 1);
            }
            if (!phone.ToString().Trim().StartsWith("84"))
            {
                phone = "84" + phone.ToString();
            }
            string telco = "UNSUPPORT";
            switch (phone)
            {
                case string sphone when (((sphone.StartsWith("8489") || sphone.StartsWith("8490") || sphone.StartsWith("8493")) && sphone.Length.Equals(11))
                || ((sphone.StartsWith("84120") || sphone.StartsWith("84121") || sphone.StartsWith("84122") || sphone.StartsWith("84126") || sphone.StartsWith("84128")) && sphone.Length.Equals(12))
                || ((sphone.StartsWith("8470") || sphone.StartsWith("8479") || sphone.StartsWith("8477") || sphone.StartsWith("8476") || sphone.StartsWith("8478")) && sphone.Length.Equals(11))):
                    //telco = "VMS";
                    telco = "Mobifone";
                    break;
                case string sphone when (((sphone.StartsWith("8488") || sphone.StartsWith("8491") || sphone.StartsWith("8494")) && sphone.Length.Equals(11)) || ((sphone.StartsWith("84123") || sphone.StartsWith("84124") || sphone.StartsWith("84125") || sphone.StartsWith("84127") || sphone.StartsWith("84129")) && sphone.Length.Equals(12)) || ((sphone.StartsWith("8483") || sphone.StartsWith("8484") || sphone.StartsWith("8485") || sphone.StartsWith("8481") || sphone.StartsWith("8482")) && sphone.Length.Equals(11))):
                    //telco = "GPC";
                    telco = "Vinaphone";
                    break;
                case string sphone when (((sphone.StartsWith("8486") || sphone.StartsWith("8497") || sphone.StartsWith("8496") || sphone.StartsWith("8498")) && phone.Length.Equals(11)) || (sphone.StartsWith("8416") && sphone.Length.Equals(12)) || (sphone.StartsWith("843") && sphone.Length.Equals(11) && !sphone.StartsWith("8430") && !sphone.StartsWith("8431"))):
                    //telco = "VIETTEL";
                    telco = "Viettel";
                    break;
                case string sphone when ((sphone.StartsWith("8492") && sphone.Length.Equals(11)) || ((sphone.StartsWith("84188") || sphone.StartsWith("84186")) && sphone.Length.Equals(12)) || ((sphone.StartsWith("8456") || sphone.StartsWith("8458") || sphone.StartsWith("8452")) && sphone.Length.Equals(11))):
                    //telco = "VNM";
                    telco = "Vietnammobile";
                    break;
                case string sphone when ((sphone.StartsWith("8459") && sphone.Length.Equals(11)) || (sphone.StartsWith("84199") && sphone.Length.Equals(12)) || ((sphone.StartsWith("84996") || sphone.StartsWith("84994") || sphone.StartsWith("84995") || sphone.StartsWith("84991") || sphone.StartsWith("84992") || sphone.StartsWith("84993") || sphone.StartsWith("84997")) && sphone.Length.Equals(11))):
                    telco = "BEELINE";
                    break;
                case string sphone when ((sphone.StartsWith("8487"))):
                    telco = "ITEL";
                    break;
                case string sphone when ((sphone.StartsWith("8455"))):
                    telco = "REDDI";
                    break;
                default:
                    break;
            }
            return telco;
        }
        public string Format_Phone(string phone)
        {
            phone = phone.Replace(" ", "");
            if (phone.ToString().Trim().StartsWith("0"))
            {
                phone = "84" + phone.ToString().Trim().Remove(0, 1);
            }
            if (!phone.ToString().Trim().StartsWith("84"))
            {
                phone = "84" + phone.ToString();
            }
            if (phone.StartsWith("841"))
            {
                phone = Convert_PhoneNumber_84(phone);
            }
            return phone;
        }
        public string Convert_PhoneNumber_84(string phone)
        {
            while (true)
            {
                switch (phone)
                {
                    //Viettel
                    case string sphone when sphone.StartsWith("8416"):
                        phone = "843" + phone.Remove(0, 4);
                        break;
                    //VMM
                    case string sphone when sphone.StartsWith("84186"):
                        phone = "8456" + phone.Remove(0, 5);
                        break;
                    case string sphone when sphone.StartsWith("84188"):
                        phone = "8458" + phone.Remove(0, 5);
                        break;
                    //VMS
                    case string sphone when sphone.StartsWith("84120"):
                        phone = "8470" + phone.Remove(0, 5);
                        break;
                    case string sphone when sphone.StartsWith("84121"):
                        phone = "8479" + phone.Remove(0, 5);
                        break;

                    case string sphone when sphone.StartsWith("84122"):
                        phone = "8477" + phone.Remove(0, 5);
                        break;
                    case string sphone when sphone.StartsWith("84126"):
                        phone = "8476" + phone.Remove(0, 5);
                        break;

                    case string sphone when sphone.StartsWith("84128"):
                        phone = "8478" + phone.Remove(0, 5);
                        break;
                    //GPC
                    case string sphone when sphone.StartsWith("84124"):
                        phone = "8484" + phone.Remove(0, 5);
                        break;
                    case string sphone when sphone.StartsWith("84127"):
                        phone = "8481" + phone.Remove(0, 5);
                        break;

                    case string sphone when sphone.StartsWith("84129"):
                        phone = "8482" + phone.Remove(0, 5);
                        break;
                    case string sphone when sphone.StartsWith("84123"):
                        phone = "8483" + phone.Remove(0, 5);
                        break;

                    case string sphone when sphone.StartsWith("84125"):
                        phone = "8485" + phone.Remove(0, 5);
                        break;
                    //GTEL
                    case string sphone when sphone.StartsWith("84199"):
                        phone = "8459" + phone.Remove(0, 5);
                        break;
                }
                break;
            }
            return phone;
        }
    }
}
