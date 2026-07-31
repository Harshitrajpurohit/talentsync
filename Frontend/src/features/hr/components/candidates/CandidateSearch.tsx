import { Search } from "lucide-react";

interface CandidateSearchProps {
  value: string;
  onChange: (value: string) => void;
}

export default function CandidateSearch({
  value,
  onChange,
}: CandidateSearchProps) {
  return (
    <div className="relative w-full md:w-80 lg:w-96">
      <Search
        size={18}
        className="absolute left-4 top-1/2 -translate-y-1/2 text-gray-400 dark:text-white/50"
      />
      <input
        type="text"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        placeholder="Search candidates..."
        className="w-full rounded-full border border-gray-200 bg-white py-2.5 pl-11 pr-4 text-sm font-medium text-gray-900 placeholder-gray-400 outline-none transition-all duration-300 focus:border-[#315343] focus:ring-2 focus:ring-[#315343]/10 dark:border-[#315343] dark:bg-[#1e3329] dark:text-white dark:placeholder-white/40"
      />
    </div>
  );
}