
export type JobStatus = 'Open' | 'Closed';

export interface Job {
  id: string;
  title: string;
  department: string;
  description: string;
  requirements: string;
  postedDate: string;
  status: JobStatus;
  hrId: string;
}

export interface CreateJobRequest {
  title: string;
  department: string;
  description: string;
  requirements: string;
}

export interface UpdateJobRequest {
  title?: string;
  department?: string;
  description?: string;
  requirements?: string;
  status?: JobStatus;
}

export interface JobListItem {
  id: string;
  title: string;
  department: string;
  postedDate: string;
  status: JobStatus;
}

export type JobResponse = Job;