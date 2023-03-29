import { combineReducers } from "redux";
import cardReducers from "./card_reducers";
import infoCurrentUserReducers from "./info_current_userr_educers";
import infoCurrentUserAminReducers from "./info_current_user_admin_educers";

export const allReducers = combineReducers({
    infoCurrentUserReducers,
    cardReducers,
    infoCurrentUserAminReducers,
});
