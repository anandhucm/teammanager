import { Component } from '@angular/core';
import { inject } from '@angular/core';
import { Location } from '@angular/common';

@Component({
  selector: 'app-not-found',
  imports: [],
  templateUrl: './not-found.html',
  styleUrl: './not-found.css',
})
export class NotFound {

  private location = inject(Location);
  goBack(){
    console.log("go back function d");
    this.location.back();
    // window.history.back();
  }

}
