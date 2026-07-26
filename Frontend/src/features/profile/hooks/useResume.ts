import { useEffect, useState } from "react";

import { getMyResume } from "../api/resumeApi";
import type { Resume } from "../types/resume";

export function useResume(enabled: boolean = true) {
  const [resume, setResume] = useState<Resume | null>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (!enabled) return;

    void loadResume();
  }, [enabled]);

  async function loadResume() {
    setLoading(true);

    try {
      const data = await getMyResume();
      setResume(data);
    } finally {
      setLoading(false);
    }
  }

  return {
    resume,
    loading,
    refresh: loadResume,
  };
}