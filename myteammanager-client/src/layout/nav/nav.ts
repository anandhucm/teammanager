import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/service/account-service';

@Component({
  selector: 'app-nav',
  imports: [FormsModule],
  templateUrl: './nav.html',
  styleUrl: './nav.css'
})
export class Nav {

  protected credentials: any = {};
  protected accountService = inject(AccountService);
  protected isLoggedIn = signal(false);
  protected darkModeVar = signal(false);
  protected memberData = this.accountService.memberData;

  login(){
    this.accountService.login(this.credentials).subscribe({
       next: (result: any) => {
        console.log(result);
        this.isLoggedIn.set(true);
        this.credentials = {};
       },
       error: (error: any) => {
        console.log(error);
       }
    })
  }

  logout(){
    this.isLoggedIn.set(false);
    localStorage.removeItem("memberData");
  }

  darkMode(){

    this.darkModeVar.set(true);

  }

}
