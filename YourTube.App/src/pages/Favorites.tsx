import { useEffect, useState } from "react";
import { Video } from "../models/Video";
import axios from "axios";
import { VideosList } from "../components/favorites/VideosList";

export const Favorites = () => {
  const [videos, setVideos] = useState<Video[]>([]);
  const username = localStorage.getItem("username");

  useEffect(() => {
    axios
      .get(`${import.meta.env.VITE_VIDEOS_FAVORITES_URL}${username}`)
      .then((response) => setVideos(response.data))
      .catch((error) => console.log(error));
  }, []);

  return (
    <div>
      <VideosList videos={videos} />
    </div>
  );
};
