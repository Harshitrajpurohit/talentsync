export type ApplicationStatus =
  | "Submitted"
  | "Screening"
  | "InterviewScheduled"
  | "InterviewCompleted"
  | "Selected"
  | "Rejected";

export type InterviewStatus =
  | "Scheduled"
  | "Completed"
  | "Cancelled"
  | "Rescheduled";


export type JobStatus =
  | "Open"
  | "Closed";