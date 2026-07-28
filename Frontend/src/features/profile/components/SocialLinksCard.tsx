import { useState } from "react";
import { SiGithub } from '@icons-pack/react-simple-icons';
import { Globe, Edit2, Check, X } from "lucide-react";
import { FaLinkedin } from 'react-icons/fa';
import { useUpdateProfile } from "../hooks/useUpdateProfile";
import type { Profile } from "../types/profile";

interface Props {
  profile: Profile;
  onUpdate: () => void;
}

export default function SocialLinksCard({ profile, onUpdate }: Props) {
  const [isEditing, setIsEditing] = useState(false);
  const { update, loading } = useUpdateProfile();
  
  const [formData, setFormData] = useState({
        linkedinUrl: profile.linkedinUrl ?? "",
        githubUrl: profile.githubUrl ?? "",
        portfolioUrl: profile.portfolioUrl ?? "",
    });

   function hasSocialLinksChanged(payload: {
    linkedinUrl?: string;
    githubUrl?: string;
    portfolioUrl?: string;
  }) {
    return (
      (profile.linkedinUrl ?? undefined) !== payload.linkedinUrl ||
      (profile.githubUrl ?? undefined) !== payload.githubUrl ||
      (profile.portfolioUrl ?? undefined) !== payload.portfolioUrl
    );
  }

  async function handleSave() {
    const payload = {
      name: profile.name,
      linkedinUrl: formData.linkedinUrl.trim() || undefined,
      githubUrl: formData.githubUrl.trim() || undefined,
      portfolioUrl: formData.portfolioUrl.trim() || undefined,
    };

    if (!hasSocialLinksChanged(payload)) {
      setIsEditing(false);
      return;
    }

    try {
      await update(payload);
      onUpdate();
      setIsEditing(false);
    } catch {
      // Axios interceptor already handles the error toast.
    }
  }

  function handleEdit() {
    setFormData({
      linkedinUrl: profile.linkedinUrl ?? "",
      githubUrl: profile.githubUrl ?? "",
      portfolioUrl: profile.portfolioUrl ?? "",
    });

    setIsEditing(true);
  }

  function handleCancel() {
    setFormData({
      linkedinUrl: profile.linkedinUrl ?? "",
      githubUrl: profile.githubUrl ?? "",
      portfolioUrl: profile.portfolioUrl ?? "",
    });

    setIsEditing(false);
  }

  return (
    <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm">
      <div className="mb-4 flex items-center justify-between">
        <h3 className="text-lg font-bold text-[#212529]">Social Links</h3>
        {!isEditing ? (
          <button onClick={handleEdit} className="text-[#75837D] hover:text-[#315343] transition-colors">
            <Edit2 size={16} />
          </button>
        ) : (
          <div className="flex items-center gap-2">
            <button onClick={handleCancel} className="text-[#75837D] hover:text-red-500 transition-colors">
              <X size={18} />
            </button>
            <button onClick={handleSave} disabled={loading} className="text-[#315343] hover:text-green-600 transition-colors">
              <Check size={18} />
            </button>
          </div>
        )}
      </div>

      <div className="space-y-4">
        {/* LinkedIn */}
        <div>
          <label className="mb-1.5 flex items-center gap-2 text-sm font-semibold text-[#212529]">
            {/* Use SiLinkedin here */}
            <FaLinkedin size={16} className="text-[#0A66C2]" /> LinkedIn
          </label>
          {isEditing ? (
            <input
              type="url"
              value={formData.linkedinUrl}
              onChange={(e) => setFormData({ ...formData, linkedinUrl: e.target.value })}
              className="w-full rounded-[10px] border border-[#E5EAE7] bg-[#EEF3F0]/50 py-2 px-3 text-sm outline-none transition-colors focus:border-[#315343]"
              placeholder="https://linkedin.com/in/username"
            />
          ) : (
            <p className="truncate text-sm text-[#75837D]">
              {profile.linkedinUrl ? (
                <a href={profile.linkedinUrl} target="_blank" rel="noreferrer" className="hover:text-[#315343] hover:underline transition-colors">
                  {profile.linkedinUrl}
                </a>
              ) : (
                "Not provided"
              )}
            </p>
          )}
        </div>

        {/* GitHub */}
        <div>
          <label className="mb-1.5 flex items-center gap-2 text-sm font-semibold text-[#212529]">
            {/* Use SiGithub here */}
            <SiGithub size={16} className="text-[#181717]" /> GitHub
          </label>
          {isEditing ? (
            <input
              type="url"
              value={formData.githubUrl}
              onChange={(e) => setFormData({ ...formData, githubUrl: e.target.value })}
              className="w-full rounded-[10px] border border-[#E5EAE7] bg-[#EEF3F0]/50 py-2 px-3 text-sm outline-none transition-colors focus:border-[#315343]"
              placeholder="https://github.com/username"
            />
          ) : (
            <p className="truncate text-sm text-[#75837D]">
              {profile.githubUrl ? (
                <a href={profile.githubUrl} target="_blank" rel="noreferrer" className="hover:text-[#315343] hover:underline transition-colors">
                  {profile.githubUrl}
                </a>
              ) : (
                "Not provided"
              )}
            </p>
          )}
        </div>

        {/* Portfolio */}
        <div>
          <label className="mb-1.5 flex items-center gap-2 text-sm font-semibold text-[#212529]">
            <Globe size={16} className="text-[#315343]" /> Portfolio
          </label>
          {isEditing ? (
            <input
              type="url"
              value={formData.portfolioUrl}
              onChange={(e) => setFormData({ ...formData, portfolioUrl: e.target.value })}
              className="w-full rounded-[10px] border border-[#E5EAE7] bg-[#EEF3F0]/50 py-2 px-3 text-sm outline-none transition-colors focus:border-[#315343]"
              placeholder="https://yourwebsite.com"
            />
          ) : (
            <p className="truncate text-sm text-[#75837D]">
              {profile.portfolioUrl ? (
                <a href={profile.portfolioUrl} target="_blank" rel="noreferrer" className="hover:text-[#315343] hover:underline transition-colors">
                  {profile.portfolioUrl}
                </a>
              ) : (
                "Not provided"
              )}
            </p>
          )}
        </div>
      </div>
    </div>
  );
}