import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";

const menuItemApi = createApi({
  reducerPath: "menuItemApi",
  baseQuery: fetchBaseQuery({
    baseUrl: "https://localhost:7054/api/",
  }),
  tagTypes: ["MenuItem"],
  endpoints: (builder) => ({
    getMenuItems: builder.query({
      query: () => ({
        url: "menuitem",
      }),
      providesTags: ["MenuItem"],
    }),
    getMenuItemById: builder.query({
      query: (id) => ({
        url: `menuitem/${id}`,
      }),
      providesTags: ["MenuItem"],
    }),
  }),
});
export const { useGetMenuItemsQuery, useGetMenuItemByIdQuery } = menuItemApi;
export default menuItemApi;
