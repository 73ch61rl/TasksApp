using EntityFrameworkCoreMock;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasksApp.Server.Controllers;
using TasksApp.Server.Data;
using Task = TasksApp.Server.Models.Task;

namespace TasksApp.Server.Test
{
    [Trait("ApiTests", "Unit")]
    public class ApiTests
    {
        private DbContextMock<TasksDbContext> GetDbContext(Task[] tasks)
        {
            DbContextMock<TasksDbContext> dbContextMock = new(new DbContextOptionsBuilder<TasksDbContext>().Options);
            dbContextMock.CreateDbSetMock(x => x.Tasks, tasks);
            return dbContextMock;
        }

        private TasksController TasksControllerInit(DbContextMock<TasksDbContext> dbContextMock)
        {
            return new TasksController(dbContextMock.Object);
        }

        private Task[] GetInitialDbEntities()
        {
            return
             [
                new Task {Id = 1, Name="Test1", Done=true},
                new Task {Id = 2, Name="Test2", Done=false},
                new Task {Id = 3, Name="Test3", Done=true}
            ];
        }

        [Fact]
        public void GetTasks_ReturnsAllTasksSuccessfully()
        {
            DbContextMock<TasksDbContext> dbContextMock = GetDbContext(GetInitialDbEntities());
            TasksController tasksController = TasksControllerInit(dbContextMock);

            ActionResult<IEnumerable<Task>> result = tasksController.GetTasks().Result;
            List<Task>? value = result.Value as List<Task>;

            Assert.Equal(3, value?.Count);
        }

        [Fact]
        public void GetTask_CorrectId_ReturnsCorrectResult()
        {
            DbContextMock<TasksDbContext> dbContextMock = GetDbContext(GetInitialDbEntities());
            TasksController tasksController = TasksControllerInit(dbContextMock);

            ActionResult<Task> result = tasksController.GetTask(1).Result;
            Task? value = result.Value;

            Assert.IsType<Task>(value);
            Assert.Equal(1, value.Id);
        }

        [Fact]
        public void GetTask_InvalidId_ReturnsNotFound()
        {
            DbContextMock<TasksDbContext> dbContextMock = GetDbContext(GetInitialDbEntities());
            TasksController tasksController = TasksControllerInit(dbContextMock);

            ActionResult? result = tasksController.GetTask(4).Result.Result;

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void PutTask_InvalidInput_ReturnsBadRequest()
        {
            DbContextMock<TasksDbContext> dbContextMock = GetDbContext(GetInitialDbEntities());
            TasksController tasksController = TasksControllerInit(dbContextMock);
            Task tobeUpdated = GetInitialDbEntities()[2];
            int id = 2;

            IActionResult result = tasksController.PutTask(id, tobeUpdated).Result;

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void DeleteTask_CorrectId_RemovesTaskSuccessfully()
        {
            DbContextMock<TasksDbContext> dbContextMock = GetDbContext(GetInitialDbEntities());
            TasksController tasksController = TasksControllerInit(dbContextMock);
            int id = 3;

            IActionResult result = tasksController.DeleteTask(id).Result;

            Assert.IsType<NoContentResult>(result);
            Assert.Null(dbContextMock.Object.Tasks.Find(id));
        }

        [Fact]
        public void DeleteTask_InvalidId_ReturnsNotFound()
        {
            DbContextMock<TasksDbContext> dbContextMock = GetDbContext(GetInitialDbEntities());
            TasksController tasksController = TasksControllerInit(dbContextMock);
            int id = 4;

            IActionResult result = tasksController.DeleteTask(id).Result;

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void PostTask_NewId_AddsSuccessfully()
        {
            DbContextMock<TasksDbContext> dbContextMock = GetDbContext(GetInitialDbEntities());
            TasksController tasksController = TasksControllerInit(dbContextMock);
            Task taskToBeAdded = new() { Id = 4, Name = "Test4", Done = false };
            _ = await tasksController.PostTask(taskToBeAdded);

            Assert.Equal(taskToBeAdded, dbContextMock.Object.Tasks.Find(taskToBeAdded.Id));
        }

        [Fact]
        public async void PostTask_ReturnsCreatedAtActionResult()
        {
            DbContextMock<TasksDbContext> dbContextMock = GetDbContext(GetInitialDbEntities());
            TasksController tasksController = TasksControllerInit(dbContextMock);
            Task taskToBeAdded = new() { Id = 4, Name = "Test4", Done = false };

            ActionResult<Task> result = await tasksController.PostTask(taskToBeAdded);

            ActionResult<Task> actionResult = Assert.IsType<ActionResult<Task>>(result);
            CreatedAtActionResult createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Task returnValue = Assert.IsType<Task>(createdAtActionResult.Value);
            Assert.Equal(taskToBeAdded, returnValue);
        }

    }
}