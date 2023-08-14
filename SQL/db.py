import sqlite3

db_path = "entries.db"


def cursor():
    connection = sqlite3.connect(database=db_path)

    return connection.cursor()


def create_table():
    connection = sqlite3.connect(database=db_path)

    cursor = connection.cursor()

    with open("Table_SessionInfo.sql", "r") as file:
        script = file.read()
        cursor.execute(script)

    connection.commit()
    connection.close()


def create_database():
    connection = sqlite3.connect(database=db_path)

    cursor = connection.cursor()

    with open("Create_db.sql", "r") as file:
        script = file.read()
        cursor.execute(script)

    connection.commit()
    connection.close()


def drop_table():
    connection = sqlite3.connect(database=db_path)

    cursor = connection.cursor()

    cursor.execute("Drop table SessionInfo")

    connection.commit()
    connection.close()
