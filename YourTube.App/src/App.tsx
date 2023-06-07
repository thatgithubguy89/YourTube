import "./App.css";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import { Home } from "./pages/Home";
import { Signin } from "./pages/Signin";
import { SignUp } from "./pages/SignUp";
import { UserProfile } from "./pages/UserProfile";
import { AddVideo } from "./pages/AddVideo";
import { VideoDetail } from "./pages/VideoDetail";
import { NavBar } from "./components/navigation/NavBar";

function App() {
  return (
    <div>
      <BrowserRouter>
        <NavBar />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/userprofile" element={<UserProfile />} />
          <Route path="/signin" element={<Signin />} />
          <Route path="/signup" element={<SignUp />} />
          <Route path="/addvideo/:userid" element={<AddVideo />} />
          <Route path="/videodetail/:id" element={<VideoDetail />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
