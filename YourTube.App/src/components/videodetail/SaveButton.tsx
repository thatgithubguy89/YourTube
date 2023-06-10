import axios from "axios";

interface Props {
  videoId: number;
}

export const SaveButton = ({ videoId }: Props) => {
  const username = localStorage.getItem("username");

  const handleSaveVideo = () => {
    const data = {
      videoId: videoId,
      username: username,
    };

    axios
      .post(`${import.meta.env.VITE_VIDEOS_FAVORITES_URL}`, data)
      .catch((error) => console.log(error));
  };

  return (
    <div
      className="ms-5"
      style={{ display: "inline-block", cursor: "pointer" }}
    >
      <div>
        <i onClick={() => handleSaveVideo()} className="bi bi-star-fill"></i>
      </div>
    </div>
  );
};
