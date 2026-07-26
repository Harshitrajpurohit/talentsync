import { Outlet } from "react-router-dom";
import Sidebar from "../../shared/components/sidebar";
import Topbar from "../../shared/components/topbar";

export default function DashboardLayout() {
  return (
    // FIXED: Added flex-col for mobile, lg:flex-row for desktop
    <div className="flex flex-col lg:flex-row h-screen w-full font-sans text-[#212529] bg-white">
      <Sidebar />

      <div className="flex flex-1 flex-col min-w-0 overflow-hidden">
        <Topbar />

        <main className="flex-1 overflow-auto bg-[#EEF3F0] p-4 lg:p-8">
          <div className="mx-auto max-w-7xl">
            <Outlet />
          </div>
        </main>
      </div>
    </div>
  );
}