import { Component, signal, inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';
import { AccountService } from '../core/service/account-service';
import { RouterOutlet } from '@angular/router';
import { Toastr } from '../toastr/toastr/toastr';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Toastr],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})

export class App implements OnInit { // if we implements OnInit interface we should implement its method, otherwise show error.

  protected accountService = inject(AccountService);
  private http = inject(HttpClient);  // class is constructed like we constructed in c# 
  protected readonly title = signal('myteammanager');
  protected responsejson = signal<any>([]);
  async ngOnInit() {
    this.responsejson.set(await this.getMembers());
    console.log('inside the ng on init');
    this.setCurrentUser();
  }

  async getMembers(){
    // try{
    //   console.log(this.http.get("https://localhost:7146/api/manageteammembers"));
    //   return await lastValueFrom(this.http.get("https://localhost:7146/api/manageteammembers"));
    // }catch(error){
    //   console.log(error);
    //   throw error;
    // }
  } 

  setCurrentUser(){
    const userString = localStorage.getItem("memberData");
    console.log(userString);
    if(!userString) return;
    const user = JSON.parse(userString);
    console.log("inside it user", user);
    this.accountService.memberData.set(user);
  }
}
