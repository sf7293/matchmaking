using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rovio.MatchMaking.Repositories;
using Rovio.MatchMaking.Repositories.Data;

namespace Rovio.MatchMaking.Console.Services
{
    public class SessionMatchmaker
    {
        private readonly AppDbContext _context;
        private readonly ISessionRepository _sessionRepository;
        private readonly IQueuedPlayerRepository _queuedPlayerRepository;

        public SessionMatchmaker(AppDbContext context, IQueuedPlayerRepository queuedPlayerRepository, ISessionRepository sessionRepository)
        {
            _context = context;
            _queuedPlayerRepository = queuedPlayerRepository;
            _sessionRepository = sessionRepository;
        }

        public async Task RunAsync()
        {
            // Step 1: Read Players from Queues and Create a HashMap
            var queuedPlayersMap = await CreateQueuedPlayersMapAsync();

            // Step 2: Read Active Sessions and Create a HashMap
            var activeSessionsMap = await CreateActiveSessionsMapAsync();

            // Step 3: Try to Add Players to Active Sessions
            foreach (var latencyLevel in queuedPlayersMap.Keys)
            {
                if (activeSessionsMap.ContainsKey(latencyLevel))
                {
                    foreach (var queuedPlayer in queuedPlayersMap[latencyLevel])
                    {
                        var addedToSession = await TryAddPlayerToActiveSessionAsync(latencyLevel, queuedPlayer, activeSessionsMap);
                        if (!addedToSession) /// ????
                        {
                            break;
                        }
                        // TODO: notifying the user about the matching process result success
                        // It could be done through Socket or Push Notification
                    }
                }
            }

            // Step 4: Create New Sessions if Necessary
            foreach (var latencyLevel in queuedPlayersMap.Keys)
            {
                var remainingQueuedPlayers = queuedPlayersMap[latencyLevel];
                while (remainingQueuedPlayers.Count >= 2)
                {
                    var sessionSize = remainingQueuedPlayers.Count >= 10 ? 10 : remainingQueuedPlayers.Count;
                    
                    using var transaction = await _context.Database.BeginTransactionAsync();
                    try {
                        var newSession = await _sessionRepository.CreateNewAsync(latencyLevel, 0, 30);

                        for (int i = 0; i < sessionSize; i++)
                        {
                            var queuedPlayer = remainingQueuedPlayers[i];
                            // I have considered that the whole matching module is called by another service, so the existence of player and validity of playerId had been checked in other modules before 
                            await _sessionRepository.AddPlayerToSessionAsync(newSession.Id, queuedPlayer.PlayerId);
                            await _queuedPlayerRepository.DeleteQueuedPlayerAsync(queuedPlayer.Id);
                            //INJA
                            // await _sessionRepository.Remove(newSession.Id, playerId);
                            //TODO: removed QueuedPlayer
                        }

                        //TODO: notify players for whom session is created
                        remainingQueuedPlayers.RemoveRange(0, sessionSize);

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    } catch (Exception ex) {
                        // Rollback the transaction if any operation fails
                        await transaction.RollbackAsync();
                        System.Console.WriteLine($"An error occurred during creating new session: PlayerIDs={string.Join(", ", remainingQueuedPlayers.GetRange(0, sessionSize))}, LatencyLevel={latencyLevel}, Error={ex.Message}");
                        Environment.Exit(1);
                    }
                }
            }

            System.Console.WriteLine("Matchmaking process completed successfully!");
        }

        private async Task<Dictionary<int, List<QueuedPlayer>>> CreateQueuedPlayersMapAsync()
        {
            //TODO: move this part to repoe
            var playersQueue = await _context.QueuedPlayers
                .OrderBy(qp => qp.CreatedAt) // Order by queueing time
                .ToListAsync();

            var queudPlayersMap = new Dictionary<int, List<QueuedPlayer>>();

            foreach (var queuedPlayer in playersQueue)
            {
                if (!queudPlayersMap.ContainsKey(queuedPlayer.LatencyLevel))
                {
                    queudPlayersMap[queuedPlayer.LatencyLevel] = new List<QueuedPlayer>();
                }
                queudPlayersMap[queuedPlayer.LatencyLevel].Add(queuedPlayer);
            }

            return queudPlayersMap;
        }

        private async Task<Dictionary<int, List<Session>>> CreateActiveSessionsMapAsync()
        {
            //TODO: move this part to repoe
            var activeSessions = await _context.Sessions
                .Where(s => s.JoinedCount < 10) // Sessions with less than 10 players
                .OrderBy(s => s.CreatedAt) // Order by creation time
                .ToListAsync();

            var activeSessionsMap = new Dictionary<int, List<Session>>();

            foreach (var session in activeSessions)
            {
                if (!activeSessionsMap.ContainsKey(session.LatencyLevel))
                {
                    activeSessionsMap[session.LatencyLevel] = new List<Session>();
                }
                activeSessionsMap[session.LatencyLevel].Add(session);
            }

            return activeSessionsMap;
        }

        private async Task<bool> TryAddPlayerToActiveSessionAsync(int latencyLevel, QueuedPlayer queuedPlayer, Dictionary<int, List<Session>> activeSessionsMap)
        {
            //TODO: fix this! return or no return!!!!
            // I have considered that the whole matching module is called by another service, so the existence of player and validity of playerId had been checked in other modules before 
            var sessions = activeSessionsMap[latencyLevel];
            foreach (var session in sessions)
            {
                if (session.JoinedCount < 10)
                {
                    using var transaction = await _context.Database.BeginTransactionAsync();
                    try {
                        await _sessionRepository.AddPlayerToSessionAsync(session.Id, queuedPlayer.PlayerId);
                        session.JoinedCount++;
                        _context.Sessions.Update(session);
                        await _queuedPlayerRepository.DeleteQueuedPlayerAsync(queuedPlayer.Id);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    } catch (Exception ex)
                    {
                        // Rollback the transaction if any operation fails
                        await transaction.RollbackAsync();
                        System.Console.WriteLine($"An error occurred during adding player to session: PlayerId={queuedPlayer.PlayerId}, LatencyLevel={latencyLevel}, SessionId={session.Id}, Error={ex.Message}");
                    }
                }
            }

            return false; // No available session for the player
        }
    }
}
