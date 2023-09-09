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
{0} , 

-- SessionEndTime : DATETIME
{1} , 

-- TotalSessionDuration : DATETIME
{2} ,

-- TrackedDirectory : TEXT
{3} ,

-- TotalActiveTime : DATETIME
{4} ,

-- TotalAFKTime : DATETIME
{5} ,

-- TotalTimesAFK : INT
{6} ,

-- TotalEvents : INT
{7} ,

-- TotalCreations : INT
{8} ,

-- TotalDeletions : INT
{9} ,

-- TotalRenamings : INT
{10} ,

-- TotalErrors : INT
{11} ,

-- SessionWasClosedBySystemEvent : BOOL/TINYINT
{12} ,

-- DefaultAFKStartLimitInMiliseconds : LONG
{13} ,

-- TotalFileChanges : INT
{14} );

