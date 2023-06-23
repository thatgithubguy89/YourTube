import { useEffect, useState } from "react";
import { Video } from "../models/Video";
import { User } from "../models/User";
import axios from "axios";
import { VideosList } from "../components/profile/VideosList";
import { useNavigate } from "react-router-dom";
import { DeleteUserButton } from "../components/profile/DeleteUserButton";
import { Loader } from "../components/common/Loader";

export const UserProfile = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [user, setUser] = useState<User | any>(null);
  const [videos, setVideos] = useState<Video[]>([]);
  const username = localStorage.getItem("username");
  const token = localStorage.getItem("token");
  const navigate = useNavigate();

  useEffect(() => {
    if (!token) {
      navigate("/signin");
    }

    axios
      .get(`${import.meta.env.VITE_USER_PATH_URL}${username}`, {
        headers: {
          Authorization: `${token}`,
        },
      })
      .then((response) => {
        setUser(response.data);
        setVideos(response.data.videos);
        setIsLoading(false);
      })
      .catch((error) => console.log(error));
  }, []);

  if (isLoading) {
    return <Loader />;
  } else {
    return (
      <div className="container mt-5">
        {user.username}
        <button
          onClick={() => navigate(`/addvideo/${user.id}`)}
          className="btn btn-primary ms-5"
        >
          Add Video
        </button>
        <DeleteUserButton id={user.id} />
        <VideosList videos={videos} />
      </div>
    );
  }
};
