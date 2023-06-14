import { useNavigate } from "react-router-dom";
import { Video } from "../../models/Video";

interface Props {
  videos: Video[];
}

export const VideosList = ({ videos }: Props) => {
  const navigate = useNavigate();

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
