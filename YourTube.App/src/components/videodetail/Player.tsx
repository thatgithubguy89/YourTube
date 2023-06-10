import { useEffect, useState } from "react";
import { Video } from "../../models/Video";
import { useParams } from "react-router-dom";
import axios from "axios";
import { Loader } from "../common/Loader";
import { Comments } from "./Comments";
import { Comment } from "../../models/Comment";
import { Likes } from "./Likes";
import { Dislikes } from "./Dislikes";
import { SaveButton } from "./SaveButton";

export const Player = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [video, setVideo] = useState<Video | any>(null);
  const [comments, setComments] = useState<Comment[]>([]);
  const { id } = useParams();
  const username = localStorage.getItem("username");
  const token = localStorage.getItem("token");

  useEffect(() => {
    axios
      .get(`${import.meta.env.VITE_VIDEOS_SINGLE_VIDEO_URL}/${id}`)
      .then((response) => {
        setVideo(response.data);
        setComments(response.data.comments);
        setIsLoading(false);
      })
      .catch((error) => console.log(error));
  }, []);

  useEffect(() => {
    const data = {
      videoId: id,
      username: username,
    };

    if (username) {
      axios
        .post(`${import.meta.env.VITE_CREATE_VIDEOVIEW_URL}`, data, {
          headers: {
            Authorization: `${token}`,
          },
        })
        .catch((error) => console.log(error));
    }
  }, []);

  if (isLoading) {
    return <Loader />;
  } else {
    return (
      <div>
        <div
          className="me-5 rounded"
          style={{ border: "1px solid white", height: "600px", width: "800px" }}
        >
          <video
            controls
            width={"100%"}
            height={"100%"}
            src={`${import.meta.env.VITE_VIDEO_PATH_URL}${video.videoUrl}`}
          ></video>
        </div>
        <small className="me-5">{video.views} Views</small>
        <Likes likes={video.liked} videoId={video.id} />
        <Dislikes dislikes={video.disliked} videoId={video.id} />
        <SaveButton videoId={Number(id)} />
        <Comments comments={comments} videoId={Number(id)} />
      </div>
    );
  }
};
