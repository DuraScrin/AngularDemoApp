import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})

export class NavComponent implements OnInit 
{
  constructor(private accountService: AccountService) { }

  ngOnInit(): void { }

  model: any = {}
  loggedIn: boolean;
  login()
  {
    this.accountService.login(this.model).subscribe(response =>
      {
        console.log(response);
        this.loggedIn = true;
      }, error =>
      {
        console.log(error);
      });
  }

  logout()
  {
    this.loggedIn = false;
  }
}
