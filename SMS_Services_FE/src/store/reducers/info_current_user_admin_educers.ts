export const infoCurrentUserAminReducers = (state = {}, action: any) => {
    switch (action.type) {
        case "login":
            return action.payload;
        default:
            return state;
    }
};

export default infoCurrentUserAminReducers;