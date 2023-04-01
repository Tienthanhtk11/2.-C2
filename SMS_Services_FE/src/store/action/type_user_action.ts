export const typeUser_add = (data: any) => {
    return {
        type: "addTypeUser",
        payload: data,
    };
};
export const typeUser_delete = () => {
    return {
        type: "deleteTypeUser",
        payload: '',
    };
};