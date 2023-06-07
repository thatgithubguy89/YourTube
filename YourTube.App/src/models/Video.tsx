import { User } from "./User";

export interface Video {
  id: number;
  title: String;
  videoUrl: String;
  liked: number;
  disliked: number;
  userId: number;
  views: number;
  user: User;
  comments: Comment[];
  createdBy: String;
  createTime: Date;
  lastEditTime: Date;
}
