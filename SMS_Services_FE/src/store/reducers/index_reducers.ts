import { combineReducers } from "redux";
import infoCurrentUserReducers from "./info_current_userr_educers";
import infoCurrentUserAminReducers from "./info_current_user_admin_educers";
import typeUserReducers from "./type_user_reducers";

export const allReducers = combineReducers({
    infoCurrentUserReducers,
    infoCurrentUserAminReducers,
    typeUserReducers,
});
