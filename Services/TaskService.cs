using SharedTaskBoardlesson.Models;

namespace SharedTaskBoardlesson.Services;

public class TaskService
{
    private readonly List<TaskItem> _tasks = [];
    private readonly object _lock = new();

    public event Action? OnChange;

    public List<TaskItem> GetAll()
    {
        lock (_lock)
        {
            return _tasks.Select(task => new TaskItem
            {
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt
            }).ToList();
        }
    }

    public void Add(string title)
    {
        lock (_lock)
        {
            _tasks.Add(new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = title,
                IsCompleted = false,
                CreatedAt = DateTime.Now
            });
        }

        OnChange?.Invoke();
    }

    public void Toggle(Guid id)
    {
        lock (_lock)
        {
            var task = _tasks.FirstOrDefault(item => item.Id == id);
            if (task is null)
            {
                return;
            }

            task.IsCompleted = !task.IsCompleted;
        }

        OnChange?.Invoke();
    }

    public void Delete(Guid id)
    {
        lock (_lock)
        {
            _tasks.RemoveAll(task => task.Id == id);
        }

        OnChange?.Invoke();
    }

    public TaskItem? GetById(Guid id)
    {
        lock (_lock)
        {
            var task = _tasks.FirstOrDefault(item => item.Id == id);
            return task is null ? null : new TaskItem
            {
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt
            };
        }
    }
}
