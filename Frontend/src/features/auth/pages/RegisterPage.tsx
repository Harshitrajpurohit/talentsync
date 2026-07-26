import { BriefcaseBusiness, FileText, Bell, Search } from "lucide-react";
import RegisterForm from "../components/RegisterForm";

export default function RegisterPage() {
  return (
    <div className="relative flex min-h-screen w-full items-center justify-center bg-[#EEF3F0] px-4 py-6 font-sans sm:px-6 lg:h-screen lg:overflow-hidden">
      
      {/* Main Container */}
      <div className="relative z-10 flex w-full max-w-5xl flex-col overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white shadow-xl lg:flex-row">
        
        {/* Left Side: Brand & Value Panel */}
        <div className="relative hidden flex-col justify-between bg-[#315343] p-8 lg:w-5/12 lg:flex lg:p-12">
          <div>
            {/* Header */}
            <div className="flex items-center gap-3">
              <div className="flex h-10 w-10 items-center justify-center rounded-full bg-[#C3F53C] text-[#315343] shadow-sm">
                <BriefcaseBusiness className="h-5 w-5" strokeWidth={2.5} />
              </div>
              <span className="text-2xl font-bold tracking-tight text-white">
                TalentSync
              </span>
            </div>

            <div className="mt-12">
              <h1 className="text-3xl font-bold tracking-tight text-white">
                Find Your Next Career Opportunity
              </h1>
              <p className="mt-4 text-sm leading-relaxed text-[#E5EAE7]/80">
                Join the platform to explore open roles, upload your resume, and connect directly with hiring team.
              </p>
            </div>
          </div>

          {/* Feature Highlights */}
          <div className="space-y-5 pt-12">
            <div className="flex items-center gap-4">
              <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-full bg-white/10 text-[#C3F53C]">
                <Search className="h-4 w-4" />
              </div>
              <p className="text-sm font-medium text-white">Browse open job opportunities</p>
            </div>
            <div className="flex items-center gap-4">
              <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-full bg-white/10 text-[#C3F53C]">
                <FileText className="h-4 w-4" />
              </div>
              <p className="text-sm font-medium text-white">Cloud-based resume uploads</p>
            </div>
            <div className="flex items-center gap-4">
              <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-full bg-white/10 text-[#C3F53C]">
                <Bell className="h-4 w-4" />
              </div>
              <p className="text-sm font-medium text-white">Real-time application status alerts</p>
            </div>
          </div>

          <div className="mt-12 text-xs font-medium text-[#E5EAE7]/50">
            © {new Date().getFullYear()} TalentSync Platform.
          </div>
        </div>

        {/* Right Side: Form Container */}
        <div className="flex flex-1 flex-col justify-center bg-white p-6 sm:p-12">
          <RegisterForm />
        </div>

      </div>
    </div>
  );
}