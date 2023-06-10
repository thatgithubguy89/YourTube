import axios from "axios";
import { useState } from "react";

interface Props {
  videoId: number;
}

export const CommentForm = ({ videoId }: Props) => {
  const [content, setContent] = useState("");
  const username = localStorage.getItem("username");

  const handleSubmit = (e: { preventDefault: () => void }): any => {
    e.preventDefault();

    const data = {
      content: content,
      videoId: videoId,
      username: username,
    };

    axios
      .post(`${import.meta.env.VITE_CREATE_COMMENT_URL}`, data)
      .then(() => window.location.reload())
      .catch((error) => console.log(error));
  };

  return (
    <form onSubmit={handleSubmit} className="mt-3">
      <input
        style={{ backgroundColor: "black", color: "white" }}
        className="form-control"
        placeholder="Add a comment..."
        onChange={(e) => setContent(e.target.value)}
      />
      <button
        className="btn btn-primary mt-2"
        type="submit"
        disabled={username ? false : true}
      >
        Comment
      </button>
    </form>
  );
};
