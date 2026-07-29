import SearchBox from "./SearchBox";
import ProfileMenu from "./ProfileMenu";
import NotificationBell from "../../../features/notifications/components/NotificationBell";

export default function Topbar() {
  return (
    <header className="sticky top-0 z-30 flex h-20 items-center justify-between bg-[#EEF3F0] px-4 lg:px-8">
      <div className="hidden w-full max-w-md lg:block">
        <SearchBox />
      </div>

      <div className="ml-auto flex items-center gap-3 lg:ml-0 lg:gap-4">
        <NotificationBell />
        <ProfileMenu />
      </div>
    </header>
  );
}