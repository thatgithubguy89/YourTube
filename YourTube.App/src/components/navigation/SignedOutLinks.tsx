export const SignedOutLinks = () => {
  return (
    <div className="d-flex">
      <li className="nav-item">
        <a className="nav-link" href="/signin">
          Sign In
        </a>
      </li>
      <li className="nav-item">
        <a className="nav-link" href="/trending">
          Trending
        </a>
      </li>
    </div>
  );
};
