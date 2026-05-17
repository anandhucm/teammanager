import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import {User} from "../../types/types";
import { tap } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private account = inject(HttpClient);
  public  baseUrl = environment.apiUrl;
  public memberData = signal<User | null>(null);
  protected isLoggedIn = signal(false);
  protected router = inject(Router);
  // constructor(){
  //   console.log("inisde the constuctr");
  //   this.logout();
  // }

  login(credentials: any){
    return this.account.post<User>(this.baseUrl + "account/login", credentials).pipe(
      tap(user=> {
        if(user){
          console.log("user", user);
          this.memberData.set(user);
          this.isLoggedIn.set(true);
          localStorage.setItem("memberData", JSON.stringify(user));
        }      
      })
    );
  }

  logout(){
    // console.log("Inisde he logout");
    this.isLoggedIn.set(false);
    localStorage.removeItem("memberData");
    this.memberData.set(null);
    this.router.navigateByUrl("/logout");
    
  }

}
