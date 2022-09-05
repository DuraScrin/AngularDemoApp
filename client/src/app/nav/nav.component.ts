import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/User';
import { AccountService } from '../_services/account.service';
import { TestService } from '../_services/test.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})

export class NavComponent implements OnInit 
{
  model: any = {}

  constructor(public accountService: AccountService, private testService: TestService) { }

  ngOnInit(): void 
  { }
  
  login()
  {
    this.accountService.login(this.model).subscribe(response =>
      {
        console.log(response);
      }, error =>
      {
        console.log(error);
      });
  }

  logout()
  {
    this.accountService.logout();
  }

  runTestServiceFunction()
  {
    this.testService.logInformation("some data...");
  }
}
