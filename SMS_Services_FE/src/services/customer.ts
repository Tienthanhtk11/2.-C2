import { host } from "./host";

export function customer() {
  let url = `${host}/customer/`;
  return {
    customer: () => {
      return {
        list: (data: any) => `${url}list?user_name=${data.user_name}`,
        item: (id: number = 0) => `${url}detail?id=${id}`,
        create: () => `${url}create`,
        modify: () => `${url}modify`,
        login: () => `${url}login`,
        listphonenumber: (customer_id: number) => `${url}list-phone-number?customer_id=${customer_id}`,
        listallphonenumber: () => `${url}list-all-phone-number`,
        createphonenumber: () => `${url}create-phone-number`,        
      };
    },
  };
}
