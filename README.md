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
| `LatencyLevel`  | `INT`        | `CHECK(LatencyLevel >= 1)` | Latency level of the player (1 to 5) |
| `JoinedCount`  | `INT`        | `DEFAULT 0 CHECK(JoinedCount <= 10)` | Number of players joined the session |
| `CreatedAt`   | `DATETIME`   | `DEFAULT CURRENT_TIMESTAMP` | Timestamp when the record was created |
| `StartsAt`   | `DATETIME`   | `` | Timestamp when the contest will start |
| `EndsAt`   | `DATETIME`   | `` | Timestamp when the contest will end |

### Sessions_Players Table
Relation of Players and Sessions

| Column Name  | Data Type                     | Constraints                                 | Description                                   |
|--------------|-------------------------------|---------------------------------------------|-----------------------------------------------|
| `Id`         | `UUID`                        | `PRIMARY KEY`                               | Unique identifier for each session            |
| `Session_id` | `UUID`                        | `NOT NULL`                                  | ID of the session the player is attending     |
| `Player_id`  | `UUID`                        | `NOT NULL`                                  | ID of the player attending the session        |
| `Status`     | `ENUM('ATTENDED','PLAYED','LEFT')` | `DEFAULT 'ATTENDED'`                         | Status of the player in the session           |
| `Score`      | `INT`                         | `DEFAULT 0`                                 | Score of the player in the contest            |
| `CreatedAt`  | `DATETIME`                    | `DEFAULT CURRENT_TIMESTAMP`                 | Timestamp when the record was created         |
| `UpdatedAt`  | `DATETIME`                    | `DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP` | Timestamp when the record was last updated    |

## Queued_Players Table

This table contains players' requests to join a session.

| Column Name   | Data Type  | Constraints                             | Description                                             |
|---------------|------------|-----------------------------------------|---------------------------------------------------------|
| `Id`          | `UUID`     | `PRIMARY KEY`                           | Unique identifier for each request                      |
| `Player_id`   | `UUID`     | `UNIQUE`                                | ID of the player requesting to join a session           |
| `LatencyLevel`| `INT`      | `CHECK(LatencyLevel >= 1 AND LatencyLevel <= 5)` | Latency level of the player (1 to 5, where 1 is best)   |
| `CreatedAt`   | `DATETIME` | `DEFAULT CURRENT_TIMESTAMP`             | Timestamp when the request was created                  |


# Code Design and Architecture

# Diagrams