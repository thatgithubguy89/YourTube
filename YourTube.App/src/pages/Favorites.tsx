import { useEffect, useState } from "react";
import { Video } from "../models/Video";
import axios from "axios";
import { VideosList } from "../components/favorites/VideosList";
import { useNavigate } from "react-router-dom";

export const Favorites = () => {
  const [videos, setVideos] = useState<Video[]>([]);
  const navigate = useNavigate();
  const username = localStorage.getItem("username");
  const token = localStorage.getItem("token");

  useEffect(() => {
    if (!token) {
      return navigate("/signin");
    }

    axios
      .get(`${import.meta.env.VITE_VIDEOS_FAVORITES_URL}${username}`, {
        headers: {
          Authorization: `${token}`,
        },
      })
      .then((response) => setVideos(response.data))
      .catch((error) => console.log(error));
  }, []);

  return (
    <div>
      <VideosList videos={videos} />
    </div>
  );
};
