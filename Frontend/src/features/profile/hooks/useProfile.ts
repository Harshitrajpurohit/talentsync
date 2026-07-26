import { useEffect, useState } from "react";

import { getProfile } from "../api/profileApi";

import type { Profile } from "../types/profile";

export function useProfile() {
  const [profile, setProfile] = useState<Profile | null>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    void loadProfile();
  }, []);

  async function loadProfile() {
    setLoading(true);

    try {
      const profile = await getProfile();
      setProfile(profile);
    } finally {
      setLoading(false);
    }
  }

  return {
    profile,
    loading,
    refresh: loadProfile,
  };
}