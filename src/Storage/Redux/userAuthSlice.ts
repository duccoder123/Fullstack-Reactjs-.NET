import { createSlice } from "@reduxjs/toolkit";
import { shoppingCartModel } from "../../Interfaces";
import userModel from "../../Interfaces/userModel";

export const emptyUserState: userModel = {
  fullName: "",
  id: "",
  email: "",
  role: "",
};

export const userAuthSlice = createSlice({
  name: "userAuth",
  initialState: emptyUserState,
  reducers: {
    setLoggerInUser: (state, action) => {
      state.fullName = action.payload.fullName;
      state.id = action.payload.id;
      state.email = action.payload.email;
      state.role = action.payload.role;
    },
  },
});

export const { setLoggerInUser } = userAuthSlice.actions;
export const userAuthReducer = userAuthSlice.reducer;