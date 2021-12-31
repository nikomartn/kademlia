using BinaryStringLib;
using Kademlia.Domain.Buckets.Contracts;
using Kademlia.Domain.Interfaces.Client;
using Kademlia.Domain.Models;
using Logger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCPLayer;

namespace Kademlia.Infraestructure.Client
{
    public class TCPClient : IClient
    {
        private readonly ILogger<TCPClient> logger;

        public TCPClient(ILogger<TCPClient> logger)
        {
            this.logger = logger;
        }

        public async Task<Contact[]> FindNodes(Contact sender, Contact contact, BinaryString id, CancellationToken cancellationToken)
        {
            var client = new TaskBasedTcpClient(contact.Ip, contact.Port, Encoding.ASCII, 3);
            try
            {
                logger.LogInfo($"Requested findNodes to {contact} key:{id}");
                var response = (await client.Communicate($"FIND_NODE;{sender};{id.StringHex}", cancellationToken)).Split(';');
                List<Contact> contacts = new List<Contact>();
                logger.LogInfo($"Received nodes: {contacts}");
                for (int i = 1; i < response.Length; i++)
                {
                    contacts.Add(new Contact(response[i]));
                }
                return contacts.ToArray();
            } catch (Exception)
            {
                logger.LogError($"{contact} did not respnd.");
                return null;
            }
        }

        public async Task<FoundResult> FindValue(Contact sender, Contact contact, BinaryString id, CancellationToken cancellationToken)
        {
            logger.LogInfo($"Requested findValue to {contact} key:{id}");
            var client = new TaskBasedTcpClient(contact.Ip, contact.Port, Encoding.ASCII, 3);
            try
            {
                var result = (await client.Communicate($"FIND_VALUE;{sender};{id.StringHex}", cancellationToken)).Split(';');
                if (result[1] == "VALUE")
                {
                    return new FoundResult() { Data = new Domain.Database.Contracts.Tuple() { Key = id, Value = result[2] }, Type = FoundResult.FoundType.FOUND };
                }
                List<Contact> contacts = new List<Contact>();
                for (int i = 2; i < result.Length; i++)
                {
                    contacts.Add(new Contact(result[i]));
                }
                return new FoundResult() { Type = FoundResult.FoundType.FOUND_NODES, Contacts = contacts.ToArray() };
            } catch (Exception)
            {
                logger.LogError($"{contact} did not respond.");
                return null;
            }
        }

        public async Task<bool> MakePing(Contact sender, Contact contact, CancellationToken cancellationToken)
        {
            logger.LogInfo($"Requested ping to {contact}");
            var client = new TaskBasedTcpClient(contact.Ip, contact.Port, Encoding.ASCII, 3);
            try
            {
                var result = await client.Communicate($"PING;{sender}", cancellationToken);
                return result == "PONG";
            } catch (Exception)
            {
                logger.LogError($"{contact} did not respond.");
                return false;
            }
        }

        public async Task<Contact> RequestIdentification(string ip, int port, CancellationToken cancellationToken)
        {
            logger.LogInfo($"Requested identification to {ip}:{port}");
            var client = new TaskBasedTcpClient(ip, port, Encoding.ASCII, 3);
            try
            {
                var response = (await client.Communicate("IDENTIFY", cancellationToken)).Split(';');
                return new Contact(response[1]);
            } catch (Exception)
            {
                logger.LogError($"{ip}:{port} did not respond.");
                return null;
            }
        }

        public async Task Store(Contact sender, Contact contact, Domain.Database.Contracts.Tuple tuple, CancellationToken cancellationToken)
        {
            logger.LogInfo($"Requested store to {contact} tuple:{tuple}");
            var client = new TaskBasedTcpClient(contact.Ip, contact.Port, Encoding.ASCII, 3);
            try
            {
                var response = (await client.Communicate($"STORE;{sender};{tuple}", cancellationToken));
            } catch (Exception)
            {
                logger.LogError($"{contact} did not respond.");
            }
        }
    }
}
