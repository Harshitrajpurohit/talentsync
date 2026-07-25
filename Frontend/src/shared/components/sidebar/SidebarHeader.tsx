import { BriefcaseBusiness } from "lucide-react";

interface Props {
  compact?: boolean;
}

export default function SidebarHeader({ compact }: Props) {
  if (compact) {
    return (
      <div className="flex items-center gap-2.5">
        <div className="flex h-8 w-8 items-center justify-center rounded-lg border border-emerald-500/20 bg-emerald-500/10 text-emerald-400">
          <BriefcaseBusiness className="h-4 w-4" />
        </div>
        <span className="text-lg font-bold tracking-tight text-white">
          TalentSync
        </span>
      </div>
    );
  }

  return (
    <div className="border-b border-slate-800/80 p-5">
      <div className="flex items-center gap-3">
        <div className="flex h-9 w-9 items-center justify-center rounded-xl border border-emerald-500/20 bg-emerald-500/10 text-emerald-400 shadow-sm">
          <BriefcaseBusiness className="h-5 w-5" />
        </div>
        <div>
          <h1 className="text-lg font-bold tracking-tight text-white">
            TalentSync
          </h1>
          <p className="text-xs font-medium text-slate-400">
            Recruitment Portal
          </p>
        </div>
      </div>
    </div>
  );
}