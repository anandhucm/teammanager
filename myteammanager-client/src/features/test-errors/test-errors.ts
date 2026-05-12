import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-test-errors',
  imports: [],
  templateUrl: './test-errors.html',
  styleUrl: './test-errors.css',
})
export class TestErrors {

  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;;

  getServerError(){
    this.http.get( this.baseUrl+"errorhandling/server-error").subscribe({
      next: response => console.log("response =>", response),
      error: error => console.log("error => ",error)
    })
  }
  getPostError(){
    this.http.post( this.baseUrl+"account/register", {}).subscribe({
      next: response => console.log("response =>", response),
      error: error => console.log("error => ",error)
    })
  }
  getBadRequestError(){
    this.http.get( this.baseUrl+"errorhandling/bad-request").subscribe({
      next: response => console.log("response =>", response),
      error: error => console.log("error => ",error)
    })
  }

  getNotFoundError(){
    this.http.get( this.baseUrl+"errorhandling/not-found").subscribe({
      next: response => console.log("response =>", response),
      error: error => console.log("error => ",error)
    })
  }
  getAuthError(){
    this.http.get( this.baseUrl+"errorhandling/auth").subscribe({
      next: response => console.log("response =>", response),
      error: error => console.log("error => ",error)
    })
  }

}
