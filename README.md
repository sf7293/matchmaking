# Assumptions
I've considered that:
- A session is where players, ranging from 2 to 10, can compete against each other.
- A player can participate in only one session at a time until it concludes.
- To join a new session, players must leave their current contest.
- A session's contest begins at a specified timestamp (if there are at least two players) and ends after a designated duration from the start time.
- I have considered that the QueuePlayerAsync method will return error sometimes, so it cannot return Session at everytime. So I changed the method response from Task<Session> to Task<IActionResult> 
# Code Design and Architecture

# Diagrams