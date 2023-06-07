import { Video } from "../../models/Video";

interface Props {
  videos: Video[];
}

export const VideosList = ({ videos }: Props) => {
  return (
    <>
      <div className="container mt-5">
        <div className="row text-center text-lg-start">
          {videos.map((video) => (
            <div className="col-lg-3 col-md-4 col-6" key={video.id}>
              <a
                style={{ textDecoration: "none", color: "white" }}
                href={`/videodetail/${video.id}`}
                className="d-block mb-4 h-100"
              >
                <video
                  className="img-fluid img-thumbnail"
                  src={`${import.meta.env.VITE_VIDEO_PATH_URL}${
                    video.videoUrl
                  }`}
                />
                <div>
                  <p>
                    {video.user.username} -{" "}
                    {video.createTime.toString().substring(0, 10)}
                  </p>
                </div>
              </a>
            </div>
          ))}
        </div>
      </div>
    </>
  );
};
