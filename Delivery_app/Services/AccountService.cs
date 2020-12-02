using AutoMapper;
using Delivery_app.Entities;
using Delivery_app.Helpers;
using Delivery_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_app.Services
{
    public interface IAccountService
    {
        UserModel Authenticate(string username, string password);
        IEnumerable<UserModel> GetAll();
        UserModel GetById(int id);
        Users Create(Users user);
        void Update(Users user, string password = null);
        void Delete(int id);
        Couriers GetCourierById(int id);
    }

    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AccountService(AppDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public UserModel Authenticate(string phone_num, string password)
        {
            if (string.IsNullOrEmpty(phone_num) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.users.SingleOrDefault(x => x.phone_num == phone_num);

            // check if user exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, Convert.FromBase64String(user.password), Convert.FromBase64String(user.password_salt)))
                return null;

            return _mapper.Map<UserModel>(user);
        }

        public Users Create(Users user)
        {
            if (string.IsNullOrWhiteSpace(user.password))
            {
                return null;
            }

            if (_context.users.Any(u => u.phone_num == user.phone_num))
            {
                throw new AppException("Phone number \"" + user.phone_num + "\" is already exist");
            }

            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(user.password, out passwordHash, out passwordSalt);

            user.password = Convert.ToBase64String(passwordHash);
            user.password_salt = Convert.ToBase64String(passwordSalt);

            _context.users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Delete(int id)
        {
            var user = _context.users.Find(id);
            if (user != null)
            {
                _context.users.Remove(user);
                _context.SaveChanges();
            }
        }

        public IEnumerable<UserModel> GetAll()
        {
            var user = _context.users.ToList();
            return user.Select((_) => _mapper.Map<UserModel>(user));
        }

        public UserModel GetById(int id)
        {
            var u = _context.users.Find(id);

            if (u == null)
            {
                return null;
            }

            return _mapper.Map<UserModel>(u);
        }

        public void Update(Users user, string password = null)
        {
            throw new NotImplementedException();
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public Couriers GetCourierById(int id)
        {
            var c = _context.couriers.Find(id);

            if (c == null)
            {
                return null;
            }

            return c;
        }
    }
}
