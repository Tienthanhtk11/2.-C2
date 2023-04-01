export const add_info_current_admin_user = (data: any) => {
    return {
        type: "loginadmin",
        payload: data,
    };
};
export const clear_info_current_admin_user = () => {
    return {
        type: "logoutadmin",  
        payload: {},      
    };
};