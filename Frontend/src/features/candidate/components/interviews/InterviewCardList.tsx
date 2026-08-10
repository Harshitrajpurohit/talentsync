import type { CandidateInterview } from "../../types/interview";
import InterviewCard from "./InterviewCard";

interface InterviewCardListProps {
  interviews: CandidateInterview[];
}

export default function InterviewCardList({ interviews }: InterviewCardListProps) {
  return (
    <div className="grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-3">
      {interviews.map((interview) => (
        <InterviewCard key={interview.id} interview={interview} />
      ))}
    </div>
  );
}