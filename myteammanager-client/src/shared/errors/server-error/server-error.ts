import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  imports: [],
  templateUrl: './server-error.html',
  styleUrl: './server-error.css',
})
export class ServerError {

  private router = inject(Router);

  constructor(){
    const navigation = this.router.getCurrentNavigation();
    console.log("navigation", navigation);
  }



}
