-- Please note: This SQL File ist just a template for python to replace the values.

INSERT INTO SessionInfo
(
    SessionID,
    SessionStartTime ,
    SessionEndTime ,
    TotalSessionDuration ,
    TrackedDirectory ,
    TotalActiveTime ,
    TotalAFKTime ,
    TotalTimesAFK ,
    TotalEvents ,
    TotalCreations ,
    TotalDeletions ,
    TotalRenamings ,
    TotalErrors ,
    SessionWasClosedBySystemEvent ,
    DefaultAFKStartLimitInMiliseconds ,
    TotalFileChanges
) VALUES
-- Auto incremented automatically
(NULL, 

-- SessionStartTime : DATETIME
("09-09-2023 12:29:35") , 

-- SessionEndTime : DATETIME
("09-09-2023 12:29:48") , 

-- TotalSessionDuration : DATETIME
("00:00:13-6644081") ,

-- TrackedDirectory : TEXT
"C:\ " ,

-- TotalActiveTime : DATETIME
("00:00:13-6644081") ,

-- TotalAFKTime : DATETIME
("00:00:00") ,

-- TotalTimesAFK : INT
0 ,

-- TotalEvents : INT
328 ,

-- TotalCreations : INT
41 ,

-- TotalDeletions : INT
38 ,

-- TotalRenamings : INT
35 ,

-- TotalErrors : INT
0 ,

-- SessionWasClosedBySystemEvent : BOOL/TINYINT
0 ,

-- DefaultAFKStartLimitInMiliseconds : LONG
600000 ,

-- TotalFileChanges : INT
214 );

