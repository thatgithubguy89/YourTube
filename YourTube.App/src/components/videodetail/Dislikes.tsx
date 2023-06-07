import axios from "axios";

interface Props {
  dislikes: number;
  videoId: number;
}

export const Dislikes = ({ dislikes, videoId }: Props) => {
  const token = localStorage.getItem("token");
  const username = localStorage.getItem("username");

  const data = {
    liked: false,
    disliked: true,
    username: username,
    videoId: videoId,
  };

  const handleCreateDislike = () => {
    axios
      .post(`${import.meta.env.VITE_CREATE_LIKE_URL}`, data, {
        headers: {
          Authorization: `${token}`,
        },
      })
      .then((response) => {
        if (response.status == 204) {
          return;
        } else {
          window.location.reload();
        }
      })
      .catch((error) => console.log(error));
  };

  return (
    <div
      onClick={() => handleCreateDislike()}
      style={{ display: "inline-block", cursor: "pointer" }}
    >
      <i className="bi bi-hand-thumbs-down-fill me-3"></i>
      {dislikes}
    </div>
  );
};
