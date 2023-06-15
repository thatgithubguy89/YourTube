import { useState } from "react";
import { Video } from "../../models/Video";

interface Props {
  videos: Video[];
}

export const VideosList = ({ videos }: Props) => {
  const [search, setSearch] = useState<string>("");

  return (
    <>
      <div className="container mt-5">
        <input
          type="text"
          className="form-control w-50 mb-5 container"
          placeholder="Search"
          onChange={(e) => setSearch(e.target.value)}
        />
        <div className="row text-center text-lg-start">
          {videos
            .filter((vid) =>
              vid.title?.toLowerCase().includes(search.toLowerCase())
            )
            .map((video) => (
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
                    <p>{video.title}</p>
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
