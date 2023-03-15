import { notification } from "antd"

export function mapSelectAntd(list: Array<any>, label: string, value: string) {
    return list.map((obj: any) => {
        return {
            value: obj[value],
            label: obj[label],
        }
    })
}

export async function sendRequest_$POST(url: string, { arg }: any) {
    return fetch(url, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(arg),
    }).then(res => res.json())
}

export async function sendRequest_$DELETE(url: string, { arg }: any) {
    return fetch(url, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(arg),
    }).then(res => res.json())
}

export function notificationSuccess(description: string) {
    return notification.success({
        message: 'Thông báo',
        description: description,
        duration: 2,
        placement: 'topRight'
    })
}

export function notificationError(description: string) {
    return notification.error({
        message: 'Thông báo',
        description: description,
        duration: 2,
        placement: 'topRight'
    })
}

export function deepCopy(data: any) {
    return JSON.parse(JSON.stringify(data));
}