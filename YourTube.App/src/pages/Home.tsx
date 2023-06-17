import { VideosList } from "../components/home/VideosList";
import axios from "axios";
import { useState, useEffect } from "react";
import { Video } from "../models/Video";
import { Loader } from "../components/common/Loader";
import { useParams } from "react-router-dom";

export const Home = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [videos, setVideos] = useState<Video[]>([]);
  const { searchphrase } = useParams();

  useEffect(() => {
    axios
      .get(`${import.meta.env.VITE_VIDEOS_URL}/${searchphrase ?? " "}`)
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
