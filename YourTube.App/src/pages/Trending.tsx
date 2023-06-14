import axios from "axios";
import { useState, useEffect } from "react";
import { VideosList } from "../components/home/VideosList";
import { Video } from "../models/Video";

export const Trending = () => {
  const [videos, setVideos] = useState<Video[]>([]);

  useEffect(() => {
    axios
      .get(`${import.meta.env.VITE_VIDEOS_TRENDING_URL}`)
      .then((response) => setVideos(response.data))
      .catch((error) => console.log(error));
  }, []);

  return (
    <div>
      <VideosList videos={videos} />
    </div>
  );
};
