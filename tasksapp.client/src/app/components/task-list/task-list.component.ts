import { Component, OnInit } from '@angular/core';
import { Task } from '../../models/task.model';
import { Router } from '@angular/router';
import { TaskListService } from '../../services/task-list.service';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html'
})
export class TaskListComponent implements OnInit {
  tasks: Task[] = [];

  constructor(
    private taskListService: TaskListService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.taskListService.getAllTasks().subscribe({
      next: (tasks) => {
        this.tasks = tasks;
      },
      error: (response) => {
        console.log(response);
      },
    });
  }

  deleteTask(id: string) {
    this.taskListService.deleteTask(id).subscribe({
      next: () => {
        let currentUrl = this.router.url;
        this.router
          .navigateByUrl('/', { skipLocationChange: true })
          .then(() => {
            this.router.navigate([currentUrl]);
          });
      }
    });
  }

}

