import { Camera, User } from "lucide-react";
import type { Profile } from "../types/profile";

interface Props {
  profile: Profile;
}

export default function ProfilePicture({ profile }: Props) {
  return (
    <div className="flex flex-col items-center rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm">
      <div className="group relative">
        <div className="flex h-32 w-32 items-center justify-center overflow-hidden rounded-full border-4 border-[#EEF3F0] bg-[#F8FAF9] text-[#315343] shadow-md">
          {profile.profilePictureUrl ? (
            <img
              src={profile.profilePictureUrl}
              alt={profile.name}
              className="h-full w-full object-cover"
            />
          ) : (
            <User size={48} />
          )}
        </div>
        
        {/* Hover Overlay for Upload */}
        <label className="absolute inset-0 flex cursor-pointer items-center justify-center rounded-full bg-black/40 opacity-0 transition-opacity group-hover:opacity-100">
          <Camera size={24} className="text-white" />
          <input type="file" className="hidden" accept="image/*" />
        </label>
      </div>

      <div className="mt-4 text-center">
        <h2 className="text-xl font-bold tracking-tight text-[#212529]">
          {profile.name}
        </h2>
        
        {/* Prominent Role Badge */}
        <div className="mt-2 flex items-center justify-center gap-2">
          {/* <span className="inline-flex items-center rounded-md bg-[#315343] px-2.5 py-1 text-xs font-semibold uppercase tracking-wider text-white">
            {profile.role}
          </span> */}
          <span className="inline-flex items-center rounded-md bg-[#C3F53C]/20 px-2.5 py-1 text-xs font-semibold text-[#315343]">
            {profile.status}
          </span>
        </div>
      </div>
    </div>
  );
}