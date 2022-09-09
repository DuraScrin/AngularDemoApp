import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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

  constructor(public accountService: AccountService, private router: Router, private testService: TestService) 
  { }

  ngOnInit(): void 
  { }
  
  login()
  {
    this.accountService.login(this.model).subscribe(response =>
      {
        console.log(response);
        this.router.navigateByUrl('/members');
      }, error =>
      {
        console.log(error);
      });
  }

  logout()
  {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

  runTestServiceFunction()
  {
    this.testService.logInformation("some data...");
  }
}
