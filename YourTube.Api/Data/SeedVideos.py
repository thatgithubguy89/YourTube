import random

number_of_users = 10
user_ids = []
for i in range(1, number_of_users + 1):
    user_ids.append(i)


def seed_videos(number_of_items: int):
    id = 1
    path = "/videos/stock.mp4"
    year = 2023

    print(
        f"INSERT INTO [Videos] ([Title], [VideoUrl], [Liked], [Disliked], [Views], [UserId], [CreateTime], [LastEditTime]) VALUES ")

    for _ in range(number_of_items):
        month = f"{random.randint(1, 9)}"
        day = f"{random.randint(1, 9)}"

        print(f"('Video {id}', " +
              f"'{path}', " +
              f"0, " +
              f"0, " +
              f"0, " +
              f"{user_ids[random.randint(0, number_of_users - 1)]}, " +
              f"'{year}/0{month}/0{day}', " +
              f"'{year}/0{month}/0{day}')" +
              f"{';' if id == number_of_items else ','}")

        id += 1


seed_videos(100)
