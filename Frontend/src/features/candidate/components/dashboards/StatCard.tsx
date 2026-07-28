import type { LucideIcon } from "lucide-react";

interface StatCardProps {
  title: string;
  value: number;
  icon: LucideIcon;
}

export function StatCard({ title, value, icon: Icon }: StatCardProps) {
  const formattedValue = value < 10 && value > 0 ? `0${value}` : value;

  return (
    <div className="group flex items-center justify-between rounded-[20px] border border-[#E5EAE7] bg-white p-6 shadow-sm transition-all duration-300 hover:-translate-y-1 hover:shadow-md dark:border-[#315343] dark:bg-[#253f33]">
      <div>
        <h3 className="text-3xl font-bold text-[#212529] dark:text-white">
          {formattedValue}
        </h3>
        <p className="mt-1.5 text-sm font-medium text-[#75837D] dark:text-[#C3F53C]/70">
          {title}
        </p>
      </div>

      <div className="flex h-12 w-12 shrink-0 items-center justify-center rounded-full bg-[#C3F53C] text-[#315343] transition-transform duration-300 group-hover:scale-110 dark:bg-[#C3F53C]/90">
        <Icon size={22} strokeWidth={2} />
      </div>
    </div>
  );
}