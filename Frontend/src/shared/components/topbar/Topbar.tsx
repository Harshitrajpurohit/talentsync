import SearchBox from "./SearchBox";
import NotificationButton from "./NotificationButton";
import ProfileMenu from "./ProfileMenu";

export default function Topbar() {
  return (
    <header className="sticky top-0 z-30 flex h-20 items-center justify-between bg-[#EEF3F0] px-4 lg:px-8">
      {/* Center / Left */}
      <div className="w-full max-w-md hidden lg:block">
        <SearchBox />
      </div>

      {/* Right */}
      <div className="flex items-center gap-3 lg:gap-4 ml-auto lg:ml-0">
        <NotificationButton />
        <ProfileMenu />
      </div>
    </header>
  );
}