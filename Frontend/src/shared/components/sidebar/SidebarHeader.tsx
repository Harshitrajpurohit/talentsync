import { BriefcaseBusiness } from "lucide-react";

interface Props {
  compact?: boolean;
}

export default function SidebarHeader({ compact }: Props) {
  if (compact) {
    return (
      <div className="flex items-center gap-2">
        <div className="flex h-6 w-6 items-center justify-center rounded-full bg-[#C3F53C] text-[#315343]">
          <BriefcaseBusiness size={14} strokeWidth={2.5} />
        </div>
        <span className="text-xl font-bold tracking-tight text-[#212529]">
          SupplySync
        </span>
      </div>
    );
  }

  return (
    <div className="flex items-center gap-3">
      <div className="flex h-8 w-8 items-center justify-center rounded-full bg-[#C3F53C] text-[#315343] shadow-sm">
        <BriefcaseBusiness size={18} strokeWidth={2.5} />
      </div>
      <div>
        <h1 className="text-2xl font-bold tracking-tight text-[#212529]">
          SupplySync
        </h1>
      </div>
    </div>
  );
}