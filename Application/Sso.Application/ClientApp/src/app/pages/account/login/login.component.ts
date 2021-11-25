import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { LoginResult } from '../../../entities/login-result';
import { User } from '../../../entities/user';
import { ELoginStatus } from '../../../enums/login-status';
import { AccountService } from '../../../services/account/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private accountService: AccountService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.queryParams.subscribe((params) => {
      this.returnUrl = params['return'] || '/';
    });
  }

  socialLoginDone(result: LoginResult) {
    switch (result.status) {
      case ELoginStatus.success: {
        this.accountService.csrfRefresh().subscribe(() => {
          this.accountService.currentUser().subscribe((user) => {
            this.loginComplete.next(user);
            this.router.navigateByUrl(this.returnUrl);
          });
        });
        break;
      }
      //case ELoginStatus.requiresTwoFactor: {
      //  this.router.navigate(['/account', 'two-factor'], { queryParams: { return: this.returnUrl } });
      //  break;
      //}
      default: {
        this.loginResult = result;
        break;
      }
    }
  }

  private returnUrl: string = '';
  loginComplete: Subject<User> = new Subject<User>();
  loginResult: LoginResult = {
    status: ELoginStatus.success,
    provider: '',
    user: null,
    error: '',
    errorDescription: '',
  };
}
