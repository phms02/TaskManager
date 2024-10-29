using Microsoft.EntityFrameworkCore;

internal class TaskManagerContext : DbContext {
    internal TaskManagerContext(DbContextOptions<TaskManagerContext> options)
        : base(options) { }

    public DbSet<Person> Persons { get; set; }
    public DbSet<Task> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if(!optionsBuilder.IsConfigured) {
            optionsBuilder.UseNpgsql("Host=localhost;Database=task_manager;Username=yourusername;Password=yourpassword");
        }
    }
}