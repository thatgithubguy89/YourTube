import axios from "axios";
import { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

export const AddVideo = () => {
  const [title, setTitle] = useState("");
  const [file, setFile] = useState<Blob>(new Blob());
  const { userid } = useParams();
  const token = localStorage.getItem("token");
  const navigate = useNavigate();

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    let file: any = event.currentTarget.files;
    setFile(file[0]);
  };

  const handleSubmit = (e: { preventDefault: () => void }): any => {
    e.preventDefault();

    const data = {
      video: {
        title: title,
        userId: userid,
      },
      file: file,
    };

    axios
      .post(`${import.meta.env.VITE_VIDEOS_ADD_VIDEO_URL}`, data, {
        headers: {
          "Content-Type": "multipart/form-data",
          Authorization: `${token}`,
        },
      })
      .then(() => navigate("/userprofile"))
      .catch((error) => console.log(error));
  };

  return (
    <>
      <form onSubmit={handleSubmit} className="container w-50">
        <div className="form-group">
          <div className="form-floating mb-3">
            <input
              type="text"
              className="form-control"
              id="floatingInput"
              placeholder="title"
              onChange={(e) => setTitle(e.target.value)}
            />
            <label htmlFor="floatingInput">Title</label>
          </div>
        </div>
        <div className="form-group">
          <input
            className="form-control"
            type="file"
            id="formFile"
            onChange={handleFileChange}
          />
        </div>
        <button type="submit" className="btn btn-primary mt-3">
          Submit
        </button>
      </form>
    </>
  );
};
