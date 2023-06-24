import { useEffect, useState } from "react";
import { Tag } from "../../models/Tag";
import { Video } from "../../models/Video";
import axios from "axios";

interface Props {
  videoId: number;
  tags: Tag[];
}

export const RecommendedList = ({ videoId, tags }: Props) => {
  const [videos, setVideos] = useState<Video[]>([]);

  useEffect(() => {
    axios
      .post(`${import.meta.env.VITE_VIDEOS_RECOMMENDATION_URL}${videoId}`, tags)
      .then((response) => setVideos(response.data))
      .catch((error) => console.log(error));
  }, []);

  return (
    <div
      className="me-5 rounded ms-5"
      style={{
        height: "600px",
        width: "400px",
      }}
    >
      {videos.map((video) => (
        <div className="col-lg-3 col-md-4 col-6" key={video.id}>
          <a
            style={{ textDecoration: "none", color: "white" }}
            href={`/videodetail/${video.id}`}
            className="d-block mb-4 h-100"
          >
            <video
              className="img-fluid img-thumbnail"
              src={`${import.meta.env.VITE_VIDEO_PATH_URL}${video.videoUrl}`}
            />
            <div>
              <small>{video.title}</small>
              <br />
              <small>{video.user.username}</small>
            </div>
          </a>
        </div>
      ))}
    </div>
  );
};
