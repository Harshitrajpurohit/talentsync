export interface PaginationRequest {
  pageNumber: number;
  pageSize: number;
}

export interface PaginationResponse<T> {
  pageNumber: number;
  pageSize: number;
  totalRecords: number;
  data: T[];
}