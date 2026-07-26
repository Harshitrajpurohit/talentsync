import { useState } from "react";

import { updateProfile } from "../api/profileApi";

import type {
  Profile,
  UpdateProfileRequest,
} from "../types/profile";

export function useUpdateProfile() {
  const [loading, setLoading] = useState(false);

  async function update(
    request: UpdateProfileRequest
  ): Promise<Profile> {
    setLoading(true);

    try {
      const profile = await updateProfile(request);
      return profile;
    } finally {
      setLoading(false);
    }
  }

  return {
    update,
    loading,
  };
}