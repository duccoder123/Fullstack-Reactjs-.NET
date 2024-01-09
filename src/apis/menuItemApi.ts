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
    createMenuItem: builder.mutation({
      query: (data) => ({
        url: "menuitem",
        method: "POST",
        body: data,
      }),
      invalidatesTags: ["MenuItem"],
    }),
    updateMenuItem: builder.mutation({
      query: ({ data, id }) => ({
        url: "menuitem/" + id,
        method: "PUT",
        body: data,
      }),
      invalidatesTags: ["MenuItem"],
    }),
    deleteMenuItem: builder.mutation({
      query: (id) => ({
        url: "menuitem/" + id,
        method: "DELETE",
      }),
      invalidatesTags: ["MenuItem"],
    }),
  }),
});
export const {
  useGetMenuItemsQuery,
  useGetMenuItemByIdQuery,
  useCreateMenuItemMutation,
  useUpdateMenuItemMutation,
  useDeleteMenuItemMutation,
} = menuItemApi;
export default menuItemApi;
