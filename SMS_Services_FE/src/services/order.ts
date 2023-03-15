import { host } from "./host";

export function order() {
  let url = `${host}/SMS/`;
  return {
    list: () => `${url}order-list`,
    item: (id: number = 0) => `${url}order-detail?id=${id}`,
    create: () => `${url}order-create`,
    modify: () => `${url}order-modify`,
    delete: (id: number = 0) => `${url}-delete?id=${id}`,
  };
}