import React from "react";
import { withAdminAuth } from "../HOC/index";

function AuthenticationTestAdmin() {
  return (
    <div>This page can only be accessed if role of logged in user is Admin</div>
  );
}

export default withAdminAuth(AuthenticationTestAdmin);
