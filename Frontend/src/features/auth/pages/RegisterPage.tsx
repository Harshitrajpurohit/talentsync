import { BriefcaseBusiness, Search, FileCheck, Bell } from "lucide-react";
import RegisterForm from "../components/RegisterForm";

export default function RegisterPage() {
  return (
    <div className="relative flex min-h-screen w-full items-center justify-center bg-slate-950 px-3 py-6 sm:px-6 lg:h-screen lg:overflow-hidden">
      {/* Background Glow */}
      <div className="pointer-events-none absolute left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2 h-[450px] w-[450px] rounded-full bg-emerald-500/10 blur-[120px]" />

      {/* Main Container */}
      <div className="relative z-10 flex w-full max-w-4xl flex-col overflow-hidden rounded-2xl border border-slate-800/80 bg-slate-900/70 shadow-2xl backdrop-blur-xl lg:flex-row">
        
        {/* Left Side: Brand & Candidate Value Panel */}
        <div className="relative hidden flex-col justify-between border-b border-slate-800 p-6 sm:flex sm:p-8 lg:w-5/12 lg:border-b-0 lg:border-r">
          <div>
            {/* Header */}
            <div className="flex items-center gap-3">
              <div className="flex h-10 w-10 items-center justify-center rounded-xl border border-emerald-500/20 bg-emerald-500/10 text-emerald-400">
                <BriefcaseBusiness className="h-5 w-5" />
              </div>
              <span className="text-xl font-bold tracking-tight text-white">
                TalentSync
              </span>
            </div>

            <div className="mt-8">
              <h1 className="text-2xl font-bold tracking-tight text-white">
                Find Your Next Career Opportunity
              </h1>
              <p className="mt-2 text-sm leading-relaxed text-slate-400">
                Explore open roles, upload your resume, and connect directly with hiring teams on a modern career platform.
              </p>
            </div>
          </div>

          {/* Candidate Feature Highlights */}
          <div className="space-y-3.5 pt-6">
            <div className="flex items-center gap-3">
              <div className="flex h-6 w-6 shrink-0 items-center justify-center rounded-full bg-emerald-500/10 text-emerald-400">
                <Search className="h-3.5 w-3.5" />
              </div>
              <p className="text-sm font-medium text-slate-300">Browse open job opportunities</p>
            </div>
            <div className="flex items-center gap-3">
              <div className="flex h-6 w-6 shrink-0 items-center justify-center rounded-full bg-emerald-500/10 text-emerald-400">
                <FileCheck className="h-3.5 w-3.5" />
              </div>
              <p className="text-sm font-medium text-slate-300">Track application status live</p>
            </div>
            <div className="flex items-center gap-3">
              <div className="flex h-6 w-6 shrink-0 items-center justify-center rounded-full bg-emerald-500/10 text-emerald-400">
                <Bell className="h-3.5 w-3.5" />
              </div>
              <p className="text-sm font-medium text-slate-300">Instant interview & screening updates</p>
            </div>
          </div>

          <div className="mt-8 text-xs text-slate-600">
            © {new Date().getFullYear()} TalentSync Platform.
          </div>
        </div>

        {/* Right Side: Form Container */}
        <div className="flex flex-1 flex-col justify-center p-5 sm:p-8">
          <RegisterForm />
        </div>

      </div>
    </div>
  );
}