import React, { createContext, useState } from "react";
import { Video } from "../models/Video";
import { Tag } from "../models/Tag";
import axios from "axios";

export interface Props {
  children: React.ReactNode;
}

export type VideosContextType = {
  videos: Video[];
  setRecommendedVideos: (tags: Tag[]) => void;
};

export const VideoContext = createContext<VideosContextType | null>(null);

export const VideoProvider: React.FC<Props> = ({ children }) => {
  const [videos, setVideos] = useState<Video[]>([]);

  const setRecommendedVideos = (tags: Tag[]) => {
    axios
      .post(`${import.meta.env.VITE_VIDEOS_RECOMMENDATION_URL}`, tags)
      .then((response) => setVideos(response.data))
      .catch((error) => console.log(error));
  };

  return (
    <VideoContext.Provider value={{ setRecommendedVideos, videos }}>
      {children}
    </VideoContext.Provider>
  );
};
