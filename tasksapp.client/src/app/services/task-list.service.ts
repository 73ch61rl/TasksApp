import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Task } from '../models/task.model';

@Injectable({
  providedIn: 'root'
})
export class TaskListService {

  apiUrl: string = "http://localhost:5072/api/tasks/";

  constructor(private http: HttpClient) { }

  getAllTasks(): Observable<Task[]> {
    return this.http.get<Task[]>(this.apiUrl);
  }

  addTask(newTask: Task): Observable<Task> {
    newTask.id = '0';
    return this.http.post<Task>(this.apiUrl, newTask);
  }

  getTask(id: string): Observable<Task> {
    return this.http.get<Task>(this.apiUrl + id);
  }

  updateTask(id: string, updateTaskRequest: Task): Observable<Task> {
    return this.http.put<Task>(this.apiUrl + id, updateTaskRequest);
  }

  deleteTask(id: string): Observable<Task> {
    return this.http.delete<Task>(this.apiUrl + id);
  }

}
