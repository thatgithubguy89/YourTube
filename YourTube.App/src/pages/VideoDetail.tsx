import { Player } from "../components/videodetail/Player";

export const VideoDetail = () => {
  const token = localStorage.getItem("token");

  return (
    <div>
      <div className="container d-flex mt-5">
        <Player />
        <div
          className="me-5 rounded"
          style={{
            border: "1px solid white",
            height: "400px",
            width: "400px",
          }}
        ></div>
      </div>
    </div>
  );
};
