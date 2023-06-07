import { Video } from "./Video";

export interface User {
  id: number;
  username: String;
  password: String;
  profileImageUrl: String;
  videos: Video[];
  createTime: Date;
  lastEditTime: Date;
}
