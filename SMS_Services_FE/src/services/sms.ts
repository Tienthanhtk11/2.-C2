import { host } from "./host";

export function sms() {
    let url = `${host}/SMS/`;
    return {
        list: () => `${url}get-list-sms-receive`,
    };
}