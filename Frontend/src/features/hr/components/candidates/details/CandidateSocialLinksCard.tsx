import { ExternalLink, Globe } from "lucide-react";
import { SiGithub } from '@icons-pack/react-simple-icons';
import { FaLinkedin } from 'react-icons/fa';
import type { User } from "../../../../../shared/types/user";

interface CandidateSocialLinksCardProps {
  candidate: User;
}

export default function CandidateSocialLinksCard({
  candidate,
}: CandidateSocialLinksCardProps) {
  const links = [
    {
      label: "LinkedIn",
      url: candidate.linkedinUrl,
      icon: FaLinkedin,
      iconColor: "text-[#0A66C2]",
    },
    {
      label: "GitHub",
      url: candidate.githubUrl,
      icon: SiGithub,
      iconColor: "text-[#181717]",
    },
    {
      label: "Portfolio",
      url: candidate.portfolioUrl,
      icon: Globe,
      iconColor: "text-[#315343]",
    },
  ];

  return (
    <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm">
      <h2 className="mb-5 text-lg font-bold text-[#212529]">
        Social Links
      </h2>

      <div className="space-y-3">
        {links.map(({ label, url, icon: Icon, iconColor }) => (
          <div
            key={label}
            className="flex items-center justify-between rounded-xl border border-[#E5EAE7] p-3.5 transition-colors hover:border-[#315343]"
          >
            <div className="flex items-center gap-3">
              <Icon size={20} className={iconColor} />
              <div>
                <p className="text-sm font-bold text-[#212529]">{label}</p>
                <p className="truncate w-40 text-xs font-medium text-[#75837D]">
                  {url ?? "Not Available"}
                </p>
              </div>
            </div>

            {url && (
              <a
                href={url}
                target="_blank"
                rel="noreferrer"
                className="flex h-8 w-8 items-center justify-center rounded-lg text-[#75837D] transition hover:bg-[#EEF3F0] hover:text-[#315343]"
              >
                <ExternalLink size={16} />
              </a>
            )}
          </div>
        ))}
      </div>
    </div>
  );
}