//ng generate service Test --skip-tests

import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })

export class TestService 
{
  constructor() { }

  logInformation(data: string)
  {
    console.log("--> " + data);
  }
}
