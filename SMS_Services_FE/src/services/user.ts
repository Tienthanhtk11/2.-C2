import { host } from "./host";

export function user() {
    let url = `${host}/admin-user/`
    return {
        admin: () => {
            return {
                list: (data: any) => `${url}list?user_name=${data.user_name}`,
                create: () => `${url}create`,
                modify: () => `${url}modify`,
            }
        },
        login: () => `${url}login`
    }
}