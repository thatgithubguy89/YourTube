import { VideosList } from "../components/home/VideosList";
import axios from "axios";
import { useState, useEffect } from "react";
import { Video } from "../models/Video";
import { Loader } from "../components/common/Loader";

export const Home = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [videos, setVideos] = useState<Video[]>([]);

  useEffect(() => {
    axios
      .get(`${import.meta.env.VITE_VIDEOS_URL}`)
      .then((response) => {
        setVideos(response.data);
        setIsLoading(false);
      })
      .catch((error) => console.log(error));
  }, []);

  if (isLoading) {
    return <Loader />;
  } else {
    return (
      <>
        <VideosList videos={videos} />
      </>
    );
  }
};
