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
      };
    },
  };
}
