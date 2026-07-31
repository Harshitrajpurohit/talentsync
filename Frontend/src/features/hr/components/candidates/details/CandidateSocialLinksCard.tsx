import {
  ExternalLink,
  Globe,
} from "lucide-react";
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
    },
    {
      label: "GitHub",
      url: candidate.githubUrl,
      icon: SiGithub,
    },
    {
      label: "Portfolio",
      url: candidate.portfolioUrl,
      icon: Globe,
    },
  ];

  return (
    <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
      <h2 className="mb-5 text-lg font-semibold text-[#212529] dark:text-white">
        Social Links
      </h2>

      <div className="space-y-4">
        {links.map(({ label, url, icon: Icon }) => (
          <div
            key={label}
            className="flex items-center justify-between rounded-xl border border-[#E5EAE7] p-4 dark:border-[#315343]"
          >
            <div className="flex items-center gap-3">
              <Icon
                size={20}
                className="text-[#315343] dark:text-[#C3F53C]"
              />

              <div>
                <p className="font-medium">{label}</p>
                <p className="text-xs text-[#75837D]">
                  {url ?? "Not Available"}
                </p>
              </div>
            </div>

            {url && (
              <a
                href={url}
                target="_blank"
                rel="noreferrer"
                className="rounded-lg p-2 transition hover:bg-[#315343]/10"
              >
                <ExternalLink size={18} />
              </a>
            )}
          </div>
        ))}
      </div>
    </div>
  );
}