import { useState } from "react";
import { useNavigate } from "react-router-dom";

export const SearchBox = () => {
  const [search, setSearch] = useState("");
  const navigate = useNavigate();

  const handleSubmit = () => {
    navigate(`/${search}`);
    window.location.reload();
  };

  return (
    <div className="offset-4">
      <input
        type="text"
        className="form-control w-50 container"
        placeholder="Search"
        onChange={(e) => setSearch(e.target.value)}
        style={{ display: "inline-block" }}
      />
      <button onClick={handleSubmit} className="btn btn-primary">
        <i className="bi bi-search"></i>
      </button>
    </div>
  );
};
