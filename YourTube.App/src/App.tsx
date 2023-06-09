import "./App.css";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import { Home } from "./pages/Home";
import { Signin } from "./pages/Signin";
import { SignUp } from "./pages/SignUp";
import { UserProfile } from "./pages/UserProfile";
import { AddVideo } from "./pages/AddVideo";
import { VideoDetail } from "./pages/VideoDetail";
import { NavBar } from "./components/navigation/NavBar";
import { Favorites } from "./pages/Favorites";
import { Trending } from "./pages/Trending";
import { SearchBox } from "./components/common/SearchBox";
import { NotFound } from "./components/common/NotFound";

function App() {
  return (
    <div>
      <BrowserRouter>
        <NavBar />
        <SearchBox />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/:searchphrase" element={<Home />} />
          <Route path="/userprofile" element={<UserProfile />} />
          <Route path="/favorites" element={<Favorites />} />
          <Route path="/signin" element={<Signin />} />
          <Route path="/signup" element={<SignUp />} />
          <Route path="/trending" element={<Trending />} />
          <Route path="/addvideo/:userid" element={<AddVideo />} />
          <Route path="/videodetail/:id" element={<VideoDetail />} />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
