import { Search } from "lucide-react";

export default function SearchBox() {
  return (
    <div className="relative">
      <Search
        size={18}
        className="absolute left-4 top-1/2 -translate-y-1/2 text-[#75837D]"
      />

      <input
        type="text"
        placeholder="Search here.."
        className="w-full rounded-full border border-transparent bg-white py-2.5 pl-11 pr-4 text-sm text-[#212529] outline-none shadow-sm transition placeholder:text-[#75837D] focus:border-[#315343] focus:ring-1 focus:ring-[#315343]"
      />
    </div>
  );
}