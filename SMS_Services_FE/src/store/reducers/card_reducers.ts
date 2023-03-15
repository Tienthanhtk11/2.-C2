export const cardReducers = (state = [], action: any) => {
    switch (action.type) {
        case "add":
            return [...state, action.payload];
        case "delete":          
            return [...state, action.payload];
        default:
            return [];
    }
};

export default cardReducers;