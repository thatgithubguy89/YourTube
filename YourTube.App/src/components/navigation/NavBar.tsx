import { SignedOutLinks } from "./SignedOutLinks";
import { SignedInLinks } from "./SignedInLinks";

export const NavBar = () => {
  const token = localStorage.getItem("token");

  return (
    <div className="mb-5">
      <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
        <div className="container-fluid">
          <a className="navbar-brand" href="/">
            YourTube
          </a>
          <div className="collapse navbar-collapse" id="navbarColor02">
            <ul className="navbar-nav me-auto">
              {!token && <SignedOutLinks />}
              {token && <SignedInLinks />}
            </ul>
          </div>
        </div>
      </nav>
    </div>
  );
};
