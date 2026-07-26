import { useProfile } from "../hooks/useProfile";
import { useResume } from "../hooks/useResume";

import { getAuth } from "../../../shared/api/authStorage";

import ProfileSkeleton from "../components/ProfileSkeleton";
import ProfileHeader from "../components/ProfileHeader";
import ProfilePicture from "../components/ProfilePicture";
import SocialLinksCard from "../components/SocialLinksCard";
import ResumeCard from "../components/ResumeCard";
import PersonalInformationCard from "../components/PersonalInformationCard";

export default function ProfilePage() {
  const {
    profile,
    loading: profileLoading,
    refresh: refreshProfile,
  } = useProfile();

  const authUser = getAuth();
  const isCandidate = authUser?.role === "Candidate";

    const {
      resume,
      loading: resumeLoading,
      refresh: refreshResume,
    } = useResume(isCandidate);

  if (profileLoading || !profile) {
    return <ProfileSkeleton />;
  }

  return (
    <div className="mx-auto max-w-6xl space-y-6">
      <ProfileHeader
        profile={profile}
        resume={resume}
        role={authUser?.role}
      />

      <div className="grid grid-cols-1 items-start gap-6 lg:grid-cols-3">
        <div className="space-y-6 lg:sticky lg:top-8 lg:col-span-1">
          <ProfilePicture profile={profile} />

          <SocialLinksCard
            profile={profile}
            onUpdate={refreshProfile}
          />

          {isCandidate && (
            <ResumeCard
              resume={resume}
              loading={resumeLoading}
              refresh={refreshResume}
            />
          )}
        </div>

        <div className="space-y-6 lg:col-span-2">
          <PersonalInformationCard
            profile={profile}
            onUpdate={refreshProfile}
          />
        </div>
      </div>
    </div>
  );
}