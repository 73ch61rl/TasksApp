import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Task } from '../../models/task.model';
import { TaskListService } from '../../services/task-list.service';

@Component({
  selector: 'app-edit-task',
  templateUrl: './edit-task.component.html'
})
export class EditTaskComponent implements OnInit {

  updateTaskRequest: Task = {
    id: '',
    name: '',
    done: false
  };

  constructor(
    private taskListService: TaskListService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');

        if (id) {
          this.taskListService.getTask(id).subscribe({
            next: (task) => {
              this.updateTaskRequest = task;
            },
          });
        }
      },
    });
  }

  updateTask() {
    this.taskListService
      .updateTask(this.updateTaskRequest.id, this.updateTaskRequest)
      .subscribe({
        next: () => {
          this.router.navigate(['tasks']);
        },
        error: (error) => {
          console.log(error);
        },
      });
  }

}
