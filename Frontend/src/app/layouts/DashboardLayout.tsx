
import Sidebar from "./Sidebar";
// import Topbar from "./Topbar";
// import Footer from "./Footer";

import { Outlet } from "react-router-dom";

export default function DashboardLayout() {
  return (
    <div className="flex min-h-screen">
      <Sidebar />

      <div className="flex flex-1 flex-col">
        {/* <Topbar /> */}

        <main className="flex-1 overflow-auto p-6">
          <Outlet />
        </main>

        {/* <Footer /> */}
      </div>
    </div>
  );
}
