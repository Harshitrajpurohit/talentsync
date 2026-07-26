import api from "../../../shared/api/axios";

import type { Profile, UpdateProfileRequest } from "../types/profile";

export async function getProfile() {
  const response = await api.get<Profile>("/users/me");
  return response.data;
}

export async function updateProfile(data: UpdateProfileRequest) {
  const response = await api.put<Profile>("/users/me", data);
  return response.data;
}