export type ScreeningResult =
  | "Pass"
  | "Fail";

export interface CreateScreeningRequest {
  applicationId: string;
  result: ScreeningResult;
  notes: string;
}

export interface ScreeningResponse {
  id: string;
  applicationId: string;
  reviewerId: string;
  reviewerName: string;
  result: ScreeningResult;
  notes: string;
  reviewedAt: string;
}