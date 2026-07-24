import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";
import LoginPage from "../../features/auth/pages/LoginPage";
import ProtectedRoute from "./ProtectedRoute";
import { AdminDashboard, CandidateDashboard, EmployeeDashboard, HrDashboard, ManagerDashboard, RecruiterDashboard } from "../../features";
import RegisterPage from "../../features/auth/pages/RegisterPage";
import DashboardLayout from "../layouts/DashboardLayout";



const RoleRedirect = () => {
  const { user } = useAuth();

  const routes: Record<string, string> = {
    Admin: "/admin",
    Recruiter: "/recruiter",
    Candidate: "/candidate",
    Employee: "/employee",
    HR: "/hr",
    Manager : "/manager",
  };

  return <Navigate to={routes[user?.role ?? ""] ?? "/login"} replace />;
};


const AppRouter = () => (
    <BrowserRouter>
        <Routes>

        {/* Public */}
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/" element={<RoleRedirect />} />

        <Route
          path="/unauthorized"
          element={<h1>403 - Unauthorized</h1>}
        />


        {/* Admin */}
        <Route element={<ProtectedRoute allowedRoles={["Admin"]} />}>
          <Route element={<DashboardLayout />}>
            <Route path="/admin" element={<AdminDashboard />} />

            {/* Future pages */}
            {/* <Route path="/admin/users" element={<UsersPage />} /> */}
            {/* <Route path="/admin/jobs" element={<JobsPage />} /> */}
          </Route>
        </Route>
          
          {/* Recruiter */}
        <Route element={<ProtectedRoute allowedRoles={["Recruiter"]} />}>
          <Route element={<DashboardLayout />}>
            <Route
              path="/recruiter"
              element={<RecruiterDashboard />}
            />
          </Route>
        </Route>

        {/* Candidate */}
        <Route element={<ProtectedRoute allowedRoles={["Candidate"]} />}>
          <Route element={<DashboardLayout />}>
            <Route
              path="/candidate"
              element={<CandidateDashboard />}
            />
          </Route>
        </Route>

        {/* Employee */}
        <Route element={<ProtectedRoute allowedRoles={["Employee"]} />}>
          <Route element={<DashboardLayout />}>
            <Route
              path="/employee"
              element={<EmployeeDashboard />}
            />
          </Route>
        </Route>

        {/* HR */}
        <Route element={<ProtectedRoute allowedRoles={["HR"]} />}>
          <Route element={<DashboardLayout />}>
            <Route
              path="/hr"
              element={<HrDashboard />}
            />
          </Route>
        </Route>

        {/* Manager */}
        <Route element={<ProtectedRoute allowedRoles={["Manager"]} />}>
          <Route element={<DashboardLayout />}>
            <Route
              path="/manager"
              element={<ManagerDashboard />}
            />
          </Route>
        </Route>

   
        </Routes>

    </BrowserRouter>
);

export default AppRouter;