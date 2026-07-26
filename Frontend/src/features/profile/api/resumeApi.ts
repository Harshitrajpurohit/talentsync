import api from "../../../shared/api/axios";
import type { Resume } from "../types/resume";

export async function getMyResume(): Promise<Resume> {
  const response = await api.get<Resume>("/resumes/my");
  return response.data;
}

export async function uploadResume(file: File): Promise<Resume> {
  const formData = new FormData();
  formData.append("resume", file);

  const response = await api.post<Resume>("/resumes", formData);

  return response.data;
}

export async function replaceResume(
  id: string,
  file: File
): Promise<Resume> {
  const formData = new FormData();
  formData.append("resume", file);

  const response = await api.put<Resume>(
    `/resumes/${id}`,
    formData
  );

  return response.data;
}