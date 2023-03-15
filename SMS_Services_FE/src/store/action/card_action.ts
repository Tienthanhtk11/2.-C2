export const add_card = (data: any) => {
    return {
        type: "add",
        payload: data,
    };
};
export const delete_card = (data: any) => {
    return {
        type: "delete",
        payload: data,
    };
};
export const clear_card = () => {
    return {
        type: "",        
    };
};