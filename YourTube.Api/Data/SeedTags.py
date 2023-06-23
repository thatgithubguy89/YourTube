import random


def seed_tags(number_of_items: int):
    videoId = 1
    year = 2023

    names = [["sports", "news", "fashion", "nature", "travel"],
             ["food", "family", "life", "fun", "style"],
             ["inspiration", "night", "cool", "business", "tech"]]

    print(
        f"INSERT INTO [Tags] ([Name], [VideoId], [CreateTime], [LastEditTime]) VALUES ")

    for _ in range(number_of_items):
        month = f"{random.randint(1, 9)}"
        day = f"{random.randint(1, 9)}"
        for i in range(3):
            print(f"('{names[i][random.randint(0, 4)]}', " +
                  f"{videoId}, " +
                  f"'{year}/0{month}/0{day}', " +
                  f"'{year}/0{month}/0{day}')" +
                  f"{';' if videoId == number_of_items and i == 2 else ','}")
        videoId += 1


seed_tags(100)
