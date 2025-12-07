import { Component, signal, inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';
import { Nav } from "../layout/nav/nav";

@Component({
  selector: 'app-root',
  imports: [Nav],
  templateUrl: './app.html',
  styleUrl: './app.css'
})

export class App implements OnInit { // if we implements OnInit interface we should implement its method, otherwise show error.
  private http = inject(HttpClient);  // class is constructed like we constructed in c# 
  protected readonly title = signal('myteammanager');
  protected responsejson = signal<any>([]);
  async ngOnInit() {
    this.responsejson.set(await this.getMembers());
  }

  async getMembers(){
    try{
      console.log(this.http.get("http://localhost:5052/api/manageteammembers"));
      return await lastValueFrom(this.http.get("http://localhost:5052/api/manageteammembers"));
    }catch(error){
      console.log(error);
      throw error;
    }
  } 
}
