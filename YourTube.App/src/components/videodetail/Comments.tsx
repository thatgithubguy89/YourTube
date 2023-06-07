import { Comment } from "../../models/Comment";
import { CommentForm } from "./CommentForm";

interface Props {
  videoId: number;
  comments: Comment[];
}

export const Comments = ({ videoId, comments }: Props) => {
  return (
    <div className="mt-5">
      <div>
        <span>{comments.length} Comments</span>
      </div>
      <CommentForm videoId={videoId} />
      <div className="mt-5">
        {comments.map((comment) => (
          <div key={comment.id} className="mt-5">
            <p>
              {comment.username} -{" "}
              {comment.createTime.toString().substring(0, 10)}
            </p>
            <p>{comment.content}</p>
          </div>
        ))}
      </div>
    </div>
  );
};
