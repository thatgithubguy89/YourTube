import { Player } from "../components/videodetail/Player";
import { RecommendedList } from "../components/videodetail/RecommendedList";

export const VideoDetail = () => {
  return (
    <div>
      <div className="container d-flex mt-5">
        <Player />
        <RecommendedList />
      </div>
    </div>
  );
};
