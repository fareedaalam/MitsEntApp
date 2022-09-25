using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public IOtpRepository OtpRepository
        {
            get => new OtpRepository(_context,_mapper);
            set => new OtpRepository(_context,_mapper);
        }

        // public IUserRepository UserRepository => new UserRepository(_context,_mapper);
        // public IMessageRepository MessageRepository => new MessageRepository(_context,_mapper);
        //  public ILikesRepository LikesRepository => new LikeRepositoty(_context);

        IUserRepository IUnitOfWork.UserRepository
        {
            get => new UserRepository(_context, _mapper);
            set => new UserRepository(_context, _mapper);
        }
        IMessageRepository IUnitOfWork.MessageRepository
        {
            get => new MessageRepository(_context, _mapper);
            set => new MessageRepository(_context, _mapper);
        }
        ILikesRepository IUnitOfWork.LikesRepository
        {
            get => new LikeRepositoty(_context);
            set => new LikeRepositoty(_context);
        }

        //  IOtpRepository IUnitOfWork.OtpRepository
        // {
        //     get => new OtpRepository(_context);
        //     set => new OtpRepository(_context);
        // }

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}