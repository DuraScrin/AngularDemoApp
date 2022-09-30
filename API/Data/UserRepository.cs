using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext contex;
        private readonly IMapper _mapper;
        public UserRepository(DataContext contex, IMapper mapper)
        {
            this._mapper = mapper;
            this.contex = contex;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await contex.Users
                    .Where(x => x.UserName == username)
                    .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await contex.Users
                    .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await contex.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await contex.Users
                        .Include(p => p.Photos)
                        .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await contex.Users
                        .Include(p => p.Photos)
                        .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            int affectedRecords = await contex.SaveChangesAsync();
            if (affectedRecords > 0)
                return true;
            else
                return false;
        }

        public void Update(AppUser user)
        {
            contex.Entry(user).State = EntityState.Modified;
        }
    }
}