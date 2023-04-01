
export const typeUserReducers = (state = '', action: any) => {

    switch (action.type) {
        case "addTypeUser":
            return action.payload;
        case "deleteTypeUser":
            return action.payload;
        default:
            return state
    }
};

export default typeUserReducers;