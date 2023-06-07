import random

content = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Inventore quibusdam natus odio molestiae"

number_of_videos = 100
video_ids = []
for i in range(1, number_of_videos + 1):
    video_ids.append(i)

number_of_users = 10
user_ids = []
for i in range(1, number_of_users + 1):
    user_ids.append(i)


def seed_comments(number_of_items: int):
    id = 1
    year = 2023

    print(
        f"INSERT INTO [Comments] ([Content], [Username], [VideoId], [CreateTime], [LastEditTime]) VALUES ")

    for _ in range(number_of_items):
        month = f"{random.randint(1, 9)}"
        day = f"{random.randint(1, 9)}"

        print(f"('{content}', " +
              f"'john@gmail.com', " +
              f"{video_ids[random.randint(0, number_of_videos - 1)]}, " +
              f"'{year}/0{month}/0{day}', " +
              f"'{year}/0{month}/0{day}')" +
              f"{';' if id == number_of_items else ','}")

        id += 1


seed_comments(500)
