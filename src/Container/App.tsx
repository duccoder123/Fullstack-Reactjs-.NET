import React from "react";
import { Header, Footer } from "../Components/Layout/index";
import {
  Home,
  NotFound,
  MenuItemDetail,
  ShoppingCart,
  Login,
  Register,
  Payment,
  OrderConfirm,
  MyOrders,
  OrderDetails,
  AllOrders,
  MenuItemList,
  MenuItemUpsert,
} from "../Pages";
import { Routes, Route } from "react-router-dom";
import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useGetShoppingCartQuery } from "../apis/shoppingCartApi";
import { setShoppingCart } from "../Storage/Redux/shoppingCartSlice";
import { userModel } from "../Interfaces";
import jwt_decode from "jwt-decode";
import { setLoggerInUser } from "../Storage/Redux/userAuthSlice";
import { RootState } from "../Storage/Redux/store";
function App() {
  const userData: userModel = useSelector(
    (state: RootState) => state.userAuthStore
  );
  const dispatch = useDispatch();
  const { data, isLoading } = useGetShoppingCartQuery(userData.id);
  useEffect(() => {
    const localToken = localStorage.getItem("token");
    if (localToken) {
      const { fullName, id, email, role }: userModel = jwt_decode(localToken);
      dispatch(setLoggerInUser({ fullName, id, email, role }));
    }
  }, []);
  useEffect(() => {
    if (!isLoading) {
      dispatch(setShoppingCart(data.result?.cartItems));
    }
  }, [data]);
  return (
    <>
      <Header />
      <div className="pb-5">
        <Routes>
          <Route path="/" element={<Home />}></Route>
          <Route
            path="/menuItemDetails/:menuItemId"
            element={<MenuItemDetail />}
          ></Route>
          <Route path="/shoppingCart" element={<ShoppingCart />}></Route>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />

          <Route path="/payment" element={<Payment />} />
          <Route path="order/orderConfirm/:id" element={<OrderConfirm />} />
          <Route path="order/myOrders" element={<MyOrders />} />
          <Route path="order/orderDetails/:id" element={<OrderDetails />} />
          <Route path="order/allOrders" element={<AllOrders />} />
          <Route path="/menuItem/menuitemlist" element={<MenuItemList />} />
          <Route
            path="/menuItem/menuItemUpsert/:id"
            element={<MenuItemUpsert />}
          />
          <Route path="/menuItem/menuItemUpsert" element={<MenuItemUpsert />} />
          <Route path="*" element={<NotFound />}></Route>
        </Routes>
      </div>
      <Footer />
    </>
  );
}

export default App;
