export type SelectionDecision =
  | "Selected"
  | "Rejected";

export interface CreateSelectionDecisionRequest {
  applicationId: string;
  decision: SelectionDecision;
  notes: string;
  department: string;
  position: string;
}

export interface SelectionResponse {
  id: string;
  applicationId: string;
  decision: SelectionDecision;
  notes: string;
  department: string;
  position: string;
  createdAt: string;
}