import sqlite3
import datetime

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


def rename_column():
    connection = sqlite3.connect(database=db_path)

    cursor = connection.cursor()

    with open("rename.sql", "r") as file:
        script = file.read()
        cursor.execute(script)
    
    connection.commit()
    connection.close()


def insert_data(session_start_time, session_end_time, total_session_duration, tracked_directory: str, total_active_time, total_afk_time, total_times_afk: int, total_events: int, total_creations: int, total_deletions: int, total_renamings: int, total_errors: int, session_was_closed_by_systemevent: bool, default_afk_startlimit_in_miliseconds) -> float:
    connection = sqlite3.connect(database=db_path)

    cursor = connection.cursor()

    # Get the template for replacing
    with open("insert_template.sql", "r") as file:
        script = file.read()
    
    # Insert the values for the sql
    new_sql_query = script.replace("{0}", str(session_start_time)).replace("{1}", str(session_end_time)).replace("{2}", str(total_session_duration)).replace("{3}", str(tracked_directory)).replace("{4}", str(total_active_time)).replace("{5}", str(total_afk_time)).replace("{6}", str(total_times_afk)).replace("{7}", str(total_events)).replace("{8}", str(total_creations)).replace("{9}", str(total_deletions)).replace("{10}", str(total_renamings)).replace("{11}", str(total_errors)).replace("{12}", str(session_was_closed_by_systemevent)).replace("{13}", str(default_afk_startlimit_in_miliseconds))

    # Write the new file
    with open("insert_data.sql", "w") as file:
        file.write(new_sql_query)

    cursor.execute(new_sql_query)

    connection.commit()
    connection.close()
