import { useEffect, useState } from "react";
import { Video } from "../models/Video";
import axios from "axios";
import { VideosList } from "../components/favorites/VideosList";
import { NotFound } from "../components/common/NotFound";

export const Favorites = () => {
  const [videos, setVideos] = useState<Video[]>([]);
  const username = localStorage.getItem("username");
  const token = localStorage.getItem("token");

  useEffect(() => {
    axios
      .get(`${import.meta.env.VITE_VIDEOS_FAVORITES_URL}${username}`)
      .then((response) => setVideos(response.data))
      .catch((error) => console.log(error));
  }, []);

  if (!token) {
    return <NotFound />;
  } else {
    return (
      <div>
        <VideosList videos={videos} />
      </div>
    );
  }
};
