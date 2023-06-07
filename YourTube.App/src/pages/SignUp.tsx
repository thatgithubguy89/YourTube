import axios from "axios";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

export const SignUp = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [file, setFile] = useState<Blob>(new Blob());
  const navigate = useNavigate();

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    let file: any = event.currentTarget.files;
    setFile(file[0]);
  };

  const handleSubmit = (e: { preventDefault: () => void }): any => {
    e.preventDefault();

    const data = {
      email: email,
      password: password,
      file: file,
    };

    axios
      .post(`${import.meta.env.VITE_AUTH_SIGNUP_URL}`, data, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then(() => navigate("/"))
      .catch((error) => console.log(error));
  };

  return (
    <>
      <form onSubmit={handleSubmit} className="container w-50">
        <div className="form-group">
          <div className="form-floating mb-3">
            <input
              type="email"
              className="form-control"
              id="floatingInput"
              placeholder="name@example.com"
              onChange={(e) => setEmail(e.target.value)}
            />
            <label htmlFor="floatingInput">Email address</label>
          </div>
          <div className="form-floating mb-3">
            <input
              type="password"
              className="form-control"
              id="floatingPassword"
              placeholder="Password"
              onChange={(e) => setPassword(e.target.value)}
            />
            <label htmlFor="floatingPassword">Password</label>
          </div>
        </div>
        <div className="form-group mb-3">
          <input
            className="form-control"
            type="file"
            id="formFile"
            onChange={handleFileChange}
          />
        </div>
        <button type="submit" className="btn btn-primary">
          Submit
        </button>
      </form>
    </>
  );
};
