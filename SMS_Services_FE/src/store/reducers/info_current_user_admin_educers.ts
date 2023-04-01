export const infoCurrentUserAminReducers = (state = {}, action: any) => {
    switch (action.type) {
        case "loginadmin":
            return action.payload;
        case "logoutadmin":
            return action.payload;
        default:
            return state;
    }
};

export default infoCurrentUserAminReducers;