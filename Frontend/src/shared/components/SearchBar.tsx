import { Search } from "lucide-react";

interface SearchProps {
  value: string;
  onChange: (value: string) => void;
}

export default function SearchBar({
  value,
  onChange,
}: SearchProps) {
  return (
    <div className="group relative w-full md:max-w-sm">
      <Search
        size={18}
        className="absolute left-3 top-1/2 -translate-y-1/2 text-[#75837D] transition-colors duration-300 group-focus-within:text-[#C3F53C] group-hover:text-[#315343]"
      />

      <input
        type="text"
        placeholder="Search..."
        value={value}
        onChange={(e) => onChange(e.target.value)}
        className="w-full rounded-[20px] border border-[#E5EAE7] bg-white py-2.5 pl-10 pr-4 text-sm text-[#212529] outline-none transition-all duration-300 placeholder:text-[#75837D] hover:border-[#315343] focus:border-[#315343] focus:ring-2 focus:ring-[#C3F53C]/50 dark:border-gray-700 dark:bg-gray-900 dark:text-white dark:focus:ring-[#315343]"
      />
    </div>
  );
}