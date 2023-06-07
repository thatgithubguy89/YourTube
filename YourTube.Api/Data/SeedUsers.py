import random

path = "/profileimages/stock.jpg"
password = "$2a$11$qcR.dkXOV.SfPDiAo3.T1ep/1qWpveC9QGAeBZ6n5SVTEkKAoWPJe"
month = f"{random.randint(1, 9)}"
day = f"{random.randint(1, 9)}"
year = 2023
users = ['john@gmail.com', 'jane@gmail.com', 'jerry@gmail.com', 'sue@gmail.com', 'bill@gmail.com',
         'donna@gmail.com', 'bob@gmail.com', 'sally@gmail.com', 'rick@gmail.com', 'june@gmail.com']


def seed_users():
    print(
        f"INSERT INTO [Users] ([Username], [Password], [ProfileImageUrl], [CreateTime], [LastEditTime]) VALUES")

    for i in range(0, len(users)):
        print(
            f"('{users[i]}', '{password}', '{path}', '{year}/0{month}/0{day}', '{year}/0{month}/0{day}')" +
            f"{';' if i == (len(users) - 1) else ','}")


seed_users()
