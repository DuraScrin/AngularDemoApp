import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { TestService } from '../_services/test.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})

export class NavComponent implements OnInit 
{
  constructor(private accountService: AccountService, private testService: TestService) { }

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

  runTestServiceFunction()
  {
    this.testService.logInformation("some data...");
  }
}
