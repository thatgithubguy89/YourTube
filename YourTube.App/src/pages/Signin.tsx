import axios from "axios";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

export const Signin = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleSubmit = (e: { preventDefault: () => void }): any => {
    e.preventDefault();

    const data = {
      email: email,
      password: password,
    };

    axios
      .post(`${import.meta.env.VITE_AUTH_SIGNIN_URL}`, data)
      .then((response) => {
        localStorage.setItem("username", `${email}`),
          localStorage.setItem("token", `Bearer ${response.data.token}`),
          localStorage.setItem(
            "profileimage",
            `${response.data.userImagePath ?? "stock.jpg"}`
          ),
          navigate("/");
        window.location.reload();
      })
      .catch((error) => console.log(error));
  };

  return (
    <>
      <form onSubmit={handleSubmit} className="container w-50 mt-5">
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
        <div>
          <button type="submit" className="btn btn-primary me-3">
            Submit
          </button>
          <a href="/signup">Create Account</a>
        </div>
      </form>
    </>
  );
};
