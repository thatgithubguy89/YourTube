import { useNavigate } from "react-router-dom";

export const SignedInLinks = () => {
  const navigate = useNavigate();
  const profileImage = localStorage.getItem("profileimage");

  const handleSignOut = () => {
    localStorage.setItem("username", "");
    localStorage.setItem("profileimage", "");
    localStorage.setItem("token", "");
    navigate("/");
    window.location.reload();
  };

  return (
    <div className="d-flex">
      <li className="nav-item">
        <a className="nav-link" href="#" onClick={() => handleSignOut()}>
          Sign Out
        </a>
      </li>
      <li className="nav-item">
        <a className="nav-link" href="/userprofile">
          <img
            style={{
              height: "25px",
              width: "25px",
            }}
            className="rounded-circle"
            src={`${import.meta.env.VITE_USER_IMAGE_PATH_URL}${profileImage}`}
          />
        </a>
      </li>
    </div>
  );
};
