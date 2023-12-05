import React from "react";
import { menuItemModel } from "../../../Interfaces";
import { useEffect } from "react";
import MenuItemCard from "./MenuItemCard";
import { useGetMenuItemsQuery } from "../../../apis/menuItemApi";
import { useDispatch } from "react-redux";
import { setMenuItem } from "../../../Storage/Redux/menuItemSlice";
function MenuItemList() {
  // const [menuItems, setMenuItems] = useState<menuItemModel[]>([]);
  const dispatch = useDispatch();
  const { data, isLoading } = useGetMenuItemsQuery(null);

  useEffect(() => {
    if (!isLoading) {
      dispatch(setMenuItem(data.result));
    }
  }, [isLoading]);
  if (isLoading) {
    return <p>Loading...</p>;
  }

  return (
    <div className="container row">
      {data.result.length > 0 &&
        data.result.map((menuItem: menuItemModel, index: number) => (
          <MenuItemCard menuItem={menuItem} key={index} />
        ))}
    </div>
  );
}

export default MenuItemList;
