import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";
import LoginPage from "../../features/auth/pages/LoginPage";
import ProtectedRoute from "./ProtectedRoute";
import { AdminDashboard, CandidateDashboard, EmployeeDashboard, ManagerDashboard, RecruiterDashboard } from "../../features";
import RegisterPage from "../../features/auth/pages/RegisterPage";
import DashboardLayout from "../layouts/DashboardLayout";
import ProfilePage from "../../features/profile";
import NotificationsPage from "../../features/notifications/pages/NotificationsPage";
import HrDashboardPage from "../../features/hr/pages/HrDashboardPage";
import { CandidateApplicationsPage, CandidateInterviewsPage, CandidateJobDetailsPage, CandidateJobsPage } from "../../features/candidate";
import { EmployeesPage } from "../../features/hr";
import { ApplicationDetailsPage, ApplicationsPage } from "../../features/application";
import { CandidateDetailsPage, CandidatesPage } from "../../features/candidate-management";
import { JobDetailsPage, JobsPage } from "../../features/jobs";
import { ManagerInterviewsPage } from "../../features/interviews";
import { AdminUsersPage } from "../../features/admin";




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

          <Route path="/admin/users" element={<AdminUsersPage/>} />
          <Route path="/admin/profile" element={<ProfilePage/>} />
          <Route path="/admin/settings" element={<>Settings</>} />

        </Route>
      </Route>



      {/* ===================== Recruiter ===================== */}

      <Route element={<ProtectedRoute allowedRoles={["Recruiter"]} />}>
        <Route element={<DashboardLayout />}>

          <Route path="/recruiter" element={<RecruiterDashboard />} />

          <Route path="/recruiter/jobs" element={<JobsPage/>} />
          <Route path="/recruiter/jobs/:id" element={<JobDetailsPage/>} />
          <Route path="/recruiter/candidates" element={<CandidatesPage/>} />
          <Route path="/recruiter/candidates/:id" element={<CandidateDetailsPage/>}/>
          <Route path="/recruiter/applications" element={<ApplicationsPage/>} />
          <Route path="/recruiter/applications/:id" element={<ApplicationDetailsPage/>} />
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
          <Route path="/candidate/interviews" element={<CandidateInterviewsPage />} />
          <Route path="/candidate/profile" element={<ProfilePage/>} />
          <Route path="/candidate/settings" element={<>Settings</>} />

        </Route>
      </Route>



      {/* ===================== Employee ===================== */}

      <Route element={<ProtectedRoute allowedRoles={["Employee"]} />}>
        <Route element={<DashboardLayout />}>

          <Route path="/employee" element={<EmployeeDashboard />} />

          <Route path="/employee/profile" element={<ProfilePage/>} />
          <Route path="/employee/settings" element={<>Settings</>} />

        </Route>
      </Route>



      {/* ===================== HR ===================== */}

      <Route element={<ProtectedRoute allowedRoles={["HR"]} />}>
        <Route element={<DashboardLayout />}>

          <Route path="/hr" element={<HrDashboardPage />} />

          <Route path="/hr/employees" element={<EmployeesPage/>} />
          <Route path="/hr/candidates" element={<CandidatesPage/>} />
          <Route path="/hr/candidates/:id" element={<CandidateDetailsPage/>}/>

          <Route path="/hr/jobs" element={<JobsPage/>} />
          <Route path="/hr/jobs/:id" element={<JobDetailsPage/>} />
          <Route path="/hr/applications" element={<ApplicationsPage/>} />
          <Route path="/hr/applications/:id" element={<ApplicationDetailsPage/>} />
          {/* <Route path="/hr/reports" element={<>Reports</>} /> */}
          <Route path="/hr/profile" element={<ProfilePage/>} />
          <Route path="/hr/settings" element={<>Settings</>} />

        </Route>
      </Route>



      {/* ===================== Manager ===================== */}

      <Route element={<ProtectedRoute allowedRoles={["Manager"]} />}>
        <Route element={<DashboardLayout />}>

          <Route path="/manager" element={<ManagerDashboard />} />

          <Route path="/manager/jobs" element={<JobsPage/>} />
          <Route path="/manager/jobs/:id" element={<JobDetailsPage/>} />
          <Route path="/manager/candidates" element={<CandidatesPage/>} />
          <Route path="/manager/candidates/:id" element={<CandidateDetailsPage/>}/>
          <Route path="/manager/interviews" element={<ManagerInterviewsPage/>} />
          <Route path="/manager/profile" element={<ProfilePage/>} />
          <Route path="/manager/settings" element={<>Settings</>} />

        </Route>
      </Route>

    </Routes>
  </BrowserRouter>
);

export default AppRouter;