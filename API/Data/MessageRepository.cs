using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MessageRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages
                    .Include(u => u.Sender)
                    .Include(u => u.Recipient)
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                    .OrderByDescending(m => m.MessageSent)
                    .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                    .AsQueryable();
            //get the message where to read the msg by switch case
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username
                            && u.RecipientDeleted == false),
                "Outbos" => query.Where(u => u.SenderUsername == messageParams.Username
                            && u.SenderDeleted == false),
                _ => query.Where(u => u.RecipientUsername ==
                    messageParams.Username && u.RecipientDeleted == false && u.DateRead == null)

            };

            //var message = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);
            return await PagedList<MessageDto>.CreateAsync(query, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            //Get the all message
            var messages = await _context.Messages
                //  .Include(u => u.Sender).ThenInclude(p => p.Photos)
                //  .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                 .Where(m => m.Recipient.UserName == currentUsername && m.RecipientDeleted == false
                     && m.Sender.UserName == recipientUsername
                     || m.Recipient.UserName == recipientUsername
                     && m.Sender.UserName == currentUsername && m.SenderDeleted == false
                     )
                     .OrderBy(m => m.MessageSent)
                     .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                     .ToListAsync();
            //Check unread messages                     
            var unreadMessage = messages.Where(m => m.DateRead == null &&
                    m.RecipientUsername == currentUsername).ToList();

            if (unreadMessage.Any())
            {
                foreach (var msg in unreadMessage)
                {
                    msg.DateRead = DateTime.Now;
                }
                //Set nnread messge into read
                await _context.SaveChangesAsync();
            }
            return messages;
        }

        // public async Task<bool> SaveAllAsync()
        // {
        //     return await _context.SaveChangesAsync() > 0;
        // }
    }
}