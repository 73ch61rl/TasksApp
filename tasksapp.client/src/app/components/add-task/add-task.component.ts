import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TaskListService } from '../../services/task-list.service';
import { Task } from '../../models/task.model';

@Component({
  selector: 'app-add-task',
  templateUrl: './add-task.component.html'
})
export class AddTaskComponent {
  newTask: Task = {
    id: '',
    name: '',
    done: false
  };

  constructor(
    private taskListService: TaskListService,
    private router: Router
  ) { }

  addTask() {
    this.taskListService.addTask(this.newTask).subscribe({
      next: () => {
        this.router.navigate(['tasks']);
      },
      error: (response) => {
        console.log(response);
      },
    });
  }
}
