import {
  Mail,
  Phone,
  MapPin,
  ShieldCheck,
  CheckCircle2,
} from "lucide-react";
import type { Profile } from "../types/profile";
import type { UserRole } from "../../../shared/components/sidebar/types";
import type { Resume } from "../types/resume";

interface Props {
  profile: Profile;
  resume: Resume | null;
  role: UserRole | undefined;
}

export default function ProfileHeader({ profile, resume,  role }: Props) {
  const fields = [
    profile.name,
    profile.email,
    profile.phone,
    profile.profilePictureUrl,
    profile.dateOfBirth,
    profile.gender,
    profile.address,
    profile.about,
    profile.linkedinUrl,
    profile.githubUrl,
    profile.portfolioUrl,
    ...(role === "Candidate" ? [resume] : []),
  ];

  const completed = fields.filter(
    (x) => x !== undefined && x !== null && x !== ""
  ).length;

  const total = fields.length;
  const percentage = Math.round((completed / total) * 100);

  // SVG Circular Chart Calculations
  const radius = 28;
  const circumference = 2 * Math.PI * radius;
  const strokeDashoffset = circumference - (percentage / 100) * circumference;


  return (
    <div className="relative overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white shadow-sm">
      {/* Top Accent Bar */}
      <div className="absolute left-0 top-0 h-2 w-full bg-[#315343]">
        <div
          className="h-full bg-[#C3F53C] transition-all duration-700 ease-out"
          style={{ width: `${percentage}%` }}
        />
      </div>

      <div className="p-6 sm:p-8">
        <div className="flex flex-col gap-6 lg:flex-row lg:items-start lg:justify-between">
          {/* Left - User Info */}
          <div>
            <h1 className="text-2xl font-bold text-[#212529]">
              Welcome back, {profile.name.split(" ")[0]}!
            </h1>

            <p className="mt-1 text-sm text-[#75837D]">
              Manage your personal information and account settings.
            </p>

            <div className="mt-6 flex flex-wrap items-center gap-x-6 gap-y-3">
              <div className="flex items-center gap-2 text-sm font-semibold text-[#315343]">
                <ShieldCheck size={18} className="text-[#C3F53C]" />
                <span className="capitalize">{role ?? "Unknown"}</span>
              </div>

              <div className="flex items-center gap-2 text-sm text-[#212529]">
                <Mail size={16} className="text-[#75837D]" />
                {profile.email}
              </div>

              {profile.phone && (
                <div className="flex items-center gap-2 text-sm text-[#212529]">
                  <Phone size={16} className="text-[#75837D]" />
                  {profile.phone}
                </div>
              )}

              {profile.address && (
                <div className="flex items-center gap-2 text-sm text-[#212529]">
                  <MapPin size={16} className="text-[#75837D]" />
                  {profile.address}
                </div>
              )}
            </div>
          </div>

          {/* Right - Profile Completion (Updated with Donut Chart) */}
          <div className="w-full shrink-0 rounded-xl border border-[#E5EAE7] bg-[#F8FAF9] p-5 lg:w-[22rem]">
            <div className="flex items-center gap-5">
              
              {/* Circular Progress SVG */}
              <div className="relative flex shrink-0 items-center justify-center h-16 w-16">
                <svg className="h-16 w-16 -rotate-90 transform">
                  {/* Background Track */}
                  <circle
                    cx="32"
                    cy="32"
                    r={radius}
                    stroke="#E5EAE7"
                    strokeWidth="6"
                    fill="transparent"
                  />
                  {/* Progress Arc */}
                  <circle
                    cx="32"
                    cy="32"
                    r={radius}
                    stroke="#315343"
                    strokeWidth="6"
                    fill="transparent"
                    strokeDasharray={circumference}
                    strokeDashoffset={strokeDashoffset}
                    strokeLinecap="round"
                    className="transition-all duration-1000 ease-out"
                  />
                </svg>
                {/* Percentage Text Centered */}
                <span className="absolute text-sm font-bold text-[#315343]">
                  {percentage}%
                </span>
              </div>

              {/* Completion Details */}
              <div className="flex flex-col">
                <span className="text-base font-semibold text-[#212529]">
                  Profile Completion
                </span>
                
                <p className="mt-0.5 text-sm text-[#75837D]">
                  {completed} of {total} sections filled
                </p>

                {/* Conditional Success Message Restored cleanly */}
                {percentage === 100 ? (
                  <div className="mt-2 flex items-center gap-1.5 text-xs font-semibold text-[#315343]">
                    <CheckCircle2 size={14} className="fill-[#C3F53C]" />
                    All set! Profile complete.
                  </div>
                ) : (
                  <p className="mt-2 text-xs font-medium text-[#75837D]">
                    Add missing details to boost your visibility.
                  </p>
                )}
              </div>
              
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}