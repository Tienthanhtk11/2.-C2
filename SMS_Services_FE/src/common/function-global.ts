import { notification } from "antd";
import { persistor, store } from "@/common/configureStore";
import { useMemo } from "react";

export function mapSelectAntd(list: Array<any>, label: string, value: string) {
    return list.map((obj: any) => {
        return {
            value: obj[value],
            label: obj[label],
        }
    })
}

export async function sendRequestLogin_$POST(url: string, { arg }: any) {
    return fetch(url, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(arg),
    }).then(res => res.json())
}

export function getToken() {

    const state = store.getState();
    let token = "";
    if (state?.infoCurrentUserReducers?.token) {       
        token = state?.infoCurrentUserReducers?.token;
    }

    if (state?.infoCurrentUserAminReducers?.token) {
        token = state?.infoCurrentUserAminReducers?.token;
    }

    const headers = useMemo(
        () => ({
            headers: {
                // Authorization: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1NDNkMWU0Mi05NTdkLTQyOTItYjI4Yy0yOTMwYzViN2UyMTciLCJ1bmlxdWVfbmFtZSI6ImN1c3RvbWVyX3RoYW5oIiwibmFtZWlkIjoiY3VzdG9tZXJfdGhhbmgiLCJlbWFpbCI6ImN1c3RvbWVyX3RoYW5oIiwic2lkIjoiMSIsImV4cCI6MTY4MDQyOTQ4NSwiaXNzIjoibXlsb2NhbC5jb20iLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3MSJ9.tCPrAdCpUhh9yZUS_hQcY0nopGBSX2SVIbzyWT0GWTI`
                Authorization: `Bearer ${token}`
            },
        }),
        [token]
    );
    
    return headers;
}

export async function sendRequest_$GET(url: string, { arg }: any) {
    const state = store.getState();
    let token = "";

    if (state?.infoCurrentUserReducers) {
        token = await state?.infoCurrentUserReducers?.token;
    }
    if (state?.infoCurrentUserAminReducers) {
        token = await state?.infoCurrentUserAminReducers?.token;
    }
    return fetch(url, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`,
        },
    }).then(res => res.json())
}

export async function sendRequest_$POST(url: string, { arg }: any) {
    const state = store.getState();
    let token = "";

    if (state?.infoCurrentUserReducers) {
        token = await state?.infoCurrentUserReducers?.token;
    }
    if (state?.infoCurrentUserAminReducers) {
        token = await state?.infoCurrentUserAminReducers?.token;
    }
    return fetch(url, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`,
        },
        body: await JSON.stringify(arg),
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