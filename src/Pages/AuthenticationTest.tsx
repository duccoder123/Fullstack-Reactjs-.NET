import React from "react";
import { withAuth } from "../HOC/index";

function AuthenticationTest() {
  return (
    <div>
      <p>This page can be accessed by any logged in user</p>
    </div>
  );
}

export default withAuth(AuthenticationTest);
