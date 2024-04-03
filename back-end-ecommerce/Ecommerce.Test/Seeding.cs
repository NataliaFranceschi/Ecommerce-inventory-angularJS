public class Seeding
{
    public static void InitializeTestDB(AppDbContext db)
    {
        db.Users.Add(new User { Email = "teste@teste.com", Password = "teste" });
        db.SaveChanges();
    }

}
