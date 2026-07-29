import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";
import LoginPage from "../../features/auth/pages/LoginPage";
import ProtectedRoute from "./ProtectedRoute";
import { AdminDashboard, CandidateDashboard, EmployeeDashboard, HrDashboard, ManagerDashboard, RecruiterDashboard } from "../../features";
import RegisterPage from "../../features/auth/pages/RegisterPage";
import DashboardLayout from "../layouts/DashboardLayout";
import JobsPage from "../../features/jobs/pages/JobsPage";
import ProfilePage from "../../features/profile";
import CandidateApplicationsPage from "../../features/candidate/pages/CandidateApplicationsPage";
import CandidateJobsPage from "../../features/candidate/pages/CandidateJobsPage";
import CandidateJobDetailsPage from "../../features/candidate/pages/CandidateJobDetailsPage";
import NotificationsPage from "../../features/notifications/pages/NotificationsPage";



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

      {/* ===================== Public ===================== */}

      <Route path="/" element={<RoleRedirect />} />
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />
      <Route path="/unauthorized" element={<h1>403 - Unauthorized</h1>} />
      <Route path="*" element={<h1>404 - Page Not Found</h1>} />
      <Route element={
          <ProtectedRoute
            allowedRoles={[
              "Admin",
              "Recruiter",
              "Candidate",
              "Employee",
              "HR",
              "Manager",
            ]}
          />
        }>
        <Route element={<DashboardLayout />}>
          <Route
            path="/notifications"
            element={<NotificationsPage />}
          />
        </Route>
      </Route>


      {/* ===================== Admin ===================== */}

      <Route element={<ProtectedRoute allowedRoles={["Admin"]} />}>
        <Route element={<DashboardLayout />}>

          <Route path="/admin" element={<AdminDashboard />} />

          <Route path="/admin/users" element={<>Users</>} />
          <Route path="/admin/roles" element={<>Roles</>} />
          <Route path="/admin/jobs" element={<>Jobs</>} />
          <Route path="/admin/applications" element={<>Applications</>} />
          <Route path="/admin/interviews" element={<>Interviews</>} />
          <Route path="/admin/reports" element={<>Reports</>} />
          <Route path="/admin/profile" element={<>Profile</>} />
          <Route path="/admin/settings" element={<>Settings</>} />

        </Route>
      </Route>



      {/* ===================== Recruiter ===================== */}

      <Route element={<ProtectedRoute allowedRoles={["Recruiter"]} />}>
        <Route element={<DashboardLayout />}>

          <Route path="/recruiter" element={<RecruiterDashboard />} />

          <Route path="/recruiter/jobs" element={<>Jobs</>} />
          <Route path="/recruiter/candidates" element={<>Candidates</>} />
          <Route path="/recruiter/applications" element={<>Applications</>} />
          <Route path="/recruiter/interviews" element={<>Interviews</>} />
          <Route path="/recruiter/profile" element={<ProfilePage/>} />
          <Route path="/recruiter/settings" element={<>Settings</>} />

        </Route>
      </Route>



      {/* ===================== Candidate ===================== */}

      <Route element={<ProtectedRoute allowedRoles={["Candidate"]} />}>
        <Route element={<DashboardLayout />}>

          <Route path="/candidate" element={<CandidateDashboard />} />

          <Route path="/candidate/jobs" element={<CandidateJobsPage />} />
          <Route path="/candidate/jobs/:id" element={<CandidateJobDetailsPage />}/>
          <Route path="/candidate/applications" element={<CandidateApplicationsPage/>} />
          <Route path="/candidate/interviews" element={<>Interviews</>} />
          <Route path="/candidate/profile" element={<ProfilePage/>} />
          <Route path="/candidate/settings" element={<>Settings</>} />

        </Route>
      </Route>



      {/* ===================== Employee ===================== */}

      <Route element={<ProtectedRoute allowedRoles={["Employee"]} />}>
        <Route element={<DashboardLayout />}>

          <Route path="/employee" element={<EmployeeDashboard />} />

          <Route path="/employee/profile" element={<>Profile</>} />
          <Route path="/employee/settings" element={<>Settings</>} />

        </Route>
      </Route>



      {/* ===================== HR ===================== */}

      <Route element={<ProtectedRoute allowedRoles={["HR"]} />}>
        <Route element={<DashboardLayout />}>

          <Route path="/hr" element={<HrDashboard />} />

          <Route path="/hr/employees" element={<>Employees</>} />
          <Route path="/hr/candidates" element={<>Candidates</>} />
          <Route path="/hr/jobs" element={<>Jobs</>} />
          <Route path="/hr/applications" element={<>Applications</>} />
          <Route path="/hr/reports" element={<>Reports</>} />
          <Route path="/hr/profile" element={<ProfilePage/>} />
          <Route path="/hr/settings" element={<>Settings</>} />

        </Route>
      </Route>



      {/* ===================== Manager ===================== */}

      <Route element={<ProtectedRoute allowedRoles={["Manager"]} />}>
        <Route element={<DashboardLayout />}>

          <Route path="/manager" element={<ManagerDashboard />} />

          <Route path="/manager/jobs" element={<>Jobs</>} />
          <Route path="/manager/candidates" element={<>Candidates</>} />
          <Route path="/manager/applications" element={<>Applications</>} />
          <Route path="/manager/interviews" element={<>Interviews</>} />
          <Route path="/manager/reports" element={<>Reports</>} />
          <Route path="/manager/profile" element={<>Profile</>} />
          <Route path="/manager/settings" element={<>Settings</>} />

        </Route>
      </Route>

    </Routes>
  </BrowserRouter>
);

export default AppRouter;