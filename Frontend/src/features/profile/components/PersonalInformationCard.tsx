import { useState } from "react";
import { Edit2, Loader2 } from "lucide-react";
import { useUpdateProfile } from "../hooks/useUpdateProfile";
import type { Profile, UpdateProfileRequest } from "../types/profile";

interface Props {
  profile: Profile;
  onUpdate: () => void;
}

export default function PersonalInformationCard({ profile, onUpdate }: Props) {
  const [isEditing, setIsEditing] = useState(false);
  const { update, loading } = useUpdateProfile();

  const [formData, setFormData] = useState<UpdateProfileRequest>({
    name: profile.name,
    phone: profile.phone || "",
    dateOfBirth: profile.dateOfBirth || "",
    gender: profile.gender || "",
    address: profile.address || "",
    about: profile.about || "",
  });
  

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();

        const payload: UpdateProfileRequest = {
            ...formData,
            phone: formData.phone || undefined,
            gender: formData.gender || undefined,
            address: formData.address || undefined,
            about: formData.about || undefined,
            linkedinUrl: formData.linkedinUrl || undefined,
            githubUrl: formData.githubUrl || undefined,
            portfolioUrl: formData.portfolioUrl || undefined,
            dateOfBirth: formData.dateOfBirth || undefined,
        };

        await update(payload);

        setIsEditing(false);
        onUpdate();
    }

  return (
    <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm lg:p-8">
      <div className="mb-6 flex items-center justify-between">
        <h2 className="text-xl font-bold text-[#212529]">Personal Information</h2>
        {!isEditing && (
          <button
            onClick={() => setIsEditing(true)}
            className="flex items-center gap-2 rounded-lg bg-[#EEF3F0] px-3 py-1.5 text-sm font-semibold text-[#315343] transition hover:bg-[#E5EAE7]"
          >
            <Edit2 size={14} /> Edit
          </button>
        )}
      </div>

      <form onSubmit={handleSubmit} className="space-y-6">
        <div className="grid grid-cols-1 gap-6 sm:grid-cols-2">
          {/* Full Name */}
          <div>
            <label className="mb-1.5 block text-sm font-semibold text-[#212529]">Full Name</label>
            <input
              type="text"
              disabled={!isEditing}
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
              className="w-full rounded-[10px] border border-[#E5EAE7] bg-white py-2.5 px-3 text-sm text-[#212529] outline-none transition focus:border-[#315343] disabled:bg-[#F8FAF9] disabled:text-[#75837D]"
            />
          </div>

          {/* Email (Read Only per your types) */}
          <div>
            <label className="mb-1.5 block text-sm font-semibold text-[#212529]">Email Address</label>
            <input
              type="email"
              disabled
              value={profile.email}
              className="w-full rounded-[10px] border border-[#E5EAE7] bg-[#F8FAF9] py-2.5 px-3 text-sm text-[#75837D] outline-none"
            />
          </div>

          {/* Phone */}
          <div>
            <label className="mb-1.5 block text-sm font-semibold text-[#212529]">Phone Number</label>
            <input
              type="tel"
              disabled={!isEditing}
              value={formData.phone}
              onChange={(e) => setFormData({ ...formData, phone: e.target.value })}
              className="w-full rounded-[10px] border border-[#E5EAE7] bg-white py-2.5 px-3 text-sm text-[#212529] outline-none transition focus:border-[#315343] disabled:bg-[#F8FAF9] disabled:text-[#75837D]"
            />
          </div>

          {/* Gender */}
          <div>
            <label className="mb-1.5 block text-sm font-semibold text-[#212529]">Gender</label>
            <select
              disabled={!isEditing}
              value={formData.gender}
              onChange={(e) => setFormData({ ...formData, gender: e.target.value })}
              className="w-full rounded-[10px] border border-[#E5EAE7] bg-white py-2.5 px-3 text-sm text-[#212529] outline-none transition focus:border-[#315343] disabled:bg-[#F8FAF9] disabled:text-[#75837D]"
            >
              <option value="">Select Gender</option>
              <option value="Male">Male</option>
              <option value="Female">Female</option>
              <option value="Other">Other</option>
            </select>
          </div>

          {/* Date of Birth */}
          <div>
            <label className="mb-1.5 block text-sm font-semibold text-[#212529]">Date of Birth</label>
            <input
              type="date"
              disabled={!isEditing}
              value={formData.dateOfBirth?.split("T")[0] || ""}
              onChange={(e) => setFormData({ ...formData, dateOfBirth: e.target.value })}
              className="w-full rounded-[10px] border border-[#E5EAE7] bg-white py-2.5 px-3 text-sm text-[#212529] outline-none transition focus:border-[#315343] disabled:bg-[#F8FAF9] disabled:text-[#75837D]"
            />
          </div>

          {/* Address */}
          <div className="sm:col-span-2">
            <label className="mb-1.5 block text-sm font-semibold text-[#212529]">Address</label>
            <input
              type="text"
              disabled={!isEditing}
              value={formData.address}
              onChange={(e) => setFormData({ ...formData, address: e.target.value })}
              className="w-full rounded-[10px] border border-[#E5EAE7] bg-white py-2.5 px-3 text-sm text-[#212529] outline-none transition focus:border-[#315343] disabled:bg-[#F8FAF9] disabled:text-[#75837D]"
            />
          </div>

          {/* About */}
          <div className="sm:col-span-2">
            <label className="mb-1.5 block text-sm font-semibold text-[#212529]">About Me</label>
            <textarea
              disabled={!isEditing}
              rows={4}
              value={formData.about}
              onChange={(e) => setFormData({ ...formData, about: e.target.value })}
              className="w-full resize-none rounded-[10px] border border-[#E5EAE7] bg-white py-2.5 px-3 text-sm text-[#212529] outline-none transition focus:border-[#315343] disabled:bg-[#F8FAF9] disabled:text-[#75837D]"
              placeholder="Tell us a little bit about yourself..."
            />
          </div>
        </div>

        {isEditing && (
          <div className="flex items-center justify-end gap-3 border-t border-[#E5EAE7] pt-5">
            <button
              type="button"
              onClick={() => setIsEditing(false)}
              className="rounded-[10px] px-4 py-2.5 text-sm font-semibold text-[#75837D] transition hover:bg-[#EEF3F0]"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={loading}
              className="flex items-center justify-center rounded-[10px] bg-[#315343] px-6 py-2.5 text-sm font-bold text-white transition hover:bg-[#233f32] active:scale-[0.99] disabled:opacity-70"
            >
              {loading ? <Loader2 size={16} className="mr-2 animate-spin text-[#C3F53C]" /> : null}
              Save Changes
            </button>
          </div>
        )}
      </form>
    </div>
  );
}