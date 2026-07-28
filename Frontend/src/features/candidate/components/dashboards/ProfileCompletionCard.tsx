import { CheckCircle2, ArrowRight } from "lucide-react";
import { Link } from "react-router-dom";

interface ProfileCompletionCardProps {
  percentage: number;
}

export default function ProfileCompletionCard({
  percentage,
}: ProfileCompletionCardProps) {
  const isComplete = percentage === 100;

  const radius = 28;
  const circumference = 2 * Math.PI * radius;
  const strokeDashoffset = circumference - (percentage / 100) * circumference;

  return (
    <div className="flex flex-col justify-center rounded-[20px] border border-[#E5EAE7] bg-white p-6 shadow-sm transition-all duration-300 dark:border-[#315343] dark:bg-[#253f33]">
      <div className="flex items-center gap-4 sm:gap-5">
        
        <div className="relative flex h-16 w-16 shrink-0 items-center justify-center">
          <svg className="h-16 w-16 -rotate-90 transform drop-shadow-sm">
            <circle
              cx="32"
              cy="32"
              r={radius}
              stroke="#E5EAE7"
              strokeWidth="6"
              fill="transparent"
              className="dark:stroke-[#1e3329]"
            />
            <circle
              cx="32"
              cy="32"
              r={radius}
              stroke="currentColor"
              strokeWidth="6"
              fill="transparent"
              strokeDasharray={circumference}
              strokeDashoffset={strokeDashoffset}
              strokeLinecap="round"
              className={`transition-all duration-1000 ease-out ${
                isComplete 
                  ? "text-[#C3F53C]" 
                  : "text-[#315343] dark:text-[#C3F53C]"
              }`}
            />
          </svg>
          <span className="absolute text-sm font-bold text-[#315343] dark:text-white">
            {percentage}%
          </span>
        </div>

        <div>
          <h3 className="text-base font-bold text-[#212529] dark:text-white">
            Profile Strength
          </h3>
          
          {isComplete ? (
            <div className="mt-1 flex items-center gap-1.5 text-xs font-semibold text-[#315343] dark:text-[#C3F53C]">
              <CheckCircle2 size={14} className="fill-[#C3F53C] text-white dark:text-[#253f33]" />
              Looking outstanding!
            </div>
          ) : (
            <>
              <p className="mt-1 text-xs font-medium text-[#75837D] dark:text-white/60">
                Complete your profile to stand out.
              </p>
              
              <Link 
                to="/candidate/profile"
                className="group mt-2.5 flex w-fit items-center gap-1 text-xs font-bold text-[#315343] transition-colors hover:text-[#212529] dark:text-[#C3F53C] dark:hover:text-white"
              >
                <span>Update Profile</span>
                <ArrowRight size={12} className="transition-transform group-hover:translate-x-1" />
              </Link>
            </>
          )}
        </div>
      </div>
    </div>
  );
}