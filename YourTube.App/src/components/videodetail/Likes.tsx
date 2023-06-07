import axios from "axios";

interface Props {
  likes: number;
  videoId: number;
}

export const Likes = ({ likes, videoId }: Props) => {
  const token = localStorage.getItem("token");
  const username = localStorage.getItem("username");

  const data = {
    liked: true,
    disliked: false,
    username: username,
    videoId: videoId,
  };

  const handleCreateLike = () => {
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
      onClick={() => handleCreateLike()}
      className="me-5 mt-3"
      style={{ display: "inline-block", cursor: "pointer" }}
    >
      <i className="bi bi-hand-thumbs-up-fill me-3"></i>
      {likes}
    </div>
  );
};
