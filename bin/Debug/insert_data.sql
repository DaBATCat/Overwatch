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
    DefaultAFKStartLimitInMiliseconds 
) VALUES
-- Auto incremented automatically
(NULL, 

-- SessionStartTime : DATETIME
("05-09-2023 15:09:44") , 

-- SessionEndTime : DATETIME
("05-09-2023 15:11:03") , 

-- TotalSessionDuration : DATETIME
("00:01:19-4293239") ,

-- TrackedDirectory : TEXT
"C:\ " ,

-- TotalActiveTime : DATETIME
("00:01:19-4293239") ,

-- TotalAFKTime : DATETIME
("00:00:00") ,

-- TotalTimesAFK : INT
0 ,

-- TotalEvents : INT
532 ,

-- TotalCreations : INT
38 ,

-- TotalDeletions : INT
38 ,

-- TotalRenamings : INT
35 ,

-- TotalErrors : INT
0 ,

-- SessionWasClosedBySystemEvent : BOOL/TINYINT
0 ,

-- DefaultAFKStartLimitInMiliseconds : LONG
600000 );

