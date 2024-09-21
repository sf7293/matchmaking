# Assumptions
I've considered that:
- A session is where players, ranging from 2 to 10, can compete against each other.
- A player can participate in only one session at a time until it concludes.
- To join a new session, players must leave their current contest.
- A session's contest begins at a specified timestamp (if there are at least two players) and ends after a designated duration from the start time.
- I have considered that the QueuePlayerAsync method will return error sometimes, so it cannot return Session at everytime. So I changed the method response from Task<Session> to Task<IActionResult> 

# Database Schema
### Sessions Table

| Column Name   | Data Type    | Constraints           | Description                     |
|---------------|--------------|-----------------------|---------------------------------|
| `Id`          | `UUID`       | `PRIMARY KEY`         | Unique identifier for each session |
| `Name`        | `VARCHAR(50)`| `NOT NULL`            | Name of the player              |
| `LatencyLevel`  | `INT`        | `CHECK(LatencyLevel >= 1)` | Latency level of the player (1 to 5) |
| `JoinedCount`  | `INT`        | `DEFAULT 0 CHECK(JoinedCount <= 10)` | Number of players joined the session |
| `CreatedAt`   | `DATETIME`   | `DEFAULT CURRENT_TIMESTAMP` | Timestamp when the record was created |
| `StartsAt`   | `DATETIME`   | `` | Timestamp when the contest will start |
| `EndsAt`   | `DATETIME`   | `` | Timestamp when the contest will end |


# Code Design and Architecture

# Diagrams