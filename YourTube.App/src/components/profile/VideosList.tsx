import { useNavigate } from "react-router-dom";
import { Video } from "../../models/Video";
import axios from "axios";

interface Props {
  videos: Video[];
}

export const VideosList = ({ videos }: Props) => {
  const navigate = useNavigate();
  const token = localStorage.getItem("token");

  const handleSubmit = (id: number) => {
    axios
      .delete(`${import.meta.env.VITE_VIDEOS_DELETE_VIDEO_URL}${id}`, {
        headers: {
          Authorization: `${token}`,
        },
      })
      .then(() => window.location.reload())
      .catch((error) => console.log(error));
  };

  return (
    <>
      <div className="container mt-5">
        <div className="row text-center text-lg-start">
          {videos.map((video) => (
            <div className="col-lg-3 col-md-4 col-6" key={video.id}>
              <div
                style={{ textDecoration: "none", color: "white" }}
                className="d-block mb-4 h-100"
              >
                <video
                  style={{ cursor: "pointer" }}
                  className="img-fluid img-thumbnail"
                  src={`${import.meta.env.VITE_VIDEO_PATH_URL}${
                    video.videoUrl
                  }`}
                  onClick={() => navigate(`/videodetail/${video.id}`)}
                />
                <div>
                  <p>
                    {video.title}
                    {video.createTime.toString().substring(0, 10)}
                    <i
                      style={{ cursor: "pointer" }}
                      className="bi bi-trash-fill ms-5"
                      onClick={() => handleSubmit(video.id)}
                    ></i>
                  </p>
                </div>
              </div>
            </div>
          ))}
        </div>
      </div>
    </>
  );
};
