public class Seeder
{
    public static void SeedUserAdmin(IAppDbContext _context)
    {
        try
        {
            var usersCount = _context.Users.Where(u => u.UserType == "admin").Count();
            if (usersCount == 0)
            {
                _context.Users.Add(new User { Email = "admin@admin.com", Password = "admin", UserType = "admin" });
                _context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            Console.Write(ex.ToString());
        }
    }
}