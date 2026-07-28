import { UserCircle2, CalendarDays, Sparkles } from "lucide-react";

interface DashboardHeaderProps {
  candidateName: string | undefined;
}

export default function DashboardHeader({
  candidateName,
}: DashboardHeaderProps) {
  const date = new Date();
  const hour = date.getHours();

  const greeting =
    hour < 12
      ? "Good Morning"
      : hour < 17
        ? "Good Afternoon"
        : "Good Evening";

  const dateString = date.toLocaleDateString("en-US", {
    weekday: "long",
    month: "short",
    day: "numeric",
  });

  return (
    <div className="relative w-full overflow-hidden rounded-[20px] bg-[#315343] shadow-sm">

      <div className="absolute -right-8 -top-8 h-32 w-32 rounded-full bg-[#C3F53C]/15 blur-2xl"></div>
      <div className="absolute -bottom-10 -left-10 h-32 w-32 rounded-full bg-white/5 blur-2xl"></div>

      <div className="relative z-10 flex flex-col justify-between gap-4 px-5 py-5 sm:flex-row sm:items-center sm:px-6 sm:py-5">
        
        
        <div className="flex flex-col gap-2.5">

          <div className="flex w-fit items-center gap-1.5 rounded-full border border-white/10 bg-white/5 px-2.5 py-1 text-xs font-medium text-[#E5EAE7] backdrop-blur-sm">
            <CalendarDays size={13} className="text-[#C3F53C]" />
            <span>{dateString}</span>
          </div>


          <div className="relative">
            <h1 className="text-xl font-extrabold tracking-tight text-white sm:text-2xl">
              {greeting}, <span className="text-[#C3F53C]">{candidateName ?? "Unknown"}</span>
            </h1>
            <p className="mt-1 max-w-xl text-sm leading-relaxed text-[#E5EAE7]/80">
              Track your applications, prepare for upcoming interviews, and manage your career profile.
            </p>
          </div>
        </div>


        <div className="hidden shrink-0 items-center justify-center sm:flex">
          <div className="relative flex h-14 w-14 items-center justify-center rounded-full border-2 border-[#C3F53C]/30 bg-white/5 backdrop-blur-md transition-transform duration-500 hover:scale-105 md:h-16 md:w-16">
            <div className="absolute inset-0 rounded-full border border-white/10"></div>
            <UserCircle2 className="h-7 w-7 text-[#C3F53C] md:h-8 md:w-8" strokeWidth={1.2} />
            
            
            <div className="absolute -right-1 -top-1 flex h-6 w-6 items-center justify-center rounded-full bg-[#C3F53C] text-[#315343] shadow-md md:h-7 md:w-7">
              <Sparkles className="h-3 w-3 md:h-3.5 md:w-3.5" fill="currentColor" />
            </div>
          </div>
        </div>


        <div className="absolute right-4 top-4 sm:hidden">
          <UserCircle2 className="h-8 w-8 text-[#C3F53C]/30" strokeWidth={1} />
        </div>

      </div>
    </div>
  );
}