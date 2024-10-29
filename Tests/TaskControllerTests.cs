using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.Controllers;
using Xunit;

namespace TaskManager.Tests;

public class TaskControllerTests {
    private readonly TaskController _controller;
    private readonly Mock<TaskManagerContext> _mockContext;
    private DbContextOptions<TaskManagerContext> _options;

    public TaskControllerTests() {
        _options = new DbContextOptionsBuilder<TaskManagerContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _mockContext = new Mock<TaskManagerContext>();
        _controller = new TaskController(_mockContext.Object);
    }

    public class TaskControllerTests {
        private readonly TaskController _controller;
        private readonly Mock<TaskManagerContext> _mockContext;

        public TaskControllerTests() {
            _mockContext = new Mock<TaskManagerContext>();
            _controller = new TaskController(_mockContext.Object);
        }

        [Fact]
        public async Task GetTasks_ReturnsAllTasks() {
            // Arrange
            var tasks = new List<Task> {
                new Task { Id = 1, Title = "Task 1" },
                new Task { Id = 2, Title = "Task 2" }
            };

            _context.Tasks.AddRange(tasks);
            _context.SaveChanges();

            // Act
            var result = await _controller.GetTasks();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Task>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Task>>(okResult.Value);
            Assert.Equal(tasks.Count, returnValue.Count());
        }

        [Fact]
        public async Task GetTask_ReturnsTask_WhenTaskExists() {
            // Arrange
            var task = new Task { Id = 1, Title = "Task 1" };

            _context.Tasks.Add(task);
            _context.SaveChanges();

            // Act
            var result = await _controller.GetTask(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Task>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<Task>(okResult.Value);
            Assert.Equal(task.Id, returnValue.Id);
        }

        [Fact]
        public async Task GetTask_ReturnsNotFound_WhenTaskDoesNotExist() {
            // Act
            var result = await _controller.GetTask(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Task>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
    }

    private static DbSet<T> MockDbSet<T>(List<T> list) where T : class {
        var queryable = list.AsQueryable();
        var dbSet = new Mock<DbSet<T>>();

        dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

        dbSet.As<IEnumerable<T>>().Setup(m => m.GetEnumerator()).Returns(() => list.GetEnumerator());

        return dbSet.Object;
    }
}