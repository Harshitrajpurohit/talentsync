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
  | "Passed"
  | "Failed"
  | "Cancelled"


export type JobStatus =
  | "Open"
  | "Closed";