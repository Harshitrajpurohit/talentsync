interface InterviewHeaderProps {
  totalInterviews: number;
}

export default function InterviewHeader({ totalInterviews }: InterviewHeaderProps) {
  return (
    <div className="flex flex-col gap-5 md:flex-row md:items-end md:justify-between">
      <div>
        <h1 className="text-2xl font-bold text-[#212529]">
          My Interviews
        </h1>
        <p className="mt-1 text-sm font-medium text-[#75837D]">
          Track your upcoming, completed, and cancelled interviews.
        </p>
      </div>

      <div className="flex shrink-0 items-center gap-4 rounded-2xl border border-[#E5EAE7] bg-white px-5 py-3 shadow-sm">
        <div className="flex h-10 w-10 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
          <span className="text-lg font-bold">{totalInterviews}</span>
        </div>
        <p className="text-sm font-bold text-[#212529]">
          Total Interviews
        </p>
      </div>
    </div>
  );
}