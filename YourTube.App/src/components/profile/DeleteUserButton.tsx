import axios from "axios";
import { useNavigate } from "react-router-dom";

interface Props {
  id: number;
}

export const DeleteUserButton = ({ id }: Props) => {
  const navigate = useNavigate();
  const token = localStorage.getItem("token");

  const handleDeletUser = () => {
    axios
      .delete(`${import.meta.env.VITE_USER_PATH_URL}${id}`, {
        headers: {
          Authorization: `${token}`,
        },
      })
      .then(() => {
        localStorage.setItem("username", "");
        localStorage.setItem("profileimage", "");
        localStorage.setItem("token", "");
        navigate("/");
        window.location.reload();
      });
  };

  return (
    <div style={{ display: "inline-block" }}>
      <button onClick={() => handleDeletUser()} className="btn btn-danger ms-5">
        Delete Account
      </button>
    </div>
  );
};
