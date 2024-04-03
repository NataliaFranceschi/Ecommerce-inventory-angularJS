using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly IAppDbContext _context;

    public UserRepository(IAppDbContext context)
    {
        _context = context;
    }

    public User Login(LoginDTO login)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == login.Email
                                                 && u.Password == login.Password);
        if (user == null)
        {
            throw new Exception("Senha ou e-mail incorreto.");
        }

        return user;

    }
}
