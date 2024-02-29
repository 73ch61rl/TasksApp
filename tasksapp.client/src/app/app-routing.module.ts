import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TaskListComponent } from './components/task-list/task-list.component';
import { AddTaskComponent } from './components/add-task/add-task.component';
import { EditTaskComponent } from './components/edit-task/edit-task.component';

const routes: Routes = [
  {
    path: '',
    component: TaskListComponent
  },
  {
    path: 'tasks',
    component: TaskListComponent
  },
  {
    path: 'tasks/add',
    component: AddTaskComponent
  },
  {
    path: 'tasks/edit/:id',
    component: EditTaskComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
